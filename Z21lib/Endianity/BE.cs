﻿using System.Buffers.Binary;

namespace Z21lib.Endianity
{
    static class BE
    {
        public static short ToInt16(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt16BigEndian(data.AsSpan(startIndex, sizeof(short)));
        }

        public static ushort ToUInt16(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(startIndex, sizeof(ushort)));
        }

        public static int ToInt32(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt32BigEndian(data.AsSpan(startIndex, sizeof(int)));
        }

        public static uint ToUInt32(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(data.AsSpan(startIndex, sizeof(uint)));
        }

        public static long ToInt64(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadInt64BigEndian(data.AsSpan(startIndex, sizeof(long)));
        }

        public static ulong ToUInt64(byte[] data, int startIndex)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(data.AsSpan(startIndex, sizeof(ulong)));
        }

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
    }
}
