namespace Z21lib.Enums
{
    public enum Power : byte
    {
        DeactivateAllOutputs = 0x00,
        ReactivateAllOutputs = 0xFF,
        DeactivateFirstOutput = 0x10,
        ReactivateFirstOutput = 0x11,
        DeactivateSecondOutput = 0x20,
        ReactivateSecondOutput = 0x22
    }
}
