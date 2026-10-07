// <copyright file="SpriteEditorForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace MushROMs.Win.Dialogs.BaseForms
{
    partial class SpriteEditorForm
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
            gbxBinary = new GroupBox();
            spriteEditorTextBox = new MushROMs.Win.Controls.SpriteEditorTextBox();
            btnCancel = new Button();
            btnOK = new Button();
            groupBox1 = new GroupBox();
            spriteEditorUserControl = new MushROMs.Win.Controls.SpriteEditorUserControl();
            gbxBinary.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            //
            // gbxBinary
            //
            gbxBinary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbxBinary.Controls.Add(spriteEditorTextBox);
            gbxBinary.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gbxBinary.Location = new Point(13, 222);
            gbxBinary.Margin = new Padding(4);
            gbxBinary.Name = "gbxBinary";
            gbxBinary.Padding = new Padding(4);
            gbxBinary.Size = new Size(149, 51);
            gbxBinary.TabIndex = 6;
            gbxBinary.TabStop = false;
            gbxBinary.Text = "Screen:Command";
            //
            // spriteEditorTextBox
            //
            spriteEditorTextBox.Location = new Point(7, 22);
            spriteEditorTextBox.Name = "spriteEditorTextBox";
            spriteEditorTextBox.PlaceholderText = "00 00 00";
            spriteEditorTextBox.Size = new Size(135, 22);
            spriteEditorTextBox.TabIndex = 0;
            spriteEditorTextBox.Text = "00:00 00";
            spriteEditorTextBox.AreaSpriteCommandChanged += SpriteEditorTextBox_AreaSpriteCommandChanged;
            spriteEditorTextBox.IsValidCommandChanged += SpriteEditorTextBox_IsValidCommandChanged;
            //
            // btnCancel
            //
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(266, 240);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 26);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOK
            //
            btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(170, 240);
            btnOK.Margin = new Padding(4);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 26);
            btnOK.TabIndex = 4;
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            //
            // groupBox1
            //
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(spriteEditorUserControl);
            groupBox1.Location = new Point(13, 13);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(341, 201);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Sprite";
            //
            // spriteEditorUserControl
            //
            spriteEditorUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            spriteEditorUserControl.Location = new Point(7, 23);
            spriteEditorUserControl.MaximumSize = new Size(1200, 726);
            spriteEditorUserControl.MinimumSize = new Size(327, 171);
            spriteEditorUserControl.Name = "spriteEditorUserControl";
            spriteEditorUserControl.Size = new Size(327, 171);
            spriteEditorUserControl.TabIndex = 0;
            spriteEditorUserControl.AreaSpriteCommandChanged += SpriteEditorUserControl_AreaSpriteCommandChanged;
            //
            // SpriteEditorForm
            //
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(367, 286);
            Controls.Add(groupBox1);
            Controls.Add(gbxBinary);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Margin = new Padding(4);
            MaximizeBox = false;
            MaximumSize = new Size(1200, 325);
            MinimizeBox = false;
            MinimumSize = new Size(383, 325);
            Name = "SpriteEditorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Sprite Editor";
            gbxBinary.ResumeLayout(false);
            gbxBinary.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbxBinary;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.SpriteEditorUserControl spriteEditorUserControl;
        private Controls.SpriteEditorTextBox spriteEditorTextBox;
    }
}
