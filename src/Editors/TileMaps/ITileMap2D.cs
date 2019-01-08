// <copyright file="ITileMap2D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System.Drawing;

    /// <summary>
    /// Defines methods and properties that represent a two-dimensional array
    /// of data as a tilemap.
    /// </summary>
    public interface ITileMap2D : ITileMap
    {
        /// <summary>
        /// Gets or sets the size of the data grid.
        /// </summary>
        Size GridSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the first data grid cell in the view
        /// area.
        /// </summary>
        Point ZeroTile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the data grid cell the user is on.
        /// </summary>
        Point ActiveGridTile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the size of the data grid cells shown in the view area.
        /// </summary>
        Size ViewableTiles
        {
            get;
        }
    }
}
