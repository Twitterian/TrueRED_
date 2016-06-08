using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrueRED.Framework
{
	class ModuleInvoker
	{
		public static void SetValue( string ModuleName, string PropertyName, string Value )
		{
			var module = ModuleManager.Get( ModuleName );
			var member = module.GetType( ).GetProperty( PropertyName );
			member.SetValue( module, Value );
		}

		public static MethodInfo GetMethod( string ModuleName, string MethodName )
		{
			var method = ModuleManager.Get(ModuleName).GetType().GetMethod(MethodName);
			return method;
		}
	}
}
