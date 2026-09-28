// <copyright file="MainForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win
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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mnuMain = new MenuStrip();
            tsmFile = new ToolStripMenuItem();
            tsmOpen = new ToolStripMenuItem();
            tsmOpenRecent = new ToolStripMenuItem();
            toolStripSeparator14 = new ToolStripSeparator();
            tsmClearRecent = new ToolStripMenuItem();
            tsmSave = new ToolStripMenuItem();
            tsmSaveAs = new ToolStripMenuItem();
            toolStripSeparator13 = new ToolStripSeparator();
            tsmSetupEmulator = new ToolStripMenuItem();
            tsmRunEmulator = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmClose = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            tsmExit = new ToolStripMenuItem();
            tsmEdit = new ToolStripMenuItem();
            tsmUndo = new ToolStripMenuItem();
            tsmRedo = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            tsmCut = new ToolStripMenuItem();
            tsmCopy = new ToolStripMenuItem();
            tsmPaste = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            tsmAddItem = new ToolStripMenuItem();
            tsmRemoveItem = new ToolStripMenuItem();
            tsmDeleteAll = new ToolStripMenuItem();
            tsmLevel = new ToolStripMenuItem();
            tsmLoadArea = new ToolStripMenuItem();
            tsmExportTileData = new ToolStripMenuItem();
            tsmEditHeader = new ToolStripMenuItem();
            tsmSpriteMode = new ToolStripMenuItem();
            tsmView = new ToolStripMenuItem();
            tsmPlayerState = new ToolStripMenuItem();
            tsmSmall = new ToolStripMenuItem();
            tsmBig = new ToolStripMenuItem();
            tsmFire = new ToolStripMenuItem();
            tsmPlayer = new ToolStripMenuItem();
            tsmMario = new ToolStripMenuItem();
            tsmLuigi = new ToolStripMenuItem();
            tsmViewObjectList = new ToolStripMenuItem();
            tsmHelp = new ToolStripMenuItem();
            tsmSettings = new ToolStripMenuItem();
            toolStripSeparator12 = new ToolStripSeparator();
            tsmSpecialThanks = new ToolStripMenuItem();
            tsmAbout = new ToolStripMenuItem();
            toolStrip = new ToolStrip();
            tsbOpen = new ToolStripButton();
            tsbOpenRecent = new ToolStripButton();
            tsbSave = new ToolStripButton();
            toolStripSeparator = new ToolStripSeparator();
            tslJumpToArea = new ToolStripLabel();
            ttbJumpToArea = new ToolStripTextBox();
            tsbJumpToArea = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            tsbLoadAreaByLevel = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            tsbUndo = new ToolStripButton();
            tsbRedo = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsbCut = new ToolStripButton();
            tsbCopy = new ToolStripButton();
            tsbPaste = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            tsbAddItem = new ToolStripButton();
            tsbRemoveItem = new ToolStripButton();
            tsbDeleteAll = new ToolStripButton();
            toolStripSeparator6 = new ToolStripSeparator();
            tsbSpriteMode = new ToolStripButton();
            toolStripSeparator9 = new ToolStripSeparator();
            tsbSpecialThanks = new ToolStripButton();
            tsbHelp = new ToolStripButton();
            hsbStartX = new HScrollBar();
            cmsMain = new ContextMenuStrip(components);
            cmiCut = new ToolStripMenuItem();
            cmiCopy = new ToolStripMenuItem();
            cmiPaste = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            cmiAddItem = new ToolStripMenuItem();
            cmiRemoveItem = new ToolStripMenuItem();
            cmiDeleteAll = new ToolStripMenuItem();
            areaControl = new Brutario.Win.Controls.DesignControl();
            headerEditor = new Brutario.Win.Views.HeaderEditor(components);
            objectEditor = new Brutario.Win.Views.ObjectEditor(components);
            spriteEditor = new Brutario.Win.Views.SpriteEditor(components);
            saveOnClosePrompt = new Brutario.Win.Views.SaveOnClosePrompt(components);
            exceptionHelper = new Brutario.Win.Views.ExceptionView(components);
            saveFileNameSelector = new Brutario.Win.Views.SaveFileNameSelector(components);
            openFileNameSelector = new Brutario.Win.Views.OpenFileNameSelector(components);
            animationTimer = new System.Windows.Forms.Timer(components);
            objectListView = new Brutario.Win.Views.ObjectListView(components);
            autoSaveTimer = new System.Windows.Forms.Timer(components);
            selectEmulatorDialog = new OpenFileDialog();
            cmsRecentRoms = new ContextMenuStrip(components);
            toolStripSeparator15 = new ToolStripSeparator();
            cmiClearRecent = new ToolStripMenuItem();
            mnuMain.SuspendLayout();
            toolStrip.SuspendLayout();
            cmsMain.SuspendLayout();
            cmsRecentRoms.SuspendLayout();
            SuspendLayout();
            // 
            // mnuMain
            // 
            mnuMain.ImageScalingSize = new Size(20, 20);
            mnuMain.Items.AddRange(new ToolStripItem[] { tsmFile, tsmEdit, tsmLevel, tsmView, tsmHelp });
            mnuMain.Location = new Point(0, 0);
            mnuMain.Name = "mnuMain";
            mnuMain.Padding = new Padding(7, 2, 0, 2);
            mnuMain.Size = new Size(901, 24);
            mnuMain.TabIndex = 0;
            mnuMain.Text = "menuStrip1";
            // 
            // tsmFile
            // 
            tsmFile.DropDownItems.AddRange(new ToolStripItem[] { tsmOpen, tsmOpenRecent, tsmSave, tsmSaveAs, toolStripSeparator13, tsmSetupEmulator, tsmRunEmulator, toolStripSeparator1, tsmClose, toolStripSeparator10, tsmExit });
            tsmFile.Name = "tsmFile";
            tsmFile.Size = new Size(37, 20);
            tsmFile.Text = "&File";
            // 
            // tsmOpen
            // 
            tsmOpen.Image = Properties.Resources.folder_open_solid;
            tsmOpen.Name = "tsmOpen";
            tsmOpen.ShortcutKeys = Keys.Control | Keys.O;
            tsmOpen.Size = new Size(186, 22);
            tsmOpen.Text = "&Open";
            tsmOpen.Click += Open_Click;
            // 
            // tsmOpenRecent
            // 
            tsmOpenRecent.DropDownItems.AddRange(new ToolStripItem[] { toolStripSeparator14, tsmClearRecent });
            tsmOpenRecent.Image = Properties.Resources.folder_multiple_solid;
            tsmOpenRecent.Name = "tsmOpenRecent";
            tsmOpenRecent.Size = new Size(186, 22);
            tsmOpenRecent.Text = "Open &Recent";
            // 
            // toolStripSeparator14
            // 
            toolStripSeparator14.Name = "toolStripSeparator14";
            toolStripSeparator14.Size = new Size(172, 6);
            // 
            // tsmClearRecent
            // 
            tsmClearRecent.Name = "tsmClearRecent";
            tsmClearRecent.Size = new Size(175, 22);
            tsmClearRecent.Text = "&Clear Recent ROMs";
            tsmClearRecent.Click += ClearRecentRoms_Click;
            // 
            // tsmSave
            // 
            tsmSave.Enabled = false;
            tsmSave.Image = Properties.Resources.floppy_disk_regular;
            tsmSave.Name = "tsmSave";
            tsmSave.ShortcutKeys = Keys.Control | Keys.S;
            tsmSave.Size = new Size(186, 22);
            tsmSave.Text = "&Save";
            tsmSave.Click += Save_Click;
            // 
            // tsmSaveAs
            // 
            tsmSaveAs.Enabled = false;
            tsmSaveAs.Name = "tsmSaveAs";
            tsmSaveAs.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            tsmSaveAs.Size = new Size(186, 22);
            tsmSaveAs.Text = "Save &As...";
            tsmSaveAs.Click += SaveAs_Click;
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(183, 6);
            // 
            // tsmSetupEmulator
            // 
            tsmSetupEmulator.Image = Properties.Resources.microchip_solid;
            tsmSetupEmulator.Name = "tsmSetupEmulator";
            tsmSetupEmulator.Size = new Size(186, 22);
            tsmSetupEmulator.Text = "Setup Emulator";
            tsmSetupEmulator.Click += SetupEmulator_Click;
            // 
            // tsmRunEmulator
            // 
            tsmRunEmulator.Enabled = false;
            tsmRunEmulator.Image = Properties.Resources.gamepad_solid;
            tsmRunEmulator.Name = "tsmRunEmulator";
            tsmRunEmulator.ShortcutKeys = Keys.F5;
            tsmRunEmulator.Size = new Size(186, 22);
            tsmRunEmulator.Text = "Run Emulator";
            tsmRunEmulator.Click += RunEmulator_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(183, 6);
            // 
            // tsmClose
            // 
            tsmClose.Enabled = false;
            tsmClose.Name = "tsmClose";
            tsmClose.ShortcutKeys = Keys.Control | Keys.F4;
            tsmClose.Size = new Size(186, 22);
            tsmClose.Text = "&Close";
            tsmClose.Click += Close_Click;
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(183, 6);
            // 
            // tsmExit
            // 
            tsmExit.Name = "tsmExit";
            tsmExit.ShortcutKeys = Keys.Alt | Keys.F4;
            tsmExit.Size = new Size(186, 22);
            tsmExit.Text = "E&xit";
            tsmExit.Click += Exit_Click;
            // 
            // tsmEdit
            // 
            tsmEdit.DropDownItems.AddRange(new ToolStripItem[] { tsmUndo, tsmRedo, toolStripSeparator7, tsmCut, tsmCopy, tsmPaste, toolStripSeparator8, tsmAddItem, tsmRemoveItem, tsmDeleteAll });
            tsmEdit.Name = "tsmEdit";
            tsmEdit.Size = new Size(39, 20);
            tsmEdit.Text = "&Edit";
            // 
            // tsmUndo
            // 
            tsmUndo.Enabled = false;
            tsmUndo.Image = Properties.Resources.arrow_rotate_left_solid;
            tsmUndo.Name = "tsmUndo";
            tsmUndo.ShortcutKeys = Keys.Control | Keys.Z;
            tsmUndo.Size = new Size(207, 22);
            tsmUndo.Text = "&Undo";
            tsmUndo.Click += Undo_Click;
            // 
            // tsmRedo
            // 
            tsmRedo.Enabled = false;
            tsmRedo.Image = Properties.Resources.arrow_rotate_right_solid;
            tsmRedo.Name = "tsmRedo";
            tsmRedo.ShortcutKeys = Keys.Control | Keys.Y;
            tsmRedo.Size = new Size(207, 22);
            tsmRedo.Text = "&Redo";
            tsmRedo.Click += Redo_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(204, 6);
            // 
            // tsmCut
            // 
            tsmCut.Enabled = false;
            tsmCut.Image = Properties.Resources.scissors_solid;
            tsmCut.Name = "tsmCut";
            tsmCut.ShortcutKeys = Keys.Control | Keys.X;
            tsmCut.Size = new Size(207, 22);
            tsmCut.Text = "Cu&t";
            tsmCut.Click += Cut_Click;
            // 
            // tsmCopy
            // 
            tsmCopy.Enabled = false;
            tsmCopy.Image = Properties.Resources.copy_regular;
            tsmCopy.Name = "tsmCopy";
            tsmCopy.ShortcutKeys = Keys.Control | Keys.C;
            tsmCopy.Size = new Size(207, 22);
            tsmCopy.Text = "&Copy";
            tsmCopy.Click += Copy_Click;
            // 
            // tsmPaste
            // 
            tsmPaste.Enabled = false;
            tsmPaste.Image = Properties.Resources.paste_regular;
            tsmPaste.Name = "tsmPaste";
            tsmPaste.ShortcutKeys = Keys.Control | Keys.V;
            tsmPaste.Size = new Size(207, 22);
            tsmPaste.Text = "&Paste";
            tsmPaste.Click += Paste_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(204, 6);
            // 
            // tsmAddItem
            // 
            tsmAddItem.Enabled = false;
            tsmAddItem.Image = Properties.Resources.plus_solid;
            tsmAddItem.Name = "tsmAddItem";
            tsmAddItem.ShortcutKeys = Keys.Insert;
            tsmAddItem.Size = new Size(207, 22);
            tsmAddItem.Text = "&Add Item";
            tsmAddItem.Click += AddItem_Click;
            // 
            // tsmRemoveItem
            // 
            tsmRemoveItem.Enabled = false;
            tsmRemoveItem.Image = Properties.Resources.minus_solid;
            tsmRemoveItem.Name = "tsmRemoveItem";
            tsmRemoveItem.ShortcutKeys = Keys.Delete;
            tsmRemoveItem.Size = new Size(207, 22);
            tsmRemoveItem.Text = "&Remove Item";
            tsmRemoveItem.Click += RemoveItem_Click;
            // 
            // tsmDeleteAll
            // 
            tsmDeleteAll.Enabled = false;
            tsmDeleteAll.Image = Properties.Resources.trash_can_solid;
            tsmDeleteAll.Name = "tsmDeleteAll";
            tsmDeleteAll.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Delete;
            tsmDeleteAll.Size = new Size(207, 22);
            tsmDeleteAll.Text = "&Delete All";
            tsmDeleteAll.Click += DeleteAll_Click;
            // 
            // tsmLevel
            // 
            tsmLevel.DropDownItems.AddRange(new ToolStripItem[] { tsmLoadArea, tsmExportTileData, tsmEditHeader, tsmSpriteMode });
            tsmLevel.Name = "tsmLevel";
            tsmLevel.Size = new Size(46, 20);
            tsmLevel.Text = "Level";
            // 
            // tsmLoadArea
            // 
            tsmLoadArea.Enabled = false;
            tsmLoadArea.Image = Properties.Resources.folder_tree_solid;
            tsmLoadArea.Name = "tsmLoadArea";
            tsmLoadArea.Size = new Size(183, 22);
            tsmLoadArea.Text = "Load Area";
            tsmLoadArea.Click += LoadArea_Click;
            // 
            // tsmExportTileData
            // 
            tsmExportTileData.Enabled = false;
            tsmExportTileData.Name = "tsmExportTileData";
            tsmExportTileData.Size = new Size(183, 22);
            tsmExportTileData.Text = "Export Tile Data";
            tsmExportTileData.Click += ExportTileData_Click;
            // 
            // tsmEditHeader
            // 
            tsmEditHeader.Enabled = false;
            tsmEditHeader.Name = "tsmEditHeader";
            tsmEditHeader.ShortcutKeys = Keys.Control | Keys.H;
            tsmEditHeader.Size = new Size(183, 22);
            tsmEditHeader.Text = "Edit Header";
            tsmEditHeader.Click += EditHeader_Click;
            // 
            // tsmSpriteMode
            // 
            tsmSpriteMode.CheckOnClick = true;
            tsmSpriteMode.Image = Properties.Resources.alien;
            tsmSpriteMode.Name = "tsmSpriteMode";
            tsmSpriteMode.ShortcutKeys = Keys.Control | Keys.M;
            tsmSpriteMode.Size = new Size(183, 22);
            tsmSpriteMode.Text = "Sprite Mode";
            tsmSpriteMode.Click += SpriteMode_Click;
            // 
            // tsmView
            // 
            tsmView.DropDownItems.AddRange(new ToolStripItem[] { tsmPlayerState, tsmPlayer, tsmViewObjectList });
            tsmView.Name = "tsmView";
            tsmView.Size = new Size(44, 20);
            tsmView.Text = "&View";
            // 
            // tsmPlayerState
            // 
            tsmPlayerState.DropDownItems.AddRange(new ToolStripItem[] { tsmSmall, tsmBig, tsmFire });
            tsmPlayerState.Name = "tsmPlayerState";
            tsmPlayerState.Size = new Size(254, 22);
            tsmPlayerState.Text = "Player &State";
            // 
            // tsmSmall
            // 
            tsmSmall.Name = "tsmSmall";
            tsmSmall.Size = new Size(103, 22);
            tsmSmall.Text = "&Small";
            tsmSmall.Click += PlayerState_Click;
            // 
            // tsmBig
            // 
            tsmBig.Name = "tsmBig";
            tsmBig.Size = new Size(103, 22);
            tsmBig.Text = "&Big";
            tsmBig.Click += PlayerState_Click;
            // 
            // tsmFire
            // 
            tsmFire.Name = "tsmFire";
            tsmFire.Size = new Size(103, 22);
            tsmFire.Text = "&Fire";
            tsmFire.Click += PlayerState_Click;
            // 
            // tsmPlayer
            // 
            tsmPlayer.DropDownItems.AddRange(new ToolStripItem[] { tsmMario, tsmLuigi });
            tsmPlayer.Name = "tsmPlayer";
            tsmPlayer.Size = new Size(254, 22);
            tsmPlayer.Text = "&Player";
            // 
            // tsmMario
            // 
            tsmMario.Name = "tsmMario";
            tsmMario.Size = new Size(105, 22);
            tsmMario.Text = "&Mario";
            tsmMario.Click += Player_Click;
            // 
            // tsmLuigi
            // 
            tsmLuigi.Name = "tsmLuigi";
            tsmLuigi.Size = new Size(105, 22);
            tsmLuigi.Text = "&Luigi";
            tsmLuigi.Click += Player_Click;
            // 
            // tsmViewObjectList
            // 
            tsmViewObjectList.CheckOnClick = true;
            tsmViewObjectList.Enabled = false;
            tsmViewObjectList.Name = "tsmViewObjectList";
            tsmViewObjectList.Size = new Size(254, 22);
            tsmViewObjectList.Text = "&Object List (Not yet implemented)";
            tsmViewObjectList.CheckedChanged += ViewObjectList_CheckedChanged;
            // 
            // tsmHelp
            // 
            tsmHelp.DropDownItems.AddRange(new ToolStripItem[] { tsmSettings, toolStripSeparator12, tsmSpecialThanks, tsmAbout });
            tsmHelp.Name = "tsmHelp";
            tsmHelp.Size = new Size(44, 20);
            tsmHelp.Text = "&Help";
            // 
            // tsmSettings
            // 
            tsmSettings.Image = Properties.Resources.gear_solid;
            tsmSettings.Name = "tsmSettings";
            tsmSettings.Size = new Size(151, 22);
            tsmSettings.Text = "Settings";
            tsmSettings.Click += Settings_Click;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(148, 6);
            // 
            // tsmSpecialThanks
            // 
            tsmSpecialThanks.Image = Properties.Resources.hands_clapping_solid;
            tsmSpecialThanks.Name = "tsmSpecialThanks";
            tsmSpecialThanks.Size = new Size(151, 22);
            tsmSpecialThanks.Text = "Special &Thanks";
            tsmSpecialThanks.Click += SpecialThanks_Click;
            // 
            // tsmAbout
            // 
            tsmAbout.Enabled = false;
            tsmAbout.Image = Properties.Resources.circle_question_regular;
            tsmAbout.Name = "tsmAbout";
            tsmAbout.ShortcutKeys = Keys.F1;
            tsmAbout.Size = new Size(151, 22);
            tsmAbout.Text = "&About";
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] { tsbOpen, tsbOpenRecent, tsbSave, toolStripSeparator, tslJumpToArea, ttbJumpToArea, tsbJumpToArea, toolStripSeparator4, tsbLoadAreaByLevel, toolStripSeparator3, tsbUndo, tsbRedo, toolStripSeparator2, tsbCut, tsbCopy, tsbPaste, toolStripSeparator5, tsbAddItem, tsbRemoveItem, tsbDeleteAll, toolStripSeparator6, tsbSpriteMode, toolStripSeparator9, tsbSpecialThanks, tsbHelp });
            toolStrip.Location = new Point(0, 24);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(901, 27);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpen.Image = Properties.Resources.folder_open_solid;
            tsbOpen.ImageTransparentColor = Color.Magenta;
            tsbOpen.Name = "tsbOpen";
            tsbOpen.Size = new Size(24, 24);
            tsbOpen.Text = "Open";
            tsbOpen.Click += Open_Click;
            // 
            // tsbOpenRecent
            // 
            tsbOpenRecent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpenRecent.Image = Properties.Resources.folder_multiple_solid;
            tsbOpenRecent.ImageTransparentColor = Color.Magenta;
            tsbOpenRecent.Name = "tsbOpenRecent";
            tsbOpenRecent.Size = new Size(24, 24);
            tsbOpenRecent.Text = "Open Recent...";
            tsbOpenRecent.Click += OpenRecent_Click;
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Enabled = false;
            tsbSave.Image = Properties.Resources.floppy_disk_regular;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(24, 24);
            tsbSave.Text = "Save";
            tsbSave.Click += Save_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(6, 27);
            // 
            // tslJumpToArea
            // 
            tslJumpToArea.Name = "tslJumpToArea";
            tslJumpToArea.Size = new Size(93, 24);
            tslJumpToArea.Text = "Jump to area: 0x";
            // 
            // ttbJumpToArea
            // 
            ttbJumpToArea.CharacterCasing = CharacterCasing.Upper;
            ttbJumpToArea.Enabled = false;
            ttbJumpToArea.MaxLength = 2;
            ttbJumpToArea.Name = "ttbJumpToArea";
            ttbJumpToArea.Size = new Size(116, 27);
            ttbJumpToArea.Text = "0";
            ttbJumpToArea.KeyDown += JumpToArea_KeyDown;
            ttbJumpToArea.TextChanged += JumpToArea_TextChanged;
            // 
            // tsbJumpToArea
            // 
            tsbJumpToArea.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbJumpToArea.Enabled = false;
            tsbJumpToArea.Image = Properties.Resources.map_regular;
            tsbJumpToArea.ImageTransparentColor = Color.Magenta;
            tsbJumpToArea.Name = "tsbJumpToArea";
            tsbJumpToArea.Size = new Size(24, 24);
            tsbJumpToArea.Text = "Jump to area";
            tsbJumpToArea.Click += JumpToArea_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 27);
            // 
            // tsbLoadAreaByLevel
            // 
            tsbLoadAreaByLevel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbLoadAreaByLevel.Enabled = false;
            tsbLoadAreaByLevel.Image = Properties.Resources.folder_tree_solid;
            tsbLoadAreaByLevel.ImageTransparentColor = Color.Magenta;
            tsbLoadAreaByLevel.Name = "tsbLoadAreaByLevel";
            tsbLoadAreaByLevel.Size = new Size(24, 24);
            tsbLoadAreaByLevel.Text = "Load area by level";
            tsbLoadAreaByLevel.ToolTipText = "Load area by level (not yet implemented)";
            tsbLoadAreaByLevel.Click += LoadArea_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 27);
            // 
            // tsbUndo
            // 
            tsbUndo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbUndo.Enabled = false;
            tsbUndo.Image = Properties.Resources.arrow_rotate_left_solid;
            tsbUndo.ImageTransparentColor = Color.Magenta;
            tsbUndo.Name = "tsbUndo";
            tsbUndo.Size = new Size(24, 24);
            tsbUndo.Text = "Undo";
            tsbUndo.Click += Undo_Click;
            // 
            // tsbRedo
            // 
            tsbRedo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRedo.Enabled = false;
            tsbRedo.Image = Properties.Resources.arrow_rotate_right_solid;
            tsbRedo.ImageTransparentColor = Color.Magenta;
            tsbRedo.Name = "tsbRedo";
            tsbRedo.Size = new Size(24, 24);
            tsbRedo.Text = "Redo";
            tsbRedo.Click += Redo_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 27);
            // 
            // tsbCut
            // 
            tsbCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCut.Enabled = false;
            tsbCut.Image = Properties.Resources.scissors_solid;
            tsbCut.ImageTransparentColor = Color.Magenta;
            tsbCut.Name = "tsbCut";
            tsbCut.Size = new Size(24, 24);
            tsbCut.Text = "Cut";
            tsbCut.ToolTipText = "Cut";
            tsbCut.Click += Cut_Click;
            // 
            // tsbCopy
            // 
            tsbCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCopy.Enabled = false;
            tsbCopy.Image = Properties.Resources.copy_regular;
            tsbCopy.ImageTransparentColor = Color.Magenta;
            tsbCopy.Name = "tsbCopy";
            tsbCopy.Size = new Size(24, 24);
            tsbCopy.Text = "Copy";
            tsbCopy.Click += Copy_Click;
            // 
            // tsbPaste
            // 
            tsbPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbPaste.Enabled = false;
            tsbPaste.Image = Properties.Resources.paste_regular;
            tsbPaste.ImageTransparentColor = Color.Magenta;
            tsbPaste.Name = "tsbPaste";
            tsbPaste.Size = new Size(24, 24);
            tsbPaste.Text = "Paste";
            tsbPaste.Click += Paste_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 27);
            // 
            // tsbAddItem
            // 
            tsbAddItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddItem.Enabled = false;
            tsbAddItem.Image = Properties.Resources.plus_solid;
            tsbAddItem.ImageTransparentColor = Color.Magenta;
            tsbAddItem.Name = "tsbAddItem";
            tsbAddItem.Size = new Size(24, 24);
            tsbAddItem.Text = "Add Item";
            tsbAddItem.Click += AddItem_Click;
            // 
            // tsbRemoveItem
            // 
            tsbRemoveItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemoveItem.Enabled = false;
            tsbRemoveItem.Image = Properties.Resources.minus_solid;
            tsbRemoveItem.ImageTransparentColor = Color.Magenta;
            tsbRemoveItem.Name = "tsbRemoveItem";
            tsbRemoveItem.Size = new Size(24, 24);
            tsbRemoveItem.Text = "Remove Item";
            tsbRemoveItem.ToolTipText = "Remove Item";
            tsbRemoveItem.Click += RemoveItem_Click;
            // 
            // tsbDeleteAll
            // 
            tsbDeleteAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteAll.Enabled = false;
            tsbDeleteAll.Image = Properties.Resources.trash_can_solid;
            tsbDeleteAll.ImageTransparentColor = Color.Magenta;
            tsbDeleteAll.Name = "tsbDeleteAll";
            tsbDeleteAll.Size = new Size(24, 24);
            tsbDeleteAll.Text = "Delete All Items";
            tsbDeleteAll.Click += DeleteAll_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 27);
            // 
            // tsbSpriteMode
            // 
            tsbSpriteMode.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSpriteMode.Image = Properties.Resources.alien;
            tsbSpriteMode.ImageTransparentColor = Color.Magenta;
            tsbSpriteMode.Name = "tsbSpriteMode";
            tsbSpriteMode.Size = new Size(24, 24);
            tsbSpriteMode.Text = "Sprite Mode";
            tsbSpriteMode.Click += SpriteMode_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 27);
            // 
            // tsbSpecialThanks
            // 
            tsbSpecialThanks.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSpecialThanks.Image = Properties.Resources.hands_clapping_solid;
            tsbSpecialThanks.ImageTransparentColor = Color.Magenta;
            tsbSpecialThanks.Name = "tsbSpecialThanks";
            tsbSpecialThanks.Size = new Size(24, 24);
            tsbSpecialThanks.Text = "Special Thanks";
            tsbSpecialThanks.Click += SpecialThanks_Click;
            // 
            // tsbHelp
            // 
            tsbHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbHelp.Enabled = false;
            tsbHelp.Image = Properties.Resources.circle_question_regular;
            tsbHelp.ImageTransparentColor = Color.Magenta;
            tsbHelp.Name = "tsbHelp";
            tsbHelp.Size = new Size(24, 24);
            tsbHelp.Text = "Help";
            // 
            // hsbStartX
            // 
            hsbStartX.Dock = DockStyle.Bottom;
            hsbStartX.Enabled = false;
            hsbStartX.LargeChange = 16;
            hsbStartX.Location = new Point(0, 342);
            hsbStartX.Maximum = 16;
            hsbStartX.Name = "hsbStartX";
            hsbStartX.Size = new Size(901, 17);
            hsbStartX.TabIndex = 4;
            hsbStartX.ValueChanged += StartX_ValueChanged;
            // 
            // cmsMain
            // 
            cmsMain.Enabled = false;
            cmsMain.ImageScalingSize = new Size(20, 20);
            cmsMain.Items.AddRange(new ToolStripItem[] { cmiCut, cmiCopy, cmiPaste, toolStripSeparator11, cmiAddItem, cmiRemoveItem, cmiDeleteAll });
            cmsMain.Name = "cmsMain";
            cmsMain.Size = new Size(208, 142);
            // 
            // cmiCut
            // 
            cmiCut.Enabled = false;
            cmiCut.Name = "cmiCut";
            cmiCut.ShortcutKeys = Keys.Control | Keys.X;
            cmiCut.Size = new Size(207, 22);
            cmiCut.Text = "C&ut";
            cmiCut.Click += Cut_Click;
            // 
            // cmiCopy
            // 
            cmiCopy.Enabled = false;
            cmiCopy.Name = "cmiCopy";
            cmiCopy.ShortcutKeys = Keys.Control | Keys.C;
            cmiCopy.Size = new Size(207, 22);
            cmiCopy.Text = "&Copy";
            cmiCopy.Click += Copy_Click;
            // 
            // cmiPaste
            // 
            cmiPaste.Enabled = false;
            cmiPaste.Name = "cmiPaste";
            cmiPaste.ShortcutKeys = Keys.Control | Keys.V;
            cmiPaste.Size = new Size(207, 22);
            cmiPaste.Text = "&Paste";
            cmiPaste.Click += Paste_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(204, 6);
            // 
            // cmiAddItem
            // 
            cmiAddItem.Enabled = false;
            cmiAddItem.Name = "cmiAddItem";
            cmiAddItem.ShortcutKeys = Keys.Insert;
            cmiAddItem.Size = new Size(207, 22);
            cmiAddItem.Text = "&Add Item";
            cmiAddItem.Click += AddItem_Click;
            // 
            // cmiRemoveItem
            // 
            cmiRemoveItem.Enabled = false;
            cmiRemoveItem.Name = "cmiRemoveItem";
            cmiRemoveItem.ShortcutKeys = Keys.Delete;
            cmiRemoveItem.Size = new Size(207, 22);
            cmiRemoveItem.Text = "&Remove Item";
            cmiRemoveItem.Click += RemoveItem_Click;
            // 
            // cmiDeleteAll
            // 
            cmiDeleteAll.Enabled = false;
            cmiDeleteAll.Name = "cmiDeleteAll";
            cmiDeleteAll.ShortcutKeys = Keys.Control | Keys.Shift | Keys.Delete;
            cmiDeleteAll.Size = new Size(207, 22);
            cmiDeleteAll.Text = "&Delete All";
            cmiDeleteAll.Click += DeleteAll_Click;
            // 
            // areaControl
            // 
            areaControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            areaControl.BorderStyle = BorderStyle.FixedSingle;
            areaControl.Enabled = false;
            areaControl.Location = new Point(0, 52);
            areaControl.Name = "areaControl";
            areaControl.Size = new Size(901, 290);
            areaControl.TabIndex = 5;
            areaControl.SizeChanged += AreaControl_SizeChanged;
            areaControl.Paint += AreaControl_Paint;
            areaControl.MouseClick += AreaControl_MouseClick;
            areaControl.MouseDoubleClick += AreaControl_MouseDoubleClick;
            areaControl.MouseDown += AreaControl_MouseDown;
            areaControl.MouseMove += AreaControl_MouseMove;
            areaControl.MouseUp += AreaControl_MouseUp;
            // 
            // headerEditor
            // 
            headerEditor.Owner = this;
            // 
            // objectEditor
            // 
            objectEditor.Owner = this;
            // 
            // spriteEditor
            // 
            spriteEditor.Owner = this;
            // 
            // saveOnClosePrompt
            // 
            saveOnClosePrompt.Caption = "Brutario";
            saveOnClosePrompt.Owner = this;
            saveOnClosePrompt.Text = "There are unsaved changes. Do you want save them before closing?";
            // 
            // exceptionHelper
            // 
            exceptionHelper.Owner = this;
            exceptionHelper.Title = "Brutario";
            // 
            // saveFileNameSelector
            // 
            saveFileNameSelector.FileName = "";
            saveFileNameSelector.Owner = this;
            // 
            // openFileNameSelector
            // 
            openFileNameSelector.FileName = "";
            openFileNameSelector.Owner = this;
            // 
            // animationTimer
            // 
            animationTimer.Interval = 3;
            animationTimer.Tick += Timer_Elapsed;
            // 
            // objectListView
            // 
            objectListView.VisibleChanged += ObjectListView_VisibleChanged;
            // 
            // autoSaveTimer
            // 
            autoSaveTimer.Interval = 1000;
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            // 
            // selectEmulatorDialog
            // 
            selectEmulatorDialog.Filter = "Emulator Executables (*.exe)|*.exe";
            selectEmulatorDialog.Title = "Select Emulator";
            // 
            // cmsRecentRoms
            // 
            cmsRecentRoms.Items.AddRange(new ToolStripItem[] { toolStripSeparator15, cmiClearRecent });
            cmsRecentRoms.Name = "cmsRecentRoms";
            cmsRecentRoms.Size = new Size(176, 32);
            // 
            // toolStripSeparator15
            // 
            toolStripSeparator15.Name = "toolStripSeparator15";
            toolStripSeparator15.Size = new Size(172, 6);
            // 
            // cmiClearRecent
            // 
            cmiClearRecent.Name = "cmiClearRecent";
            cmiClearRecent.Size = new Size(175, 22);
            cmiClearRecent.Text = "&Clear Recent ROMs";
            cmiClearRecent.Click += ClearRecentRoms_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(901, 359);
            Controls.Add(areaControl);
            Controls.Add(hsbStartX);
            Controls.Add(toolStrip);
            Controls.Add(mnuMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mnuMain;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(917, 398);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Brutario";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            mnuMain.ResumeLayout(false);
            mnuMain.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            cmsMain.ResumeLayout(false);
            cmsRecentRoms.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem tsmFile;
        private System.Windows.Forms.ToolStripMenuItem tsmOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.ToolStripLabel tslJumpToArea;
        private System.Windows.Forms.ToolStripTextBox ttbJumpToArea;
        private System.Windows.Forms.ToolStripButton tsbJumpToArea;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.HScrollBar hsbStartX;
        private System.Windows.Forms.ToolStripMenuItem tsmLevel;
        private System.Windows.Forms.ToolStripMenuItem tsmLoadArea;
        private System.Windows.Forms.ToolStripButton tsbLoadAreaByLevel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmExportTileData;
        private System.Windows.Forms.ToolStripMenuItem tsmView;
        private System.Windows.Forms.ToolStripMenuItem tsmEditHeader;
        private System.Windows.Forms.ToolStripMenuItem tsmSpriteMode;
        private System.Windows.Forms.ToolStripMenuItem tsmPlayer;
        private System.Windows.Forms.ToolStripMenuItem tsmPlayerState;
        private System.Windows.Forms.ToolStripMenuItem tsmSave;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveAs;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripButton tsbSpecialThanks;
        private System.Windows.Forms.ToolStripButton tsbSpriteMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmUndo;
        private System.Windows.Forms.ToolStripMenuItem tsmRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tsmCut;
        private System.Windows.Forms.ToolStripMenuItem tsmCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmSpecialThanks;
        private System.Windows.Forms.ToolStripMenuItem tsmAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsmAddItem;
        private System.Windows.Forms.ToolStripMenuItem tsmRemoveItem;
        private System.Windows.Forms.ToolStripMenuItem tsmDeleteAll;
        private System.Windows.Forms.ToolStripButton tsbAddItem;
        private System.Windows.Forms.ToolStripButton tsbRemoveItem;
        private System.Windows.Forms.ToolStripButton tsbDeleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem tsmMario;
        private System.Windows.Forms.ToolStripMenuItem tsmLuigi;
        private System.Windows.Forms.ToolStripMenuItem tsmSmall;
        private System.Windows.Forms.ToolStripMenuItem tsmBig;
        private System.Windows.Forms.ToolStripMenuItem tsmFire;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem cmiCut;
        private System.Windows.Forms.ToolStripMenuItem cmiCopy;
        private System.Windows.Forms.ToolStripMenuItem cmiPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem cmiAddItem;
        private System.Windows.Forms.ToolStripMenuItem cmiRemoveItem;
        private System.Windows.Forms.ToolStripMenuItem cmiDeleteAll;
        private Controls.DesignControl areaControl;
        private Views.HeaderEditor headerEditor;
        private Views.ObjectEditor objectEditor;
        private Views.SpriteEditor spriteEditor;
        private Views.SaveOnClosePrompt saveOnClosePrompt;
        private Views.ExceptionView exceptionHelper;
        private Views.SaveFileNameSelector saveFileNameSelector;
        private Views.OpenFileNameSelector openFileNameSelector;
        private System.Windows.Forms.Timer animationTimer;
        private ToolStripMenuItem tsmViewObjectList;
        private Views.ObjectListView objectListView;
        private System.Windows.Forms.Timer autoSaveTimer;
        private ToolStripMenuItem tsmSettings;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripMenuItem tsmSetupEmulator;
        private ToolStripMenuItem tsmRunEmulator;
        private OpenFileDialog selectEmulatorDialog;
        private ToolStripMenuItem tsmOpenRecent;
        private ToolStripButton tsbOpenRecent;
        private ContextMenuStrip cmsRecentRoms;
        private ToolStripMenuItem cmiClearRecent;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripMenuItem tsmClearRecent;
        private ToolStripSeparator toolStripSeparator15;
    }
}

