namespace Z21lib.Structs
{
    public unsafe struct Hw_Info
    {
        public ushort HwID;
        public byte FW_Version_Major;
        public byte FW_Version_Minor;
        public ushort FW_Version_Build;
        public byte[] MAC_Address;
        public byte[] Name;
        public byte Reserved;
    }
}
