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
		private Globals( ) { }
		#endregion

		private Tweetinvi.Core.Interfaces.IAuthenticatedUser _user;
		public Tweetinvi.Core.Interfaces.IAuthenticatedUser User
		{
			get
			{
				if ( _user == null )
				{
					_user = Tweetinvi.User.GetAuthenticatedUser( );
				}
				return _user;
			}
		}
	}

}
