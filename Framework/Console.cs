using System;
using System.Runtime.InteropServices;

namespace TrueRED.Framework
{
	public class ConsoleTool
	{
        private static class NativeMethods
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        }

		private const int SW_HIDE = 0;
		private const int SW_SHOW = 5;

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
				var handle = NativeMethods.GetConsoleWindow();
				NativeMethods.ShowWindow( handle, ConsoleVisible ? SW_SHOW : SW_HIDE );
			}
		}
	}
}
