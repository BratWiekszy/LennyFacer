using System;
using System.Threading;
using System.Windows.Forms;

namespace BW.Lennier
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (Mutex me = new Mutex(true, "LennyFacer", out bool single))
			{
				if (single)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
					var context = new LennyApplicationContext();

					Application.ThreadException += (s, args) => context.HandleException(args);

					Application.Run(context);
				}
			}
		}
	}
}
