using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BW.Lennier
{
	internal sealed partial class OptionsBaloonForm : Form
	{
		private const int ButtonHeight = 36;
		private const int ButtonWidth  = 182;

		internal OptionsBaloonForm(IEnumerable<Button> buttons, int count, Point position)
		{
			Location = position;
			InitializeComponent(buttons, count);
			Visible = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			Controls.Clear();
			base.OnClosed(e);
		}

		/*
		private void AddFaceButton_Click(object sender, EventArgs e)
		{

		}

		private void FacesButton_Click(object sender, EventArgs e)
		{

		}*/
	}
}
