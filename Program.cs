using System;
using System.Collections.Generic;
using System.IO;
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
			#region Initialize Program

			Log.Init( );
			StringSetsManager.LoadStringSets( "Stringsets" );

			var setting = new INIParser( Path.Combine( Directory.GetCurrentDirectory( ), "setting.ini" ) );
			string consumerKey = setting.GetValue( "Authenticate", "ConsumerKey" );
			string consumerSecret = setting.GetValue( "Authenticate", "CconsumerSecret" );
			string accessToken = setting.GetValue( "Authenticate", "AccessToken" );
			string accessSecret = setting.GetValue( "Authenticate", "AccessSecret" );

			var twitter = new TwitterService(consumerKey, consumerSecret);
			twitter.AuthenticateWith( accessToken, accessSecret );

			var id = twitter.VerifyCredentials(new VerifyCredentialsOptions
			{
				SkipStatus = true
			}).Id;

			Log.Debug( "User ID is : ", id.ToString( ) );

			#endregion

			#region Initialize Modules

			var YoruHello = new Modules.Reactor.Module( twitter, id, "YoruHelloReactor", new TimeSet(20), new TimeSet(27));
			var TimeTweet = new Modules.Scheduler.Module(twitter, id, "Settings/TimeTweet.ini" );

			var iniModules = new List<UseSetting>();
			iniModules.Add( TimeTweet );

			var streamModules = new List<StreamListener>();
			streamModules.Add( YoruHello );

			#endregion

			foreach ( var item in iniModules )
			{
				item.OpenSettings( );
			}

			CreateStream( twitter, streamModules );

			new Display.AppConsole( ).ShowDialog( );
			
			foreach ( var item in iniModules )
			{
				item.SaveSettings( );
			}
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
