using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using TrueRED.Modules;

namespace TrueRED.Display
{
	public partial class MakeModule : MaterialForm
	{
		private readonly MaterialSkinManager materialSkinManager;
		List<Type> types = null;
		private string LogHeader { get; set; } = "MakeModule";

		public MakeModule( )
		{
			InitializeComponent( );

			materialSkinManager = MaterialSkinManager.Instance;
			materialSkinManager.AddFormToManage( this );
			materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
			materialSkinManager.ColorScheme = new ColorScheme( Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE );

			LoadModuleTypes( );
		}


		void LoadModuleTypes( )
		{
			types = typeof( Modules.Module ).Assembly.GetTypes( ).Where( t => t.BaseType == typeof( Modules.Module ) ).ToList( );
			var modules = new Tuple<Type, TabPage>[types.Count()];
			for ( int i = 0; i < types.Count( ); i++ )
			{
				//TODO: Maybe heavycost
				var factory = ( Module ) Activator.CreateInstance( types[i] );
				modules[i] = new Tuple<Type, TabPage>( types[i], new ModuleFace(
					types[i],
					factory.GetModuleFace( ),
					delegate ( Type t, object[] @params )
					{
						var instance = factory.CreateModule(@params);
						if ( instance != null )
						{
							Framework.Log.Print( LogHeader, string.Format( "New {0} Module [{1}] maked.", instance.GetType( ).Name, instance.Name ) );
							Framework.ModuleManager.Modules.Add( instance );
						}
						this.Close( );
					}
				) );
			}

			tabpage_modules.SuspendLayout( );
			AttachDoneButton( );
			for ( int i = 0; i < modules.Length; i++ )
			{
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
			}
			if ( tabpage_modules.Controls.Count > 0 )
			{
				tabpage_modules.Controls[0].Select( );
			}
			tabpage_modules.ResumeLayout( );
		}

		private void AttachDoneButton( )
		{
			var button = new MaterialFlatButton();
			button.Text = "Done";
			button.Location = new Point( tabpage_modules.Size.Width - button.Size.Width - 10, tabpage_modules.Size.Height - button.Size.Height );
			button.Click += delegate
			{
				tabControl.SelectTab( 1 );
			};
			button.Anchor = ( AnchorStyles.Bottom | AnchorStyles.Right );
			tabpage_modules.Controls.Add( button );
		}

		MaterialRadioButton NewModuleType( int i, Type module )
		{
			//TODO: Maybe heavycost
			var factory = ( Module ) Activator.CreateInstance( module );
			MaterialRadioButton button = new MaterialRadioButton();
			button.AutoSize = true;
			button.Depth = 0;
			button.Font = new System.Drawing.Font( "Roboto", 10F );
			button.Location = new Point( 10, 10 + 31 * i );
			button.Margin = new System.Windows.Forms.Padding( 0 );
			button.MouseLocation = new System.Drawing.Point( -1, -1 );
			button.MouseState = MaterialSkin.MouseState.HOVER;
			button.Text = factory.ModuleName + " : " + factory.ModuleDescription;
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
