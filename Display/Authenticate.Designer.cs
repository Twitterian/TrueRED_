namespace TrueRED.Display
{
	partial class Authenticate
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
			this.button_request = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textbox_consumertoken = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textbox_consumersecret = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textbox_usertoken = new System.Windows.Forms.TextBox();
			this.button_activate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button_request
			// 
			this.button_request.Enabled = false;
			this.button_request.Location = new System.Drawing.Point(14, 60);
			this.button_request.Name = "button_request";
			this.button_request.Size = new System.Drawing.Size(438, 23);
			this.button_request.TabIndex = 0;
			this.button_request.Text = "Request";
			this.button_request.UseVisualStyleBackColor = true;
			this.button_request.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Consumer Token";
			// 
			// textbox_consumertoken
			// 
			this.textbox_consumertoken.Location = new System.Drawing.Point(121, 6);
			this.textbox_consumertoken.Name = "textbox_consumertoken";
			this.textbox_consumertoken.Size = new System.Drawing.Size(331, 21);
			this.textbox_consumertoken.TabIndex = 2;
			this.textbox_consumertoken.TextChanged += new System.EventHandler(this.textbox_consumersecret_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "Consumer Secret";
			// 
			// textbox_consumersecret
			// 
			this.textbox_consumersecret.Location = new System.Drawing.Point(121, 33);
			this.textbox_consumersecret.Name = "textbox_consumersecret";
			this.textbox_consumersecret.Size = new System.Drawing.Size(331, 21);
			this.textbox_consumersecret.TabIndex = 2;
			this.textbox_consumersecret.TextChanged += new System.EventHandler(this.textbox_consumersecret_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(11, 92);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "User Token";
			// 
			// textbox_usertoken
			// 
			this.textbox_usertoken.Enabled = false;
			this.textbox_usertoken.Location = new System.Drawing.Point(121, 89);
			this.textbox_usertoken.Name = "textbox_usertoken";
			this.textbox_usertoken.Size = new System.Drawing.Size(331, 21);
			this.textbox_usertoken.TabIndex = 2;
			this.textbox_usertoken.TextChanged += new System.EventHandler(this.textbox_usertoken_TextChanged);
			// 
			// button_activate
			// 
			this.button_activate.Enabled = false;
			this.button_activate.Location = new System.Drawing.Point(14, 116);
			this.button_activate.Name = "button_activate";
			this.button_activate.Size = new System.Drawing.Size(438, 23);
			this.button_activate.TabIndex = 0;
			this.button_activate.Text = "Activate";
			this.button_activate.UseVisualStyleBackColor = true;
			this.button_activate.Click += new System.EventHandler(this.button_activate_Click);
			// 
			// Authenticate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(464, 144);
			this.Controls.Add(this.textbox_usertoken);
			this.Controls.Add(this.textbox_consumersecret);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textbox_consumertoken);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button_activate);
			this.Controls.Add(this.button_request);
			this.Name = "Authenticate";
			this.Text = "Authenticate";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button_request;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textbox_consumertoken;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textbox_consumersecret;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textbox_usertoken;
		private System.Windows.Forms.Button button_activate;
	}
}