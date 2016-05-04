using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueRED.Framework;
using TrueRED.Modules;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.Streaminvi;

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
			var AuthData = "Authenticate2";
			string consumerKey = setting.GetValue( AuthData, "ConsumerKey" );
			string consumerSecret = setting.GetValue( AuthData, "CconsumerSecret" );
			string accessToken = setting.GetValue( AuthData, "AccessToken" );
			string accessSecret = setting.GetValue( AuthData, "AccessSecret" );
			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );
			var user = Globals.Instance.User;
			Log.Http( "UserCredentials", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );
			#endregion

			InitDirectories( );

			#region Initialize Modules

			ModuleManager.LoadAllModules( "Modules" );

			#endregion

			foreach ( var item in ModuleManager.Modules )
			{
				OnModuleAdd_StartTimeTask( item );
			}
			var stream = CreateStream( ModuleManager.Modules );


			ModuleManager.Modules.OnModuleAttachLiestner.Add( delegate ( Module module )
			{
				OnModuleAdd_StartTimeTask( module );
				OnModuleAdd_AttachStream( stream, module );
			} );

			new Display.AppConsole( ).ShowDialog( );

			foreach ( var module in ModuleManager.Modules )
			{
				var parser = new INIParser(Path.Combine( "Modules", module.Name + ".ini" ));
				module.SaveSettings( parser );
				parser.Save( );
			}

			setting.Save( );

			Console.WriteLine( "종료하시려면 아무 키나 누르세요." );
			Console.Read( );
		}

		private static void OnModuleAdd_StartTimeTask( Module module )
		{
			if ( module is ITimeTask )
			{
				var timetask = (ITimeTask)module;
				Task.Factory.StartNew( ( ) => timetask.Run( ) );
			}
		}
		private static void OnModuleAdd_AttachStream( IUserStream userStream, Module module )
		{
			if ( module is IStreamListener )
			{
				var streamlistener = (IStreamListener)module;
				userStream.TweetCreatedByAnyone += streamlistener.TweetCreateByAnyone;
				userStream.MessageSent += streamlistener.MessageSent;
				userStream.MessageReceived += streamlistener.MessageReceived;
				userStream.TweetFavouritedByAnyone += streamlistener.TweetFavouritedByAnyone;
				userStream.TweetUnFavouritedByAnyone += streamlistener.TweetUnFavouritedByAnyone;
				userStream.ListCreated += streamlistener.ListCreated;
				userStream.ListUpdated += streamlistener.ListUpdated;
				userStream.ListDestroyed += streamlistener.ListDestroyed;
				userStream.BlockedUser += streamlistener.BlockedUser;
				userStream.UnBlockedUser += streamlistener.UnBlockedUser;
				userStream.FollowedUser += streamlistener.FollowedUser;
				userStream.FollowedByUser += streamlistener.FollowedByUser;
				userStream.UnFollowedUser += streamlistener.UnFollowedUser;
				userStream.AuthenticatedUserProfileUpdated += streamlistener.AuthenticatedUserProfileUpdated;
				userStream.FriendIdsReceived += streamlistener.FriendIdsReceived;
				userStream.AccessRevoked += streamlistener.AccessRevoked;
			}
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

		static IUserStream CreateStream( IEnumerable<Module> modules )
		{
			if ( modules.Count( ) == 0 ) return null;

			var userStream = Tweetinvi.Stream.CreateUserStream();
			foreach ( var module in modules )
			{
				OnModuleAdd_AttachStream( userStream, module );
			}
			userStream.StreamStopped += delegate
			{
				Log.Error( "Program", "Stream was stopped. restarting..." );
				CreateStream( modules );
			};
			userStream.StartStreamAsync( );
			Log.Http( "Program", "Stream is running now" );
			return userStream;
		}



		#region Test Modules

		static void WindowDebugMode( )
		{
			new Display.MakeModule( ).ShowDialog( );
		}

		#endregion

	}
}
