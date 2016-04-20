using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace TrueRED.Display
{
	public partial class MakeModule : MaterialForm
	{
		private readonly MaterialSkinManager materialSkinManager;
		public MakeModule( )
		{
			InitializeComponent( );
			
			// Initialize MaterialSkinManager
			materialSkinManager = MaterialSkinManager.Instance;
			materialSkinManager.AddFormToManage( this );
			materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
			materialSkinManager.ColorScheme = new ColorScheme( Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE );

			LoadModuleTypes( );
			LoadModuleConfigs( );
		}

		void LoadModuleTypes( )
		{
			var modules = new Type[]
			{
				typeof(Modules.ReactorModule),
				typeof(Modules.ReflectorModule),
				typeof(Modules.SchedulerModule),
				typeof(Modules.WeatherModule)
			};

			for ( int i = 0; i < modules.Length; i++ )
			{
				tabpage_modules.SuspendLayout( );
				MaterialRadioButton button = new MaterialRadioButton();
				button.AutoSize = true;
				button.Depth = 0;
				button.Font = new System.Drawing.Font( "Roboto", 10F );
				button.Location = new Point( 10, 31 * i );
				button.Margin = new System.Windows.Forms.Padding( 0 );
				button.MouseLocation = new System.Drawing.Point( -1, -1 );
				button.MouseState = MaterialSkin.MouseState.HOVER;
				button.Text = modules[i].GetProperty("ModuleName").GetValue(modules[i]).ToString() + " : " + modules[i].GetProperty( "ModuleDescription" ).GetValue( modules[i] ).ToString( );
				button.Ripple = true;
				button.Size = new System.Drawing.Size( 163, 30 );
				button.TabIndex = 10;
				button.TabStop = true;
				button.Name = modules[i].Name;
				button.UseVisualStyleBackColor = true;
				if ( i == 0 ) button.Select( );

				tabpage_modules.Controls.Add( button );
				tabpage_modules.ResumeLayout( );
			}
		}

		void AddModuleType( )
		{

		}

		void LoadModuleConfigs( )
		{

		}

		void AddModuleConfig( )
		{

		}
	}
}
