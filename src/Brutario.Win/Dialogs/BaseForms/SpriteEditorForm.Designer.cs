// <copyright file="SpriteEditorForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms
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
            tbxManualInput = new TextBox();
            chkUseManualInput = new CheckBox();
            btnCancel = new Button();
            btnOK = new Button();
            groupBox1 = new GroupBox();
            spriteEditorUserControl = new Brutario.Win.Controls.SpriteEditorUserControl();
            gbxBinary.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // gbxBinary
            // 
            gbxBinary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbxBinary.Controls.Add(tbxManualInput);
            gbxBinary.Controls.Add(chkUseManualInput);
            gbxBinary.Location = new Point(13, 222);
            gbxBinary.Margin = new Padding(4);
            gbxBinary.Name = "gbxBinary";
            gbxBinary.Padding = new Padding(4);
            gbxBinary.Size = new Size(149, 55);
            gbxBinary.TabIndex = 6;
            gbxBinary.TabStop = false;
            // 
            // tbxManualInput
            // 
            tbxManualInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbxManualInput.CharacterCasing = CharacterCasing.Upper;
            tbxManualInput.Location = new Point(8, 24);
            tbxManualInput.Margin = new Padding(4);
            tbxManualInput.MaxLength = 8;
            tbxManualInput.Name = "tbxManualInput";
            tbxManualInput.Size = new Size(133, 23);
            tbxManualInput.TabIndex = 1;
            tbxManualInput.WordWrap = false;
            tbxManualInput.TextChanged += ManualInput_TextChanged;
            // 
            // chkUseManualInput
            // 
            chkUseManualInput.AutoSize = true;
            chkUseManualInput.Location = new Point(10, 0);
            chkUseManualInput.Margin = new Padding(4);
            chkUseManualInput.Name = "chkUseManualInput";
            chkUseManualInput.Size = new Size(136, 19);
            chkUseManualInput.TabIndex = 0;
            chkUseManualInput.Text = "Enter value manually";
            chkUseManualInput.UseVisualStyleBackColor = true;
            chkUseManualInput.CheckedChanged += UseManualInput_CheckedChanged;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(266, 242);
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
            btnOK.Location = new Point(170, 242);
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
            ClientSize = new Size(367, 290);
            Controls.Add(groupBox1);
            Controls.Add(gbxBinary);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Margin = new Padding(4);
            MaximizeBox = false;
            MaximumSize = new Size(1200, 329);
            MinimizeBox = false;
            MinimumSize = new Size(383, 329);
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
        private System.Windows.Forms.TextBox tbxManualInput;
        private System.Windows.Forms.CheckBox chkUseManualInput;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.SpriteEditorUserControl spriteEditorUserControl;
    }
}
