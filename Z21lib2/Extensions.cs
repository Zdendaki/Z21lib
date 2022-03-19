using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static bool FlagPresent<T>(this uint input, T flag) where T : Enum
        {
            return (input & (uint)(object)flag) != 0; 
        }

        public static bool FlagPresent<T>(this int input, T flag) where T : Enum
        {
            return (input & (int)(object)flag) != 0;
        }

        public static int FromBCD(this byte input)
        {
            return 10 * (input / 16) + (input % 16);
        }

        public static bool Bit(this byte input, int bit)
        {
            return (input & (1 << bit)) == (1 << bit);
        }

        public static ArgumentOutOfRangeException GetException(string param, string message)
        {
            return new ArgumentOutOfRangeException(param, new Exception(message));
        }
    }
}
