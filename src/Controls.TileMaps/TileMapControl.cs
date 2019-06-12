// <copyright file="TileMapControl.cs" company="Public Domain">
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

    public class TileMapControl : DesignControl
    {
        private TileMap _tileMap;

        public event EventHandler TileMapChanged;

        public TileMap TileMap
        {
            get
            {
                return _tileMap;
            }

            set
            {
                if (TileMap != null)
                {
                    TileMap.TileSizeChanged -= TileMap_SizeChanged;
                    TileMap.ViewSizeChanged -= TileMap_SizeChanged;
                    TileMap.GridSizeChanged -= TileMap_Redraw;
                    TileMap.OriginChanged -= TileMap_Redraw;
                }

                _tileMap = value;
                if (TileMap != null)
                {
                    TileMap.TileSizeChanged += TileMap_SizeChanged;
                    TileMap.ViewSizeChanged += TileMap_SizeChanged;
                    TileMap.GridSizeChanged += TileMap_Redraw;
                    TileMap.OriginChanged += TileMap_Redraw;
                }

                OnTileMapChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        internal ResizeMode ResizeMode
        {
            get;
            set;
        }

        protected override void SetBoundsCore(
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified)
        {
            var padding = BorderPadding;
            var clientSize = new Size(
                width - padding.Horizontal,
                height - padding.Vertical);

            var size = new Size(
                clientSize.Width + padding.Horizontal,
                clientSize.Height + padding.Vertical);

            base.SetBoundsCore(
                x,
                y,
                size.Width,
                size.Height,
                specified);
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            if (ResizeMode == ResizeMode.ControlResize)
            {
                return;
            }

            if (ResizeMode == ResizeMode.None)
            {
                ResizeMode = ResizeMode.ControlResize;
            }

            base.SetClientSizeCore(x, y);

            if (ResizeMode == ResizeMode.ControlResize)
            {
                ResizeMode = ResizeMode.None;
            }
        }

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            TileMapChanged?.Invoke(this, e);
            if (TileMap != null)
            {
                SetClientSizeFromTileMap();
            }
        }

        private void SetClientSizeFromTileMap()
        {
            if (TileMap.Size == ClientSize)
            {
                return;
            }

            if (ResizeMode == ResizeMode.None)
            {
                ResizeMode = ResizeMode.TileMapResize;
            }

            SetClientSizeCore(TileMap.Size.Width, TileMap.Size.Height);

            if (ResizeMode == ResizeMode.TileMapResize)
            {
                ResizeMode = ResizeMode.None;
            }
        }

        private void TileMap_SizeChanged(object sender, EventArgs e)
        {
            SetClientSizeFromTileMap();
        }

        private void TileMap_Redraw(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
