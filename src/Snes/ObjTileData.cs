// <copyright file="ObjTileData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Editors;

    public class ObjTileData : EditorData<ObjTile>
    {
        protected override IEnumerable<byte> GetByteData(ObjTile item)
        {
            yield return (byte)item.Value;
            yield return (byte)(item.Value >> 8);
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (startOffset - offset) * ObjTile.SizeOf;
        }

        protected override ObjTile GetItem(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var list = new List<byte>(ObjTile.SizeOf);
            foreach (var item in data)
            {
                // Fail we want to move more than 2.
                if (list.Count == ObjTile.SizeOf)
                {
                    throw new ArgumentException();
                }

                list.Add(item);
            }

            // Fail if we do not have exactly two bytes.
            if (list.Count != ObjTile.SizeOf)
            {
                throw new ArgumentException();
            }

            return list[0] | (list[1] << 8);
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * ObjTile.SizeOf);
        }

        public override int GetSizeOfItem(ObjTile item)
        {
            return ObjTile.SizeOf;
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return ObjTile.SizeOf;
        }
    }
}
