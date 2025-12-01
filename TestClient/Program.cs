using System.Diagnostics;
using System.Reflection;
using Z21lib;
using Z21lib.Enums;
using Z21lib.Messages;
using Z21lib.Structs;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Z21Client client = new Z21Client(new Z21Info("192.168.1.222", 21105));
            client.MessageReceived += Client_MessageReceived;

            Console.WriteLine("Reconnecting...");

            client.Connect();

            client.LanSetBroadcastflags(BroadcastFlags.BasicData | BroadcastFlags.AllLocomotives);

            bool stop = false;
            Stopwatch sw = new();
            sw.Start();
            byte i = 0;

            /*while (true)
            {
                if (!client.IsConnected)
                {
                    Console.WriteLine("Reconnecting...");
                    if (!client.Connect())
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                }

                if (sw.ElapsedMilliseconds > 100)
                {
                    client.LanXSetLocoDrive(new LocoAddress(3), new(SpeedSteps.DCC128, LocoDirection.Forward, i, stop));
                    client.LanXSetLocoDrive(new LocoAddress(4), new(SpeedSteps.DCC128, LocoDirection.Forward, i, stop));
                    client.LanXSetLocoDrive(new LocoAddress(5), new(SpeedSteps.DCC128, LocoDirection.Forward, i, stop));
                    sw.Restart();
                    i++;
                }

                if (i > 100)
                    i = 0;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.S)
                        stop = !stop;
                }

                Thread.Sleep(10);
            }*/

            
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
                {
                    Address address = new(ushort.Parse(bytes.Replace("ss:", null).Replace("+", null).Replace("-", null)));
                    bool output = bytes.Last() != '+';
                    client.LanXSetTurnout(address, true, output, true);
                    Thread.Sleep(100);
                    client.LanXSetTurnout(address, false, output, true);
                }
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

        private static object _lock = new object();
        private static void Client_MessageReceived(Message message)
        {
            lock (_lock)
            {
                MessageType type = message.Type;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Enum.GetName(typeof(MessageType), type));
                Console.ForegroundColor = ConsoleColor.White;
                ReflectionForeach(message.GetType(), message, 0);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private static void ReflectionForeach(Type type, object? obj, int indetation)
        {
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.Name == "Type")
                    continue;

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{new string(' ', indetation)}{prop.Name}:");
                    Console.ForegroundColor = ConsoleColor.White;
                    ReflectionForeach(prop.PropertyType, prop.GetValue(obj), indetation + 4);
                }
                else
                {
                    try
                    {
                        Console.WriteLine($"{new string(' ', indetation)}{prop.Name} = {prop.GetValue(obj)}");
                    }
                    catch
                    {
                        Console.WriteLine($"{new string(' ', indetation)}{prop.Name} = <error retrieving value>");
                    }
                }
            }
        }
    }
}
