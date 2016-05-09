using System;
using System.Collections.Generic;
using TrueRED.Display;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;

namespace TrueRED.Modules
{
	public class ReflectorModule : Module, IStreamListener
	{
		public override string ModuleName
		{
			get
			{
				return "Reflector";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "Auto mutal-follow";
			}
		}

		public ReflectorModule( ) : base( string.Empty )
		{

		}
		public ReflectorModule( string name ) : base( name )
		{

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
			Globals.Instance.User.FollowUser( args.User );
			Log.Http( this.Name, string.Format( "Auto followed {0}({1})", args.User.Name, args.User.ScreenName ) );
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

		public override void OpenSettings( INIParser parser )
		{

		}

		public override void SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
		}

		protected override void Release( )
		{
		}

		public override Module CreateModule( object[] @params )
		{
			return new ReflectorModule( ( string ) @params[0] );
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new ModuleFaceCategory("Module" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			face.Add( category1 );

			return face;
		}
	}
}
