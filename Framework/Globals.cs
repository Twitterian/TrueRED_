using System;
using System.Collections.Generic;
using Tweetinvi;

namespace TrueRED.Framework
{
	/// <summary>
	/// 프로그램 전역에 걸친 인스턴스를 관리하는 클래스
	/// </summary>
	class Globals
	{
		#region 생성자
		private static Globals _instance = null;
		public static Globals Instance
		{
			get
			{
				if ( _instance == null )
					_instance = new Globals( );
				return _instance;
			}
		}
		private Globals( )
		{
		}
		private const string LogHeader = "Globals";
		#endregion
		
		public List<Action> AppInitializedListener { get; private set; } = new List<Action>( );

		private Tweetinvi.Core.Interfaces.IAuthenticatedUser _user;
		public Tweetinvi.Core.Interfaces.IAuthenticatedUser User
		{
			get
			{
				if ( _user == null )
				{
					Log.Error( LogHeader, "먼저 Globals를 초기화해주세요 ㅜㅜ" );
					throw new Exception( );
				}
				return _user;
			}
		}

		internal void Initialize( string consumerKey, string consumerSecret, string accessToken, string accessSecret )
		{
			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );
			_user = Tweetinvi.User.GetAuthenticatedUser( );
			ModuleManager.ReloadStream( );

			foreach ( var item in AppInitializedListener )
			{
				item( );
			}

			Log.Http( LogHeader, string.Format( "다음 계정으로 트위터에 로그인했습니다 : {0}({1}) [{2}]", User.Name, User.ScreenName, User.Id ) );
			//Tweet.PublishTweet( string.Format( "다음 계정으로 트위터에 로그인했습니다 : {0}({1}) [{2}]", User.Name, User.ScreenName, User.Id ) );
		}
	}

}
