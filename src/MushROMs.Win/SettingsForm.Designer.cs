// <copyright file="SettingsForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

partial class SettingsForm
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
        btnOK = new Button();
        btnCancel = new Button();
        tabControl = new TabControl();
        tabAutoSave = new TabPage();
        autoSaveSettings = new AutoSaveSettingsUserControl();
        tabMisc = new TabPage();
        chkLoadLastOpenedRom = new CheckBox();
        tabControl.SuspendLayout();
        tabAutoSave.SuspendLayout();
        tabMisc.SuspendLayout();
        SuspendLayout();
        //
        // btnOK
        //
        btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnOK.DialogResult = DialogResult.OK;
        btnOK.Location = new Point(205, 201);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(94, 29);
        btnOK.TabIndex = 0;
        btnOK.Text = "&OK";
        btnOK.UseVisualStyleBackColor = true;
        //
        // btnCancel
        //
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(305, 201);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(94, 29);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "&Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // tabControl
        //
        tabControl.Appearance = TabAppearance.FlatButtons;
        tabControl.Controls.Add(tabAutoSave);
        tabControl.Controls.Add(tabMisc);
        tabControl.Location = new Point(12, 12);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(387, 183);
        tabControl.SizeMode = TabSizeMode.Fixed;
        tabControl.TabIndex = 2;
        //
        // tabAutoSave
        //
        tabAutoSave.BackColor = Color.Transparent;
        tabAutoSave.Controls.Add(autoSaveSettings);
        tabAutoSave.Location = new Point(4, 27);
        tabAutoSave.Name = "tabAutoSave";
        tabAutoSave.Padding = new Padding(3);
        tabAutoSave.Size = new Size(379, 152);
        tabAutoSave.TabIndex = 0;
        tabAutoSave.Text = "Auto Save";
        //
        // autoSaveSettings
        //
        autoSaveSettings.AutoSaveInterval = TimeSpan.Parse("00:00:00");
        autoSaveSettings.EnableAutoSave = true;
        autoSaveSettings.EnablePruning = true;
        autoSaveSettings.HardCutoff = false;
        autoSaveSettings.Location = new Point(1, 6);
        autoSaveSettings.Name = "autoSaveSettings";
        autoSaveSettings.PruningInterval = TimeSpan.Parse("00:00:00");
        autoSaveSettings.Size = new Size(368, 136);
        autoSaveSettings.TabIndex = 0;
        //
        // tabMisc
        //
        tabMisc.BackColor = Color.Transparent;
        tabMisc.Controls.Add(chkLoadLastOpenedRom);
        tabMisc.Location = new Point(4, 27);
        tabMisc.Name = "tabMisc";
        tabMisc.Padding = new Padding(3);
        tabMisc.Size = new Size(379, 152);
        tabMisc.TabIndex = 1;
        tabMisc.Text = "Misc.";
        //
        // chkLoadLastOpenedRom
        //
        chkLoadLastOpenedRom.AutoSize = true;
        chkLoadLastOpenedRom.Checked = true;
        chkLoadLastOpenedRom.CheckState = CheckState.Checked;
        chkLoadLastOpenedRom.Location = new Point(6, 6);
        chkLoadLastOpenedRom.Name = "chkLoadLastOpenedRom";
        chkLoadLastOpenedRom.Size = new Size(203, 19);
        chkLoadLastOpenedRom.TabIndex = 0;
        chkLoadLastOpenedRom.Text = "Load last opened ROM on startup";
        chkLoadLastOpenedRom.UseVisualStyleBackColor = true;
        //
        // SettingsForm
        //
        AcceptButton = btnOK;
        CancelButton = btnCancel;
        ClientSize = new Size(411, 242);
        Controls.Add(tabControl);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingsForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        Text = "Settings";
        tabControl.ResumeLayout(false);
        tabAutoSave.ResumeLayout(false);
        tabMisc.ResumeLayout(false);
        tabMisc.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private TabControl tabControl;
    private TabPage tabAutoSave;
    private TabPage tabMisc;
    private AutoSaveSettingsUserControl autoSaveSettings;
    private CheckBox chkLoadLastOpenedRom;
}
