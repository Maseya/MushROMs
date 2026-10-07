

namespace MushROMs.Win.Dialogs.BaseForms
{
    partial class HeaderEditorForm
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
            headerEditorUserControl = new MushROMs.Win.Controls.HeaderEditorUserControl();
            SuspendLayout();
            //
            // btnOK
            //
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(233, 198);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 16;
            btnOK.Text = "&OK";
            btnOK.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(329, 198);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // headerEditorUserControl
            //
            headerEditorUserControl.Location = new Point(12, 12);
            headerEditorUserControl.Name = "headerEditorUserControl";
            headerEditorUserControl.Size = new Size(405, 180);
            headerEditorUserControl.TabIndex = 18;
            headerEditorUserControl.AreaHeaderChanged += HeaderEditorUserControl_AreaHeaderChanged;
            //
            // HeaderEditorForm
            //
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(429, 242);
            Controls.Add(headerEditorUserControl);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HeaderEditorForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Edit Header";
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.HeaderEditorUserControl headerEditorUserControl;
    }
}
