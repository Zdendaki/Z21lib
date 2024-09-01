namespace Z21lib
{
    public struct LocoAddress
    {
        public byte MSB
        {
            get
            {
                byte val = (byte)(Number >> 8);
                if (val >= 128)
                    val |= 0xC0;
                return val;
            }
        }

        public byte LSB { get => (byte)(Number % 256); }

        public int Number { get; set; }

        public LocoAddress(byte msb, byte lsb)
        {
            Number = ((msb & 0x3F) << 8) + lsb;
        }

        public LocoAddress(int number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return $"L{Number} [{MSB}, {LSB}]";
        }

        public override bool Equals(object? obj)
        {
            return obj is not null && obj.GetType() == typeof(LocoAddress) && (LocoAddress)obj == this;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public static bool operator ==(LocoAddress left, LocoAddress right)
        {
            return left.Number == right.Number;
        }

        public static bool operator !=(LocoAddress left, LocoAddress right)
        {
            return left.Number != right.Number;
        }
    }
}
