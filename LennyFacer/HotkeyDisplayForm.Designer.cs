namespace BW.Lennier
{
	partial class HotkeyDisplayForm
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
		private void InitializeComponent(System.Windows.Forms.ListViewItem[] items)
		{
			/*System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Control + L",
            "Invokes Options Pane",
            "Copies last used face to clipboard."}, -1);*/
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeyDisplayForm));
			this.hotkeyList = new System.Windows.Forms.ListView();
			this.Hotkeys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Actions = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// hotkeyList
			// 
			this.hotkeyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Hotkeys,
            this.Actions});
			this.hotkeyList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hotkeyList.GridLines = true;
			this.hotkeyList.Items.AddRange(items);
			this.hotkeyList.Location = new System.Drawing.Point(0, 0);
			this.hotkeyList.Margin = new System.Windows.Forms.Padding(4);
			this.hotkeyList.Name = "hotkeyList";
			this.hotkeyList.Size = new System.Drawing.Size(482, 252);
			this.hotkeyList.TabIndex = 0;
			this.hotkeyList.UseCompatibleStateImageBehavior = false;
			this.hotkeyList.View = System.Windows.Forms.View.Details;
			this.hotkeyList.ShowItemToolTips = true;
			// 
			// Hotkeys
			// 
			this.Hotkeys.Text = "Hotkeys";
			this.Hotkeys.Width = 100;
			// 
			// Actions
			// 
			this.Actions.Text = "Actions";
			this.Actions.Width = 372;
			// 
			// HotkeyDisplayForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(482, 252);
			this.Controls.Add(this.hotkeyList);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HotkeyDisplayForm";
			this.Opacity = 0.95D;
			this.Text = "Hotkey actions";
			//this.TopMost = true;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView hotkeyList;
		private System.Windows.Forms.ColumnHeader Hotkeys;
		private System.Windows.Forms.ColumnHeader Actions;
	}
}