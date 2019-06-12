// <copyright file="TileMap.cs" company="Public Domain">
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

    /// <summary>
    /// Provides a base implementation of representing data as a tilemap.
    /// </summary>
    public abstract class TileMap : Component
    {
        /// <summary>
        /// The default value to assign to <see cref="_tileSize"/>.
        /// </summary>
        public static readonly Size FallbackTileSize = new Size(8, 8);

        /// <summary>
        /// The default value to assign to <see cref="_viewSize"/>.
        /// </summary>
        public static readonly Size FallbackViewSize = new Size(16, 8);

        private Point _origin;

        /// <summary>
        /// The height and width, in pixels (before zoom scaling) of a tilemap
        /// cell.
        /// </summary>
        private Size _tileSize;

        /// <summary>
        /// The height and width, in cell coordinates, of the tilemap view
        /// area.
        /// </summary>
        private Size _viewSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileMap"/> class.
        /// </summary>
        protected TileMap()
        {
            _tileSize = FallbackTileSize;
            _viewSize = FallbackViewSize;
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

        /// <summary>
        /// Occurs when <see cref="GridSize"/> changes.
        /// </summary>
        public event EventHandler GridSizeChanged;

        /// <summary>
        /// Occurs when <see cref="Origin"/> changes.
        /// </summary>
        public event EventHandler OriginChanged;

        /// <summary>
        /// Occurs when <see cref="TileSize"/> changes.
        /// </summary>
        public event EventHandler TileSizeChanged;

        /// <summary>
        /// Occurs when <see cref="ViewSize"/> changes.
        /// </summary>
        public event EventHandler ViewSizeChanged;

        public Point Origin
        {
            get
            {
                return _origin;
            }

            set
            {
                if (Origin == value)
                {
                    return;
                }

                _origin = value;
                OnOriginChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int OriginX
        {
            get
            {
                return Origin.X;
            }

            set
            {
                if (OriginX == value)
                {
                    return;
                }

                _origin.X = value;
                OnOriginChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int OriginY
        {
            get
            {
                return Origin.Y;
            }

            set
            {
                if (OriginY == value)
                {
                    return;
                }

                _origin.Y = value;
                OnOriginChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public abstract Size GridSize
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int GridWidth
        {
            get
            {
                return GridSize.Width;
            }

            set
            {
                GridSize = new Size(value, GridHeight);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int GridHeight
        {
            get
            {
                return GridSize.Height;
            }

            set
            {
                GridSize = new Size(GridWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the height and width, in pixels, of a tilemap cell.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public Size TileSize
        {
            get
            {
                return _tileSize;
            }

            set
            {
                if (value == TileSize)
                {
                    return;
                }

                if (value.Width <= 0 || value.Height <= 0)
                {
                    throw new ArgumentException();
                }

                _tileSize = value;
                OnTileSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of a tilemap cell.
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
                TileSize = new Size(value, TileHeight);
            }
        }

        /// <summary>
        /// Gets or sets the height, in pixels, of a tilemap cell.
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
                TileSize = new Size(TileWidth, value);
            }
        }

        /// <summary>
        /// Gets or sets the height and width, in tile coordinates, of the
        /// tilemap view area.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value set is less than or equal to zero.
        /// </exception>
        public Size ViewSize
        {
            get
            {
                return _viewSize;
            }

            set
            {
                if (ViewSize == value)
                {
                    return;
                }

                if (value.Width <= 0 || value.Height <= 0)
                {
                    throw new ArgumentException();
                }

                _viewSize = value;
                OnViewSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the width, in tile coordinates, of the tilemap view
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
                ViewSize = new Size(value, ViewHeight);
            }
        }

        /// <summary>
        /// Gets or sets the height, in tile coordinates, of the tilemap view
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
                ViewSize = new Size(ViewWidth, value);
            }
        }

        /// <summary>
        /// Gets the number of tiles in the view region.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ViewArea
        {
            get
            {
                return ViewHeight * ViewWidth;
            }
        }

        /// <summary>
        /// Gets the number of grid tiles in the view region.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public abstract int VisibleGridTileCount
        {
            get;
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
        /// Gets a tile coordinate from a pixel coordinate.
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
        /// Gets a pixel coordinate from a tile coordinate.
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

        public bool ViewAreaContainsPixel(Point pixel)
        {
            return pixel.X >= 0
                && pixel.Y >= 0
                && pixel.X < Width
                && pixel.Y < Height;
        }

        /// <summary>
        /// Gets a value that determines whether a tile coordinate is in the
        /// view area.
        /// </summary>
        /// <param name="viewTile">
        /// The tile coordinate to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="viewTile"/> is inside of
        /// <see cref="ViewSize"/>; otherwise <see langword=" false"/>.
        /// </returns>
        public bool ViewContainsViewTile(Point viewTile)
        {
            return new Rectangle(Point.Empty, ViewSize).Contains(viewTile);
        }

        public bool ViewContainsGridTile(Point gridTile)
        {
            return ViewContainsViewTile(GetViewTile(gridTile));
        }

        public abstract bool GridContainsViewTile(Point viewTile);

        public abstract bool GridContainsGridTile(Point gridTile);

        public Point GetViewTile(Point gridTile)
        {
            var width = gridTile.X - OriginX;
            return GridWidth > 0
                ? new Point(
                    width % GridWidth,
                    gridTile.Y - OriginY + (width / GridWidth))
                : new Point(
                    width,
                    gridTile.Y - OriginY);
        }

        public Point GetGridTile(Point viewTile)
        {
            return new Point(
                viewTile.X + Origin.X,
                viewTile.Y + Origin.Y);
        }

        public abstract IEnumerable<Point> GetGridTilesInView();

        public IEnumerable<Point> GetViewTiles()
        {
            for (var y = 0; y < ViewHeight; y++)
            {
                for (var x = 0; x < ViewWidth; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public abstract IEnumerable<Point> GetVisibleGridTiles();

        public abstract IEnumerable<Point> GetGridTiles();

        /// <summary>
        /// Raises the <see cref="OriginChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnOriginChanged(EventArgs e)
        {
            OriginChanged?.Invoke(this, e);
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
    }
}
