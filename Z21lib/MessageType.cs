﻿namespace Z21lib
{
    public enum MessageType
    {
        LAN_GET_SERIAL_NUMBER,
        LAN_LOGOFF,
        LAN_X_GET_VERSION,
        LAN_X_GET_STATUS,
        LAN_X_SET_TRACK_POWER_OFF,
        LAN_X_SET_TRACK_POWER_ON,
        LAN_X_BC_TRACK_POWER_OFF,
        LAN_X_BC_TRACK_POWER_ON,
        LAN_X_BC_PROGRAMMING_MODE,
        LAN_X_BC_TRACK_SHORT_CIRCUIT,
        LAN_X_UNKNOWN_COMMAND,
        LAN_X_STATUS_CHANGED,
        LAN_X_SET_STOP,
        LAN_X_BC_STOPPED,
        LAN_X_GET_FIRMWARE_VERSION,
        LAN_SET_BROADCASTFLAGS,
        LAN_GET_BROADCASTFLAGS,
        LAN_SYSTEMSTATE_DATACHANGED,
        LAN_SYSTEMSTATE_GETDATA,
        LAN_GET_HWINFO,
        LAN_GET_CODE,
        LAN_GET_LOCOMODE,
        LAN_SET_LOCOMODE,
        LAN_GET_TURNOUTMODE,
        LAN_SET_TURNOUTMODE,
        LAN_X_GET_LOCO_INFO,
        LAN_X_SET_LOCO_DRIVE,
        LAN_X_SET_LOCO_FUNCTION,
        LAN_X_LOCO_INFO,
        LAN_X_GET_TURNOUT_INFO,
        LAN_X_SET_TURNOUT,
        LAN_X_TURNOUT_INFO,
        LAN_X_SET_EXT_ACCESSORY,
        LAN_X_GET_EXT_ACCESSORY_INFO,
        LAN_X_EXT_ACCESSORY_INFO,
        LAN_X_CV_READ,
        LAN_X_CV_WRITE,
        LAN_X_CV_NACK_SC,
        LAN_X_CV_NACK,
        LAN_X_CV_RESULT,
        LAN_X_CV_POM_WRITE_BYTE,
        LAN_X_CV_POM_WRITE_BIT,
        LAN_X_CV_POM_READ_BYTE,
        LAN_X_CV_POM_ACCESSORY_WRITE_BYTE,
        LAN_X_CV_POM_ACCESSORY_WRITE_BIT,
        LAN_X_CV_POM_ACCESSORY_READ_BYTE,
        LAN_X_MM_WRITE_BYTE,
        LAN_X_DCC_READ_REGISTER,
        LAN_X_DCC_WRITE_REGISTER,
        LAN_RMBUS_DATACHANGED,
        LAN_RMBUS_GETDATA,
        LAN_RMBUS_PROGRAMMODULE,
        LAN_RAILCOM_DATACHANGED,
        LAN_RAILCOM_GETDATA,
        LAN_LOCONET_Z21_RX,
        LAN_LOCONET_Z21_TX,
        LAN_LOCONET_FROM_LAN,
        LAN_LOCONET_DISPATCH_ADDR,
        LAN_LOCONET_DETECTOR,
        LAN_CAN_DETECTOR,
        LAN_ZLINK_GET_HWINFO,
        LAN_BOOSTER_GET_DESCRIPTION,
        LAN_BOOSTER_SYSTEMSTATE_GETDATA,
        LAN_BOOSTER_SYSTEMSTATE_DATACHANGED,
        LAN_DECODER_GET_DESCRIPTION,
        LAN_DECODER_SYSTEMSTATE_GETDATA,
        LAN_DECODER_SYSTEMSTATE_DATACHANGED,
        NOT_IMPLEMENTED
    }
}
