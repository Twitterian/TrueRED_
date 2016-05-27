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
							var inputset = new List<string>();
							
							var output=item.Item2;
							inputset.Add( output );

							if ( item.Item2.StartsWith( "__" ) && item.Item2.EndsWith( "__" ) )
							{
								inputset = new List<string>(StringSetsManager.GetStrings(item.Item2 .Substring(2, item.Item2 .Length-4)));
							}

							while ( true )
							{
								output = inputset[_selector.Next( inputset.Count )];
								var result = Globals.Instance.User.PublishTweet( output );
								if ( result != null )
								{
									Log.Print( this.Name, "Tweeted [{0} : {1:yyyy-MM-dd HH:mm:ss}]", result.Text, result.CreatedAt );
									break;
								}
								else
								{
									Log.Print( this.Name, "트윗하지 못했습니다 - [{0}]", output );
									inputset.Remove( output );
									if ( inputset.Count > 0 )
									{
										Log.Print( this.Name, "재시도. 남은 case 수 : {0}", inputset.Count );
									}
									else
									{
										Log.Print( this.Name, "모든 트윗이 중복으로 처리되어 시보를 트윗하지 못했습니다." );
										break;
									}
								}
							}
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
