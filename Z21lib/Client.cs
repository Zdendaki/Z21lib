using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Z21lib.Messages;
using static Z21lib.Messages.HardwareTypeMessage;

namespace Z21lib
{
    public class Client : UdpClient
    {
        public delegate void MessageReceivedEventHandler(Message message);
        public event MessageReceivedEventHandler MessageReceived;
        
        private IPAddress IP;
        private int Port;
        
        public Client(Z21Info info) : base (info.Port)
        {
            IP = IPAddress.Parse(info.IP);
            Port = info.Port;
            Log.DebugLog = true; // TODO: Debug
        }

        public void Connect()
        {
            Connect(IP, Port);
            DontFragment = false;
            EnableBroadcast = true;
            BeginReceive(new AsyncCallback(Callback), null);
            Log.Write("Connected to Z21!");
        }

        public void Disconnect()
        {

        }

        public void Callback(IAsyncResult result)
        {
            IPEndPoint sender = null;
            byte[] buffer = EndReceive(result, ref sender);
            BeginReceive(new AsyncCallback(Callback), null);
            ParseData(buffer);
        }

        public void ParseData(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 4)
                return;

            int position = 0;

            while (position < buffer.Length)
            {
                int length = GetMessageLength(buffer);

                if (length < 4)
                    break;

                ParseMessage(buffer.SubArray(position, length));
                position += length;
            }
        }

        private int GetMessageLength(byte[] input)
        {
            if (input.Length < 4)
                throw Extensions.GetException("input", "Input data are too short!");

            return LE.ToInt16(input);
        }

        private void ParseMessage(byte[] message)
        {
            int len = message.Length;

            switch (message[2])
            {
                // LAN_GET_SERIAL_NUMBER
                case 0x10:
                    int serial = LE.ToInt32(message, 4);
                    MessageReceived?.Invoke(new SerialNumberMessage(serial));
                    return;

                // X-BUS
                case 0x40:
                    switch (message[4])
                    {
                        // X-BUS Version
                        case 0x63:
                            MessageReceived?.Invoke(new XBusVersionMessage(message[6].FromBCD(), message[7]));
                            return;

                        // Track power
                        case 0x61:
                            switch (message[5])
                            {
                                // Power off
                                case 0x00:
                                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_POWER_OFF));
                                    return;

                                // Power on
                                case 0x01:
                                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_POWER_ON));
                                    return;

                                // Programming mode
                                case 0x02:
                                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_PROGRAMMING_MODE));
                                    return;

                                // Short Circuit
                                case 0x08:
                                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_SHORT_CIRCUIT));
                                    return;

                                // Unknown command
                                case 0x82:
                                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_UNKNOWN_COMMAND));
                                    return;
                            }
                            return;

                        // Status changed
                        case 0x62:
                            MessageReceived?.Invoke(new TrackStatusChangedMessage((CentralState)message[6]));
                            return;

                        // Stopped
                        case 0x81:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_STOPPED));
                            return;

                        // Firmware version
                        case 0xF3:
                            MessageReceived?.Invoke(new FirmwareVersionMessage(message[6].FromBCD(), message[7].FromBCD()));
                            return;
                    }
                    return;

                // LAN_GET_BROADCASTFLAGS
                case 0x51:
                    MessageReceived?.Invoke(new BroadcastFlagsMessage((BroadcastFlags)LE.ToUInt32(message, 4)));
                    return;

                // LAN_SYSTEMSTATE_DATACHANGED
                case 0x84:
                    MessageReceived?.Invoke(new SystemStateMessage(message.SubArray(4, 16)));
                    return;

                // LAN_GET_HWINFO
                case 0x1A:
                    HardwareType hw = (HardwareType)LE.ToUInt32(message, 4);
                    MessageReceived?.Invoke(new HardwareTypeMessage(hw, message[9].FromBCD(), message[8].FromBCD()));
                    return;
            }

            
            if (message[2] == 0x10)
            {
                int serial = LE.ToInt32(message, 4);
                MessageReceived?.Invoke(new SerialNumberMessage(serial));
            }
        }
    }
}
