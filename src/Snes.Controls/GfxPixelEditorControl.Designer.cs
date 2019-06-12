namespace Maseya.Snes.Controls
{
    partial class GfxPixelEditorControl
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
            this.tileMap2D = new Maseya.TileMaps.TileMap2D();
            this.SuspendLayout();
            // 
            // tileMap2D
            // 
            this.tileMap2D.Origin = new System.Drawing.Point(0, 0);
            this.tileMap2D.TileSize = new System.Drawing.Size(16, 16);
            this.tileMap2D.ViewSize = new System.Drawing.Size(16, 16);
            // 
            // GfxPixelEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "GfxPixelEditorControl";
            this.Size = new System.Drawing.Size(256, 256);
            this.TileMap = this.tileMap2D;
            this.ResumeLayout(false);

        }

        #endregion

        private TileMaps.TileMap2D tileMap2D;
    }
}
