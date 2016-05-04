using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace TrueRED.Display
{
	public partial class ModuleFace : TabPage
	{
		public List<Control> InputFields { get; private set; } = new List<Control>( );

		public ModuleFace( Type type, IEnumerable<ModuleFaceCategory> moduleFaceInfo, Action<Type, object[]> doneCallback )
		{
			SuspendLayout( );
			InitializeComponent( type.Name );
			AttachModuleFace( moduleFaceInfo );
			AttachDoneButton( type, doneCallback );
			ResumeLayout( );
		}

		private void AttachDoneButton( Type type, Action<Type, object[]> donebuttonClickLiestner )
		{
			var button = new MaterialFlatButton();
			button.Text = "Done";
			button.Location = new Point( this.Size.Width - button.Size.Width - 10, this.Size.Height - button.Size.Height );
			button.Click += delegate
			{
				var @params = new object[InputFields.Count];
				for ( int i = 0; i < InputFields.Count; i++ )
				{
					var item = InputFields[i];
					if ( item is MaterialSingleLineTextField )
					{
						@params[i] = ( ( MaterialSingleLineTextField ) item ).Text;
					}
					else if ( item is NumericUpDown )
					{
						@params[i] = ( int ) ( ( ( NumericUpDown ) item ).Value );
					}
				}
				donebuttonClickLiestner( type, @params );
			};
			button.Anchor = ( AnchorStyles.Bottom | AnchorStyles.Right );
			this.Controls.Add( button );
		}

		private void InitializeComponent( string moduleName )
		{
			var controlname = string.Format( "{0} config", moduleName );
			this.BackColor = Color.White;
			this.Location = new Point( 0, 0 );
			this.Name = controlname;
			this.Text = controlname;
			this.Size = new Size( 550, 300 );
		}

		private void AttachModuleFace( IEnumerable<ModuleFaceCategory> moduleFaceInfo )
		{
			const int PADDING = 10;
			const int LABEL_X_POSITION = 20;
			const int LABEL_X_SIZE = 150;
			const int INPUT_X_POSITION = 170;
			const int INPUT_X_SIZE = 250;
			const int CONTROL_HEIGHT = 35;

			int Current_Y_Position = 0;

			foreach ( var category in moduleFaceInfo )
			{
				// TODO: 탭 인댄트 정리
				var title = new MaterialLabel( );
				title.Text = category.CategoryName;
				title.Location = new Point( LABEL_X_POSITION, Current_Y_Position * CONTROL_HEIGHT + PADDING );
				title.Size = new Size( LABEL_X_SIZE, CONTROL_HEIGHT );
				this.Controls.Add( title );
				Current_Y_Position++;

				foreach ( var item in category.CategoryItem )
				{
					var itemname = new MaterialLabel();
					itemname.Text = item.Item2;
					itemname.Size = new Size( LABEL_X_SIZE, CONTROL_HEIGHT );
					itemname.Location = new Point( LABEL_X_POSITION, Current_Y_Position * CONTROL_HEIGHT + PADDING );
					Control iteminput = null;
					switch ( item.Item1 )
					{
						case ModuleFaceCategory.ModuleFaceTypes.String:
							iteminput = new MaterialSingleLineTextField( );
							break;
						case ModuleFaceCategory.ModuleFaceTypes.Int:
							iteminput = new NumericUpDown( );
							( ( NumericUpDown ) iteminput ).Maximum = decimal.MaxValue;
							( ( NumericUpDown ) iteminput ).Minimum = decimal.MinValue;
							break;
					}
					iteminput.Location = new Point( INPUT_X_POSITION, Current_Y_Position * CONTROL_HEIGHT + PADDING );
					iteminput.Size = new Size( INPUT_X_SIZE, CONTROL_HEIGHT );
					Current_Y_Position++;
					this.Controls.Add( itemname );
					this.Controls.Add( iteminput );
					InputFields.Add( iteminput );
				}
			}
		}

	}

	public class ModuleFaceCategory
	{
		public string CategoryName { get; set; }
		public List<Tuple<ModuleFaceTypes, string>> CategoryItem { get; set; } = new List<Tuple<ModuleFaceTypes, string>>( );

		public ModuleFaceCategory( string categoryName )
		{
			this.CategoryName = categoryName;
		}

		public ModuleFaceCategory( string categoryName, List<Tuple<ModuleFaceTypes, string>> items ) : this( categoryName )
		{
			this.CategoryItem = items;
		}

		public void Add( ModuleFaceTypes type, string name )
		{
			CategoryItem.Add( new Tuple<ModuleFaceTypes, string>( type, name ) );
		}

		public enum ModuleFaceTypes { String, Int }
	}
}
