using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z21lib.Messages
{
    public class LocoInfoMessage : Message
    {
        public LocoAddress Address { get; set; }

        public bool Busy { get; set; }

        public SpeedSteps SpeedSteps { get; set; }

        public Direction Direction { get; set; }

        public int Speed { get; set; }

        public bool DoubleTraction { get; set; }

        public bool SmartSearch { get; set; }

        public bool Light { get; set; }

        public bool[] Functions { get; set; }

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

        public static LocoInfoMessage Parse(byte[] message)
        {
            LocoInfoMessage lm = new LocoInfoMessage();
            lm.Address = new LocoAddress(message[5], message[6]);
            bool[] functions = new bool[29];

            if (message.Length > 8)
            {
                byte db2 = message[7];
                lm.Busy = db2.Bit(3);
                lm.SpeedSteps = (SpeedSteps)(db2 & 0b111);
            }
            if (message.Length > 9)
            {
                byte db3 = message[8];
                lm.Direction = db3.Bit(7) ? Direction.Forward : Direction.Backward;
                lm.Speed = ParseSpeed(lm.SpeedSteps, db3);
            }
            if (message.Length > 10)
            {
                byte db4 = message[9];
                lm.DoubleTraction = db4.Bit(6);
                lm.SmartSearch = db4.Bit(5);
                lm.Light = functions[0] = db4.Bit(4);
                functions[4] = db4.Bit(3);
                functions[3] = db4.Bit(2);
                functions[2] = db4.Bit(1);
                functions[1] = db4.Bit(0);
            }
            if (message.Length > 11)
            {
                FillFunctions(message[10], ref functions, 5);
            }
            if (message.Length > 12)
            {
                FillFunctions(message[11], ref functions, 13);
            }
            if (message.Length > 13)
            {
                FillFunctions(message[12], ref functions, 21);
            }

            return lm;
        }

        private static void FillFunctions(byte data, ref bool[] functions, int start)
        {
            functions[start + 7] = data.Bit(7);
            functions[start + 6] = data.Bit(6);
            functions[start + 5] = data.Bit(5);
            functions[start + 4] = data.Bit(4);
            functions[start + 3] = data.Bit(3);
            functions[start + 2] = data.Bit(2);
            functions[start + 1] = data.Bit(1);
            functions[start] = data.Bit(0);
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

    public enum SpeedSteps : byte
    {
        DCC14 = 0x0,
        DCC28 = 0x2,
        DCC128 = 0x4
    }

    public enum Direction
    {
        Forward,
        Neutral,
        Backward
    }
}
