// <copyright file="ChrTileEditor.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Maseya.Editors;
    using Maseya.Editors.Collections;

    public class ChrTileEditor : SharedListChildEditor<ChrTile>
    {
        public ChrTileEditor(SharedListParentEditor parent)
            : base(parent)
        {
        }

        private ChrTileEditor(ISharedListParent parent)
            : base(parent)
        {
        }

        public override IEnumerable<byte> GetByteData(ChrTile item)
        {
            yield return (byte)item.Value;
            yield return (byte)(item.Value >> 8);
        }

        public override ChrTile GetItem(IEnumerable<byte> data)
        {
            var bytes = new List<byte>(Enumerable.Take(data, ChrTile.SizeOf));

            return bytes[0] | (bytes[1] << 8);
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return ChrTile.SizeOf;
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (startOffset - offset) * ChrTile.SizeOf;
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * ChrTile.SizeOf);
        }

        public override int GetSizeOfItem(ChrTile item)
        {
            return ChrTile.SizeOf;
        }

        protected override ISharedListChild<ChrTile> Create(
                            ISharedListParent parent)
        {
            return new ChrTileEditor(parent)
            {
                StartOffset = StartOffset,
            };
        }
    }
}
