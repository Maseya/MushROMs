// <copyright file="GfxTileEditorControl.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Maseya.Controls.TileMaps;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public partial class GfxTileEditorControl : TileMapUIControl
    {
        private const int PaletteTilePixelSize = 8;

        private IGfxEditor _gfx;

        public GfxTileEditorControl()
        {
            InitializeComponent();
        }

        public event EventHandler ZoomSizeChanged;

        public event EventHandler GfxChanged;

        public event EventHandler PaletteChanged;

        public ZoomSize ZoomSize
        {
            get
            {
                return (ZoomSize)(TileMap.TileWidth / PaletteTilePixelSize);
            }

            set
            {
                TileMap.TileSize = new Size(
                    PaletteTilePixelSize * (int)value,
                    PaletteTilePixelSize * (int)value);

                OnZoomSizeChanged(EventArgs.Empty);
            }
        }

        public IGfxEditor Gfx
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

                if (Gfx != null)
                {
                    Gfx.WriteDataComplete -= Editor_WriteDataComplete;
                }

                _gfx = value;
                if (Gfx != null)
                {
                    Gfx.WriteDataComplete += Editor_WriteDataComplete;
                    gfxTileRenderer.Gfx =
                        new ReadOnlyCollection<GfxTile>(Gfx);
                }
                else
                {
                    gfxTileRenderer.Palette = null;
                }

                OnPaletteChanged(EventArgs.Empty);
            }
        }

        public IReadOnlyList<Color15BppBgr> Palette
        {
            get
            {
                return gfxTileRenderer.Palette;
            }

            set
            {
                gfxTileRenderer.Palette = value;
            }
        }

        public IListSelection CurrentListSelection
        {
            get
            {
                return gfxTileRenderer.CurrentSelection;
            }
        }

        public new TileMap1D TileMap
        {
            get
            {
                return base.TileMap as TileMap1D;
            }

            set
            {
                base.TileMap = value;
            }
        }

        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
        }

        protected override void OnCurrentSelectionChanged(EventArgs e)
        {
            var listSelection = ConvertSelection(CurrentSelection);
            gfxTileRenderer.CurrentSelection = listSelection;
            if (Palette != null)
            {
                Gfx.CurrentSelection = listSelection;
            }

            base.OnCurrentSelectionChanged(e);

            IListSelection ConvertSelection(ITileMapSelection selection)
            {
                if (selection is null)
                {
                    return null;
                }

                var indexes = selection.Select(
                    p => TileMap1D.GetTileIndex(p, selection.GridWidth));

                return new EnumerableIndexListSelection(indexes);
            }
        }

        protected override void OnTileMapChanged(EventArgs e)
        {
            if (base.TileMap is null || base.TileMap is TileMap1D)
            {
                base.OnTileMapChanged(e);
            }
            else
            {
                // Enforce that only 1D TileMaps be set.
                throw new InvalidOperationException();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var start = DateTime.Now;
            base.OnPaint(e);
            var elapsed = DateTime.Now - start;
            Parent.Text = elapsed.Milliseconds.ToString();
        }

        protected virtual void OnGfxChanged(EventArgs e)
        {
            GfxChanged?.Invoke(this, e);
        }

        protected virtual void OnPaletteChanged(EventArgs e)
        {
            PaletteChanged?.Invoke(this, e);
        }

        private void Editor_WriteDataComplete(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (Gfx is null)
            {
                throw new InvalidOperationException();
            }

            Gfx.Cut();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (Gfx is null)
            {
                throw new InvalidOperationException();
            }

            Gfx.Copy();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (Gfx is null)
            {
                throw new InvalidOperationException();
            }

            Gfx.Paste();
        }

        private void Renderer_PaletteChanged(object sender, EventArgs e)
        {
            OnPaletteChanged(EventArgs.Empty);
        }
    }
}
