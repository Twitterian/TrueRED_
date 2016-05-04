using System;
using System.Collections.Generic;
using System.IO;
using TrueRED.Modules;

namespace TrueRED.Framework
{
	class ModuleManager
	{
		public static ModuleList<Module> Modules { get; private set; } = new ModuleList<Module>( ); // 어플리케이션에 로딩된 모듈 목록

		public static string ModulePath;

		public static bool LoadAllModules( string modulePath )
		{
			if ( !Directory.Exists( modulePath ) ) return false;
			ModuleManager.ModulePath = modulePath;
			foreach ( var item in Modules )
			{
				item.Release( );
			}
			Modules.Clear( );

			var module_settings = Directory.GetFiles( ModulePath );
			foreach ( var item in module_settings )
			{
				if ( item.ToLower( ).EndsWith( ".ini" ) )
				{
					var module = LoadModule( item );
					if ( module != null ) Modules.Add( module );
				}
			}
			return true;
		}
		public static bool LoadAllModules( )
		{
			return LoadAllModules( ModulePath );
		}

		private static Module LoadModule( string path )
		{
			var parser = new INIParser(path);
			var module = Module.Create( parser );
			return module;
		}
	}

	public class ModuleList<T> : List<T>
	{
		public List<Action<T>> OnModuleAttachLiestner { get; private set; } = new List<Action<T>>( );
		public List<Action<T>> OnModuleDetachLiestner { get; private set; } = new List<Action<T>>( );

		public new void Add( T item )
		{
			foreach ( var liestner in OnModuleAttachLiestner )
			{
				liestner( item );
			}
			base.Add( item );
		}

		public new void Remove( T item )
		{
			foreach ( var liestner in OnModuleDetachLiestner )
			{
				liestner( item );
			}
			base.Remove( item );
		}

	}
}
