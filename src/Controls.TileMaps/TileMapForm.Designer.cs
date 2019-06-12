namespace Maseya.Controls.TileMaps
{
    partial class TileMapForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.scrollHelper = new Maseya.Controls.TileMaps.ScrollHelper(this.components);
            this.tileMapHScrollBar = new System.Windows.Forms.HScrollBar();
            this.tileMapVScrollBar = new System.Windows.Forms.VScrollBar();
            this.formResizeHelper = new Maseya.Controls.TileMaps.FormResizeHelper(this.components);
            this.SuspendLayout();
            // 
            // scrollHelper
            // 
            this.scrollHelper.HorizontalScrollBar = this.tileMapHScrollBar;
            this.scrollHelper.TileMap = null;
            this.scrollHelper.VerticalScrollBar = this.tileMapVScrollBar;
            // 
            // tileMapHScrollBar
            // 
            this.tileMapHScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileMapHScrollBar.Location = new System.Drawing.Point(1, 260);
            this.tileMapHScrollBar.Name = "tileMapHScrollBar";
            this.tileMapHScrollBar.Size = new System.Drawing.Size(258, 17);
            this.tileMapHScrollBar.TabIndex = 3;
            // 
            // tileMapVScrollBar
            // 
            this.tileMapVScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileMapVScrollBar.Location = new System.Drawing.Point(260, 1);
            this.tileMapVScrollBar.Name = "tileMapVScrollBar";
            this.tileMapVScrollBar.Size = new System.Drawing.Size(17, 258);
            this.tileMapVScrollBar.TabIndex = 4;
            // 
            // formResizeHelper
            // 
            this.formResizeHelper.DesignForm = this;
            this.formResizeHelper.MaximumTileSize = new System.Drawing.Size(0, 0);
            this.formResizeHelper.MinimumTileSize = new System.Drawing.Size(0, 0);
            this.formResizeHelper.TileMap = null;
            // 
            // TileMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 278);
            this.Controls.Add(this.tileMapVScrollBar);
            this.Controls.Add(this.tileMapHScrollBar);
            this.Name = "TileMapForm";
            this.Text = "TileMapForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ScrollHelper scrollHelper;
        private FormResizeHelper formResizeHelper;
        private System.Windows.Forms.HScrollBar tileMapHScrollBar;
        private System.Windows.Forms.VScrollBar tileMapVScrollBar;
    }
}