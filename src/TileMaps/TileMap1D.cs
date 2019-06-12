// <copyright file="TileMap1D.cs" company="Public Domain">
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
    using static Helper.ThrowHelper;

    /// <summary>
    /// Represents a one-dimensional array of data as a tilemap.
    /// </summary>
    public class TileMap1D : TileMap
    {
        /// <summary>
        /// The length of the data grid.
        /// </summary>
        private int _gridLength;

        public TileMap1D()
            : base()
        {
        }

        public TileMap1D(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Occurs when <see cref="GridLength"/> changes.
        /// </summary>
        public event EventHandler GridLengthChanged;

        /// <summary>
        /// Gets or sets the length of the data grid.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than zero.
        /// </exception>
        public int GridLength
        {
            get
            {
                return _gridLength;
            }

            set
            {
                if (GridLength == value)
                {
                    return;
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _gridLength = value;
                OnGridLengthChanged(EventArgs.Empty);
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
        public int OriginIndex
        {
            get
            {
                return GetTileIndex(Origin, ViewWidth, 0);
            }

            set
            {
                Origin = GetTile(value, ViewWidth, 0);
            }
        }

        public override int VisibleGridTileCount
        {
            get
            {
                return Min(Max(GridLength - OriginIndex, 0), ViewArea);
            }
        }

        public override Size GridSize
        {
            get
            {
                return new Size(
                    ViewWidth,
                    GridLength > 0 ? ((GridLength - 1) / ViewWidth) + 1 : 0);
            }

            set
            {
                GridLength = GetViewTileIndex((Point)value);
            }
        }

        /// <summary>
        /// Get the view-area location of a data grid cell.
        /// </summary>
        /// <param name="gridTileIndex">
        /// The index of the data grid cell to get the view-area location of.
        /// </param>
        /// <param name="gridWidth">
        /// The width of the view area.
        /// </param>
        /// <param name="originIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The location (in cell coordinates) in the view area of <paramref
        /// name="gridTileIndex"/>.
        /// </returns>
        public static Point GetTile(
            int gridTileIndex,
            int gridWidth,
            int originIndex = 0)
        {
            if (gridWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }

            return new Point(
                (gridTileIndex - originIndex) % gridWidth,
                (gridTileIndex - originIndex) / gridWidth);
        }

        /// <summary>
        /// Gets the data grid index of a cell in the view area.
        /// </summary>
        /// <param name="tile">
        /// The view-area location.
        /// </param>
        /// <param name="gridWidth">
        /// The width of the grid.
        /// </param>
        /// <param name="originIndex">
        /// The index of the first data cell in the view area.
        /// </param>
        /// <returns>
        /// The data grid index of <paramref name="tile"/>.
        /// </returns>
        public static int GetTileIndex(
            Point tile,
            int gridWidth,
            int originIndex = 0)
        {
            if (gridWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridWidth));
            }

            return (tile.Y * gridWidth) + tile.X + originIndex;
        }

        /// <summary>
        /// Gets the location (in cell coordinates) of a data grid cell in the
        /// view area.
        /// </summary>
        /// <param name="viewTileIndex">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// The location (in cell coordinates) of <paramref name="
        /// viewTileIndex"/>.
        /// </returns>
        public Point GetViewTile(int viewTileIndex)
        {
            return GetTile(viewTileIndex, ViewWidth, OriginIndex);
        }

        public Point GetGridTile(int gridTileIndex)
        {
            return GetTile(gridTileIndex, ViewWidth, 0);
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
        public int GetViewTileIndex(Point viewTile)
        {
            return GetTileIndex(viewTile, ViewWidth, OriginIndex);
        }

        public int GetGridTileIndex(Point gridTile)
        {
            return GetTileIndex(gridTile, ViewWidth);
        }

        /// <summary>
        /// Gets a value that determines whether a data grid cell index is
        /// within <see cref="GridLength"/>.
        /// </summary>
        /// <param name="tileIndex">
        /// The index of the data grid cell.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="tileIndex"/> is greater
        /// than or equal to zero and less than <see cref="GridLength"/>;
        /// otherwise <see langword="false"/>.
        /// </returns>
        public bool GridTileIsInGrid(int tileIndex)
        {
            return tileIndex >= 0 && tileIndex < GridLength;
        }

        public override bool GridContainsViewTile(Point viewTile)
        {
            return GridTileIsInGrid(GetViewTileIndex(viewTile));
        }

        public override bool GridContainsGridTile(Point gridTile)
        {
            return GridTileIsInGrid(GetGridTileIndex(gridTile));
        }

        public override IEnumerable<Point> GetGridTilesInView()
        {
            for (var tileIndex = 0; tileIndex < ViewArea; tileIndex++)
            {
                yield return GetGridTile(tileIndex + OriginIndex);
            }
        }

        public override IEnumerable<Point> GetVisibleGridTiles()
        {
            for (var tileIndex = 0; tileIndex < VisibleGridTileCount; tileIndex++)
            {
                yield return GetGridTile(tileIndex + OriginIndex);
            }
        }

        public override IEnumerable<Point> GetGridTiles()
        {
            for (var tileIndex = 0; tileIndex < GridLength; tileIndex++)
            {
                yield return GetGridTile(tileIndex);
            }
        }

        protected virtual void OnGridLengthChanged(EventArgs e)
        {
            GridLengthChanged?.Invoke(this, e);
            OnGridSizeChanged(EventArgs.Empty);
        }
    }
}
