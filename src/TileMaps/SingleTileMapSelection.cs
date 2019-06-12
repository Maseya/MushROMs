// <copyright file="SingleTileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class SingleTileMapSelection : TileMapSelection
    {
        public SingleTileMapSelection(Point gridTile, int gridWidth)
            : base(gridTile, gridWidth)
        {
        }

        public override int MaxIndex
        {
            get
            {
                return MinIndex;
            }
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        public override Point this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return Origin;
            }
        }

        public override bool Contains(Point gridTile)
        {
            return gridTile == Origin;
        }

        public override TileMapSelection Move(Point gridTile, int gridWidth)
        {
            return new SingleTileMapSelection(gridTile, gridWidth);
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            yield return Origin;
        }
    }
}
