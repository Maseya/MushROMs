// <copyright file="ITileMapSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System.Collections.Generic;
    using System.Drawing;

    public interface ITileMapSelection : IReadOnlyList<Point>
    {
        Point Origin
        {
            get;
        }

        int MinIndex
        {
            get;
        }

        int MaxIndex
        {
            get;
        }

        int GridWidth
        {
            get;
        }

        bool Contains(Point gridTile);

        ITileMapSelection Copy();

        ITileMapSelection Move(Point gridTile, int gridWidth);

        IEnumerable<T> EnumerateValues<T>(IReadOnlyList<T> list);

        IEnumerable<(Point gridTile, T value)> EnumeratePointValues<T>(
            IReadOnlyList<T> list);

        IEnumerable<(Point gridTile, int index)> EnumerateGridIndexes();

        IDictionary<Point, T> GetValueDictionary<T>(IReadOnlyList<T> list);
    }
}
