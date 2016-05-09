using System;
using System.Data;
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

			for ( int i = 0; i < ModuleManager.Modules.Count; i++ )
			{
				checkedlistbox_modules.Items.Add( ModuleManager.Modules[i].Name, ModuleManager.Modules[i].IsRunning );
				var index = i;
				ModuleManager.Modules[i].ModuleStateChangeListener.Add( delegate ( bool running )
				{
					this.Invoke( new MethodInvoker( delegate
					{
						this.checkedlistbox_modules.ItemCheck -= this.checkedlistbox_modules_ItemCheck;
						checkedlistbox_modules.SetItemChecked( index, ModuleManager.Modules[index].IsRunning );
						this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
					} ) );
				} );
				checkedlistbox_modules.SetItemChecked( i, ModuleManager.Modules[i].IsRunning );
			}
			this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;

			ModuleManager.Modules.OnModuleAttachLiestner.Add( delegate ( Module module )
			{
				var index = checkedlistbox_modules.Items.Count;
				checkedlistbox_modules.Items.Add( module.Name, module.IsRunning );
				module.ModuleStateChangeListener.Add( delegate ( bool running )
				{
					this.Invoke( new MethodInvoker( delegate
					{
						this.checkedlistbox_modules.ItemCheck -= this.checkedlistbox_modules_ItemCheck;
						checkedlistbox_modules.SetItemChecked( index, ModuleManager.Modules[index].IsRunning );
						this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
					} ) );
				} );
				checkedlistbox_modules.SetItemChecked( index, module.IsRunning );
			} );
			ModuleManager.Modules.OnModuleDetachLiestner.Add( delegate ( Module module )
			{
				for ( int i = 0; i < checkedlistbox_modules.Items.Count; i++ )
				{
					var item = checkedlistbox_modules.Items[i];
					if ( item.ToString( ) == module.Name ) checkedlistbox_modules.Items.Remove( item );
				}
			} );
		}

		private void button_exit_Click( object sender, EventArgs e )
		{
			this.Close( );
		}

		private void button_getModule_Click( object sender, EventArgs e )
		{
			string result = string.Empty;
			foreach ( var item in ModuleManager.Modules )
			{
				result += string.Format( "\n    {0} : {1}", item.Name, item.IsRunning.ToString( ) );
			}
			Log.Debug( "AppConsole", string.Format( "{0}", result ) );
		}

		private void checkedlistbox_modules_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			var i = checkedlistbox_modules.SelectedIndex;
			if ( i < 0 ) return;
			ModuleManager.Modules[i].IsRunning = !ModuleManager.Modules[i].IsRunning;

			Log.Debug( "AppConsole", string.Format( "{0} 모듈이 AppConsole에 의하여 {1}활성화", ModuleManager.Modules[i].Name, ( ModuleManager.Modules[i].IsRunning ? "" : "비" ) ) );
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
			if ( MessageBox.Show( "정말 삭제하시겠어요?\n예를 누르면 모듈이 완전히 삭제됩니다.", "경고", MessageBoxButtons.YesNo ) == DialogResult.Yes )
			{
				var item = checkedlistbox_modules.SelectedItem.ToString( );
				ModuleManager.UnloadModule( item );
				ModuleManager.RemoveModule( item );
			}
		}

		private void button_module_reload_Click( object sender, EventArgs e )
		{
			this.checkedlistbox_modules.Items.Clear( );
			ModuleManager.UnloadAllModule( );
			ModuleManager.LoadAllModules( );
		}

		private void button_stringset_reload_click( object sender, EventArgs e )
		{
			StringSetsManager.ClearStringSets( );
			StringSetsManager.LoadStringSets( );
		}

		private void button_changetwitterID_Click( object sender, EventArgs e )
		{
			new Authenticate( ).ShowDialog( );
		}
	}
}
