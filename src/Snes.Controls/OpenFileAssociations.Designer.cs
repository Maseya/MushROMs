namespace Maseya.Snes.Controls
{
    partial class OpenFileAssociations
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
            this.chrFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.objFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.obj16FileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.gfxFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.rpfFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.mw3FileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.tplFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            this.palFileAssociation = new Maseya.Editors.IO.OpenFileAssociation(this.components);
            // 
            // chrFileAssociation
            // 
            this.chrFileAssociation.Description = "8x8 Sprite Tile (.spr8)";
            this.chrFileAssociation.EditorClass = "CHR";
            this.chrFileAssociation.Extension = ".spr8";
            // 
            // objFileAssociation
            // 
            this.objFileAssociation.Description = "8x8 Object Tile (.map8)";
            this.objFileAssociation.EditorClass = "OBJ";
            this.objFileAssociation.Extension = ".map8";
            // 
            // obj16FileAssociation
            // 
            this.obj16FileAssociation.Description = "16x16 Object Tile (.map16)";
            this.obj16FileAssociation.EditorClass = "OBJ16";
            this.obj16FileAssociation.Extension = ".map16";
            // 
            // gfxFileAssociation
            // 
            this.gfxFileAssociation.Description = "YY-CHR GFX (.chr)";
            this.gfxFileAssociation.EditorClass = "GFX";
            this.gfxFileAssociation.Extension = ".chr";
            // 
            // rpfFileAssociation
            // 
            this.rpfFileAssociation.Description = "Raw palette file (.rpf)";
            this.rpfFileAssociation.EditorClass = "Palette";
            this.rpfFileAssociation.Extension = ".rpf";
            // 
            // mw3FileAssociation
            // 
            this.mw3FileAssociation.Description = "Lunar Magic Palette (.mw3)";
            this.mw3FileAssociation.EditorClass = "Palette";
            this.mw3FileAssociation.Extension = ".mw3";
            // 
            // tplFileAssociation
            // 
            this.tplFileAssociation.Description = "Tile Layer Pro Palette (.tpl)";
            this.tplFileAssociation.EditorClass = "Palette";
            this.tplFileAssociation.Extension = ".tpl";
            // 
            // palFileAssociation
            // 
            this.palFileAssociation.Description = "24-bit Palette File (.pal)";
            this.palFileAssociation.EditorClass = "Palette";
            this.palFileAssociation.Extension = ".pal";

        }

        #endregion

        private Editors.IO.OpenFileAssociation chrFileAssociation;
        private Editors.IO.OpenFileAssociation objFileAssociation;
        private Editors.IO.OpenFileAssociation obj16FileAssociation;
        private Editors.IO.OpenFileAssociation gfxFileAssociation;
        private Editors.IO.OpenFileAssociation rpfFileAssociation;
        private Editors.IO.OpenFileAssociation mw3FileAssociation;
        private Editors.IO.OpenFileAssociation tplFileAssociation;
        private Editors.IO.OpenFileAssociation palFileAssociation;
    }
}
