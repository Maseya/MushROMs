// <copyright file="GfxTile.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Format = GraphicsFormat;

    public unsafe partial struct GfxTile
    {
        public const int PixelsPerPlane = 8;
        public const int PlanesPerTile = PixelsPerPlane;
        public const int PixelsPerTile = PixelsPerPlane * PlanesPerTile;
        public const int SizeOf = PixelsPerTile * sizeof(byte);

        [CLSCompliant(false)]
        public fixed byte Pixels[PixelsPerTile];

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return Pixels[index];
            }

            set
            {
                if (index < 0 || index >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Pixels[index] = value;
            }
        }

        public GfxTile FlipX()
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                for (var y = PlanesPerTile; --y >= 0;)
                {
                    var srcRow = src + (y * PixelsPerPlane);
                    var dstRow = dst + (y * PixelsPerPlane);

                    var i = 0;
                    for (var x = PixelsPerPlane / 2; --x >= 0; i++)
                    {
                        dstRow[i] = srcRow[x];
                        dstRow[x] = srcRow[i];
                    }
                }
            }

            return result;
        }

        public GfxTile FlipY()
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                for (var x = PixelsPerPlane; --x >= 0;)
                {
                    var srcPlane = src + x;
                    var dstPlane = dst + x;

                    const int HalfTileSize = PixelsPerTile / 2;

                    for (var i = 0; i < HalfTileSize; i += PixelsPerPlane)
                    {
                        var j = HalfTileSize - i - PixelsPerPlane;
                        dstPlane[i] = srcPlane[j];
                        dstPlane[j] = srcPlane[i];
                    }
                }
            }

            return result;
        }

        public GfxTile Rotate90()
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                var i = 0;
                var j = PixelsPerTile;
                var k = PlanesPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    k--;
                    j -= PixelsPerPlane;

                    var n = 0;
                    var m = PixelsPerTile;
                    var o = PixelsPerPlane;

                    for (var x = 0; x < PixelsPerPlane / 2; x++)
                    {
                        o--;
                        m -= PixelsPerPlane;

                        dst[i + x] = src[m + y];
                        dst[m + y] = src[j + o];
                        dst[j + o] = src[n + k];
                        dst[n + k] = src[i + x];

                        n += PixelsPerPlane;
                    }

                    i += PixelsPerPlane;
                }
            }

            return result;
        }

        public GfxTile Rotate180()
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                var i = 0;
                var j = PixelsPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    j -= PixelsPerPlane;

                    var n = 0;
                    var o = PixelsPerPlane;

                    for (var x = 0; x < PixelsPerPlane / 2; x++)
                    {
                        o--;

                        dst[j + o] = src[i + x];
                        dst[i + x] = src[j + o];

                        n += PixelsPerPlane;
                    }

                    i += PixelsPerPlane;
                }
            }

            return result;
        }

        public GfxTile Rotate270()
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                var i = 0;
                var j = PixelsPerTile;
                var k = PlanesPerTile;

                for (var y = 0; y < PlanesPerTile / 2; y++)
                {
                    k--;
                    j -= PixelsPerPlane;

                    var n = 0;
                    var m = PixelsPerTile;
                    var o = PixelsPerPlane;

                    for (var x = 0; x < PixelsPerPlane / 2; x++)
                    {
                        o--;
                        m -= PixelsPerPlane;

                        dst[m + y] = src[i + x];
                        dst[j + o] = src[m + y];
                        dst[n + k] = src[j + o];
                        dst[i + x] = src[n + k];

                        n += PixelsPerPlane;
                    }

                    i += PixelsPerPlane;
                }
            }

            return result;
        }

        public GfxTile ReplaceColor(byte original, byte replacement)
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                for (var i = PixelsPerTile; --i >= 0;)
                {
                    var value = src[i];

                    dst[i] = value == original ? replacement : value;
                }
            }

            return result;
        }

        public GfxTile SwapColors(byte color1, byte color2)
        {
            var result = default(GfxTile);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                for (var i = PixelsPerTile; --i >= 0;)
                {
                    var value = src[i];

                    dst[i] = value == color1
                        ? color2
                        : value == color2
                        ? color1
                        : value;
                }
            }

            return result;
        }

        public GfxTile RotateColors(byte first, byte last, byte shift)
        {
            var result = default(GfxTile);
            var length = (byte)(last - first + 1);

            fixed (byte* src = Pixels)
            {
                var dst = result.Pixels;

                for (var i = PixelsPerTile; --i >= 0;)
                {
                    var value = src[i];

                    if (value >= first && value <= last)
                    {
                        value -= first;
                        value += shift;
                        if (length != 0)
                        {
                            value %= length;
                        }

                        value += first;
                    }

                    dst[i] = value;
                }
            }

            return result;
        }
    }
}
