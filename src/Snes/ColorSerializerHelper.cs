// <copyright file="ColorSerializerHelper.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Provides helper methods for serializing and deserializing color arrays.
/// </summary>
internal static class ColorSerializerHelper
{
    /// <summary>
    /// Serializes colors into a newly allocated byte array.
    /// </summary>
    /// <param name="serializer">
    /// The serializer used to encode the colors.
    /// </param>
    /// <param name="colors">
    /// The colors to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the serialized colors.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The serializer cannot serialize the colors into the allocated array.
    /// </exception>
    internal static byte[] Serialize(
        IColorSerializer serializer,
        ReadOnlySpan<Color32BppArgb> colors)
    {
        var result = new byte[serializer.GetSerializedLength(colors.Length)];
        _ = serializer.Serialize(colors, result);
        return result;
    }

    /// <summary>
    /// Deserializes bytes into a newly allocated color array.
    /// </summary>
    /// <param name="serializer">
    /// The serializer used to decode the bytes.
    /// </param>
    /// <param name="bytes">
    /// The bytes to deserialize.
    /// </param>
    /// <returns>
    /// An array containing the deserialized colors.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The serialized bytes are invalid or cannot fit in the allocated array.
    /// </exception>
    internal static Color32BppArgb[] Deserialize(
        IColorSerializer serializer,
        ReadOnlySpan<byte> bytes)
    {
        var result = new Color32BppArgb[
            serializer.GetDeserializedColorCount(bytes.Length)];
        _ = serializer.Deserialize(bytes, result);
        return result;
    }
}
