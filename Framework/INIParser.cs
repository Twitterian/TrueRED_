using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace TrueRED.Framework
{
	class INIParser
	{
		private string iniPath;
		private FileIniDataParser parser;
		private IniData data;

		public INIParser( string path )
		{
			this.iniPath = path;
			parser = new FileIniDataParser( );
			if ( File.Exists( iniPath ) )
				data = new FileIniDataParser( ).ReadFile( iniPath );
			else
				data = new IniData( );
		}

		#region WINApi

		//[DllImport( "kernel32.dll" )]
		//private static extern int GetPrivateProfileString(
		//	String section,
		//	String key,
		//	String def,
		//	StringBuilder retVal,
		//	int size,
		//	String filePath );

		//[DllImport( "kernel32.dll" )]
		//private static extern long WritePrivateProfileString(
		//	String section,
		//	String key,
		//	String val,
		//	String filePath );

		//public String GetValue( String Section, String Key )
		//{
		//	StringBuilder temp = new StringBuilder(255);
		//	int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
		//	return temp.ToString( );
		//}

		//public void SetValue( String Section, String Key, String Value )
		//{
		//	WritePrivateProfileString( Section, Key, Value, iniPath );
		//}

		#endregion

		public string GetValue( string Section, string Key )
		{
			try
			{
				return data[Section][Key];
			}
			catch
			{
				return null;
			}
		}

		public void SetValue( string Section, string Key, object Value )
		{
			if ( !data.Sections.ContainsSection( Section ) ) data.Sections.AddSection( Section );
			if ( !data[Section].ContainsKey( Key ) ) data[Section].AddKey( Key );

			data[Section][Key] = Value.ToString( );
		}

		internal void Save( )
		{
			parser.WriteFile( iniPath, data, Encoding.UTF8 );
		}

		#region FileIniDataParser



		#endregion
	}
}