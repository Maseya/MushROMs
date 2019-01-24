namespace Maseya.Controls.Editors
{
    partial class CreateEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateEditorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pbxPreview = new System.Windows.Forms.PictureBox();
            this.dgvNewFileList = new Maseya.Controls.BufferedDataGridView();
            this.dgcIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDescription.Name = "lblDescription";
            // 
            // pbxPreview
            // 
            resources.ApplyResources(this.pbxPreview, "pbxPreview");
            this.pbxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxPreview.Name = "pbxPreview";
            this.pbxPreview.TabStop = false;
            // 
            // dgvNewFileList
            // 
            this.dgvNewFileList.AllowUserToAddRows = false;
            this.dgvNewFileList.AllowUserToDeleteRows = false;
            this.dgvNewFileList.AllowUserToResizeColumns = false;
            this.dgvNewFileList.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvNewFileList, "dgvNewFileList");
            this.dgvNewFileList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvNewFileList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvNewFileList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvNewFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNewFileList.ColumnHeadersVisible = false;
            this.dgvNewFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcIcon,
            this.dgcName});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNewFileList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNewFileList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvNewFileList.EnableHeadersVisualStyles = false;
            this.dgvNewFileList.MultiSelect = false;
            this.dgvNewFileList.Name = "dgvNewFileList";
            this.dgvNewFileList.ReadOnly = true;
            this.dgvNewFileList.RowHeadersVisible = false;
            this.dgvNewFileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvNewFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNewFileList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellDoubleClick);
            this.dgvNewFileList.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellMouseEnter);
            this.dgvNewFileList.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellMouseLeave);
            this.dgvNewFileList.CurrentCellChanged += new System.EventHandler(this.CurrentCellChanged);
            // 
            // dgcIcon
            // 
            this.dgcIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.dgcIcon, "dgcIcon");
            this.dgcIcon.Name = "dgcIcon";
            this.dgcIcon.ReadOnly = true;
            this.dgcIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dgcName
            // 
            this.dgcName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgcName, "dgcName");
            this.dgcName.Name = "dgcName";
            this.dgcName.ReadOnly = true;
            this.dgcName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // CreateEditorForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.pbxPreview);
            this.Controls.Add(this.dgvNewFileList);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateEditorForm";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFileList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription;
        private BufferedDataGridView dgvNewFileList;
        private System.Windows.Forms.PictureBox pbxPreview;
        private System.Windows.Forms.DataGridViewImageColumn dgcIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcName;
    }
}