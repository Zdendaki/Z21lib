using System.Runtime.CompilerServices;

namespace Z21lib
{
    static class Extensions
    {
        public static byte[] SubArray(this byte[] input, int position, int length)
        {
            byte[] output = new byte[length];
            Array.Copy(input, position, output, 0, length);
            return output;
        }

        public static byte[] ToArray(this ReadOnlySpan<byte> span)
        {
            return span.ToArray();
        }

        public static bool FlagPresent<T>(this uint input, T flag) where T : Enum
        {
            return (input & (uint)(object)flag) != 0;
        }

        public static bool FlagPresent<T>(this int input, T flag) where T : Enum
        {
            return (input & (int)(object)flag) != 0;
        }

        public static byte FromBCD(this byte input)
        {
            return (byte)(10 * (input / 16) + (input % 16));
        }

        public static bool Bit(this byte input, int bit)
        {
            return (input & (1 << bit)) == (1 << bit);
        }

        public static ArgumentOutOfRangeException GetException(string param, string message)
        {
            return new ArgumentOutOfRangeException(param, message);
        }

        public static unsafe byte[] SerializeValueType<T>(in T value) where T : unmanaged
        {
            byte[] result = new byte[sizeof(T)];
            Unsafe.As<byte, T>(ref result[0]) = value;
            return result;
        }

        public static unsafe T DeserializeValueType<T>(byte[] data) where T : unmanaged
        {
            return Unsafe.As<byte, T>(ref data[0]);
        }
    }
}
