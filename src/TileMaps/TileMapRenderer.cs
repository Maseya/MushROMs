// <copyright file="TileMapRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading.Tasks;
    using Maseya.Helper.PixelFormat;
    using static System.ComponentModel.DesignerSerializationVisibility;

    public abstract class TileMapRenderer : Component
    {
        private TileMap _tileMap;

        public TileMapRenderer()
        {
        }

        public TileMapRenderer(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }
        }

        public event EventHandler TileMapChanged;

        public TileMap TileMap
        {
            get
            {
                return _tileMap;
            }

            set
            {
                if (TileMap == value)
                {
                    return;
                }

                _tileMap = value;
                OnTileMapChanged(EventArgs.Empty);
            }
        }

        public TimeSpan Elapsed
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size TileSize
        {
            get
            {
                return TileMap.TileSize;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int TileWidth
        {
            get
            {
                return TileMap.TileSize.Width;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int TileHeight
        {
            get
            {
                return TileMap.TileSize.Height;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int TilePixelCount
        {
            get
            {
                return TileWidth * TileHeight;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ViewWidth
        {
            get
            {
                return TileMap.ViewSize.Width;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ViewHeight
        {
            get
            {
                return TileMap.ViewSize.Height;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ViewTileCount
        {
            get
            {
                return ViewWidth * ViewHeight;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int Width
        {
            get
            {
                return TileMap.Size.Width;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int Height
        {
            get
            {
                return TileMap.Size.Height;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size Size
        {
            get
            {
                return TileMap.Size;
            }
        }

        public IEnumerable<int> SelectTilePixels(Point gridTile)
        {
            if (TileMap is null)
            {
                throw new InvalidOperationException();
            }

            var startIndex = GetPixelIndex(gridTile);
            for (var y = 0; y < TileHeight; y++, startIndex += Width)
            {
                for (var x = 0; x < TileWidth; x++)
                {
                    yield return startIndex + x;
                }
            }
        }

        public abstract Color32BppArgb[] RenderPixels();

        protected virtual void OnTileMapChanged(EventArgs e)
        {
            TileMapChanged?.Invoke(this, e);
        }

        private int GetPixelIndex(Point viewTile)
        {
            return (viewTile.Y * Width * TileHeight)
                + (viewTile.X * TileWidth);
        }
    }
}
