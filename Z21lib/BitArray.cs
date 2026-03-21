namespace Z21lib
{
    public class BitArray
    {
        private readonly byte[] _array;

        public BitArray()
        {
            _array = new byte[8];

            for (int i = 0; i < 8; i++)
                _array[i] = 0;
        }

        public BitArray(params byte[] input)
        {
            _array = new byte[8];

            FromBits(input);
        }

        public BitArray(byte input)
        {
            _array = new byte[8];

            FromByte(input);
        }

        public void FromByte(byte input)
        {
            for (int i = 0; i < 8; i++)
            {
                _array[i] = (byte)(input.Bit(i) ? 1 : 0);
            }
        }

        public void FromBits(params byte[] input)
        {
            Array.Reverse(input);
            for (int i = 0; i < 8; i++)
            {
                _array[i] = (byte)(input[i] != 0 ? 1 : 0);
            }
        }

        public void SetBit(int index, byte value)
        {
            _array[index] = (byte)(value != 0 ? 1 : 0);
        }

        public byte ToByte()
        {
            byte output = 0;

            for (int i = 0; i < 8; i++)
            {
                output += (byte)(_array[i] << i);
            }

            return output;
        }
    }
}
