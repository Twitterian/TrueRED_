using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;

namespace TrueRED.Display.ModuleFaces
{
	public partial class EditWeather : TabPage
	{
		public EditWeather( )
		{
			InitializeComponent( );

		}

		private void InitializeComponent( )
		{
			this.BackColor = System.Drawing.Color.White;
			this.Location = new System.Drawing.Point( 4, 22 );
			this.Name = "tabPage2";
			this.Padding = new System.Windows.Forms.Padding( 3 );
			this.Size = new System.Drawing.Size( 678, 297 );
			this.TabIndex = 1;
			this.Text = "Weather Config";
		}
	}
}
