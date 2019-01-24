// <copyright file="ITileMap.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// Defines methods and properties that represent data as a tilemap grid.
    /// </summary>
    public interface ITileMap : IComponent
    {
        event EventHandler ViewSizeChanged;

        event EventHandler TileSizeChanged;

        event EventHandler GridSizeChanged;

        event EventHandler ZeroTileChanged;

        /// <summary>
        /// Gets or sets the height and width, in cell coordinates, of the
        /// tilemap of the tilemap view area.
        /// </summary>
        Size ViewSize
        {
            get;
            set;
        }

        Size TileSize
        {
            get;
            set;
        }

        Size Size
        {
            get;
        }

        bool ViewTileIsInGrid(Point viewTile);
    }
}
