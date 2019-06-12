// <copyright file="TileMapImageRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls.TileMaps
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using Maseya.Controls;
    using Maseya.Helper.PixelFormat;
    using Maseya.TileMaps;

    public class TileMapImageRenderer : Component, IImageRenderer
    {
        private TileMapRenderer _tileMapRenderer;

        public TileMapImageRenderer()
        {
        }

        public TileMapImageRenderer(IContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler Redraw;

        public event EventHandler TileMapRendererChanged;

        public TileMapRenderer TileMapRenderer
        {
            get
            {
                return _tileMapRenderer;
            }

            set
            {
                if (TileMapRenderer == value)
                {
                    return;
                }

                _tileMapRenderer = value;
                OnTileMaprendererChanged(EventArgs.Empty);
            }
        }

        public Bitmap RenderImage()
        {
            if (TileMapRenderer?.TileMap is null)
            {
                return null;
            }

            var pixels = TileMapRenderer.RenderPixels();
            if (pixels.Length == 0)
            {
                return null;
            }

            var imageWidth = TileMapRenderer.Width;
            var imageHeight = pixels.Length / imageWidth;
            var imageSize = new Size(imageWidth, imageHeight);

            Bitmap temp = null;
            Bitmap result = null;
            try
            {
                temp = new Bitmap(
                    imageWidth,
                    imageHeight,
                    PixelFormat.Format32bppArgb);

                var bitmapData = temp.LockBits(
                    new Rectangle(Point.Empty, imageSize),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);

                unsafe
                {
                    fixed (Color32BppArgb* src = pixels)
                    {
                        var dest = (Color32BppArgb*)bitmapData.Scan0;
                        for (var i = 0; i < pixels.Length; i++)
                        {
                            dest[i] = src[i];
                        }
                    }
                }

                temp.UnlockBits(bitmapData);

                result = temp;
                temp = null;
                return result;
            }
            finally
            {
                temp?.Dispose();
            }
        }

        Image IImageRenderer.RenderImage()
        {
            return RenderImage();
        }

        public void Draw(Graphics graphics)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (TileMapRenderer is null)
            {
                return;
            }

            var pixels = TileMapRenderer.RenderPixels();
            if (pixels.Length == 0)
            {
                return;
            }

            var imageWidth = TileMapRenderer.Width;
            var imageHeight = pixels.Length / imageWidth;
            unsafe
            {
                fixed (Color32BppArgb* scan0 = pixels)
                {
                    var image = new Bitmap(
                        imageWidth,
                        imageHeight,
                        imageWidth * Color32BppArgb.SizeOf,
                        PixelFormat.Format32bppArgb,
                        (IntPtr)scan0);

                    graphics.DrawImageUnscaled(image, Point.Empty);
                }
            }
        }

        protected virtual void OnTileMaprendererChanged(EventArgs e)
        {
            TileMapRendererChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnRedraw(EventArgs e)
        {
            Redraw?.Invoke(this, e);
        }
    }
}
