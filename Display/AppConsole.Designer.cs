using MaterialSkin.Controls;
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
			this.button_exit = new MaterialSkin.Controls.MaterialFlatButton();
			this.checkedlistbox_modules = new System.Windows.Forms.CheckedListBox();
			this.button_getModule = new MaterialSkin.Controls.MaterialFlatButton();
			this.button_console = new MaterialSkin.Controls.MaterialFlatButton();
			this.button_addModule = new MaterialSkin.Controls.MaterialFlatButton();
			this.label_module = new MaterialSkin.Controls.MaterialLabel();
			this.button_rmvModule = new MaterialSkin.Controls.MaterialFlatButton();
			this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
			this.button_stringset_reload = new MaterialSkin.Controls.MaterialFlatButton();
			this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
			this.button_module_reload = new MaterialSkin.Controls.MaterialFlatButton();
			this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
			this.button_changetwitterID = new MaterialSkin.Controls.MaterialFlatButton();
			this.SuspendLayout();
			// 
			// button_exit
			// 
			this.button_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_exit.AutoSize = true;
			this.button_exit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_exit.Depth = 0;
			this.button_exit.Location = new System.Drawing.Point(293, 578);
			this.button_exit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_exit.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_exit.Name = "button_exit";
			this.button_exit.Primary = false;
			this.button_exit.Size = new System.Drawing.Size(41, 36);
			this.button_exit.TabIndex = 0;
			this.button_exit.Text = "Exit";
			this.button_exit.UseVisualStyleBackColor = true;
			this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
			// 
			// checkedlistbox_modules
			// 
			this.checkedlistbox_modules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkedlistbox_modules.FormattingEnabled = true;
			this.checkedlistbox_modules.Location = new System.Drawing.Point(13, 77);
			this.checkedlistbox_modules.Name = "checkedlistbox_modules";
			this.checkedlistbox_modules.Size = new System.Drawing.Size(172, 532);
			this.checkedlistbox_modules.TabIndex = 1;
			// 
			// button_getModule
			// 
			this.button_getModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_getModule.AutoSize = true;
			this.button_getModule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_getModule.Depth = 0;
			this.button_getModule.Location = new System.Drawing.Point(280, 541);
			this.button_getModule.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_getModule.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_getModule.Name = "button_getModule";
			this.button_getModule.Primary = false;
			this.button_getModule.Size = new System.Drawing.Size(54, 36);
			this.button_getModule.TabIndex = 2;
			this.button_getModule.Text = "STATE";
			this.button_getModule.UseVisualStyleBackColor = true;
			this.button_getModule.Click += new System.EventHandler(this.button_getModule_Click);
			// 
			// button_console
			// 
			this.button_console.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_console.AutoSize = true;
			this.button_console.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_console.Depth = 0;
			this.button_console.Location = new System.Drawing.Point(261, 504);
			this.button_console.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_console.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_console.Name = "button_console";
			this.button_console.Primary = false;
			this.button_console.Size = new System.Drawing.Size(73, 36);
			this.button_console.TabIndex = 2;
			this.button_console.Text = "Console";
			this.button_console.UseVisualStyleBackColor = true;
			this.button_console.Click += new System.EventHandler(this.button_console_Click);
			// 
			// button_addModule
			// 
			this.button_addModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_addModule.AutoSize = true;
			this.button_addModule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_addModule.Depth = 0;
			this.button_addModule.Location = new System.Drawing.Point(295, 200);
			this.button_addModule.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_addModule.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_addModule.Name = "button_addModule";
			this.button_addModule.Primary = false;
			this.button_addModule.Size = new System.Drawing.Size(39, 36);
			this.button_addModule.TabIndex = 2;
			this.button_addModule.Text = "Add";
			this.button_addModule.UseVisualStyleBackColor = true;
			this.button_addModule.Click += new System.EventHandler(this.button_addModule_Click);
			// 
			// label_module
			// 
			this.label_module.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label_module.AutoSize = true;
			this.label_module.Depth = 0;
			this.label_module.Font = new System.Drawing.Font("Roboto", 11F);
			this.label_module.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.label_module.Location = new System.Drawing.Point(276, 175);
			this.label_module.MouseState = MaterialSkin.MouseState.HOVER;
			this.label_module.Name = "label_module";
			this.label_module.Size = new System.Drawing.Size(59, 19);
			this.label_module.TabIndex = 0;
			this.label_module.Text = "Module";
			// 
			// button_rmvModule
			// 
			this.button_rmvModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_rmvModule.AutoSize = true;
			this.button_rmvModule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_rmvModule.Depth = 0;
			this.button_rmvModule.Location = new System.Drawing.Point(268, 237);
			this.button_rmvModule.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_rmvModule.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_rmvModule.Name = "button_rmvModule";
			this.button_rmvModule.Primary = false;
			this.button_rmvModule.Size = new System.Drawing.Size(66, 36);
			this.button_rmvModule.TabIndex = 2;
			this.button_rmvModule.Text = "Remove";
			this.button_rmvModule.UseVisualStyleBackColor = true;
			this.button_rmvModule.Click += new System.EventHandler(this.button_rmvModule_Click);
			// 
			// materialFlatButton1
			// 
			this.materialFlatButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.materialFlatButton1.AutoSize = true;
			this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.materialFlatButton1.Depth = 0;
			this.materialFlatButton1.Location = new System.Drawing.Point(295, 375);
			this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialFlatButton1.Name = "materialFlatButton1";
			this.materialFlatButton1.Primary = false;
			this.materialFlatButton1.Size = new System.Drawing.Size(39, 36);
			this.materialFlatButton1.TabIndex = 2;
			this.materialFlatButton1.Text = "Add";
			this.materialFlatButton1.UseVisualStyleBackColor = true;
			this.materialFlatButton1.Click += new System.EventHandler(this.button_addModule_Click);
			// 
			// button_stringset_reload
			// 
			this.button_stringset_reload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_stringset_reload.AutoSize = true;
			this.button_stringset_reload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_stringset_reload.Depth = 0;
			this.button_stringset_reload.Location = new System.Drawing.Point(268, 412);
			this.button_stringset_reload.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_stringset_reload.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_stringset_reload.Name = "button_stringset_reload";
			this.button_stringset_reload.Primary = false;
			this.button_stringset_reload.Size = new System.Drawing.Size(63, 36);
			this.button_stringset_reload.TabIndex = 2;
			this.button_stringset_reload.Text = "Reload";
			this.button_stringset_reload.UseVisualStyleBackColor = true;
			this.button_stringset_reload.Click += new System.EventHandler(this.button_stringset_reload_click);
			// 
			// materialLabel1
			// 
			this.materialLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.materialLabel1.AutoSize = true;
			this.materialLabel1.Depth = 0;
			this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
			this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.materialLabel1.Location = new System.Drawing.Point(266, 350);
			this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialLabel1.Name = "materialLabel1";
			this.materialLabel1.Size = new System.Drawing.Size(69, 19);
			this.materialLabel1.TabIndex = 0;
			this.materialLabel1.Text = "Stringset";
			// 
			// button_module_reload
			// 
			this.button_module_reload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_module_reload.AutoSize = true;
			this.button_module_reload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_module_reload.Depth = 0;
			this.button_module_reload.Location = new System.Drawing.Point(270, 274);
			this.button_module_reload.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_module_reload.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_module_reload.Name = "button_module_reload";
			this.button_module_reload.Primary = false;
			this.button_module_reload.Size = new System.Drawing.Size(63, 36);
			this.button_module_reload.TabIndex = 2;
			this.button_module_reload.Text = "Reload";
			this.button_module_reload.UseVisualStyleBackColor = true;
			this.button_module_reload.Click += new System.EventHandler(this.button_module_reload_Click);
			// 
			// materialLabel2
			// 
			this.materialLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.materialLabel2.AutoSize = true;
			this.materialLabel2.Depth = 0;
			this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
			this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.materialLabel2.Location = new System.Drawing.Point(259, 77);
			this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
			this.materialLabel2.Name = "materialLabel2";
			this.materialLabel2.Size = new System.Drawing.Size(76, 19);
			this.materialLabel2.TabIndex = 0;
			this.materialLabel2.Text = "Current ID";
			// 
			// button_changetwitterID
			// 
			this.button_changetwitterID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_changetwitterID.AutoSize = true;
			this.button_changetwitterID.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button_changetwitterID.Depth = 0;
			this.button_changetwitterID.Location = new System.Drawing.Point(267, 102);
			this.button_changetwitterID.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.button_changetwitterID.MouseState = MaterialSkin.MouseState.HOVER;
			this.button_changetwitterID.Name = "button_changetwitterID";
			this.button_changetwitterID.Primary = false;
			this.button_changetwitterID.Size = new System.Drawing.Size(67, 36);
			this.button_changetwitterID.TabIndex = 2;
			this.button_changetwitterID.Text = "Change";
			this.button_changetwitterID.UseVisualStyleBackColor = true;
			this.button_changetwitterID.Click += new System.EventHandler(this.button_changetwitterID_Click);
			// 
			// AppConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(347, 618);
			this.Controls.Add(this.materialLabel1);
			this.Controls.Add(this.button_stringset_reload);
			this.Controls.Add(this.materialLabel2);
			this.Controls.Add(this.label_module);
			this.Controls.Add(this.materialFlatButton1);
			this.Controls.Add(this.button_module_reload);
			this.Controls.Add(this.button_rmvModule);
			this.Controls.Add(this.button_changetwitterID);
			this.Controls.Add(this.button_addModule);
			this.Controls.Add(this.button_console);
			this.Controls.Add(this.button_getModule);
			this.Controls.Add(this.checkedlistbox_modules);
			this.Controls.Add(this.button_exit);
			this.Name = "AppConsole";
			this.Text = "Bot management";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.CheckedListBox checkedlistbox_modules;
		private MaterialFlatButton button_exit;
		private MaterialFlatButton button_getModule;
		private MaterialFlatButton button_console;
		private MaterialFlatButton button_addModule;
		private MaterialLabel label_module;
		private MaterialFlatButton button_rmvModule;
		private MaterialFlatButton materialFlatButton1;
		private MaterialFlatButton button_stringset_reload;
		private MaterialLabel materialLabel1;
		private MaterialFlatButton button_module_reload;
		private MaterialLabel materialLabel2;
		private MaterialFlatButton button_changetwitterID;
	}
}