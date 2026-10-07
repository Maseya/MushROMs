// <copyright file="AutoSaveSettingsUserControl.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win;

partial class AutoSaveSettingsUserControl
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
        gbxPruneMode = new GroupBox();
        rdbProgressive = new RadioButton();
        rdbConstant = new RadioButton();
        cbxCutoffUnits = new ComboBox();
        chkPrune = new CheckBox();
        cbxUnits = new ComboBox();
        chkAutoSave = new CheckBox();
        nudCutoffTime = new NumericUpDown();
        nudTime = new NumericUpDown();
        gbxPruneMode.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudCutoffTime).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudTime).BeginInit();
        SuspendLayout();
        //
        // gbxPruneMode
        //
        gbxPruneMode.Controls.Add(rdbProgressive);
        gbxPruneMode.Controls.Add(rdbConstant);
        gbxPruneMode.Location = new Point(0, 58);
        gbxPruneMode.Name = "gbxPruneMode";
        gbxPruneMode.Size = new Size(366, 76);
        gbxPruneMode.TabIndex = 15;
        gbxPruneMode.TabStop = false;
        gbxPruneMode.Text = "Pruning Mode";
        //
        // rdbProgressive
        //
        rdbProgressive.AutoSize = true;
        rdbProgressive.Location = new Point(6, 51);
        rdbProgressive.Name = "rdbProgressive";
        rdbProgressive.Size = new Size(307, 19);
        rdbProgressive.TabIndex = 1;
        rdbProgressive.TabStop = true;
        rdbProgressive.Text = "Retain some saves but delete progressively older ones";
        rdbProgressive.UseVisualStyleBackColor = true;
        //
        // rdbConstant
        //
        rdbConstant.AutoSize = true;
        rdbConstant.Location = new Point(6, 26);
        rdbConstant.Name = "rdbConstant";
        rdbConstant.Size = new Size(222, 19);
        rdbConstant.TabIndex = 0;
        rdbConstant.TabStop = true;
        rdbConstant.Text = "Delete all saves older than cutoff date";
        rdbConstant.UseVisualStyleBackColor = true;
        //
        // cbxCutoffUnits
        //
        cbxCutoffUnits.FormattingEnabled = true;
        cbxCutoffUnits.Items.AddRange(new object[] { "minutes", "hours", "days", "weeks" });
        cbxCutoffUnits.Location = new Point(250, 30);
        cbxCutoffUnits.Name = "cbxCutoffUnits";
        cbxCutoffUnits.Size = new Size(116, 23);
        cbxCutoffUnits.TabIndex = 14;
        //
        // chkPrune
        //
        chkPrune.AutoSize = true;
        chkPrune.Checked = true;
        chkPrune.CheckState = CheckState.Checked;
        chkPrune.Location = new Point(0, 31);
        chkPrune.Name = "chkPrune";
        chkPrune.Size = new Size(176, 19);
        chkPrune.TabIndex = 12;
        chkPrune.Text = "Delete auto-saves older than";
        chkPrune.UseVisualStyleBackColor = true;
        chkPrune.CheckedChanged += Prune_CheckedChanged;
        //
        // cbxUnits
        //
        cbxUnits.FormattingEnabled = true;
        cbxUnits.Items.AddRange(new object[] { "seconds", "minutes", "hours" });
        cbxUnits.Location = new Point(250, 0);
        cbxUnits.Name = "cbxUnits";
        cbxUnits.Size = new Size(116, 23);
        cbxUnits.TabIndex = 11;
        //
        // chkAutoSave
        //
        chkAutoSave.AutoSize = true;
        chkAutoSave.Checked = true;
        chkAutoSave.CheckState = CheckState.Checked;
        chkAutoSave.Location = new Point(0, 2);
        chkAutoSave.Name = "chkAutoSave";
        chkAutoSave.Size = new Size(109, 19);
        chkAutoSave.TabIndex = 9;
        chkAutoSave.Text = "Auto save every";
        chkAutoSave.UseVisualStyleBackColor = true;
        chkAutoSave.CheckedChanged += AutoSave_CheckedChanged;
        //
        // nudCutoffTime
        //
        nudCutoffTime.Location = new Point(182, 30);
        nudCutoffTime.Name = "nudCutoffTime";
        nudCutoffTime.Size = new Size(61, 23);
        nudCutoffTime.TabIndex = 16;
        //
        // nudTime
        //
        nudTime.Location = new Point(182, 0);
        nudTime.Name = "nudTime";
        nudTime.Size = new Size(61, 23);
        nudTime.TabIndex = 17;
        //
        // AutoSaveSettingsUserControl
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(nudTime);
        Controls.Add(nudCutoffTime);
        Controls.Add(gbxPruneMode);
        Controls.Add(cbxCutoffUnits);
        Controls.Add(chkPrune);
        Controls.Add(cbxUnits);
        Controls.Add(chkAutoSave);
        Name = "AutoSaveSettingsUserControl";
        Size = new Size(366, 135);
        gbxPruneMode.ResumeLayout(false);
        gbxPruneMode.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudCutoffTime).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudTime).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox gbxPruneMode;
    private RadioButton rdbProgressive;
    private RadioButton rdbConstant;
    private ComboBox cbxCutoffUnits;
    private CheckBox chkPrune;
    private ComboBox cbxUnits;
    private CheckBox chkAutoSave;
    private NumericUpDown nudCutoffTime;
    private NumericUpDown nudTime;
}
