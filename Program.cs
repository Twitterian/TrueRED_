using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hammock.Streaming;
using TrueRED.Framework;
using TrueRED.Modules;
using Tweetinvi;

namespace TrueRED
{
	class Program
	{
		static void Main( string[] args )
		{
			#region Initialize Program

			Log.Init( );
			StringSetsManager.LoadStringSets( "Stringsets" );

			var setting = new INIParser( Path.Combine( Directory.GetCurrentDirectory( ), "setting.ini" ) );
			string consumerKey = setting.GetValue( "Authenticate", "ConsumerKey" );
			string consumerSecret = setting.GetValue( "Authenticate", "CconsumerSecret" );
			string accessToken = setting.GetValue( "Authenticate", "AccessToken" );
			string accessSecret = setting.GetValue( "Authenticate", "AccessSecret" );

			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );

			var user = User.GetAuthenticatedUser( );
			Log.Debug( "UserCredentials :", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );

			#endregion

			#region Initialize Modules

			var YoruHello = new Modules.Reactor.Module(user,  "YoruHelloReactor", new TimeSet(20), new TimeSet(29));
			var AsaHello = new Modules.Reactor.Module(user,  "AsaHelloReactor", new TimeSet(5), new TimeSet(12));
			var TimeTweet = new Modules.Scheduler.Module(user, "TimeTweet" );

			var iniModules = new List<UseSetting>();

			var streamModules = new List<StreamListener>();
			streamModules.Add( YoruHello );

			var timetasks = new List<TimeTask>();
			timetasks.Add( TimeTweet );

			#endregion

			foreach ( var item in timetasks )
			{
				Task.Factory.StartNew( ( ) => item.Run( ) );
			}

			foreach ( var item in iniModules )
			{
				item.OpenSettings( );
			}

			CreateStream( streamModules );

			new Display.AppConsole( ).ShowDialog( );

			foreach ( var item in iniModules )
			{
				item.SaveSettings( );
			}
		}

		static void CreateStream( List<StreamListener> modules )
		{
			if ( modules.Count == 0 ) return;

			var userStream = Tweetinvi.Stream.CreateUserStream();
			foreach ( var module in modules )
			{
				userStream.TweetCreatedByAnyone += module.TweetCreateByAnyone;
				userStream.MessageSent += module.MessageSent;
				userStream.MessageReceived += module.MessageReceived;
				userStream.TweetFavouritedByAnyone += module.TweetFavouritedByAnyone;
				userStream.TweetUnFavouritedByAnyone += module.TweetUnFavouritedByAnyone;
				userStream.ListCreated += module.ListCreated;
				userStream.ListUpdated += module.ListUpdated;
				userStream.ListDestroyed += module.ListDestroyed;
				userStream.BlockedUser += module.BlockedUser;
				userStream.UnBlockedUser += module.UnBlockedUser;
				userStream.FollowedUser += module.FollowedUser;
				userStream.FollowedByUser += module.FollowedByUser;
				userStream.UnFollowedUser += module.UnFollowedUser;
				userStream.AuthenticatedUserProfileUpdated += module.AuthenticatedUserProfileUpdated;
				userStream.FriendIdsReceived += module.FriendIdsReceived;
				userStream.AccessRevoked += module.AccessRevoked;
			}
			userStream.StartStream( );

		}
	}
}
