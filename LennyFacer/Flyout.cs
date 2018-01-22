using System;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace BW.Lennier
{
	public sealed class Flyout : Form
	{
		private Button         _add;
		private TextBox        _faceInput;
		private Action<string> _callback;

		public Flyout([NotNull] Action<string> inputCallback)
		{
			FormBorderStyle = FormBorderStyle.FixedDialog;
			StartPosition = FormStartPosition.Manual;
			InitializeComponent();
			Rectangle r = Screen.PrimaryScreen.Bounds;
			Location    = new Point(r.Width - 190, r.Height - 140);
			Visible     = true;
			_callback   = inputCallback ?? throw new ArgumentNullException();
			LostFocus  += (sender, args) => { Close(); };
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Flyout));
			this._faceInput = new System.Windows.Forms.TextBox();
			this._add = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _faceInput
			// 
			this._faceInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._faceInput.Dock = System.Windows.Forms.DockStyle.Top;
			this._faceInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
			this._faceInput.Location = new System.Drawing.Point(0, 0);
			this._faceInput.Margin = new System.Windows.Forms.Padding(5, 5, 5, 4);
			this._faceInput.MaxLength = 20;
			this._faceInput.Name = "_faceInput";
			this._faceInput.Size = new System.Drawing.Size(180, 35);
			this._faceInput.TabIndex = 0;
			this._faceInput.WordWrap = false;
			// 
			// _add
			// 
			this._add.Dock = System.Windows.Forms.DockStyle.Top;
			this._add.Location = new System.Drawing.Point(0, 35);
			this._add.Name = "_add";
			this._add.Size = new System.Drawing.Size(180, 35);
			this._add.TabIndex = 1;
			this._add.Text = "Add";
			this._add.UseVisualStyleBackColor = true;
			this._add.Click += new System.EventHandler(this.OnAdded);
			// 
			// Flyout
			// 
			this.ClientSize = new System.Drawing.Size(180, 70);
			this.Controls.Add(this._add);
			this.Controls.Add(this._faceInput);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Flyout";
			this.Opacity = 0.9D;
			this.Text = "Add face";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void OnAdded(object sender, EventArgs e)
		{
			_callback.Invoke(_faceInput.Text);
			_callback = null;
			Close();
		}
	}
}
