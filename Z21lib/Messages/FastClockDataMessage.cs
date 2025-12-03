using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class FastClockDataMessage : Message
    {
        public byte[] Header { get; init; } = [0x66, 0x25];

        public DayOfWeek Day { get; init; }

        public TimeOnly Time { get; init; }

        public bool Stop { get; init; }

        public bool Halt { get; init; }

        public byte Rate { get; init; }

        public FcSettings Settings { get; init; }

        public byte XOR { get; init; }

        public FastClockDataMessage() : base(MessageType.LAN_FAST_CLOCK_DATA)
        {
        }

        internal static FastClockDataMessage Parse(ReadOnlySpan<byte> data)
        {
            return new FastClockDataMessage
            {
                Header = data.Slice(4, 2).ToArray(),
                Day = GetDay(data[6]),
                Time = GetTime(data.Slice(6, 3)),
                Stop = data[8].Bit(7),
                Halt = data[8].Bit(6),
                Rate = (byte)(data[9] & 0b0011_1111),
                Settings = (FcSettings)data[10],
                XOR = data[11]
            };
        }

        private static DayOfWeek GetDay(byte input)
        {
            byte rawDay = (byte)((input >> 5) & 0b111);
            if (rawDay == 0)
                rawDay = 6;
            else
                rawDay -= 1;

            return (DayOfWeek)rawDay;
        }

        private static TimeOnly GetTime(ReadOnlySpan<byte> data)
        {
            int hour = data[0] & 0b0001_1111;
            int minute = data[1] & 0b0011_1111;
            int second = data[2] & 0b0011_1111;

            return new TimeOnly(hour, minute, second);
        }
    }
}
