namespace Maseya.Snes.Controls
{
    partial class GfxTileEditorControl
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
            this.components = new System.ComponentModel.Container();
            this.tileMap1D = new Maseya.TileMaps.TileMap1D(this.components);
            this.gfxTileRenderer = new Maseya.Snes.GfxTileRenderer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileMap1D
            // 
            this.tileMap1D.GridLength = 0;
            this.tileMap1D.Origin = new System.Drawing.Point(0, 0);
            this.tileMap1D.TileSize = new System.Drawing.Size(16, 16);
            this.tileMap1D.ViewSize = new System.Drawing.Size(16, 16);
            // 
            // gfxTileRenderer
            // 
            this.gfxTileRenderer.Elapsed = System.TimeSpan.Parse("00:00:00");
            this.gfxTileRenderer.Gfx = null;
            this.gfxTileRenderer.PaletteStartIndex = 0;
            this.gfxTileRenderer.TileMap = this.tileMap1D;
            this.gfxTileRenderer.PaletteChanged += new System.EventHandler(this.Renderer_PaletteChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCut,
            this.cmiCopy,
            this.cmiPaste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 70);
            // 
            // cmiCut
            // 
            this.cmiCut.Image = global::Maseya.Snes.Controls.Properties.Resources.CutHS;
            this.cmiCut.Name = "cmiCut";
            this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cmiCut.Size = new System.Drawing.Size(144, 22);
            this.cmiCut.Text = "&Cut";
            this.cmiCut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // cmiCopy
            // 
            this.cmiCopy.Image = global::Maseya.Snes.Controls.Properties.Resources.CopyHS;
            this.cmiCopy.Name = "cmiCopy";
            this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cmiCopy.Size = new System.Drawing.Size(144, 22);
            this.cmiCopy.Text = "&Copy";
            this.cmiCopy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // cmiPaste
            // 
            this.cmiPaste.Image = global::Maseya.Snes.Controls.Properties.Resources.PasteHS;
            this.cmiPaste.Name = "cmiPaste";
            this.cmiPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.cmiPaste.Size = new System.Drawing.Size(144, 22);
            this.cmiPaste.Text = "&Paste";
            this.cmiPaste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // GfxTileEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GfxTileEditorControl";
            this.Size = new System.Drawing.Size(256, 256);
            this.TileMap = this.tileMap1D;
            this.TileMapRenderer = this.gfxTileRenderer;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TileMaps.TileMap1D tileMap1D;
        private GfxTileRenderer gfxTileRenderer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmiCut;
        private System.Windows.Forms.ToolStripMenuItem cmiCopy;
        private System.Windows.Forms.ToolStripMenuItem cmiPaste;
    }
}
