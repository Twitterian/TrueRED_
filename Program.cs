using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using TrueRED.Modules;
using TweetSharp;

namespace TrueRED
{
	class Program
	{
		static void Main( string[] args )
		{
			Log.Init( );

			const string consumerKey = "kbxPh2hxMFm0sazIE81TKnk2a";
			const string consumerSecret = "nPKGe0t5EoQB9ssMa9geOjkqwKEslMXfaKuX4Kvw3TAFGY47P3";
			const string accessToken = "4071252374-PpVVKvHtE4s0Fb7MSeWIFinggnOPpeQ9RFnnEfi";
			const string accessSecret = "x0j9sVc12cz0YGAeGwkmPF046uUH4JVgi0YTPKw9dDUK0";

			// Initialize user authenticate.
			var twitter = new TwitterService(consumerKey, consumerSecret);
			twitter.AuthenticateWith( accessToken, accessSecret );

			// Initialize parameters
			var id = twitter.VerifyCredentials(new VerifyCredentialsOptions
			{
				SkipStatus = true
			}).Id;

			Log.Debug( "User Id is", id.ToString() );

			// Initialize modules
			var goodbye = new Modules.Reactor.Module( twitter, id );
            var timetweet = new Modules.TimeTweet.Module( );

			var streamModules = new List<StreamListener>();
			streamModules.Add( goodbye );

			CreateStream( twitter, streamModules );

			new Display.AppConsole( ).ShowDialog( );
		}

		static void CreateStream( TwitterService service, List<StreamListener> modules )
		{
			service.StreamUser( ( streamEvent, response ) =>
			{
				if ( streamEvent is TwitterUserStreamEnd )
				{
					var end = ( TwitterUserStreamEnd ) streamEvent;
					Log.Error( "TwitterUserStreamEnd", end.RawSource );

					foreach ( var item in modules )
					{
						item.End( end );
					}
				}

				if ( response.StatusCode == 0 )
				{
					if ( streamEvent is TwitterUserStreamFriends )
					{
						var friends = ( TwitterUserStreamFriends ) streamEvent;
						foreach ( var item in modules )
						{
							item.Friends( friends );
						}
					}

					if ( streamEvent is TwitterUserStreamEvent )
					{
						var @event = (TwitterUserStreamEvent)streamEvent;
						foreach ( var item in modules )
						{
							item.Event( @event );
						}
					}

					if ( streamEvent is TwitterUserStreamStatus )
					{
						var status = ((TwitterUserStreamStatus)streamEvent).Status;
						foreach ( var item in modules )
						{
							item.Status( status );
						}
					}

					if ( streamEvent is TwitterUserStreamDirectMessage )
					{
						var dm = ((TwitterUserStreamDirectMessage)streamEvent).DirectMessage;
						foreach ( var item in modules )
						{
							item.DirectMessage( dm );
						}
					}

					if ( streamEvent is TwitterUserStreamDeleteStatus )
					{
						var deleted = (TwitterUserStreamDeleteStatus)streamEvent;
						foreach ( var item in modules )
						{
							item.DeleteStatus( deleted );
						}
					}

					if ( streamEvent is TwitterUserStreamDeleteDirectMessage )
					{
						var deleted = (TwitterUserStreamDeleteDirectMessage)streamEvent;
						foreach ( var item in modules )
						{
							item.DeleteDirectMessage( deleted );
						}
					}
				}
			} );
		}
	}
}
