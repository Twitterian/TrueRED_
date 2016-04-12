using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace TrueRED.Modules
{
	public interface StreamListener
	{
		void Friends( TwitterUserStreamFriends friends );

		void Event( TwitterUserStreamEvent @event );

		void Status( TwitterStatus status );

		void DirectMessage( TwitterDirectMessage dm );

		void DeleteStatus( TwitterUserStreamDeleteStatus delete );

		void DeleteDirectMessage( TwitterUserStreamDeleteDirectMessage delete );

		void End( TwitterUserStreamEnd end );
	}


	public interface TimeLimiter
	{
		bool Verification( );
	}

	public interface UseSetting
	{
		bool OpenSettings( );

		bool SaveSettings( );
	}
}
