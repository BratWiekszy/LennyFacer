using System;
using System.Windows.Forms;

namespace BW.Lennier
{
	internal class AppHandleProvider : NativeWindow
	{
		public AppHandleProvider()
		{
			CreateHandle(new CreateParams());
		}

	}
}