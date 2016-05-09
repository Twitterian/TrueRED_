using System;
using System.Collections.Generic;
using System.Threading;
using TrueRED.Display;
using TrueRED.Framework;
using Tweetinvi;

namespace TrueRED.Modules
{
	class RegularTweet : Module, ITimeTask
	{
		public override string ModuleName
		{
			get
			{
				return "RegularTweet";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "Random tweet";
			}
		}		

		Random _selector = new Random();

		string stringsetname;
		string[] stringset;
		int duration;
		int variation;

		public RegularTweet( ) : base( string.Empty )
		{

		}
		public RegularTweet( string name ) : base( name )
		{

		}

		void ITimeTask.Run( )
		{
			while ( !Disposed )
			{
				if ( IsRunning )
				{
					var index = _selector.Next(stringset.Length);
					var result = Globals.Instance.User.PublishTweet( stringset[index] );
					if ( result != null ) Log.Print( this.Name, string.Format( "Tweeted [{0}]", result.Text ) );
					else continue;
				}
				Thread.Sleep( duration - ( ( variation / 2 ) + ( _selector.Next( variation ) ) ) );
			}
		}

		public override void OpenSettings( INIParser parser )
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

		public override void SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
			parser.SetValue( "Module", "ReactorStringset", stringsetname );

			parser.SetValue( "Cycle", "Duration", duration );
			parser.SetValue( "Cycle", "Variation", variation );
		}

		protected override void Release( )
		{

		}

		public override Module CreateModule( object[] @params )
		{
			var module = new RegularTweet( (string)@params[0] );
			module.stringsetname = ( string ) @params[1];
			module.stringset = StringSetsManager.GetStrings( module.stringsetname );
			module.duration = ( int ) @params[2];
			module.variation = ( int ) @params[3];
			module.IsRunning = false;
			return module;
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new ModuleFaceCategory("Module" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "문자셋" );
			face.Add( category1 );

			var category2 = new ModuleFaceCategory("Cycle" );
			category2.Add( ModuleFaceCategory.ModuleFaceTypes.Int, "Duration" );
			category2.Add( ModuleFaceCategory.ModuleFaceTypes.Int, "Variation" );
			face.Add( category2 );

			return face;
		}
	}
}
