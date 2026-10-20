// <copyright file="IColorSerializer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes <see cref="Color32BppArgb"/> arrays.
/// </summary>
public interface IColorSerializer
{
    /// <summary>
    /// Gets the number of bytes needed to serialize a number of colors.
    /// </summary>
    /// <param name="colorLength">
    /// The number of colors to serialize.
    /// </param>
    /// <returns>
    /// The required serialized size, in bytes.
    /// </returns>
    int GetSerializedLength(int colorLength);

    /// <summary>
    /// Gets the number of complete colors that fit in a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The total serialized length, in bytes, of the serialized color data.
    /// </param>
    /// <returns>
    /// The number of complete colors represented by the data.
    /// </returns>
    /// <remarks>
    /// Any trailing bytes that do not form a complete color are ignored.
    /// </remarks>
    int GetDeserializedColorCount(int byteLength);

    /// <summary>
    /// Determines whether a byte length contains only complete colors.
    /// </summary>
    /// <param name="byteLength">
    /// The total length of the byte array to test.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the length contains complete colors;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValidByteLength(int byteLength);

    /// <summary>
    /// Tries to get the number of complete colors represented by a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The total serialized length in bytes, including any format header.
    /// </param>
    /// <param name="colorCount">
    /// When this method returns, contains the number of complete colors in
    /// <paramref name="byteLength"/>. Trailing partial color data is excluded.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="byteLength"/> is valid for
    /// deserialization; otherwise, <see langword="false"/>.
    /// </returns>
    bool TryGetDeserializedColorCount(int byteLength, out int colorCount);

    /// <summary>
    /// Serializes an array of <see cref="Color32BppArgb"/> into a byte array.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <param name="destination">
    /// The destination for the serialized data.
    /// </param>
    /// <returns>
    /// The number of bytes written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="destination"/> is too small to contain
    /// <paramref name="colors"/>.
    /// </exception>
    int Serialize(ReadOnlySpan<Color32BppArgb> colors, Span<byte> destination);

    /// <summary>
    /// Serializes an array of <see cref="Color32BppArgb"/> into a byte array.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// The serialized color data.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated byte array is too small to contain
    /// <paramref name="colors"/>.
    /// </exception>
    byte[] Serialize(ReadOnlySpan<Color32BppArgb> colors)
    {
        return ColorSerializerHelper.Serialize(this, colors);
    }

    /// <summary>
    /// Deserializes <paramref name="bytes"/> into
    /// <paramref name="destination"/>.
    /// </summary>
    /// <param name="bytes">
    /// The serialized color data.
    /// </param>
    /// <param name="destination">
    /// The destination span for the deserialized colors.
    /// </param>
    /// <returns>
    /// The number of colors written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="destination"/> is too small to contain the colors
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    int Deserialize(ReadOnlySpan<byte> bytes, Span<Color32BppArgb> destination);

    /// <summary>
    /// Deserializes <paramref name="bytes"/> into a newly allocated color
    /// array.
    /// </summary>
    /// <param name="bytes">
    /// The serialized color data.
    /// </param>
    /// <returns>
    /// The colors represented by <paramref name="bytes"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated color array is too small to contain the colors
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    Color32BppArgb[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return ColorSerializerHelper.Deserialize(this, bytes);
    }
}
