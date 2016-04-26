using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
	public class ReactorModule : Module, IStreamListener, IUseSetting, ITimeTask
	{
		public static string ModuleName { get; protected set; } = "Reactor";
		public static string ModuleDescription { get; protected set; } = "Reaction timeline";

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

		public ReactorModule( string name, IAuthenticatedUser user, IUser owner ) : base( name, user, owner)
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
					Log.Error( "Reactor.Status", string.Format( "Not correct reactor stringset '{0}'", reactor[i] ) );
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

		enum TweetMatchResult { Match, NotMatch, Expire}

		private TweetMatchResult IsMatch( string category, string input, ITweet status )
		{
			lock ( ExpireUsers )
			{
				if ( ExpireUsers.ContainsKey( status.CreatedBy.Id ) )
				{
					var ExpireTimeset = ExpireUsers[status.CreatedBy.Id];
					if ( TimeSet.Verification( new TimeSet( DateTime.Now ), ExpireTimeset, new TimeSet( ExpireTimeset.Hour, ExpireTimeset.Minute + ExpireTime ) ) )
					{
						Log.Print( "ReactorRejected", string.Format( "User {0} rejected by expire : to {1}", status.CreatedBy.ScreenName, ExpireTimeset.ToString( ) ) );
						return TweetMatchResult.Expire;
					}
					else
					{
						ExpireUsers.Remove( status.CreatedBy.Id );
					}
				}
			}

			if ( status.Text.Replace( " ", "" ).Replace( "\n", "" ).Contains( input.Replace( " ", "" ).Replace( "\n", "" ) ) )
			{
				switch ( category )
				{
					case "All":
						Log.Print( "Reactor catch tweet (All)", string.Format( "[{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
						return TweetMatchResult.Match;
					case "Mention":
						if ( status.InReplyToUserId == user.Id )
						{
							Log.Print( "Reactor catch tweet (Mention)", string.Format( "[{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
							return TweetMatchResult.Match;
						}
						break;
					case "Public":
						if ( status.InReplyToStatusId == null && status.InReplyToScreenName == null && !new Regex( "^\\s@\\s" ).IsMatch( status.Text ) )
						{
							Log.Print( "Reactor catch tweet (Public)", string.Format( "[{0}({1}) : {2}]", status.CreatedBy.Name, status.CreatedBy.ScreenName, status.Text ) );
							return TweetMatchResult.Match;
						}
						break;
				}
			}
			return TweetMatchResult.NotMatch;
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
			if ( tweet.CreatedBy.Id == user.Id ) return;
			if ( tweet.IsRetweet == true ) return;
			
			var cases = new List<int>();
			for ( int i = 0; i < reactor_category.Count; i++ )
			{
				var category = reactor_category[i];
				var input = reactor_input[i];

				if ( input.StartsWith( "__" ) && input.EndsWith( "__" ) )
				{
					var inputset = StringSetsManager.GetStrings(input.Substring(2, input.Length-4));
					for ( int j = 0; j < inputset.Length; j++ )
					{
						var matchResult = IsMatch( category, inputset[j], tweet );
						if ( matchResult == TweetMatchResult.Match )
						{
							cases.Add( i );
						}
						else if ( matchResult == TweetMatchResult.Expire ) break;
					}
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
					var result = Tweet.PublishTweetInReplyTo(pString.String,pString.Id);
					Log.Print( "Reactor", string.Format( "Send tweet [{0}]", result.Text ) );

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

		void IUseSetting.OpenSettings( INIParser path )
		{
			stringset = path.GetValue("Module", "ReactorStringset");

			var expiretime = path.GetValue("Expire", "Time");
			var expiredelay = path.GetValue("Expire", "Delay");

			var starttime = path.GetValue("TimeLimit", "StartTime");
			var endtime = path.GetValue("TimeLimit", "EndTime");
			
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

		void IUseSetting.SaveSettings( INIParser path )
		{
			path.SetValue( "Module", "IsRunning", IsRunning );
			path.SetValue( "Module", "Type", this.GetType( ).FullName );
			path.SetValue( "Module", "Name", Name );
			path.SetValue( "Module", "ReactorStringset", stringset );

			path.SetValue( "Expire", "Time", ExpireTime );
			path.SetValue( "Expire", "Delay", ExpireDelay );

			path.SetValue( "TimeLimit", "StartTime", moduleWakeup );
			path.SetValue( "TimeLimit", "EndTime", moduleSleep );
		}

		void ITimeTask.Run( )
		{
			while ( true )
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
	}
}
