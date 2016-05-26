using System;
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

		public static void LoadStringsets( string rootpath, bool isRootCall = true )
		{
			if ( !Directory.Exists( rootpath ) ) return;
			StringSetsManager.RootPath = rootpath;
			var subdirs = Directory.GetDirectories(rootpath );
			var files = Directory.GetFiles(rootpath);
			foreach ( var item in subdirs )
			{
				LoadStringsets( item, false );
			}
			foreach ( var item in files )
			{
				if ( item.ToLower( ).EndsWith( ".stringset" ) ) LoadStringset( item );
			}

			if(isRootCall) ReferenceStringsets( );
		}

		public static void LoadStringsets( )
		{
			LoadStringsets( StringSetsManager.RootPath, true );
		}

		private static void LoadStringset( string path )
		{
			var lines = File.ReadAllLines(path, Encoding.UTF8).ToList();
			var name = lines[0];
			lines.RemoveAt( 0 );
			_list.Add( name, lines );

			Log.Debug( "LoadStringSet", "from [ {0} ], {1} : {2} string loaded.", path, name, lines.Count );
		}

		private static void ReferenceStringsets()
		{
			var keys = _list.Keys.ToList();
			for ( int i = 0; i < _list.Count; i++ )
			{
				_list[keys[i]] = ReferenceStrings( _list[keys[i]] );
			}
		}

		private static IList<string> ReferenceStrings(IList<string> strings)
		{
			var linebuilder = new List<string>();
			var lineremover = new List<string>();
			for ( int i = 0; i < strings.Count; i++ )
			{
				if ( strings[i].StartsWith( "__" ) && strings[i].EndsWith( "__" ) )
				{
					// include stringset
					var @ref = StringSetsManager.GetStrings(strings[i].Substring(2, strings[i].Length-4));

					var lines = ReferenceStrings( @ref );
					foreach ( var @string in lines )
					{
						linebuilder.Add( @string );
					}
					lineremover.Add( strings[i] );
				}
				else if ( strings[i].StartsWith( "-__" ) && strings[i].EndsWith( "__" ) )
				{
					// declude stringset

				}
				else
				{
					// normal string

				}
			}

			// detach strings
			foreach ( var item in lineremover )
			{
				strings.Remove( item );
			}

			// attach strings
			foreach ( var item in linebuilder )
			{
				strings.Add( item );
			}

			foreach ( var item in strings )
			{
				Console.WriteLine( item );
			}
			Console.WriteLine( "" );


			return strings;
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
