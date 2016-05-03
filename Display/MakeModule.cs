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
		}

		private void ModuleFace_DoneButton_Click( object sender, EventArgs e )
		{
			Framework.Log.Print( "Callback", "Hello" );
		}

		void LoadModuleTypes( )
		{
			var types = typeof(Modules.Module).Assembly.GetTypes().Where(t => t.BaseType == typeof(Modules.Module)).ToList();
			var modules = new Tuple<Type, TabPage>[types.Count()];
			for ( int i = 0; i < types.Count( ); i++ )
			{
				modules[i] = new Tuple<Type, TabPage>( types[i], new ModuleFace(
					types[i].Name,
					( IEnumerable<ModuleFaceCategory> ) types[i].GetMethod( "GetModuleFace" ).Invoke( null, null ),
					ModuleFace_DoneButton_Click
				) );
			}

			for ( int i = 0; i < modules.Length; i++ )
			{
				tabpage_modules.SuspendLayout( );
				var radiobutton = NewModuleType(i, modules[i].Item1);
				if ( radiobutton == null ) continue;
				int index = i;
				radiobutton.CheckedChanged += delegate
				{
					if ( radiobutton.Checked )
						tabControl.Controls.Add( modules[index].Item2 );
					else
						tabControl.Controls.Remove( modules[index].Item2 );

					materialSkinManager.ColorScheme = new ColorScheme( Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE );
				};
				tabpage_modules.Controls.Add( radiobutton );
				tabpage_modules.ResumeLayout( );
			}
			if ( tabpage_modules.Controls.Count > 0 )
			{
				tabpage_modules.Controls[0].Select( );
			}
			else
			{
				// TODO: 모듈 없음 에러
			}
		}

		MaterialRadioButton NewModuleType( int i, Type module )
		{
			MaterialRadioButton button = new MaterialRadioButton();
			button.AutoSize = true;
			button.Depth = 0;
			button.Font = new System.Drawing.Font( "Roboto", 10F );
			button.Location = new Point( 10, 10 + 31 * i );
			button.Margin = new System.Windows.Forms.Padding( 0 );
			button.MouseLocation = new System.Drawing.Point( -1, -1 );
			button.MouseState = MaterialSkin.MouseState.HOVER;
			button.Text = module.GetProperty( "ModuleName" ).GetValue( module ).ToString( ) + " : " + module.GetProperty( "ModuleDescription" ).GetValue( module ).ToString( );
			button.Ripple = true;
			button.Size = new System.Drawing.Size( 163, 30 );
			button.TabIndex = 10;
			button.TabStop = true;
			button.Name = module.Name;
			button.UseVisualStyleBackColor = true;
			return button;
		}


	}
}
