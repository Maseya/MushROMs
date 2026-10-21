// <copyright file="TplColorSerializer.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes colors in Tile Layer Pro palette files.
/// </summary>
/// <remarks>
/// TPL is a palette format used by Tile Layer Pro, a retro game graphics
/// editor. Each file starts with a four-byte header: the ASCII bytes "TPL"
/// followed by a byte whose value is not validated. Serialization uses
/// 0x02 for the fourth byte.
/// </remarks>
public class TplColorSerializer : IDataSerializerBase<Color32BppArgb>
{
    /// <summary>
    /// Gets the default <see cref="TplColorSerializer"/> instance.
    /// </summary>
    public static readonly TplColorSerializer Default = new();

    /// <summary>
    /// Canonical "TPL\x02" header written when serializing TPL files.
    /// </summary>
    public static readonly byte[] Header = [0x54, 0x50, 0x4C, 0x02];

    private const int HeaderSignatureLength = 3;

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
        return Header.Length + (Math.Max(colorCount, 0) * sizeof(ushort));
    }

    /// <summary>
    /// Gets the number of complete colors that fit in a TPL byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The total TPL length in bytes, including the header.
    /// </param>
    /// <returns>
    /// The number of complete colors represented by the length.
    /// </returns>
    /// <remarks>
    /// The length includes the four-byte header. Any trailing byte that does
    /// not form a complete color is ignored.
    /// A negative byte length returns zero.
    /// </remarks>
    public int GetDeserializedDataCount(int byteLength)
    {
        return (Math.Max(byteLength, Header.Length) - Header.Length) / sizeof(ushort);
    }

    /// <summary>
    /// Determines whether a byte length contains only complete TPL colors.
    /// </summary>
    /// <param name="byteLength">
    /// The total TPL length in bytes, including the header.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the length contains complete colors;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValidByteLength(int byteLength)
    {
        return (byteLength >= Header.Length)
            && ((byteLength - Header.Length) % sizeof(ushort) == 0);
    }

    /// <summary>
    /// Tries to get the number of complete colors represented by a TPL length.
    /// </summary>
    /// <param name="byteLength">
    /// The total TPL length in bytes, including the header.
    /// </param>
    /// <param name="colorCount">
    /// When this method returns, contains the number of complete colors in
    /// <paramref name="byteLength"/>. Trailing partial color data is excluded.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="byteLength"/> is valid for
    /// TPL deserialization; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetDeserializedDataCount(int byteLength, out int colorCount)
    {
        return SerializerHelper.TryGetDeserializedDataCount(
            this,
            byteLength,
            out colorCount);
    }

    /// <summary>
    /// Serializes an array of <see cref="Color32BppArgb"/> into a TPL palette
    /// format.
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
    /// The <paramref name="destination"/> span is too small to contain the
    /// serialized <paramref name="colors"/> and the TPL header.
    /// </exception>
    /// <remarks>
    /// Each TPL file starts with the four-byte header, "TPL\x02", followed
    /// by raw palette data in the RPF (Raw Palette Format).
    /// </remarks>
    public int Serialize(ReadOnlySpan<Color32BppArgb> colors, Span<byte> destination)
    {
        var requiredLength = GetSerializedLength(colors.Length);
        if (destination.Length < requiredLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        Header.CopyTo(destination);
        _ = RpfColorSerializer.Default.Serialize(colors, destination[Header.Length..]);

        return requiredLength;
    }

    /// <summary>
    /// Serializes an array of <see cref="Color32BppArgb"/> into a TPL palette
    /// format.
    /// </summary>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the TPL header and serialized color data.
    /// </returns>
    /// <remarks>
    /// Each TPL file starts with the four-byte header, "TPL\x02", followed
    /// by raw palette data in the RPF (Raw Palette Format).
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The allocated byte array is too small to contain the serialized
    /// <paramref name="colors"/> and the TPL header.
    /// </exception>
    public byte[] Serialize(ReadOnlySpan<Color32BppArgb> colors)
    {
        return SerializerHelper.Serialize(this, colors);
    }

    /// <summary>
    /// Deserializes TPL color data into an array of
    /// <see cref="Color32BppArgb"/>.
    /// </summary>
    /// <param name="bytes">
    /// The TPL bytes to deserialize, with 16-bit SNES colors in
    /// little-endian order.
    /// </param>
    /// <param name="destination">
    /// The destination span for the colors.
    /// </param>
    /// <returns>
    /// The number of colors written.
    /// </returns>
    /// <exception cref="InvalidTplHeaderException">
    /// <paramref name="bytes"/> does not begin with the TPL header.
    /// </exception>
    /// <exception cref="BufferTooSmallException">
    /// <paramref name="destination"/> is too small for the colors.
    /// </exception>
    /// <remarks>
    /// The four-byte header starts with the ASCII bytes "TPL"; its fourth
    /// byte is ignored. The remaining data contains 16-bit SNES colors.
    /// </remarks>
    public int Deserialize(ReadOnlySpan<byte> bytes, Span<Color32BppArgb> destination)
    {
        var hasHeader = bytes.Length >= Header.Length
            && bytes.StartsWith(Header.AsSpan(0, HeaderSignatureLength));
        var colorData = hasHeader
            ? bytes[Header.Length..]
            : throw new InvalidTplHeaderException(nameof(bytes));

        return RpfColorSerializer.Default.Deserialize(colorData, destination);
    }

    /// <summary>
    /// Deserializes TPL color data into an array of
    /// <see cref="Color32BppArgb"/>.
    /// </summary>
    /// <param name="bytes">
    /// The TPL bytes to deserialize, with 16-bit SNES colors in
    /// little-endian order.
    /// </param>
    /// <returns>
    /// An array of <see cref="Color32BppArgb"/> colors, one for each complete
    /// group of two bytes in <paramref name="bytes"/>.
    /// </returns>
    /// <exception cref="InvalidTplHeaderException">
    /// <paramref name="bytes"/> does not begin with the TPL header.
    /// </exception>
    /// <remarks>
    /// The four-byte header starts with the ASCII bytes "TPL"; its fourth
    /// byte is ignored. The remaining data contains 16-bit SNES colors.
    /// </remarks>
    public Color32BppArgb[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return SerializerHelper.Deserialize(this, bytes);
    }
}
