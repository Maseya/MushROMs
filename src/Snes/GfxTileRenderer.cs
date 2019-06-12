// <copyright file="GfxTileRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class GfxTileRenderer : TileMap1DRenderer
    {
        private IReadOnlyList<GfxTile> _gfx;

        private IReadOnlyList<Color15BppBgr> _palette;

        private int _paltteStartIndex;

        private IListSelection _currentSelection;

        public GfxTileRenderer()
            : base()
        {
        }

        public GfxTileRenderer(IContainer container)
            : base(container)
        {
        }

        public event EventHandler GfxChanged;

        public event EventHandler CurrentSelectionChanged;

        public event EventHandler PaletteChanged;

        public event EventHandler PaletteStartIndexChanged;

        public IReadOnlyList<GfxTile> Gfx
        {
            get
            {
                return _gfx;
            }

            set
            {
                if (Gfx == value)
                {
                    return;
                }

                _gfx = value;
                OnGfxChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public IListSelection CurrentSelection
        {
            get
            {
                return _currentSelection;
            }

            set
            {
                if (CurrentSelection == value)
                {
                    return;
                }

                _currentSelection = value;
                OnCurrentSelectionChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public IReadOnlyList<Color15BppBgr> Palette
        {
            get
            {
                return _palette;
            }

            set
            {
                if (Palette == value)
                {
                    return;
                }

                _palette = value;
                OnPaletteChanged(EventArgs.Empty);
            }
        }

        public int PaletteStartIndex
        {
            get
            {
                return _paltteStartIndex;
            }

            set
            {
                if (PaletteStartIndex == value)
                {
                    return;
                }

                _paltteStartIndex = value;
                OnPaletteStartIndexChanged(EventArgs.Empty);
            }
        }

        public override unsafe Color32BppArgb[] RenderPixels()
        {
            if (Gfx is null)
            {
                return new Color32BppArgb[0];
            }

            var colors = new Color32BppArgb[0x100];
            for (var i = 0; i < Math.Min(colors.Length, Palette.Count); i++)
            {
                colors[i] = Palette[i];
            }

            var tileWidth = TileWidth;
            var tileHeight = TileHeight;

            var width = Width;

            var pixelsPerRow = width * tileHeight;

            var viewWidth = ViewWidth;
            var viewHeight = ViewHeight;
            var viewTileCount = TileMap.VisibleGridTileCount;
            if (viewTileCount == 0)
            {
                return new Color32BppArgb[0];
            }

            var imageHeight = (((viewTileCount - 1) / viewWidth) + 1)
                * tileHeight;

            var pixelY = new int[tileHeight];
            for (var y = 0; y < tileHeight; y++)
            {
                pixelY[y] = y * GfxTile.PixelsPerPlane / tileHeight;
                pixelY[y] *= GfxTile.PlanesPerTile;
            }

            var pixelX = new int[tileWidth];
            for (var x = 0; x < tileWidth; x++)
            {
                pixelX[x] = x * GfxTile.PixelsPerPlane / tileWidth;
            }

            var result = new Color32BppArgb[width * imageHeight];
            var gridTileIndex = TileMap.GetGridTileIndex(TileMap.Origin);
            for (var tileIndex = 0; tileIndex < viewTileCount; tileIndex++)
            {
                var tileX = tileIndex % viewWidth;
                var tileY = tileIndex / viewWidth;
                var destIndex = (tileX * TileWidth) + (tileY * pixelsPerRow);

                var tile = Gfx[gridTileIndex];
                var alpha = GetAlpha();
                for (var y = 0; y < tileHeight; y++)
                {
                    var pixelHeight = pixelY[y];
                    for (var x = 0; x < tileWidth; x++)
                    {
                        var colorIndex = tile.Pixels[pixelHeight + pixelX[x]];
                        var color = colors[colorIndex];

                        color.Alpha = alpha;
                        result[destIndex + x] = color;
                    }

                    destIndex += width;
                }

                gridTileIndex++;
            }

            return result;

            byte GetAlpha()
            {
                return (byte)(SelectionContainsTile(gridTileIndex)
                    ? 0xFF
                    : Math.Max(
                        Byte.MaxValue - (CurrentSelection.Count / 2),
                        0x80));
            }
        }

        protected virtual void OnGfxChanged(EventArgs e)
        {
            SetTileMapGridLength();
            GfxChanged?.Invoke(this, e);
        }

        protected virtual void OnCurrentSelectionChanged(EventArgs e)
        {
            CurrentSelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnPaletteChanged(EventArgs e)
        {
            PaletteChanged?.Invoke(this, e);
        }

        protected virtual void OnPaletteStartIndexChanged(EventArgs e)
        {
            PaletteStartIndexChanged?.Invoke(this, e);
        }

        protected override void OnTileMapChanged(EventArgs e)
        {
            SetTileMapGridLength();
            base.OnTileMapChanged(e);
        }

        private bool SelectionContainsTile(int gridTileIndex)
        {
            return CurrentSelection is null
                || CurrentSelection.ContainsIndex(gridTileIndex);
        }

        private void SetTileMapGridLength()
        {
            if (TileMap != null)
            {
                TileMap.GridLength = Gfx != null ? Gfx.Count : 0;
            }
        }
    }
}
