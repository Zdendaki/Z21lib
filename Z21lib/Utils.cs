using System.Text;

namespace Z21lib
{
    static class Utils
    {
        internal static byte[] ConvertString(string input, int length)
        {
            input = input.Replace("\"", null).Replace(@"\", null);
            byte[] text = Encoding.Latin1.GetBytes(input);
            byte[] data = new byte[length];
            Array.Copy(text, 0, data, 0, Math.Min(text.Length, length));
            return data;
        }

        internal static string ReadString(byte[] data, int offset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (data[offset + i] == 0x0)
                {
                    length = i;
                    break;
                }
            }
            return Encoding.Latin1.GetString(data, offset, length);
        }
    }
}
