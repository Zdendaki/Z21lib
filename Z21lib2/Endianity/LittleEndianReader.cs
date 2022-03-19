using System;

namespace Z21lib.Endianity
{
    class LittleEndianReader : IDisposable
    {
        byte[] buffer;
        int position = 0;

        public LittleEndianReader(byte[] data)
        {
            buffer = data;
        }

        public short ReadInt16()
        {
            short output = LE.ToInt16(buffer, position);
            position += 2;
            return output;
        }

        public ushort ReadUInt16()
        {
            ushort output = LE.ToUInt16(buffer, position);
            position += 2;
            return output;
        }

        public int ReadInt32()
        {
            int output = LE.ToInt32(buffer, position);
            position += 4;
            return output;
        }

        public uint ReadUInt32()
        {
            uint output = LE.ToUInt32(buffer, position); 
            position += 4;
            return output;
        }

        public long ReadInt64()
        {
            long output = LE.ToInt64(buffer, position);
            position += 8;
            return output;
        }

        public ulong ReadUInt64()
        {
            ulong output = LE.ToUInt64(buffer, position);
            position += 8;
            return output;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)buffer[position++];
        }

        public byte ReadByte()
        {
            return buffer[position++];
        }

        public void Dispose()
        {
            buffer = null;
            position = 0;
        }
    }
}
