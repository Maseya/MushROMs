// <copyright file="NewFileIconAssociation.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.Editors
{
    using System.ComponentModel;
    using System.Drawing;
    using Maseya.Editors.IO;

    public class NewFileIconAssociation : NewFileAssociation
    {
        public NewFileIconAssociation()
            : base()
        {
        }

        public NewFileIconAssociation(IContainer container)
            : base(container)
        {
        }

        public Image FileIcon
        {
            get;
            set;
        }

        public Image PreviewImage
        {
            get;
            set;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FileIcon?.Dispose();
                PreviewImage?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
