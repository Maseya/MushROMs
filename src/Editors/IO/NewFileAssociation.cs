// <copyright file="NewFileAssociation.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.IO
{
    using System;
    using System.ComponentModel;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class NewFileAssociation : Component
    {
        public NewFileAssociation()
        {
        }

        public NewFileAssociation(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public string FileType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public CreateEditorCallback CreateEditorCallback
        {
            get;
            set;
        }
    }
}
