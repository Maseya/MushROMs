// <copyright file="Color32BppArgbTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System.Drawing;
using System.Runtime.InteropServices;
using Xunit;

public class Color32BppArgbTests
{
    [Fact]
    public void Constructor_SetsArgbChannels()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        Assert.Equal(0x11, color.A);
        Assert.Equal(0x22, color.R);
        Assert.Equal(0x33, color.G);
        Assert.Equal(0x44, color.B);
    }

    [Fact]
    public void Constructor_TruncatesChannelValuesToEightBits()
    {
        var color = new Color32BppArgb(0x111, 0x222, 0x333, 0x444);

        Assert.Equal(0x11, color.A);
        Assert.Equal(0x22, color.R);
        Assert.Equal(0x33, color.G);
        Assert.Equal(0x44, color.B);
    }

    [Fact]
    public void Properties_CanUpdateChannels()
    {
        var color = new Color32BppArgb(0, 0, 0, 0)
        {
            A = 0x11,
            R = 0x22,
            G = 0x33,
            B = 0x44,
        };

        Assert.Equal(new Color32BppArgb(0x11, 0x22, 0x33, 0x44), color);
    }

    [Fact]
    public void Value_ReturnsPackedArgbValue()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        Assert.Equal(unchecked((int)0x11223344), color.Value);
    }

    [Fact]
    public void FromSnesColor_ExpandsChannelsAndSetsOpaqueAlpha()
    {
        var color = Color32BppArgb.FromSnesColor(0x061F);

        Assert.Equal(new Color32BppArgb(0xFF, 0xF8, 0x80, 0x08), color);
    }

    [Fact]
    public void FromSnesColor_CombinesLowAndHighBytes()
    {
        var color = Color32BppArgb.FromSnesColor(0x1F, 0x06);

        Assert.Equal(Color32BppArgb.FromSnesColor(0x061F), color);
    }

    [Fact]
    public void ToSnesColor_PacksFiveBitRgbChannelsAndIgnoresAlpha()
    {
        var color = new Color32BppArgb(0x11, 0xFF, 0x80, 0x08);

        Assert.Equal(0x061F, Color32BppArgb.ToSnesColor(color));
    }

    [Fact]
    public void EqualityOperators_CompareAllChannels()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var equalColor = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var differentColor = new Color32BppArgb(0xFF, 0x22, 0x33, 0x44);

        Assert.True(color == equalColor);
        Assert.False(color != equalColor);
        Assert.False(color == differentColor);
        Assert.True(color != differentColor);
    }

    [Fact]
    public void ImplicitIntegerConversions_PreservePackedArgbValue()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        int value = color;
        Color32BppArgb convertedColor = value;

        Assert.Equal(unchecked((int)0x11223344), value);
        Assert.Equal(color, convertedColor);
    }

    [Fact]
    public void ColorConversions_PreserveArgbChannels()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        Color systemColor = color;
        var convertedColor = (Color32BppArgb)systemColor;

        Assert.Equal(Color.FromArgb(0x11, 0x22, 0x33, 0x44), systemColor);
        Assert.Equal(color, convertedColor);
    }

    [Fact]
    public void Equals_CompareChannelValues()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var equalColor = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        Assert.True(color.Equals(equalColor));
        Assert.True(color.Equals((object)equalColor));
        Assert.False(color.Equals((object?)null));
        Assert.False(color.Equals("not a color"));
    }

    [Theory]
    [InlineData(0x11223344, "11223344")]
    [InlineData(0x00000000, "000000")]
    public void ToString_ReturnsUppercasePaddedHexValue(int value, string expected)
    {
        Color32BppArgb color = value;

        Assert.Equal(expected, color.ToString());
    }
}
