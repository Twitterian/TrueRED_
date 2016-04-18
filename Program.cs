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
using Tweetinvi.Core.Interfaces;

namespace TrueRED
{
	class Program
	{
		static void Main( string[] args )
		{
#if DEBUG
			Body( );
#else
			try
			{
				Body( );
			}
			catch ( Exception e )
			{
				try
				{
					Log.Error( e.ToString( ), e.StackTrace );
					Tweet.PublishTweet( e.ToString( ) );
				}
				catch
				{
					return;
				}
            }
#endif
		}

		static void Body( )
		{
			#region Initialize Program

			Log.Init( );
			StringSetsManager.LoadStringSets( "Stringsets" );

			var setting = new INIParser( "Globals.ini" );
			var AuthData = "Authenticate";
			string consumerKey = setting.GetValue( AuthData, "ConsumerKey" );
			string consumerSecret = setting.GetValue( AuthData, "CconsumerSecret" );
			string accessToken = setting.GetValue( AuthData, "AccessToken" );
			string accessSecret = setting.GetValue( AuthData, "AccessSecret" );
			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );
			var user = User.GetAuthenticatedUser( );
			Log.Debug( "UserCredentials :", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );

			long ownerID = 0;
			try
			{
				ownerID = long.Parse( setting.GetValue( "AppInfo", "OwnerID" ) );
			}
			catch ( FormatException e )
			{
				ownerID = 0;
			}
			var owner = User.GetUserFromId( ownerID );
			#endregion

			InitDirectories( );

			#region Initialize Modules

			var modules = new Dictionary<string, Module>();
			modules.Add( "YoruHello", new ReactorModule( user, owner, "YoruHelloReactor" ) );
			modules.Add( "AsaHello", new ReactorModule( user, owner, "AsaHelloReactor" ) );
			modules.Add( "TimeTweet", new SchedulerModule( user, "TimeTweet" ) );
			modules.Add( "Reflector", new ReflectorModule( user ) );
			modules.Add( "Controller", new ControllerModule( user, owner, modules ) );
			modules.Add( "Weather", new WeatherModule( user ) );

			#endregion

			foreach ( var item in modules )
			{
				if ( item.Value is ITimeTask )
				{
					var module = (ITimeTask)item.Value;
					Task.Factory.StartNew( ( ) => module.Run( ) );
				}

				if ( item.Value is IUseSetting )
				{
					var module = (IUseSetting)item.Value;
					module.OpenSettings( Path.Combine( "Settings", item.Key + ".ini" ) );
				}
			}

			CreateStream( modules.Values.OfType<IStreamListener>( ) );

			new Display.AppConsole( modules ).ShowDialog( );

			foreach ( var item in modules )
			{
				if ( item.Value is IUseSetting )
				{
					var module = (IUseSetting)item.Value;
					module.SaveSettings( Path.Combine( "Settings", item.Key + ".ini" ) );
				}
			}
		}

		private static void InitDirectories( )
		{
			var settings = Path.Combine( Directory.GetCurrentDirectory( ), "Settings" ) ;
			if ( !Directory.Exists( settings ) )
			{
				Directory.CreateDirectory( settings );
			}

			var stringsets = Path.Combine( Directory.GetCurrentDirectory( ), "StringSets" ) ;
			if ( !Directory.Exists( stringsets ) )
			{
				Directory.CreateDirectory( stringsets );
			}
		}

		static void CreateStream( IEnumerable<IStreamListener> modules )
		{
			if ( modules.Count( ) == 0 ) return;

			var userStream = Tweetinvi.Stream.CreateUserStream();
			foreach ( IStreamListener module in modules )
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
			userStream.StreamStopped += delegate
			{
				Log.Error( "Program", "Stream was stopped. restarting..." );
				CreateStream( modules );
			};
			userStream.StartStreamAsync( );
			Log.Http( "Program", "Stream is running now" );
		}
	}
}
