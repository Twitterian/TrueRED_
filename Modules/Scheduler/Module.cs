using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using TweetSharp;

namespace TrueRED.Modules.Scheduler
{
	class Module : UseSetting
	{
		private TwitterService twitter;
		private long Id;
		private string iniPath;

		public Module( TwitterService twitter, long userId, string iniPath )
		{
			this.twitter = twitter;
			this.Id = userId;
			this.iniPath = iniPath;
		}

		public bool OpenSettings( )
		{
			return true;
		}

		public bool SaveSettings( )
		{
			return true;
		}
	}
}
