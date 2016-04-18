using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
	class ControllerModule : Module, IStreamListener
	{
		IAuthenticatedUser user;
		IUser owner;
		Dictionary<string, Module> modules;

		public ControllerModule( IAuthenticatedUser user, IUser owner, Dictionary<string, Modules.Module> modules )
		{
			this.user = user;
			this.owner = owner;
			this.modules = modules;
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
			var tweet = args.Tweet;

			if ( tweet.CreatedBy.Id == user.Id ) return;
			if ( tweet.CreatedBy.Id != owner.Id ) return;
			if ( tweet.IsRetweet == true ) return;
			if ( tweet.InReplyToUserId != user.Id ) return;

			if ( tweet.Text.Contains( "Deactivate" ) )
			{
				Log.Debug( "Controller", string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

				if ( tweet.Text.Contains( "All" ) )
				{
					StopNyang( tweet );
				}
				foreach ( var item in modules.Keys )
				{
					if ( tweet.Text.Contains( item ) )
					{
						StopNyang( tweet, item );
					}
				}
			}
			else if ( tweet.Text.Contains( "Activate" ) )
			{
				Log.Debug( "Controller", string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

				if ( tweet.Text.Contains( "All" ) )
				{
					GoNyang( tweet );
				}
				foreach ( var item in modules.Keys )
				{
					if ( tweet.Text.Contains( item ) )
					{
						GoNyang( tweet, item );
					}
				}
			}
			else if ( tweet.Text.Contains( "GetModuleState" ) )
			{
				Log.Debug( "Controller", string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

				GetModuleState( tweet );
			}
		}

		// To-do : 모듈이 많으면 트윗 되지 않을 것임. 나눠서 트윗하는 함수를 만들어야함
		private void GetModuleState( ITweet tweet )
		{
			if ( modules == null ) Log.Error( "Controller", "Modules undefined" );
			string result = string.Empty;
			for ( int i = 0; i < modules.Count; i++ )
			{
				result += string.Format( "{0} : {1}\n", modules.Keys.ToArray( )[i], modules.Values.ToArray( )[i].IsRunning.ToString( ) );
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}", owner.ScreenName, result ), tweet.Id );
		}

		private void GoNyang( ITweet tweet )
		{
			if ( modules == null ) Log.Error( "Controller", "Modules undefined" );
			foreach ( Modules.Module module in modules.Values )
			{
				module.IsRunning = true;
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}개의 모듈을 활성화 했어", owner.ScreenName, modules.Count ), tweet.Id );
			Log.Debug( "Controller", "All Module Activated" );
		}

		private void StopNyang( ITweet tweet )
		{
			if ( modules == null ) Log.Error( "Controller", "Modules undefined" );
			foreach ( Modules.Module module in modules.Values )
			{
				module.IsRunning = false;
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}개의 모듈을 비활성화 했어", owner.ScreenName, modules.Count ), tweet.Id );
			Log.Debug( "Controller", "All Module Deactivated" );
		}

		private void GoNyang( ITweet tweet, string module )
		{
			if ( modules == null ) Log.Error( "Controller", "Modules undefined" );
			if ( modules.ContainsKey( module ) )
			{
				modules[module].IsRunning = true;
				Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1} 모듈을 활성화 했어", owner.ScreenName, module ), tweet.Id );
				Log.Debug( "Controller", module + " Module Activated" );
			}
			else
			{
				Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1} 모듈을 찾을 수 없었어", owner.ScreenName, module ), tweet.Id );
				Log.Debug( "Controller", module + " Not Found" );
			}
		}

		private void StopNyang( ITweet tweet, string module )
		{
			if ( modules == null ) Log.Error( "Controller", "Modules undefined" );
			if ( modules.ContainsKey( module ) )
			{
				modules[module].IsRunning = false;
				Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1} 모듈을 비활성화 했어", owner.ScreenName, module ), tweet.Id );
				Log.Debug( "Controller", module + " Module Deactivated" );
			}
			else
			{
				Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1} 모듈을 찾을 수 없었어", owner.ScreenName, module ), tweet.Id );
				Log.Debug( "Controller", module + " Not Found" );
			}
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
