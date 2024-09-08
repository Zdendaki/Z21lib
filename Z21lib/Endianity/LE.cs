namespace Z21lib.Endianity
{
    static class LE
    {
        public static short ToInt16(byte[] data, int startIndex)
        {
            return (short)(data[startIndex] | (data[startIndex + 1] << 8));
        }

        public static ushort ToUInt16(byte[] data, int startIndex)
        {
            return (ushort)(data[startIndex] | (data[startIndex + 1] << 8));
        }

        public static int ToInt32(byte[] data, int startIndex)
        {
            return data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24);
        }

        public static uint ToUInt32(byte[] data, int startIndex)
        {
            return (uint)(data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24));
        }

        public static long ToInt64(byte[] data, int startIndex)
        {
            return data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24) | (data[startIndex + 4] << 32) | (data[startIndex + 5] << 40) | (data[startIndex + 6] << 48) | (data[startIndex + 7] << 56);
        }

        public static ulong ToUInt64(byte[] data, int startIndex)
        {
            return (ulong)(data[startIndex] | (data[startIndex + 1] << 8) | (data[startIndex + 2] << 16) | (data[startIndex + 3] << 24) | (data[startIndex + 4] << 32) | (data[startIndex + 5] << 40) | (data[startIndex + 6] << 48) | (data[startIndex + 7] << 56));
        }

        public static byte[] Parse(short data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }

        public static byte[] Parse(ushort data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }

        public static byte[] Parse(int data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }

        public static byte[] Parse(uint data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }

        public static byte[] Parse(long data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }

        public static byte[] Parse(ulong data)
        {
            byte[] binary = BitConverter.GetBytes(data);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(binary);
            return binary;
        }
    }
}
