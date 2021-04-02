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
                string bytes = Console.ReadLine();
                client.Send(bytes.ToByteArray());
            }
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
