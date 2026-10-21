// <copyright file="PalColorSerializer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes colors as tightly packed 24-bit RGB values.
/// </summary>
/// <remarks>
/// Each color is stored as three consecutive bytes in red, green, blue order.
/// The alpha channel is not serialized and is restored as fully opaque.
/// </remarks>
public class PalColorSerializer : IDataSerializerBase<Color32BppArgb>
{
    /// <summary>
    /// Gets the default <see cref="PalColorSerializer"/> instance.
    /// </summary>
    public static readonly PalColorSerializer Default = new();

    /// <summary>
    /// Gets the number of bytes needed to serialize a number of colors.
    /// </summary>
    /// <param name="colorCount">
    /// The number of colors to serialize.
    /// </param>
    /// <returns>
    /// The required serialized size, in bytes.
    /// </returns>
    public int GetSerializedLength(int colorCount)
    {
        return Math.Max(colorCount, 0) * 3;
    }

    /// <summary>
    /// Gets the number of complete RGB colors that fit in a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <returns>
    /// The number of complete colors represented by the data.
    /// </returns>
    /// <remarks>
    /// Any trailing bytes that do not form a complete RGB color are ignored.
    /// A negative byte length returns zero.
    /// </remarks>
    public int GetDeserializedDataCount(int byteLength)
    {
        return Math.Max(byteLength, 0) / 3;
    }

    /// <summary>
    /// Determines whether a byte length contains only complete RGB colors.
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
        return (byteLength >= 0) && (byteLength % 3 == 0);
    }

    /// <summary>
    /// Tries to get the number of complete RGB colors represented by a byte
    /// length.
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
    public bool TryGetDeserializedDataCount(int byteLength, out int colorCount)
    {
        return SerializerHelper.TryGetDeserializedDataCount(
            this,
            byteLength,
            out colorCount);
    }

    /// <summary>
    /// Serializes each color as three 8-bit RGB bytes.
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
            destination[offset++] = colors[i].R;
            destination[offset++] = colors[i].G;
            destination[offset++] = colors[i].B;
        }

        return requiredLength;
    }

    /// <summary>
    /// Serializes each color as three 8-bit RGB bytes.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the serialized RGB color data.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated byte array is too small to contain
    /// <paramref name="colors"/>.
    /// </exception>
    public byte[] Serialize(ReadOnlySpan<Color32BppArgb> colors)
    {
        return SerializerHelper.Serialize(this, colors);
    }

    /// <summary>
    /// Deserializes complete groups of three RGB bytes into colors.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in red, green, blue order.
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
    /// Deserialized colors have an alpha value of 255. Any trailing bytes that
    /// do not form a complete color are ignored.
    /// </remarks>
    public int Deserialize(ReadOnlySpan<byte> bytes, Span<Color32BppArgb> destination)
    {
        var requiredLength = GetDeserializedDataCount(bytes.Length);
        if (destination.Length < requiredLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        for (int i = 0, offset = 0; i < requiredLength; i++)
        {
            destination[i] = new Color32BppArgb(
                alpha: Byte.MaxValue,
                red: bytes[offset++],
                green: bytes[offset++],
                blue: bytes[offset++]);
        }

        return requiredLength;
    }

    /// <summary>
    /// Deserializes PAL color data into an array of
    /// <see cref="Color32BppArgb"/>.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in red, green, blue order.
    /// </param>
    /// <returns>
    /// The colors represented by complete groups of three bytes.
    /// </returns>
    /// <remarks>
    /// Deserialized colors have an alpha value of 255. Any trailing bytes that
    /// do not form a complete color are ignored.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The allocated color array is too small to contain the colors
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    public Color32BppArgb[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return SerializerHelper.Deserialize(this, bytes);
    }
}
