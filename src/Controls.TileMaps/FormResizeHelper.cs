// <copyright file="FormResizeHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.TileMaps;
    using static System.Math;
    using static ResizeMode;
    using static WinApiMethods;

    public class FormResizeHelper : Component
    {
        private static readonly Size DefaultMinimumTileSize = new Size(1, 1);

        private TileMap _tileMap;

        private DesignForm _designForm;

        private Size _minimumTileSize;

        private Size _maximumTileSize;

        public FormResizeHelper()
        {
        }

        public FormResizeHelper(IContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler TileMapChanged;

        public event EventHandler TileMapControlChanged;

        public event EventHandler DesignFormChanged;

        public TileMap TileMap
        {
            get
            {
                return _tileMap;
            }

            set
            {
                if (TileMap == value)
                {
                    return;
                }

                if (TileMap != null)
                {
                    TileMap.TileSizeChanged -= TileMap_TileSizeChanged;
                    TileMap.ViewSizeChanged -= TileMap_TileSizeChanged;
                }

                _tileMap = value;
                if (TileMap != null)
                {
                    TileMap.TileSizeChanged += TileMap_TileSizeChanged;
                    TileMap.ViewSizeChanged += TileMap_TileSizeChanged;
                }

                OnTileMapChanged(EventArgs.Empty);
            }
        }

        public DesignForm DesignForm
        {
            get
            {
                return _designForm;
            }

            set
            {
                if (DesignForm != null)
                {
                    DesignForm.SizeChanged -= DesignForm_SizeChanged;
                    DesignForm.AdjustWindowBounds -= Form_AdjustWindowBounds;
                    DesignForm.AdjustWindowSize += Form_AdjustWindowSize;
                }

                _designForm = value;
                if (DesignForm != null)
                {
                    DesignForm.SizeChanged += DesignForm_SizeChanged;
                    DesignForm.AdjustWindowBounds += Form_AdjustWindowBounds;
                    DesignForm.AdjustWindowSize += Form_AdjustWindowSize;
                }

                TileMapPadding = Padding.Empty;
                OnDesignFormChanged(EventArgs.Empty);
            }
        }

        public Size MinimumTileSize
        {
            get
            {
                return _minimumTileSize;
            }

            set
            {
                _minimumTileSize = new Size(
                    Max(value.Width, 0),
                    Max(value.Height, 0));
            }
        }

        public Size MaximumTileSize
        {
            get
            {
                return _maximumTileSize;
            }

            set
            {
                _maximumTileSize = new Size(
                    Max(value.Width, 0),
                    Max(value.Height, 0));
            }
        }

        private Padding TileMapPadding
        {
            get;
            set;
        }

        private ResizeMode TileMapResizeMode
        {
            get;
            set;
        }

        private bool TileMapResized
        {
            get;
            set;
        }

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            TileMapResized = false;
            SetTileMapPadding();
            TileMapChanged?.Invoke(this, e);
        }

        protected virtual void OnTileMapControlChanged(EventArgs e)
        {
            TileMapControlChanged?.Invoke(this, e);
        }

        protected virtual void OnDesignFormChanged(EventArgs e)
        {
            SetTileMapPadding();
            DesignFormChanged?.Invoke(this, e);
        }

        private void Form_AdjustWindowSize(object sender, SizeEventArgs e)
        {
            if (TileMapPadding == Padding.Empty)
            {
                return;
            }

            var tileMap = GetTileMapRectangle(
                new Rectangle(Point.Empty, e.Size)).Size;

            tileMap = GetBoundTileSize(tileMap);

            if (TileMapResizeMode != TileMapResize)
            {
                TileMap.ViewSize = (Size)TileMap.GetViewTileFromPixel(
                    (Point)tileMap);
            }

            e.Size = InflateSize(tileMap, TileMapPadding);
        }

        private void Form_AdjustWindowBounds(
            object sender,
            RectangleEventArgs e)
        {
            if (TileMapPadding == Padding.Empty)
            {
                return;
            }

            var tileMap = GetTileMapRectangle(e.Rectangle);
            tileMap.Size = GetBoundTileSize(tileMap.Size);

            if (TileMapResizeMode != TileMapResize)
            {
                TileMap.ViewSize = (Size)TileMap.GetViewTileFromPixel(
                    (Point)tileMap.Size);
            }

            e.Rectangle = InflateRectangle(tileMap, TileMapPadding);
        }

        private void SetTileMapPadding()
        {
            if (TileMap is null || DesignForm is null)
            {
                TileMapPadding = Padding.Empty;
                return;
            }

            var form = GetWindowRectangle(DesignForm);
            var tileMap = new Rectangle(Point.Empty, TileMap.Size);
            TileMapPadding = GetPadding(form, tileMap);
        }

        private void SetSizeFromTileMap()
        {
            if (TileMapResizeMode == ControlResize)
            {
                return;
            }

            if (TileMapResizeMode == None)
            {
                TileMapResizeMode = ControlResize;
            }

            var window = InflateSize(TileMap.Size, TileMapPadding);

            if (TileMapResizeMode == ControlResize)
            {
                TileMapResizeMode = None;
            }
        }

        private Size AdjustSize(Size window)
        {
            var tileMap = GetTileMapRectangle(window).Size;
            tileMap = GetBoundTileSize(tileMap);

            if (TileMapResizeMode != TileMapResize)
            {
                TileMap.ViewSize = new Size(
                    tileMap.Width / TileMap.TileSize.Width,
                    tileMap.Height / TileMap.TileSize.Height);
            }

            return InflateSize(tileMap, TileMapPadding);
        }

        private Rectangle GetTileMapRectangle(Size window)
        {
            return GetTileMapRectangle(new Rectangle(Point.Empty, window));
        }

        private Rectangle GetTileMapRectangle(Rectangle window)
        {
            if (window.Size == default)
            {
                return default;
            }

            var tileMap = DeflateRectangle(window, TileMapPadding);
            var client = new Rectangle(Point.Empty, TileMap.Size);

            var residualWidth = tileMap.Width % TileMap.TileSize.Width;
            var residualHeight = tileMap.Height % TileMap.TileSize.Height;

            var parent = GetWindowRectangle(DesignForm);

            tileMap.Width -= residualWidth;
            tileMap.Height -= residualHeight;

            // Left or top adjust the client if sizing on those borders.
            if (window.Left != parent.Left && window.Right == parent.Right)
            {
                if (tileMap.Width >= TileMap.TileSize.Width)
                {
                    tileMap.X += residualWidth;
                }
                else
                {
                    tileMap.X = client.X;
                }
            }

            if (window.Top != parent.Top && window.Bottom == parent.Bottom)
            {
                if (tileMap.Height >= TileMap.TileSize.Height)
                {
                    tileMap.Y += residualHeight;
                }
                else
                {
                    tileMap.Y = client.Y;
                }
            }

            // Ensure non-negative values.
            if (tileMap.Width <= 0)
            {
                tileMap.Width = TileMap.TileSize.Width;
            }

            if (tileMap.Height <= 0)
            {
                tileMap.Height = TileMap.TileSize.Height;
            }

            return tileMap;
        }

        private Size GetBoundTileSize(Size window)
        {
            var cellW = TileMap.TileSize.Width;
            var cellH = TileMap.TileSize.Height;

            // Defines the possible minimum and maximum tile sizes.
            var min = new List<Size>();
            var max = new List<Size>();

            // Min/Max tile size according to the form min/max size.
            min.Add(GetTileMapRectangle(DesignForm.MinimumSize).Size);
            max.Add(GetTileMapRectangle(DesignForm.MaximumSize).Size);

            // Min/Max tile size according to system-defined min/max size.
            min.Add(GetTileMapRectangle(SystemInformation.MinimumWindowSize).Size);
            max.Add(GetTileMapRectangle(SystemInformation.PrimaryMonitorMaximizedWindowSize).Size);

            // Min/Max tile size according to the derived value.
            var tileMin = MinimumTileSize;
            var tileMax = MaximumTileSize;

            // The dimensions need to be on a pixel scale
            tileMin.Width *= cellW;
            tileMin.Height *= cellH;
            tileMax.Width *= cellW;
            tileMax.Height *= cellH;

            min.Add(tileMin);
            max.Add(tileMax);

            // Edge case to prevent zero size
            min.Add(new Size(cellW, cellH));

            // Restrict the lower bound of the tilemap.
            foreach (var size in min)
            {
                window.Width = Max(window.Width, size.Width);
                window.Height = Max(window.Height, size.Height);
            }

            // Restrict upper bounds
            foreach (var size in max)
            {
                if (size.Width > 0)
                {
                    window.Width = Min(window.Width, size.Width);
                }

                if (size.Height > 0)
                {
                    window.Height = Min(window.Height, size.Height);
                }
            }

            return window;
        }

        private void DesignForm_SizeChanged(object sender, EventArgs e)
        {
            if (!TileMapResized)
            {
                SetTileMapPadding();
            }
        }

        private void TileMap_TileSizeChanged(object sender, EventArgs e)
        {
            TileMapResized = true;
            var size = InflateSize(TileMap.Size, TileMapPadding);
            DesignForm.Size = size;
        }
    }
}
