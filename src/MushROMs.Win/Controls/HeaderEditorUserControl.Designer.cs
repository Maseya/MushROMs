// <copyright file="HeaderEditorUserControl.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Controls;

partial class HeaderEditorUserControl
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
        cbxTerrainMode = new ComboBox();
        lblTerrainMode = new Label();
        cbxBackgroundScenery = new ComboBox();
        lblAreaPlatformType = new Label();
        cbxAreaPlatformType = new ComboBox();
        lblForeground = new Label();
        cbxForeground = new ComboBox();
        cbxPosition = new ComboBox();
        cbxTime = new ComboBox();
        lblScenery = new Label();
        lblPosition = new Label();
        lblTime = new Label();
        SuspendLayout();
        //
        // cbxTerrainMode
        //
        cbxTerrainMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxTerrainMode.FormattingEnabled = true;
        cbxTerrainMode.Items.AddRange(new object[] { "None", "2-tile-high floor with no ceiling", "2-tile-high floor with 1-tile-high ceiling", "2-tile-high floor with 3-tile-high ceiling", "2-tile-high floor with 4-tile-high ceiling", "2-tile-high floor with 8-tile-high ceiling", "5-tile-high floor with 1-tile-high ceiling", "5-tile-high floor with 3-tile-high ceiling", "5-tile-high floor with 4-tile-high ceiling", "6-tile-high floor with 1-tile-high ceiling", "No floor with 1-tile-high ceiling", "6-tile-high floor with 4-tile-high ceiling", "9-tile-high floor with 1-tile-high ceiling", "2-tile-high floor with 1-tile-high ceiling and 5 tiles in the middle", "2-tile-high floor with 1-tile-high ceiling and 4 tiles in the middle", "Floor tiles everywhere" });
        cbxTerrainMode.Location = new Point(120, 157);
        cbxTerrainMode.Margin = new Padding(4, 3, 4, 3);
        cbxTerrainMode.Name = "cbxTerrainMode";
        cbxTerrainMode.Size = new Size(285, 23);
        cbxTerrainMode.TabIndex = 27;
        cbxTerrainMode.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // lblTerrainMode
        //
        lblTerrainMode.AutoSize = true;
        lblTerrainMode.Location = new Point(0, 161);
        lblTerrainMode.Margin = new Padding(4, 0, 4, 0);
        lblTerrainMode.Name = "lblTerrainMode";
        lblTerrainMode.Size = new Size(76, 15);
        lblTerrainMode.TabIndex = 26;
        lblTerrainMode.Text = "Terrain Mode";
        //
        // cbxBackgroundScenery
        //
        cbxBackgroundScenery.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxBackgroundScenery.FormattingEnabled = true;
        cbxBackgroundScenery.Items.AddRange(new object[] { "Nothing", "Clouds", "Mountain", "Fence" });
        cbxBackgroundScenery.Location = new Point(120, 126);
        cbxBackgroundScenery.Margin = new Padding(4, 3, 4, 3);
        cbxBackgroundScenery.Name = "cbxBackgroundScenery";
        cbxBackgroundScenery.Size = new Size(285, 23);
        cbxBackgroundScenery.TabIndex = 25;
        cbxBackgroundScenery.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // lblAreaPlatformType
        //
        lblAreaPlatformType.AutoSize = true;
        lblAreaPlatformType.Location = new Point(0, 97);
        lblAreaPlatformType.Margin = new Padding(4, 0, 4, 0);
        lblAreaPlatformType.Name = "lblAreaPlatformType";
        lblAreaPlatformType.Size = new Size(107, 15);
        lblAreaPlatformType.TabIndex = 24;
        lblAreaPlatformType.Text = "Area Platform Type";
        //
        // cbxAreaPlatformType
        //
        cbxAreaPlatformType.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxAreaPlatformType.FormattingEnabled = true;
        cbxAreaPlatformType.Items.AddRange(new object[] { "Trees", "Mushrooms", "Bullet Bill Turrets", "Cloud Ground" });
        cbxAreaPlatformType.Location = new Point(120, 93);
        cbxAreaPlatformType.Margin = new Padding(4, 3, 4, 3);
        cbxAreaPlatformType.Name = "cbxAreaPlatformType";
        cbxAreaPlatformType.Size = new Size(285, 23);
        cbxAreaPlatformType.TabIndex = 23;
        cbxAreaPlatformType.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // lblForeground
        //
        lblForeground.AutoSize = true;
        lblForeground.Location = new Point(0, 66);
        lblForeground.Margin = new Padding(4, 0, 4, 0);
        lblForeground.Name = "lblForeground";
        lblForeground.Size = new Size(69, 15);
        lblForeground.TabIndex = 22;
        lblForeground.Text = "Foreground";
        //
        // cbxForeground
        //
        cbxForeground.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxForeground.FormattingEnabled = true;
        cbxForeground.Items.AddRange(new object[] { "None", "Underwater", "Castle Wall (Unused)", "Over Water", "Night (Unused)", "Snow (Unused)", "Night and Snow (Unused)", "Castle (unused)" });
        cbxForeground.Location = new Point(120, 62);
        cbxForeground.Margin = new Padding(4, 3, 4, 3);
        cbxForeground.Name = "cbxForeground";
        cbxForeground.Size = new Size(285, 23);
        cbxForeground.TabIndex = 21;
        cbxForeground.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // cbxPosition
        //
        cbxPosition.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxPosition.FormattingEnabled = true;
        cbxPosition.Items.AddRange(new object[] { "-1", "-1; from another area", "10", "4", "-1", "-1", "10 (Autowalk)", "10 (Autowalk)" });
        cbxPosition.Location = new Point(120, 31);
        cbxPosition.Margin = new Padding(4, 3, 4, 3);
        cbxPosition.Name = "cbxPosition";
        cbxPosition.Size = new Size(285, 23);
        cbxPosition.TabIndex = 20;
        cbxPosition.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // cbxTime
        //
        cbxTime.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxTime.FormattingEnabled = true;
        cbxTime.Items.AddRange(new object[] { "Not Set", "400", "300", "200" });
        cbxTime.Location = new Point(120, 0);
        cbxTime.Margin = new Padding(4, 3, 4, 3);
        cbxTime.Name = "cbxTime";
        cbxTime.Size = new Size(285, 23);
        cbxTime.TabIndex = 19;
        cbxTime.SelectedIndexChanged += Value_SelectedIndexChanged;
        //
        // lblScenery
        //
        lblScenery.AutoSize = true;
        lblScenery.Location = new Point(0, 129);
        lblScenery.Margin = new Padding(4, 0, 4, 0);
        lblScenery.Name = "lblScenery";
        lblScenery.Size = new Size(48, 15);
        lblScenery.TabIndex = 18;
        lblScenery.Text = "Scenery";
        //
        // lblPosition
        //
        lblPosition.AutoSize = true;
        lblPosition.Location = new Point(0, 34);
        lblPosition.Margin = new Padding(4, 0, 4, 0);
        lblPosition.Name = "lblPosition";
        lblPosition.Size = new Size(50, 15);
        lblPosition.TabIndex = 17;
        lblPosition.Text = "Position";
        //
        // lblTime
        //
        lblTime.AutoSize = true;
        lblTime.Location = new Point(0, 3);
        lblTime.Margin = new Padding(4, 0, 4, 0);
        lblTime.Name = "lblTime";
        lblTime.Size = new Size(33, 15);
        lblTime.TabIndex = 16;
        lblTime.Text = "Time";
        //
        // HeaderEditorUserControl
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(cbxTerrainMode);
        Controls.Add(lblTerrainMode);
        Controls.Add(cbxBackgroundScenery);
        Controls.Add(lblAreaPlatformType);
        Controls.Add(cbxAreaPlatformType);
        Controls.Add(lblForeground);
        Controls.Add(cbxForeground);
        Controls.Add(cbxPosition);
        Controls.Add(cbxTime);
        Controls.Add(lblScenery);
        Controls.Add(lblPosition);
        Controls.Add(lblTime);
        Name = "HeaderEditorUserControl";
        Size = new Size(405, 180);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ComboBox cbxTerrainMode;
    private Label lblTerrainMode;
    private ComboBox cbxBackgroundScenery;
    private Label lblAreaPlatformType;
    private ComboBox cbxAreaPlatformType;
    private Label lblForeground;
    private ComboBox cbxForeground;
    private ComboBox cbxPosition;
    private ComboBox cbxTime;
    private Label lblScenery;
    private Label lblPosition;
    private Label lblTime;
}
