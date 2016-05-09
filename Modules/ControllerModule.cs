using System;
using System.Collections.Generic;
using TrueRED.Display;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Parameters;

namespace TrueRED.Modules
{
	class ControllerModule : Module, IStreamListener
	{
		public ControllerModule( ) : base( string.Empty )
		{
			
		}
		public ControllerModule( string name ) : base( name )
		{

		}

		public long OwnerID { get; set; }

		public override string ModuleName
		{
			get
			{
				return "Controller";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "Activaste / Deactivate modules with mention";
			}
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

			if ( tweet.CreatedBy.Id == Globals.Instance.User.Id ) return;
			if ( tweet.CreatedBy.Id != OwnerID ) return;
			if ( tweet.IsRetweet == true ) return;
			if ( tweet.InReplyToUserId != Globals.Instance.User.Id ) return;

			var modules = ModuleManager.Modules;

			if ( tweet.Text.Contains( "Deactivate" ) )
			{
				Log.Debug( this.Name, string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

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
				Log.Debug( this.Name, string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

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
				Log.Debug( this.Name, string.Format( "Owner tweet detected [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );

				GetModuleState( tweet );
			}
		}

		// TODO: 모듈이 많으면 트윗 되지 않음. 나눠서 트윗하는 함수를 만들어야함
		private void GetModuleState( ITweet tweet )
		{
			string result = string.Empty;
			var modules = ModuleManager.Modules;
			for ( int i = 0; i < modules.Count; i++ )
			{
				result += string.Format( "{0} : {1}\n", modules[i].Name, modules[i].IsRunning.ToString( ) );
			}

			var tweetResult = Globals.Instance.User.PublishTweet( string.Format( "@{0} {1}", tweet.CreatedBy.ScreenName, result ), new PublishTweetOptionalParameters()
			{
				InReplyToTweetId = tweet.Id
			});
		}

		private void GoNyang( ITweet tweet )
		{
			var modules = ModuleManager.Modules;
			foreach ( Module module in modules )
			{
				module.IsRunning = true;
			}

			var tweetResult = Globals.Instance.User.PublishTweet( string.Format( "@{0} {1}개의 모듈을 활성화했어", tweet.CreatedBy.ScreenName, modules.Count ), new PublishTweetOptionalParameters()
			{
				InReplyToTweetId = tweet.Id
			});
			Log.Debug( this.Name, "Controller에 의해 모든 모듈이 활성화되었습니다." );
		}

		private void StopNyang( ITweet tweet )
		{
			var modules = ModuleManager.Modules;
			foreach ( Module module in modules )
			{
				module.IsRunning = false;
			}

			var tweetResult = Globals.Instance.User.PublishTweet( string.Format( "@{0} {1}개의 모듈을 비활성화했어", tweet.CreatedBy.ScreenName, modules.Count ), new PublishTweetOptionalParameters()
			{
				InReplyToTweetId = tweet.Id
			});
			Log.Debug( this.Name, "Controller에 의해 모든 모듈이 비활성화되었습니다." );
		}

		private void GoNyang( ITweet tweet, Module module )
		{
			module.IsRunning = true;

			var tweetResult = Globals.Instance.User.PublishTweet( string.Format( "@{0} 모듈[{1}]을 활성화했어", tweet.CreatedBy.ScreenName, module.Name ), new PublishTweetOptionalParameters()
			{
				InReplyToTweetId = tweet.Id
			});
			Log.Debug( this.Name, module.Name + " 모듈이 활성화되었습니다." );
		}

		private void StopNyang( ITweet tweet, Module module )
		{
			module.IsRunning = false;

			var tweetResult = Globals.Instance.User.PublishTweet( string.Format( "@{0} 모듈[{1}]을 비활성화했어", tweet.CreatedBy.ScreenName, module.Name ), new PublishTweetOptionalParameters()
			{
				InReplyToTweetId = tweet.Id
			});
			Log.Debug( this.Name, module.Name + " 모듈이 비활성화되었습니다." );
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

		public override void OpenSettings( INIParser parser )
		{
			var ownerID = parser.GetValue( "Module","OwnerID" );
			if ( string.IsNullOrEmpty( ownerID ) )
			{
				IsRunning = false;
				this.OwnerID = 0;
			}
			else
			{
				this.OwnerID = long.Parse( ownerID );
			}
		}

		public override void SaveSettings( INIParser parser )
		{
			WriteBaseSetting( parser );
			parser.SetValue( "Module", "OwnerID", OwnerID );
		}

		protected override void Release( )
		{

		}

		public override Module CreateModule( object[] @params )
		{
			return new ControllerModule( @params[0].ToString( ) );
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<Display.ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new Display.ModuleFaceCategory("Module" );
			category1.Add( Display.ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			face.Add( category1 );

			return face;
		}
	}
}
