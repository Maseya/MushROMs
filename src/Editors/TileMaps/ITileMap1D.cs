// <copyright file="ITileMap1D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System.Drawing;

    /// <summary>
    /// Defines methods and properties that represent a one-dimensional array
    /// of data as a tilemap.
    /// </summary>
    public interface ITileMap1D : ITileMap
    {
        /// <summary>
        /// Gets or sets the length of the data grid.
        /// </summary>
        int GridSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets index of the first data grid cell in the view area.
        /// </summary>
        int ZeroTile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of data grid cells present in the view area.
        /// </summary>
        int ViewableTiles
        {
            get;
        }

        int GetGridTile(Point viewTile);

        Point GetViewTile(int gridTile);
    }
}
