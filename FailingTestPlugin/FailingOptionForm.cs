using System;
using System.Windows.Forms;

namespace FailingTestPlugin
{
	public partial class FailingOptionForm : Form
	{
		public FailingOptionForm()
		{
			InitializeComponent();
			Visible = true;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			throw new ArgumentNullException();
		}
	}
}
