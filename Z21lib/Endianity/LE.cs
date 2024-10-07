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
    }
}
