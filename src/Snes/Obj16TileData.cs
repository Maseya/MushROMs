// <copyright file="Obj16TileData.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Editors;

    public class Obj16TileData : EditorData<Obj16Tile>
    {
        protected override IEnumerable<byte> GetByteData(Obj16Tile item)
        {
            return item.GetBytes();
        }

        public override int GetIndex(int offset, int startOffset)
        {
            return (startOffset - offset) * Obj16Tile.SizeOf;
        }

        protected override Obj16Tile GetItem(IEnumerable<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var list = new List<byte>(Obj16Tile.SizeOf);
            foreach (var item in data)
            {
                // Fail we want to move more than 2.
                if (list.Count == Obj16Tile.SizeOf)
                {
                    throw new ArgumentException();
                }

                list.Add(item);
            }

            // Fail if we do not have exactly two bytes.
            if (list.Count != Obj16Tile.SizeOf)
            {
                throw new ArgumentException();
            }

            var result = default(Obj16Tile);
            for (var i = 0; i < Obj16Tile.NumberOfTiles; i++)
            {
                result[i + 0] = list[(i >> 1) + 0] | (list[(i >> 1) + 1] << 8);
            }

            return result;
        }

        public override int GetOffset(int index, int startOffset)
        {
            return startOffset + (index * Obj16Tile.SizeOf);
        }

        public override int GetSizeOfItem(Obj16Tile item)
        {
            return Obj16Tile.SizeOf;
        }

        public override int GetSizeOfItemAtOffset(int offset)
        {
            return Obj16Tile.SizeOf;
        }
    }
}
