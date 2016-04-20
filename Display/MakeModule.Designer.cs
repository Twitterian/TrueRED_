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
			this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
			this.tabpage_modules = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.materialTabControl1.SuspendLayout();
			this.tabpage_modules.SuspendLayout();
			this.SuspendLayout();
			// 
			// materialTabSelector1
			// 
			this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
			this.materialTabSelector1.Depth = 0;
			this.materialTabSelector1.Location = new System.Drawing.Point(0, 59);
			this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialTabSelector1.Name = "materialTabSelector1";
			this.materialTabSelector1.Size = new System.Drawing.Size(721, 44);
			this.materialTabSelector1.TabIndex = 17;
			this.materialTabSelector1.Text = "materialTabSelector1";
			// 
			// materialTabControl1
			// 
			this.materialTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.materialTabControl1.Controls.Add(this.tabpage_modules);
			this.materialTabControl1.Controls.Add(this.tabPage2);
			this.materialTabControl1.Depth = 0;
			this.materialTabControl1.Location = new System.Drawing.Point(0, 102);
			this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialTabControl1.Name = "materialTabControl1";
			this.materialTabControl1.SelectedIndex = 0;
			this.materialTabControl1.Size = new System.Drawing.Size(686, 323);
			this.materialTabControl1.TabIndex = 18;
			// 
			// tabpage_modules
			// 
			this.tabpage_modules.BackColor = System.Drawing.Color.White;
			this.tabpage_modules.Location = new System.Drawing.Point(4, 22);
			this.tabpage_modules.Name = "tabpage_modules";
			this.tabpage_modules.Padding = new System.Windows.Forms.Padding(3);
			this.tabpage_modules.Size = new System.Drawing.Size(678, 297);
			this.tabpage_modules.TabIndex = 0;
			this.tabpage_modules.Text = "Module Type";
			tabpage_modules.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(678, 297);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Module Config";
			// 
			// MakeModule
			// 
			this.ClientSize = new System.Drawing.Size(689, 428);
			this.Controls.Add(this.materialTabControl1);
			this.Controls.Add(this.materialTabSelector1);
			this.Name = "MakeModule";
			this.Text = "Create new module";
			this.materialTabControl1.ResumeLayout(false);
			this.tabpage_modules.ResumeLayout(false);
			this.tabpage_modules.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		
		private MaterialTabSelector materialTabSelector1;
		private MaterialTabControl materialTabControl1;
		private System.Windows.Forms.TabPage tabpage_modules;
		private System.Windows.Forms.TabPage tabPage2;
	}
}