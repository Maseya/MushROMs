// <copyright file="TileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class TileMapSelection : ITileMapSelection
    {
        protected TileMapSelection(Point origin, int gridWidth)
        {
            if (origin.X < 0 || origin.Y < 0)
            {
                throw new ArgumentException();
            }

            if (gridWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }

            Origin = origin;
            GridWidth = gridWidth;
            MinIndex = TileMap1D.GetTileIndex(Origin, GridWidth);
        }

        public Point Origin
        {
            get;
        }

        public int MinIndex
        {
            get;
        }

        public abstract int MaxIndex
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public int GridWidth
        {
            get;
        }

        public abstract Point this[int index]
        {
            get;
        }

        public abstract TileMapSelection Move(Point gridTile, int gridWidth);

        public TileMapSelection Copy()
        {
            return Move(Origin, GridWidth);
        }

        public abstract bool Contains(Point gridTile);

        public IEnumerable<T> EnumerateValues<T>(
            IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var gridTile in this)
            {
                var index = TileMap1D.GetTileIndex(gridTile, GridWidth);
                yield return list[index];
            }
        }

        public IEnumerable<(Point gridTile, T value)> EnumeratePointValues<T>(
            IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var gridTile in this)
            {
                var index = TileMap1D.GetTileIndex(gridTile, GridWidth);
                yield return (gridTile, list[index]);
            }
        }

        public Dictionary<Point, T> GetValueDictionary<T>(
            IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = new Dictionary<Point, T>(list.Count);
            foreach (var (point, value) in EnumeratePointValues(list))
            {
                result.Add(point, value);
            }

            return result;
        }

        public IEnumerable<(Point gridTile, int index)> EnumerateGridIndexes()
        {
            foreach (var gridTile in this)
            {
                var index = TileMap1D.GetTileIndex(gridTile, GridWidth);
                yield return (gridTile, index);
            }
        }

        public abstract IEnumerator<Point> GetEnumerator();

        ITileMapSelection ITileMapSelection.Move(Point gridTile, int gridWidth)
        {
            return Move(gridTile, gridWidth);
        }

        ITileMapSelection ITileMapSelection.Copy()
        {
            return Copy();
        }

        IDictionary<Point, T> ITileMapSelection.GetValueDictionary<T>(
            IReadOnlyList<T> list)
        {
            return GetValueDictionary(list);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
