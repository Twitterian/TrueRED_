using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TrueRED.Framework
{
	class INIParser
	{
		private string iniPath;

		public INIParser( string path )
		{
			this.iniPath = path;
		}

		[DllImport( "kernel32.dll" )]
		private static extern int GetPrivateProfileString(
			String section,
			String key,
			String def,
			StringBuilder retVal,
			int size,
			String filePath );

		[DllImport( "kernel32.dll" )]
		private static extern long WritePrivateProfileString(
			String section,
			String key,
			String val,
			String filePath );

		public String GetValue( String Section, String Key )
		{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
			return temp.ToString( );
		}

		public void SetValue( String Section, String Key, String Value )
		{
			WritePrivateProfileString( Section, Key, Value, iniPath );
		}
	}
}