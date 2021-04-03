using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib
{
    public struct AccesoryAddress
    {
        public int Address { get; set; }

        public byte MSB
        {
            get
            {
                return (byte)(Address >> 8);
            }
        }

        public byte LSB
        {
            get
            {
                return (byte)(Address % 256);
            }
        }

        public AccesoryAddress(int address)
        {
            Address = address;
        }

        public AccesoryAddress(byte msb, byte lsb)
        {
            Address = (msb << 8) + lsb;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(AccesoryAddress) && (AccesoryAddress)obj == this;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override string ToString()
        {
            return $"A{Address} [{MSB}, {LSB}]";
        }

        public static bool operator ==(AccesoryAddress left, AccesoryAddress right)
        {
            return left.Address == right.Address;
        }

        public static bool operator !=(AccesoryAddress left, AccesoryAddress right)
        {
            return left.Address != right.Address;
        }
    }
}
