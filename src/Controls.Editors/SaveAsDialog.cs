// <copyright file="SaveAsDialog.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using Maseya.Editors.IO;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class SaveAsDialog : DialogProxy
    {
        public SaveAsDialog()
            : base()
        {
            SaveAsForm = new SaveAsForm();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public SaveEditorCallback SaveEditorCallback
        {
            get
            {
                return SaveAsForm.SaveEditorCallback;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return SaveAsForm;
            }
        }

        private SaveAsForm SaveAsForm
        {
            get;
        }

        public void ClearEditors()
        {
            SaveAsForm.ClearEditors();
        }

        public void AddAssociation(
                string extension,
                string description,
                string editorClass,
                SaveEditorCallback saveEditorCallback)
        {
            SaveAsForm.AddAssociation(
                extension,
                description,
                editorClass,
                saveEditorCallback);
        }

        public void AddAssociation(SaveFileAssociation association)
        {
            SaveAsForm.AddAssociation(association);
        }
    }
}
