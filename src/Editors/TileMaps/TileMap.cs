// <copyright file="TileMap.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Drawing;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Provides a base implementation of representing data as a
    /// tilemap.
    /// </summary>
    public abstract class TileMap : ITileMap
    {
        /// <summary>
        /// The default value to assign to <see cref="tileSize"/>.
        /// </summary>
        public static readonly Size FallbackTileSize = new Size(8, 8);

        /// <summary>
        /// The default value to assign to <see cref="zoomSize"/>.
        /// </summary>
        public static readonly Size FallbackZoomSize = new Size(2, 2);

        /// <summary>
        /// The default value to assign to <see cref="viewSize"/>.
        /// </summary>
        public static readonly Size FallbackViewSize = new Size(16, 8);

        /// <summary>
        /// The height and width, in pixels (before zoom scaling) of a
        /// tilemap cell.
        /// </summary>
        private Size tileSize;

        /// <summary>
        /// The horizontal and vertical zoom factors of a tilemap cell.
        /// </summary>
        private Size zoomSize;

        /// <summary>
        /// The height and width, in cell coordinates, of the tilemap
        /// view area.
        /// </summary>
        private Size viewSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/>
        /// class.
        /// </summary>
        protected TileMap()
        {
            tileSize = FallbackTileSize;
            zoomSize = FallbackZoomSize;
            viewSize = FallbackViewSize;
        }

        public event EventHandler GridSizeChanged;

        /// <summary>
        /// Occurs when the start reference tile changes.
        /// </summary>
        public event EventHandler ZeroTileChanged;

        /// <summary>
        /// Occurs when active tile changes.
        /// </summary>
        public event EventHandler ActiveGridTileChanged;

        /// <summary>
        /// Occurs when <see cref="TileSize"/> changes.
        /// </summary>
        public event EventHandler TileSizeChanged;

        /// <summary>
        /// Occurs when <see cref="ZoomSize"/> changes.
        /// </summary>
        public event EventHandler ZoomSizeChanged;

        /// <summary>
        /// Occurs when <see cref="ViewSize"/> changes.
        /// </summary>
        public event EventHandler ViewSizeChanged;

        /// <summary>
        /// Gets or sets the height and width, in pixels (before zoom
        /// scaling) of a tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public Size TileSize
        {
            get
            {
                return tileSize;
            }

            set
            {
                SetTileWidthInternal(value.Width);
                SetTileHeightInternal(value.Height);
                OnTileSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the width, in pixels (before zoom scaling) of a
        /// tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int TileWidth
        {
            get
            {
                return TileSize.Width;
            }

            set
            {
                if (value == TileWidth)
                {
                    return;
                }

                SetTileWidthInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the height, in pixels (before zoom scaling) of
        /// a tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int TileHeight
        {
            get
            {
                return TileSize.Height;
            }

            set
            {
                if (value == TileHeight)
                {
                    return;
                }

                SetTileHeightInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal and vertical zoom factors of a
        /// tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public Size ZoomSize
        {
            get
            {
                return zoomSize;
            }

            set
            {
                SetZoomWidthInternal(value.Width);
                SetZoomHeightInternal(value.Height);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal zoom factor a tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int ZoomWidth
        {
            get
            {
                return ZoomSize.Width;
            }

            set
            {
                if (value == ZoomWidth)
                {
                    return;
                }

                SetZoomWidthInternal(value);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the vertical zoom factor of a tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int ZoomHeight
        {
            get
            {
                return ZoomSize.Height;
            }

            set
            {
                if (value == ZoomHeight)
                {
                    return;
                }

                SetZoomHeightInternal(value);
                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the height and width, in cell coordinates, of
        /// the tilemap view area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public Size ViewSize
        {
            get
            {
                return viewSize;
            }

            set
            {
                SetViewWidthInternal(value.Width);
                SetViewHeightInternal(value.Height);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the width, in cell coordinates, of the tilemap
        /// view area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int ViewWidth
        {
            get
            {
                return ViewSize.Width;
            }

            set
            {
                if (value == ViewWidth)
                {
                    return;
                }

                SetViewWidthInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the height, in cell coordinates, of the tilemap
        /// view area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public int ViewHeight
        {
            get
            {
                return ViewSize.Height;
            }

            set
            {
                if (value == ViewHeight)
                {
                    return;
                }

                SetViewHeightInternal(value);
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the height and width, in pixels, of a tilemap cell.
        /// </summary>
        public Size CellSize
        {
            get
            {
                return new Size(CellWidth, CellHeight);
            }
        }

        /// <summary>
        /// Gets the height, in pixels, of a tilemap cell.
        /// </summary>
        public int CellWidth
        {
            get
            {
                return TileWidth * ZoomWidth;
            }
        }

        /// <summary>
        /// Gets the width, in pixels, of a tilemap cell.
        /// </summary>
        public int CellHeight
        {
            get
            {
                return TileHeight * ZoomHeight;
            }
        }

        /// <summary>
        /// Gets the height and width, in pixels, of the viewable
        /// tilemap area.
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
        }

        /// <summary>
        /// Gets the width, in pixels, of the viewable tilemap area.
        /// </summary>
        public int Width
        {
            get
            {
                return CellWidth * ViewWidth;
            }
        }

        /// <summary>
        /// Gets the height, in pixels, of the viewable tilemap area.
        /// </summary>
        public int Height
        {
            get
            {
                return CellHeight * ViewHeight;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the view
        /// location (in cell coordinates) of the data grid cell the
        /// user is on.
        /// </summary>
        public abstract Point ActiveViewTile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a tilemap cell coordinate from a pixel coordinate.
        /// </summary>
        /// <param name="pixel">
        /// The pixel coordinate to convert.
        /// </param>
        /// <param name="zoom">
        /// If <see langword="true"/>, the result will be scaled by
        /// <see cref="ZoomSize"/>; otherwise, no scaling occurs.
        /// </param>
        /// <returns>
        /// The tilemap cell coordinate of <paramref name="pixel"/>.
        /// </returns>
        public Point GetCellCoordinateFromPixel(Point pixel, bool zoom)
        {
            var size = zoom ? CellSize : TileSize;
            var x = pixel.X / size.Width;
            var y = pixel.Y / size.Height;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a pixel coordinate from a tilemap cell coordinate.
        /// </summary>
        /// <param name="tile">
        /// The tilemap cell coordinate to convert.
        /// </param>
        /// <param name="zoom">
        /// If <see langword="true"/>, the result will be scaled by
        /// <see cref="ZoomSize"/>; otherwise, no scaling occurs.
        /// </param>
        /// <returns>
        /// The pixel coordinate of <paramref name="tile"/>.
        /// </returns>
        public Point GetPixelFromViewTile(Point tile, bool zoom)
        {
            var size = zoom ? CellSize : TileSize;
            var x = tile.X * size.Width;
            var y = tile.Y * size.Height;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a value that determines whether a tilemap cell
        /// coordinate is in the view tilemap's area.
        /// </summary>
        /// <param name="viewTile">
        /// The tilemap cell coordinate to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="viewTile"/> is
        /// inside of <see cref="ViewSize"/>; otherwise <see langword="
        /// false"/>.
        /// </returns>
        public bool ViewTileIsInViewRegion(Point viewTile)
        {
            var rectangle = new Rectangle(Point.Empty, ViewSize);
            return rectangle.Contains(viewTile);
        }

        /// <summary>
        /// Raises the <see cref="TileSizeChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnTileSizeChanged(EventArgs e)
        {
            TileSizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ZoomSizeChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ViewSizeChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnViewSizeChanged(EventArgs e)
        {
            ViewSizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="GridSizeChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnGridSizeChanged(EventArgs e)
        {
            GridSizeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ZeroTileChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnZeroTileChanged(EventArgs e)
        {
            ZeroTileChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ActiveGridTileChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Throws an instance of <see cref="
        /// ArgumentOutOfRangeException"/> if <paramref name="value"/>
        /// is not greater than or equal to zero.
        /// </summary>
        /// <param name="value">
        /// The value to test.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than zero.
        /// </exception>
        private protected void AssertGreaterThanEqualToZero(int value)
        {
            if (value < 0)
            {
                throw ValueNotGreaterThan(
                    nameof(value),
                    value);
            }
        }

        /// <summary>
        /// Throws an instance of <see cref="
        /// ArgumentOutOfRangeException"/> if the passed parameter is
        /// not greater than zero.
        /// </summary>
        /// <param name="value">
        /// The value to test.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private protected void AssertGreaterThanZero(int value)
        {
            if (value <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(value),
                    value);
            }
        }

        /// <summary>
        /// Sets the <see cref="Size.Width"/> property of <see cref="
        /// tileSize"/> without raising the <see cref="
        /// TileSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetTileWidthInternal(int value)
        {
            AssertGreaterThanZero(value);
            tileSize.Width = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Height"/> property of <see cref="
        /// tileSize"/> without raising the <see cref="
        /// TileSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetTileHeightInternal(int value)
        {
            AssertGreaterThanZero(value);
            tileSize.Height = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Width"/> property of <see cref="
        /// zoomSize"/> without raising the <see cref="
        /// ZoomSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetZoomWidthInternal(int value)
        {
            AssertGreaterThanZero(value);
            zoomSize.Width = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Height"/> property of <see cref="
        /// zoomSize"/> without raising the <see cref="
        /// ZoomSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetZoomHeightInternal(int value)
        {
            AssertGreaterThanZero(value);
            zoomSize.Height = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Width"/> property of <see cref="
        /// viewSize"/> without raising the <see cref="
        /// ViewSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetViewWidthInternal(int value)
        {
            AssertGreaterThanZero(value);
            viewSize.Width = value;
        }

        /// <summary>
        /// Sets the <see cref="Size.Height"/> property of <see cref="
        /// zoomSize"/> without raising the <see cref="
        /// ViewSizeChanged"/> event.
        /// </summary>
        /// <param name="value">
        /// The new tile width.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is less than or equal to zero.
        /// </exception>
        private void SetViewHeightInternal(int value)
        {
            AssertGreaterThanZero(value);
            viewSize.Height = value;
        }
    }
}
