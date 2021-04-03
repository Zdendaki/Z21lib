using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Z21lib.Endianity;
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
            Send(new byte[] { 0x04, 0x00, 0x10, 0x00 });
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
            //int len = message.Length;

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
                            break;

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

                        // LAN_X_LOCO_INFO
                        case 0xEF:
                            MessageReceived?.Invoke(LocoInfoMessage.Parse(message));
                            return;

                        // LAN_X_EXT_ACCESSORY_INFO
                        case 0x44:
                            MessageReceived?.Invoke(new AccessoryInfoMessage(new AccesoryAddress(message[5], message[6]), message[7], (AccessoryStatus)message[8]));
                            return;
                    }
                    break;

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

                // LAN_GET_CODE
                case 0x18:
                    MessageReceived?.Invoke(new DeviceCodeMessage((DeviceCode)message[4]));
                    return;
            }

            MessageReceived?.Invoke(new NotImplementedMessage(message));
        }

        public void Send(byte[] data)
        {
            try
            {
                Send(data, data.Length);
            }
            catch (Exception e)
            {
                ConsoleColor col = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = col;
            }
        }

        public void GetLocoInfo(int address)
        {
            LocoAddress la = new LocoAddress(address);
            byte[] request = new byte[9];
            request[0] = 0x09;
            request[1] = 0x00;
            request[2] = 0x40;
            request[3] = 0x00;
            request[4] = 0xE3;
            request[5] = 0xF0;
            request[6] = la.MSB;
            request[7] = la.LSB;
            request[8] = (byte)(request[4] ^ request[5] ^ request[6] ^ request[7]);

            Send(request);
        }

        public void GetAccessoryInfo(int address)
        {
            AccesoryAddress la = new AccesoryAddress(address);
            byte[] request = new byte[8];
            request[0] = 0x08;
            request[1] = 0x00;
            request[2] = 0x40;
            request[3] = 0x00;
            request[4] = 0x43;
            request[5] = la.MSB;
            request[6] = la.LSB;
            request[7] = (byte)(request[4] ^ request[5] ^ request[6]);

            Send(request);
        }
    }
}
