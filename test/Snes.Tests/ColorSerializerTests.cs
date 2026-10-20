// <copyright file="ColorSerializerTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class ColorSerializerTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 8)]
    public void GetSerializedLength_ReturnsRequiredByteCount(
        int colorLength,
        int expectedByteLength)
    {
        Assert.Equal(
            expectedByteLength,
            ColorSerializer.Default.GetSerializedLength(colorLength));
    }

    [Theory]
    [InlineData(-1, false, 0)]
    [InlineData(0, true, 0)]
    [InlineData(4, true, 1)]
    [InlineData(5, false, 1)]
    public void ByteLengthMethods_ReportValidityAndCompleteColorCount(
        int byteLength,
        bool expectedValidity,
        int expectedColorCount)
    {
        Assert.Equal(
            expectedColorCount,
            ColorSerializer.Default.GetDeserializedColorCount(byteLength));
        Assert.Equal(
            expectedValidity,
            ColorSerializer.Default.IsValidByteLength(byteLength));
        Assert.Equal(
            expectedValidity,
            ColorSerializer.Default.TryGetDeserializedColorCount(
                byteLength,
                out var colorCount));
        Assert.Equal(expectedColorCount, colorCount);
    }

    [Fact]
    public void Serialize_WritesColorsToDestinationSpan()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var destination = new byte[5];

        var bytesWritten = ColorSerializer.Default.Serialize([color], destination);

        Assert.Equal(4, bytesWritten);
        Assert.Equal([0x11, 0x22, 0x33, 0x44, 0x00], destination);
    }

    [Fact]
    public void Serialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var destination = new byte[3];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => ColorSerializer.Default.Serialize([color], destination));

        Assert.Equal("destination", exception.ParamName);
    }

    [Fact]
    public void Deserialize_WritesColorsToDestinationSpan()
    {
        byte[] bytes = [0x11, 0x22, 0x33, 0x44, 0xFF];
        var destination = new Color32BppArgb[2];

        var colorsWritten = ColorSerializer.Default.Deserialize(bytes, destination);

        Assert.Equal(1, colorsWritten);
        Assert.Equal(new Color32BppArgb(0x11, 0x22, 0x33, 0x44), destination[0]);
    }

    [Fact]
    public void Deserialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        byte[] bytes = [0x11, 0x22, 0x33, 0x44];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => ColorSerializer.Default.Deserialize(bytes, []));

        Assert.Equal("destination", exception.ParamName);
    }
}
