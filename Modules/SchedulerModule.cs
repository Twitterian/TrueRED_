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
	public class SchedulerModule : Modules.Module, ITimeTask, IUseSetting
	{

		public static string ModuleName { get; protected set; } = "Scheduler";
		public static string ModuleDescription { get; protected set; } = "Tweet on setted time";
		public static List<Display.ModuleFaceCategory> GetModuleFace( )
		{
			List<Display.ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new Display.ModuleFaceCategory("Module" );
			category1.Add( Display.ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			category1.Add( Display.ModuleFaceCategory.ModuleFaceTypes.String, "문자셋" );
			face.Add( category1 );

			return face;
		}
		public static SchedulerModule CreateModule( List<System.Windows.Forms.Control> InputForms )
		{
			return null;
		}

		List<Tuple<TimeSet, string>> pair = new List<Tuple<TimeSet, string>>();
		private string stringset;

		public SchedulerModule( string name) : base( name)
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
					Log.Error( "Scheduler.Status", string.Format( "Not correct scheduler stringset {0}", scheduler[i] ) );
					continue;
				}
				pair.Add( new Tuple<TimeSet, string>( new TimeSet( int.Parse( tags[0] ), int.Parse( tags[1] ) ), tags[2] ) );
			}
		}

		void ITimeTask.Run( )
		{
			while ( true )
			{
				if ( IsRunning )
				{
					foreach ( var item in pair )
					{
						if ( DateTime.Now.Hour == item.Item1.Hour &&
						DateTime.Now.Minute == item.Item1.Minute &&
						DateTime.Now.Second == 0 )
						{
							var tweet= Tweet.PublishTweet( item.Item2 );
							Log.Print( "Scheduler tweet", string.Format( "[{0} : {1}]", tweet.Text, tweet.CreatedAt.ToString( ) ) );
						}
					}
				}
				Thread.Sleep( 1000 );
			}
		}

		void IUseSetting.OpenSettings( INIParser parser )
		{
			stringset = parser.GetValue("Module", "ReactorStringset");

			LoadStringsets( stringset );
		}

		void IUseSetting.SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
			parser.SetValue( "Module", "ReactorStringset", stringset );
		}
	}
}
