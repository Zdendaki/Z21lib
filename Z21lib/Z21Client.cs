using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using Z21lib.Endianity;
using Z21lib.Enums;
using Z21lib.Messages;
using Z21lib.Structs;

namespace Z21lib
{
    /// <summary>
    /// Z21 Client (Firmware V1.42)
    /// <br /><br />
    /// <seealso href="https://www.z21.eu/media/Kwc_Basic_DownloadTag_Component/root-en-main_47-1652-959-downloadTag-download/default/d559b9cf/1699290380/z21-lan-protokoll-en.pdf">Z21 API Documentation</seealso>
    /// </summary>
    public class Z21Client : UdpClient, IDisposable
    {
        public delegate void MessageReceivedEventHandler(Message message);
        public event MessageReceivedEventHandler MessageReceived = default!;

        private const int MAX_PACKET_SIZE = 1472;
        private const int SEND_INTERVAL = 50;

        private IPAddress IP;
        private int Port;
        private bool _connected = false;

        private readonly Channel<byte[]> _sendQueue = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _sendingTask;

        public bool IsConnected => Active;

        public Z21Client(Z21Info info) : base(info.Port)
        {
            IP = IPAddress.Parse(info.IP);
            Port = info.Port;
        }

        public bool Connect()
        {
            try
            {
                Connect(IP, Port);

            if (!Active)
                return false;

                DontFragment = true;
                EnableBroadcast = false;
                BeginReceive(new AsyncCallback(Callback), null);

                _cancellationTokenSource = new CancellationTokenSource();
                _sendingTask = SendingLoopAsync(_cancellationTokenSource.Token);

                _connected = true;
                return LanGetSerialNumber();
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            _connected = false;
            _cancellationTokenSource?.Cancel();
            _sendingTask?.Wait(TimeSpan.FromSeconds(2));
        }

        public bool Reconnect()
        {
            Disconnect();
            return Connect();
        }

        private async Task SendingLoopAsync(CancellationToken cancellationToken)
        {
            byte[] packetBuffer = ArrayPool<byte>.Shared.Rent(MAX_PACKET_SIZE);
            
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bufferPosition = 0;
                    long startTicks = Stopwatch.GetTimestamp();

                    while (bufferPosition < MAX_PACKET_SIZE)
                    {
                        if (_sendQueue.Reader.TryPeek(out var data))
                        {
                            if (bufferPosition + data.Length > MAX_PACKET_SIZE)
                                break;

                            data.CopyTo(packetBuffer.AsSpan(bufferPosition));
                            bufferPosition += data.Length;
                            _sendQueue.Reader.TryRead(out _);
                        }
                        else
                        {
                            double elapsed = Stopwatch.GetElapsedTime(startTicks).TotalMilliseconds;
                            
                            if (bufferPosition > 0 || elapsed >= SEND_INTERVAL)
                                break;

                            try
                            {
                                await Task.Delay(10, cancellationToken);
                            }
                            catch (OperationCanceledException)
                            {
                                throw;
                            }
                        }
                    }

                    // Send combined buffer if we have any data
                    if (bufferPosition > 0)
                    {
                        if (!Active || !_connected)
                            break;

                        try
                        {
                            Send(packetBuffer, bufferPosition);
                        }
                        catch
                        {
                            Disconnect();
                            break;
                        }
                    }

                    // Maintain send interval
                    double elapsedMs = Stopwatch.GetElapsedTime(startTicks).TotalMilliseconds;
                    int remainingDelay = SEND_INTERVAL - (int)elapsedMs;
                    
                    if (remainingDelay > 0)
                    {
                        try
                        {
                            await Task.Delay(remainingDelay, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(packetBuffer);
            }
        }

        private void Callback(IAsyncResult result)
        {
            IPEndPoint? sender = null!;
            byte[] buffer = EndReceive(result, ref sender);
            if (Active)
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
                int length = GetMessageLength(buffer.AsSpan(position));

                if (length < 4 || position + length > buffer.Length)
                    break;

                ParseMessage(buffer.AsSpan(position, length));
                position += length;
            }
        }

        private int GetMessageLength(ReadOnlySpan<byte> input)
        {
            if (input.Length < 4)
                throw Extensions.GetException(nameof(input), "Input data are too short!");

            return LE.ToUInt16(input);
        }

        private void ParseMessage(ReadOnlySpan<byte> message)
        {
            ushort length = LE.ToUInt16(message);
            ushort header = LE.ToUInt16(message.Slice(2));

            switch (header)
            {
                // 2.1 LAN_GET_SERIAL_NUMBER
                case 0x10:
                    uint serial = LE.ToUInt32(message.Slice(4));
                    MessageReceived?.Invoke(new SerialNumberMessage(serial));
                    return;

                // All X-BUS messages
                case 0x40:
                    ParseXbusMessage(message, length);
                    return;

                // 2.17 LAN_GET_BROADCASTFLAGS
                case 0x51:
                    MessageReceived?.Invoke(BroadcastFlagsMessage.Parse(message));
                    return;

                // 2.18 LAN_SYSTEMSTATE_DATACHANGED
                case 0x84:
                    MessageReceived?.Invoke(SystemStateMessage.Parse(message));
                    return;

                // 2.20 LAN_GET_HWINFO
                case 0x1A:
                    MessageReceived?.Invoke(HardwareTypeMessage.Parse(message));
                    return;

                // 2.21 LAN_GET_CODE
                case 0x18:
                    MessageReceived?.Invoke(new DeviceCodeMessage((DeviceCode)message[4]));
                    return;

                // 3.1 LAN_GET_LOCOMODE
                case 0x60:
                    MessageReceived?.Invoke(LocoModeMessage.Parse(message));
                    return;

                // 3.3 LAN_GET_TURNOUTMODE
                case 0x70:
                    MessageReceived?.Invoke(TurnoutModeMessage.Parse(message));
                    return;

                // 7.1 LAN_RMBUS_DATACHANGED
                case 0x80:
                    MessageReceived?.Invoke(new RBusDataChangedMessage(message.Slice(4, 11)));
                    return;

                // 8.1 LAN_RAILCOM_DATACHANGED
                case 0x88:
                    MessageReceived?.Invoke(RailComDataMessage.Parse(message));
                    return;

                // 9.1 LAN_LOCONET_Z21_RX
                case 0xA0:
                    MessageReceived?.Invoke(new LoconetMessage(MessageType.LAN_LOCONET_Z21_RX, message));
                    return;

                // 9.2 LAN_LOCONET_Z21_TX
                case 0xA1:
                    MessageReceived?.Invoke(new LoconetMessage(MessageType.LAN_LOCONET_Z21_TX, message));
                    return;

                // 9.3 LAN_LOCONET_FROM_LAN
                case 0xA2:
                    MessageReceived?.Invoke(new LoconetMessage(MessageType.LAN_LOCONET_FROM_LAN, message));
                    return;

                // 9.4 LAN_LOCONET_DISPATCH_ADDR
                case 0xA3:
                    MessageReceived?.Invoke(LoconetDispatchMessage.Parse(message));
                    return;

                // 9.5 LAN_LOCONET_DETECTOR
                case 0xA4:
                    MessageReceived?.Invoke(LoconetDetectorMessage.Parse(message));
                    return;

                // 10.1 LAN_CAN_DETECTOR
                case 0xC4:
                    MessageReceived?.Invoke(CanDetectorMessage.Parse(message));
                    return;

                // 10.2.1 LAN_CAN_DEVICE_GET_DESCRIPTION
                case 0xC8:
                    MessageReceived?.Invoke(CanBoosterDescriptionMessage.Parse(message));
                    return;

                // 10.2.3 LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD
                case 0xCA:
                    MessageReceived?.Invoke(CanBoosterSystemstateMessage.Parse(message));
                    return;

                // 11.1.1.1 LAN_ZLINK_GET_HWINFO
                case 0xE8:
                    MessageReceived?.Invoke(ZlinkHwInfoMessage.Parse(message));
                    return;

                // 11.2.1 LAN_BOOSTER_GET_DESCRIPTION
                case 0xB8:
                    MessageReceived?.Invoke(LanBoosterDescriptionMessage.Parse(message));
                    return;

                // 11.2.4 LAN_BOOSTER_SYSTEMSTATE_DATACHANGED
                case 0xBA:
                    MessageReceived?.Invoke(LanBoosterSystemStateMessage.Parse(message));
                    return;
            }

            // Message is not implemented / was not recognized
            MessageReceived?.Invoke(new NotImplementedMessage(message));
        }

        private void ParseXbusMessage(ReadOnlySpan<byte> message, ushort length)
        {
            if (length < 6)
                return;

            switch (message[4])
            {
                // 2.3 LAN_X_GET_VERSION
                case 0x63:
                    MessageReceived?.Invoke(XBusVersionMessage.Parse(message));
                    return;

                case 0x61:
                    switch (message[5])
                    {
                        // 2.7 LAN_X_BC_TRACK_POWER_OFF
                        case 0x00:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_POWER_OFF));
                            return;

                        // 2.8 LAN_X_BC_TRACK_POWER_ON
                        case 0x01:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_POWER_ON));
                            return;

                        // 2.9 LAN_X_BC_PROGRAMMING_MODE
                        case 0x02:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_PROGRAMMING_MODE));
                            return;

                        // 2.10 LAN_X_BC_TRACK_SHORT_CIRCUIT
                        case 0x08:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_TRACK_SHORT_CIRCUIT));
                            return;

                        // 2.11 LAN_X_UNKNOWN_COMMAND
                        case 0x82:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_UNKNOWN_COMMAND));
                            return;
                        // 6.3 LAN_X_CV_NACK_SC
                        case 0x12:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_CV_NACK_SC));
                            return;
                        // 6.4 LAN_X_CV_NACK
                        case 0x13:
                            MessageReceived?.Invoke(new Message(MessageType.LAN_X_CV_NACK));
                            return;
                    }
                    return;

                // 2.12 LAN_X_STATUS_CHANGED
                case 0x62:
                    MessageReceived?.Invoke(new TrackStatusChangedMessage((CentralState)message[6]));
                    return;

                // 2.14 LAN_X_BC_STOPPED
                case 0x81:
                    MessageReceived?.Invoke(new Message(MessageType.LAN_X_BC_STOPPED));
                    return;

                // 2.15 LAN_X_GET_FIRMWARE_VERSION
                case 0xF3:
                    MessageReceived?.Invoke(FirmwareVersionMessage.Parse(message));
                    return;

                // 4.4 LAN_X_LOCO_INFO
                case 0xEF:
                    MessageReceived?.Invoke(LocoInfoMessage.Parse(message));
                    return;

                // 5.3 LAN_X_TURNOUT_INFO
                case 0x43:
                    MessageReceived?.Invoke(TurnoutInfoMessage.Parse(message));
                    return;

                // 5.6 LAN_X_EXT_ACCESSORY_INFO
                case 0x44:
                    MessageReceived?.Invoke(AccessoryInfoMessage.Parse(message));
                    return;

                // 6.5 LAN_X_CV_RESULT
                case 0x64:
                    MessageReceived?.Invoke(CVResultMessage.Parse(message));
                    return;
            }
        }
        #region Requests
        /// <summary>
        /// Sends buffer to Z21 command station
        /// </summary>
        /// <param name="data">Data buffer</param>
        public bool Send(byte[] data)
        {
            if (!Active || !_connected)
                return false;

            return _sendQueue.Writer.TryWrite(data);
        }

        /// <summary>
        /// 2.1 LAN_GET_SERIAL_NUMBER
        /// </summary>
        public bool LanGetSerialNumber()
        {
            return Send([0x04, 0x00, 0x10, 0x00]);
        }

        /// <summary>
        /// 2.2 LAN_LOGOFF
        /// </summary>
        public bool LanLogoff()
        {
            return Send([0x04, 0x00, 0x30, 0x00]);
        }

        /// <summary>
        /// 2.3 LAN_X_GET_VERSION
        /// </summary>
        public bool LanXGetVersion()
        {
            return Send([0x07, 0x00, 0x40, 0x00, 0x21, 0x21, 0x00]);
        }

        /// <summary>
        /// 2.5 LAN_X_SET_TRACK_POWER_OFF
        /// +
        /// 2.6 LAN_X_SET_TRACK_POWER_ON 
        /// </summary>
        public bool LanXSetTrackPower(bool power)
        {
            return Send(RequestMessage.CreateXBUS([0x21, (byte)(power ? 0x81 : 0x80)]));
        }

        /// <summary>
        /// 2.13 LAN_X_SET_STOP
        /// </summary>
        public bool LanXSetStop()
        {
            return Send([0x06, 0x00, 0x40, 0x00, 0x80, 0x80]);
        }

        /// <summary>
        /// 2.15 LAN_X_GET_FIRMWARE_VERSION 
        /// </summary>
        public bool LanXGetFirmwareVersion()
        {
            return Send([0x07, 0x00, 0x40, 0x00, 0xF1, 0x0A, 0xFB]);
        }

        /// <summary>
        /// 2.16 LAN_SET_BROADCASTFLAGS
        /// </summary>
        /// <param name="flags">Required broadcast messages</param>
        public bool LanSetBroadcastflags(BroadcastFlags flags)
        {
            return Send(RequestMessage.Create(0x50, LE.Parse((uint)flags)));
        }

        /// <summary>
        /// 2.17 LAN_GET_BROADCASTFLAGS
        /// </summary>
        public bool LanGetBroadcastflags()
        {
            return Send([0x04, 0x00, 0x51, 0x00]);
        }

        /// <summary>
        /// 2.19 LAN_SYSTEMSTATE_GETDATA
        /// </summary>
        public bool LanSystemstateGetdata()
        {
            return Send([0x04, 0x00, 0x85, 0x00]);
        }

        /// <summary>
        /// 2.20 LAN_GET_HWINFO
        /// </summary>
        public bool LanGetHwinfo()
        {
            return Send([0x04, 0x00, 0x1A, 0x00]);
        }

        /// <summary>
        /// 2.21 LAN_GET_CODE
        /// </summary>
        public bool LanGetCode()
        {
            return Send([0x04, 0x00, 0x18, 0x00]);
        }

        /// <summary>
        /// 3.1 LAN_GET_LOCOMODE
        /// </summary>
        /// <param name="address">Locomotive address (big endian)</param>
        public bool LanGetLocomode(LocoAddress address)
        {
            return Send(RequestMessage.Create(0x60, BE.Parse(address.Address)));
        }

        /// <summary>
        /// 3.2 LAN_SET_LOCOMODE
        /// </summary>
        /// <param name="address">Locomotive address (big endian)</param>
        /// <param name="mode">Locomotive mode </param>
        public bool LanSetLocomode(LocoAddress address, DecoderMode mode)
        {
            return Send(RequestMessage.CreateMode(0x61, BE.Parse(address.Address), (byte)mode));
        }

        /// <summary>
        /// 3.3 LAN_GET_TURNOUTMODE
        /// </summary>
        /// <param name="address">Accessory address</param>
        public bool LanGetTurnoutmode(Address address)
        {
            return Send(RequestMessage.Create(0x70, [address.MSB, address.LSB]));
        }

        /// <summary>
        /// 3.4 LAN_GET_TURNOUTMODE 
        /// </summary>
        /// <param name="address">Accessory address</param>
        /// <param name="mode">Accessory mode</param>
        public bool LanSetTurnoutmode(Address address, DecoderMode mode)
        {
            return Send(RequestMessage.CreateMode(0x71, [address.MSB, address.LSB], (byte)mode));
        }

        /// <summary>
        /// 4.1 LAN_X_GET_LOCO_INFO
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public bool LanXGetLocoInfo(LocoAddress address)
        {
            return Send(RequestMessage.CreateXBUS([0xE3, 0xF0, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 4.2 LAN_X_SET_LOCO_DRIVE
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="speed">Speed</param>
        public bool LanXSetLocoDrive(LocoAddress address, LocoSpeed speed)
        {
            return Send(RequestMessage.CreateXBUS([0xE4, (byte)(0x10 + speed.SpeedSteps), address.MSB, address.LSB, speed.GetByte()]));
        }

        /// <summary>
        /// 4.3.1 LAN_X_SET_LOCO_FUNCTION
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="state">Function state</param>
        /// <param name="function">Function address (0-31)</param>
        public bool LanXSetLocoFunction(LocoAddress address, FunctionSwitch state, byte function)
        {
            if (function > 31)
                throw Extensions.GetException(nameof(function), "Function must be between 0 and 31!");

            return Send(RequestMessage.CreateXBUS([0xE4, 0xF8, address.MSB, address.LSB, (byte)(((byte)state << 6) + function)]));
        }

        /// <summary>
        /// 4.3.2 LAN_X_SET_LOCO_FUNCTION_GROUP
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="group">Group code (see protocol specification)</param>
        /// <param name="functions">Group data (see protocol specification)</param>
        public bool LanXSetLocoFunctionGroup(LocoAddress address, byte group, byte functions)
        {
            return Send(RequestMessage.CreateXBUS([0xE4, group, address.MSB, address.LSB, functions]));
        }

        /// <summary>
        /// 4.3.3 LAN_X_SET_LOCO_BINARY_STATE
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="on">Binary state is ON</param>
        /// <param name="low">Data low byte</param>
        /// <param name="high">Data high byte</param>
        public bool LanXSetLocoBinaryState(LocoAddress address, bool on, byte low, byte high)
        {
            low = (byte)(low & 0x7F | (on ? 0x80 : 0x00));
            return Send(RequestMessage.CreateXBUS([0xE5, 0x5F, address.MSB, address.LSB, low, high]));
        }

        /// <summary>
        /// 4.5 LAN_X_SET_LOCO_E_STOP
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public bool LanXSetLocoEStop(LocoAddress address)
        {
            return Send(RequestMessage.CreateXBUS([0x92, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 4.6 LAN_X_PURGE_LOCO
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public bool LanXPurgeLoco(LocoAddress address)
        {
            return Send(RequestMessage.CreateXBUS([0xE3, 0x44, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 5.1 LAN_X_GET_TURNOUT_INFO
        /// </summary>
        /// <param name="address">Function address</param>
        public bool LanXGetTurnoutInfo(Address address)
        {
            return Send(RequestMessage.CreateXBUS([0x43, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 5.2 LAN_X_SET_TURNOUT
        /// </summary>
        /// <param name="address">Function address</param>
        /// <param name="activate"><see langword="true"/>: activate output | <see langword="false"/>: deactivate output</param>
        /// <param name="output"><see langword="true"/>: select output 2 | <see langword="false"/>: select output 1</param>
        /// <param name="useQueue">Use queue (see protocol specification)</param>
        public bool LanXSetTurnout(Address address, bool activate, bool output, bool useQueue)
        {
            byte data = 0b1000_0000;
            if (activate)
                data |= 0b0000_1000;
            if (output)
                data |= 0b0000_0001;
            if (useQueue)
                data |= 0b0010_0000;
            return Send(RequestMessage.CreateXBUS([0x53, address.MSB, address.LSB, data]));
        }

        /// <summary>
        /// 5.4 LAN_X_SET_EXT_ACCESSORY
        /// </summary>
        /// <param name="address">Function address</param>
        /// <param name="data">Extended accessory data (see accessory documentation)</param>
        public bool LanXSetExtAccessory(Address address, byte data)
        {
            return Send(RequestMessage.CreateXBUS([0x54, address.MSB, address.LSB, data, 0x00]));
        }

        /// <summary>
        /// 5.6 LAN_X_GET_EXT_ACCESSORY_INFO
        /// </summary>
        /// <param name="address">Function address</param>
        public bool LanXGetExtAccessoryInfo(Address address)
        {
            return Send(RequestMessage.CreateXBUS([0x44, address.MSB, address.LSB, 0x00]));
        }

        /// <summary>
        /// 6.1 LAN_X_CV_READ 
        /// </summary>
        /// <param name="address">CV address</param>
        public bool LanXCVRead(Address address)
        {
            return Send(RequestMessage.CreateXBUS([0x23, 0x11, address.MSB, address.LSB]));
        }

        /// <summary>
        /// 6.2 LAN_X_CV_WRITE
        /// </summary>
        /// <param name="address">CV address</param>
        /// <param name="value">CV value</param>
        public bool LanXCVWrite(Address address, byte value)
        {
            return Send(RequestMessage.CreateXBUS([0x24, 0x12, address.MSB, address.LSB, value]));
        }

        /// <summary>
        /// 6.6 LAN_X_CV_POM_WRITE_BYTE
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="value">CV value</param>
        public bool LanXCVPOMWriteByte(LocoAddress addressL, Address addressC, byte value)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");

            byte db3 = (byte)(0xEC + (addressC.MSB & 3));
            return Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, value]));
        }

        /// <summary>
        /// 6.7 LAN_X_CV_POM_WRITE_BIT 
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="position">Bit position</param>
        /// <param name="value">Bit value</param>
        public bool LanXCVPOMWriteBit(LocoAddress addressL, Address addressC, byte position, bool value)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");
            if (position > 7)
                throw Extensions.GetException(nameof(position), "Position mush be between 0 and 7!");

            byte db3 = (byte)(0xE8 + (addressC.MSB & 3));
            byte db5 = (byte)(position + (value ? 8 : 0));
            return Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, db5]));
        }

        /// <summary>
        /// 6.8 LAN_X_CV_POM_READ_BYTE
        /// </summary>
        /// <param name="addressL">Locomotive address</param>
        /// <param name="addressC">CV address</param>
        public bool LanXCVPOMReadByte(LocoAddress addressL, Address addressC)
        {
            if (addressC.Value > 0x3FF)
                throw Extensions.GetException(nameof(addressC), "CV address must be between 0 and 1023!");

            byte db3 = (byte)(0xE4 + (addressC.MSB & 3));
            return Send(RequestMessage.CreateXBUS([0xE6, 0x30, addressL.MSB, addressL.LSB, db3, addressC.LSB, 0]));
        }

        /// <summary>
        /// 6.9 LAN_X_CV_POM_ACCESSORY_WRITE_BYTE
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        /// <param name="value">Byte value</param>
        public bool LanXCVPomAccessoryWriteByte(Address addressF, Address addressC, byte output, byte value)
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
            return Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, value]));
        }

        /// <summary>
        /// 6.10 LAN_X_CV_POM_ACCESSORY_WRITE_BIT
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        /// <param name="position">Bit position</param>
        /// <param name="value">Bit value</param>
        public bool LanXCVPomAccessoryWriteBit(Address addressF, Address addressC, byte output, byte position, bool value)
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
            return Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, db5]));
        }

        /// <summary>
        /// 6.11 LAN_X_CV_POM_ACCESSORY_READ_BYTE
        /// </summary>
        /// <param name="addressF">Accessory address</param>
        /// <param name="addressC">CV address</param>
        /// <param name="output">Decoder output (0 = all)</param>
        public bool LanXCVPOMAccessoryReadByte(Address addressF, Address addressC, byte output)
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
            return Send(RequestMessage.CreateXBUS([0xE6, 0x31, db1, db2, db3, addressC.LSB, 0]));
        }

        /// <summary>
        /// 6.12 LAN_X_MM_WRITE_BYTE 
        /// </summary>
        /// <param name="regAdr">Register address (0=Register1, 1=Register2, ...)</param>
        /// <param name="value">Register value</param>
        public bool LanXMMWriteByte(byte regAdr, byte value)
        {
            return Send(RequestMessage.CreateXBUS([0x24, 0xFF, 0, regAdr, value]));
        }

        /// <summary>
        /// 6.13 LAN_X_DCC_READ_REGISTER
        /// </summary>
        /// <param name="register">Register (0x01=Register1, 0x02=Register2, ...)</param>
        public bool LanXDCCReadRegister(byte register)
        {
            return Send(RequestMessage.CreateXBUS([0x22, 0x11, register]));
        }

        /// <summary>
        /// 6.14 LAN_X_DCC_WRITE_REGISTER
        /// </summary>
        /// <param name="register">Register (0x01=Register1, 0x02=Register2, ...)</param>
        /// <param name="value">Register value</param>
        public bool LanXDCCWriteRegister(byte register, byte value)
        {
            return Send(RequestMessage.CreateXBUS([0x23, 0x12, register, value]));
        }

        /// <summary>
        /// 7.2 LAN_RMBUS_GETDATA
        /// </summary>
        /// <param name="group">Group index</param>
        public bool LanRmbusGetdata(byte group)
        {
            return Send([0x05, 0x00, 0x81, 0x00, group]);
        }

        /// <summary>
        /// 7.3 LAN_RMBUS_PROGRAMMODULE
        /// </summary>
        /// <param name="address">R-BUS address</param>
        public bool LanRmbusProgrammodule(byte address)
        {
            return Send([0x05, 0x00, 0x82, 0x00, address]);
        }

        /// <summary>
        /// 8.2 LAN_RAILCOM_GETDATA
        /// </summary>
        /// <param name="address">Locomotive address</param>
        /// <param name="type">Data poll type</param>
        public bool LanRailcomGetdata(LocoAddress address, byte type = 0x01)
        {
            return Send([0x07, 0x00, 0x89, 0x00, type, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 9.3 LAN_LOCONET_FROM_LAN
        /// </summary>
        /// <param name="data">LocoNet message (excl. checksum)</param>
        public bool LanLoconetFromLan(byte[] data)
        {
            return Send(RequestMessage.CreateLocoNet(0xA2, data));
        }

        /// <summary>
        /// 9.4 LAN_LOCONET_DISPATCH_ADDR
        /// </summary>
        /// <param name="address">Locomotive address</param>
        public bool LanLoconetDispatchAddr(LocoAddress address)
        {
            return Send([0x06, 0x00, 0xA3, 0x00, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 9.5 LAN_LOCONET_DETECTOR
        /// </summary>
        /// <param name="type">Request type</param>
        /// <param name="address">Report address</param>
        public bool LanLoconetDetector(LoconetRequestType type, Address address)
        {
            return Send([0x07, 0x00, 0xA4, 0x00, (byte)type, address.LSB, address.MSB]);
        }

        /// <summary>
        /// 10.1 LAN_CAN_DETECTOR
        /// </summary>
        /// <param name="type">Request type</param>
        /// <param name="networkId">CAN-Network ID</param>
        public bool LanCanDetector(byte type, Address networkId)
        {
            return Send(RequestMessage.Create(0xC4, [type, networkId.LSB, networkId.MSB]));
        }

        /// <summary>
        /// 10.2.1 LAN_CAN_DEVICE_GET_DESCRIPTION
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        public bool LanCanDeviceGetDescription(Address networkId)
        {
            return Send(RequestMessage.Create(0xC8, [networkId.LSB, networkId.MSB]));
        }

        /// <summary>
        /// 10.2.2 LAN_CAN_DEVICE_SET_DESCRIPTION
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        /// <param name="description">CAN device description (max length: 16)</param>
        public bool LanCanDeviceSetDescription(Address networkId, string description)
        {
            byte[] data = new byte[18];
            Array.Copy(LE.Parse(networkId.Value), data, 2);
            Array.Copy(Utils.ConvertString(description, 16), 0, data, 2, 16);
            return Send(RequestMessage.Create(0xC9, data));
        }

        /// <summary>
        /// 10.2.4 LAN_CAN_BOOSTER_SET_TRACKPOWER
        /// </summary>
        /// <param name="networkId">CAN-Network ID</param>
        /// <param name="power">Power mode</param>
        public bool LanCanBoosterSetTrackpower(Address networkId, Power power)
        {
            return Send(RequestMessage.Create(0xCB, [networkId.LSB, networkId.MSB, (byte)power]));
        }

        /// <summary>
        /// 11.1.1.1 LAN_ZLINK_GET_HWINFO
        /// </summary>
        public bool LanZlinkGetHwinfo()
        {
            return Send([0x05, 0x00, 0xE8, 0x00, 0x06]);
        }

        /// <summary>
        /// 11.2.1 LAN_BOOSTER_GET_DESCRIPTION
        /// </summary>
        public bool LanBoosterGetDescription()
        {
            return Send([0x04, 0x00, 0xB8, 0x00]);
        }

        /// <summary>
        /// 11.2.2 LAN_BOOSTER_SET_DESCRIPTION
        /// </summary>
        /// <param name="description">Description as Latin-1 string (max length: 32, recommended: 16)</param>
        public bool LanBoosterSetDescription(string description)
        {
            return Send(RequestMessage.Create(0xB9, Utils.ConvertString(description, 32)));
        }

        /// <summary>
        /// 11.2.3 LAN_BOOSTER_SYSTEMSTATE_GETDATA
        /// </summary>
        public bool LanBoosterSystemstateGetdata()
        {
            return Send([0x04, 0x00, 0xBB, 0x00]);
        }

        /// <summary>
        /// 11.2.5 LAN_BOOSTER_SET_POWER
        /// </summary>
        /// <param name="port">Booster port (flags)</param>
        /// <param name="activate">Is active</param>
        public bool LanBoosterSetPower(BoosterPort port, bool activate)
        {
            return Send(RequestMessage.Create(0xB2, [(byte)port, (byte)(activate ? 0x01 : 0x00)]));
        }

        /// <summary>
        /// 11.3.1 LAN_DECODER_GET_DESCRIPTION
        /// </summary>
        public bool LanDecoderGetDescription()
        {
            return Send([0x04, 0x00, 0xD8, 0x00]);
        }

        /// <summary>
        /// 11.3.2 LAN_DECODER_SET_DESCRIPTION
        /// </summary>
        /// <param name="description">Description as Latin-1 string (max length: 32, recommended: 16)</param>
        public bool LanDecoderSetDescription(string description)
        {
            return Send(RequestMessage.Create(0xD9, Utils.ConvertString(description, 32)));
        }

        /// <summary>
        /// 11.3.3 LAN_DECODER_SYSTEMSTATE_GETDATA
        /// </summary>
        public bool LanDecoderSystemstateGetdata()
        {
            return Send([0x04, 0x00, 0xDB, 0x00]);
        }

        /// <summary>
        /// 12.1.1 Get Fast Clock Time
        /// </summary>
        /// <remarks>
        /// 12.1 LAN_FAST_CLOCK_CONTROL
        /// </remarks>
        public bool LanFastClockControl_GetFastClockTime()
        {
            return Send([0x07, 0x00, 0xCC, 0x00, 0x21, 0x2A, 0x0B]);
        }

        /// <summary>
        /// 12.1.2 Set Fast Clock Time
        /// </summary>
        /// <remarks>
        /// 12.1 LAN_FAST_CLOCK_CONTROL
        /// </remarks>
        /// <param name="time">Time to be set</param>
        /// <param name="rate">Clock rate (0 - stopped, 1 - real time, 2 - 2x real time, 3 - 3x real time, ..., 63 - 63x real time)</param>
        public bool LanFastClockControl_SetFastClockTime(DateTime time, byte rate)
        {
            if (rate > 63)
                Extensions.GetException(nameof(rate), "Rate must be between 0 and 63!");

            byte data2 = (byte)(GetDayOfWeek(time.DayOfWeek) + time.Hour);
            byte data3 = (byte)time.Minute;

            return Send(RequestMessage.CreateXOR(0xCC, [0x24, 0x2B, data2, data3, rate]));
        }

        /// <summary>
        /// 12.1.3 Start Fast Clock Time
        /// </summary>
        /// <remarks>
        /// 12.1 LAN_FAST_CLOCK_CONTROL
        /// </remarks>
        public bool LanFastClockControl_StartFastClockTime()
        {
            return Send([0x07, 0x00, 0xCC, 0x00, 0x21, 0x2C, 0x0D]);
        }

        /// <summary>
        /// 12.1.4 Stop Fast Clock Time
        /// </summary>
        /// <remarks>
        /// 12.1 LAN_FAST_CLOCK_CONTROL
        /// </remarks>
        public bool LanFastClockControl_StopFastClockTime()
        {
            return Send([0x07, 0x00, 0xCC, 0x00, 0x21, 0x2D, 0x0C]);
        }

        /// <summary>
        /// 12.3 LAN_FAST_CLOCK_SETTINGS_GET
        /// </summary>
        public bool LanFastClockSettingsGet()
        {
            return Send([0x05, 0x00, 0xCE, 0x00, 0x04]);
        }

        /// <summary>
        /// 12.4 LAN_FAST_CLOCK_SETTINGS_SET
        /// </summary>
        /// <param name="settings">Settings flags</param>
        public bool LanFastClockSettingsSet(FcSettings settings)
        {
            return Send([0x05, 0x00, 0xCF, 0x00, (byte)settings]);
        }

        /// <summary>
        /// 12.4 LAN_FAST_CLOCK_SETTINGS_SET
        /// </summary>
        /// <param name="settings">Settings flags</param>
        /// <param name="rate">Clock rate (0 - stopped, 1 - real time, 2 - 2x real time, 3 - 3x real time, ..., 63 - 63x real time)</param>
        public bool LanFastClockSettingsSet(FcSettings settings, byte rate)
        {
            if (rate > 63)
                Extensions.GetException(nameof(rate), "Rate must be between 0 and 63!");

            return Send([0x06, 0x00, 0xCF, 0x00, (byte)settings, rate]);
        }

        /// <summary>
        /// 12.4 LAN_FAST_CLOCK_SETTINGS_SET
        /// </summary>
        /// <param name="settings">Settings flags</param>
        /// <param name="rate">Clock rate (0 - stopped, 1 - real time, 2 - 2x real time, 3 - 3x real time, ..., 63 - 63x real time)</param>
        /// <param name="time">Time to be set</param>
        public bool LanFastClockSettingsSet(FcSettings settings, byte rate, DateTime time)
        {
            if (rate > 63)
                Extensions.GetException(nameof(rate), "Rate must be between 0 and 63!");

            byte data2 = (byte)(GetDayOfWeek(time.DayOfWeek) + time.Hour);
            byte data3 = (byte)time.Minute;

            return Send([0x08, 0x00, 0xCF, 0x00, (byte)settings, rate, data2, data3]);
        }
        #endregion

        private static int GetDayOfWeek(DayOfWeek day)
        {
            return (((int)day + 6) % 7) << 5;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connected = false;

                // Cancel sending task
                _cancellationTokenSource?.Cancel();

                // Wait for sending task to complete
                try
                {
                    _sendingTask?.Wait(TimeSpan.FromSeconds(5));
                }
                catch (AggregateException)
                {
                    // Ignore cancellation exceptions
                }

                // Dispose resources
                _cancellationTokenSource?.Dispose();
                _sendingTask?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}