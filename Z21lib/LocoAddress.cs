namespace Z21lib
{
    public struct LocoAddress
    {
        public byte MSB
        {
            get
            {
                int num = (Address & 0x3F00) >> 8;
                if (LSB >= 128)
                    num |= 0xC0;
                return (byte)num;
            }
        }

        public byte LSB => (byte)(Address & 0xFF);

        public ushort Address { get; }

        public LocoAddress(byte msb, byte lsb)
        {
            Address = (ushort)(((msb & 0x3F) << 8) + lsb);

            if (Address > 10239)
                throw new ArgumentOutOfRangeException(nameof(msb), "Maximum allowed address is 10239.");
        }

        public LocoAddress(ushort number)
        {
            if (number > 10239)
                throw new ArgumentOutOfRangeException(nameof(number), "Maximum allowed address is 10239.");

            Address = number;
        }

        public override string ToString()
        {
            return $"L{Address} [{MSB}, {LSB}]";
        }
    }
}
