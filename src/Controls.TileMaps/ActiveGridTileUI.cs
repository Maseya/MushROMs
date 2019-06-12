// <copyright file="ActiveGridTileUI.cs" company="Public Domain">
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
    using static System.ComponentModel.DesignerSerializationVisibility;
    using static System.Math;

    public class ActiveGridTileUI : TileMapComponent
    {
        private DesignControl _control;

        private Point _activeGridTile;

        public ActiveGridTileUI()
            : base()
        {
        }

        public ActiveGridTileUI(IContainer container)
            : base(container)
        {
        }

        public event EventHandler ControlChanged;

        public event EventHandler ActiveGridTileChanged;

        public DesignControl Control
        {
            get
            {
                return _control;
            }

            set
            {
                if (Control == value)
                {
                    return;
                }

                if (Control != null)
                {
                    Control.MouseMove += Control_MouseMove;
                    Control.KeyDown += Control_KeyDown;
                }

                _control = value;
                if (Control != null)
                {
                    Control.MouseMove += Control_MouseMove;
                    Control.KeyDown += Control_KeyDown;
                }

                OnControlChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Point ActiveGridTile
        {
            get
            {
                return _activeGridTile;
            }

            set
            {
                if (ActiveGridTile == value)
                {
                    return;
                }

                if (!TileMap.GridContainsGridTile(value))
                {
                    return;
                }

                _activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnControlChanged(EventArgs e)
        {
            ControlChanged?.Invoke(this, e);
        }

        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (TileMap is null)
            {
                return;
            }

            if (e.Button == MouseButtons.None
                && !TileMap.ViewAreaContainsPixel(e.Location))
            {
                return;
            }

            if (!Control.MouseHovering)
            {
                SetViewTile(TileMap.GetViewTileFromPixel(e.Location));
            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (TileMap is null)
            {
                return;
            }

            var viewTile = TileMap.GetViewTile(ActiveGridTile);
            var (x, y) = (viewTile.X, viewTile.Y);
            switch (e.KeyCode)
            {
            case Keys.Left:
                x--;
                break;

            case Keys.Right:
                x++;
                break;

            case Keys.Up:
                y--;
                break;

            case Keys.Down:
                y++;
                break;
            }

            SetViewTile(new Point(x, y));
        }

        private void SetViewTile(Point viewTile)
        {
            var result = TileMap.GetGridTile(viewTile);
            if (result == ActiveGridTile)
            {
                return;
            }

            var x = viewTile.X;
            var y = viewTile.Y;
            var lastX = TileMap.ViewWidth - 1;
            if (x < 0)
            {
                TileMap.OriginX = Max(TileMap.OriginX - 1, 0);
                x = 0;
            }
            else if (x > lastX)
            {
                TileMap.OriginX = Min(
                    TileMap.OriginX + 1,
                    TileMap.GridWidth - 1);

                x = lastX;
            }

            var lastY = TileMap.ViewHeight - 1;
            if (y < 0)
            {
                TileMap.OriginY = Max(TileMap.OriginY - 1, 0);
                y = 0;
            }
            else if (y > lastY)
            {
                TileMap.OriginY = Min(
                    TileMap.OriginY + 1,
                    TileMap.GridHeight - 1);

                y = lastY;
            }

            result = TileMap.GetGridTile(new Point(x, y));
            if (result == ActiveGridTile)
            {
                return;
            }

            ActiveGridTile = result;
        }
    }
}
