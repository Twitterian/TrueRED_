namespace TrueRED.Display
{
	partial class AppConsole
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
			this.button_exit = new System.Windows.Forms.Button();
			this.checkedlistbox_modules = new System.Windows.Forms.CheckedListBox();
			this.button_getModule = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button_exit
			// 
			this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_exit.Location = new System.Drawing.Point(147, 204);
			this.button_exit.Name = "button_exit";
			this.button_exit.Size = new System.Drawing.Size(75, 23);
			this.button_exit.TabIndex = 0;
			this.button_exit.Text = "종료";
			this.button_exit.UseVisualStyleBackColor = true;
			this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
			// 
			// checkedlistbox_modules
			// 
			this.checkedlistbox_modules.FormattingEnabled = true;
			this.checkedlistbox_modules.Location = new System.Drawing.Point(13, 13);
			this.checkedlistbox_modules.Name = "checkedlistbox_modules";
			this.checkedlistbox_modules.Size = new System.Drawing.Size(120, 212);
			this.checkedlistbox_modules.TabIndex = 1;
			// 
			// button_betModule
			// 
			this.button_getModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_getModule.Location = new System.Drawing.Point(146, 175);
			this.button_getModule.Name = "button_getModule";
			this.button_getModule.Size = new System.Drawing.Size(75, 23);
			this.button_getModule.TabIndex = 2;
			this.button_getModule.Text = "GetModuleState";
			this.button_getModule.UseVisualStyleBackColor = true;
			this.button_getModule.Click += new System.EventHandler(this.button_getModule_Click);
			// 
			// AppConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(234, 239);
			this.Controls.Add(this.button_getModule);
			this.Controls.Add(this.checkedlistbox_modules);
			this.Controls.Add(this.button_exit);
			this.Name = "AppConsole";
			this.Text = "AppConsole";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button_exit;
		private System.Windows.Forms.CheckedListBox checkedlistbox_modules;
		private System.Windows.Forms.Button button_getModule;
	}
}