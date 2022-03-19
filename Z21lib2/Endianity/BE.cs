namespace Z21lib.Endianity
{
    static class BE
    {
        public static short ToInt16(byte[] data, int startIndex = 0)
        {
            return (short)((data[startIndex] << 8) | data[startIndex + 1]);
        }

        public static ushort ToUInt16(byte[] data, int startIndex = 0)
        {
            return (ushort)((data[startIndex] << 8) | data[startIndex + 1]);
        }

        public static int ToInt32(byte[] data, int startIndex = 0)
        {
            return (data[startIndex] << 24) | (data[startIndex + 1] << 16) | (data[startIndex + 2] << 8) | data[startIndex + 3];
        }

        public static uint ToUInt32(byte[] data, int startIndex = 0)
        {
            return (uint)((data[startIndex] << 24) | (data[startIndex + 1] << 16) | (data[startIndex + 2] << 8) | data[startIndex + 3]);
        }

        public static long ToInt64(byte[] data, int startIndex = 0)
        {
            return (data[startIndex] << 56) | (data[startIndex + 1] << 48) | (data[startIndex + 2] << 40) | (data[startIndex + 3] << 32) | (data[startIndex + 4] << 24) | (data[startIndex + 5] << 16) | (data[startIndex + 6] << 8) | data[startIndex + 7];
        }

        public static ulong ToUInt64(byte[] data, int startIndex = 0)
        {
            return (ulong)((data[startIndex] << 56) | (data[startIndex + 1] << 48) | (data[startIndex + 2] << 40) | (data[startIndex + 3] << 32) | (data[startIndex + 4] << 24) | (data[startIndex + 5] << 16) | (data[startIndex + 6] << 8) | data[startIndex + 7]);
        }
    }
}
