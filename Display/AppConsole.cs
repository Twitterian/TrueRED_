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
using TrueRED.Framework;
using TrueRED.Modules;

namespace TrueRED.Display
{
	public partial class AppConsole : MaterialForm
	{
		private readonly MaterialSkinManager materialSkinManager;
		public DataSet ds { get; private set; }

		public AppConsole( )
		{
			InitializeComponent( );
			
			materialSkinManager = MaterialSkinManager.Instance;
			materialSkinManager.AddFormToManage( this );
			materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
			materialSkinManager.ColorScheme = new ColorScheme( Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE );
			
			for ( int i = 0; i < Globals.Instance.Modules.Count; i++ )
			{
				checkedlistbox_modules.Items.Add( Globals.Instance.Modules[i].Name, Globals.Instance.Modules[i].IsRunning );
					var index = i;
				Globals.Instance.Modules[i].ModuleStateChangeListener.Add( delegate ( bool running )
				{
					this.Invoke( new MethodInvoker( delegate
					{
						this.checkedlistbox_modules.ItemCheck -= this.checkedlistbox_modules_ItemCheck;
						checkedlistbox_modules.SetItemChecked( index, Globals.Instance.Modules[index].IsRunning );
						this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
					} ) );
				} );
				checkedlistbox_modules.SetItemChecked( i, Globals.Instance.Modules[i].IsRunning );
			}
			this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;

			Globals.Instance.Modules.OnModuleAttachLiestner.Add( delegate ( Module module )
			 {
				 var index = checkedlistbox_modules.Items.Count;
				 checkedlistbox_modules.Items.Add( module.Name, module.IsRunning );
				 module.ModuleStateChangeListener.Add( delegate ( bool running )
				 {
					 this.Invoke( new MethodInvoker( delegate
					 {
						 this.checkedlistbox_modules.ItemCheck -= this.checkedlistbox_modules_ItemCheck;
						 checkedlistbox_modules.SetItemChecked( index, Globals.Instance.Modules[index].IsRunning );
						 this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
					 } ) );
				 } );
				 checkedlistbox_modules.SetItemChecked( index, module.IsRunning );
			 } );
		}

		private void button_exit_Click( object sender, EventArgs e )
		{
			this.Close( );
		}

		private void button_getModule_Click( object sender, EventArgs e )
		{
			string result = string.Empty;
			foreach ( var item in Globals.Instance.Modules )
			{
				result += string.Format( "\n    {0} : {1}", item.Name, item.IsRunning.ToString( ) );
			}
			Log.Debug( "AppConsole", string.Format( "{0}", result ) );
		}

		private void checkedlistbox_modules_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			var i = checkedlistbox_modules.SelectedIndex;
			Globals.Instance.Modules[i].IsRunning = !Globals.Instance.Modules[i].IsRunning;

			Log.Debug( "AppConsole", string.Format( "{0} 모듈이 AppConsole에 의하여 {1}활성화", Globals.Instance.Modules[i].Name, ( Globals.Instance.Modules[i].IsRunning ? "" : "비" ) ) );
		}

		private void button_console_Click( object sender, EventArgs e )
		{
			ConsoleTool.ConsoleVisible = !ConsoleTool.ConsoleVisible;
			button_getModule.Enabled = ConsoleTool.ConsoleVisible;
        }

		private void button_addModule_Click( object sender, EventArgs e )
		{
			new MakeModule( ).ShowDialog( );
		}

		private void button_rmvModule_Click( object sender, EventArgs e )
		{

		}
	}
}
