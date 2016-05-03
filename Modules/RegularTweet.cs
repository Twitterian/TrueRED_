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
	class RegularTweet : Module, ITimeTask, IUseSetting
	{
		public static string ModuleName { get; protected set; } = "RegularTweet";
		public static string ModuleDescription { get; protected set; } = "Random tweet";

		Random _selector = new Random();

		string stringsetname;
		string[] stringset;
		int duration;
		int variation;

		public RegularTweet( string name, IAuthenticatedUser user, IUser owner ) : base( name, user, owner )
		{

		}
		
		void ITimeTask.Run( )
		{
			while ( true )
			{
				if ( IsRunning )
				{
					var index = _selector.Next(stringset.Length);
					var result = Tweet.PublishTweet( stringset[index] );
					if ( result != null ) Log.Print( "Regular", string.Format( "Tweeted [{0}]", result.Text ) );
					else continue;
				}
				Thread.Sleep( duration - ( ( variation / 2 ) + ( _selector.Next( variation ) ) ));
			}
		}

		void IUseSetting.OpenSettings( INIParser parser )
		{
			stringsetname = parser.GetValue( "Module", "ReactorStringset" );
			var duration_val = parser.GetValue("Cycle", "Duration");
			var variation_val = parser.GetValue("Cycle", "Variation");

			stringset = StringSetsManager.GetStrings( stringsetname );

			if ( !string.IsNullOrEmpty( duration_val ) )
			{
				duration = int.Parse( duration_val );
			}
			else
			{
				duration = 10;
			}

			if ( !string.IsNullOrEmpty( variation_val ) )
			{
				variation = int.Parse( variation_val );
			}
			else
			{
				variation = 5;
			}
		}

		void IUseSetting.SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
			parser.SetValue( "Module", "ReactorStringset", stringsetname );
			
			parser.SetValue( "Cycle", "Duration", duration );
			parser.SetValue( "Cycle", "Variation", variation );
		}
	}
}
