using System.Buffers.Binary;

namespace Z21lib.Endianity
{
    static class LE
    {
        public static short ToInt16(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(startIndex, sizeof(short)));
        }

        public static ushort ToUInt16(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(startIndex, sizeof(ushort)));
        }

        public static int ToInt32(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(startIndex, sizeof(int)));
        }

        public static uint ToUInt32(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(startIndex, sizeof(uint)));
        }

        public static long ToInt64(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(data.AsSpan(startIndex, sizeof(long)));
        }

        public static ulong ToUInt64(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(data.AsSpan(startIndex, sizeof(ulong)));
        }

        // Span-based read methods
        public static short ToInt16(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(data);
        }

        public static ushort ToUInt16(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(data);
        }

        public static int ToInt32(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(data);
        }

        public static uint ToUInt32(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(data);
        }

        public static long ToInt64(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(data);
        }

        public static ulong ToUInt64(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(data);
        }

        public static byte[] Parse(short data)
        {
            byte[] binary = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16LittleEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(ushort data)
        {
            byte[] binary = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16LittleEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(int data)
        {
            byte[] binary = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32LittleEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(uint data)
        {
            byte[] binary = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32LittleEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(long data)
        {
            byte[] binary = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64LittleEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(ulong data)
        {
            byte[] binary = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64LittleEndian(binary, data);
            return binary;
        }

        // Span-based write methods for zero-allocation writes
        public static void Write(Span<byte> destination, short value)
        {
            BinaryPrimitives.WriteInt16LittleEndian(destination, value);
        }

        public static void Write(Span<byte> destination, ushort value)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(destination, value);
        }

        public static void Write(Span<byte> destination, int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(destination, value);
        }

        public static void Write(Span<byte> destination, uint value)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(destination, value);
        }

        public static void Write(Span<byte> destination, long value)
        {
            BinaryPrimitives.WriteInt64LittleEndian(destination, value);
        }

        public static void Write(Span<byte> destination, ulong value)
        {
            BinaryPrimitives.WriteUInt64LittleEndian(destination, value);
        }
    }
}
