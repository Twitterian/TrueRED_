using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrueRED.Framework;
using TrueRED.Modules;

namespace TrueRED.Display
{
	public partial class AppConsole : Form
	{
		List<ModuleObject> Modules = new List<ModuleObject>();
		public DataSet ds { get; private set; }

		public AppConsole( Dictionary<string, Module> modules )
		{
			InitializeComponent( );
			var keys = new List<string>( modules.Keys );
			for ( int i = 0; i < modules.Count; i++ )
			{
				Modules.Add( new ModuleObject( i, keys[i], modules[keys[i]] ) );
			}

			// 바인딩 안 됨.
			( ( ListBox ) checkedlistbox_modules ).DataSource = Modules;
			( ( ListBox ) checkedlistbox_modules ).DisplayMember = "Name";
			( ( ListBox ) checkedlistbox_modules ).ValueMember = "IsRunning";

			for ( int i = 0; i < checkedlistbox_modules.Items.Count; i++ )
			{
				ModuleObject obj = (ModuleObject)checkedlistbox_modules.Items[i];
				checkedlistbox_modules.SetItemChecked( i, obj.Module.IsRunning );
				Modules[i].Module.ModuleStateChangeListener.Add( i, delegate ( int index, bool running )
				{
					this.Invoke( new MethodInvoker( delegate
					  {
						  this.checkedlistbox_modules.ItemCheck -= this.checkedlistbox_modules_ItemCheck;
						  checkedlistbox_modules.SetItemChecked( index, Modules[index].Module.IsRunning );
						  this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
					  } ) );
				} );

			}
			this.checkedlistbox_modules.ItemCheck += this.checkedlistbox_modules_ItemCheck;
		}

		private void button_exit_Click( object sender, EventArgs e )
		{
			this.Close( );
		}

		private void button_getModule_Click( object sender, EventArgs e )
		{
			if ( Modules == null ) Log.Error( "Controller", "Modules undefined" );
			string result = string.Empty;
			foreach ( var item in Modules )
			{
				result += string.Format( "\t{0} : {1}\n", item.Name, item.Module.IsRunning.ToString( ) );
			}
			Log.Debug( "AppConsole", string.Format( "\n{0}", result ) );
		}

		private void checkedlistbox_modules_ItemCheck( object sender, ItemCheckEventArgs e )
		{
			var i = checkedlistbox_modules.SelectedIndex;
			Modules[i].IsRunning = !Modules[i].IsRunning;

			Log.Debug( "AppConsole", string.Format( "{0} 모듈이 {1}활성화 했어", Modules[i].Name, ( Modules[i].IsRunning ? "" : "비" ) ) );
		}
	}

	public class ModuleObject
	{
		public Module Module { get; set; }
		public string Name { get; set; }
		public int Index
		{
			get; set;
		}
		public bool IsRunning
		{
			get
			{
				return Module.IsRunning;
			}
			set
			{
				Module.IsRunning = value;
			}
		}

		public ModuleObject( int index, string name, Module module )
		{
			this.Index = index;
			this.Name = name;
			this.Module = module;
		}
	}
}
