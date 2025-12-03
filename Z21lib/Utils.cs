using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Z21lib
{
    static class Utils
    {
        internal static byte[] ConvertString(string input, int maxLength)
        {
            ReadOnlySpan<char> cleanInput = input.AsSpan();
            Span<char> buffer = stackalloc char[input.Length];
            int writeIndex = 0;
            
            foreach (char c in cleanInput)
            {
                if (c != '"' && c != '\\')
                {
                    buffer[writeIndex++] = c;
                }
            }
            
            byte[] data = new byte[maxLength];
            Encoding.Latin1.GetBytes(buffer[..writeIndex], data);
            
            return data;
        }

        internal static string ReadString(ReadOnlySpan<byte> data, int maxLength)
        {
            int length = data[..maxLength].IndexOf((byte)0x0);

            if (length == -1)
                length = maxLength;
            
            return Encoding.Latin1.GetString(data[..length]);
        }
        internal static TEnum[] GetEnums<TEnum>(ReadOnlySpan<byte> data) where TEnum : unmanaged, Enum
        {
            TEnum[] states = new TEnum[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                states[i] = Unsafe.As<byte, TEnum>(ref MemoryMarshal.GetReference(data.Slice(i, 1)));
            }
            return states;
        }
    }
}
