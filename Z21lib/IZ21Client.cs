using Z21lib.Enums;

namespace Z21lib
{
    public interface IZ21Client
    {
        int ErrorCount { get; }
        bool IsConnected { get; }

        event Z21Client.MessageReceivedEventHandler MessageReceived;

        bool Connect();
        void Disconnect();
        bool LanBoosterGetDescription();
        bool LanBoosterSetDescription(string description);
        bool LanBoosterSetPower(BoosterPort port, bool activate);
        bool LanBoosterSystemstateGetdata();
        bool LanCanBoosterSetTrackpower(Address networkId, Power power);
        bool LanCanDetector(byte type, Address networkId);
        bool LanCanDeviceGetDescription(Address networkId);
        bool LanCanDeviceSetDescription(Address networkId, string description);
        bool LanDecoderGetDescription();
        bool LanDecoderSetDescription(string description);
        bool LanDecoderSystemstateGetdata();
        bool LanFastClockControl_GetFastClockTime();
        bool LanFastClockControl_SetFastClockTime(DateTime time, byte rate);
        bool LanFastClockControl_StartFastClockTime();
        bool LanFastClockControl_StopFastClockTime();
        bool LanFastClockSettingsGet();
        bool LanFastClockSettingsSet(FcSettings settings);
        bool LanFastClockSettingsSet(FcSettings settings, byte rate);
        bool LanFastClockSettingsSet(FcSettings settings, byte rate, DateTime time);
        bool LanGetBroadcastflags();
        bool LanGetCode();
        bool LanGetHwinfo();
        bool LanGetLocomode(LocoAddress address);
        bool LanGetSerialNumber();
        bool LanGetTurnoutmode(Address address);
        bool LanLoconetDetector(LoconetRequestType type, Address address);
        bool LanLoconetDispatchAddr(LocoAddress address);
        bool LanLoconetFromLan(byte[] data);
        bool LanLogoff();
        bool LanRailcomGetdata(LocoAddress address, byte type = 1);
        bool LanRmbusGetdata(byte group);
        bool LanRmbusProgrammodule(byte address);
        bool LanSetBroadcastflags(BroadcastFlags flags);
        bool LanSetLocomode(LocoAddress address, DecoderMode mode);
        bool LanSetTurnoutmode(Address address, DecoderMode mode);
        bool LanSystemstateGetdata();
        bool LanXCVPOMAccessoryReadByte(Address addressF, Address addressC, byte output);
        bool LanXCVPomAccessoryWriteBit(Address addressF, Address addressC, byte output, byte position, bool value);
        bool LanXCVPomAccessoryWriteByte(Address addressF, Address addressC, byte output, byte value);
        bool LanXCVPOMReadByte(LocoAddress addressL, Address addressC);
        bool LanXCVPOMWriteBit(LocoAddress addressL, Address addressC, byte position, bool value);
        bool LanXCVPOMWriteByte(LocoAddress addressL, Address addressC, byte value);
        bool LanXCVRead(Address address);
        bool LanXCVWrite(Address address, byte value);
        bool LanXDCCReadRegister(byte register);
        bool LanXDCCWriteRegister(byte register, byte value);
        bool LanXGetExtAccessoryInfo(Address address);
        bool LanXGetFirmwareVersion();
        bool LanXGetLocoInfo(LocoAddress address);
        bool LanXGetTurnoutInfo(Address address);
        bool LanXGetVersion();
        bool LanXMMWriteByte(byte regAdr, byte value);
        bool LanXPurgeLoco(LocoAddress address);
        bool LanXSetExtAccessory(Address address, byte data);
        bool LanXSetLocoBinaryState(LocoAddress address, bool on, byte low, byte high);
        bool LanXSetLocoDrive(LocoAddress address, LocoSpeed speed);
        bool LanXSetLocoEStop(LocoAddress address);
        bool LanXSetLocoFunction(LocoAddress address, FunctionSwitch state, byte function);
        bool LanXSetLocoFunctionGroup(LocoAddress address, byte group, byte functions);
        bool LanXSetStop();
        bool LanXSetTrackPower(bool power);
        bool LanXSetTurnout(Address address, bool activate, bool output, bool useQueue);
        bool LanZlinkGetHwinfo();
        bool Send(byte[] data);
    }
}