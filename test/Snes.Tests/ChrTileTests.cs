// <copyright file="ChrTileTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class ChrTileTests
{
    [Theory]
    [InlineData(0x000, 0, LayerPriority.Priority0, TileFlip.None, 0x0000)]
    [InlineData(0x1FF, 7, LayerPriority.Priority3, TileFlip.Both, 0xFFFF)]
    [InlineData(0x000, 0, LayerPriority.Priority0, TileFlip.Veritcal, 0x8000)]
    [InlineData(0x1FF, 7, LayerPriority.Priority3, TileFlip.Horizontal, 0x7FFF)]
    [InlineData(0x12C, 5, LayerPriority.Priority2, TileFlip.Horizontal, 0x6B2C)]
    [InlineData(0x03C, 5, LayerPriority.Priority1, TileFlip.Horizontal, 0x5A3C)]
    public void Constructor_PacksTileAttributesIntoValue(
        int tileIndex,
        int paletteIndex,
        LayerPriority priority,
        TileFlip tileFlip,
        int expectedValue)
    {
        var tile = new ChrTile(
            tileIndex,
            paletteIndex,
            priority,
            tileFlip);

        Assert.Equal(expectedValue, tile.Value);
        Assert.Equal(tileIndex, tile.TileIndex);
        Assert.Equal(paletteIndex, tile.PaletteIndex);
        Assert.Equal(priority, tile.Priority);
        Assert.Equal(tileFlip, tile.TileFlip);
    }

    [Theory]
    [InlineData(0x1AB, 0x1AB)]
    [InlineData(0x3AB, 0x1AB)]
    [InlineData(0x200, 0x000)]
    public void TileIndexSetter_UpdatesOnlyTileIndex(
        int value,
        int expectedTileIndex)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            TileFlip.Both)
        {
            TileIndex = value
        };

        Assert.Equal(expectedTileIndex, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority2, tile.Priority);
        Assert.Equal(TileFlip.Both, tile.TileFlip);
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(0xB, 3)]
    [InlineData(0, 0)]
    public void PaletteIndexSetter_UpdatesOnlyPaletteIndex(
        int value,
        int expectedPaletteIndex)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            TileFlip.Both)
        {
            PaletteIndex = value
        };

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(expectedPaletteIndex, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority2, tile.Priority);
        Assert.Equal(TileFlip.Both, tile.TileFlip);
    }

    [Theory]
    [InlineData(LayerPriority.Priority0)]
    [InlineData(LayerPriority.Priority1)]
    [InlineData(LayerPriority.Priority3)]
    public void PrioritySetter_UpdatesOnlyPriority(LayerPriority value)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            TileFlip.Both)
        {
            Priority = value
        };

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(value, tile.Priority);
        Assert.Equal(TileFlip.Both, tile.TileFlip);
    }

    [Theory]
    [InlineData(TileFlip.None)]
    [InlineData(TileFlip.Horizontal)]
    [InlineData(TileFlip.Veritcal)]
    [InlineData(TileFlip.Both)]
    public void TileFlipSetter_UpdatesOnlyTileFlip(TileFlip value)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            TileFlip.None)
        {
            TileFlip = value
        };

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority2, tile.Priority);
        Assert.Equal(value, tile.TileFlip);
    }

    [Theory]
    [InlineData(TileFlip.None, true, TileFlip.Horizontal)]
    [InlineData(TileFlip.Horizontal, false, TileFlip.None)]
    [InlineData(TileFlip.Veritcal, false, TileFlip.Veritcal)]
    [InlineData(TileFlip.Both, false, TileFlip.Veritcal)]
    [InlineData(TileFlip.Veritcal, true, TileFlip.Both)]
    public void XFlippedSetter_UpdatesOnlyHorizontalFlip(
        TileFlip initialFlip,
        bool value,
        TileFlip expectedFlip)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            initialFlip)
        {
            XFlipped = value
        };

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority2, tile.Priority);
        Assert.Equal(expectedFlip, tile.TileFlip);
    }

    [Theory]
    [InlineData(TileFlip.None, true, TileFlip.Veritcal)]
    [InlineData(TileFlip.Veritcal, false, TileFlip.None)]
    [InlineData(TileFlip.Horizontal, false, TileFlip.Horizontal)]
    [InlineData(TileFlip.Both, false, TileFlip.Horizontal)]
    [InlineData(TileFlip.Horizontal, true, TileFlip.Both)]
    public void YFlippedSetter_UpdatesOnlyVerticalFlip(
        TileFlip initialFlip,
        bool value,
        TileFlip expectedFlip)
    {
        var tile = new ChrTile(
            0x12C,
            5,
            LayerPriority.Priority2,
            initialFlip)
        {
            YFlipped = value
        };

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority2, tile.Priority);
        Assert.Equal(expectedFlip, tile.TileFlip);
    }

    [Theory]
    [InlineData(0x0000)]
    [InlineData(0xFFFF)]
    [InlineData(0x8000)]
    [InlineData(0x7FFF)]
    [InlineData(0x1234)]
    [InlineData(0xABCD)]
    [InlineData(0x5A3C)]
    public void IntegerConversion_PreservesValue(int value)
    {
        ChrTile tile = value;

        int convertedValue = tile;
        ChrTile convertedTile = convertedValue;

        Assert.Equal(value, tile.Value);
        Assert.Equal(value, convertedValue);
        Assert.Equal(tile, convertedTile);
    }

    [Theory]
    [InlineData(0x16B2C, 0x6B2C)]
    [InlineData(0x10000, 0x0000)]
    [InlineData(0x1FFFF, 0xFFFF)]
    public void IntegerConversion_TruncatesToSixteenBits(
        int value,
        int expectedValue)
    {
        ChrTile tile = value;

        Assert.Equal(expectedValue, tile.Value);
    }

    [Theory]
    [InlineData(0x0000)]
    [InlineData(0xFFFF)]
    [InlineData(0x8000)]
    [InlineData(0x7FFF)]
    [InlineData(0x1234)]
    [InlineData(0xABCD)]
    public void Equality_UsesValue(int value)
    {
        ChrTile first = value;
        ChrTile sameValue = first.Value;
        ChrTile differentValue = first.Value ^ 0x4000;

        Assert.Equal(first, sameValue);
        Assert.True(first == sameValue);
        Assert.NotEqual(first, differentValue);
        Assert.True(first != differentValue);
        Assert.Equal(first.GetHashCode(), sameValue.GetHashCode());
    }

    [Theory]
    [InlineData(0x0000, "0000")]
    [InlineData(0xFFFF, "FFFF")]
    [InlineData(0x8000, "8000")]
    [InlineData(0x7FFF, "7FFF")]
    [InlineData(0x1234, "1234")]
    [InlineData(0xABCD, "ABCD")]
    [InlineData(0x5A3C, "5A3C")]
    public void ToString_FormatsValueAsFourDigitHexadecimal(
        int value,
        string expected)
    {
        ChrTile tile = value;

        Assert.Equal(expected, tile.ToString());
    }
}
