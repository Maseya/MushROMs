// <copyright file="OpenWithForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.Windows.Forms;
    using Maseya.Editors.IO;

    internal partial class OpenWithForm : Form
    {
        public OpenWithForm()
        {
            InitializeComponent();
        }

        public OpenEditorCallback OpenEditorCallback
        {
            get
            {
                var info = lbxEditors.SelectedItem as
                    OpenFileAssociation;

                return info.OpenEditorCallback;
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
                OpenEditorCallback openEditorCallback)
        {
            var association = new OpenFileAssociation(
                extension,
                description,
                editorClass,
                openEditorCallback);

            AddAssociation(association);
        }

        public void AddAssociation(OpenFileAssociation association)
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
