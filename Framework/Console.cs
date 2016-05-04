using System;
using System.Runtime.InteropServices;

namespace TrueRED.Framework
{
	public class ConsoleTool
	{
		[DllImport( "kernel32.dll" )]
		private static extern IntPtr GetConsoleWindow( );

		[DllImport( "user32.dll" )]
		private static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;

		private static bool _ConsoleVisible = true;
		public static bool ConsoleVisible
		{
			get
			{
				return _ConsoleVisible;
			}
			set
			{
				_ConsoleVisible = value;
				var handle = GetConsoleWindow();
				ShowWindow( handle, ConsoleVisible ? SW_SHOW : SW_HIDE );
			}
		}
	}
}
