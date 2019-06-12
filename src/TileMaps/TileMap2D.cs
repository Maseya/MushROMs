// <copyright file="TileMap2D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using static System.Math;

    /// <summary>
    /// Represents a two-dimensional array as a tilemap.
    /// </summary>
    public class TileMap2D : TileMap
    {
        private Size _gridSize;

        /// <summary>
        /// Gets or sets the size of the data grid.
        /// </summary>
        public override Size GridSize
        {
            get
            {
                return _gridSize;
            }

            set
            {
                if (GridSize == value)
                {
                    return;
                }

                if (value.Width < 0 || value.Height < 0)
                {
                    throw new ArgumentException();
                }

                _gridSize = value;
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the number of data grid cells present in the view area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size ViewableTiles
        {
            get
            {
                return new Size(
                    Min(Max(GridWidth - OriginX, 0), ViewWidth),
                    Min(Max(GridHeight - OriginY, 0), ViewHeight));
            }
        }

        public override int VisibleGridTileCount
        {
            get
            {
                return ViewableTiles.Width * ViewableTiles.Height;
            }
        }

        /// <summary>
        /// Gets the data-grid location of a view-area cell.
        /// </summary>
        /// <param name="viewTile">
        /// The location of the view-area cell.
        /// </param>
        /// <param name="orgin">
        /// The location of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in grid space) of <paramref name=" viewTile"/>.
        /// </returns>
        public static Point GetGridTile(Point viewTile, Point orgin)
        {
            return new Point(
                viewTile.X + orgin.X,
                viewTile.Y + orgin.Y);
        }

        /// <summary>
        /// Gets the view-area location of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The location of the data grid cell.
        /// </param>
        /// <param name="origin">
        /// The location of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in view space) of <paramref name=" gridTile"/>.
        /// </returns>
        public static Point GetViewTile(Point gridTile, Point origin)
        {
            return new Point(
                gridTile.X - origin.X,
                gridTile.Y - origin.Y);
        }

        public override bool GridContainsViewTile(Point viewTile)
        {
            return GridContainsGridTile(GetGridTile(viewTile));
        }

        public override bool GridContainsGridTile(Point gridTile)
        {
            return new Rectangle(Point.Empty, GridSize).Contains(gridTile);
        }

        public override IEnumerable<Point> GetGridTilesInView()
        {
            return GetGridTilesFromView(ViewSize);
        }

        public override IEnumerable<Point> GetVisibleGridTiles()
        {
            return GetGridTilesFromView(ViewableTiles);
        }

        public override IEnumerable<Point> GetGridTiles()
        {
            for (var y = 0; y < GridHeight; y++)
            {
                for (var x = 0; x < GridWidth; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private IEnumerable<Point> GetGridTilesFromView(Size size)
        {
            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    yield return GetGridTile(new Point(x, y));
                }
            }
        }
    }
}
