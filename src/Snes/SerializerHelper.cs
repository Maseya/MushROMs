// <copyright file="ColorSerializerHelper.cs" organization="Maseya">
//     Copyright (c) 2026 spel werdz rite. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Snes;

using System;

/// <summary>
/// Provides helper methods for serializing and deserializing data arrays.
/// </summary>
internal static class SerializerHelper
{
    internal static bool TryGetDeserializedDataCount<T>(
        IDataSerializerBase<T> serializer,
        int byteLength,
        out int dataCount)
    {
        dataCount = serializer.GetDeserializedDataCount(byteLength);
        return serializer.IsValidByteLength(byteLength);
    }

    /// <summary>
    /// Serializes data into a newly allocated byte array.
    /// </summary>
    /// <param name="serializer">
    /// The <see cref="IDataSerializerBase{T}"/> used to encode the data.
    /// </param>
    /// <param name="data">
    /// The array of <typeparamref name="T"/> to serialize.
    /// </param>
    /// <returns>
    /// A byte array containing the serialized data.
    /// </returns>
    internal static byte[] Serialize<T>(
        IDataSerializerBase<T> serializer,
        ReadOnlySpan<T> data)
    {
        var result = new byte[serializer.GetSerializedLength(data.Length)];
        _ = serializer.Serialize(data, result);
        return result;
    }

    /// <summary>
    /// Deserializes bytes into a newly allocated <typeparamref name="T"/>
    /// array.
    /// </summary>
    /// <param name="serializer">
    /// The serializer used to decode the bytes.
    /// </param>
    /// <param name="bytes">
    /// The bytes to deserialize.
    /// </param>
    /// <returns>
    /// An array containing the deserialized data.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The serialized bytes are invalid.
    /// </exception>
    internal static T[] Deserialize<T>(
        IDataSerializerBase<T> serializer,
        ReadOnlySpan<byte> bytes)
    {
        var result = new T[
            serializer.GetDeserializedDataCount(bytes.Length)];
        _ = serializer.Deserialize(bytes, result);
        return result;
    }
}
