// <copyright file="ObjTileTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class ObjTileTests
{
    [Fact]
    public void SizeOf_ReportsTwoByteEntry()
    {
        Assert.Equal(sizeof(ushort), ObjTile.SizeOf);
    }

    [Theory]
    [InlineData(0x0000, 0x000, 0, LayerPriority.Priority0, TileFlip.None)]
    [InlineData(0xFFFF, 0x3FF, 7, LayerPriority.Priority1, TileFlip.Both)]
    [InlineData(0xD234, 0x234, 4, LayerPriority.Priority0, TileFlip.Both)]
    [InlineData(0xF52C, 0x12C, 5, LayerPriority.Priority1, TileFlip.Both)]
    public void IntegerConversion_ExposesPackedTileAttributes(
        int value,
        int expectedTileIndex,
        int expectedPaletteIndex,
        LayerPriority expectedPriority,
        TileFlip expectedTileFlip)
    {
        ObjTile tile = value;

        Assert.Equal(value, tile.Value);
        Assert.Equal(value, (int)tile);
        Assert.Equal(expectedTileIndex, tile.TileIndex);
        Assert.Equal(expectedPaletteIndex, tile.PaletteIndex);
        Assert.Equal(expectedPriority, tile.Priority);
        Assert.Equal(expectedTileFlip, tile.TileFlip);
    }

    [Theory]
    [InlineData(0x16B2C, 0x6B2C)]
    [InlineData(0x10000, 0x0000)]
    [InlineData(0x1FFFF, 0xFFFF)]
    public void IntegerConversion_TruncatesToSixteenBits(
        int value,
        int expectedValue)
    {
        ObjTile tile = value;

        Assert.Equal(expectedValue, tile.Value);
    }

    [Fact]
    public void ValueSetter_TruncatesToSixteenBits()
    {
        ObjTile tile = 0xFFFF;
        tile.Value = 0x10001;

        Assert.Equal(0x0001, tile.Value);
    }

    [Theory]
    [InlineData(0x1AB, 0x1AB)]
    [InlineData(0x7AB, 0x3AB)]
    [InlineData(0x400, 0x000)]
    public void TileIndexSetter_UpdatesOnlyTileIndex(
        int value,
        int expectedTileIndex)
    {
        ObjTile tile = 0xF52C;
        tile.TileIndex = value;

        Assert.Equal(expectedTileIndex, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority1, tile.Priority);
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
        ObjTile tile = 0xF52C;
        tile.PaletteIndex = value;

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(expectedPaletteIndex, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority1, tile.Priority);
        Assert.Equal(TileFlip.Both, tile.TileFlip);
    }

    [Theory]
    [InlineData(LayerPriority.Priority0)]
    [InlineData(LayerPriority.Priority1)]
    [InlineData(LayerPriority.Priority2)]
    [InlineData(LayerPriority.Priority3)]
    public void PrioritySetter_UpdatesOnlyPriority(LayerPriority value)
    {
        ObjTile tile = 0xF52C;
        tile.Priority = value;

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal((LayerPriority)((int)value & 1), tile.Priority);
        Assert.Equal(TileFlip.Both, tile.TileFlip);
    }

    [Theory]
    [InlineData(TileFlip.None)]
    [InlineData(TileFlip.Horizontal)]
    [InlineData(TileFlip.Veritcal)]
    [InlineData(TileFlip.Both)]
    public void TileFlipSetter_UpdatesOnlyTileFlip(TileFlip value)
    {
        ObjTile tile = 0x352C;
        tile.TileFlip = value;

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority1, tile.Priority);
        Assert.Equal(value, tile.TileFlip);
    }

    [Theory]
    [InlineData(TileFlip.None, true, TileFlip.Horizontal)]
    [InlineData(TileFlip.Horizontal, false, TileFlip.None)]
    [InlineData(TileFlip.Veritcal, false, TileFlip.Veritcal)]
    [InlineData(TileFlip.Both, false, TileFlip.Veritcal)]
    [InlineData(TileFlip.Veritcal, true, TileFlip.Both)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Test readability")]
    public void XFlippedSetter_UpdatesOnlyHorizontalFlip(
        TileFlip initialFlip,
        bool value,
        TileFlip expectedFlip)
    {
        var tile = new ObjTile
        {
            TileIndex = 0x12C,
            PaletteIndex = 5,
            Priority = LayerPriority.Priority1,
            TileFlip = initialFlip,
        };

        tile.XFlipped = value;

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority1, tile.Priority);
        Assert.Equal(expectedFlip, tile.TileFlip);
        Assert.Equal(
            (expectedFlip & TileFlip.Horizontal) != 0,
            tile.XFlipped);
    }

    [Theory]
    [InlineData(TileFlip.None, true, TileFlip.Veritcal)]
    [InlineData(TileFlip.Veritcal, false, TileFlip.None)]
    [InlineData(TileFlip.Horizontal, false, TileFlip.Horizontal)]
    [InlineData(TileFlip.Both, false, TileFlip.Horizontal)]
    [InlineData(TileFlip.Horizontal, true, TileFlip.Both)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Test readability")]
    public void YFlippedSetter_UpdatesOnlyVerticalFlip(
        TileFlip initialFlip,
        bool value,
        TileFlip expectedFlip)
    {
        var tile = new ObjTile
        {
            TileIndex = 0x12C,
            PaletteIndex = 5,
            Priority = LayerPriority.Priority1,
            TileFlip = initialFlip,
        };

        tile.YFlipped = value;

        Assert.Equal(0x12C, tile.TileIndex);
        Assert.Equal(5, tile.PaletteIndex);
        Assert.Equal(LayerPriority.Priority1, tile.Priority);
        Assert.Equal(expectedFlip, tile.TileFlip);
        Assert.Equal(
            (expectedFlip & TileFlip.Veritcal) != 0,
            tile.YFlipped);
    }

    [Theory]
    [InlineData(0x352C, 0x752C)]
    [InlineData(0x752C, 0x352C)]
    public void FlipX_ReturnsCopyWithHorizontalFlipToggled(
        int initialValue,
        int expectedValue)
    {
        ObjTile tile = initialValue;

        var flipped = tile.FlipX();

        Assert.Equal(initialValue, tile.Value);
        Assert.Equal(expectedValue, flipped.Value);
    }

    [Theory]
    [InlineData(0x352C, 0xB52C)]
    [InlineData(0xB52C, 0x352C)]
    public void FlipY_ReturnsCopyWithVerticalFlipToggled(
        int initialValue,
        int expectedValue)
    {
        ObjTile tile = initialValue;

        var flipped = tile.FlipY();

        Assert.Equal(initialValue, tile.Value);
        Assert.Equal(expectedValue, flipped.Value);
    }

    [Fact]
    public void Equality_AndHashCode_AgreeForCopies()
    {
        ObjTile first = 0x1234;
        var copy = first;

        Assert.True(first.Equals(copy));
        Assert.True(first == copy);
        Assert.False(first != copy);
        Assert.Equal(first.GetHashCode(), copy.GetHashCode());
    }

    [Fact]
    public void Equals_ObjectRejectsOtherTypes()
    {
        ObjTile tile = 0x1234;

        Assert.False(tile.Equals("1234"));
        Assert.False(tile.Equals(null));
    }

    [Theory]
    [InlineData(0x0000, "0000")]
    [InlineData(0xFFFF, "FFFF")]
    [InlineData(0x8000, "8000")]
    [InlineData(0x7FFF, "7FFF")]
    [InlineData(0x1234, "1234")]
    [InlineData(0xABCD, "ABCD")]
    public void ToString_FormatsValueAsFourDigitHexadecimal(
        int value,
        string expected)
    {
        ObjTile tile = value;

        Assert.Equal(expected, tile.ToString());
    }
}
