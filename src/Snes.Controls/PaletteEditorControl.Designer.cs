namespace Maseya.Snes.Controls
{
    partial class PaletteEditorControl
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
            this.paletteRenderer = new Maseya.Snes.PaletteRenderer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiInvertColors = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiBlend = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiColorize = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiGrayscale = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiHorizontalGradient = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiVerticalGradient = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileMap1D
            // 
            this.tileMap1D.GridLength = 0;
            this.tileMap1D.Origin = new System.Drawing.Point(0, 0);
            this.tileMap1D.TileSize = new System.Drawing.Size(16, 16);
            this.tileMap1D.ViewSize = new System.Drawing.Size(16, 16);
            // 
            // paletteRenderer
            // 
            this.paletteRenderer.Elapsed = System.TimeSpan.Parse("00:00:00");
            this.paletteRenderer.TileMap = this.tileMap1D;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiCut,
            this.cmiCopy,
            this.tsmPaste,
            this.toolStripSeparator1,
            this.cmiInvertColors,
            this.cmiBlend,
            this.cmiColorize,
            this.cmiGrayscale,
            this.cmiHorizontalGradient,
            this.cmiVerticalGradient});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(221, 230);
            // 
            // cmiCut
            // 
            this.cmiCut.Image = global::Maseya.Snes.Controls.Properties.Resources.CutHS;
            this.cmiCut.Name = "cmiCut";
            this.cmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cmiCut.Size = new System.Drawing.Size(220, 22);
            this.cmiCut.Text = "Cu&t";
            this.cmiCut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // cmiCopy
            // 
            this.cmiCopy.Image = global::Maseya.Snes.Controls.Properties.Resources.CopyHS;
            this.cmiCopy.Name = "cmiCopy";
            this.cmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cmiCopy.Size = new System.Drawing.Size(220, 22);
            this.cmiCopy.Text = "&Copy";
            this.cmiCopy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // tsmPaste
            // 
            this.tsmPaste.Image = global::Maseya.Snes.Controls.Properties.Resources.PasteHS;
            this.tsmPaste.Name = "tsmPaste";
            this.tsmPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.tsmPaste.Size = new System.Drawing.Size(220, 22);
            this.tsmPaste.Text = "&Paste";
            this.tsmPaste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // cmiInvertColors
            // 
            this.cmiInvertColors.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuInvert;
            this.cmiInvertColors.Name = "cmiInvertColors";
            this.cmiInvertColors.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.cmiInvertColors.Size = new System.Drawing.Size(220, 22);
            this.cmiInvertColors.Text = "&Invert Colors";
            this.cmiInvertColors.Click += new System.EventHandler(this.InvertColors_Click);
            // 
            // cmiBlend
            // 
            this.cmiBlend.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuBlend;
            this.cmiBlend.Name = "cmiBlend";
            this.cmiBlend.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.cmiBlend.Size = new System.Drawing.Size(220, 22);
            this.cmiBlend.Text = "&Blend";
            // 
            // cmiColorize
            // 
            this.cmiColorize.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuColorize;
            this.cmiColorize.Name = "cmiColorize";
            this.cmiColorize.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.cmiColorize.Size = new System.Drawing.Size(220, 22);
            this.cmiColorize.Text = "C&olorize";
            // 
            // cmiGrayscale
            // 
            this.cmiGrayscale.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuGrayscale;
            this.cmiGrayscale.Name = "cmiGrayscale";
            this.cmiGrayscale.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.cmiGrayscale.Size = new System.Drawing.Size(220, 22);
            this.cmiGrayscale.Text = "&Grayscale";
            // 
            // cmiHorizontalGradient
            // 
            this.cmiHorizontalGradient.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuHorizontalGradient;
            this.cmiHorizontalGradient.Name = "cmiHorizontalGradient";
            this.cmiHorizontalGradient.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.cmiHorizontalGradient.Size = new System.Drawing.Size(220, 22);
            this.cmiHorizontalGradient.Text = "&Horizontal Gradient";
            // 
            // cmiVerticalGradient
            // 
            this.cmiVerticalGradient.Image = global::Maseya.Snes.Controls.Properties.Resources.mnuVerticalGradient;
            this.cmiVerticalGradient.Name = "cmiVerticalGradient";
            this.cmiVerticalGradient.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.cmiVerticalGradient.Size = new System.Drawing.Size(220, 22);
            this.cmiVerticalGradient.Text = "&Vertical Gradient";
            // 
            // PaletteEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Name = "PaletteEditorControl";
            this.Size = new System.Drawing.Size(256, 256);
            this.TileMap = this.tileMap1D;
            this.TileMapRenderer = this.paletteRenderer;
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TileMaps.TileMap1D tileMap1D;
        private PaletteRenderer paletteRenderer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem cmiCut;
        private System.Windows.Forms.ToolStripMenuItem cmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmiInvertColors;
        private System.Windows.Forms.ToolStripMenuItem cmiBlend;
        private System.Windows.Forms.ToolStripMenuItem cmiColorize;
        private System.Windows.Forms.ToolStripMenuItem cmiGrayscale;
        private System.Windows.Forms.ToolStripMenuItem cmiHorizontalGradient;
        private System.Windows.Forms.ToolStripMenuItem cmiVerticalGradient;
    }
}
