// <copyright file="SelectionUI.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Maseya.TileMaps;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using static System.Math;

    public class SelectionUI : TileMapComponent
    {
        private DesignControl _control;

        private Point _activeGridTile;

        private ITileMapSelection _currentSelection;

        public SelectionUI()
            : base()
        {
        }

        public SelectionUI(IContainer container)
            : base(container)
        {
        }

        public event EventHandler ControlChanged;

        public event EventHandler ActiveGridTileChanged;

        public event EventHandler CurrentSelectionChanged;

        public DesignControl Control
        {
            get
            {
                return _control;
            }

            set
            {
                if (Control != null)
                {
                    Control.MouseDown -= Control_MouseDown;
                    Control.MouseUp += Control_MouseUp;
                    Control.KeyDown -= Control_KeyDown;
                    Control.KeyUp -= Control_KeyUp;
                    Control.KeyPress -= Control_KeyPress;
                }

                _control = value;
                if (Control != null)
                {
                    Control.MouseDown += Control_MouseDown;
                    Control.MouseUp += Control_MouseUp;
                    Control.KeyDown += Control_KeyDown;
                    Control.KeyUp += Control_KeyUp;
                    Control.KeyPress += Control_KeyPress;
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

                _activeGridTile = value;
                OnActiveGridTileChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ITileMapSelection CurrentSelection
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

        private Point FirstGridTile
        {
            get;
            set;
        }

        private bool IsSelectionMode
        {
            get;
            set;
        }

        protected virtual void OnControlChanged(EventArgs e)
        {
            ControlChanged?.Invoke(this, e);
            UpdateSelection();
        }

        protected virtual void OnActiveGridTileChanged(EventArgs e)
        {
            ActiveGridTileChanged?.Invoke(this, e);
            UpdateSelection();
        }

        protected virtual void OnCurrentSelectionChanged(EventArgs e)
        {
            CurrentSelectionChanged?.Invoke(this, e);
        }

        private void InitializeSelectionMode()
        {
            if (IsSelectionMode)
            {
                return;
            }

            IsSelectionMode = true;
            FirstGridTile = ActiveGridTile;
            UpdateSelection();
        }

        private void EndSelectionMode()
        {
            IsSelectionMode = false;
        }

        private void UpdateSelection()
        {
            if (Control is null || TileMap is null)
            {
                CurrentSelection = null;
                return;
            }

            if (!IsSelectionMode)
            {
                return;
            }

            CurrentSelection = ChooseSelection();
        }

        private ITileMapSelection ChooseSelection()
        {
            ITileMapSelection result = null;
            if (DesignControl.AltKeyHeld)
            {
                var firstIndex = TileMap1D.GetTileIndex(
                    FirstGridTile,
                    TileMap.GridWidth);

                var activeIndex = TileMap1D.GetTileIndex(
                    ActiveGridTile,
                    TileMap.GridWidth);

                var originIndex = Min(firstIndex, activeIndex);
                var endIndex = Max(firstIndex, activeIndex);
                var count = endIndex - originIndex + 1;
                result = new LinearTileMapSelection(
                    firstIndex < activeIndex ? FirstGridTile : ActiveGridTile,
                    count,
                    TileMap.GridWidth);
            }
            else
            {
                var origin = new Point(
                    Min(FirstGridTile.X, ActiveGridTile.X),
                    Min(FirstGridTile.Y, ActiveGridTile.Y));

                var end = new Point(
                    Max(FirstGridTile.X, ActiveGridTile.X),
                    Max(FirstGridTile.Y, ActiveGridTile.Y));

                var size = new Size(
                    end.X + 1 - origin.X,
                    end.Y + 1 - origin.Y);
                result = new BoxTileMapSelection(
                    origin,
                    size,
                    TileMap.GridWidth);
            }

            if (DesignControl.ControlKeyHeld && CurrentSelection != null)
            {
                result = new EnumerableTileMapSelection(
                    CurrentSelection.Union(result),
                    TileMap.GridWidth);
            }

            return result;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                InitializeSelectionMode();
            }
            else if (e.Button == MouseButtons.Right
                && CurrentSelection != null
                && !CurrentSelection.Contains(ActiveGridTile))
            {
                CurrentSelection = null;
            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                InitializeSelectionMode();
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (!DesignControl.ShiftKeyHeld)
                {
                    EndSelectionMode();
                }
            }
        }

        private void Control_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift)
            {
                if (!DesignControl.MouseButtons.HasFlag(MouseButtons.Left))
                {
                    EndSelectionMode();
                }
            }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsSelectionMode)
            {
                CurrentSelection = null;
            }
        }
    }
}
