namespace Maseya.Snes.Controls
{
    partial class GfxEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GfxEditorForm));
            this.pixelMapControl = new Maseya.Controls.TileMaps.TileMapControl();
            this.tileScrollHelper = new Maseya.Controls.TileMaps.ScrollHelper(this.components);
            this.tileHscrollBar = new System.Windows.Forms.HScrollBar();
            this.tileVScrolBar = new System.Windows.Forms.VScrollBar();
            this.pixelScrollHelper = new Maseya.Controls.TileMaps.ScrollHelper(this.components);
            this.pixelHScrollBar = new System.Windows.Forms.HScrollBar();
            this.pixelVscrollBar = new System.Windows.Forms.VScrollBar();
            this.resizeHelper = new Maseya.Controls.TileMaps.FormResizeHelper(this.components);
            this.gfxTileEditorControl = new Maseya.Snes.Controls.GfxTileEditorControl();
            this.SuspendLayout();
            // 
            // pixelMapControl
            // 
            this.pixelMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelMapControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pixelMapControl.BackgroundImage")));
            this.pixelMapControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pixelMapControl.Location = new System.Drawing.Point(277, 0);
            this.pixelMapControl.Name = "pixelMapControl";
            this.pixelMapControl.Size = new System.Drawing.Size(258, 258);
            this.pixelMapControl.TabIndex = 1;
            this.pixelMapControl.TileMap = null;
            // 
            // tileScrollHelper
            // 
            this.tileScrollHelper.HorizontalScrollBar = this.tileHscrollBar;
            this.tileScrollHelper.TileMap = null;
            this.tileScrollHelper.VerticalScrollBar = this.tileVScrolBar;
            // 
            // tileHscrollBar
            // 
            this.tileHscrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileHscrollBar.Location = new System.Drawing.Point(0, 259);
            this.tileHscrollBar.Name = "tileHscrollBar";
            this.tileHscrollBar.Size = new System.Drawing.Size(258, 17);
            this.tileHscrollBar.TabIndex = 2;
            // 
            // tileVScrolBar
            // 
            this.tileVScrolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tileVScrolBar.Location = new System.Drawing.Point(259, 0);
            this.tileVScrolBar.Name = "tileVScrolBar";
            this.tileVScrolBar.Size = new System.Drawing.Size(17, 258);
            this.tileVScrolBar.TabIndex = 4;
            // 
            // pixelScrollHelper
            // 
            this.pixelScrollHelper.HorizontalScrollBar = this.pixelHScrollBar;
            this.pixelScrollHelper.TileMap = null;
            this.pixelScrollHelper.VerticalScrollBar = this.pixelVscrollBar;
            // 
            // pixelHScrollBar
            // 
            this.pixelHScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelHScrollBar.Location = new System.Drawing.Point(277, 259);
            this.pixelHScrollBar.Name = "pixelHScrollBar";
            this.pixelHScrollBar.Size = new System.Drawing.Size(258, 17);
            this.pixelHScrollBar.TabIndex = 3;
            // 
            // pixelVscrollBar
            // 
            this.pixelVscrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelVscrollBar.Location = new System.Drawing.Point(536, 0);
            this.pixelVscrollBar.Name = "pixelVscrollBar";
            this.pixelVscrollBar.Size = new System.Drawing.Size(17, 258);
            this.pixelVscrollBar.TabIndex = 5;
            // 
            // resizeHelper
            // 
            this.resizeHelper.DesignForm = this;
            this.resizeHelper.MaximumTileSize = new System.Drawing.Size(0, 0);
            this.resizeHelper.MinimumTileSize = new System.Drawing.Size(0, 0);
            this.resizeHelper.TileMap = null;
            // 
            // gfxTileEditorControl
            // 
            this.gfxTileEditorControl.ActiveViewTile = new System.Drawing.Point(0, 0);
            this.gfxTileEditorControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gfxTileEditorControl.BackgroundImage")));
            this.gfxTileEditorControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gfxTileEditorControl.CurrentSelection = null;
            this.gfxTileEditorControl.Gfx = null;
            this.gfxTileEditorControl.Location = new System.Drawing.Point(0, 0);
            this.gfxTileEditorControl.Name = "gfxTileEditorControl";
            this.gfxTileEditorControl.Palette = null;
            this.gfxTileEditorControl.Size = new System.Drawing.Size(258, 258);
            this.gfxTileEditorControl.TabIndex = 6;
            this.gfxTileEditorControl.ZoomSize = Maseya.Controls.TileMaps.ZoomSize.Zoom2x;
            // 
            // GfxEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 278);
            this.Controls.Add(this.gfxTileEditorControl);
            this.Controls.Add(this.pixelVscrollBar);
            this.Controls.Add(this.tileVScrolBar);
            this.Controls.Add(this.pixelHScrollBar);
            this.Controls.Add(this.tileHscrollBar);
            this.Controls.Add(this.pixelMapControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GfxEditorForm";
            this.Text = "GFX Editor";
            this.ResumeLayout(false);

        }

        #endregion
        private Maseya.Controls.TileMaps.TileMapControl pixelMapControl;
        private Maseya.Controls.TileMaps.ScrollHelper tileScrollHelper;
        private Maseya.Controls.TileMaps.ScrollHelper pixelScrollHelper;
        private System.Windows.Forms.HScrollBar tileHscrollBar;
        private System.Windows.Forms.HScrollBar pixelHScrollBar;
        private System.Windows.Forms.VScrollBar tileVScrolBar;
        private System.Windows.Forms.VScrollBar pixelVscrollBar;
        private GfxTileEditorControl gfxTileEditorControl;
        private Maseya.Controls.TileMaps.FormResizeHelper resizeHelper;
    }
}