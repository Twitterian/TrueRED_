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
		
		public ModuleList<Module> Modules { get; set; } = new ModuleList<Module>( ); // 어플리케이션에 로딩된 모듈 목록
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

	public class ModuleList<T> : List<T>
	{
		public List<Action<T>> OnModuleAttachLiestner { get; private set; } = new List<Action<T>>( );
		public List<Action<T>> OnModuleDetachLiestner { get; private set; } = new List<Action<T>>( );

		public new void Add( T item )
		{
			foreach ( var liestner in OnModuleAttachLiestner )
			{
				liestner( item );
			}
			base.Add( item );
		}

		public new void Remove( T item )
		{
			foreach ( var liestner in OnModuleDetachLiestner )
			{
				liestner( item );
			}
			base.Remove( item );
		}

	}
}
