// <copyright file="TileMap.cs" company="Public Domain">
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
    using static Helper.ThrowHelper;

    /// <summary>
    /// Provides a base implementation of representing data as a tilemap.
    /// </summary>
    public abstract class TileMap : Component, ITileMap
    {
        /// <summary>
        /// The default value to assign to <see cref="tileSize"/>.
        /// </summary>
        public static readonly Size FallbackTileSize = new Size(8, 8);

        /// <summary>
        /// The default value to assign to <see cref="viewSize"/>.
        /// </summary>
        public static readonly Size FallbackViewSize = new Size(16, 8);

        /// <summary>
        /// The height and width, in pixels (before zoom scaling) of a tilemap
        /// cell.
        /// </summary>
        private Size tileSize;

        /// <summary>
        /// The height and width, in cell coordinates, of the tilemap view
        /// area.
        /// </summary>
        private Size viewSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        protected TileMap()
        {
            tileSize = FallbackTileSize;
            viewSize = FallbackViewSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        /// <param name="container">
        /// The <see cref="IContainer"/> to add this <see cref="TileMap"/> to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container"/> is <see langword="null"/>.
        /// </exception>
        protected TileMap(IContainer container = null)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler GridSizeChanged;

        /// <summary>
        /// Occurs when the start reference tile changes.
        /// </summary>
        public event EventHandler ZeroTileChanged;

        /// <summary>
        /// Occurs when <see cref="TileSize"/> changes.
        /// </summary>
        public event EventHandler TileSizeChanged;

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
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
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
        /// Gets or sets the height, in pixels (before zoom scaling) of a
        /// tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
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
        /// Gets or sets the height and width, in cell coordinates, of the
        /// tilemap view area.
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
        /// Gets or sets the width, in cell coordinates, of the tilemap view
        /// area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
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
        /// Gets or sets the height, in cell coordinates, of the tilemap view
        /// area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
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
        /// Gets the width, in pixels, of the viewable tilemap area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int Width
        {
            get
            {
                return TileWidth * ViewWidth;
            }
        }

        /// <summary>
        /// Gets the height, in pixels, of the viewable tilemap area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int Height
        {
            get
            {
                return TileHeight * ViewHeight;
            }
        }

        /// <summary>
        /// Gets the height and width, in pixels, of the viewable tilemap area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
        }

        /// <summary>
        /// Gets a tilemap cell coordinate from a pixel coordinate.
        /// </summary>
        /// <param name="pixel">
        /// The pixel coordinate to convert.
        /// </param>
        /// <returns>
        /// The tilemap cell coordinate of <paramref name="pixel"/>.
        /// </returns>
        public Point GetViewTileFromPixel(Point pixel)
        {
            var x = pixel.X / TileWidth;
            var y = pixel.Y / TileHeight;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a pixel coordinate from a tilemap cell coordinate.
        /// </summary>
        /// <param name="tile">
        /// The tilemap cell coordinate to convert.
        /// </param>
        /// <returns>
        /// The pixel coordinate of <paramref name="tile"/>.
        /// </returns>
        public Point GetPixelFromViewTile(Point tile)
        {
            var x = tile.X * TileWidth;
            var y = tile.Y * TileHeight;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a value that determines whether a tilemap cell coordinate is
        /// in the view tilemap's area.
        /// </summary>
        /// <param name="viewTile">
        /// The tilemap cell coordinate to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="viewTile"/> is inside of
        /// <see cref="ViewSize"/>; otherwise <see langword=" false"/>.
        /// </returns>
        public bool ViewTileIsInViewRegion(Point viewTile)
        {
            var rectangle = new Rectangle(Point.Empty, ViewSize);
            return rectangle.Contains(viewTile);
        }

        public abstract bool ViewTileIsInGrid(Point point);

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
        /// Throws an instance of <see cref=" ArgumentOutOfRangeException"/> if
        /// <paramref name="value"/> is not greater than or equal to zero.
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
        /// Throws an instance of <see cref=" ArgumentOutOfRangeException"/> if
        /// the passed parameter is not greater than zero.
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
        /// tileSize"/> without raising the <see cref=" TileSizeChanged"/>
        /// event.
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
        /// tileSize"/> without raising the <see cref=" TileSizeChanged"/>
        /// event.
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
        /// viewSize"/> without raising the <see cref=" ViewSizeChanged"/>
        /// event.
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
        /// viewSize"/> without raising the <see cref=" ViewSizeChanged"/>
        /// event.
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
