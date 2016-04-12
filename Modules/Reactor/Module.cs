using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueRED.Framework;
using TweetSharp;

namespace TrueRED.Modules.Reactor
{
	class Module : StreamListener, TimeLimiter
	{
		private TwitterService twitter;
		private long Id;
		private TimeSet moduleWakeup;
		private TimeSet moduleSleep;

		List<string> reactor_category    = new List<string>();
		List<string> reactor_input       = new List<string>();
		List<string> reactor_output      = new List<string>();

		private Random _selector = new Random();

		public Module( TwitterService twitter, long userId, string reactorStringset )
		{
			this.twitter = twitter;
			this.Id = userId;
			LoadStringsets( reactorStringset );
		}

		public Module( TwitterService twitter, long userId, string reactorStringset, TimeSet moduleWakeup, TimeSet moduleSleep ) : this( twitter, userId, reactorStringset )
		{
			this.moduleWakeup = moduleWakeup;
			this.moduleSleep = moduleSleep;
		}

		public void LoadStringsets( string stringSet )
		{
			var reactor = StringSetsManager.GetStrings(stringSet);

			for ( int i = 0; i < reactor.Length; i++ )
			{
				var tags = reactor[i].Split('∥');
				if ( tags.Length != 3 )
				{
					Log.Error( "Reactor.Status", string.Format( "Not correct reactor stringset {0}", reactor[i] ) );
					continue;
				}
				reactor_category.Add( tags[0] );
				reactor_input.Add( tags[1] );
				reactor_output.Add( tags[2] );
			}
		}

		public void DeleteDirectMessage( TwitterUserStreamDeleteDirectMessage delete )
		{

		}

		public void DeleteStatus( TwitterUserStreamDeleteStatus delete )
		{

		}

		public void DirectMessage( TwitterDirectMessage dm )
		{

		}

		public void End( TwitterUserStreamEnd end )
		{

		}

		public void Event( TwitterUserStreamEvent @event )
		{

		}

		public void Friends( TwitterUserStreamFriends friends )
		{

		}

		public void Status( TwitterStatus status )
		{
			if ( !Verification( ) ) return;
			if ( status.User.Id == Id ) return;
			if ( status.RetweetedStatus != null ) return;

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
						if ( IsMatch( category, inputset[j], status ) )
						{
							cases.Add( i );
						}
					}
				}
				else
				{
					if ( IsMatch( category, input, status ) )
					{
						cases.Add( i );
					}
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
				var pString = ParseEscapeString(@out, status);
				if ( pString.Flag )
				{
					twitter.BeginSendTweet( new SendTweetOptions
					{
						Status = pString.String,
						InReplyToStatusId = pString.Id
					} );
				}
			}
		}

		private class ParseEscapeResult
		{
			public bool Flag { get; set; }
			public string String { get; set; }
			public long Id { get; set; }
		}

		private ParseEscapeResult ParseEscapeString( string output, TwitterStatus status )
		{
			var result = new ParseEscapeResult {Flag = true, String = string.Empty, Id = status.Id };


			// :Tweet_Username:
			// 이 부분을 상대방의 닉네임으로 치환합니다.
			result.String = output.Replace( ":Tweet_Username:", status.User.Name );

			// :NotMention:
			// 이 트윗을 멘션으로 취급하지 않습니다.
			if ( !result.String.Contains( ":NotMention:" ) )
			{
				result.String = string.Format( "@{0} {1}", status.User.ScreenName, result.String );
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

		private bool IsMatch( string category, string input, TwitterStatus status )
		{
			if ( status.Text.Replace(" ", "").Replace("\n", "").Contains( input.Replace( " ", "" ).Replace( "\n", "" ) ) )
			{
				switch ( category )
				{
					case "All":
						return true;
					case "Mention":
						if ( status.InReplyToUserId == Id )
						{
							return true;
						}
						break;
					case "Public":
						if ( status.InReplyToStatusId == null && status.InReplyToScreenName == null )
						{
							return true;
						}
						break;
				}
			}
			return false;
		}

		public bool Verification( )
		{
			if ( this.moduleWakeup == null || this.moduleSleep == null ) return true;
			return TimeSet.Verification( TimeSet.GetCurrentTimeset( DateTime.Now ), this.moduleWakeup, this.moduleSleep );
		}
	}
}
