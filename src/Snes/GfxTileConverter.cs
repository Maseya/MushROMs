// <copyright file="GfxTileConverter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using static GraphicsFormat;

    public abstract unsafe class GfxTileConverter
    {
        public static readonly GfxTileConverter Convert1Bpp8x8 =
            new Format1Bpp8x8TileConverter();

        public static readonly GfxTileConverter Convert4BppSnes =
            new Format4BppSnesTileConverter();

        public static readonly GfxTileConverter Convert8BppMode7 =
            new Format8BppMode7TileConverter();

        private const int PixelsPerPlane = GfxTile.PixelsPerPlane;
        private const int PlanesPerTile = GfxTile.PlanesPerTile;
        private const int PixelsPerTile = GfxTile.PixelsPerTile;

        private static readonly Dictionary<GraphicsFormat, GfxTileConverter>
            Converters = new Dictionary<GraphicsFormat, GfxTileConverter>()
            {
                { Format1Bpp8x8, Convert1Bpp8x8 },
                { Format4BppSnes, Convert4BppSnes },
                { Format8BppMode7, Convert8BppMode7 },
            };

        public static GfxTileConverter GetTileConverter(
            GraphicsFormat format)
        {
            if (Converters.TryGetValue(format, out var converter))
            {
                return converter;
            }

            throw new InvalidEnumArgumentException(
                nameof(format),
                (int)format,
                typeof(GraphicsFormat));
        }

        public static byte ReadPixel(
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex,
            GraphicsFormat format)
        {
            return GetTileConverter(format).ReadPixel(
                bytes,
                byteIndex,
                pixelIndex);
        }

        public static void WritePixel(
            byte pixel,
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex,
            GraphicsFormat format)
        {
            GetTileConverter(format).WritePixel(
                pixel,
                bytes,
                byteIndex,
                pixelIndex);
        }

        public static GfxTile ReadTileFromFormattedBytes(
            IList<byte> bytes,
            int index,
            GraphicsFormat format)
        {
            var converter = GetTileConverter(format);
            return converter.ReadTileFromFormattedBytes(bytes, index);
        }

        public static void WriteTileToFormattedBytes(
            in GfxTile tile,
            IList<byte> bytes,
            int index,
            GraphicsFormat format)
        {
            var converter = GetTileConverter(format);
            converter.WriteTileToFormattedBytes(in tile, bytes, index);
        }

        public static int BitsPerPixel(GraphicsFormat format)
        {
            if (!Enum.IsDefined(typeof(GraphicsFormat), (int)format))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(GraphicsFormat));
            }

            return (int)format & 0x0F;
        }

        public static int BytesPerPlane(GraphicsFormat format)
        {
            return BitsPerPixel(format);
        }

        public static int ColorsPerPixel(GraphicsFormat format)
        {
            return 1 << BitsPerPixel(format);
        }

        public static int BytesPerTile(GraphicsFormat format)
        {
            return BytesPerPlane(format) * PlanesPerTile;
        }

        public abstract byte ReadPixel(
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex);

        public abstract void WritePixel(
            byte pixel,
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex);

        public abstract GfxTile ReadTileFromFormattedBytes(
            IList<byte> bytes,
            int startIndex);

        public abstract void WriteTileToFormattedBytes(
            in GfxTile tile,
            IList<byte> bytes,
            int index);

        private static byte ReadBit(int value, int bit)
        {
            return (byte)((value >> (bit & 7)) & 1);
        }

        private static byte WriteBit(int value, int bit, int flag)
        {
            return flag != 0 ? SetBit(value, bit) : ClearBit(value, bit);
        }

        private static byte SetBit(int value, int bit)
        {
            return (byte)(value | (1 << (bit & 7)));
        }

        private static byte ClearBit(int value, int bit)
        {
            return (byte)(value & ~(1 << (bit & 7)));
        }

        private class Format1Bpp8x8TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                var index = byteIndex + ((pixelIndex >> 3) & 7);
                return ReadBit(bytes[index], ~pixelIndex);
            }

            public override void WritePixel(
                byte pixel,
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                var index = byteIndex + ((pixelIndex >> 3) & 7);
                bytes[index] = WriteBit(bytes[index], ~pixel, pixel & 1);
            }

            public override GfxTile ReadTileFromFormattedBytes(
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format1Bpp8x8) >= bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var result = default(GfxTile);
                var pixelIndex = 0;
                for (var y = 0; y < PlanesPerTile; y++)
                {
                    var value = bytes[index + y];

                    // We have to read the bits in reverse order. The lowest
                    // order bit is the rightmost pixel.
                    for (var x = PixelsPerPlane; --x >= 0;)
                    {
                        result.Pixels[pixelIndex++] = ReadBit(value, x);
                    }
                }

                return result;
            }

            public override void WriteTileToFormattedBytes(
                in GfxTile tile,
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format1Bpp8x8) >= bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var pixelIndex = 0;
                for (var y = 0; y < PlanesPerTile; y++)
                {
                    var value = (byte)0;
                    for (var x = PixelsPerPlane; --x >= 0;)
                    {
                        value = WriteBit(value, x, tile[pixelIndex++] & 1);
                    }

                    bytes[index + y] = value;
                }
            }
        }

        private class Format4BppSnesTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                if (bytes is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes));
                }

                if (byteIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(byteIndex));
                }

                if (byteIndex + BytesPerTile(Format4BppSnes) > bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (pixelIndex < 0 || pixelIndex >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(pixelIndex));
                }

                var y = pixelIndex / PixelsPerPlane;
                var x = pixelIndex % PixelsPerPlane;
                var offset = byteIndex + (y << 1);

                var val1 = bytes[offset + 0];
                var val2 = bytes[offset + 1];
                var val3 = bytes[offset + 0 + (2 * PlanesPerTile)];
                var val4 = bytes[offset + 1 + (2 * PlanesPerTile)];

                return (byte)(
                    (((val1 >> x) & 1) << 0) |
                    (((val2 >> x) & 1) << 1) |
                    (((val3 >> x) & 1) << 2) |
                    (((val4 >> x) & 1) << 3));
            }

            public override void WritePixel(
                byte pixel,
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                if (bytes is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes));
                }

                if (byteIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(byteIndex));
                }

                if (byteIndex + BytesPerTile(Format4BppSnes) > bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (pixelIndex < 0 || pixelIndex >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(pixelIndex));
                }

                var y = pixelIndex / PixelsPerPlane;
                var x = pixelIndex % PixelsPerPlane;
                var offset = byteIndex + (y << 1);

                var val1 = bytes[offset + 0] & ~(1 << x);
                var val2 = bytes[offset + 1] & ~(1 << x);
                var val3 = bytes[offset + 0 + (2 * PlanesPerTile)] & ~(1 << x);
                var val4 = bytes[offset + 1 + (2 * PlanesPerTile)] & ~(1 << x);

                val1 |= ((pixel >> 0) & 1) << x;
                val2 |= ((pixel >> 1) & 1) << x;
                val3 |= ((pixel >> 2) & 1) << x;
                val4 |= ((pixel >> 3) & 1) << x;

                bytes[offset + 0] = (byte)val1;
                bytes[offset + 1] = (byte)val2;
                bytes[offset + 0 + (2 * PlanesPerTile)] = (byte)val3;
                bytes[offset + 1 + (2 * PlanesPerTile)] = (byte)val4;
            }

            public override GfxTile ReadTileFromFormattedBytes(
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format4BppSnes) > bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var result = default(GfxTile);
                var pixelIndex = 0;
                for (var y = 0; y < PlanesPerTile; y++)
                {
                    var offset = index + (y << 1);

                    var val1 = bytes[offset + 0];
                    var val2 = bytes[offset + 1];
                    var val3 = bytes[offset + 0 + (2 * PlanesPerTile)];
                    var val4 = bytes[offset + 1 + (2 * PlanesPerTile)];

                    for (var x = PixelsPerPlane; --x >= 0;)
                    {
                        result[pixelIndex++] = (byte)(
                            (((val1 >> x) & 1) << 0) |
                            (((val2 >> x) & 1) << 1) |
                            (((val3 >> x) & 1) << 2) |
                            (((val4 >> x) & 1) << 3));
                    }
                }

                return result;
            }

            public override void WriteTileToFormattedBytes(
                in GfxTile tile,
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format4BppSnes) >= bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var pixelIndex = 0;
                for (var y = PlanesPerTile; --y >= 0;)
                {
                    var offset = index + (y << 1);

                    var val1 = 0;
                    var val2 = 0;
                    var val3 = 0;
                    var val4 = 0;
                    for (var x = PixelsPerPlane; --x >= 0;)
                    {
                        var value = tile.Pixels[pixelIndex++];

                        val1 |= ((value >> 0) & 1) << x;
                        val2 |= ((value >> 1) & 1) << x;
                        val3 |= ((value >> 2) & 1) << x;
                        val4 |= ((value >> 3) & 1) << x;
                    }

                    bytes[offset + 0] = (byte)val1;
                    bytes[offset + 1] = (byte)val2;
                    bytes[offset + 0 + (2 * PlanesPerTile)] = (byte)val3;
                    bytes[offset + 1 + (2 * PlanesPerTile)] = (byte)val4;
                }
            }
        }

        private class Format8BppMode7TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                if (bytes is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes));
                }

                if (byteIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(byteIndex));
                }

                if (byteIndex + BytesPerTile(Format4BppSnes) > bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (pixelIndex < 0 || pixelIndex >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(pixelIndex));
                }

                return bytes[byteIndex + pixelIndex];
            }

            public override void WritePixel(
                byte pixel,
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                if (bytes is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes));
                }

                if (byteIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(byteIndex));
                }

                if (byteIndex + BytesPerTile(Format4BppSnes) > bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (pixelIndex < 0 || pixelIndex >= PixelsPerTile)
                {
                    throw new ArgumentOutOfRangeException(nameof(pixelIndex));
                }

                bytes[byteIndex + pixelIndex] = pixel;
            }

            public override GfxTile ReadTileFromFormattedBytes(
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format8BppMode7) >= bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var result = default(GfxTile);
                for (var i = 0; i < PixelsPerTile; i++)
                {
                    result[i] = bytes[index + i];
                }

                return result;
            }

            public override void WriteTileToFormattedBytes(
                in GfxTile tile,
                IList<byte> bytes,
                int index)
            {
                if (bytes is null)
                {
                    throw new ArgumentNullException(nameof(bytes));
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (index + BytesPerTile(Format1Bpp8x8) >= bytes.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                for (var i = 0; i < PixelsPerTile; i++)
                {
                    bytes[index + i] = tile[i];
                }
            }
        }
    }
}
