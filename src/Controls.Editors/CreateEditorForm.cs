// <copyright file="CreateEditorForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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

            Entries = new List<Entry>();
        }

        public CreateEditorCallback CreateEditorCallback
        {
            get
            {
                return CurrentEntry.CreateEditorCallback;
            }
        }

        private List<Entry> Entries
        {
            get;
        }

        private Entry CurrentEntry
        {
            get
            {
                return Entries[CurrentRowIndex];
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

        public void AddEntry(
            Image fileIcon,
            string fileType,
            string description,
            Image previewImage,
            CreateEditorCallback createEditorCallback)
        {
            var entry = new Entry(
                fileIcon,
                fileType,
                description,
                previewImage,
                createEditorCallback);

            AddEntry(entry);
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

        private void AddEntry(Entry entry)
        {
            Entries.Add(entry);
            Rows.Add(entry.FileIcon, entry.FileType);
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

        private class Entry : IComponent
        {
            public Entry(
                Image fileIcon,
                string fileType,
                string description,
                Image previewImage,
                CreateEditorCallback createEditorCallback)
            {
                if (String.IsNullOrEmpty(fileType))
                {
                    throw new ArgumentNullException(nameof(fileType));
                }

                FileIcon = fileIcon != null
                    ? new Bitmap(fileIcon)
                    : new Bitmap(1, 1);

                PreviewImage = previewImage != null
                    ? new Bitmap(previewImage)
                    : new Bitmap(1, 1);

                FileType = fileType;
                Description = description;

                CreateEditorCallback = createEditorCallback
                    ?? throw new ArgumentNullException(
                        nameof(createEditorCallback));
            }

            public event EventHandler Disposed;

            public Bitmap FileIcon
            {
                get;
            }

            public string FileType
            {
                get;
            }

            public string Description
            {
                get;
            }

            public Bitmap PreviewImage
            {
                get;
            }

            public CreateEditorCallback CreateEditorCallback
            {
                get;
            }

            public ISite Site
            {
                get;
                set;
            }

            public override string ToString()
            {
                return FileType;
            }

            public void Dispose()
            {
                FileIcon.Dispose();
                PreviewImage.Dispose();

                Disposed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
