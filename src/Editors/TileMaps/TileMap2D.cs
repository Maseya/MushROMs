// <copyright file="TileMap2D.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Drawing;
    using static System.Math;

    /// <summary>
    /// Represents a two-dimensional array as a tilemap.
    /// </summary>
    public class TileMap2D : TileMap, ITileMap2D
    {
        private Size gridSize;
        private Point zeroTile;
        private Point activeGridTile;

        /// <summary>
        /// Gets or sets the size of the data grid.
        /// </summary>
        public Size GridSize
        {
            get
            {
                return gridSize;
            }

            set
            {
                SetGridWidthInternal(value.Width);
                SetGridHeightInternal(value.Height);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the width of the data grid.
        /// </summary>
        public int GridWidth
        {
            get
            {
                return GridSize.Width;
            }

            set
            {
                SetGridWidthInternal(value);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the height of the data grid.
        /// </summary>
        public int GridHeight
        {
            get
            {
                return GridSize.Height;
            }

            set
            {
                SetGridHeightInternal(value);
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the location of the first data grid cell in the view
        /// area.
        /// </summary>
        public Point ZeroTile
        {
            get
            {
                return zeroTile;
            }

            set
            {
                if (ZeroTile == value)
                {
                    return;
                }

                zeroTile = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the first data grid cell in the
        /// view area.
        /// </summary>
        public int ZeroTileX
        {
            get
            {
                return ZeroTile.X;
            }

            set
            {
                if (ZeroTileX == value)
                {
                    return;
                }

                zeroTile.X = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the first data grid cell in the
        /// view area.
        /// </summary>
        public int ZeroTileY
        {
            get
            {
                return ZeroTile.Y;
            }

            set
            {
                if (ZeroTileY == value)
                {
                    return;
                }

                zeroTile.Y = value;
                OnZeroTileChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the location of the data grid cell the user is on.
        /// </summary>
        public Point ActiveGridTile
        {
            get
            {
                return activeGridTile;
            }

            set
            {
                if (ActiveGridTile == value)
                {
                    return;
                }

                activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the view location (in cell coordinates) of the data
        /// grid cell the user is on.
        /// </summary>
        public override Point ActiveViewTile
        {
            get
            {
                return GetViewTile(ActiveGridTile);
            }

            set
            {
                ActiveGridTile = GetGridTile(value);
            }
        }

        /// <summary>
        /// Gets the number of data grid cells present in the view area.
        /// </summary>
        public Size ViewableTiles
        {
            get
            {
                return new Size(
                    Min(Max(GridWidth - ZeroTileX, 0), ViewWidth),
                    Min(Max(GridHeight - ZeroTileY, 0), ViewHeight));
            }
        }

        /// <summary>
        /// Gets the data-grid X-coordinate of a view-area cell.
        /// </summary>
        /// <param name="viewTileX">
        /// The X-coordinate of the view-area cell.
        /// </param>
        /// <param name="zeroTileX">
        /// The X-coordinate of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The X-coordinate (in grid space) of <paramref name=" viewTileX"/>.
        /// </returns>
        public static int GetGridTileX(int viewTileX, int zeroTileX)
        {
            return viewTileX + zeroTileX;
        }

        /// <summary>
        /// Gets the data-grid Y-coordinate of a view-area cell.
        /// </summary>
        /// <param name="viewTileY">
        /// The Y-coordinate of the view-area cell.
        /// </param>
        /// <param name="zeroTileY">
        /// The Y-coordinate of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in grid space) of <paramref name=" viewTileY"/>.
        /// </returns>
        public static int GetGridTileY(int viewTileY, int zeroTileY)
        {
            return viewTileY + zeroTileY;
        }

        /// <summary>
        /// Gets the data-grid location of a view-area cell.
        /// </summary>
        /// <param name="viewTile">
        /// The location of the view-area cell.
        /// </param>
        /// <param name="zeroTile">
        /// The location of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in grid space) of <paramref name=" viewTile"/>.
        /// </returns>
        public static Point GetGridTile(Point viewTile, Point zeroTile)
        {
            var x = GetGridTileX(viewTile.X, zeroTile.X);
            var y = GetGridTileY(viewTile.Y, zeroTile.Y);
            return new Point(x, y);
        }

        /// <summary>
        /// Gets the view-area X-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTileX">
        /// The X-coordinate of the data grid cell.
        /// </param>
        /// <param name="zeroTileX">
        /// The X-coordinate of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The X-coordinate (in view space) of <paramref name=" gridTileX"/>.
        /// </returns>
        public static int GetViewTileX(int gridTileX, int zeroTileX)
        {
            return gridTileX - zeroTileX;
        }

        /// <summary>
        /// Gets the view-area Y-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTileY">
        /// The Y-coordinate of the data grid cell.
        /// </param>
        /// <param name="zeroTileY">
        /// The Y-coordinate of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in view space) of <paramref name=" gridTileY"/>.
        /// </returns>
        public static int GetViewTileY(int gridTileY, int zeroTileY)
        {
            return gridTileY - zeroTileY;
        }

        /// <summary>
        /// Gets the view-area location of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The location of the data grid cell.
        /// </param>
        /// <param name="zeroTile">
        /// The location of the first data grid cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in view space) of <paramref name=" gridTile"/>.
        /// </returns>
        public static Point GetViewTile(Point gridTile, Point zeroTile)
        {
            return new Point(
                GetViewTileX(gridTile.X, zeroTile.X),
                GetViewTileY(gridTile.Y, zeroTile.Y));
        }

        /// <summary>
        /// Gets a value that determines whether a data grid cell location is
        /// within <see cref="GridSize"/>.
        /// </summary>
        /// <param name="tile">
        /// The location of the data grid cell.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="tile"/> is inside of a
        /// rectangle starting at the origin and with a size of <see
        /// cref="GridSize"/>; otherwise <see langword="false"/>.
        /// </returns>
        public bool TileIsInGrid(Point tile)
        {
            var rectangle = new Rectangle(Point.Empty, GridSize);
            return rectangle.Contains(tile);
        }

        /// <summary>
        /// Gets the data-grid X-coordinate of a view-area cell.
        /// </summary>
        /// <param name="viewTileX">
        /// The X-coordinate of the view-area cell.
        /// </param>
        /// <returns>
        /// The X-coordinate (in grid space) of <paramref name=" viewTileX"/>.
        /// </returns>
        public int GetGridTileX(int viewTileX)
        {
            return GetGridTileX(viewTileX, ZeroTileX);
        }

        /// <summary>
        /// Gets the data-grid Y-coordinate of a view-area cell.
        /// </summary>
        /// <param name="viewTileY">
        /// The Y-coordinate of the view-area cell.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in grid space) of <paramref name=" viewTileY"/>.
        /// </returns>
        public int GetGridTileY(int viewTileY)
        {
            return GetGridTileY(viewTileY, ZeroTileY);
        }

        /// <summary>
        /// Gets the data-grid location of a view-area cell.
        /// </summary>
        /// <param name="viewTile">
        /// The location of the view-area cell.
        /// </param>
        /// <returns>
        /// The location (in grid space) of <paramref name=" viewTile"/>.
        /// </returns>
        public Point GetGridTile(Point viewTile)
        {
            return GetGridTile(viewTile, ZeroTile);
        }

        /// <summary>
        /// Gets the view-area X-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTileX">
        /// The X-coordinate of the data grid cell.
        /// </param>
        /// <returns>
        /// The X-coordinate (in view space) of <paramref name=" gridTileX"/>.
        /// </returns>
        public int GetViewTileX(int gridTileX)
        {
            return GetViewTileX(gridTileX, ZeroTileX);
        }

        /// <summary>
        /// Gets the view-area Y-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTileY">
        /// The Y-coordinate of the data grid cell.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in view space) of <paramref name=" gridTileY"/>.
        /// </returns>
        public int GetViewTileY(int gridTileY)
        {
            return GetViewTileY(gridTileY, ZeroTileY);
        }

        /// <summary>
        /// Gets the view-area location of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The location of the data grid cell.
        /// </param>
        /// <returns>
        /// The location (in view space) of <paramref name=" gridTile"/>.
        /// </returns>
        public Point GetViewTile(Point gridTile)
        {
            return GetViewTile(gridTile, ZeroTile);
        }

        /// <summary>
        /// Sets the <see cref="Size.Width"/> property of <see cref="
        /// gridSize"/> without raising <see cref="TileMap. GridSizeChanged"/>
        /// event.
        /// </summary>
        /// <param name="value">
        /// The new grid width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetGridWidthInternal(int value)
        {
            if (GridWidth == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            gridSize.Width = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Height"/> property of <see cref="
        /// gridSize"/> without raising <see cref="TileMap. GridSizeChanged"/>
        /// event.
        /// </summary>
        /// <param name="value">
        /// The new grid height.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetGridHeightInternal(int value)
        {
            if (GridHeight == value)
            {
                return;
            }

            AssertGreaterThanZero(value);
            gridSize.Height = value;
        }
    }
}
