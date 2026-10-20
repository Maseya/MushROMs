// <copyright file="Obj16TileTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class Obj16TileTests
{
    [Fact]
    public void Constants_ReportTileCountIndicesAndSize()
    {
        Assert.Equal(4, Obj16Tile.NumberOfTiles);
        Assert.Equal(0, Obj16Tile.TopLeftIndex);
        Assert.Equal(1, Obj16Tile.TopRightIndex);
        Assert.Equal(2, Obj16Tile.BottomLeftIndex);
        Assert.Equal(3, Obj16Tile.BottomRightIndex);
        Assert.Equal(Obj16Tile.NumberOfTiles * ObjTile.SizeOf, Obj16Tile.SizeOf);
    }

    [Fact]
    public void Constructor_StoresEachTileInItsNamedPosition()
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        Assert.Equal((ObjTile)0x0001, tile.TopLeft);
        Assert.Equal((ObjTile)0x0002, tile.TopRight);
        Assert.Equal((ObjTile)0x0003, tile.BottomLeft);
        Assert.Equal((ObjTile)0x0004, tile.BottomRight);
    }

    [Theory]
    [InlineData(Obj16Tile.TopLeftIndex, 0x0001)]
    [InlineData(Obj16Tile.TopRightIndex, 0x0002)]
    [InlineData(Obj16Tile.BottomLeftIndex, 0x0003)]
    [InlineData(Obj16Tile.BottomRightIndex, 0x0004)]
    public void Indexer_ReturnsTileAtRowMajorIndex(int index, int expectedValue)
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        Assert.Equal((ObjTile)expectedValue, tile[index]);
    }

    [Theory]
    [InlineData(Obj16Tile.TopLeftIndex, 0x0010, 0x0010, 0x0003, 0x0002, 0x0004)]
    [InlineData(Obj16Tile.TopRightIndex, 0x0020, 0x0001, 0x0003, 0x0020, 0x0004)]
    [InlineData(Obj16Tile.BottomLeftIndex, 0x0030, 0x0001, 0x0030, 0x0002, 0x0004)]
    [InlineData(Obj16Tile.BottomRightIndex, 0x0040, 0x0001, 0x0003, 0x0002, 0x0040)]
    public void IndexerSetter_UpdatesTileAtRowMajorIndex(
        int index,
        int value,
        int expectedTopLeft,
        int expectedBottomLeft,
        int expectedTopRight,
        int expectedBottomRight)
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        tile[index] = value;

        Assert.Equal((ObjTile)expectedTopLeft, tile.TopLeft);
        Assert.Equal((ObjTile)expectedBottomLeft, tile.BottomLeft);
        Assert.Equal((ObjTile)expectedTopRight, tile.TopRight);
        Assert.Equal((ObjTile)expectedBottomRight, tile.BottomRight);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(4)]
    public void Indexer_WithOutOfRangeIndex_ThrowsArgumentOutOfRangeException(
        int index)
    {
        Obj16Tile tile = default;

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => tile[index]);
        _ = Assert.Throws<ArgumentOutOfRangeException>(
            () => tile[index] = 0x0001);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    [InlineData(2, 0, 1)]
    [InlineData(3, 1, 1)]
    public void CoordinateMethods_ReturnRowMajorCoordinates(
        int index,
        int expectedX,
        int expectedY)
    {
        Assert.Equal(expectedX, Obj16Tile.GetXCoordinate(index));
        Assert.Equal(expectedY, Obj16Tile.GetYCoordinate(index));
    }

    [Fact]
    public void FlipX_ReturnsHorizontallyFlippedCopy()
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        var flipped = tile.FlipX();

        Assert.Equal((ObjTile)0x0001, tile.TopLeft);
        Assert.Equal((ObjTile)0x0002, tile.TopRight);
        Assert.Equal((ObjTile)0x0003, tile.BottomLeft);
        Assert.Equal((ObjTile)0x0004, tile.BottomRight);
        Assert.Equal((ObjTile)0x4002, flipped.TopLeft);
        Assert.Equal((ObjTile)0x4001, flipped.TopRight);
        Assert.Equal((ObjTile)0x4004, flipped.BottomLeft);
        Assert.Equal((ObjTile)0x4003, flipped.BottomRight);
    }

    [Fact]
    public void FlipY_ReturnsVerticallyFlippedCopy()
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        var flipped = tile.FlipY();

        Assert.Equal((ObjTile)0x0001, tile.TopLeft);
        Assert.Equal((ObjTile)0x0002, tile.TopRight);
        Assert.Equal((ObjTile)0x0003, tile.BottomLeft);
        Assert.Equal((ObjTile)0x0004, tile.BottomRight);
        Assert.Equal((ObjTile)0x8003, flipped.TopLeft);
        Assert.Equal((ObjTile)0x8004, flipped.TopRight);
        Assert.Equal((ObjTile)0x8001, flipped.BottomLeft);
        Assert.Equal((ObjTile)0x8002, flipped.BottomRight);
    }

    [Fact]
    public void Equality_AndHashCode_AgreeForCopies()
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);
        var copy = tile;

        Assert.True(tile.Equals(copy));
        Assert.True(tile == copy);
        Assert.False(tile != copy);
        Assert.Equal(tile.GetHashCode(), copy.GetHashCode());
    }

    [Fact]
    public void Equals_ObjectRejectsOtherTypes()
    {
        Obj16Tile tile = default;

        Assert.False(tile.Equals("tile"));
        Assert.False(tile.Equals(null));
    }

    [Fact]
    public void ToString_FormatsTilesInPropertyOrder()
    {
        Obj16Tile tile = new(0x0001, 0x0002, 0x0003, 0x0004);

        Assert.Equal("0001-0002-0003-0004", tile.ToString());
    }
}
