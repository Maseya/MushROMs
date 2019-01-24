// <copyright file="TileMap1DRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Editors.TileMaps
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Maseya.Editors.Collections;
    using Maseya.Helper.PixelFormat;
    using static System.Math;

    public abstract class TileMap1DRenderer
    {
        private ITileMap1D _tileMap;

        public ITileMap1D TileMap
        {
            get
            {
                return _tileMap;
            }

            set
            {
                if (TileMap != null)
                {
                }

                _tileMap = value;
                if (TileMap != null)
                {
                }
            }
        }

        private int[] PixelIndexMap
        {
            get;
            set;
        }

        private Size TileSize
        {
            get
            {
                return TileMap.TileSize;
            }
        }

        private int TileWidth
        {
            get
            {
                return TileMap.TileSize.Width;
            }
        }

        private int TileHeight
        {
            get
            {
                return TileMap.TileSize.Height;
            }
        }

        private int TilePixelCount
        {
            get
            {
                return TileWidth * TileHeight;
            }
        }

        private int ViewWidth
        {
            get
            {
                return TileMap.ViewSize.Width;
            }
        }

        private int ViewHeight
        {
            get
            {
                return TileMap.ViewSize.Height;
            }
        }

        private int ViewTileCount
        {
            get
            {
                return ViewWidth * ViewHeight;
            }
        }

        private int Width
        {
            get
            {
                return TileMap.Size.Width;
            }
        }

        private int Height
        {
            get
            {
                return TileMap.Size.Height;
            }
        }

        private int ZeroTile
        {
            get
            {
                return TileMap.ZeroTile;
            }
        }

        public Color32BppArgb[] RenderPixels()
        {
            if (TileMap is null)
            {
                return null;
            }

            var pixels = new Color32BppArgb[PixelIndexMap.Length];
            var tileIndex = Max(ZeroTile, 0);
            var pixelCount = PixelIndexMap.Length;
            for (var pixelIndex = 0; pixelIndex < pixelCount;)
            {
                foreach (var pixel in GetTilePixels(tileIndex++))
                {
                    pixels[pixelIndex++] = pixel;
                }
            }

            return pixels;
        }

        public BoxListSelection SelectTilePixels(int tileIndex)
        {
            return new BoxListSelection(
                GetPixelIndex(tileIndex),
                TileSize,
                Width);
        }

        protected abstract IEnumerable<Color32BppArgb> GetTilePixels(
            int tileIndex);

        private void ResetPixelMap()
        {
            if (TileMap is null)
            {
                PixelIndexMap = null;
                return;
            }

            var result = new int[Width * Height];

            var i = 0;
            for (var tileIndex = 0; tileIndex < ViewTileCount; tileIndex++)
            {
                foreach (var pixelIndex in SelectTilePixels(tileIndex))
                {
                    result[i++] = pixelIndex;
                }
            }

            PixelIndexMap = result;
        }

        private int GetPixelIndex(int tileIndex)
        {
            var viewPoint = TileMap.GetViewTile(tileIndex + ZeroTile);
            return (viewPoint.Y * Width * TileHeight)
                + (viewPoint.X * TileWidth);
        }

        private void TileMap_SizeChanged(object sender, EventArgs e)
        {
            ResetPixelMap();
        }
    }
}
