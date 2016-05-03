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
	class ControllerModule : Module, IStreamListener, IUseSetting
	{
		public static string ModuleName { get; protected set; } = "Controller";
		public static string ModuleDescription { get; protected set; } = "Activaste / Deactivate modules with mention";
		public static List<Display.ModuleFaceCategory> GetModuleFace( )
		{
			List<Display.ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			return face;
		}
		public static ControllerModule CreateModule( List<System.Windows.Forms.Control> InputForms )
		{
			return new ControllerModule( ModuleName );
		}

		public ControllerModule( string name ) : base( name )
		{

		}

		public long OwnerID { get; set; }

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

			if ( tweet.CreatedBy.Id == User.Id ) return;
			if ( tweet.CreatedBy.Id != OwnerID ) return;
			if ( tweet.IsRetweet == true ) return;
			if ( tweet.InReplyToUserId != User.Id ) return;

			var modules = Globals.Instance.Modules;

			if ( tweet.Text.Contains( "Deactivate" ) )
			{
				Log.Debug( "Controller", string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

				if ( tweet.Text.Contains( "All" ) )
				{
					StopNyang( tweet );
				}
				foreach ( var item in modules )
				{
					if ( tweet.Text.Contains( item.Name ) )
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
				foreach ( var item in modules )
				{
					if ( tweet.Text.Contains( item.Name ) )
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

		// TODO: 모듈이 많으면 트윗 되지 않을 것임. 나눠서 트윗하는 함수를 만들어야함
		private void GetModuleState( ITweet tweet )
		{
			string result = string.Empty;
			var modules = Globals.Instance.Modules;
			for ( int i = 0; i < modules.Count; i++ )
			{
				result += string.Format( "{0} : {1}\n", modules[i].Name, modules[i].IsRunning.ToString( ) );
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}", tweet.CreatedBy.ScreenName, result ), tweet.Id );
		}

		private void GoNyang( ITweet tweet )
		{
			var modules = Globals.Instance.Modules;
			foreach ( Module module in modules )
			{
				module.IsRunning = true;
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}개의 모듈을 활성화했어", tweet.CreatedBy.ScreenName, modules.Count ), tweet.Id );
			Log.Debug( "Controller", "All Module Activated" );
		}

		private void StopNyang( ITweet tweet )
		{
			var modules = Globals.Instance.Modules;
			foreach ( Module module in modules )
			{
				module.IsRunning = false;
			}
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} {1}개의 모듈을 비활성화했어", tweet.CreatedBy.ScreenName, modules.Count ), tweet.Id );
			Log.Debug( "Controller", "All Module Deactivated" );
		}

		private void GoNyang( ITweet tweet, Module module )
		{
			module.IsRunning = true;
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} 모듈[{1}]을 활성화했어", tweet.CreatedBy.ScreenName, module.Name ), tweet.Id );
			Log.Debug( "Controller", module.Name+ " Module Activated" );
		}

		private void StopNyang( ITweet tweet, Module module )
		{
			module.IsRunning = false;
			Tweet.PublishTweetInReplyTo( string.Format( "@{0} 모듈[{1}]을 비활성화했어", tweet.CreatedBy.ScreenName, module.Name ), tweet.Id );
			Log.Debug( "Controller", module.Name + " Module Deactivated" );
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

		void IUseSetting.OpenSettings( INIParser parser )
		{
			var ownerID = parser.GetValue( "Module","OwnerID" );
			if ( string.IsNullOrEmpty( ownerID ) )
			{
				IsRunning = false;
				this.OwnerID =0;
			}
			else
			{
				this.OwnerID = long.Parse( ownerID );
			}
		}

		void IUseSetting.SaveSettings( INIParser parser )
		{
			WriteBaseSetting( parser );
			parser.SetValue( "Module", "OwnerID", OwnerID );
		}

	}
}
