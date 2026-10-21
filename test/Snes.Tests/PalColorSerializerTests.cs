// <copyright file="PalColorSerializerTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class PalColorSerializerTests
{
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 3)]
    [InlineData(2, 6)]
    public void GetSerializedLength_ReturnsRequiredByteCount(
        int colorLength,
        int expectedByteLength)
    {
        Assert.Equal(
            expectedByteLength,
            PalColorSerializer.Default.GetSerializedLength(colorLength));
    }

    [Theory]
    [InlineData(-1, false, 0)]
    [InlineData(0, true, 0)]
    [InlineData(2, false, 0)]
    [InlineData(3, true, 1)]
    [InlineData(4, false, 1)]
    public void ByteLengthMethods_ReportValidityAndCompleteColorCount(
        int byteLength,
        bool expectedValidity,
        int expectedColorCount)
    {
        Assert.Equal(
            expectedColorCount,
            PalColorSerializer.Default.GetDeserializedDataCount(byteLength));
        Assert.Equal(
            expectedValidity,
            PalColorSerializer.Default.IsValidByteLength(byteLength));
        Assert.Equal(
            expectedValidity,
            PalColorSerializer.Default.TryGetDeserializedDataCount(
                byteLength,
                out var colorCount));
        Assert.Equal(expectedColorCount, colorCount);
    }

    [Fact]
    public void Serialize_WritesRedGreenBlueBytesAndDropsAlpha()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        var bytes = PalColorSerializer.Default.Serialize([color]);

        Assert.Equal([0x22, 0x33, 0x44], bytes);
    }

    [Fact]
    public void Serialize_WritesRgbDataToDestinationSpan()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var destination = new byte[4];

        var bytesWritten = PalColorSerializer.Default.Serialize([color], destination);

        Assert.Equal(3, bytesWritten);
        Assert.Equal([0x22, 0x33, 0x44, 0x00], destination);
    }

    [Fact]
    public void Serialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        var exception = Assert.Throws<BufferTooSmallException>(
            () => PalColorSerializer.Default.Serialize([color], new byte[2]));

        Assert.Equal("destination", exception.ParamName);
    }

    [Fact]
    public void Deserialize_AssignsOpaqueAlphaToRgbData()
    {
        byte[] bytes = [0x22, 0x33, 0x44];

        var colors = PalColorSerializer.Default.Deserialize(bytes);

        Assert.Equal([new Color32BppArgb(0xFF, 0x22, 0x33, 0x44)], colors);
    }

    [Fact]
    public void Deserialize_IgnoresTrailingBytes()
    {
        byte[] bytes = [0x22, 0x33, 0x44, 0x55];

        var colors = PalColorSerializer.Default.Deserialize(bytes);

        Assert.Equal([new Color32BppArgb(0xFF, 0x22, 0x33, 0x44)], colors);
    }

    [Fact]
    public void Deserialize_WritesColorsToDestinationSpan()
    {
        byte[] bytes = [0x22, 0x33, 0x44];
        var destination = new Color32BppArgb[2];

        var colorsWritten = PalColorSerializer.Default.Deserialize(bytes, destination);

        Assert.Equal(1, colorsWritten);
        Assert.Equal(new Color32BppArgb(0xFF, 0x22, 0x33, 0x44), destination[0]);
    }

    [Fact]
    public void Deserialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        byte[] bytes = [0x22, 0x33, 0x44];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => PalColorSerializer.Default.Deserialize(bytes, []));

        Assert.Equal("destination", exception.ParamName);
    }
}
