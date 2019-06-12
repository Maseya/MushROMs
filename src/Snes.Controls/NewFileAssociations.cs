// <copyright file="NewFileAssociations.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Maseya.Controls.Editors;

    public partial class NewFileAssociations : Component
    {
        public NewFileAssociations()
        {
            InitializeComponent();
        }

        public NewFileAssociations(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public List<NewFileIconAssociation> GetAssociations()
        {
            return new List<NewFileIconAssociation>()
            {
                paletteAssociation,
                gfxAssociation,
                chrAssociation,
                objAssociation,
                obj16Association,
            };
        }
    }
}
