using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using TweetSharp;

namespace TrueRED.Modules.Reactor
{
	class Module : StreamListener
	{
		private TwitterService twitter;
		private long Id;

		public Module( TwitterService twitter, long userid)
		{
			this.twitter = twitter;
			this.Id = userid;
		}

		public void DeleteDirectMessage( TwitterUserStreamDeleteDirectMessage delete )
		{

		}

		public void DeleteStatus( TwitterUserStreamDeleteStatus delete )
		{

		}

		public void DirectMessage( TwitterDirectMessage dm )
		{

		}

		public void End( TwitterUserStreamEnd end )
		{

		}

		public void Event( TwitterUserStreamEvent @event )
		{

		}

		public void Friends( TwitterUserStreamFriends friends )
		{

		}

		public void Status( TwitterStatus status )
		{
			if ( status.User.Id == Id ) return;
			if ( status.RetweetedStatus != null ) return;


		}
	}
}
