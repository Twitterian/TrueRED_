using System;
using System.Diagnostics;

namespace TrueRED.Framework
{
	public class Log
	{
		public static bool IsInited { get; private set; }
		public static bool LogTrace { get; private set; }

		public static void Init( bool LogTrace = false )
		{
			try
			{
				IsInited = true;
				Trace.Listeners.Add( new TextWriterTraceListener( Console.Out ) );
				Trace.AutoFlush = true;
			}
			catch
			{
				IsInited = false;
			}
		}

		public static void Print( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.White;
			Output( tag, message );
		}
		public static void Http( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Output( tag, message );
		}
		public static void Debug( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Output( tag, message );
		}
		public static void Warning( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Output( tag, message );
		}
		public static void Error( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Output( tag, message );
		}
		public static void CriticalError( string tag, string message )
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Output( tag, message );
			//TODO: 크리티컬 에러 메세지 (팝업 등으로)
		}

		private static void Output( string tag, string message )
		{
			string log = string.Format( "{0} : {1}", tag, message );
			if ( IsInited )
			{
				Trace.WriteLine( log );
			}
			else
			{
				Console.WriteLine( log );
			}
		}

		public static void StackTrace( )
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			if ( IsInited )
			{
				Trace.Write( new StackTrace( true ) );
			}
			else
			{
				Console.WriteLine( string.Format( "Inflate stacktrace() : \n{0}", new StackTrace( true ) ) );
			}
		}

		public static void Indent( )
		{
			if ( !IsInited ) return;
			Trace.Indent( );
		}

		public static void Unindent( )
		{
			if ( !IsInited ) return;
			Trace.Unindent( );
		}
	}

}
