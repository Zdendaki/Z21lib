using System.Net;
using System.Net.Sockets;
using System.Text;
using Z21lib.Endianity;
using Z21lib.Enums;
using Z21lib.Messages;

namespace Z21lib
{
    /// <summary>
    /// Z21 Client (Firmware V1.42)
    /// </summary>
    public class Z21Client : UdpClient
    {
        public delegate void MessageReceivedEventHandler(Message message);
        public event MessageReceivedEventHandler MessageReceived = default!;

        private IPAddress IP;
        private int Port;

        public Z21Client(Z21Info info) : base(info.Port)
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
            Send([0x04, 0x00, 0x10, 0x00]);
            Log.Write("Connected to Z21!");
        }

        public void Disconnect()
        {

        }

        public void Callback(IAsyncResult result)
        {
            IPEndPoint? sender = null!;
            byte[] buffer = EndReceive(result, ref sender);
            BeginReceive(new AsyncCallback(Callback), null);
            ParseData(buffer);
        }

        private void ParseData(byte[] buffer)
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
                            MessageReceived?.Invoke(new AccessoryInfoMessage(new Address(message[5], message[6]), message[7], (AccessoryStatus)message[8]));
                            return;

                        // LAN_X_TURNOUT_INFO
                        case 0x43:
                            MessageReceived?.Invoke(new TurnoutInfoMessage(new Address(message[5], message[6]), (TurnoutState)message[7]));
                            return;
                    }
                    break;

                // LAN_GET_BROADCASTFLAGS
                case 0x51:
                    MessageReceived?.Invoke(new BroadcastFlagsMessage((BroadcastFlags)LE.ToUInt32(message, 4)));
                    return;

                // LAN_RMBUS_DATACHANGED
                case 0x80:
                    MessageReceived?.Invoke(new RBusDataChangedMessage(message.SubArray(4, 11)));
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

        /// <summary>
        /// 2.1 LAN_GET_SERIAL_NUMBER
        /// </summary>
        public void LanGetSerialNumber()
        {
            Send([0x04, 0x00, 0x10, 0x00]);
        }

        /// <summary>
        /// 2.2 LAN_LOGOFF
        /// </summary>
        public void LanLogoff()
        {
            Send([0x04, 0x00, 0x30, 0x00]);
        }

        /// <summary>
        /// 2.3 LAN_X_GET_VERSION
        /// </summary>
        public void LanXGetVersion()
        {
            Send([0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00]);
        }

        /// <summary>
        /// 2.5 LAN_X_SET_TRACK_POWER_OFF
        /// +
        /// 2.6 LAN_X_SET_TRACK_POWER_ON 
        /// </summary>
        public void LanXSetTrackPower(bool power)
        {
            Send(RequestMessage.CreateXBUS([0x21, (byte)(power ? 0x81 : 0x80)]));
        }

        /// <summary>
        /// 2.13 LAN_X_SET_STOP
        /// </summary>
        public void LanXSetStop()
        {
            Send([0x06, 0x00, 0x40, 0x00, 0x80, 0x80]);
        }

        /// <summary>
        /// 2.15 LAN_X_GET_FIRMWARE_VERSION 
        /// </summary>
        public void LanXGetFirmwareVersion()
        {
            Send([0x07, 0x00, 0x40, 0x00, 0xF1, 0x0A, 0xFB]);
        }

        /// <summary>
        /// 2.16 LAN_SET_BROADCASTFLAGS
        /// </summary>
        /// <param name="flags">Required broadcast messages</param>
        public void LanSetBroadcastflags(BroadcastFlags flags)
        {
            Send(RequestMessage.Create(0x50, LE.Parse((uint)flags)));
        }

        /// <summary>
        /// 2.17 LAN_GET_BROADCASTFLAGS
        /// </summary>
        public void LanGetBroadcastflags()
        {
            Send([0x04, 0x00, 0x51, 0x00]);
        }

        /// <summary>
        /// 2.19 LAN_SYSTEMSTATE_GETDATA
        /// </summary>
        public void LanSystemstateGetdata()
        {
            Send([0x04, 0x00, 0x85, 0x00]);
        }

        /// <summary>
        /// 2.20 LAN_GET_HWINFO
        /// </summary>
        public void LanGetHwinfo()
        {
            Send([0x04, 0x00, 0x1A, 0x00]);
        }

        /// <summary>
        /// 2.21 LAN_GET_CODE
        /// </summary>
        public void LanGetCode()
        {
            Send([0x04, 0x00, 0x18, 0x00]);
        }

        /// <summary>
        /// 3.1 LAN_GET_LOCOMODE
        /// </summary>
        /// <param name="address">Locomotive address (big endian)</param>
        public void LanGetLocomode(LocoAddress address)
        {
            Send(RequestMessage.Create(0x60, BE.Parse(address.Address)));
        }

        /// <summary>
        /// 3.2 LAN_SET_LOCOMODE
        /// </summary>
        /// <param name="address">Locomotive address (big endian)</param>
        /// <param name="mode">Locomotive mode </param>
        public void LanSetLocomode(LocoAddress address, DecoderMode mode)
        {
            Send(RequestMessage.CreateMode(0x61, BE.Parse(address.Address), (byte)mode));
        }

        /// <summary>
        /// 3.3 LAN_GET_TURNOUTMODE
        /// </summary>
        /// <param name="address">Accessory address</param>
        public void LanGetTurnoutmode(Address address)
        {
            Send(RequestMessage.Create(0x70, [address.MSB, address.LSB]));
        }

        /// <summary>
        /// 3.4 LAN_GET_TURNOUTMODE 
        /// </summary>
        /// <param name="address">Accessory address</param>
        /// <param name="mode">Accessory mode</param>
        public void LanSetTurnoutmode(Address address, DecoderMode mode)
        {
            Send(RequestMessage.CreateMode(0x71, [address.MSB, address.LSB], (byte)mode));
        }

        /// <summary>
        /// 4.1 LAN_X_GET_LOCO_INFO
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public void LanXGetLocoInfo(LocoAddress address)
        {
            Send(RequestMessage.CreateXBUS([0xE3, 0xF0, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 4.2 LAN_X_SET_LOCO_DRIVE
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="speed">Speed</param>
        public void LanXSetLocoDrive(LocoAddress address, LocoSpeed speed)
        {
            Send(RequestMessage.CreateXBUS([0xE4, (byte)(0x10 + speed.SpeedSteps), address.MSB, address.LSB, speed.GetByte()]));
        }

        /// <summary>
        /// 4.3.1 LAN_X_SET_LOCO_FUNCTION
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="state">Function state</param>
        /// <param name="function">Function address (0-31)</param>
        public void LanXSetLocoFunction(LocoAddress address, FunctionSwitch state, byte function)
        {
            if (function > 31)
                throw Extensions.GetException(nameof(function), "Function must be between 0 and 31!");

            Send(RequestMessage.CreateXBUS([0xE4, 0xF8, address.MSB, address.LSB, (byte)(((byte)state << 6) + function)]));
        }

        /// <summary>
        /// 4.3.2 LAN_X_SET_LOCO_FUNCTION_GROUP
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="group">Group code (see protocol specification)</param>
        /// <param name="functions">Group data (see protocol specification)</param>
        public void LanXSetLocoFunctionGroup(LocoAddress address, byte group, byte functions)
        {
            Send(RequestMessage.CreateXBUS([0xE4, group, address.MSB, address.LSB, functions]));
        }

        /// <summary>
        /// 4.3.3 LAN_X_SET_LOCO_BINARY_STATE
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="on">Binary state is ON</param>
        /// <param name="low">Data low byte</param>
        /// <param name="high">Data high byte</param>
        public void LanXSetLocoBinaryState(LocoAddress address, bool on, byte low, byte high)
        {
            low = (byte)(low & 0x7F | (on ? 0x80 : 0x00));
            Send(RequestMessage.CreateXBUS([0xE5, 0x5F, address.MSB, address.LSB, low, high]));
        }

        /// <summary>
        /// 4.5 LAN_X_SET_LOCO_E_STOP
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public void LanXSetLocoEStop(LocoAddress address)
        {
            Send(RequestMessage.CreateXBUS([0x92, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 4.6 LAN_X_PURGE_LOCO
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public void LanXPurgeLoco(LocoAddress address)
        {
            Send(RequestMessage.CreateXBUS([0xE3, 0x44, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 5.1 LAN_X_GET_TURNOUT_INFO
        /// </summary>
        /// <param name="address">Function address</param>
        public void LanXGetTurnoutInfo(Address address)
        {
            Send(RequestMessage.CreateXBUS([0x43, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 5.2 LAN_X_SET_TURNOUT
        /// </summary>
        /// <param name="address">Function address</param>
        /// <param name="activate"><see langword="true"/>: activate output | <see langword="false"/>: deactivate output</param>
        /// <param name="output"><see langword="true"/>: select output 2 | <see langword="false"/>: select output 1</param>
        /// <param name="useQueue">Use queue (see protocol specification)</param>
        public void LanXSetTurnout(Address address, bool activate, bool output, bool useQueue)
        {
            byte data = 0b1000_0000;
            if (activate)
                data |= 0b0000_1000;
            if (output)
                data |= 0b0000_0001;
            if (useQueue)
                data |= 0b0010_0000;
            Send(RequestMessage.CreateXBUS([0x53, address.MSB, address.LSB, data]));
        }

        /// <summary>
        /// 5.4 LAN_X_SET_EXT_ACCESSORY
        /// </summary>
        /// <param name="address">Function address</param>
        /// <param name="data">Extended accessory data (see accessory documentation)</param>
        public void LanXSetExtAccessory(Address address, byte data)
        {
            Send(RequestMessage.CreateXBUS([0x54, address.MSB, address.LSB, data, 0x00]));
        }

        /// <summary>
        /// 5.6 LAN_X_GET_EXT_ACCESSORY_INFO
        /// </summary>
        /// <param name="address">Function address</param>
        public void LanXGetExtAccessoryInfo(Address address)
        {
            Send(RequestMessage.CreateXBUS([0x44, address.MSB, address.LSB, 0x00]));
        }

        /// <summary>
        /// 6.1 LAN_X_CV_READ 
        /// </summary>
        /// <param name="address">CV address</param>
        public void LanXCVRead(Address address)
        {
            Send(RequestMessage.CreateXBUS([0x23, 0x11, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 6.2 LAN_X_CV_WRITE
        /// </summary>
        /// <param name="address">CV address</param>
        /// <param name="value">CV value</param>
        public void LanXCVWrite(Address address, byte value)
        {
            Send(RequestMessage.CreateXBUS([0x24, 0x12, address.MSB, address.LSB, value]));
        }

        /// <summary>
        /// 6.6 LAN_X_CV_POM_WRITE_BYTE
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="value">CV value</param>
        public void LanXCVPOMWriteByte(LocoAddress addressL, Address addressC, byte value)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");

            byte db3 = (byte)(0xEC + (addressC.MSB & 3));
            Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, value]));
        }

        /// <summary>
        /// 6.7 LAN_X_CV_POM_WRITE_BIT 
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="position">Bit position</param>
        /// <param name="value">Bit value</param>
        public void LanXCVPOMWriteBit(LocoAddress addressL, Address addressC, byte position, bool value)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");
            if (position > 7)
                throw Extensions.GetException(nameof(position), "Position mush be between 0 and 7!");

            byte db3 = (byte)(0xE8 + (addressC.MSB & 3));
            byte db5 = (byte)(position + (value ? 8 : 0));
            Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, db5]));
        }

        /// <summary>
        /// 6.8 LAN_X_CV_POM_READ_BYTE
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        public void LanXCVPOMReadByte(LocoAddress addressL, Address addressC)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");

            byte db3 = (byte)(0xE4 + (addressC.MSB & 3));
            Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, 0]));
        }

        /// <summary>
        /// 6.9 LAN_X_CV_POM_ACCESSORY_WRITE_BYTE
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        /// <param name="value">Byte value</param>
        public void LanXCVPomAccessoryWriteByte(Address addressF, Address addressC, byte output, byte value)
        {
            if (addressF.Value > 0x1FF)
                throw Extensions.GetException(nameof(addressC), "Accessory address must be between 0 and 511!");
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");
            if (output > 7)
                throw Extensions.GetException(nameof(output), "Position mush be between 0 and 7!");

            if (output > 0)
                output |= 8;

            byte db1 = (byte)((addressF.Value & 0x1F0) >> 4);
            byte db2 = (byte)(((addressF.Value & 0xF) << 4) | output);
            byte db3 = (byte)(0xEC + (addressC.MSB & 3));
            Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, value]));
        }

        /// <summary>
        /// 6.10 LAN_X_CV_POM_ACCESSORY_WRITE_BIT
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        /// <param name="position">Bit position</param>
        /// <param name="value">Bit value</param>
        public void LanXCVPomAccessoryWriteBit(Address addressF, Address addressC, byte output, byte position, bool value)
        {
            if (addressF.Value > 0x1FF)
                throw Extensions.GetException(nameof(addressC), "Accessory address must be between 0 and 511!");
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");
            if (output > 7)
                throw Extensions.GetException(nameof(output), "Position mush be between 0 and 7!");
            if (position > 7)
                throw Extensions.GetException(nameof(position), "Position mush be between 0 and 7!");

            if (output > 0)
                output |= 8;

            byte db1 = (byte)((addressF.Value & 0x1F0) >> 4);
            byte db2 = (byte)(((addressF.Value & 0xF) << 4) | output);
            byte db3 = (byte)(0xE8 + (addressC.MSB & 3));
            byte db5 = (byte)(position + (value ? 8 : 0));
            Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, db5]));
        }

        /// <summary>
        /// 6.11 LAN_X_CV_POM_ACCESSORY_READ_BYTE
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        public void LanXCVPOMAccessoryReadByte(Address addressF, Address addressC, byte output)
        {
            if (addressF.Value > 0x1FF)
                throw Extensions.GetException(nameof(addressC), "Accessory address must be between 0 and 511!");
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");
            if (output > 7)
                throw Extensions.GetException(nameof(output), "Position mush be between 0 and 7!");

            if (output > 0)
                output |= 8;

            byte db1 = (byte)((addressF.Value & 0x1F0) >> 4);
            byte db2 = (byte)(((addressF.Value & 0xF) << 4) | output);
            byte db3 = (byte)(0xE4 + (addressC.MSB & 3));
            Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, 0]));
        }

        /// <summary>
        /// 6.12 LAN_X_MM_WRITE_BYTE 
        /// </summary>
        /// <param name="regAdr">Register address (0=Register1, 1=Register2, ...)</param>
        /// <param name="value">Register value</param>
        public void LanXMMWriteByte(byte regAdr, byte value)
        {
            Send(RequestMessage.CreateXBUS([0x24, 0xFF, 0, regAdr, value]));
        }

        /// <summary>
        /// 6.13 LAN_X_DCC_READ_REGISTER
        /// </summary>
        /// <param name="register">Register (0x01=Register1, 0x02=Register2, ...)</param>
        public void LanXDCCReadRegister(byte register)
        {
            Send(RequestMessage.CreateXBUS([0x22, 0x11, register]));
        }

        /// <summary>
        /// 6.14 LAN_X_DCC_WRITE_REGISTER
        /// </summary>
        /// <param name="register">Register (0x01=Register1, 0x02=Register2, ...)</param>
        /// <param name="value">Register value</param>
        public void LanXDCCWriteRegister(byte register, byte value)
        {
            Send(RequestMessage.CreateXBUS([0x23, 0x12, register, value]));
        }

        /// <summary>
        /// 7.2 LAN_RMBUS_GETDATA
        /// </summary>
        /// <param name="group">Group index</param>
        public void LanRmbusGetdata(byte group)
        {
            Send([0x05, 0x00, 0x81, 0x00, group]);
        }

        /// <summary>
        /// 7.3 LAN_RMBUS_PROGRAMMODULE
        /// </summary>
        /// <param name="address">R-BUS address</param>
        public void LanRmbusProgrammodule(byte address)
        {
            Send([0x05, 0x00, 0x82, 0x00, address]);
        }

        /// <summary>
        /// 8.2 LAN_RAILCOM_GETDATA
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="type">Data poll type</param>
        public void LanRailcomGetdata(LocoAddress address, byte type = 0x01)
        {
            Send([0x07, 0x00, 0x89, 0x00, type, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 9.3 LAN_LOCONET_FROM_LAN
        /// </summary>
        /// <param name="data">LocoNet message (excl. checksum)</param>
        public void LanLoconetFromLan(byte[] data)
        {
            Send(RequestMessage.CreateLocoNet(0xA2, data));
        }

        /// <summary>
        /// 9.4 LAN_LOCONET_DISPATCH_ADDR
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public void LanLoconetDispatchAddr(LocoAddress address)
        {
            Send([0x06, 0x00, 0xA3, 0x00, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 9.5 LAN_LOCONET_DETECTOR
        /// </summary>
        /// <param name="type">Request type</param>
        /// <param name="address">Report address</param>
        public void LanLoconetDetector(LoconetRequestType type, Address address)
        {
            Send([0x07, 0x00, 0xA4, 0x00, (byte)type, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 10.1 LAN_CAN_DETECTOR
        /// </summary>
        /// <param name="type">Request type</param>
        /// <param name="networkId">CAN-Network ID</param>
        public void LanCanDetector(byte type, Address networkId)
        {
            Send(RequestMessage.Create(0xC4, [type, networkId.LSB, networkId.MSB]));
        }

        /// <summary>
        /// 10.2.1 LAN_CAN_DEVICE_GET_DESCRIPTION
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        public void LanCanDeviceGetDescription(Address networkId)
        {
            Send(RequestMessage.Create(0xC8, [networkId.LSB, networkId.MSB]));
        }

        /// <summary>
        /// 10.2.2 LAN_CAN_DEVICE_SET_DESCRIPTION
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        /// <param name="description">CAN device description (max length: 16)</param>
        public void LanCanDeviceSetDescription(Address networkId, string description)
        {
            description = description.Replace("\"", null).Replace(@"\", null);
            byte[] text = Encoding.Latin1.GetBytes(description);
            byte[] data = new byte[18];
            Array.Copy(LE.Parse(networkId.Value), data, 2);
            Array.Copy(text, 0, data, 2, Math.Min(text.Length, 16));
            Send(RequestMessage.Create(0xC9, data));
        }

        /// <summary>
        /// 10.2.4 LAN_CAN_BOOSTER_SET_TRACKPOWER
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        /// <param name="power">Power mode</param>
        public void LanCanBoosterSetTrackpower(Address networkId, Power power)
        {
            Send(RequestMessage.Create(0xCB, [networkId.LSB, networkId.MSB, (byte)power]));
        }
    }
}
