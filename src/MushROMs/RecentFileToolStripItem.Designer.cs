namespace Maseya.MushROMs
{
    partial class RecentFileToolStripItem
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tsmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOpenAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmRemove = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // tsmOpen
            // 
            this.tsmOpen.Image = global::Maseya.MushROMs.Properties.Resources.openHS;
            this.tsmOpen.Name = "tsmOpen";
            this.tsmOpen.Size = new System.Drawing.Size(119, 22);
            this.tsmOpen.Text = "&Open";
            // 
            // tsmOpenAs
            // 
            this.tsmOpenAs.Name = "tsmOpenAs";
            this.tsmOpenAs.Size = new System.Drawing.Size(119, 22);
            this.tsmOpenAs.Text = "Open &As";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(116, 6);
            // 
            // tsmRemove
            // 
            this.tsmRemove.Name = "tsmRemove";
            this.tsmRemove.Size = new System.Drawing.Size(119, 22);
            this.tsmRemove.Text = "R&emove";
            // 
            // RecentFileToolStripItem
            // 
            this.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOpen,
            this.tsmOpenAs,
            this.toolStripSeparator1,
            this.tsmRemove});

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmOpenAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmRemove;
    }
}
