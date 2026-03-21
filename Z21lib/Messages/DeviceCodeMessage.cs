using System.Diagnostics.CodeAnalysis;
using Z21lib.Enums;

namespace Z21lib.Messages;

[method: SetsRequiredMembers]
public class DeviceCodeMessage(DeviceCode code) : Message(MessageType.LAN_GET_CODE)
{
    public required DeviceCode Code { get; init; } = code;
}

public enum DeviceCode : byte
{
    /// <summary>
    /// All features permitted
    /// </summary>
    Z21_NO_LOCK = 0x00,
    /// <summary>
    /// Z21 start: driving and switching is blocked
    /// </summary>
    Z21_START_LOCKED = 0x01,
    /// <summary>
    /// Z21 start: driving and switching is permitted
    /// </summary>
    Z21_START_UNLOCKED = 0x02
}
