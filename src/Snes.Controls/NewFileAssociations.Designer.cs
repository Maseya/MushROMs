namespace Maseya.Snes.Controls
{
    partial class NewFileAssociations
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
            this.paletteAssociation = new Maseya.Controls.Editors.NewFileIconAssociation(this.components);
            this.gfxAssociation = new Maseya.Controls.Editors.NewFileIconAssociation(this.components);
            this.chrAssociation = new Maseya.Controls.Editors.NewFileIconAssociation(this.components);
            this.objAssociation = new Maseya.Controls.Editors.NewFileIconAssociation(this.components);
            this.obj16Association = new Maseya.Controls.Editors.NewFileIconAssociation(this.components);
            // 
            // paletteAssociation
            // 
            this.paletteAssociation.Description = "";
            this.paletteAssociation.FileIcon = global::Maseya.Snes.Controls.Properties.Resources.Palette;
            this.paletteAssociation.FileType = "15-bit BGR (SNES) palette";
            this.paletteAssociation.PreviewImage = null;
            // 
            // gfxAssociation
            // 
            this.gfxAssociation.Description = null;
            this.gfxAssociation.FileIcon = null;
            this.gfxAssociation.FileType = "Graphics Pixel Map (GFX)";
            this.gfxAssociation.PreviewImage = null;
            // 
            // chrAssociation
            // 
            this.chrAssociation.Description = null;
            this.chrAssociation.FileIcon = null;
            this.chrAssociation.FileType = "16-bit 8x8 CHR Tilemap";
            this.chrAssociation.PreviewImage = null;
            // 
            // objAssociation
            // 
            this.objAssociation.Description = null;
            this.objAssociation.FileIcon = null;
            this.objAssociation.FileType = "16-bit 8x8 OBJ Tilemap";
            this.objAssociation.PreviewImage = null;
            // 
            // obj16Association
            // 
            this.obj16Association.Description = null;
            this.obj16Association.FileIcon = null;
            this.obj16Association.FileType = "64-bit 16x16 OBJ Tilemap";
            this.obj16Association.PreviewImage = null;

        }

        #endregion

        private Maseya.Controls.Editors.NewFileIconAssociation paletteAssociation;
        private Maseya.Controls.Editors.NewFileIconAssociation gfxAssociation;
        private Maseya.Controls.Editors.NewFileIconAssociation chrAssociation;
        private Maseya.Controls.Editors.NewFileIconAssociation objAssociation;
        private Maseya.Controls.Editors.NewFileIconAssociation obj16Association;
    }
}
