using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules.Scheduler
{
	class Module : TimeTask
	{
		public bool isActive { get; set; }
		IAuthenticatedUser user;
		private string stringsetPath;
		List<Tuple<TimeSet, string>> pair = new List<Tuple<TimeSet, string>>();

		public Module( IAuthenticatedUser user, string stringsetPath )
		{
			this.user = user;
			this.stringsetPath = stringsetPath;
			isActive = true;
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

		void TimeTask.Run( )
		{
			while ( isActive )
			{
				foreach ( var item in pair )
				{
					if ( DateTime.Now.Hour == item.Item1.Hour &&
					DateTime.Now.Minute == item.Item1.Minute &&
					DateTime.Now.Second == 0 )
					{
						Tweet.PublishTweet( item.Item2 );
					}
				}
				Thread.Sleep( 1000 );
			}
		}
	}
}
