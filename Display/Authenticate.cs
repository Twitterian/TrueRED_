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
using Tweetinvi;
using Tweetinvi.Core.Authentication;

namespace TrueRED.Display
{
	public partial class Authenticate : Form
	{
		private IAuthenticationContext authenticationContext;
		public bool Result { get; private set; } = false;

		public Authenticate( )
		{
			InitializeComponent( );
		}

		private void button1_Click( object sender, EventArgs e )
		{
			textbox_consumertoken.Enabled = false;
			textbox_consumersecret.Enabled = false;

			var applicationCredentials = new ConsumerCredentials(textbox_consumertoken.Text, textbox_consumersecret.Text);
			authenticationContext = AuthFlow.InitAuthentication(applicationCredentials);

			System.Diagnostics.Process.Start( authenticationContext.AuthorizationURL );

			button_request.Enabled = false;
			textbox_usertoken.Enabled = true;
		}

		private void textbox_consumersecret_TextChanged( object sender, EventArgs e )
		{
			if(textbox_consumersecret.Text.Length > 0 && textbox_consumertoken.Text.Length > 0)
			{
				button_request.Enabled = true;
			}
			else
			{
				button_request.Enabled = false;
			}
		}
		

		private void textbox_usertoken_TextChanged( object sender, EventArgs e )
		{
			if(textbox_usertoken.Text.Length > 0)
			{
				button_activate.Enabled = true;
			}
			else
			{
				button_activate.Enabled = false;
			}
		}

		private void button_activate_Click( object sender, EventArgs e )
		{
			button_activate.Enabled = false;
			var newCredentials = AuthFlow.CreateCredentialsFromVerifierCode(textbox_usertoken.Text, authenticationContext);
			Log.Http( "Access Token = {0}", newCredentials.AccessToken );
			Log.Http( "Access Token Secret = {0}", newCredentials.AccessTokenSecret );

			string consumerKey = textbox_consumertoken.Text;
			string consumerSecret = textbox_consumersecret.Text;
			string accessToken = newCredentials.AccessToken;
			string accessSecret = newCredentials.AccessTokenSecret;

			var parser = new INIParser("Globals.ini");
			parser.SetValue( "Authenticate", "ConsumerKey", consumerKey );
			parser.SetValue( "Authenticate", "CconsumerSecret", consumerSecret );
			parser.SetValue( "Authenticate", "AccessToken", accessToken );
			parser.SetValue( "Authenticate", "AccessSecret", accessSecret );
			parser.Save( );

			Globals.Instance.Initialize( consumerKey, consumerSecret, accessToken, accessSecret );

			Result = true;

            this.Close( );
		}
	}
}
