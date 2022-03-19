﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z21lib;
using Z21lib.Messages;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(new Z21Info("192.168.0.111", 21105));
            client.MessageReceived += Client_MessageReceived;

            client.Connect();
            while (true)
            {
                string bytes = Console.ReadLine().ToLower();

                if (bytes.EndsWith("xx"))
                    client.Send(ComputeXOR(bytes.Replace("xx", null).ToByteArray()));
                else if (bytes.StartsWith("ai:"))
                    client.GetAccessoryInfo(int.Parse(bytes.Replace("ai:", null)));
                else if (bytes.StartsWith("ei:"))
                    client.GetExtendedAccessoryInfo(int.Parse(bytes.Replace("ei:", null)));
                else if (bytes.StartsWith("li:"))
                    client.GetLocoInfo(int.Parse(bytes.Replace("li:", null)));
                else if (bytes.StartsWith("ss:"))
                    client.SetAccessory(int.Parse(bytes.Replace("ss:", null).Replace("+", null).Replace("-", null)), bytes.Last() == '+');
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