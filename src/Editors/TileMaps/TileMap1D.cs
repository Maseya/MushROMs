// <copyright file="TileMap1D.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using static System.Math;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Represents a one-dimensional array of data as a tilemap.
    /// </summary>
    public class TileMap1D : TileMap, ITileMap1D
    {
        /// <summary>
        /// The length of the data grid.
        /// </summary>
        private int gridSize;

        /// <summary>
        /// The index of the first data cell in the view area.
        /// </summary>
        private int zeroTile;

        public TileMap1D()
            : base()
        {
        }

        public TileMap1D(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets or sets the length of the data grid.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A value less than zero is set.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int GridSize
        {
            get
            {
                return gridSize;
            }

            set
            {
                if (GridSize == value)
                {
                    return;
                }

                AssertGreaterThanEqualToZero(value);
                gridSize = value;
                OnGridSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets index of the first data grid cell in the view area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A value less than zero is set.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ZeroTile
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
        /// Gets the number of data grid cells present in the view area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ViewableTiles
        {
            get
            {
                return Min(
                    Max(GridSize - ZeroTile, 0),
                    ViewHeight * ViewWidth);
            }
        }

        /// <summary>
        /// Get the view-area X-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell to get the view-area X-coordinate
        /// of.
        /// </param>
        /// <param name="viewWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="zeroIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The X-coordinate (in cell coordinates) in the view area of
        /// <paramref name="gridTile"/>.
        /// </returns>
        public static int GetViewTileX(
            int gridTile,
            int viewWidth,
            int zeroIndex = 0)
        {
            AssertViewWidth(viewWidth);
            return (gridTile - zeroIndex) % viewWidth;
        }

        /// <summary>
        /// Get the view-area Y-coordinate of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell to get the view-area Y-coordinate
        /// of.
        /// </param>
        /// <param name="viewWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="zeroIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in cell coordinates) in the view area of
        /// <paramref name="gridTile"/>.
        /// </returns>
        public static int GetViewTileY(
            int gridTile,
            int viewWidth,
            int zeroIndex = 0)
        {
            AssertViewWidth(viewWidth);
            return (gridTile - zeroIndex) / viewWidth;
        }

        /// <summary>
        /// Get the view-area location of a data grid cell.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell to get the view-area location of.
        /// </param>
        /// <param name="viewWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="zeroIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in cell coordinates) in the view area of <paramref
        /// name="gridTile"/>.
        /// </returns>
        public static Point GetViewTile(
            int gridTile,
            int viewWidth,
            int zeroIndex = 0)
        {
            var x = GetViewTileX(gridTile, viewWidth, zeroIndex);
            var y = GetViewTileY(gridTile, viewWidth, zeroIndex);
            return new Point(x, y);
        }

        /// <summary>
        /// Gets the data grid index of a cell in the view area.
        /// </summary>
        /// <param name="viewTile">
        /// The view-area location.
        /// </param>
        /// <param name="viewWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="zeroIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The data grid index of <paramref name="viewTile"/>.
        /// </returns>
        public static int GetGridTile(
            Point viewTile,
            int viewWidth,
            int zeroIndex = 0)
        {
            return GetGridTile(
                viewTile.X,
                viewTile.Y,
                viewWidth,
                zeroIndex);
        }

        /// <summary>
        /// Gets the data grid index of a cell in the view area.
        /// </summary>
        /// <param name="viewTileX">
        /// The view-area X-coordinate.
        /// </param>
        /// <param name="viewTileY">
        /// The view-area Y-coordinate.
        /// </param>
        /// <param name="viewWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="zeroIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The data grid index of the location described by <paramref
        /// name="viewTileX"/> and <paramref name="viewTileY"/>.
        /// </returns>
        public static int GetGridTile(
            int viewTileX,
            int viewTileY,
            int viewWidth,
            int zeroIndex = 0)
        {
            AssertViewWidth(viewWidth);
            return (viewTileY * viewWidth) + viewTileX + zeroIndex;
        }

        /// <summary>
        /// Gets a value that determines whether a data grid cell index is
        /// within <see cref="GridSize"/>.
        /// </summary>
        /// <param name="tile">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="tile"/> is greater than
        /// or equal to zero and less than <see cref="GridSize"/>; otherwise
        /// <see langword="false"/>.
        /// </returns>
        public bool TileIsInGrid(int tile)
        {
            return tile >= 0 && tile < GridSize;
        }

        /// <summary>
        /// Gets the X-coordinate (in cell coordinates) of a data grid cell in
        /// the view area.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// The X-coordinate (in cell coordinates) of <paramref name="
        /// gridTile"/>.
        /// </returns>
        public int GetViewTileX(int gridTile)
        {
            return GetViewTileX(gridTile, ViewWidth, ZeroTile);
        }

        /// <summary>
        /// Gets the Y-coordinate (in cell coordinates) of a data grid cell in
        /// the view area.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// The Y-coordinate (in cell coordinates) of <paramref name="
        /// gridTile"/>.
        /// </returns>
        public int GetViewTileY(int gridTile)
        {
            return GetViewTileY(gridTile, ViewWidth, ZeroTile);
        }

        /// <summary>
        /// Gets the location (in cell coordinates) of a data grid cell in the
        /// view area.
        /// </summary>
        /// <param name="gridTile">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// The location (in cell coordinates) of <paramref name=" gridTile"/>.
        /// </returns>
        public Point GetViewTile(int gridTile)
        {
            return GetViewTile(gridTile, ViewWidth, ZeroTile);
        }

        /// <summary>
        /// Gets the data grid index of a cell in the view area.
        /// </summary>
        /// <param name="viewTile">
        /// The view-area location.
        /// </param>
        /// <returns>
        /// The data grid index of <paramref name="viewTile"/>.
        /// </returns>
        public int GetGridTile(Point viewTile)
        {
            return GetGridTile(viewTile, ViewWidth, ZeroTile);
        }

        /// <summary>
        /// Gets the data grid index of a cell in the view area.
        /// </summary>
        /// <param name="viewTileX">
        /// The view-area X-coordinate.
        /// </param>
        /// <param name="viewTileY">
        /// The view-area Y-coordinate.
        /// </param>
        /// <returns>
        /// The data grid index of the location described by <paramref
        /// name="viewTileX"/> and <paramref name="viewTileY"/>.
        /// </returns>
        public int GetGridTile(int viewTileX, int viewTileY)
        {
            return GetGridTile(
                viewTileX,
                viewTileY,
                ViewWidth,
                ZeroTile);
        }

        public override bool ViewTileIsInGrid(Point viewTile)
        {
            return TileIsInGrid(GetGridTile(viewTile));
        }

        /// <summary>
        /// Throws an instance of <see cref=" ArgumentOutOfRangeException"/> if
        /// the passed parameter is not greater than zero.
        /// </summary>
        /// <param name="viewWidth">
        /// The value to test.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="viewWidth"/> is less than or equal to zero.
        /// </exception>
        private static void AssertViewWidth(int viewWidth)
        {
            if (viewWidth <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(viewWidth),
                    viewWidth);
            }
        }
    }
}
