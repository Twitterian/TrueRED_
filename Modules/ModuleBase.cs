using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace TrueRED.Modules
{
	public interface StreamListener
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


	public interface TimeLimiter
	{
		bool Verification( );
	}

	public interface UseSetting
	{
		bool OpenSettings( );

		bool SaveSettings( );
	}

	public interface TimeTask
	{
		void Run( );
	}
}
