using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Modules;

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

		public List<Module> Modules { get; set; } = new List<Module>( ); // 어플리케이션에 로딩된 모듈 목록
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
