// <copyright file="TplColorSerializerTests.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes.Tests;

using Xunit;

public class TplColorSerializerTests
{
    [Theory]
    [InlineData(0, 4)]
    [InlineData(2, 8)]
    public void GetSerializedLength_ReturnsRequiredByteCount(
        int colorLength,
        int expectedByteLength)
    {
        Assert.Equal(
            expectedByteLength,
            TplColorSerializer.Default.GetSerializedLength(colorLength));
    }

    [Theory]
    [InlineData(-1, false, 0)]
    [InlineData(3, false, 0)]
    [InlineData(4, true, 0)]
    [InlineData(5, false, 0)]
    [InlineData(6, true, 1)]
    [InlineData(7, false, 1)]
    public void ByteLengthMethods_ReportValidityAndCompleteColorCount(
        int byteLength,
        bool expectedValidity,
        int expectedColorCount)
    {
        Assert.Equal(
            expectedColorCount,
            TplColorSerializer.Default.GetDeserializedDataCount(byteLength));
        Assert.Equal(
            expectedValidity,
            TplColorSerializer.Default.IsValidByteLength(byteLength));
        Assert.Equal(
            expectedValidity,
            TplColorSerializer.Default.TryGetDeserializedDataCount(
                byteLength,
                out var colorCount));
        Assert.Equal(expectedColorCount, colorCount);
    }

    [Fact]
    public void Serialize_PrependsTplHeaderToColorData()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);

        var bytes = TplColorSerializer.Default.Serialize([color]);

        Assert.Equal([0x54, 0x50, 0x4C, 0x02, 0xC4, 0x20], bytes);
    }

    [Fact]
    public void Serialize_WithoutColors_ReturnsOnlyTplHeader()
    {
        var bytes = TplColorSerializer.Default.Serialize([]);

        Assert.Equal([0x54, 0x50, 0x4C, 0x02], bytes);
    }

    [Fact]
    public void Serialize_WritesTplDataToDestinationSpan()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var destination = new byte[7];

        var bytesWritten = TplColorSerializer.Default.Serialize([color], destination);

        Assert.Equal(6, bytesWritten);
        Assert.Equal(
            [0x54, 0x50, 0x4C, 0x02, 0xC4, 0x20, 0x00],
            destination);
    }

    [Fact]
    public void Serialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        var color = new Color32BppArgb(0x11, 0x22, 0x33, 0x44);
        var destination = new byte[5];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => TplColorSerializer.Default.Serialize([color], destination));

        Assert.Equal("destination", exception.ParamName);
    }

    [Fact]
    public void Deserialize_RemovesTplHeaderBeforeReadingColorData()
    {
        byte[] bytes = [0x54, 0x50, 0x4C, 0x02, 0xC4, 0x20];

        var colors = TplColorSerializer.Default.Deserialize(bytes);

        Assert.Equal([Color32BppArgb.FromSnesColor(0x20C4)], colors);
    }

    [Fact]
    public void Deserialize_WritesColorsToDestinationSpan()
    {
        byte[] bytes = [0x54, 0x50, 0x4C, 0x02, 0xC4, 0x20];
        var destination = new Color32BppArgb[2];

        var colorsWritten = TplColorSerializer.Default.Deserialize(bytes, destination);

        Assert.Equal(1, colorsWritten);
        Assert.Equal(Color32BppArgb.FromSnesColor(0x20C4), destination[0]);
    }

    [Theory]
    [InlineData(0x00)]
    [InlineData(0xFF)]
    public void Deserialize_AcceptsAnyFourthHeaderByte(int fourthHeaderByte)
    {
        byte[] bytes =
        [
            0x54,
            0x50,
            0x4C,
            (byte)fourthHeaderByte,
            0xC4,
            0x20,
        ];

        var colors = TplColorSerializer.Default.Deserialize(bytes);
        var destination = new Color32BppArgb[1];
        var colorsWritten = TplColorSerializer.Default.Deserialize(
            bytes,
            destination);

        Assert.Equal([Color32BppArgb.FromSnesColor(0x20C4)], colors);
        Assert.Equal(1, colorsWritten);
        Assert.Equal(Color32BppArgb.FromSnesColor(0x20C4), destination[0]);
    }

    [Fact]
    public void Deserialize_WithInsufficientDestination_ThrowsBufferTooSmallException()
    {
        byte[] bytes = [0x54, 0x50, 0x4C, 0x02, 0xC4, 0x20];

        var exception = Assert.Throws<BufferTooSmallException>(
            () => TplColorSerializer.Default.Deserialize(
                bytes,
                []));

        Assert.Equal("destination", exception.ParamName);
    }

    [Theory]
    [InlineData(new byte[] { })]
    [InlineData(new byte[] { 0x54, 0x50, 0x4C })]
    [InlineData(new byte[] { 0x54, 0x50, 0x58, 0x00 })]
    public void Deserialize_WithoutTplHeader_ThrowsInvalidTplHeaderException(
        byte[] bytes)
    {
        var exception = Assert.Throws<InvalidTplHeaderException>(
            () => TplColorSerializer.Default.Deserialize(bytes));

        Assert.Equal("bytes", exception.ParamName);

        exception = Assert.Throws<InvalidTplHeaderException>(
            () => TplColorSerializer.Default.Deserialize(
                bytes,
                []));

        Assert.Equal("bytes", exception.ParamName);
    }
}
