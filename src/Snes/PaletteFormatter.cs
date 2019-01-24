﻿// <copyright file="PaletteFormatter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes
{
    using System;
    using System.Collections.Generic;
    using Maseya.Editors;
    using Maseya.Helper.PixelFormat;

    public abstract class PaletteFormatter : IDataFormatter<Color15BppBgr>
    {
        public static readonly PaletteFormatter Rpf =
            new RpfFormatter();

        public static readonly PaletteFormatter Tpl =
            new TplFormatter();

        public static readonly PaletteFormatter Pal =
            new PalFormatter();

        public abstract IEnumerable<Color15BppBgr> ToFormattedData(
            IEnumerable<byte> byteData);

        public abstract IEnumerable<byte> ToByteData(
            IEnumerable<Color15BppBgr> formattedData);

        private class RpfFormatter : PaletteFormatter
        {
            public override IEnumerable<Color15BppBgr> ToFormattedData(
                IEnumerable<byte> byteData)
            {
                if (byteData is null)
                {
                    throw new ArgumentNullException(nameof(byteData));
                }

                using (var en = byteData.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        var low = en.Current;
                        if (!en.MoveNext())
                        {
                            throw new ArgumentException();
                        }

                        var high = en.Current;
                        yield return new Color15BppBgr(low, high);
                    }
                }
            }

            public override IEnumerable<byte> ToByteData(
                IEnumerable<Color15BppBgr> formattedData)
            {
                foreach (var color in formattedData)
                {
                    yield return color.Low;
                    yield return color.High;
                }
            }
        }

        private class TplFormatter : PaletteFormatter
        {
            private static readonly string TplHeader = "TPL\x02";

            public override IEnumerable<Color15BppBgr> ToFormattedData(
                IEnumerable<byte> byteData)
            {
                if (byteData is null)
                {
                    throw new ArgumentNullException(nameof(byteData));
                }

                using (var en = byteData.GetEnumerator())
                {
                    foreach (var code in TplHeader)
                    {
                        if (!en.MoveNext() || en.Current != (byte)code)
                        {
                            throw new ArgumentException();
                        }
                    }

                    while (en.MoveNext())
                    {
                        var low = en.Current;
                        if (!en.MoveNext())
                        {
                            throw new ArgumentException();
                        }

                        var high = en.Current;
                        yield return new Color15BppBgr(low, high);
                    }
                }
            }

            public override IEnumerable<byte> ToByteData(
                IEnumerable<Color15BppBgr> formattedData)
            {
                foreach (var code in TplHeader)
                {
                    yield return (byte)TplHeader[0];
                }

                foreach (var color in formattedData)
                {
                    yield return color.Low;
                    yield return color.High;
                }
            }
        }

        private class PalFormatter : PaletteFormatter
        {
            public override IEnumerable<Color15BppBgr> ToFormattedData(
                IEnumerable<byte> byteData)
            {
                if (byteData is null)
                {
                    throw new ArgumentNullException(nameof(byteData));
                }

                using (var en = byteData.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        var red = en.Current;
                        if (!en.MoveNext())
                        {
                            throw new ArgumentException();
                        }

                        var green = en.Current;
                        if (!en.MoveNext())
                        {
                            throw new ArgumentException();
                        }

                        var blue = en.Current;
                        yield return
                            (Color15BppBgr)new Color24BppRgb(red, green, blue);
                    }
                }
            }

            public override IEnumerable<byte> ToByteData(
                IEnumerable<Color15BppBgr> formattedData)
            {
                foreach (var color in formattedData)
                {
                    var color24 = (Color24BppRgb)color;
                    yield return color.Red;
                    yield return color.Green;
                    yield return color.Blue;
                }
            }
        }
    }
}
