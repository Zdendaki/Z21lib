using System.Buffers.Binary;

namespace Z21lib.Endianity
{
    static class BE
    {
        public static byte[] Parse(short data)
        {
            byte[] binary = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(ushort data)
        {
            byte[] binary = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(int data)
        {
            byte[] binary = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(uint data)
        {
            byte[] binary = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(long data)
        {
            byte[] binary = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(binary, data);
            return binary;
        }

        public static byte[] Parse(ulong data)
        {
            byte[] binary = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(binary, data);
            return binary;
        }

        // Span-based methods for zero-allocation writes
        public static void Write(Span<byte> destination, short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(destination, value);
        }

        public static void Write(Span<byte> destination, ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(destination, value);
        }

        public static void Write(Span<byte> destination, int value)
        {
            BinaryPrimitives.WriteInt32BigEndian(destination, value);
        }

        public static void Write(Span<byte> destination, uint value)
        {
            BinaryPrimitives.WriteUInt32BigEndian(destination, value);
        }

        public static void Write(Span<byte> destination, long value)
        {
            BinaryPrimitives.WriteInt64BigEndian(destination, value);
        }

        public static void Write(Span<byte> destination, ulong value)
        {
            BinaryPrimitives.WriteUInt64BigEndian(destination, value);
        }
    }
}
