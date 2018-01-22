using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BW.Lennier
{
	sealed partial class OptionsBaloonForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent(IEnumerable<Button> buttons, int count)
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsBaloonForm));
			this.SuspendLayout();
			// 
			// OptionsBaloonForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(ButtonWidth, ButtonHeight * count);
			foreach(var button in buttons.Select((b, i) =>
				{
					b.Location = new System.Drawing.Point(0, ButtonHeight * i);
					b.Size = new System.Drawing.Size(ButtonWidth, ButtonHeight);
					b.Dock = DockStyle.Bottom;
					b.TabIndex = i;
					b.UseVisualStyleBackColor = true;
					this.Controls.Add(b);
					return 0;
				})
			)
			{ }

			//this.Controls.Add(this.AddFaceButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsBaloonForm";
			this.Opacity = 0.95D;
			this.Text = "Lenny actions";
			this.TopMost = true;
			this.StartPosition = FormStartPosition.Manual;
			this.ResumeLayout(false);
		}

		#endregion

		//private System.Windows.Forms.Button AddFaceButton;
		//private System.Windows.Forms.Button FacesButton;
	}
}