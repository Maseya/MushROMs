namespace Maseya.Snes;

using System;

/// <summary>
/// Serializes and deserializes data arrays.
/// </summary>
public interface IDataSerializerBase<T>
{
    /// <summary>
    /// Gets the number of bytes needed to serialize an array of
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="dataLength">
    /// The number of <typeparamref name="T"/> to serialize.
    /// </param>
    /// <returns>
    /// The required serialized size, in bytes.
    /// </returns>
    int GetSerializedLength(int dataLength);

    /// <summary>
    /// Gets the number of <typeparamref name="T"/> entries that fit ina byte
    /// length.
    /// </summary>
    /// <param name="byteLength">
    /// The total serialized length, in bytes, of the serialized data.
    /// </param>
    /// <returns>
    /// The number of <typeparamref name="T"/> entries represented by the data.
    /// </returns>
    int GetDeserializedDataCount(int byteLength);

    /// <summary>
    /// Determines whether a byte length contains only complete data.
    /// </summary>
    /// <param name="byteLength">
    /// The total length of the byte array to test.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the length contains complete data;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsValidByteLength(int byteLength);

    /// <summary>
    /// Tries to get the number of complete <typeparamref name="T"/>
    /// represented by a byte length.
    /// </summary>
    /// <param name="byteLength">
    /// The total serialized length in bytes, including any formatting.
    /// </param>
    /// <param name="dataCount">
    /// When this method returns, contains the number of
    /// <typeparamref name="T"/> entries in <paramref name="byteLength"/>.
    /// Trailing partial data is excluded.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="byteLength"/> is valid for
    /// deserialization; otherwise, <see langword="false"/>.
    /// </returns>
    bool TryGetDeserializedDataCount(int byteLength, out int dataCount)
    {
        return SerializerHelper.TryGetDeserializedDataCount(
            this,
            byteLength,
            out dataCount);
    }

    /// <summary>
    /// Serializes an array of <typeparamref name="T"/> into a byte array.
    /// </summary>
    /// <param name="data">
    /// The array of <typeparamref name="T"/> to serialize.
    /// </param>
    /// <param name="destination">
    /// The destination for the serialized data.
    /// </param>
    /// <returns>
    /// The number of bytes written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="destination"/> is too small to contain
    /// <paramref name="data"/>.
    /// </exception>
    int Serialize(ReadOnlySpan<T> data, Span<byte> destination);

    /// <summary>
    /// Serializes an array of <typeparamref name="T"/> into a byte array.
    /// </summary>
    /// <param name="data">
    /// The array of <typeparamref name="T"/> to serialize.
    /// </param>
    /// <returns>
    /// The serialized color data.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated byte array is too small to contain
    /// <paramref name="data"/>.
    /// </exception>
    byte[] Serialize(ReadOnlySpan<T> data)
    {
        return SerializerHelper.Serialize(this, data);
    }

    /// <summary>
    /// Deserializes a byte array into an array of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="bytes">
    /// The serialized data.
    /// </param>
    /// <param name="destination">
    /// The destination span for the deserialized data.
    /// </param>
    /// <returns>
    /// The number of entries written.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="destination"/> is too small to contain the data
    /// represented by <paramref name="bytes"/>.
    /// </exception>
    int Deserialize(ReadOnlySpan<byte> bytes, Span<T> destination);

    /// <summary>
    /// Deserializes <paramref name="bytes"/> into a newly allocated
    /// <typeparamref name="T"/> array.
    /// </summary>
    /// <param name="bytes">
    /// The serialized data.
    /// </param>
    /// <returns>
    /// The data represented by <paramref name="bytes"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The allocated <typeparamref name="T"/> array is too small to contain
    /// the data represented by <paramref name="bytes"/>.
    /// </exception>
    T[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return SerializerHelper.Deserialize(this, bytes);
    }
}
