// <copyright file="RpfColorSerializer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes colors in the RPF (Raw Palette Format).
/// </summary>
/// <remarks>
/// RPF is a custom format that stores colors exactly as they are stored in
/// SNES VRAM: 16-bit little-endian values containing 15-bit SNES colors.
/// </remarks>
public class RpfColorSerializer : IColorSerializer
{
    /// <summary>
    /// Gets the default <see cref="RpfColorSerializer"/> instance.
    /// </summary>
    public static readonly RpfColorSerializer Default = new();

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
        return Math.Max(colorLength, 0) * sizeof(ushort);
    }

    /// <summary>
    /// Gets the number of complete SNES colors that fit in a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <returns>
    /// The number of complete SNES colors represented by the data.
    /// </returns>
    /// <remarks>
    /// Any trailing byte that does not form a complete SNES color is ignored.
    /// A negative byte length returns zero.
    /// </remarks>
    public int GetDeserializedColorCount(int byteLength)
    {
        return Math.Max(byteLength, 0) / sizeof(ushort);
    }

    /// <summary>
    /// Determines whether a byte length contains only complete SNES colors.
    /// </summary>
    /// <param name="byteLength">
    /// The serialized length in bytes.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the length contains complete SNES colors;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValidByteLength(int byteLength)
    {
        return (byteLength >= 0) && (byteLength % sizeof(ushort) == 0);
    }

    /// <summary>
    /// Tries to get the number of complete SNES colors represented by a
    /// byte length.
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
    /// Serializes <paramref name="colors"/> as 16-bit SNES color values.
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
            var value = Color32BppArgb.ToSnesColor(colors[i]);
            destination[offset++] = (byte)value;
            destination[offset++] = (byte)(value >> 8);
        }

        return requiredLength;
    }

    /// <summary>
    /// Serializes <paramref name="colors"/> as 16-bit SNES color values.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing little-endian 16-bit SNES color values.
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
    /// Deserializes RPF color data into an array of
    /// <see cref="Color32BppArgb"/>.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in little-endian order.
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
    /// Any trailing byte that does not form a complete color is ignored.
    /// </remarks>
    public int Deserialize(ReadOnlySpan<byte> bytes, Span<Color32BppArgb> destination)
    {
        var requiredLength = GetDeserializedColorCount(bytes.Length);
        if (destination.Length < requiredLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        for (int i = 0, offset = 0; i < requiredLength; i++)
        {
            destination[i] = Color32BppArgb.FromSnesColor(
                bytes[offset++],
                bytes[offset++]);
        }

        return requiredLength;
    }

    /// <summary>
    /// Deserializes RPF color data into an array of
    /// <see cref="Color32BppArgb"/>.
    /// </summary>
    /// <param name="bytes">
    /// The bytes to deserialize, in little-endian order.
    /// </param>
    /// <returns>
    /// The colors represented by complete groups of two bytes.
    /// </returns>
    /// <remarks>
    /// Any trailing byte that does not form a complete color is ignored.
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
