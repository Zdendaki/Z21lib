namespace Z21lib
{
    public struct AccessoryAddress
    {
        public int Address { get; set; }

        public byte MSB
        {
            get
            {
                return (byte)((Address - 1) >> 8);
            }
        }

        public byte LSB
        {
            get
            {
                return (byte)((Address - 1) % 256);
            }
        }

        public AccessoryAddress(int address)
        {
            Address = address;
        }

        public AccessoryAddress(byte msb, byte lsb)
        {
            Address = (msb << 8) + lsb + 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is not null && obj.GetType() == typeof(AccessoryAddress) && (AccessoryAddress)obj == this;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override string ToString()
        {
            return $"A{Address} [{MSB}, {LSB}]";
        }

        public static bool operator ==(AccessoryAddress left, AccessoryAddress right)
        {
            return left.Address == right.Address;
        }

        public static bool operator !=(AccessoryAddress left, AccessoryAddress right)
        {
            return left.Address != right.Address;
        }
    }
}
