// <copyright file="CreateEditorDialog.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.Editors.IO;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class CreateEditorDialog : DialogProxy
    {
        public CreateEditorDialog()
            : base()
        {
            CreateEditorForm = new CreateEditorForm();
        }

        public CreateEditorDialog(IContainer container)
            : base(container)
        {
            CreateEditorForm = new CreateEditorForm();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public CreateEditorCallback CreateEditorCallback
        {
            get
            {
                return CreateEditorForm.CreateEditorCallback;
            }
        }

        protected override Form BaseForm
        {
            get
            {
                return CreateEditorForm;
            }
        }

        private CreateEditorForm CreateEditorForm
        {
            get;
        }

        public void AddAssociation(
            Image fileIcon,
            string fileType,
            string description,
            Image previewImage,
            CreateEditorCallback createEditorCallback)
        {
            CreateEditorForm.AddAssociation(
                fileIcon,
                fileType,
                description,
                previewImage,
                createEditorCallback);
        }

        public void AddAssociation(NewFileIconAssociation association)
        {
            CreateEditorForm.AddAssociation(association);
        }
    }
}
