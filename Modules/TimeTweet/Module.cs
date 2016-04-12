using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using TweetSharp;

namespace TrueRED.Modules.TimeTweet
{
	class Module : StreamListener
	{
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
			
		}
	}
}
