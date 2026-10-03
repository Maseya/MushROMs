// <copyright file="ObjectEditorForm.Designer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Brutario.Win.Dialogs.BaseForms
{
    partial class ObjectEditorForm
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
            groupBox1 = new GroupBox();
            objectEditorUserControl = new Brutario.Win.Controls.ObjectEditorUserControl();
            gbxBinary = new GroupBox();
            tbxManualInput = new TextBox();
            chkUseManualInput = new CheckBox();
            groupBox1.SuspendLayout();
            gbxBinary.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(295, 215);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 0;
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(390, 215);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(objectEditorUserControl);
            groupBox1.Location = new Point(14, 14);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(463, 171);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Object";
            // 
            // objectEditorUserControl
            // 
            objectEditorUserControl.AreaPlatformType = Maseya.Smas.Smb1.AreaData.ObjectData.AreaPlatformType.Trees;
            objectEditorUserControl.Location = new Point(7, 18);
            objectEditorUserControl.Name = "objectEditorUserControl";
            objectEditorUserControl.Size = new Size(449, 147);
            objectEditorUserControl.TabIndex = 0;
            objectEditorUserControl.AreaPlatformTypeChanged += ObjectEditorUserControl_AreaPlatformTypeChanged;
            objectEditorUserControl.AreaObjectCommandChanged += ObjectEditorUserControl_AreaObjectCommandChanged;
            // 
            // gbxBinary
            // 
            gbxBinary.Controls.Add(tbxManualInput);
            gbxBinary.Controls.Add(chkUseManualInput);
            gbxBinary.Location = new Point(14, 192);
            gbxBinary.Margin = new Padding(4, 3, 4, 3);
            gbxBinary.Name = "gbxBinary";
            gbxBinary.Padding = new Padding(4, 3, 4, 3);
            gbxBinary.Size = new Size(219, 68);
            gbxBinary.TabIndex = 3;
            gbxBinary.TabStop = false;
            // 
            // tbxManualInput
            // 
            tbxManualInput.CharacterCasing = CharacterCasing.Upper;
            tbxManualInput.Location = new Point(10, 27);
            tbxManualInput.Margin = new Padding(4, 3, 4, 3);
            tbxManualInput.MaxLength = 8;
            tbxManualInput.Name = "tbxManualInput";
            tbxManualInput.Size = new Size(201, 23);
            tbxManualInput.TabIndex = 1;
            tbxManualInput.WordWrap = false;
            tbxManualInput.TextChanged += ManualInput_TextChanged;
            // 
            // chkUseManualInput
            // 
            chkUseManualInput.AutoSize = true;
            chkUseManualInput.Location = new Point(10, 0);
            chkUseManualInput.Margin = new Padding(4, 3, 4, 3);
            chkUseManualInput.Name = "chkUseManualInput";
            chkUseManualInput.Size = new Size(136, 19);
            chkUseManualInput.TabIndex = 0;
            chkUseManualInput.Text = "Enter value manually";
            chkUseManualInput.UseVisualStyleBackColor = true;
            chkUseManualInput.CheckedChanged += UseManualInput_CheckedChanged;
            // 
            // ObjectEditorForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(491, 273);
            Controls.Add(gbxBinary);
            Controls.Add(groupBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(452, 312);
            Name = "ObjectEditorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Object Editor";
            groupBox1.ResumeLayout(false);
            gbxBinary.ResumeLayout(false);
            gbxBinary.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbxBinary;
        private System.Windows.Forms.TextBox tbxManualInput;
        private System.Windows.Forms.CheckBox chkUseManualInput;
        private Controls.ObjectEditorUserControl objectEditorUserControl;
    }
}
