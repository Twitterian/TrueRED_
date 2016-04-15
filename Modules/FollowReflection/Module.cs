using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules.FollowReflection
{
	class Module : StreamListener
	{
		IAuthenticatedUser user;
		public Module( IAuthenticatedUser user )
		{
			this.user = user;
		}

		void StreamListener.AccessRevoked( object sender, AccessRevokedEventArgs args )
		{

		}

		void StreamListener.AuthenticatedUserProfileUpdated( object sender, AuthenticatedUserUpdatedEventArgs args )
		{

		}

		void StreamListener.BlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void StreamListener.FollowedByUser( object sender, UserFollowedEventArgs args )
		{
			user.FollowUser( args.User );
			Log.Http( "followrelection", string.Format( "AutoFollowed {0}({1})", args.User.Name, args.User.ScreenName )) ;
		}

		void StreamListener.FollowedUser( object sender, UserFollowedEventArgs args )
		{
		}

		void StreamListener.FriendIdsReceived( object sender, GenericEventArgs<IEnumerable<long>> args )
		{

		}

		void StreamListener.ListCreated( object sender, ListEventArgs args )
		{

		}

		void StreamListener.ListDestroyed( object sender, ListEventArgs args )
		{

		}

		void StreamListener.ListUpdated( object sender, ListEventArgs args )
		{

		}

		void StreamListener.MessageReceived( object sender, MessageEventArgs args )
		{

		}

		void StreamListener.MessageSent( object sender, MessageEventArgs args )
		{

		}

		void StreamListener.TweetCreateByAnyone( object sender, TweetReceivedEventArgs args )
		{
		}

		void StreamListener.TweetFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void StreamListener.TweetUnFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void StreamListener.UnBlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void StreamListener.UnFollowedUser( object sender, UserFollowedEventArgs args )
		{

		}
	}
}
