// <copyright file="ScrollHelper.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.TileMaps;

    public class ScrollHelper : Component
    {
        private TileMap _tileMap;
        private ScrollBar _verticalScrollBar;
        private ScrollBar _horizontalScrollBar;

        public ScrollHelper()
        {
        }

        public ScrollHelper(IContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler TileMapChanged;

        public event EventHandler VerticalScrollBarChanged;

        public event EventHandler HorizontalScrollBarChanged;

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
                    TileMap.ViewSizeChanged -= TileMap_SizePropertyChanged;
                    TileMap.GridSizeChanged -= TileMap_SizePropertyChanged;
                    TileMap.OriginChanged -= TileMap_ZeroTileChanged;
                }

                _tileMap = value;
                if (TileMap != null)
                {
                    TileMap.ViewSizeChanged += TileMap_SizePropertyChanged;
                    TileMap.GridSizeChanged += TileMap_SizePropertyChanged;
                    TileMap.OriginChanged += TileMap_ZeroTileChanged;
                }

                OnTileMapChanged(EventArgs.Empty);
            }
        }

        public ScrollBar VerticalScrollBar
        {
            get
            {
                return _verticalScrollBar;
            }

            set
            {
                if (VerticalScrollBar == value)
                {
                    return;
                }

                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll -= VScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged -= VScrollBar_ValueChanged;
                }

                _verticalScrollBar = value;
                if (VerticalScrollBar != null)
                {
                    VerticalScrollBar.Scroll += VScrollBar_Scroll;
                    VerticalScrollBar.ValueChanged += VScrollBar_ValueChanged;
                }

                OnVerticalScrollBarChanged(EventArgs.Empty);
            }
        }

        public ScrollBar HorizontalScrollBar
        {
            get
            {
                return _horizontalScrollBar;
            }

            set
            {
                if (HorizontalScrollBar == value)
                {
                    return;
                }

                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll += HScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged += HScrollBar_ValueChanged;
                }

                _horizontalScrollBar = value;
                if (HorizontalScrollBar != null)
                {
                    HorizontalScrollBar.Scroll += HScrollBar_Scroll;
                    HorizontalScrollBar.ValueChanged += HScrollBar_ValueChanged;
                }

                OnHorizontalScrollBarChanged(EventArgs.Empty);
            }
        }

        public void ResetScrollBars()
        {
            ResetVerticalScrollBar();
            ResetHorizontalScrollBar();
        }

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            TileMapChanged?.Invoke(this, e);
        }

        protected virtual void OnVerticalScrollBarChanged(EventArgs e)
        {
            ResetVerticalScrollBar();
            VerticalScrollBarChanged?.Invoke(this, e);
        }

        protected virtual void OnHorizontalScrollBarChanged(EventArgs e)
        {
            ResetHorizontalScrollBar();
            HorizontalScrollBarChanged?.Invoke(this, e);
        }

        private void ResetVerticalScrollBar()
        {
            if (VerticalScrollBar is null)
            {
                return;
            }

            if (TileMap is null)
            {
                VerticalScrollBar.Enabled = false;
                return;
            }

            var viewHeight = TileMap.ViewSize.Height;
            var workHeight = TileMap.GridSize.Height;
            var enabled = workHeight > 1;
            if (VerticalScrollBar.Enabled = enabled)
            {
                VerticalScrollBar.Minimum = 0;
                VerticalScrollBar.Maximum = viewHeight + workHeight - 2;
                VerticalScrollBar.SmallChange = 1;
                VerticalScrollBar.LargeChange = viewHeight;
                VerticalScrollBar.Value = TileMap.Origin.Y;
            }
        }

        private void ResetHorizontalScrollBar()
        {
            if (HorizontalScrollBar is null)
            {
                return;
            }

            if (TileMap is null)
            {
                HorizontalScrollBar.Enabled = false;
                return;
            }

            var viewWidth = TileMap.ViewSize.Width;
            var workWidth = TileMap.GridSize.Width;
            var enabled = workWidth > 1;
            if (HorizontalScrollBar.Enabled = enabled)
            {
                HorizontalScrollBar.Minimum = 0;
                HorizontalScrollBar.Maximum = viewWidth + workWidth - 2;
                HorizontalScrollBar.SmallChange = 1;
                HorizontalScrollBar.LargeChange = viewWidth;
                HorizontalScrollBar.Value = TileMap.Origin.X;
            }
        }

        private void AdjustScrollBarPositions()
        {
            if (TileMap is null)
            {
                return;
            }

            HorizontalScrollBar.Value = TileMap.Origin.X;
            VerticalScrollBar.Value = TileMap.Origin.Y;
        }

        private void ScrollTileMapVertical(int value)
        {
            if (TileMap is null)
            {
                return;
            }

            TileMap.Origin = new Point(TileMap.Origin.X, value);
        }

        private void ScrollTileMapHorizontal(int value)
        {
            if (TileMap is null)
            {
                return;
            }

            TileMap.Origin = new Point(value, TileMap.Origin.Y);
        }

        private void TileMap_ZeroTileChanged(object sender, EventArgs e)
        {
            AdjustScrollBarPositions();
        }

        private void TileMap_SizePropertyChanged(object sender, EventArgs e)
        {
            ResetScrollBars();
        }

        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                ScrollTileMapVertical(e.NewValue);
            }
        }

        private void VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapVertical(VerticalScrollBar.Value);
        }

        private void HScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ScrollTileMapHorizontal(e.NewValue);
            }
        }

        private void HScrollBar_ValueChanged(object sender, EventArgs e)
        {
            ScrollTileMapHorizontal(HorizontalScrollBar.Value);
        }
    }
}
