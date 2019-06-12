namespace Maseya.MushROMs
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.tsmFile = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCustomize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMain = new System.Windows.Forms.ToolStrip();
            this.toolBarSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.EditorSelector = new Maseya.Editors.EditorSelector(this.components);
            this.NewFileHelper = new Maseya.Controls.Editors.NewFileHelper(this.components);
            this.ExceptionMessageBox = new Maseya.Controls.ExceptionMessageBox(this.components);
            this.openFileHelper = new Maseya.Controls.Editors.OpenFileHelper(this.components);
            this.paletteMenus = new Maseya.Snes.Controls.PaletteMenus(this.components);
            this.snesNewFileAssociations = new Maseya.Snes.Controls.NewFileAssociations(this.components);
            this.snesOpenFileAssociations = new Maseya.Snes.Controls.OpenFileAssociations(this.components);
            this.fileMenuHelper = new Maseya.MushROMs.FileMenuHelper(this.components);
            this.EditorFormHelper = new Maseya.MushROMs.EditorFormHelper();
            this.editMenuHelper = new Maseya.MushROMs.EditMenuHelper(this.components);
            this.mnuMain.SuspendLayout();
            this.tspMain.SuspendLayout();
            this.stsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFile,
            this.tsmEdit,
            this.tsmTools,
            this.tsmHelp});
            this.mnuMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            resources.ApplyResources(this.mnuMain, "mnuMain");
            this.mnuMain.Name = "mnuMain";
            // 
            // tsmFile
            // 
            this.tsmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileSeparator,
            this.tsmExit});
            this.tsmFile.Name = "tsmFile";
            resources.ApplyResources(this.tsmFile, "tsmFile");
            // 
            // fileSeparator
            // 
            this.fileSeparator.Name = "fileSeparator";
            resources.ApplyResources(this.fileSeparator, "fileSeparator");
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            resources.ApplyResources(this.tsmExit, "tsmExit");
            this.tsmExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // tsmEdit
            // 
            this.tsmEdit.Name = "tsmEdit";
            resources.ApplyResources(this.tsmEdit, "tsmEdit");
            // 
            // tsmTools
            // 
            this.tsmTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCustomize,
            this.tsmOptions});
            this.tsmTools.Name = "tsmTools";
            resources.ApplyResources(this.tsmTools, "tsmTools");
            // 
            // tsmCustomize
            // 
            resources.ApplyResources(this.tsmCustomize, "tsmCustomize");
            this.tsmCustomize.Name = "tsmCustomize";
            // 
            // tsmOptions
            // 
            resources.ApplyResources(this.tsmOptions, "tsmOptions");
            this.tsmOptions.Name = "tsmOptions";
            // 
            // tsmHelp
            // 
            this.tsmHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAbout});
            this.tsmHelp.Name = "tsmHelp";
            resources.ApplyResources(this.tsmHelp, "tsmHelp");
            // 
            // tsmAbout
            // 
            this.tsmAbout.Image = global::Maseya.MushROMs.Properties.Resources.Help;
            this.tsmAbout.Name = "tsmAbout";
            resources.ApplyResources(this.tsmAbout, "tsmAbout");
            this.tsmAbout.Click += new System.EventHandler(this.About_Click);
            // 
            // tspMain
            // 
            this.tspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarSeparator,
            this.tsbAbout});
            this.tspMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            resources.ApplyResources(this.tspMain, "tspMain");
            this.tspMain.Name = "tspMain";
            // 
            // toolBarSeparator
            // 
            this.toolBarSeparator.Name = "toolBarSeparator";
            resources.ApplyResources(this.toolBarSeparator, "toolBarSeparator");
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = global::Maseya.MushROMs.Properties.Resources.Help;
            resources.ApplyResources(this.tsbAbout, "tsbAbout");
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Click += new System.EventHandler(this.About_Click);
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatus});
            resources.ApplyResources(this.stsMain, "stsMain");
            this.stsMain.Name = "stsMain";
            // 
            // tssStatus
            // 
            resources.ApplyResources(this.tssStatus, "tssStatus");
            this.tssStatus.Name = "tssStatus";
            this.tssStatus.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // NewFileHelper
            // 
            this.NewFileHelper.ExceptionHandler = this.ExceptionMessageBox;
            // 
            // ExceptionMessageBox
            // 
            this.ExceptionMessageBox.Caption = null;
            this.ExceptionMessageBox.Owner = this;
            // 
            // openFileHelper
            // 
            this.openFileHelper.ExceptionHandler = this.ExceptionMessageBox;
            // 
            // paletteMenus
            // 
            this.paletteMenus.BlendEnabled = true;
            this.paletteMenus.CanExportImageEnabled = true;
            this.paletteMenus.ColorizeEnabled = true;
            this.paletteMenus.GrayscaleEnabled = true;
            this.paletteMenus.HorizontalGradientEnabled = true;
            this.paletteMenus.InvertColorsEnabled = true;
            this.paletteMenus.VerticalGradientEnabled = true;
            // 
            // fileMenuHelper
            // 
            this.fileMenuHelper.EditorSelector = this.EditorSelector;
            this.fileMenuHelper.NewFileHelper = this.NewFileHelper;
            this.fileMenuHelper.OpenFileHelper = this.openFileHelper;
            // 
            // EditorFormHelper
            // 
            this.EditorFormHelper.EditorSelector = this.EditorSelector;
            this.EditorFormHelper.MainForm = null;
            this.EditorFormHelper.EditorFormAdded += new System.EventHandler<Maseya.MushROMs.EditorFormEventArgs>(this.EditorFormAdded);
            // 
            // editMenuHelper
            // 
            this.editMenuHelper.EditorSelector = this.EditorSelector;
            this.editMenuHelper.ExportImageClick += new System.EventHandler(this.ExportImage_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.tspMain);
            this.Controls.Add(this.mnuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.tspMain.ResumeLayout(false);
            this.tspMain.PerformLayout();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem tsmFile;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.ToolStripMenuItem tsmTools;
        private System.Windows.Forms.ToolStripMenuItem tsmCustomize;
        private System.Windows.Forms.ToolStripMenuItem tsmOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout;
        private System.Windows.Forms.ToolStrip tspMain;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStripStatusLabel tssStatus;
        private Controls.Editors.NewFileHelper NewFileHelper;
        private Controls.ExceptionMessageBox ExceptionMessageBox;
        private Controls.Editors.OpenFileHelper openFileHelper;
        private Editors.EditorSelector EditorSelector;
        private FileMenuHelper fileMenuHelper;
        private Snes.Controls.PaletteMenus paletteMenus;
        private System.Windows.Forms.ToolStripSeparator fileSeparator;
        private System.Windows.Forms.ToolStripSeparator toolBarSeparator;
        private Snes.Controls.NewFileAssociations snesNewFileAssociations;
        private Snes.Controls.OpenFileAssociations snesOpenFileAssociations;
        private EditorFormHelper EditorFormHelper;
        private EditMenuHelper editMenuHelper;
        private System.Windows.Forms.ToolStripMenuItem tsmEdit;
    }
}

