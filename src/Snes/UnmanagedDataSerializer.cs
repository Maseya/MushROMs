namespace Maseya.Snes;

using System;
using System.Runtime.InteropServices;

public class UnmanagedDataSerializer<T> : DataSerializerBase<T>
    where T : unmanaged
{
    public static readonly UnmanagedDataSerializer<T> Default = new();

    public override int GetSerializedLength(int dataLength)
    {
        return Math.Max(dataLength, 0) * Marshal.SizeOf<T>();
    }

    public override int GetDeserializedDataCount(int byteLength)
    {
        return Math.Max(byteLength, 0) / Marshal.SizeOf<T>();
    }

    public override bool IsValidByteLength(int byteLength)
    {
        return (byteLength >= 0) && (byteLength % Marshal.SizeOf<T>() == 0);
    }

    public override int Serialize(ReadOnlySpan<T> data, Span<byte> destination)
    {
        var requiredByteLength = GetSerializedLength(data.Length);
        if (destination.Length < requiredByteLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        unsafe
        {
            fixed (T* ptr = data)
            {
                var source = new ReadOnlySpan<byte>((byte*)ptr, requiredByteLength);
                source.CopyTo(destination);
            }
        }

        return requiredByteLength;
    }

    public override int Deserialize(
        ReadOnlySpan<byte> bytes,
        Span<T> destination)
    {
        var requiredArrayLength = GetDeserializedDataCount(bytes.Length);
        if (destination.Length < requiredArrayLength)
        {
            throw new BufferTooSmallException(nameof(destination));
        }

        var byteLength = requiredArrayLength * Marshal.SizeOf<T>();
        unsafe
        {
            fixed (T* ptr = destination)
            {
                var dest = new Span<byte>((byte*)ptr, byteLength);
                bytes[..byteLength].CopyTo(dest);
            }
        }

        return requiredArrayLength;
    }
}
