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
		private const string LogHeader = "Program";

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
			if (
				string.IsNullOrEmpty( consumerKey ) ||
				string.IsNullOrEmpty( consumerSecret ) ||
				string.IsNullOrEmpty( accessToken ) ||
				string.IsNullOrEmpty( accessSecret ) )
			{
				Log.Error( LogHeader, "유저 인증 정보를 찾을 수 없습니다. 토큰 발급 창으로 이동합니다." );
				var frm = new Display.Authenticate();
				frm.ShowDialog( );
				if(frm.Result == true)
				{
					
				}
				else
				{
					Log.Error( LogHeader, "유저 인증에 실패했습니다. 프로그램을 종료합니다." );
					Exit( );
					return;
				}
			}
			else
			{
				//TODO: User가 null일 경우의 대처
				Globals.Instance.Initialize( consumerKey, consumerSecret, accessToken, accessSecret );
			}
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
			Exit( );
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

		static void Exit( )
		{
			Console.WriteLine( "" );
			Log.Print(LogHeader, "종료하시려면 아무 키나 누르세요." );
			Console.Read( );
		}

		#region Test Modules

		static void WindowDebugMode( )
		{
			new Display.MakeModule( ).ShowDialog( );
		}

		#endregion

	}
}
