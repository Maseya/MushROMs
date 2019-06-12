// <copyright file="SaveAsForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.Windows.Forms;
    using Maseya.Editors.IO;

    internal partial class SaveAsForm : Form
    {
        public SaveAsForm()
        {
            InitializeComponent();
        }

        public SaveEditorCallback SaveEditorCallback
        {
            get
            {
                var info = lbxEditors.SelectedItem as
                    SaveFileAssociation;

                return info.SaveEditorCallback;
            }
        }

        public void ClearEditors()
        {
            lbxEditors.Items.Clear();
        }

        public void AddAssociation(
                string extension,
                string description,
                string editorClass,
                SaveEditorCallback saveEditorCallback)
        {
            var association = new SaveFileAssociation()
            {
                Extension = extension,
                Description = description,
                EditorClass = editorClass,
                SaveEditorCallback = saveEditorCallback,
            };

            AddAssociation(association);
        }

        public void AddAssociation(SaveFileAssociation association)
        {
            if (association is null)
            {
                throw new ArgumentNullException(nameof(association));
            }

            lbxEditors.Items.Add(association);
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (lbxEditors.SelectedItem is null)
            {
                return;
            }

            btnOK.PerformClick();
        }

        private void OpenWithForm_Load(object sender, EventArgs e)
        {
            if (lbxEditors.Items.Count > 0)
            {
                lbxEditors.SelectedIndex = 0;
            }
        }
    }
}
