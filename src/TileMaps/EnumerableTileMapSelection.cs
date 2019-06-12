// <copyright file="EnumerableTileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class EnumerableTileMapSelection : TileMapSelection
    {
        public EnumerableTileMapSelection(
            IEnumerable<Point> collection,
            int gridWidth)
            : base(GetFirst(collection), gridWidth)
        {
            List = new List<Point>(collection);
            HashSet = new HashSet<Point>(List);
            List.Sort(Compare);

            MaxIndex = TileMap1D.GetTileIndex(
                List.LastOrDefault(),
                GridWidth);

            int Compare(Point left, Point right)
            {
                return left.X != right.X ? left.X - right.X : left.Y - right.Y;
            }
        }

        public override int MaxIndex
        {
            get;
        }

        public override int Count
        {
            get
            {
                return List.Count;
            }
        }

        private List<Point> List
        {
            get;
        }

        private HashSet<Point> HashSet
        {
            get;
        }

        public override Point this[int index]
        {
            get
            {
                return List[index];
            }
        }

        public override bool Contains(Point gridTile)
        {
            return HashSet.Contains(gridTile);
        }

        public override TileMapSelection Move(Point gridTile, int gridWidth)
        {
            return new EnumerableTileMapSelection(
                List.Select(MovePoint),
                gridWidth);

            Point MovePoint(Point point)
            {
                return new Point(
                    point.X + gridTile.X - Origin.X,
                    point.Y + gridTile.Y - Origin.Y);
            }
        }

        public override IEnumerator<Point> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        private static Point GetFirst(IEnumerable<Point> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection.FirstOrDefault();
        }
    }
}
