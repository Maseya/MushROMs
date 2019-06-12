// <copyright file="SelectionRenderer.cs" company="Public Domain">
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
    using Maseya.TileMaps;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public class SelectionRenderer : TileMapComponent, IGraphicsRenderer
    {
        private ITileMapSelection _selection;

        private IPathRenderer _pathRenderer;

        public SelectionRenderer()
            : base()
        {
        }

        public SelectionRenderer(IContainer container)
            : base(container)
        {
        }

        public event EventHandler Redraw;

        public event EventHandler SelectionChanged;

        public event EventHandler PathRendererChanged;

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public ITileMapSelection Selection
        {
            get
            {
                return _selection;
            }

            set
            {
                if (Selection == value)
                {
                    return;
                }

                _selection = value;
                OnSelectionChanged(EventArgs.Empty);
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

                _pathRenderer = value;
                OnPathRendererChanged(EventArgs.Empty);
            }
        }

        public GraphicsPath RenderPath()
        {
            if (Selection is null)
            {
                throw new InvalidOperationException();
            }

            GraphicsPath temp = null;
            GraphicsPath result = null;
            try
            {
                temp = new GraphicsPath();

                foreach (var gridTile in TileMap.GetVisibleGridTiles())
                {
                    if (!Selection.Contains(gridTile))
                    {
                        continue;
                    }

                    var viewTile = TileMap.GetViewTile(gridTile);
                    var adjacentGridTiles = GetAdjacentGridTiles(gridTile);
                    var cornerPixels = GetCornerPixels(viewTile);
                    for (var i = adjacentGridTiles.Length; --i >= 0;)
                    {
                        if (IsSelectedGridTile(adjacentGridTiles[i]))
                        {
                            continue;
                        }

                        temp.StartFigure();
                        temp.AddLine(
                            cornerPixels[i],
                            cornerPixels[(i - 1) & 3]);
                    }
                }

                result = temp;
                temp = null;
                return result;
            }
            finally
            {
                temp?.Dispose();
            }

            Point[] GetAdjacentGridTiles(Point viewTile)
            {
                var x = viewTile.X;
                var y = viewTile.Y;
                return new Point[]
                {
                    new Point(x - 1, y),
                    new Point(x, y - 1),
                    new Point(x + 1, y),
                    new Point(x, y + 1),
                };
            }

            Point[] GetCornerPixels(Point viewTile)
            {
                var x = viewTile.X;
                var y = viewTile.Y;
                var clips = new int[]
                {
                    x * TileMap.TileSize.Width,
                    y * TileMap.TileSize.Height,
                    ((x + 1) * TileMap.TileSize.Width) - 1,
                    ((y + 1) * TileMap.TileSize.Height) - 1,
                };

                return new Point[4]
                {
                    new Point(clips[0], clips[1]),
                    new Point(clips[2], clips[1]),
                    new Point(clips[2], clips[3]),
                    new Point(clips[0], clips[3]),
                };
            }

            bool IsSelectedGridTile(Point gridTile)
            {
                return Selection.Contains(gridTile)
                    && TileMap.GridContainsGridTile(gridTile);
            }
        }

        public void Draw(Graphics graphics)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (PathRenderer is null || Selection is null)
            {
                return;
            }

            using (var graphicsPath = RenderPath())
            {
                PathRenderer.DrawPath(graphics, graphicsPath);
            }
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TileMap?.Dispose();
                PathRenderer?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
