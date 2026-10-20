// <copyright file="MathHelperTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using System;

using Xunit;

public class MathHelperTests
{
    [Theory]
    [InlineData(unchecked((int)0x80000000), 0x0)]
    [InlineData(-0x1, 0x0)]
    [InlineData(0x0, 0x0)]
    [InlineData(0x1, 0x1)]
    [InlineData(0x2, 0x2)]
    [InlineData(0x3, 0x2)]
    [InlineData(0x4, 0x4)]
    [InlineData(0x5, 0x4)]
    [InlineData(0x7, 0x4)]
    [InlineData(0x8, 0x8)]
    [InlineData(0x9, 0x8)]
    [InlineData(0xF, 0x8)]
    [InlineData(0x10, 0x10)]
    [InlineData(0x11, 0x10)]
    [InlineData(0x3FF, 0x200)]
    [InlineData(0x400, 0x400)]
    [InlineData(0x401, 0x400)]
    [InlineData(0x40000000, 0x40000000)]
    [InlineData(0x7FFFFFFF, 0x40000000)]
    public void BitFloor_ValidInput_ReturnsLargestPowerOfTwoLowerBound(
        int x,
        int expected)
    {
        var actual = MathHelper.BitFloor(x);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(unchecked((int)0x80000000), 0x0)]
    [InlineData(-0x1, 0x0)]
    [InlineData(0x0, 0x0)]
    [InlineData(0x1, 0x1)]
    [InlineData(0x2, 0x2)]
    [InlineData(0x3, 0x4)]
    [InlineData(0x4, 0x4)]
    [InlineData(0x5, 0x8)]
    [InlineData(0x7, 0x8)]
    [InlineData(0x8, 0x8)]
    [InlineData(0x9, 0x10)]
    [InlineData(0xF, 0x10)]
    [InlineData(0x10, 0x10)]
    [InlineData(0x11, 0x20)]
    [InlineData(0x3FF, 0x400)]
    [InlineData(0x400, 0x400)]
    [InlineData(0x401, 0x800)]
    [InlineData(0x40000000, 0x40000000)]
    public void BitCeil_ValidInput_ReturnsSmallestPowerOfTwoUpperBound(
        int x,
        int expected)
    {
        var actual = MathHelper.BitCeil(x);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0x40000001)]
    [InlineData(0x7FFFFFFF)]
    public void BitCeil_ResultOutOfRange_ThrowsOverflowException(int x)
    {
        _ = Assert.Throws<OverflowException>(() => MathHelper.BitCeil(x));
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(0, 8, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(1, 2, 0)]
    [InlineData(1, 8, 0)]
    [InlineData(1, Int32.MaxValue, 0)]
    [InlineData(1, -8, -1)]
    [InlineData(8, 8, 1)]
    [InlineData(8, -8, -1)]
    [InlineData(9, -8, -2)]
    [InlineData(15, 8, 1)]
    [InlineData(16, 8, 2)]
    [InlineData(17, 8, 2)]
    [InlineData(17, -8, -3)]
    [InlineData(Int32.MaxValue, 1, Int32.MaxValue)]
    [InlineData(Int32.MaxValue, Int32.MaxValue, 1)]
    [InlineData(Int32.MaxValue, Int32.MinValue, -1)]
    [InlineData(Int32.MaxValue, -1, -Int32.MaxValue)]
    [InlineData(-1, 1, -1)]
    [InlineData(-1, 8, -1)]
    [InlineData(-1, -1, 1)]
    [InlineData(-1, Int32.MinValue, 0)]
    [InlineData(-2, 8, -1)]
    [InlineData(-8, 8, -1)]
    [InlineData(-9, 8, -2)]
    [InlineData(-9, -8, 1)]
    [InlineData(-16, 8, -2)]
    [InlineData(-17, 8, -3)]
    [InlineData(-17, -8, 2)]
    [InlineData(Int32.MinValue, 1, Int32.MinValue)]
    [InlineData(Int32.MinValue, Int32.MaxValue, -2)]
    [InlineData(Int32.MinValue, Int32.MinValue, 1)]
    public void FloorDiv_ValidInput_ReturnsValidOutput(int x, int m, int expected)
    {
        var actual = MathHelper.FloorDiv(x, m);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void FloorDiv_ZeroDivisor_ThrowsArgumentOutOfRangeException(int x)
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(
            () => MathHelper.FloorDiv(x, 0));
    }

    [Theory]
    [InlineData(Int32.MinValue, -1)]
    public void FloorDiv_ResultOutOfRange_ThrowsOverflowException(int x, int m)
    {
        _ = Assert.Throws<OverflowException>(
            () => MathHelper.FloorDiv(x, m));
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(0, 8, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 8, 1)]
    [InlineData(1, Int32.MaxValue, 1)]
    [InlineData(1, -8, 0)]
    [InlineData(8, 8, 1)]
    [InlineData(8, -8, -1)]
    [InlineData(9, -8, -1)]
    [InlineData(15, 8, 2)]
    [InlineData(16, 8, 2)]
    [InlineData(17, 8, 3)]
    [InlineData(17, -8, -2)]
    [InlineData(Int32.MaxValue, 1, Int32.MaxValue)]
    [InlineData(Int32.MaxValue, Int32.MaxValue, 1)]
    [InlineData(Int32.MaxValue, Int32.MinValue, 0)]
    [InlineData(Int32.MaxValue, -1, -Int32.MaxValue)]
    [InlineData(-1, 1, -1)]
    [InlineData(-1, 8, 0)]
    [InlineData(-1, -1, 1)]
    [InlineData(-1, Int32.MinValue, 1)]
    [InlineData(-2, 8, 0)]
    [InlineData(-8, 8, -1)]
    [InlineData(-9, 8, -1)]
    [InlineData(-9, -8, 2)]
    [InlineData(-16, 8, -2)]
    [InlineData(-17, 8, -2)]
    [InlineData(-17, -8, 3)]
    [InlineData(Int32.MinValue, 1, Int32.MinValue)]
    [InlineData(Int32.MinValue, Int32.MaxValue, -1)]
    [InlineData(Int32.MinValue, Int32.MinValue, 1)]
    public void CeilDiv_ValidInput_ReturnsValidOutput(int x, int m, int expected)
    {
        var actual = MathHelper.CeilDiv(x, m);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void CeilDiv_ZeroDivisor_Throws(int x)
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(
            () => MathHelper.CeilDiv(x, 0));
    }

    [Theory]
    [InlineData(Int32.MinValue, -1)]
    public void CeilDiv_ResultOutOfRange_ThrowsOverflowException(int x, int m)
    {
        _ = Assert.Throws<OverflowException>(
            () => MathHelper.CeilDiv(x, m));
    }

    [Theory]
    [InlineData("00", true, 0x00)]
    [InlineData("0F", true, 0x0F)]
    [InlineData("a5", true, 0xA5)]
    [InlineData("FF", true, 0xFF)]
    [InlineData("A", false, 0x0A)]
    [InlineData("001", false, 0x01)]
    [InlineData("G1", false, 0x00)]
    [InlineData("100", false, 0x00)]
    public void TryGetHexByte_ValidatesLengthAndHexFormat(
        string token,
        bool expectedResult,
        byte expectedValue)
    {
        var actualResult = MathHelper.TryGetHexByte(token, out var actualValue);

        Assert.Equal(expectedResult, actualResult);
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData("zz")]
    [InlineData("hello")]
    [InlineData("not a number")]
    [InlineData("0x")]
    [InlineData("0x00")]
    [InlineData("x00")]
    public void TryGetHexByte_InvalidText_ReturnsFalse(string token)
    {
        var result = MathHelper.TryGetHexByte(token, out _);

        Assert.False(result);
    }
}
