﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules.FollowReflection
{
	class Module : Modules.Module, IStreamListener
	{
		IAuthenticatedUser user;
		public Module( IAuthenticatedUser user )
		{
			this.IsRunning = true;
			this.user = user;
		}

		void IStreamListener.AccessRevoked( object sender, AccessRevokedEventArgs args )
		{

		}

		void IStreamListener.AuthenticatedUserProfileUpdated( object sender, AuthenticatedUserUpdatedEventArgs args )
		{

		}

		void IStreamListener.BlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.FollowedByUser( object sender, UserFollowedEventArgs args )
		{
			if ( !IsRunning ) return;
			user.FollowUser( args.User );
			Log.Http( "Reflector worked", string.Format( "AutoFollowed {0}({1})", args.User.Name, args.User.ScreenName )) ;
		}

		void IStreamListener.FollowedUser( object sender, UserFollowedEventArgs args )
		{
		}

		void IStreamListener.FriendIdsReceived( object sender, GenericEventArgs<IEnumerable<long>> args )
		{

		}

		void IStreamListener.ListCreated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListDestroyed( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListUpdated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.MessageReceived( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.MessageSent( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.TweetCreateByAnyone( object sender, TweetReceivedEventArgs args )
		{
		}

		void IStreamListener.TweetFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.TweetUnFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.UnBlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.UnFollowedUser( object sender, UserFollowedEventArgs args )
		{

		}
	}
}
