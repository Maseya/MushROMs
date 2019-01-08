// <copyright file="ChrTileData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Editors;

    public class ChrTileData : EditorData<ChrTile>
    {
        protected override IEnumerable<byte> GetByteData(ChrTile item)
        {
            yield return (byte)item.Value;
            yield return (byte)(item.Value >> 8);
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (startOffset - offset) * ChrTile.SizeOf;
        }

        protected override ChrTile GetItem(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var list = new List<byte>(ChrTile.SizeOf);
            foreach (var item in data)
            {
                // Fail we want to move more than 2.
                if (list.Count == ChrTile.SizeOf)
                {
                    throw new ArgumentException();
                }

                list.Add(item);
            }

            // Fail if we do not have exactly two bytes.
            if (list.Count != ChrTile.SizeOf)
            {
                throw new ArgumentException();
            }

            return list[0] | (list[1] << 8);
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * ChrTile.SizeOf);
        }

        public override int GetSizeOfItem(ChrTile item)
        {
            return ChrTile.SizeOf;
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return ChrTile.SizeOf;
        }
    }
}
