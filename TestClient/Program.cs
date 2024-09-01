using Z21lib;
using Z21lib.Enums;
using Z21lib.Messages;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Z21Client client = new Z21Client(new Z21Info("192.168.0.111", 21105));
            client.MessageReceived += Client_MessageReceived;

            client.Connect();
            while (true)
            {
                string bytes = Console.ReadLine()!.ToLower();

                if (bytes.EndsWith("xx"))
                    client.Send(ComputeXOR(bytes.Replace("xx", null).ToByteArray()));
                else if (bytes.StartsWith("ai:"))
                    client.LanXGetTurnoutInfo(new(ushort.Parse(bytes.Replace("ai:", null))));
                else if (bytes.StartsWith("ei:"))
                    client.LanXGetExtAccessoryInfo(new(ushort.Parse(bytes.Replace("ei:", null))));
                else if (bytes.StartsWith("li:"))
                    client.LanXGetLocoInfo(new(ushort.Parse(bytes.Replace("li:", null))));
                else if (bytes.StartsWith("ss:"))
                    client.LanXSetTurnout(new(ushort.Parse(bytes.Replace("ss:", null).Replace("+", null).Replace("-", null))), bytes.Last() == '+', false, true);
                else
                    client.Send(bytes.ToByteArray());
            }
        }

        private static byte[] ComputeXOR(byte[] input)
        {
            byte xor = input[4];
            byte[] output = new byte[input.Length + 1];
            for (int i = 5; i < input.Length; i++)
                xor = (byte)(xor ^ input[i]);

            Array.Copy(input, output, input.Length);
            output[input.Length] = xor;
            return output;
        }

        private static void Client_MessageReceived(Message message)
        {
            MessageType type = message.Type;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Enum.GetName(typeof(MessageType), type));
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var prop in message.GetType().GetProperties())
            {
                if (prop.Name != "Type")
                    Console.WriteLine($"{prop.Name} = {prop.GetValue(message)}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
