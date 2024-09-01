using Z21lib.Endianity;

namespace Z21lib
{
    public static class RequestMessage
    {
        /// <summary>
        /// Creates message
        /// </summary>
        /// <param name="header">Header (big endian)</param>
        /// <param name="data">Message data</param>
        /// <param name="mode">Message modus</param>
        /// <param name="hasXor">Has XOR</param>
        /// <returns></returns>
        public static byte[] CreateMode(ushort header, byte[] data, byte mode)
        {
            ushort length = (ushort)(data.Length + 5);
            byte[] message = new byte[length];
            Array.Copy(LE.Parse(length), message, 2);
            Array.Copy(LE.Parse(header), 0, message, 2, 2);
            Array.Copy(data, 0, message, 4, data.Length);
            message[message.Length - 2] = mode;
            return message;
        }

        /// <summary>
        /// Creates message
        /// </summary>
        /// <param name="header">Header (big endian)</param>
        /// <param name="data">Message data</param>
        /// <returns>Message byte array</returns>
        /// <param name="hasXor">Has XOR</param>
        public static byte[] Create(ushort header, byte[] data)
        {
            ushort length = (ushort)(data.Length + 4);
            byte[] message = new byte[length];
            Array.Copy(LE.Parse(length), message, 2);
            Array.Copy(LE.Parse(header), 0, message, 2, 2);
            Array.Copy(data, 0, message, 4, data.Length);
            return message;
        }

        public static byte[] CreateXBUS(byte[] data)
        {
            ushort length = (ushort)(data.Length + 5);

            byte[] message = new byte[length];
            message[2] = 0x40;
            message[3] = 0x00;
            Array.Copy(LE.Parse(length), message, 2);
            Array.Copy(data, 0, message, 4, data.Length);
            byte xor = 0;
            for (int i = 0; i < data.Length; i++)
            {
                xor = (byte)(xor ^ data[i]);
            }
            message[message.Length - 1] = xor;
            return message;
        }

        public static byte[] CreateLocoNet(ushort header, byte[] data)
        {
            ushort length = (ushort)(data.Length + 5);

            byte[] message = new byte[length];
            Array.Copy(LE.Parse(length), message, 2);
            Array.Copy(LE.Parse(header), 0, message, 2, 2);
            Array.Copy(data, 0, message, 4, data.Length);
            byte xor = 0;
            for (int i = 0; i < data.Length; i++)
            {
                xor = (byte)(xor ^ data[i]);
            }
            message[message.Length - 1] = (byte)~xor;
            return message;
        }

        /// <summary>
        /// Creates message
        /// </summary>
        /// <param name="header">Header (big endian)</param>
        /// <returns>Message byte array</returns>
        public static byte[] Create(ushort header)
        {
            byte[] message = new byte[4];
            message[0] = 0x04;
            message[1] = 0x00;
            Array.Copy(LE.Parse(header), 0, message, 2, 2);
            return message;
        }
    }
}
