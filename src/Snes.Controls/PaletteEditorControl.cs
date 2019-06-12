// <copyright file="PaletteEditorControl.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Maseya.Controls.TileMaps;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public partial class PaletteEditorControl : TileMapUIControl
    {
        private const int PaletteTilePixelSize = 8;

        private IListEditor<Color15BppBgr> _palette;

        public PaletteEditorControl()
        {
            InitializeComponent();
        }

        public event EventHandler ZoomSizeChanged;

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

        public IListEditor<Color15BppBgr> Palette
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

                if (Palette != null)
                {
                    Palette.WriteDataComplete -= Editor_WriteDataComplete;
                }

                _palette = value;
                if (Palette != null)
                {
                    Palette.WriteDataComplete += Editor_WriteDataComplete;
                    paletteRenderer.Palette =
                        new ReadOnlyCollection<Color15BppBgr>(Palette);
                }
                else
                {
                    paletteRenderer.Palette = null;
                }

                OnPaletteChanged(EventArgs.Empty);
            }
        }

        public IListSelection CurrentListSelection
        {
            get
            {
                return paletteRenderer.CurrentSelection;
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

        public void InvertColors()
        {
            if (Palette is null)
            {
                throw new InvalidOperationException();
            }

            if (CurrentListSelection is null)
            {
                var index = TileMap.GetGridTileIndex(ActiveViewTile);
                Palette[index] ^= (Color15BppBgr)0x7FFF;
            }
            else
            {
                var data = new ListSelectionData<Color15BppBgr>(
                    CurrentListSelection,
                    new ReadOnlyCollection<Color15BppBgr>(Palette));

                foreach (var index in data.Selection)
                {
                    data[index] ^= (Color15BppBgr)0x7FFF;
                }

                Palette.WriteSelection(data);
            }
        }

        public void EditCurrentColor()
        {
            var index = TileMap.GetGridTileIndex(ActiveViewTile);
            using (var dialog = new ColorDialog())
            {
                dialog.Color = Palette[index];
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Palette[index] = (Color15BppBgr)dialog.Color;
                }
            }
        }

        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            ZoomSizeChanged?.Invoke(this, e);
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

        protected override void OnCurrentSelectionChanged(EventArgs e)
        {
            var listSelection = ConvertSelection(CurrentSelection);
            paletteRenderer.CurrentSelection = listSelection;
            if (Palette != null)
            {
                Palette.CurrentSelection = listSelection;
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

        protected virtual void OnPaletteChanged(EventArgs e)
        {
            PaletteChanged?.Invoke(this, e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Button == MouseButtons.Left)
            {
                EditCurrentColor();
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Modifiers == Keys.None && e.KeyCode == Keys.Space)
            {
                EditCurrentColor();
            }

            base.OnKeyDown(e);
        }

        private void Editor_WriteDataComplete(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            if (Palette is null)
            {
                throw new InvalidOperationException();
            }

            Palette.Cut();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (Palette is null)
            {
                throw new InvalidOperationException();
            }

            Palette.Copy();
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (Palette is null)
            {
                throw new InvalidOperationException();
            }

            Palette.Paste();
        }

        private void InvertColors_Click(object sender, EventArgs e)
        {
            if (Palette is null)
            {
                throw new InvalidOperationException();
            }

            InvertColors();
        }
    }
}
