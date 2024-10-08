﻿namespace Z21lib.Enums
{
    public enum MessageType
    {
        LAN_GET_SERIAL_NUMBER,
        LAN_X_GET_VERSION,
        LAN_X_BC_TRACK_POWER_OFF,
        LAN_X_BC_TRACK_POWER_ON,
        LAN_X_BC_PROGRAMMING_MODE,
        LAN_X_BC_TRACK_SHORT_CIRCUIT,
        LAN_X_UNKNOWN_COMMAND,
        LAN_X_STATUS_CHANGED,
        LAN_X_BC_STOPPED,
        LAN_X_GET_FIRMWARE_VERSION,
        LAN_GET_BROADCASTFLAGS,
        LAN_SYSTEMSTATE_DATACHANGED,
        LAN_GET_HWINFO,
        LAN_GET_CODE,
        LAN_GET_LOCOMODE,
        LAN_GET_TURNOUTMODE,
        LAN_X_LOCO_INFO,
        LAN_X_TURNOUT_INFO,
        LAN_X_EXT_ACCESSORY_INFO,
        LAN_X_CV_NACK_SC,
        LAN_X_CV_NACK,
        LAN_X_CV_RESULT,
        LAN_RMBUS_DATACHANGED,
        LAN_RAILCOM_DATACHANGED,
        LAN_LOCONET_Z21_RX,
        LAN_LOCONET_Z21_TX,
        LAN_LOCONET_FROM_LAN,
        LAN_LOCONET_DISPATCH_ADDR,
        LAN_LOCONET_DETECTOR,
        LAN_CAN_DETECTOR,
        LAN_CAN_DEVICE_GET_DESCRIPTION,
        LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD,
        LAN_ZLINK_GET_HWINFO,
        LAN_BOOSTER_GET_DESCRIPTION,
        LAN_BOOSTER_SYSTEMSTATE_DATACHANGED,
        LAN_DECODER_GET_DESCRIPTION,
        LAN_DECODER_SYSTEMSTATE_DATACHANGED_SWITCH,
        LAN_DECODER_SYSTEMSTATE_DATACHANGED_SIGNAL,
        LAN_FAST_CLOCK_DATA,
        LAN_FAST_CLOCK_SETTINGS_GET,
        NOT_IMPLEMENTED = int.MaxValue
    }
}
