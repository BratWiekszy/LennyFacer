using System;
using System.Windows.Forms;

namespace BW.Lennier
{
	public partial class HotkeyDisplayForm : Form
	{
		public HotkeyDisplayForm(ListViewItem[] items)
		{
			InitializeComponent(items);
			Visible = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			hotkeyList.Items.Clear();
			base.OnClosed(e);
		}
	}
}
