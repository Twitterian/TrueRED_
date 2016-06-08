using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueRED.Framework
{
	class ModuleLibrary
	{
		public static Dictionary<string, object> Library { get; private set; } = new Dictionary<string, object>( );

		public static bool SetValue( string Address, object Value )
		{
			if ( Library.ContainsKey( Address ) )
			{
				Library[Address] = Value;
			}
			else
			{
				Library.Add( Address, Value );
			}
			return true;
		}

		public static object GetValue( string Address )
		{
			if ( Library.ContainsKey( Address ) )
			{
				return Library[Address];
			}
			else return null;
		}

		internal static void RemoveValue( string Address )
		{
			if ( Library.ContainsKey( Address ) )
			{
				Library.Remove( Address );
			}
		}
	}
}
