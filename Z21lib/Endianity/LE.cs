﻿namespace Z21lib.Endianity
{
    static class LE
    {
        public static short ToInt16(byte[] data, int startIndex = 0)
        {
            return (short)(data[startIndex] | (data[startIndex + 1] << 8));
        }

        public static ushort ToUInt16(byte[] data, int startIndex = 0)
        {
            return (ushort)(data[startIndex] | (data[startIndex + 1] << 8));
        }

        public static int ToInt32(byte[] data, int startIndex = 0)
        {
            return data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24);
        }

        public static uint ToUInt32(byte[] data, int startIndex = 0)
        {
            return (uint)(data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24));
        }

        public static long ToInt64(byte[] data, int startIndex = 0)
        {
            return data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24) | (data[startIndex + 4] << 32) | (data[startIndex + 5] << 40) | (data[startIndex + 6] << 48) | (data[startIndex + 7] << 56);
        }

        public static ulong ToUInt64(byte[] data, int startIndex = 0)
        {
            return (ulong)(data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24) | (data[startIndex + 4] << 32) | (data[startIndex + 5] << 40) | (data[startIndex + 6] << 48) | (data[startIndex + 7] << 56));
        }
    }
}