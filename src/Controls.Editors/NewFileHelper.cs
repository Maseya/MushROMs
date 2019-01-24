// <copyright file="NewFileHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.Editors.IO;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class NewFileHelper : NewFileHelperBase
    {
        public NewFileHelper()
        {
            CreateEditorDialog = new CreateEditorDialog();
        }

        public NewFileHelper(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public IWin32Window Owner
        {
            get;
            set;
        }

        private CreateEditorDialog CreateEditorDialog
        {
            get;
        }

        public void AddEntry(
            Image fileIcon,
            string fileType,
            string description,
            Image previewImage,
            CreateEditorCallback createEditorCallback)
        {
            CreateEditorDialog.AddEntry(
                fileIcon,
                fileType,
                description,
                previewImage,
                createEditorCallback);
        }

        public void NewFile(IWin32Window owner)
        {
            Owner = owner;
            NewFile();
        }

        protected override CreateEditorCallback UISelectCreateEditorCallback()
        {
            return CreateEditorDialog.ShowDialog(Owner) == DialogResult.OK
                ? CreateEditorDialog.CreateEditorCallback
                : null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CreateEditorDialog.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
