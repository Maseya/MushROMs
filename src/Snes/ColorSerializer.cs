// <copyright file="ColorSerializer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes <see cref="Color32BppArgb"/> values directly
/// as 8-bit ARGB byte sequences.
/// </summary>
/// <remarks>
/// Each color is serialized directly from its four 8-bit components in
/// alpha, red, green, and blue order, without color conversion.
/// </remarks>
public class ColorSerializer : IColorSerializer
{
    /// <summary>
    /// Gets the default <see cref="ColorSerializer"/> instance.
    /// </summary>
    public static readonly ColorSerializer Default = new();

    /// <summary>
    /// Gets the number of bytes needed to serialize a number of colors.
    /// </summary>
    /// <param name="colorLength">
    /// The number of colors to serialize.
    /// </param>
    /// <returns>
    /// The required serialized size, in bytes.
    /// </returns>
    public int GetSerializedLength(int colorLength)
    {
        return Math.Max(colorLength, 0) * Color32BppArgb.SizeOf;
    }

    /// <summary>
    /// Gets the number of complete colors that fit in a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <returns>
    /// The number of complete colors represented by the data.
    /// </returns>
    /// <remarks>
    /// Any trailing bytes that do not form a complete color are ignored.
    /// A negative byte length returns zero.
    /// </remarks>
    public int GetDeserializedColorCount(int byteLength)
    {
        return Math.Max(byteLength, 0) / Color32BppArgb.SizeOf;
    }

    /// <summary>
    /// Determines whether a byte length contains only complete colors.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the length contains complete colors;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValidByteLength(int byteLength)
    {
        return (byteLength >= 0) && (byteLength % Color32BppArgb.SizeOf == 0);
    }

    /// <summary>
    /// Tries to get the number of complete colors represented by a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <param name="colorCount">
    /// When this method returns, contains the number of complete colors in
    /// <paramref name="byteLength"/>. Trailing partial color data is excluded.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="byteLength"/> is valid for
    /// deserialization; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetDeserializedColorCount(int byteLength, out int colorCount)
    {
        colorCount = GetDeserializedColorCount(byteLength);
        return IsValidByteLength(byteLength);
    }

    /// <summary>
    /// Serializes each <see cref="Color32BppArgb"/> directly as four 8-bit
    /// ARGB bytes.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <param name="destination">
    /// The destination span for the serialized bytes.
    /// </param>
    /// <returns>
    /// The number of bytes written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The <paramref name="destination"/> span is too small to contain
    /// <paramref name="colors"/>.
    /// </exception>
    public int Serialize(ReadOnlySpan<Color32BppArgb> colors, Span<byte> destination)
    {
        var requiredLength = GetSerializedLength(colors.Length);
        if (destination.Length < requiredLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        for (int i = 0, offset = 0; i < colors.Length; i++)
        {
            destination[offset++] = colors[i].A;
            destination[offset++] = colors[i].R;
            destination[offset++] = colors[i].G;
            destination[offset++] = colors[i].B;
        }

        return requiredLength;
    }

    /// <summary>
    /// Serializes each <see cref="Color32BppArgb"/> directly as four 8-bit
    /// ARGB bytes.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the serialized color data.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated byte array is too small to contain
    /// <paramref name="colors"/>.
    /// </exception>
    public byte[] Serialize(ReadOnlySpan<Color32BppArgb> colors)
    {
        return ColorSerializerHelper.Serialize(this, colors);
    }

    /// <summary>
    /// Deserializes complete groups of four ARGB bytes into
    /// <paramref name="destination"/>.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in alpha, red, green, blue order.
    /// </param>
    /// <param name="destination">
    /// The destination span for the colors.
    /// </param>
    /// <returns>
    /// The number of colors written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="destination"/> is too small to contain the colors
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    /// <remarks>
    /// Any trailing bytes in <paramref name="bytes"/> that do not form a
    /// complete color are ignored.
    /// </remarks>
    public int Deserialize(ReadOnlySpan<byte> bytes, Span<Color32BppArgb> destination)
    {
        var requiredLength = GetDeserializedColorCount(bytes.Length);
        if (destination.Length < requiredLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        for (var i = 0; i < requiredLength; i++)
        {
            destination[i] = new Color32BppArgb(
                alpha: bytes[(i * Color32BppArgb.SizeOf) + 0],
                red: bytes[(i * Color32BppArgb.SizeOf) + 1],
                green: bytes[(i * Color32BppArgb.SizeOf) + 2],
                blue: bytes[(i * Color32BppArgb.SizeOf) + 3]);
        }

        return requiredLength;
    }

    /// <summary>
    /// Deserializes complete groups of four ARGB bytes from
    /// <paramref name="bytes"/> into a color array.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in alpha, red, green, blue order.
    /// </param>
    /// <returns>
    /// An array of <see cref="Color32BppArgb"/> colors, one for each complete
    /// group of four bytes in <paramref name="bytes"/>.
    /// </returns>
    /// <remarks>
    /// Any trailing bytes in <paramref name="bytes"/> that do not form a
    /// complete color are ignored.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The allocated color array is too small to contain the colors
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    public Color32BppArgb[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return ColorSerializerHelper.Deserialize(this, bytes);
    }
}
