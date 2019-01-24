// <copyright file="OpenWithDialog.cs" company="Public Domain">
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

    public class OpenWithDialog : DialogProxy
    {
        public OpenWithDialog()
            : base()
        {
            OpenWithForm = new OpenWithForm();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public OpenEditorCallback OpenEditorCallback
        {
            get
            {
                return OpenWithForm.OpenEditorCallback;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return OpenWithForm;
            }
        }

        private OpenWithForm OpenWithForm
        {
            get;
        }

        public void ClearEditors()
        {
            OpenWithForm.ClearEditors();
        }

        public void AddAssociation(
                string extension,
                string description,
                string editorClass,
                OpenEditorCallback openEditorCallback)
        {
            OpenWithForm.AddAssociation(
                extension,
                description,
                editorClass,
                openEditorCallback);
        }
    }
}
