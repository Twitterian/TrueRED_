using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using TrueRED.Display;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Parameters;

// TODO: Expire기간중에 트윗하면 다른 모듈 참조하도록 설정하기?
namespace TrueRED.Modules
{
	public class ReactorModule : Module, IStreamListener, ITimeTask
	{
		public override string ModuleName
		{
			get
			{
				return "Reactor";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "타임라인에 반응해요";
			}
		}

		string stringset;
		TimeSet moduleWakeup = null;
		TimeSet moduleSleep = null;

		List<string> reactor_category    = new List<string>();
		List<string> reactor_input       = new List<string>();
		List<string> reactor_output      = new List<string>();

		Random _selector = new Random();

		Dictionary<long, TimeSet> ExpireUsers = new Dictionary<long, TimeSet>();
		int ExpireTime;
		int ExpireDelay;

		public ReactorModule( ) : base( string.Empty )
		{

		}
		public ReactorModule( string name ) : base( name )
		{
			this.moduleWakeup = this.moduleSleep = new TimeSet( -1 );
		}

		public void LoadStringsets( string stringSet )
		{
			var reactor = StringSetsManager.GetStrings(stringSet);

			for ( int i = 0; i < reactor.Length; i++ )
			{
				var tags = reactor[i].Split('∥');
				if ( tags.Length != 3 )
				{
					Log.Error( this.Name, string.Format( "Not correct reactor stringset '{0}'", reactor[i] ) );
					continue;
				}
				reactor_category.Add( tags[0] );
				reactor_input.Add( tags[1] );
				reactor_output.Add( tags[2] );
			}
		}

		private class ParseEscapeResult
		{
			public bool Flag { get; set; }
			public string String { get; set; }
			public long Id { get; set; }
		}

		private ParseEscapeResult ParseEscapeString( string output, ITweet status )
		{
			var result = new ParseEscapeResult {Flag = true, String = string.Empty, Id = status.Id };


			// :Tweet_Username:
			// 이 부분을 상대방의 닉네임으로 치환합니다.
			result.String = output.Replace( ":Tweet_Username:", status.CreatedBy.Name );

			// :NotMention:
			// 이 트윗을 멘션으로 취급하지 않습니다.
			if ( !result.String.Contains( ":NotMention:" ) )
			{
				result.String = string.Format( "@{0} {1}", status.CreatedBy.ScreenName, result.String );
			}
			else
			{
				result.String = result.String.Replace( ":NotMention:", "" );
				result.Id = 0;
			}

			// :Pakuri:
			// 원본 트윗을 파쿠리합니다.
			if ( result.String.Contains( ":Pakuri:" ) )
			{
				var regex = new Regex( "/[^(가-힣|ㄱ-ㅎ|ㅏ-ㅣ|\\s)]/gi" );
				result.String = result.String.Replace( ":Pakuri:", regex.Replace( status.Text, "" ) );
			}

			// :Number%:
			// 제시된 확률을 바탕으로 트윗을 보낼지 말지 결정합니다.
			for ( var i = 1; i <= 100; i++ )
			{
				if ( result.String.Contains( ":" + i + "%:" ) )
				{
					var drop = i;
					var lotto = _selector.Next((int)(100 / drop));
					if ( lotto != 0 )
					{
						result.Flag = false;
					}
					result.String = result.String.Replace( ":" + i + "%:", "" );
					break;
				}
			}

			return result;

		}

		enum TweetMatchResult { Match, NotMatch, Expire }

		private TweetMatchResult IsMatch( string category, string input, ITweet status )
		{

			TweetMatchResult state = TweetMatchResult.NotMatch;

			if ( status.Text.Replace( " ", "" ).Replace( "\n", "" ).Contains( input.Replace( " ", "" ).Replace( "\n", "" ) ) )
			{
				switch ( category )
				{
					case "All":
						Log.Print( this.Name, string.Format( "catch tweet (all) [{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
						state = TweetMatchResult.Match;
						break;
					case "Mention":
						if ( status.InReplyToUserId == Globals.Instance.User.Id )
						{
							Log.Print( this.Name, string.Format( "catch tweet (Mention) [{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
							state = TweetMatchResult.Match;
						}
						break;
					case "Public":
						if ( status.InReplyToStatusId == null && status.InReplyToScreenName == null && !new Regex( "^\\s@\\s" ).IsMatch( status.Text ) )
						{
							Log.Print( this.Name, string.Format( "catch tweet (Public) [{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
							state = TweetMatchResult.Match;
						}
						break;
				}


				lock ( ExpireUsers )
				{
					if ( ExpireUsers.ContainsKey( status.CreatedBy.Id ) )
					{
						var ExpireTimeset = ExpireUsers[status.CreatedBy.Id];
						if ( TimeSet.Verification( new TimeSet( DateTime.Now ), ExpireTimeset, new TimeSet( ExpireTimeset.Hour, ExpireTimeset.Minute + ExpireTime ) ) )
						{
							Log.Print( this.Name, string.Format( "User {0} rejected by expire : to {1}", status.CreatedBy.ScreenName, ExpireTimeset.ToString( ) ) );
							state = TweetMatchResult.Expire;
						}
						else
						{
							ExpireUsers.Remove( status.CreatedBy.Id );
						}
					}
				}
			}
			return state;
		}

		public bool Verification( )
		{
			if ( moduleWakeup == null && moduleSleep == null ) return true;
			if ( moduleWakeup.Hour == -1 && moduleSleep.Hour == -1 ) return true;
			return TimeSet.Verification( new TimeSet( DateTime.Now ), this.moduleWakeup, this.moduleSleep );
		}

		void IStreamListener.TweetCreateByAnyone( object sender, TweetReceivedEventArgs args )
		{
			if ( !IsRunning ) return;
			var tweet = args.Tweet;
			if ( !Verification( ) ) return;
			if ( tweet.CreatedBy.Id == Globals.Instance.User.Id ) return;
			if ( tweet.IsRetweet == true ) return;

			var cases = new List<int>();
			for ( int i = 0; i < reactor_category.Count; i++ )
			{
				var category = reactor_category[i];
				var input = reactor_input[i];

				if ( input.StartsWith( "__" ) && input.EndsWith( "__" ) )
				{
					var inputset = StringSetsManager.GetStrings(input.Substring(2, input.Length-4));
					var loopbreak = false;
					for ( int j = 0; j < inputset.Length; j++ )
					{
						var matchResult = IsMatch( category, inputset[j], tweet );
						if ( matchResult == TweetMatchResult.Match )
						{
							cases.Add( i );
						}
						else if ( matchResult == TweetMatchResult.Expire )
						{
							loopbreak = true;
							break;
						}
					}
					if ( loopbreak ) break;
				}
				else
				{
					var matchResult = IsMatch( category, input, tweet );
					if ( matchResult == TweetMatchResult.Match )
					{
						cases.Add( i );
					}
					else if ( matchResult == TweetMatchResult.Expire ) break;
				}
			}
			if ( cases.Count > 0 )
			{
				var i = _selector.Next(cases.Count);
				string @out = string.Empty;
				if ( reactor_output[cases[i]].StartsWith( "__" ) && reactor_output[cases[i]].EndsWith( "__" ) )
					@out = StringSetsManager.GetRandomString( reactor_output[cases[i]].Substring( 2, reactor_output[cases[i]].Length - 4 ) );
				else @out = reactor_output[cases[i]];
				if ( string.IsNullOrEmpty( @out ) ) return;
				var pString = ParseEscapeString(@out, tweet);
				if ( pString.Flag )
				{
					var result = Globals.Instance.User.PublishTweet( pString.String, new PublishTweetOptionalParameters()
					{
						InReplyToTweetId = pString.Id
					});
					Log.Print( this.Name, string.Format( "Send tweet [{0}]", result.Text ) );

					if ( ExpireUsers.ContainsKey( tweet.CreatedBy.Id ) ) ExpireUsers[tweet.CreatedBy.Id] = new TimeSet( DateTime.Now );
					ExpireUsers.Add( tweet.CreatedBy.Id, new TimeSet( DateTime.Now ) );
				}
			}
		}

		void IStreamListener.MessageSent( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.MessageReceived( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.TweetFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.TweetUnFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.ListCreated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListUpdated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListDestroyed( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.BlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.UnBlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.FollowedUser( object sender, UserFollowedEventArgs args )
		{

		}

		void IStreamListener.FollowedByUser( object sender, UserFollowedEventArgs args )
		{

		}

		void IStreamListener.UnFollowedUser( object sender, UserFollowedEventArgs args )
		{

		}

		void IStreamListener.AuthenticatedUserProfileUpdated( object sender, AuthenticatedUserUpdatedEventArgs args )
		{

		}

		void IStreamListener.FriendIdsReceived( object sender, GenericEventArgs<IEnumerable<long>> args )
		{

		}

		void IStreamListener.AccessRevoked( object sender, AccessRevokedEventArgs args )
		{

		}

		void ITimeTask.Run( )
		{
			while ( !Disposed )
			{
				if ( IsRunning )
				{
					lock ( ExpireUsers )
					{
						for ( int i = 0; i < ExpireUsers.Count; i++ )
						{
							var keys = new List<long>( ExpireUsers.Keys ) ;
							var key = keys[i];
							var item = ExpireUsers[key];
							if ( TimeSet.Verification( new TimeSet( DateTime.Now ), item, new TimeSet( item.Hour, item.Minute + ExpireTime ) ) )
							{
								continue;
							}
							else
							{
								ExpireUsers.Remove( key );
							}
						}
					}
				}
				Thread.Sleep( ExpireDelay * 60 );
			}
		}

		public override void OpenSettings( INIParser parser )
		{
			stringset = parser.GetValue( "Module", "ReactorStringset" );

			var expiretime = parser.GetValue("Expire", "Time");
			var expiredelay = parser.GetValue("Expire", "Delay");

			var starttime = parser.GetValue("TimeLimit", "StartTime");
			var endtime = parser.GetValue("TimeLimit", "EndTime");

			LoadStringsets( stringset );

			if ( !string.IsNullOrEmpty( expiretime ) )
			{
				ExpireTime = int.Parse( expiretime );
			}
			else
			{
				ExpireTime = 10;
			}

			if ( !string.IsNullOrEmpty( expiredelay ) )
			{
				ExpireDelay = int.Parse( expiredelay );
			}
			else
			{
				ExpireDelay = 5;
			}

			if ( !string.IsNullOrEmpty( starttime ) )
			{
				this.moduleWakeup = TimeSet.FromString( starttime );
			}
			else
			{
				this.moduleWakeup = new TimeSet( -1 );
			}

			if ( !string.IsNullOrEmpty( endtime ) )
			{
				this.moduleSleep = TimeSet.FromString( endtime );
			}
			else
			{
				this.moduleSleep = new TimeSet( -1 );
			}
		}

		public override void SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
			parser.SetValue( "Module", "ReactorStringset", stringset );

			parser.SetValue( "Expire", "Time", ExpireTime );
			parser.SetValue( "Expire", "Delay", ExpireDelay );

			parser.SetValue( "TimeLimit", "StartTime", moduleWakeup );
			parser.SetValue( "TimeLimit", "EndTime", moduleSleep );
		}

		protected override void Release( )
		{
		}

		public override Module CreateModule( object[] @params )
		{
			var moduleName = (string)@params[0];
			var StringSet = (string)@params[0];
			var Time = (int)@params[0];
			var Delay = (int)@params[0];
			var StartTime = (string)@params[0];
			var EndTime = (string)@params[0];

			var module = new ReactorModule( moduleName );
			module.stringset = StringSet;
			module.LoadStringsets( StringSet );
			module.ExpireTime = Time;
			module.ExpireDelay = Delay;
			module.moduleWakeup = TimeSet.FromString( StartTime );
			module.moduleSleep = TimeSet.FromString( EndTime );
			return module;
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new ModuleFaceCategory("Module" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "문자셋" );
			face.Add( category1 );

			var category2 = new ModuleFaceCategory("Expire" );
			category2.Add( ModuleFaceCategory.ModuleFaceTypes.Int, "Time" );
			category2.Add( ModuleFaceCategory.ModuleFaceTypes.Int, "Delay" );
			face.Add( category2 );

			var category3 = new ModuleFaceCategory("TimeLimit" );
			category3.Add( ModuleFaceCategory.ModuleFaceTypes.String, "StartTime" );
			category3.Add( ModuleFaceCategory.ModuleFaceTypes.String, "EndTime" );
			face.Add( category3 );

			return face;
		}
	}
}
