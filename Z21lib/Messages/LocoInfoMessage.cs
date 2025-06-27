using Z21lib.Enums;

namespace Z21lib.Messages
{
    public class LocoInfoMessage : Message
    {
        public LocoAddress Address { get; init; }

        public DecoderMode Mode { get; init; }

        public bool Busy { get; init; }

        public LocoSpeed Speed { get; init; }

        public bool DoubleTraction { get; init; }

        public bool SmartSearch { get; init; }

        public bool Light { get; init; }

        public bool[] Functions { get; init; }

        public string Func
        {
            get
            {
                string output = "";
                foreach (bool f in Functions)
                    output += f ? "1" : "0";
                return output;
            }
        }

        public LocoInfoMessage() : base(MessageType.LAN_X_LOCO_INFO)
        {
            Functions = new bool[29];
        }

        internal static LocoInfoMessage Parse(byte[] message)
        {
            bool busy = false;
            DecoderMode mode = DecoderMode.DCC;
            SpeedSteps steps = SpeedSteps.DCC14;
            LocoDirection direction = LocoDirection.Forward;
            byte speed = 0;
            bool doubleTraction = false;
            bool smartSearch = false;
            bool light = false;
            bool[] functions = new bool[32];

            if (message.Length > 8)
            {
                byte db2 = message[7];
                mode = db2.Bit(4) ? DecoderMode.Motorola : DecoderMode.DCC;
                busy = db2.Bit(3);
                steps = (SpeedSteps)(db2 & 0b111);
            }
            if (message.Length > 9)
            {
                byte db3 = message[8];
                direction = db3.Bit(7) ? LocoDirection.Forward : LocoDirection.Backward;
                speed = (byte)(db3 & 0x7F);
            }
            if (message.Length > 10)
            {
                byte db4 = message[9];
                doubleTraction = db4.Bit(6);
                smartSearch = db4.Bit(5);
                light = functions[0] = db4.Bit(4);
                functions[4] = db4.Bit(3);
                functions[3] = db4.Bit(2);
                functions[2] = db4.Bit(1);
                functions[1] = db4.Bit(0);
            }
            if (message.Length > 11)
            {
                FillFunctions(message[10], ref functions, 5, 8);
            }
            if (message.Length > 12)
            {
                FillFunctions(message[11], ref functions, 13, 8);
            }
            if (message.Length > 13)
            {
                FillFunctions(message[12], ref functions, 21, 8);
            }
            if (message.Length > 14)
            {
                FillFunctions(message[13], ref functions, 29, 3);
            }

            LocoInfoMessage lm = new()
            {
                Address = new LocoAddress(message[5], message[6]),
                Mode = mode,
                Busy = busy,
                Speed = LocoSpeed.Parse(steps, direction, speed),
                DoubleTraction = doubleTraction,
                SmartSearch = smartSearch,
                Light = light,
                Functions = functions
            };

            return lm;
        }

        private static void FillFunctions(byte data, ref bool[] functions, int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                functions[start + i] = data.Bit(i);
            }
        }

        private static int ParseSpeed(SpeedSteps steps, byte speedData)
        {
            byte l = (byte)(speedData & 0b1111);
            if (l == 0b0000)
                return 0;
            else if (l == 0b0001)
                return -1;

            switch (steps)
            {
                case SpeedSteps.DCC14:
                    return (speedData & 0b1111) - 1;
                case SpeedSteps.DCC28:
                    int i = (speedData & 0b10000) == 0b10000 ? 1 : 0;
                    return (speedData & 0b1111) - 1 + i;
                case SpeedSteps.DCC128:
                    return (speedData & 0b1111111) - 1;
                default:
                    return 0;
            }
        }
    }

    public enum Direction
    {
        Forward,
        Neutral,
        Backward
    }
}
