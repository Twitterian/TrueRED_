using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			//WindowDebugMode( );
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
			var user = Globals.Instance.User;
			Log.Http( "UserCredentials", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );

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

			var module_settings = Directory.GetFiles( "Modules" );
			foreach ( var item in module_settings )
			{
				if ( item.ToLower( ).EndsWith( ".ini" ) )
				{
					var parser = new INIParser(item);
					var module = Module.Create( parser );
					if ( module != null ) Globals.Instance.Modules.Add( module );
				}
			}

			#endregion

			foreach ( var item in Globals.Instance.Modules )
			{
				if ( item is ITimeTask )
				{
					var module = (ITimeTask)item;
					Task.Factory.StartNew( ( ) => module.Run( ) );
				}
			}

			CreateStream( Globals.Instance.Modules.OfType<IStreamListener>( ) );

			new Display.AppConsole( Globals.Instance.Modules ).ShowDialog( );

			foreach ( var item in Globals.Instance.Modules )
			{
				var module = (IUseSetting)item;
				var parser = new INIParser(Path.Combine( "Modules", item.Name + ".ini" ));
				module.SaveSettings( parser );
				parser.Save( );
			}

			setting.Save( );
		}

		static void InitDirectories( )
		{
			var settings = Path.Combine( Directory.GetCurrentDirectory( ), "Modules" ) ;
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


		#region Test Modules

		static void WindowDebugMode( )
		{
			new Display.MakeModule( ).ShowDialog( );
		}

		#endregion

	}
}
