using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
	class SchedulerModule : Modules.Module, ITimeTask, IUseSetting
	{
		IAuthenticatedUser user;
		private string StringsetPath;
		List<Tuple<TimeSet, string>> pair = new List<Tuple<TimeSet, string>>();

		public SchedulerModule( IAuthenticatedUser user, string stringsetPath )
		{
			this.IsRunning = true;
			this.StringsetPath = stringsetPath;
			LoadStringsets( stringsetPath );
		}

		public void LoadStringsets( string stringSet )
		{
			var scheduler = StringSetsManager.GetStrings(stringSet);

			for ( int i = 0; i < scheduler.Length; i++ )
			{
				var tags = scheduler[i].Split('∥');
				if ( tags.Length != 3 )
				{
					Log.Error( "Scheduler.Status", string.Format( "Not correct scheduler stringset {0}", scheduler[i] ) );
					continue;
				}
				pair.Add( new Tuple<TimeSet, string>( new TimeSet( int.Parse( tags[0] ), int.Parse( tags[1] ) ), tags[2] ) );
			}
		}

		void ITimeTask.Run( )
		{
			while ( true )
			{
				if ( IsRunning )
				{
					foreach ( var item in pair )
					{
						if ( DateTime.Now.Hour == item.Item1.Hour &&
						DateTime.Now.Minute == item.Item1.Minute &&
						DateTime.Now.Second == 0 )
						{
							var tweet= Tweet.PublishTweet( item.Item2 );
							Log.Print( "Scheduler tweet", string.Format( "[{0} : {1}]", tweet.Text, tweet.CreatedAt.ToString( ) ) );
						}
					}
				}
				Thread.Sleep( 1000 );
			}
		}

		void IUseSetting.OpenSettings( string path )
		{
			var setting = new INIParser(path);
			var running = setting.GetValue("Module", "IsRunning");

			if ( !string.IsNullOrEmpty( running ) )
			{
				IsRunning = bool.Parse( running );
			}
			else
			{
				IsRunning = true;
			}
		}

		void IUseSetting.SaveSettings( string path )
		{
			var setting = new INIParser(path);
			setting.SetValue( "Module", "IsRunning", IsRunning.ToString( ) );
		}
	}
}
