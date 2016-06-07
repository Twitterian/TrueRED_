using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;

namespace TrueRED.Framework
{
	public static class Log
	{
		private static bool IsInited;

		public static bool DMLogFlag { get; set; }

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
			if ( DMLogFlag )
			{
				// TODO: 보면 뭐가 문젠지 알겁니다
				Task.Factory.StartNew( delegate
				{
					Globals.Instance.User.PublishMessage( new PublishMessageParameters( log, Globals.Instance.OwnerID ) );
				} );
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
				Console.WriteLine( "Inflate stacktrace() :" );
				Console.WriteLine( new StackTrace( true ) );
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
