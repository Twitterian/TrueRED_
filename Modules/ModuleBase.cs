using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

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
	/// ITimeLimiter 인터페이스를 통해 스크립트가 작동할 시간대를 설정할 수 있습니다.
	/// </summary>
	public interface ITimeLimiter
	{
		bool Verification( );
	}

	/// <summary>
	/// IUserSetting 인터페이스를 통해 별도 ini파일의 설정을 사용할 수 있습니다.
	/// </summary>
	public interface IUseSetting
	{
		void OpenSettings( string path );

		void SaveSettings( string path );
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
		public bool IsRunning { get; set; }
	}
}
