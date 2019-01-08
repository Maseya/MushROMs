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

    public abstract class GfxTileConverter
    {
        private const int DotsPerPlane = GfxTile.DotsPerPlane;
        private const int PlanesPerTile = GfxTile.PlanesPerTile;
        private const int DotsPerTile = GfxTile.DotsPerTile;

        public static readonly GfxTileConverter Format1Bpp8x8 =
            new Format1BppTileConverter();

        public static readonly GfxTileConverter Format2BppNes =
            new Format2BppNesTileConverter();

        public static readonly GfxTileConverter Format2BppGb =
            new Format2BppGbTileConverter();

        public static readonly GfxTileConverter Format2BppNgp =
            new Format2BppNgpTileConverter();

        public static readonly GfxTileConverter Format2BppVb =
            new Format2BppVbTileConverter();

        public static readonly GfxTileConverter Format3BppSnes =
            new Format3BppSnesTileConverter();

        public static readonly GfxTileConverter Format3Bpp8x8 =
            new Format3Bpp8x8TileConverter();

        public static readonly GfxTileConverter Format4BppSnes =
            new Format4BppSnesTileConverter();

        public static readonly GfxTileConverter Format4BppGba =
            new Format4BppGbaTileConverter();

        public static readonly GfxTileConverter Format4BppSms =
            new Format4BppSmsTileConverter();

        public static readonly GfxTileConverter Format4BppMsx2 =
            new Format4BppMsx2TileConverter();

        public static readonly GfxTileConverter Format4Bpp8x8 =
            new Format4Bpp8x8TileConverter();

        public static readonly GfxTileConverter Format8BppSnes =
            new Format8BppSnesTileConverter();

        public static readonly GfxTileConverter Format8BppMode7 =
            new Format8BppMode7TileConverter();

        private static readonly Dictionary<GraphicsFormat, GfxTileConverter>
            Converters = new Dictionary<GraphicsFormat, GfxTileConverter>()
            {
                { GraphicsFormat.Format1Bpp8x8, Format1Bpp8x8 },
                { GraphicsFormat.Format2BppNes, Format2BppNes },
                { GraphicsFormat.Format2BppGb, Format2BppGb },
                { GraphicsFormat.Format2BppNgp, Format2BppNgp },
                { GraphicsFormat.Format2BppVb, Format2BppVb },
                { GraphicsFormat.Format3BppSnes, Format3BppSnes },
                { GraphicsFormat.Format3Bpp8x8, Format3Bpp8x8 },
                { GraphicsFormat.Format4BppSnes, Format4BppSnes },
                { GraphicsFormat.Format4BppGba, Format4BppGba },
                { GraphicsFormat.Format4BppSms, Format4BppSms },
                { GraphicsFormat.Format4BppMsx2, Format4BppMsx2 },
                { GraphicsFormat.Format4Bpp8x8, Format4Bpp8x8 },
                { GraphicsFormat.Format8BppSnes, Format8BppSnes },
                { GraphicsFormat.Format8BppMode7, Format8BppMode7 },
            };

        public abstract byte ReadPixel(
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex);

        public abstract void WritePixel(
            byte pixel,
            IList<byte> bytes,
            int byteIndex,
            int pixelIndex);

        public abstract IEnumerable<byte> GetPixels(IEnumerable<byte> bytes);

        public abstract IEnumerable<byte> GetBytes(IEnumerable<byte> pixels);

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

        public static void Write(
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

        public static IEnumerable<byte> GetPixels(
            IEnumerable<byte> bytes,
            GraphicsFormat format)
        {
            return GetTileConverter(format).GetPixels(bytes);
        }

        public static IEnumerable<byte> GetBytes(
            IEnumerable<byte> pixels,
            GraphicsFormat format)
        {
            return GetTileConverter(format).GetBytes(pixels);
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

        private class Format1BppTileConverter : GfxTileConverter
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

            public override IEnumerable<byte> GetPixels(
                IEnumerable<byte> bytes)
            {
                var planeCount = 0;
                using (var en = bytes.GetEnumerator())
                {
                    while (en.MoveNext() && planeCount++ < PlanesPerTile)
                    {
                        var value = en.Current;
                        for (var x = DotsPerPlane; --x >= 0;)
                        {
                            yield return ReadBit(value, x);
                        }
                    }
                }

                if (planeCount != PlanesPerTile)
                {
                    throw new ArgumentException();
                }
            }

            public override IEnumerable<byte> GetBytes(
                IEnumerable<byte> pixels)
            {
                using (var en = pixels.GetEnumerator())
                {
                    for (var y = 0; y < PlanesPerTile; y++)
                    {
                        var x = DotsPerPlane;
                        var value = 0;
                        while (en.MoveNext() && --x >= DotsPerPlane)
                        {
                            value = WriteBit(value, x, en.Current & 1);
                        }

                        if (x != 0)
                        {
                            throw new ArgumentException();
                        }

                        yield return (byte)value;
                    }
                }
            }
        }

        private class Format2BppNesTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                var index1 = byteIndex + (pixelIndex >> 3);
                var index2 = index1 + PlanesPerTile;
                var value1 = ReadBit(bytes[index1], ~pixelIndex);
                var value2 = ReadBit(bytes[index2], ~pixelIndex);
                return (byte)(value1 | (value2 << 1));
            }

            public override void WritePixel(
                byte pixel,
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                var index1 = byteIndex + (pixelIndex >> 3);
                var index2 = index1 + PlanesPerTile;
                bytes[index1] = WriteBit(
                    bytes[index1],
                    ~pixelIndex,
                    pixel & 1);

                bytes[index2] = WriteBit(
                    bytes[index2],
                    ~pixelIndex,
                    pixel & 2);
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format2BppGbTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format2BppNgpTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format2BppVbTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format3BppSnesTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format3Bpp8x8TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format4BppSnesTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format4BppGbaTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format4BppSmsTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format4BppMsx2TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format4Bpp8x8TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format8BppSnesTileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override void WritePixel(byte pixel, IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetPixels(IEnumerable<byte> bytes)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<byte> GetBytes(IEnumerable<byte> pixels)
            {
                throw new NotImplementedException();
            }
        }

        private class Format8BppMode7TileConverter : GfxTileConverter
        {
            public override byte ReadPixel(IList<byte> bytes, int byteIndex, int pixelIndex)
            {
                return bytes[byteIndex + pixelIndex];
            }

            public override void WritePixel(
                byte pixel,
                IList<byte> bytes,
                int byteIndex,
                int pixelIndex)
            {
                bytes[byteIndex + pixelIndex] = pixel;
            }

            public override IEnumerable<byte> GetPixels(
                IEnumerable<byte> bytes)
            {
                var dotCount = 0;
                using (var en = bytes.GetEnumerator())
                {
                    while (en.MoveNext() && dotCount++ < DotsPerTile)
                    {
                        yield return en.Current;
                    }
                }

                if (dotCount != DotsPerTile)
                {
                    throw new ArgumentException();
                }
            }

            public override IEnumerable<byte> GetBytes(
                IEnumerable<byte> pixels)
            {
                var dotCount = 0;
                using (var en = pixels.GetEnumerator())
                {
                    while (en.MoveNext() && dotCount++ < DotsPerTile)
                    {
                        yield return en.Current;
                    }
                }

                if (dotCount != DotsPerTile)
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}
