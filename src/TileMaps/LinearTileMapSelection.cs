// <copyright file="LinearTileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class LinearTileMapSelection : TileMapSelection
    {
        public LinearTileMapSelection(
            Point startGridTile,
            int length,
            int gridWidth)
            : base(startGridTile, gridWidth)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Count = length;
            MaxIndex = MinIndex + Count - 1;
        }

        public override int MaxIndex
        {
            get;
        }

        public override int Count
        {
            get;
        }

        public override Point this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return TileMap1D.GetTile(index, GridWidth);
            }
        }

        public override bool Contains(Point gridTile)
        {
            var index = TileMap1D.GetTileIndex(gridTile, GridWidth);
            return index >= MinIndex && index <= MaxIndex;
        }

        public override TileMapSelection Move(Point gridTile, int gridWidth)
        {
            return new LinearTileMapSelection(gridTile, Count, gridWidth);
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            for (var gridIndex = MinIndex; gridIndex <= MaxIndex; gridIndex++)
            {
                yield return TileMap1D.GetTile(gridIndex, GridWidth);
            }
        }
    }
}
