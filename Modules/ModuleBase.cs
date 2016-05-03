using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
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
	/// IUserSetting 인터페이스를 통해 별도 ini파일의 설정을 사용할 수 있습니다.
	/// </summary>
	public interface IUseSetting
	{
		void OpenSettings( INIParser parser );
		void SaveSettings( INIParser parser );
	}

	/// <summary>
	/// ITimeTask 인터페이스를 통해 주기적으로 실행되는 작업을 정의할 수 있습니다.
	/// </summary>
	public interface ITimeTask
	{
		void Run( );
	}

	public class Module
	{
		private bool _IsRunning;
		public bool IsRunning
		{
			get { return _IsRunning; }
			set
			{
				_IsRunning = value;
				if ( ModuleStateChangeListener.Count > 0 )
				{
					foreach ( var item in ModuleStateChangeListener )
					{
						item.Value( item.Key, _IsRunning );
					}
				}
			}
		}

		public string Name { get; private set; }
		protected IAuthenticatedUser User { get; private set; }

		private Dictionary<int, Action<int, bool>> _ModuleStateChangeListener = new Dictionary<int, Action<int, bool>>();
		public Dictionary<int, Action<int, bool>> ModuleStateChangeListener
		{
			get
			{
				return _ModuleStateChangeListener;
			}
		}

		protected Module( string name )
		{
			this.Name = name;
			this.User = Globals.Instance.User;
		}

		public static Module Create( INIParser parser )
		{
			var running = parser.GetValue("Module", "IsRunning");
			var type = parser.GetValue("Module", "Type");
			var name = parser.GetValue("Module", "Name");

			Module module = null;

			if ( type == typeof( ReactorModule ).FullName )
			{
				module = new ReactorModule( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else if ( type == typeof( ControllerModule ).FullName )
			{
				module = new ControllerModule( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else if ( type == typeof( ReflectorModule ).FullName )
			{
				module = new ReflectorModule( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else if ( type == typeof( SchedulerModule ).FullName )
			{
				module = new SchedulerModule( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else if ( type == typeof( WeatherModule ).FullName )
			{
				module = new WeatherModule( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else if ( type == typeof( RegularTweet ).FullName )
			{
				module = new RegularTweet( name );
				( ( IUseSetting ) module ).OpenSettings( parser );
			}
			else
			{
				return null;
			}

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

	}
}
