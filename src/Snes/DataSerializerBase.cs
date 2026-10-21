namespace Maseya.Snes;

using System;

public abstract class DataSerializerBase<T> : IDataSerializerBase<T>
{
    public abstract int GetSerializedLength(int dataLength);

    public abstract int GetDeserializedDataCount(int byteLength);

    public abstract bool IsValidByteLength(int byteLength);

    public virtual bool TryGetDeserializedDataCount(int byteLength, out int dataCount)
    {
        return SerializerHelper.TryGetDeserializedDataCount(
            this,
            byteLength,
            out dataCount);
    }

    public abstract int Serialize(ReadOnlySpan<T> data, Span<byte> destination);

    public virtual byte[] Serialize(ReadOnlySpan<T> data)
    {
        return SerializerHelper.Serialize(this, data);
    }

    public abstract int Deserialize(ReadOnlySpan<byte> bytes, Span<T> destination);

    public virtual T[] Deserialize(ReadOnlySpan<byte> bytes)
    {
        return SerializerHelper.Deserialize(this, bytes);
    }
}
