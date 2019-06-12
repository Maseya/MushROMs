// <copyright file="BoxTileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    public class BoxTileMapSelection : TileMapSelection
    {
        public BoxTileMapSelection(Point origin, Size size, int gridWidth)
            : base(origin, gridWidth)
        {
            if (size.Width < 0 || size.Height < 0)
            {
                throw new ArgumentException();
            }

            Size = size;
            Count = Size.Width * Size.Height;
            MaxIndex = TileMap1D.GetTileIndex(
                Point.Add(origin, size),
                GridWidth);
        }

        public Size Size
        {
            get;
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(Origin, Size);
            }
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

                var point = TileMap1D.GetTile(index, Size.Width);
                return Point.Add(Origin, (Size)point);
            }
        }

        public override bool Contains(Point gridTile)
        {
            return Bounds.Contains(gridTile);
        }

        public override TileMapSelection Move(Point gridTile, int gridWidth)
        {
            return new BoxTileMapSelection(gridTile, Size, gridWidth);
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            for (var y = Bounds.Top; y < Bounds.Bottom; y++)
            {
                for (var x = Bounds.Left; x < Bounds.Right; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
