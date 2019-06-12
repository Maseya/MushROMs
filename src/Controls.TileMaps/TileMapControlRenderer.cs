// <copyright file="TileMapControlRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class TileMapControlRenderer : Component
    {
        private IImageRenderer _backgroundRender;

        private IImageRenderer _imageRenderer;

        private ViewTileRenderer _viewTileRenderer;

        private SelectionRenderer _selectionRenderer;

        private TileMapControl _tileMapControl;

        public TileMapControlRenderer()
        {
        }

        public TileMapControlRenderer(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }
        }

        public event EventHandler BackgroundRendererChanged;

        public event EventHandler ImageRendererChanged;

        public event EventHandler SelectionRendererChanged;

        public event EventHandler ViewTileRendererChanged;

        public event EventHandler ControlChanged;

        public IImageRenderer BackgroundRenderer
        {
            get
            {
                return _backgroundRender;
            }

            set
            {
                if (BackgroundRenderer == value)
                {
                    return;
                }

                if (BackgroundRenderer != null)
                {
                    BackgroundRenderer.Redraw += BackgroundRenderer_Redraw;
                }

                _backgroundRender = value;
                if (BackgroundRenderer != null)
                {
                    BackgroundRenderer.Redraw += BackgroundRenderer_Redraw;
                }

                OnBackgroundRendererChanged(EventArgs.Empty);
            }
        }

        public IImageRenderer ImageRenderer
        {
            get
            {
                return _imageRenderer;
            }

            set
            {
                if (ImageRenderer == value)
                {
                    return;
                }

                if (ImageRenderer != null)
                {
                    ImageRenderer.Redraw -= ForegroundRenderer_Redraw;
                }

                _imageRenderer = value;
                if (ImageRenderer != null)
                {
                    ImageRenderer.Redraw += ForegroundRenderer_Redraw;
                }

                OnImageRendererChanged(EventArgs.Empty);
            }
        }

        public SelectionRenderer SelectionRenderer
        {
            get
            {
                return _selectionRenderer;
            }

            set
            {
                if (SelectionRenderer == value)
                {
                    return;
                }

                if (SelectionRenderer != null)
                {
                    SelectionRenderer.Redraw -= ForegroundRenderer_Redraw;
                }

                _selectionRenderer = value;
                if (SelectionRenderer != null)
                {
                    SelectionRenderer.Redraw += ForegroundRenderer_Redraw;
                }

                OnSelectionRendererChanged(EventArgs.Empty);
            }
        }

        public ViewTileRenderer ViewTileRenderer
        {
            get
            {
                return _viewTileRenderer;
            }

            set
            {
                if (ViewTileRenderer == value)
                {
                    return;
                }

                if (ViewTileRenderer != null)
                {
                    ViewTileRenderer.Redraw -= ForegroundRenderer_Redraw;
                }

                _viewTileRenderer = value;
                if (ViewTileRenderer != null)
                {
                    ViewTileRenderer.Redraw += ForegroundRenderer_Redraw;
                }

                OnViewTileRendererChanged(EventArgs.Empty);
            }
        }

        public TileMapControl TileMapControl
        {
            get
            {
                return _tileMapControl;
            }

            set
            {
                if (TileMapControl == value)
                {
                    return;
                }

                if (TileMapControl != null)
                {
                    TileMapControl.Paint -= TileMapControl_Paint;
                }

                _tileMapControl = value;
                if (TileMapControl != null)
                {
                    TileMapControl.Paint += TileMapControl_Paint;
                }

                OnControlChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnBackgroundRendererChanged(EventArgs e)
        {
            SetBackground();
            BackgroundRendererChanged?.Invoke(this, e);
        }

        protected virtual void OnImageRendererChanged(EventArgs e)
        {
            ImageRendererChanged?.Invoke(this, e);
        }

        protected virtual void OnSelectionRendererChanged(EventArgs e)
        {
            SelectionRendererChanged?.Invoke(this, e);
        }

        protected virtual void OnViewTileRendererChanged(EventArgs e)
        {
            ViewTileRendererChanged?.Invoke(this, e);
        }

        protected virtual void OnControlChanged(EventArgs e)
        {
            SetBackground();
            ControlChanged?.Invoke(this, e);
        }

        private void SetBackground()
        {
            if (TileMapControl != null)
            {
                TileMapControl.BackgroundImage = BackgroundRenderer.
                    RenderImage();
            }
        }

        private void ForegroundRenderer_Redraw(object sender, EventArgs e)
        {
            TileMapControl?.Invalidate();
        }

        private void BackgroundRenderer_Redraw(object sender, EventArgs e)
        {
            SetBackground();
        }

        private void TileMapControl_Paint(object sender, PaintEventArgs e)
        {
            foreach (var renderer in GetRenderers())
            {
                renderer?.Draw(e.Graphics);
            }

            IGraphicsRenderer[] GetRenderers()
            {
                return new IGraphicsRenderer[]
                {
                    ImageRenderer,
                    ViewTileRenderer,
                    SelectionRenderer,
                };
            }
        }
    }
}
