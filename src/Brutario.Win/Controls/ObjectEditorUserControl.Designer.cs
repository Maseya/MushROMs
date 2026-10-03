// <copyright file="ObjectEditorUserControl.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Controls;

partial class ObjectEditorUserControl
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
        lblPage = new Label();
        nudPage = new NumericUpDown();
        lblForegroundScenery = new Label();
        cbxBackgroundScenery = new ComboBox();
        cbxForegroundScenery = new ComboBox();
        lblBackgroundScenery = new Label();
        cbxTerrainMode = new ComboBox();
        lblTerrainMode = new Label();
        nudLength = new NumericUpDown();
        lblLength = new Label();
        lblObject = new Label();
        cbxAreaObjectCode = new ComboBox();
        lblY = new Label();
        lblX = new Label();
        nudY = new NumericUpDown();
        nudX = new NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)nudPage).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLength).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        SuspendLayout();
        // 
        // lblPage
        // 
        lblPage.AutoSize = true;
        lblPage.Location = new Point(99, 2);
        lblPage.Margin = new Padding(4, 0, 4, 0);
        lblPage.Name = "lblPage";
        lblPage.Size = new Size(33, 15);
        lblPage.TabIndex = 39;
        lblPage.Text = "Page";
        // 
        // nudPage
        // 
        nudPage.Location = new Point(143, 0);
        nudPage.Margin = new Padding(4, 3, 4, 3);
        nudPage.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nudPage.Name = "nudPage";
        nudPage.Size = new Size(41, 23);
        nudPage.TabIndex = 38;
        nudPage.TextAlign = HorizontalAlignment.Center;
        nudPage.ValueChanged += Item_ValueChanged;
        // 
        // lblForegroundScenery
        // 
        lblForegroundScenery.AutoSize = true;
        lblForegroundScenery.Location = new Point(0, 127);
        lblForegroundScenery.Margin = new Padding(4, 0, 4, 0);
        lblForegroundScenery.Name = "lblForegroundScenery";
        lblForegroundScenery.Size = new Size(69, 15);
        lblForegroundScenery.TabIndex = 37;
        lblForegroundScenery.Text = "Foreground";
        // 
        // cbxBackgroundScenery
        // 
        cbxBackgroundScenery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbxBackgroundScenery.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxBackgroundScenery.FormattingEnabled = true;
        cbxBackgroundScenery.Items.AddRange(new object[] { "Nothing", "Clouds", "Mountain", "Fence" });
        cbxBackgroundScenery.Location = new Point(89, 92);
        cbxBackgroundScenery.Margin = new Padding(4, 3, 4, 3);
        cbxBackgroundScenery.Name = "cbxBackgroundScenery";
        cbxBackgroundScenery.Size = new Size(360, 23);
        cbxBackgroundScenery.TabIndex = 36;
        cbxBackgroundScenery.SelectedIndexChanged += Item_ValueChanged;
        // 
        // cbxForegroundScenery
        // 
        cbxForegroundScenery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbxForegroundScenery.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxForegroundScenery.FormattingEnabled = true;
        cbxForegroundScenery.Items.AddRange(new object[] { "None", "Underwater", "Castle Wall (Unused)", "Over Water", "Night (Unused)", "Snow (Unused)", "Night and Snow (Unused)", "Castle (unused)" });
        cbxForegroundScenery.Location = new Point(89, 124);
        cbxForegroundScenery.Margin = new Padding(4, 3, 4, 3);
        cbxForegroundScenery.Name = "cbxForegroundScenery";
        cbxForegroundScenery.Size = new Size(360, 23);
        cbxForegroundScenery.TabIndex = 35;
        cbxForegroundScenery.SelectedIndexChanged += Item_ValueChanged;
        // 
        // lblBackgroundScenery
        // 
        lblBackgroundScenery.AutoSize = true;
        lblBackgroundScenery.Location = new Point(0, 96);
        lblBackgroundScenery.Margin = new Padding(4, 0, 4, 0);
        lblBackgroundScenery.Name = "lblBackgroundScenery";
        lblBackgroundScenery.Size = new Size(48, 15);
        lblBackgroundScenery.TabIndex = 34;
        lblBackgroundScenery.Text = "Scenery";
        // 
        // cbxTerrainMode
        // 
        cbxTerrainMode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbxTerrainMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxTerrainMode.FormattingEnabled = true;
        cbxTerrainMode.Items.AddRange(new object[] { "None", "2-tile-high floor with no ceiling", "2-tile-high floor with 1-tile-high ceiling", "2-tile-high floor with 3-tile-high ceiling", "2-tile-high floor with 4-tile-high ceiling", "2-tile-high floor with 8-tile-high ceiling", "5-tile-high floor with 1-tile-high ceiling", "5-tile-high floor with 3-tile-high ceiling", "5-tile-high floor with 4-tile-high ceiling", "6-tile-high floor with 1-tile-high ceiling", "No floor with 1-tile-high ceiling", "6-tile-high floor with 4-tile-high ceiling", "9-tile-high floor with 1-tile-high ceiling", "2-tile-high floor with 1-tile-high ceiling and 5 tiles in the middle", "2-tile-high floor with 1-tile-high ceiling and 4 tiles in the middle", "Floor tiles everywhere" });
        cbxTerrainMode.Location = new Point(89, 61);
        cbxTerrainMode.Margin = new Padding(4, 3, 4, 3);
        cbxTerrainMode.Name = "cbxTerrainMode";
        cbxTerrainMode.Size = new Size(360, 23);
        cbxTerrainMode.TabIndex = 33;
        cbxTerrainMode.SelectedIndexChanged += Item_ValueChanged;
        // 
        // lblTerrainMode
        // 
        lblTerrainMode.AutoSize = true;
        lblTerrainMode.Location = new Point(0, 65);
        lblTerrainMode.Margin = new Padding(4, 0, 4, 0);
        lblTerrainMode.Name = "lblTerrainMode";
        lblTerrainMode.Size = new Size(76, 15);
        lblTerrainMode.TabIndex = 32;
        lblTerrainMode.Text = "Terrain Mode";
        // 
        // nudLength
        // 
        nudLength.Location = new Point(408, 0);
        nudLength.Margin = new Padding(4, 3, 4, 3);
        nudLength.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
        nudLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLength.Name = "nudLength";
        nudLength.Size = new Size(41, 23);
        nudLength.TabIndex = 31;
        nudLength.TextAlign = HorizontalAlignment.Center;
        nudLength.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudLength.ValueChanged += Item_ValueChanged;
        // 
        // lblLength
        // 
        lblLength.AutoSize = true;
        lblLength.Location = new Point(355, 2);
        lblLength.Margin = new Padding(4, 0, 4, 0);
        lblLength.Name = "lblLength";
        lblLength.Size = new Size(44, 15);
        lblLength.TabIndex = 30;
        lblLength.Text = "Length";
        // 
        // lblObject
        // 
        lblObject.AutoSize = true;
        lblObject.Location = new Point(0, 34);
        lblObject.Margin = new Padding(4, 0, 4, 0);
        lblObject.Name = "lblObject";
        lblObject.Size = new Size(42, 15);
        lblObject.TabIndex = 29;
        lblObject.Text = "Object";
        // 
        // cbxAreaObjectCode
        // 
        cbxAreaObjectCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbxAreaObjectCode.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxAreaObjectCode.FormattingEnabled = true;
        cbxAreaObjectCode.Location = new Point(51, 30);
        cbxAreaObjectCode.Margin = new Padding(4, 3, 4, 3);
        cbxAreaObjectCode.Name = "cbxAreaObjectCode";
        cbxAreaObjectCode.Size = new Size(397, 23);
        cbxAreaObjectCode.TabIndex = 28;
        cbxAreaObjectCode.SelectedIndexChanged += AreaObectCode_SelectedIndexChanged;
        // 
        // lblY
        // 
        lblY.AutoSize = true;
        lblY.Location = new Point(191, 2);
        lblY.Margin = new Padding(4, 0, 4, 0);
        lblY.Name = "lblY";
        lblY.Size = new Size(36, 15);
        lblY.TabIndex = 27;
        lblY.Text = "Y pos";
        // 
        // lblX
        // 
        lblX.AutoSize = true;
        lblX.Location = new Point(0, 2);
        lblX.Margin = new Padding(4, 0, 4, 0);
        lblX.Name = "lblX";
        lblX.Size = new Size(36, 15);
        lblX.TabIndex = 26;
        lblX.Text = "X pos";
        // 
        // nudY
        // 
        nudY.Location = new Point(238, 0);
        nudY.Margin = new Padding(4, 3, 4, 3);
        nudY.Maximum = new decimal(new int[] { 11, 0, 0, 0 });
        nudY.Name = "nudY";
        nudY.Size = new Size(41, 23);
        nudY.TabIndex = 25;
        nudY.TextAlign = HorizontalAlignment.Center;
        nudY.ValueChanged += Item_ValueChanged;
        // 
        // nudX
        // 
        nudX.Location = new Point(51, 0);
        nudX.Margin = new Padding(4, 3, 4, 3);
        nudX.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
        nudX.Name = "nudX";
        nudX.Size = new Size(41, 23);
        nudX.TabIndex = 24;
        nudX.TextAlign = HorizontalAlignment.Center;
        nudX.ValueChanged += Item_ValueChanged;
        // 
        // ObjectEditorUserControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(lblPage);
        Controls.Add(nudPage);
        Controls.Add(lblForegroundScenery);
        Controls.Add(cbxBackgroundScenery);
        Controls.Add(cbxForegroundScenery);
        Controls.Add(lblBackgroundScenery);
        Controls.Add(cbxTerrainMode);
        Controls.Add(lblTerrainMode);
        Controls.Add(nudLength);
        Controls.Add(lblLength);
        Controls.Add(lblObject);
        Controls.Add(cbxAreaObjectCode);
        Controls.Add(lblY);
        Controls.Add(lblX);
        Controls.Add(nudY);
        Controls.Add(nudX);
        Name = "ObjectEditorUserControl";
        Size = new Size(449, 147);
        ((System.ComponentModel.ISupportInitialize)nudPage).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLength).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblPage;
    private NumericUpDown nudPage;
    private Label lblForegroundScenery;
    private ComboBox cbxBackgroundScenery;
    private ComboBox cbxForegroundScenery;
    private Label lblBackgroundScenery;
    private ComboBox cbxTerrainMode;
    private Label lblTerrainMode;
    private NumericUpDown nudLength;
    private Label lblLength;
    private Label lblObject;
    private ComboBox cbxAreaObjectCode;
    private Label lblY;
    private Label lblX;
    private NumericUpDown nudY;
    private NumericUpDown nudX;
}
