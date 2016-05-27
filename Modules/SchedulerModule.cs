using System;
using System.Collections.Generic;
using System.Threading;
using TrueRED.Display;
using TrueRED.Framework;

namespace TrueRED.Modules
{
	public class SchedulerModule : Module, ITimeTask
	{
		public override string ModuleName
		{
			get
			{
				return "Scheduler";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "Tweet on setted time";
			}
		}

		List<Tuple<TimeSet, string>> pair = new List<Tuple<TimeSet, string>>();
		private string stringset;
		private Random _selector = new Random();

		public SchedulerModule( ) : base( string.Empty )
		{

		}
		public SchedulerModule( string name ) : base( name )
		{

		}

		public void LoadStringsets( string stringSet )
		{
			var scheduler = StringSetsManager.GetStrings(stringSet);

			for ( int i = 0; i < scheduler.Length; i++ )
			{
				var tags = scheduler[i].Split('∥');
				if ( tags.Length != 3 )
				{
					Log.Error( this.Name, "Not correct scheduler stringset {0}", scheduler[i] );
					continue;
				}
				pair.Add( new Tuple<TimeSet, string>( new TimeSet( int.Parse( tags[0] ), int.Parse( tags[1] ) ), tags[2] ) );
			}
		}

		void ITimeTask.Run( )
		{
			while ( !Disposed )
			{
				if ( IsRunning )
				{
					foreach ( var item in pair )
					{
						if ( DateTime.Now.Hour == item.Item1.Hour &&
							 DateTime.Now.Minute == item.Item1.Minute &&
							 DateTime.Now.Second == 0 )
						{
							var output=item.Item2;
							if ( item.Item2.StartsWith( "__" ) && item.Item2.EndsWith( "__" ) )
							{
								var inputset = StringSetsManager.GetStrings(item.Item2 .Substring(2, item.Item2 .Length-4));
								output = inputset[_selector.Next( inputset.Length )];
							}
							var tweet= Globals.Instance.User.PublishTweet( output );
							Log.Print( this.Name, "Tweeted [{0} : {1:yyyy-MM-dd HH:mm:ss}]", tweet.Text, tweet.CreatedAt );
						}
					}
				}
				Thread.Sleep( 500 );
			}
		}

		public override void OpenSettings( INIParser parser )
		{
			stringset = parser.GetValue( "Module", "ReactorStringset" );

			LoadStringsets( stringset );
		}

		public override void SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
			parser.SetValue( "Module", "ReactorStringset", stringset );
		}

		protected override void Release( )
		{

		}

		public override Module CreateModule( object[] @params )
		{
			var module =new SchedulerModule((string)@params[0]);
			module.stringset = ( string ) @params[1];
			module.LoadStringsets( module.stringset );
			return module;
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new ModuleFaceCategory("Module" );
            category1.Add( ModuleFaceTypes.String, "모듈 이름" );
			category1.Add( ModuleFaceTypes.String, "문자셋" );
			face.Add( category1 );

			return face;
		}

	}
}
