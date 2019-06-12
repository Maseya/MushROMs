// <copyright file="Glyph.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public enum ResizeDirection
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    public partial class Glyph : UserControl
    {
        private ResizeDirection _resizeDirection;

        public Glyph()
        {
            Size = new Size(6, 6);
        }

        public ResizeDirection ResizeDirection
        {
            get
            {
                return _resizeDirection;
            }

            set
            {
                if (ResizeDirection == value)
                {
                    return;
                }

                if (!Enum.IsDefined(typeof(ResizeDirection), value))
                {
                    throw new InvalidEnumArgumentException(
                        nameof(value),
                        (int)value,
                        typeof(ResizeDirection));
                }

                _resizeDirection = value;
            }
        }

        public Control Control
        {
            get;
            set;
        }

        private Cursor SavedCursor
        {
            get;
            set;
        }

        private bool CanResize
        {
            get;
            set;
        }

        private bool IsResizing
        {
            get;
            set;
        }

        private Size OriginalControlSize
        {
            get;
            set;
        }

        private Point OriginalLocation
        {
            get;
            set;
        }

        private Point MouseStartLocation
        {
            get;
            set;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(
                Pens.Black,
                new Rectangle(0, 0, 5, 5));

            e.Graphics.FillRectangle(
                Brushes.White,
                new Rectangle(1, 1, 4, 4));

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Control != null && SavedCursor is null)
            {
                var cursor = Cursor;
                switch (ResizeDirection)
                {
                    case ResizeDirection.Left:
                    case ResizeDirection.Right:
                        cursor = Cursors.SizeWE;
                        break;

                    case ResizeDirection.Top:
                    case ResizeDirection.Bottom:
                        cursor = Cursors.SizeNS;
                        break;

                    case ResizeDirection.TopLeft:
                    case ResizeDirection.BottomRight:
                        cursor = Cursors.SizeNWSE;
                        break;

                    case ResizeDirection.TopRight:
                    case ResizeDirection.BottomLeft:
                        cursor = Cursors.SizeNESW;
                        break;
                }

                if (CanResize = cursor != Control.Cursor)
                {
                    SavedCursor = Cursor;
                    Cursor = cursor;
                }
            }

            if (IsResizing)
            {
                var size = new Size(
                    e.Location.X + Location.X - MouseStartLocation.X,
                    e.Location.Y + Location.Y - MouseStartLocation.Y);

                Location = new Point(
                    OriginalLocation.X + size.Width,
                    OriginalLocation.Y + size.Height);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (DefaultCursor != null)
            {
                Cursor = DefaultCursor;
                SavedCursor = null;
                CanResize = false;
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Control != null && CanResize && !IsResizing)
            {
                OriginalControlSize = Control.Size;
                OriginalLocation = Location;
                MouseStartLocation = new Point(
                    OriginalLocation.X + e.Location.X,
                    OriginalLocation.Y + e.Location.Y);
                IsResizing = true;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            IsResizing = false;

            base.OnMouseUp(e);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            if (Control != null && IsResizing)
            {
                var size = new Size(
                    Location.X - OriginalLocation.X,
                    Location.Y - OriginalLocation.Y);

                Control.Size = new Size(
                    OriginalControlSize.Width + size.Width,
                    OriginalControlSize.Height + size.Height);
            }

            base.OnLocationChanged(e);
        }
    }
}
