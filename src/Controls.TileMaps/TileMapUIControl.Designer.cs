namespace Maseya.Controls.TileMaps
{
    partial class TileMapUIControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TileMapUIControl));
            this.renderer = new Maseya.Controls.TileMaps.TileMapControlRenderer(this.components);
            this.checkerPatternRenderer = new Maseya.Controls.CheckerPatternRenderer(this.components);
            this.tileMapImageRenderer = new Maseya.Controls.TileMaps.TileMapImageRenderer(this.components);
            this.selectionPathRenderer = new Maseya.Controls.AnimatedPathRenderer(this.components);
            this.selectionRenderer = new Maseya.Controls.TileMaps.SelectionRenderer(this.components);
            this.viewTilePathRenderer = new Maseya.Controls.AnimatedPathRenderer(this.components);
            this.viewTileRenderer = new Maseya.Controls.TileMaps.ViewTileRenderer(this.components);
            this.selectionUI = new Maseya.Controls.TileMaps.SelectionUI(this.components);
            this.activeGridTileUI = new Maseya.Controls.TileMaps.ActiveGridTileUI(this.components);
            this.SuspendLayout();
            // 
            // renderer
            // 
            this.renderer.BackgroundRenderer = this.checkerPatternRenderer;
            this.renderer.ImageRenderer = this.tileMapImageRenderer;
            this.renderer.SelectionRenderer = this.selectionRenderer;
            this.renderer.TileMapControl = this;
            this.renderer.ViewTileRenderer = this.viewTileRenderer;
            // 
            // checkerPatternRenderer
            // 
            this.checkerPatternRenderer.Color1 = System.Drawing.SystemColors.Control;
            this.checkerPatternRenderer.Color2 = System.Drawing.SystemColors.ControlDark;
            this.checkerPatternRenderer.Size = new System.Drawing.Size(2, 2);
            // 
            // tileMapImageRenderer
            // 
            this.tileMapImageRenderer.TileMapRenderer = null;
            // 
            // selectionPathRenderer
            // 
            this.selectionPathRenderer.Interval = 250;
            this.selectionPathRenderer.Length1 = 4;
            this.selectionPathRenderer.Length2 = 4;
            // 
            // selectionRenderer
            // 
            this.selectionRenderer.PathRenderer = this.selectionPathRenderer;
            this.selectionRenderer.TileMap = null;
            // 
            // viewTilePathRenderer
            // 
            this.viewTilePathRenderer.Interval = 250;
            this.viewTilePathRenderer.Length1 = 2;
            this.viewTilePathRenderer.Length2 = 2;
            // 
            // viewTileRenderer
            // 
            this.viewTileRenderer.Padding = new System.Windows.Forms.Padding(1);
            this.viewTileRenderer.PathRenderer = this.viewTilePathRenderer;
            this.viewTileRenderer.TileMap = null;
            // 
            // selectionUI
            // 
            this.selectionUI.Control = this;
            this.selectionUI.TileMap = null;
            this.selectionUI.CurrentSelectionChanged += new System.EventHandler(this.SelectionUI_CurrentSelectionChanged);
            // 
            // activeGridTileUI
            // 
            this.activeGridTileUI.Control = this;
            this.activeGridTileUI.TileMap = null;
            this.activeGridTileUI.ActiveGridTileChanged += new System.EventHandler(this.ActiveGridTileUI_ActiveViewTileChanged);
            // 
            // TileMapUIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Name = "TileMapUIControl";
            this.Size = new System.Drawing.Size(148, 148);
            this.ResumeLayout(false);

        }

        #endregion

        private TileMapControlRenderer renderer;
        private CheckerPatternRenderer checkerPatternRenderer;
        private TileMapImageRenderer tileMapImageRenderer;
        private AnimatedPathRenderer selectionPathRenderer;
        private SelectionRenderer selectionRenderer;
        private AnimatedPathRenderer viewTilePathRenderer;
        private ViewTileRenderer viewTileRenderer;
        private SelectionUI selectionUI;
        private ActiveGridTileUI activeGridTileUI;
    }
}
