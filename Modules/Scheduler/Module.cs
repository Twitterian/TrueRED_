using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules.Scheduler
{
	class Module : UseSetting
	{
		IAuthenticatedUser user;
		private long Id;
		private string iniPath;

		public Module( IAuthenticatedUser user, string iniPath )
		{
			this.user = user;
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
