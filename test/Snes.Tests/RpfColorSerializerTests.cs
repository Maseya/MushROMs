// <copyright file="RpfColorSerializerTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class RpfColorSerializerTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 4)]
    public void GetSerializedLength_ReturnsRequiredByteCount(
        int colorLength,
        int expectedByteLength)
    {
        Assert.Equal(
            expectedByteLength,
            RpfColorSerializer.Default.GetSerializedLength(colorLength));
    }

    [Theory]
    [InlineData(-1, false, 0)]
    [InlineData(0, true, 0)]
    [InlineData(2, true, 1)]
    [InlineData(3, false, 1)]
    public void ByteLengthMethods_ReportValidityAndCompleteColorCount(
        int byteLength,
        bool expectedValidity,
        int expectedColorCount)
    {
        Assert.Equal(
            expectedColorCount,
            RpfColorSerializer.Default.GetDeserializedDataCount(byteLength));
        Assert.Equal(
            expectedValidity,
            RpfColorSerializer.Default.IsValidByteLength(byteLength));
        Assert.Equal(
            expectedValidity,
            RpfColorSerializer.Default.TryGetDeserializedDataCount(
                byteLength,
                out var colorCount));
        Assert.Equal(expectedColorCount, colorCount);
    }

    [Fact]
    public void Serialize_ConvertsColorsToLittleEndianSnesValues()
    {
        var color = new Color32BppArgb(0xFF, 0xF8, 0x80, 0x08);

        var bytes = RpfColorSerializer.Default.Serialize([color]);

        Assert.Equal([0x1F, 0x06], bytes);
    }

    [Fact]
    public void Serialize_WritesSnesValuesToDestinationSpan()
    {
        var color = new Color32BppArgb(0xFF, 0xF8, 0x80, 0x08);
        var destination = new byte[3];

        var bytesWritten = RpfColorSerializer.Default.Serialize([color], destination);

        Assert.Equal(2, bytesWritten);
        Assert.Equal([0x1F, 0x06, 0x00], destination);
    }

    [Fact]
    public void Serialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        var color = new Color32BppArgb(0xFF, 0xF8, 0x80, 0x08);
        var destination = new byte[1];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => RpfColorSerializer.Default.Serialize([color], destination));

        Assert.Equal("destination", exception.ParamName);
    }

    [Fact]
    public void Deserialize_ConvertsLittleEndianSnesValuesToColors()
    {
        byte[] bytes = [0x1F, 0x06];

        var colors = RpfColorSerializer.Default.Deserialize(bytes);

        Assert.Equal([Color32BppArgb.FromSnesColor(0x061F)], colors);
    }

    [Fact]
    public void Deserialize_IgnoresTrailingByte()
    {
        byte[] bytes = [0x1F, 0x06, 0xFF];

        var colors = RpfColorSerializer.Default.Deserialize(bytes);

        Assert.Equal([Color32BppArgb.FromSnesColor(0x061F)], colors);
    }

    [Fact]
    public void Deserialize_WritesColorsToDestinationSpan()
    {
        byte[] bytes = [0x1F, 0x06, 0xFF];
        var destination = new Color32BppArgb[2];

        var colorsWritten = RpfColorSerializer.Default.Deserialize(bytes, destination);

        Assert.Equal(1, colorsWritten);
        Assert.Equal(Color32BppArgb.FromSnesColor(0x061F), destination[0]);
    }

    [Fact]
    public void Deserialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        byte[] bytes = [0x1F, 0x06];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => RpfColorSerializer.Default.Deserialize(
                bytes,
                []));

        Assert.Equal("destination", exception.ParamName);
    }
}
