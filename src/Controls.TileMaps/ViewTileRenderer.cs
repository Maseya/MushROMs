// <copyright file="ViewTileRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class ViewTileRenderer : TileMapComponent, IGraphicsRenderer
    {
        private Point _gridTile;

        private Padding _padding;

        private IPathRenderer _pathRenderer;

        public ViewTileRenderer()
            : base()
        {
        }

        public ViewTileRenderer(IContainer container)
            : base(container)
        {
        }

        public event EventHandler GridTileChanged;

        public event EventHandler PaddingChanged;

        public event EventHandler PathRendererChanged;

        public event EventHandler Redraw;

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Point GridTile
        {
            get
            {
                return _gridTile;
            }

            set
            {
                if (GridTile == value)
                {
                    return;
                }

                _gridTile = value;
                OnGridTileChanged(EventArgs.Empty);
            }
        }

        public Padding Padding
        {
            get
            {
                return _padding;
            }

            set
            {
                if (Padding == value)
                {
                    return;
                }

                _padding = value;
                OnPaddingChanged(EventArgs.Empty);
            }
        }

        public IPathRenderer PathRenderer
        {
            get
            {
                return _pathRenderer;
            }

            set
            {
                if (PathRenderer == value)
                {
                    return;
                }

                if (PathRenderer != null)
                {
                    PathRenderer.Redraw -= PathRenderer_Redraw;
                }

                _pathRenderer = value;
                if (PathRenderer != null)
                {
                    PathRenderer.Redraw += PathRenderer_Redraw;
                }

                OnPathRendererChanged(EventArgs.Empty);
            }
        }

        public GraphicsPath RenderPath()
        {
            if (TileMap is null)
            {
                throw new InvalidOperationException();
            }

            GraphicsPath temp = null;
            GraphicsPath result = null;
            try
            {
                temp = new GraphicsPath();

                var viewTile = TileMap.GetViewTile(GridTile);
                var x = viewTile.X * TileMap.TileSize.Width;
                var y = viewTile.Y * TileMap.TileSize.Height;
                var rectangle = new Rectangle(
                    x + Padding.Left,
                    y + Padding.Top,
                    TileMap.TileSize.Width - 1 - Padding.Horizontal,
                    TileMap.TileSize.Height - 1 - Padding.Vertical);

                temp.AddRectangle(rectangle);

                result = temp;
                temp = null;
                return result;
            }
            finally
            {
                temp?.Dispose();
            }
        }

        public void Draw(Graphics graphics)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (PathRenderer is null || TileMap is null)
            {
                return;
            }

            using (var graphicsPath = RenderPath())
            {
                PathRenderer.DrawPath(graphics, graphicsPath);
            }
        }

        protected virtual void OnGridTileChanged(EventArgs e)
        {
            GridTileChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnPaddingChanged(EventArgs e)
        {
            PaddingChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnPathRendererChanged(EventArgs e)
        {
            PathRendererChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnRedraw(EventArgs e)
        {
            Redraw?.Invoke(this, e);
        }

        private void PathRenderer_Redraw(object sender, EventArgs e)
        {
            OnRedraw(EventArgs.Empty);
        }
    }
}
