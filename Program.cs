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
			InitDirectories( );
			StringSetsManager.LoadStringSets( "Stringsets" );

			var setting = new INIParser( "Globals.ini" );
			var AuthData = "Authenticate";
			string consumerKey = setting.GetValue( AuthData, "ConsumerKey" );
			string consumerSecret = setting.GetValue( AuthData, "CconsumerSecret" );
			string accessToken = setting.GetValue( AuthData, "AccessToken" );
			string accessSecret = setting.GetValue( AuthData, "AccessSecret" );
			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );
			//TODO: User가 null일 경우의 대처
			var user = Globals.Instance.User;
			Log.Http( "UserCredentials", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );
			#endregion
			
			ModuleManager.Initialize( );
			ModuleManager.LoadAllModules( "Modules" );

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
		
		#region Test Modules

		static void WindowDebugMode( )
		{
			new Display.MakeModule( ).ShowDialog( );
		}

		#endregion

	}
}
