// <copyright file="CreateEditorForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.Editors.IO;

    internal partial class CreateEditorForm : Form
    {
        private static readonly Color HoveringCellColor =
            SystemColors.InactiveCaption;

        public CreateEditorForm()
        {
            InitializeComponent();

            dgvNewFileList.RowsAdded += (sender, e) => UpdateOkEnabled();
            dgvNewFileList.RowsRemoved += (sender, e) => UpdateOkEnabled();

            Associations = new List<NewFileIconAssociation>();
        }

        public CreateEditorCallback CreateEditorCallback
        {
            get
            {
                return CurrentEntry.CreateEditorCallback;
            }
        }

        private List<NewFileIconAssociation> Associations
        {
            get;
        }

        private NewFileIconAssociation CurrentEntry
        {
            get
            {
                return Associations[CurrentRowIndex];
            }
        }

        private DataGridViewRowCollection Rows
        {
            get
            {
                return dgvNewFileList.Rows;
            }
        }

        private DataGridViewRow CurrentRow
        {
            get
            {
                return dgvNewFileList.CurrentRow;
            }
        }

        private int CurrentRowIndex
        {
            get
            {
                return CurrentRow.Index;
            }
        }

        public void AddAssociation(
            Image fileIcon,
            string fileType,
            string description,
            Image previewImage,
            CreateEditorCallback createEditorCallback)
        {
            var entry = new NewFileIconAssociation()
            {
                FileIcon = fileIcon,
                FileType = fileType,
                Description = description,
                PreviewImage = previewImage,
                CreateEditorCallback = createEditorCallback,
            };

            AddAssociation(entry);
        }

        public void AddAssociation(NewFileIconAssociation association)
        {
            Associations.Add(association);
            Rows.Add(association.FileIcon, association.FileType);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.KeyCode == Keys.Enter)
            {
                AcceptButton.PerformClick();
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        private void SetRowBackColor(int rowIndex, Color color)
        {
            Rows[rowIndex].DefaultCellStyle.BackColor = color;
        }

        private void CurrentCellChanged(object sender, EventArgs e)
        {
            lblDescription.Text = CurrentEntry.Description;
            pbxPreview.Image = CurrentEntry.PreviewImage;
        }

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            SetRowBackColor(e.RowIndex, HoveringCellColor);
        }

        private void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            SetRowBackColor(
                e.RowIndex,
                dgvNewFileList.RowsDefaultCellStyle.BackColor);
        }

        private void UpdateOkEnabled()
        {
            btnOK.Enabled = dgvNewFileList.RowCount > 0;
        }

        private void CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
