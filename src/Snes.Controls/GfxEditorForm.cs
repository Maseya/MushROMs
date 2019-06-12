// <copyright file="GfxEditorForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Maseya.Controls;
    using Maseya.Controls.TileMaps;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public partial class GfxEditorForm : DesignForm, IZoomComponent
    {
        private const int DefaultGfxTileWidth = 8;

        private const int DefaultGfxTileHeight = 8;

        public GfxEditorForm()
        {
            InitializeComponent();

            resizeHelper.TileMap =
            tileScrollHelper.TileMap = gfxTileEditorControl.TileMap;
        }

        public event EventHandler ZoomSizeChanged;

        public event EventHandler GfxChanged;

        public ZoomSize ZoomSize
        {
            get
            {
                return (ZoomSize)(TileMap.TileWidth / DefaultGfxTileWidth);
            }

            set
            {
                TileMap.TileSize = new Size(
                    DefaultGfxTileWidth * (int)value,
                    DefaultGfxTileHeight * (int)value);

                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        public TileMap1D TileMap
        {
            get
            {
                return gfxTileEditorControl.TileMap;
            }
        }

        public IGfxEditor Gfx
        {
            get
            {
                return gfxTileEditorControl.Gfx;
            }

            set
            {
                gfxTileEditorControl.Gfx = value;
                OnGfxChanged(EventArgs.Empty);
            }
        }

        public IReadOnlyList<Color15BppBgr> Palette
        {
            get
            {
                return gfxTileEditorControl.Palette;
            }

            set
            {
                gfxTileEditorControl.Palette = value;
            }
        }

        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
        }

        protected virtual void OnGfxChanged(EventArgs e)
        {
            GfxChanged?.Invoke(this, e);
        }
    }
}
