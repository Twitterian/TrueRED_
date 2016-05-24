﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TrueRED.Framework
{
	public static class StringSetsManager
	{
		private static Dictionary<string, IList<string>> _list = new Dictionary<string, IList<string>>();
		private static Random _selector = new Random();
		public static string RootPath { get; private set; }

		public static void LoadStringSets( string rootpath )
		{
			if ( !Directory.Exists( rootpath ) ) return;
			StringSetsManager.RootPath = rootpath;
			var subdirs = Directory.GetDirectories(rootpath );
			var files = Directory.GetFiles(rootpath);
			foreach ( var item in subdirs )
			{
				LoadStringSets( item );
			}
			foreach ( var item in files )
			{
				if ( item.ToLower( ).EndsWith( ".stringset" ) ) LoadStringSet( item );
			}
		}

		public static void LoadStringSets( )
		{
			LoadStringSets( StringSetsManager.RootPath );
		}

		private static void LoadStringSet( string path )
		{
			var lines = File.ReadAllLines(path, Encoding.UTF8).ToList();
			var name = lines[0];
			lines.RemoveAt( 0 );
			_list.Add( name, lines );

			Log.Debug( "LoadStringSet", "from [ {0} ], {1} : {2} string loaded.", path, name, lines.Count );
		}

		public static string GetRandomString( string StringSet )
		{
			if ( _list.ContainsKey( StringSet ) )
			{
				return _list[StringSet][_selector.Next( _list[StringSet].Count )];
			}
			else
			{
				Log.Error( "GetRandomString", "Coudn't find StringSet [{0}]", StringSet );
				return string.Empty;
			}

		}

		public static string[] GetStrings( string StringSet )
		{
			try
			{
				return _list[StringSet].ToArray( );
			}
			catch
			{
				Log.Error( "GetStrings", "Coudn't find StringSet [{0}]", StringSet );
				return new string[0];
			}
		}

		internal static void ClearStringSets( )
		{
			_list.Clear( );
		}
	}
}
