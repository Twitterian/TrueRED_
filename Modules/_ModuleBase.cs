using System;
using System.Collections.Generic;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
	// TODO: 모듈의 싱글 인스턴스 제한 옵션 추가
	// 설정의 의무화

	/// <summary>
	/// IStreamListener 인터페이스를 통해 스트림 이벤트 동작을 정의할 수 있습니다.
	/// </summary>
	public interface IStreamListener
	{
		void TweetCreateByAnyone( object sender, TweetReceivedEventArgs args );

		void MessageSent( object sender, MessageEventArgs args );

		void MessageReceived( object sender, MessageEventArgs args );

		void TweetFavouritedByAnyone( object sender, TweetFavouritedEventArgs args );

		void TweetUnFavouritedByAnyone( object sender, TweetFavouritedEventArgs args );

		void ListCreated( object sender, ListEventArgs args );

		void ListUpdated( object sender, ListEventArgs args );

		void ListDestroyed( object sender, ListEventArgs args );

		void BlockedUser( object sender, UserBlockedEventArgs args );

		void UnBlockedUser( object sender, UserBlockedEventArgs args );

		void FollowedUser( object sender, UserFollowedEventArgs args );

		void FollowedByUser( object sender, UserFollowedEventArgs args );

		void UnFollowedUser( object sender, UserFollowedEventArgs args );

		void AuthenticatedUserProfileUpdated( object sender, AuthenticatedUserUpdatedEventArgs args );

		void FriendIdsReceived( object sender, GenericEventArgs<IEnumerable<long>> args );

		void AccessRevoked( object sender, AccessRevokedEventArgs args );
	}

	/// <summary>
	/// ITimeTask 인터페이스를 통해 주기적으로 실행되는 작업을 정의할 수 있습니다.
	/// </summary>
	public interface ITimeTask
	{
		void Run( );
	}


	/// <summary>
	/// 모든 모듈의 상위 클래스가 되어야 합니다.
	/// </summary>
	public abstract class Module
	{
		public string Name { get; protected set; } // 모듈의 식별용 이름입니다.
		private bool _IsRunning;
		public bool IsRunning // 모듈이 현재 켜져있는지 상태를 반환합니다.
		{
			get { return _IsRunning; }
			set
			{
				_IsRunning = value;
				if ( ModuleStateChangeListener.Count > 0 )
				{
					foreach ( var item in ModuleStateChangeListener )
					{
						item( _IsRunning );
					}
				}
			}
		}
		public bool Disposed { get; private set; }
		private List<Action<bool>> _ModuleStateChangeListener = new List<Action<bool>>();
		public List<Action<bool>> ModuleStateChangeListener // 모듈의 상태가 바뀔 때 발생하는 이벤트입니다.
		{
			get
			{
				return _ModuleStateChangeListener;
			}
		}
		protected Module( string name )
		{
			this.Name = name;
		}

		/// <summary>
		/// 새 모듈을 생성합니다.
		/// </summary>
		/// <param name="parser">모듈 INI</param>
		/// <returns></returns>
		public static Module Create( INIParser parser )
		{
			var running = parser.GetValue("Module", "IsRunning");
			var type = parser.GetValue("Module", "Type");
			var name = parser.GetValue("Module", "Name");

			Module module = null;

			if ( type == typeof( ReactorModule ).FullName )
			{
				module = new ReactorModule( name );
			}
			else if ( type == typeof( ControllerModule ).FullName )
			{
				module = new ControllerModule( name );
			}
			else if ( type == typeof( ReflectorModule ).FullName )
			{
				module = new ReflectorModule( name );
			}
			else if ( type == typeof( SchedulerModule ).FullName )
			{
				module = new SchedulerModule( name );
			}
			else if ( type == typeof( WeatherModule ).FullName )
			{
				module = new WeatherModule( name );
			}
			else if ( type == typeof( RegularTweet ).FullName )
			{
				module = new RegularTweet( name );
			}
			else
			{
				return null;
			}

			module.OpenSettings( parser );

			if ( !string.IsNullOrEmpty( running ) )
			{
				module.IsRunning = bool.Parse( running );
			}
			else
			{
				module.IsRunning = true;
			}

			return module;
		}

		protected void WriteBaseSetting( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
		}

		public abstract void OpenSettings( INIParser parser );
		public abstract void SaveSettings( INIParser parser );
		protected abstract void Release( );
		public void Dispose( )
		{
			Disposed = true;
			Release( );
		}

		#region Metadatas
		public abstract string ModuleName { get; }
		public abstract string ModuleDescription { get; }
		public abstract Module CreateModule( object[] @params );
		public abstract List<Display.ModuleFaceCategory> GetModuleFace( );
		#endregion
	}

}
