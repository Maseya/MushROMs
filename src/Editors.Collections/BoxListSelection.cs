// <copyright file="BoxListSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Maseya.TileMaps;

    public class BoxListSelection : ListSelection
    {
        public BoxListSelection(int index, Size size, int gridWidth)
        {
            if (size.Width < 0 || size.Height < 0)
            {
                throw new ArgumentException();
            }

            Size = size;
            MinIndex = index;
            Count = size.Width * size.Height;
            GridWidth = gridWidth;
        }

        public Size Size
        {
            get;
        }

        public override int Count
        {
            get;
        }

        public override int MinIndex
        {
            get;
        }

        public override int MaxIndex
        {
            get
            {
                return MinIndex + Count - 1;
            }
        }

        public int GridWidth
        {
            get;
        }

        public override int this[int index]
        {
            get
            {
                if (!ContainsIndex(index))
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return GetIndex(GetPoint(index));
            }
        }

        public override bool ContainsIndex(int index)
        {
            return new Rectangle(Point.Empty, Size).Contains(GetPoint(index));
        }

        public override ListSelection Move(int offset)
        {
            return new BoxListSelection(MinIndex + offset, Size, GridWidth);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                var index = GetIndex(new Point(0, y));
                for (var x = 0; x < Size.Width; x++)
                {
                    yield return index++;
                }
            }
        }

        private int GetIndex(Point point)
        {
            return TileMap1D.GetTileIndex(point, GridWidth, MinIndex);
        }

        private Point GetPoint(int index)
        {
            return TileMap1D.GetTile(index, GridWidth, MinIndex);
        }
    }
}
