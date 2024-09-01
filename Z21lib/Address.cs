namespace Z21lib
{
    public readonly struct Address
    {
        public ushort Value { get; init; }

        public byte MSB
        {
            get
            {
                return (byte)(Value >> 8);
            }
        }

        public byte LSB
        {
            get
            {
                return (byte)(Value & 0xFF);
            }
        }

        public Address(ushort address)
        {
            Value = address;
        }

        public Address(byte msb, byte lsb)
        {
            Value = (ushort)((msb << 8) + lsb);
        }

        public override string ToString()
        {
            return $"A{Value} [{MSB}, {LSB}]";
        }
    }
}
