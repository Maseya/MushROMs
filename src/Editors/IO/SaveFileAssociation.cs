// <copyright file="SaveFileAssociation.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.IO
{
    using System;
    using System.ComponentModel;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class SaveFileAssociation : Component
    {
        public SaveFileAssociation()
        {
        }

        public SaveFileAssociation(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public string Extension
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string EditorClass
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public SaveEditorCallback SaveEditorCallback
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
