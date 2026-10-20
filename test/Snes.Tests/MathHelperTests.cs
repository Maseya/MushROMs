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
