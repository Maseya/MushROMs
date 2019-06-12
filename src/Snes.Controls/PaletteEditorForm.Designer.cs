namespace Maseya.Snes.Controls
{
    partial class PaletteEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditorForm));
            this.paletteEditorControl = new Maseya.Snes.Controls.PaletteEditorControl();
            this.SuspendLayout();
            // 
            // paletteEditorControl
            // 
            this.paletteEditorControl.ActiveViewTile = new System.Drawing.Point(0, 0);
            this.paletteEditorControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("paletteEditorControl.BackgroundImage")));
            this.paletteEditorControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paletteEditorControl.CurrentSelection = null;
            this.paletteEditorControl.Location = new System.Drawing.Point(0, 0);
            this.paletteEditorControl.Name = "paletteEditorControl";
            this.paletteEditorControl.Palette = null;
            this.paletteEditorControl.Size = new System.Drawing.Size(258, 258);
            this.paletteEditorControl.TabIndex = 0;
            this.paletteEditorControl.ZoomSize = Maseya.Controls.TileMaps.ZoomSize.Zoom2x;
            this.paletteEditorControl.PaletteChanged += new System.EventHandler(this.Control_PaletteChanged);
            // 
            // PaletteEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 278);
            this.Controls.Add(this.paletteEditorControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PaletteEditorForm";
            this.Text = "PaletteEditorForm";
            this.Controls.SetChildIndex(this.paletteEditorControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private PaletteEditorControl paletteEditorControl;
    }
}