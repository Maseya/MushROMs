// <copyright file="OpenFileAssociations.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Maseya.Editors;
    using Maseya.Editors.Collections;
    using Maseya.Editors.IO;
    using Maseya.Helper.PixelFormat;
    using Maseya.Snes;

    public partial class OpenFileAssociations : Component
    {
        public OpenFileAssociations()
        {
            InitializeComponent();
            rpfFileAssociation.OpenEditorCallback = GetCallback<Color15BppBgr>(
                PaletteFormatter.Rpf);

            // MW3 files are exactly like RPF files
            mw3FileAssociation.OpenEditorCallback = GetCallback<Color15BppBgr>(
                PaletteFormatter.Mw3);

            tplFileAssociation.OpenEditorCallback = GetCallback<Color15BppBgr>(
                PaletteFormatter.Tpl);

            palFileAssociation.OpenEditorCallback = GetCallback<Color15BppBgr>(
                PaletteFormatter.Pal);

            gfxFileAssociation.OpenEditorCallback = GfxEditor.OpenByteFile(
                GraphicsFormat.Format4BppSnes);

            OpenEditorCallback GetCallback<T>(IDataFormatter<T> dataFormatter)
            {
                return ListEditor<T>.OpenByteFile(
                    dataFormatter.ToFormattedData);
            }
        }

        public OpenFileAssociations(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }
        }

        public List<OpenFileAssociation> GetAssociations()
        {
            return new List<OpenFileAssociation>()
            {
                rpfFileAssociation,
                mw3FileAssociation,
                tplFileAssociation,
                palFileAssociation,
                gfxFileAssociation,
                chrFileAssociation,
                objFileAssociation,
                obj16FileAssociation,
            };
        }
    }
}
