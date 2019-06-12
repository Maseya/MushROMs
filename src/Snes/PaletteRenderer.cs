// <copyright file="PaletteRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class PaletteRenderer : TileMap1DRenderer
    {
        private IReadOnlyList<Color15BppBgr> _palette;

        private IListSelection _currentSelection;

        public PaletteRenderer()
            : base()
        {
        }

        public PaletteRenderer(IContainer container)
            : base(container)
        {
        }

        public event EventHandler PaletteChanged;

        public event EventHandler CurrentSelectionChanged;

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
                _currentSelection = value;
                OnCurrentSelectionChanged(EventArgs.Empty);
            }
        }

        public override Color32BppArgb[] RenderPixels()
        {
            if (Palette is null)
            {
                return new Color32BppArgb[0];
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

            var result = new Color32BppArgb[width * imageHeight];
            var gridStartTile = TileMap.GetGridTileIndex(TileMap.Origin);
            Parallel.For(0, viewTileCount, DrawSquare);
            return result;

            void DrawSquare(int viewTileIndex)
            {
                var tileX = viewTileIndex % viewWidth;
                var tileY = viewTileIndex / viewWidth;
                var destIndex = (tileX * TileWidth) + (tileY * pixelsPerRow);
                var color = GetPaletteColor(gridStartTile + viewTileIndex);

                for (var y = 0; y < tileHeight; y++)
                {
                    for (var x = 0; x < tileWidth; x++)
                    {
                        result[destIndex + x] = color;
                    }

                    destIndex += width;
                }
            }
        }

        protected virtual void OnPaletteChanged(EventArgs e)
        {
            SetTileMapGridLength();
            PaletteChanged?.Invoke(this, e);
        }

        protected virtual void OnCurrentSelectionChanged(EventArgs e)
        {
            CurrentSelectionChanged?.Invoke(this, e);
        }

        protected override void OnTileMapChanged(EventArgs e)
        {
            SetTileMapGridLength();
            base.OnTileMapChanged(e);
        }

        private Color32BppArgb GetPaletteColor(int gridTileIndex)
        {
            var color = (Color32BppArgb)Palette[gridTileIndex];
            if (!SelectionContainsTile(gridTileIndex))
            {
                // This will slightly reduce the transparency of non-selected
                // tiles as the selection gets bigger. This makes it easier to
                // know which tiles are selected and which aren't, while at the
                // same time not looking too distracting for very small
                // selections.
                color.Alpha = (byte)Math.Max(
                    Byte.MaxValue - (CurrentSelection.Count / 2),
                    0x80);
            }

            return color;
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
                TileMap.GridLength = Palette != null ? Palette.Count : 0;
            }
        }
    }
}
