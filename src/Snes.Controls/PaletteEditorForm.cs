// <copyright file="PaletteEditorForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Controls
{
    using System;
    using Maseya.Controls.TileMaps;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public partial class PaletteEditorForm : TileMapForm
    {
        public PaletteEditorForm()
        {
            InitializeComponent();
            TileMap = paletteEditorControl.TileMap;
        }

        public event EventHandler PaletteChanged;

        public IListEditor<Color15BppBgr> Palette
        {
            get
            {
                return paletteEditorControl.Palette;
            }

            set
            {
                paletteEditorControl.Palette = value;
            }
        }

        private new TileMap1D TileMap
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

        protected override void OnTileMapChanged(EventArgs e)
        {
            if (TileMap != paletteEditorControl.TileMap)
            {
                TileMap = paletteEditorControl.TileMap;
                return;
            }

            base.OnTileMapChanged(e);
        }

        protected virtual void OnPaletteChanged(EventArgs e)
        {
            PaletteChanged?.Invoke(this, e);
        }

        private void Control_PaletteChanged(object sender, EventArgs e)
        {
            OnPaletteChanged(EventArgs.Empty);
        }
    }
}
