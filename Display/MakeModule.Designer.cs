using MaterialSkin.Controls;

namespace TrueRED.Display
{
	partial class MakeModule
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
			this.tabControl = new MaterialSkin.Controls.MaterialTabControl();
			this.tabpage_modules = new System.Windows.Forms.TabPage();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// materialTabSelector1
			// 
			this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.materialTabSelector1.BaseTabControl = this.tabControl;
			this.materialTabSelector1.Depth = 0;
			this.materialTabSelector1.Location = new System.Drawing.Point(0, 59);
			this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialTabSelector1.Name = "materialTabSelector1";
			this.materialTabSelector1.Size = new System.Drawing.Size(592, 44);
			this.materialTabSelector1.TabIndex = 17;
			this.materialTabSelector1.Text = "materialTabSelector1";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabpage_modules);
			this.tabControl.Depth = 0;
			this.tabControl.Location = new System.Drawing.Point(0, 102);
			this.tabControl.MouseState = MaterialSkin.MouseState.HOVER;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(557, 315);
			this.tabControl.TabIndex = 18;
			// 
			// tabpage_modules
			// 
			this.tabpage_modules.BackColor = System.Drawing.Color.White;
			this.tabpage_modules.Location = new System.Drawing.Point(4, 22);
			this.tabpage_modules.Name = "tabpage_modules";
			this.tabpage_modules.Padding = new System.Windows.Forms.Padding(3);
			this.tabpage_modules.Size = new System.Drawing.Size(549, 289);
			this.tabpage_modules.TabIndex = 0;
			this.tabpage_modules.Text = "Module Type";
			this.tabpage_modules.UseVisualStyleBackColor = true;
			// 
			// MakeModule
			// 
			this.ClientSize = new System.Drawing.Size(560, 420);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.materialTabSelector1);
			this.Name = "MakeModule";
			this.Text = "Create new module";
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		
		private MaterialTabSelector materialTabSelector1;
		private MaterialTabControl tabControl;
		private System.Windows.Forms.TabPage tabpage_modules;
	}
}