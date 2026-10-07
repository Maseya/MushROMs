// <copyright file="SpriteEditorUserControl.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Controls;

partial class SpriteEditorUserControl
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
        lblScreen = new Label();
        nudScreen = new NumericUpDown();
        lblAreaNumber = new Label();
        nudWorld = new NumericUpDown();
        lblWorld = new Label();
        lblDestPage = new Label();
        nudDestScreen = new NumericUpDown();
        chkHardFlag = new CheckBox();
        lblObject = new Label();
        cbxAreaSpriteCode = new ComboBox();
        lblY = new Label();
        lblX = new Label();
        nudY = new NumericUpDown();
        nudX = new NumericUpDown();
        gbxAreaPointer = new GroupBox();
        nudAreaNumber = new NumericUpDown();
        ((System.ComponentModel.ISupportInitialize)nudScreen).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudWorld).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDestScreen).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        gbxAreaPointer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAreaNumber).BeginInit();
        SuspendLayout();
        //
        // lblScreen
        //
        lblScreen.AutoSize = true;
        lblScreen.Location = new Point(0, 2);
        lblScreen.Margin = new Padding(4, 0, 4, 0);
        lblScreen.Name = "lblScreen";
        lblScreen.Size = new Size(45, 15);
        lblScreen.TabIndex = 0;
        lblScreen.Text = "Screen:";
        //
        // nudScreen
        //
        nudScreen.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudScreen.Location = new Point(53, 0);
        nudScreen.Margin = new Padding(4);
        nudScreen.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nudScreen.Name = "nudScreen";
        nudScreen.Size = new Size(41, 22);
        nudScreen.TabIndex = 1;
        nudScreen.ValueChanged += Item_ValueChanged;
        //
        // lblAreaNumber
        //
        lblAreaNumber.AutoSize = true;
        lblAreaNumber.Location = new Point(7, 23);
        lblAreaNumber.Margin = new Padding(4, 0, 4, 0);
        lblAreaNumber.Name = "lblAreaNumber";
        lblAreaNumber.Size = new Size(144, 15);
        lblAreaNumber.TabIndex = 0;
        lblAreaNumber.Text = "Destination Area Number:";
        //
        // nudWorld
        //
        nudWorld.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudWorld.Location = new Point(178, 81);
        nudWorld.Margin = new Padding(4);
        nudWorld.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
        nudWorld.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudWorld.Name = "nudWorld";
        nudWorld.Size = new Size(41, 22);
        nudWorld.TabIndex = 5;
        nudWorld.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudWorld.ValueChanged += Item_ValueChanged;
        //
        // lblWorld
        //
        lblWorld.AutoSize = true;
        lblWorld.Location = new Point(7, 82);
        lblWorld.Margin = new Padding(4, 0, 4, 0);
        lblWorld.Name = "lblWorld";
        lblWorld.Size = new Size(163, 15);
        lblWorld.TabIndex = 4;
        lblWorld.Text = "Area Pointer is valid on World";
        //
        // lblDestPage
        //
        lblDestPage.AutoSize = true;
        lblDestPage.Location = new Point(7, 52);
        lblDestPage.Margin = new Padding(4, 0, 4, 0);
        lblDestPage.Name = "lblDestPage";
        lblDestPage.Size = new Size(108, 15);
        lblDestPage.TabIndex = 2;
        lblDestPage.Text = "Destination Screen:";
        //
        // nudDestScreen
        //
        nudDestScreen.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudDestScreen.Hexadecimal = true;
        nudDestScreen.Location = new Point(178, 51);
        nudDestScreen.Margin = new Padding(4);
        nudDestScreen.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nudDestScreen.Name = "nudDestScreen";
        nudDestScreen.Size = new Size(41, 22);
        nudDestScreen.TabIndex = 3;
        nudDestScreen.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudDestScreen.ValueChanged += Item_ValueChanged;
        //
        // chkHardFlag
        //
        chkHardFlag.AutoSize = true;
        chkHardFlag.Location = new Point(250, 1);
        chkHardFlag.Margin = new Padding(4);
        chkHardFlag.Name = "chkHardFlag";
        chkHardFlag.Size = new Size(77, 19);
        chkHardFlag.TabIndex = 6;
        chkHardFlag.Text = "Hard Flag";
        chkHardFlag.UseVisualStyleBackColor = true;
        chkHardFlag.CheckedChanged += Item_ValueChanged;
        //
        // lblObject
        //
        lblObject.AutoSize = true;
        lblObject.Location = new Point(0, 34);
        lblObject.Margin = new Padding(4, 0, 4, 0);
        lblObject.Name = "lblObject";
        lblObject.Size = new Size(40, 15);
        lblObject.TabIndex = 7;
        lblObject.Text = "Sprite:";
        //
        // cbxAreaSpriteCode
        //
        cbxAreaSpriteCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbxAreaSpriteCode.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxAreaSpriteCode.FormattingEnabled = true;
        cbxAreaSpriteCode.Location = new Point(53, 31);
        cbxAreaSpriteCode.Margin = new Padding(4);
        cbxAreaSpriteCode.Name = "cbxAreaSpriteCode";
        cbxAreaSpriteCode.Size = new Size(274, 23);
        cbxAreaSpriteCode.TabIndex = 8;
        cbxAreaSpriteCode.SelectedIndexChanged += Item_ValueChanged;
        //
        // lblY
        //
        lblY.AutoSize = true;
        lblY.Location = new Point(176, 2);
        lblY.Margin = new Padding(4, 0, 4, 0);
        lblY.Name = "lblY";
        lblY.Size = new Size(17, 15);
        lblY.TabIndex = 4;
        lblY.Text = "Y:";
        //
        // lblX
        //
        lblX.AutoSize = true;
        lblX.Location = new Point(102, 2);
        lblX.Margin = new Padding(4, 0, 4, 0);
        lblX.Name = "lblX";
        lblX.Size = new Size(17, 15);
        lblX.TabIndex = 2;
        lblX.Text = "X:";
        //
        // nudY
        //
        nudY.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudY.Hexadecimal = true;
        nudY.Location = new Point(201, 0);
        nudY.Margin = new Padding(4);
        nudY.Maximum = new decimal(new int[] { 13, 0, 0, 0 });
        nudY.Name = "nudY";
        nudY.Size = new Size(41, 22);
        nudY.TabIndex = 5;
        nudY.ValueChanged += Item_ValueChanged;
        //
        // nudX
        //
        nudX.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudX.Hexadecimal = true;
        nudX.Location = new Point(127, 0);
        nudX.Margin = new Padding(4);
        nudX.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
        nudX.Name = "nudX";
        nudX.Size = new Size(41, 22);
        nudX.TabIndex = 3;
        nudX.ValueChanged += Item_ValueChanged;
        //
        // gbxAreaPointer
        //
        gbxAreaPointer.Controls.Add(nudDestScreen);
        gbxAreaPointer.Controls.Add(lblDestPage);
        gbxAreaPointer.Controls.Add(nudAreaNumber);
        gbxAreaPointer.Controls.Add(nudWorld);
        gbxAreaPointer.Controls.Add(lblAreaNumber);
        gbxAreaPointer.Controls.Add(lblWorld);
        gbxAreaPointer.Location = new Point(0, 61);
        gbxAreaPointer.Name = "gbxAreaPointer";
        gbxAreaPointer.Size = new Size(225, 110);
        gbxAreaPointer.TabIndex = 9;
        gbxAreaPointer.TabStop = false;
        gbxAreaPointer.Text = "Area Pointer";
        //
        // nudAreaNumber
        //
        nudAreaNumber.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudAreaNumber.Hexadecimal = true;
        nudAreaNumber.Location = new Point(178, 22);
        nudAreaNumber.Maximum = new decimal(new int[] { 127, 0, 0, 0 });
        nudAreaNumber.Name = "nudAreaNumber";
        nudAreaNumber.Size = new Size(41, 22);
        nudAreaNumber.TabIndex = 6;
        nudAreaNumber.Value = new decimal(new int[] { 37, 0, 0, 0 });
        nudAreaNumber.ValueChanged += Item_ValueChanged;
        //
        // SpriteEditorUserControl
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbxAreaPointer);
        Controls.Add(lblScreen);
        Controls.Add(nudScreen);
        Controls.Add(chkHardFlag);
        Controls.Add(lblObject);
        Controls.Add(cbxAreaSpriteCode);
        Controls.Add(lblY);
        Controls.Add(lblX);
        Controls.Add(nudY);
        Controls.Add(nudX);
        MaximumSize = new Size(1200, 726);
        MinimumSize = new Size(327, 171);
        Name = "SpriteEditorUserControl";
        Size = new Size(327, 171);
        ((System.ComponentModel.ISupportInitialize)nudScreen).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudWorld).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDestScreen).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        gbxAreaPointer.ResumeLayout(false);
        gbxAreaPointer.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudAreaNumber).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblScreen;
    private NumericUpDown nudScreen;
    private Label lblAreaNumber;
    private NumericUpDown nudWorld;
    private Label lblWorld;
    private Label lblDestPage;
    private NumericUpDown nudDestScreen;
    private CheckBox chkHardFlag;
    private Label lblObject;
    private ComboBox cbxAreaSpriteCode;
    private Label lblY;
    private Label lblX;
    private NumericUpDown nudY;
    private NumericUpDown nudX;
    private GroupBox gbxAreaPointer;
    private NumericUpDown nudAreaNumber;
}
