using System;
using System.Diagnostics;

namespace TrueRED.Framework
{
	public static class Log
	{
		private static bool IsInited;

		public static void Init( )
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

		public static void Print( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.White;
			Output( tag, message, parameters );
		}
		public static void Http( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Output( tag, message, parameters );
		}
		public static void Debug( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Output( tag, message, parameters );
		}
		public static void Warning( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Output( tag, message, parameters );
		}
		public static void Error( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Output( tag, message, parameters );
		}
		public static void CriticalError( string tag, string message, params object[] parameters )
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Output( tag, message, parameters );
			//TODO: 크리티컬 에러 메세지 (팝업 등으로)
		}

		private static void Output( string tag, string message, params object[] parameters )
		{
			string log = string.Format( "{0} : {1}", tag, string.Format(message, parameters) );
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
                Console.WriteLine("Inflate stacktrace() :");
                Console.WriteLine(new StackTrace(true));
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
