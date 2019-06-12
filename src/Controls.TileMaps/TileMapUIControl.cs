// <copyright file="TileMapUIControl.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Maseya.TileMaps;

    public partial class TileMapUIControl : TileMapControl
    {
        public TileMapUIControl()
        {
            InitializeComponent();
        }

        public event EventHandler ActiveGridTileChanged;

        public event EventHandler CurrentSelectionChanged;

        public TileMapRenderer TileMapRenderer
        {
            get
            {
                return tileMapImageRenderer.TileMapRenderer;
            }

            set
            {
                tileMapImageRenderer.TileMapRenderer = value;
            }
        }

        public ITileMapSelection CurrentSelection
        {
            get
            {
                return selectionUI.CurrentSelection;
            }

            set
            {
                selectionUI.CurrentSelection = value;
            }
        }

        public Point ActiveViewTile
        {
            get
            {
                return activeGridTileUI.ActiveGridTile;
            }

            set
            {
                activeGridTileUI.ActiveGridTile = value;
            }
        }

        protected override void OnTileMapChanged(EventArgs e)
        {
            selectionRenderer.TileMap =
            viewTileRenderer.TileMap =
            selectionUI.TileMap =
            activeGridTileUI.TileMap = TileMap;
            if (tileMapImageRenderer?.TileMapRenderer != null)
            {
                tileMapImageRenderer.TileMapRenderer.TileMap = TileMap;
            }

            base.OnTileMapChanged(e);
        }

        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
        }

        protected virtual void OnCurrentSelectionChanged(EventArgs e)
        {
            CurrentSelectionChanged?.Invoke(this, e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (TileMap != null)
            {
                if (e is null)
                {
                    throw new ArgumentNullException(nameof(e));
                }

                if (e.Delta > 0 && TileMap.OriginY > 0)
                {
                    TileMap.OriginY--;
                }

                if (e.Delta < 0 && TileMap.OriginY + 1 <= TileMap.GridHeight)
                {
                    TileMap.OriginY++;
                }
            }

            base.OnMouseWheel(e);
        }

        private void ActiveGridTileUI_ActiveViewTileChanged(
            object sender,
            EventArgs e)
        {
            viewTileRenderer.GridTile = activeGridTileUI.ActiveGridTile;
            selectionUI.ActiveGridTile = activeGridTileUI.ActiveGridTile;
            OnActiveGridTileChanged(EventArgs.Empty);
        }

        private void SelectionUI_CurrentSelectionChanged(
            object sender,
            EventArgs e)
        {
            var selection = selectionUI.CurrentSelection;
            selectionRenderer.Selection = selection;
            OnCurrentSelectionChanged(EventArgs.Empty);
        }
    }
}
