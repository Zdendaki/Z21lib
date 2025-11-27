namespace Z21lib.Endianity
{
    ref struct LittleEndianReader
    {
        private ReadOnlySpan<byte> _buffer;
        private int _position;

        public LittleEndianReader(ReadOnlySpan<byte> data)
        {
            _buffer = data;
            _position = 0;
        }

        public LittleEndianReader(ReadOnlySpan<byte> data, int startPosition)
        {
            _buffer = data;
            _position = startPosition;
        }

        public readonly int Position => _position;

        public readonly int Remaining => _buffer.Length - _position;

        public short ReadInt16()
        {
            short output = LE.ToInt16(_buffer.Slice(_position));
            _position += 2;
            return output;
        }

        public ushort ReadUInt16()
        {
            ushort output = LE.ToUInt16(_buffer.Slice(_position));
            _position += 2;
            return output;
        }

        public int ReadInt32()
        {
            int output = LE.ToInt32(_buffer.Slice(_position));
            _position += 4;
            return output;
        }

        public uint ReadUInt32()
        {
            uint output = LE.ToUInt32(_buffer.Slice(_position));
            _position += 4;
            return output;
        }

        public long ReadInt64()
        {
            long output = LE.ToInt64(_buffer.Slice(_position));
            _position += 8;
            return output;
        }

        public ulong ReadUInt64()
        {
            ulong output = LE.ToUInt64(_buffer.Slice(_position));
            _position += 8;
            return output;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)_buffer[_position++];
        }

        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public ReadOnlySpan<byte> ReadBytes(int count)
        {
            ReadOnlySpan<byte> result = _buffer.Slice(_position, count);
            _position += count;
            return result;
        }

        public void Skip(int count)
        {
            _position += count;
        }
    }
}
