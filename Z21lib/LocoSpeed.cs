using Z21lib.Enums;

namespace Z21lib
{
    public readonly struct LocoSpeed
    {
        public bool EmergencyStop { get; init; }

        public SpeedSteps SpeedSteps { get; init; }

        public LocoDirection Direction { get; init; }

        public byte Speed { get; init; }

        public LocoSpeed(SpeedSteps steps, LocoDirection direction, byte speed, bool emergencyStop = false)
        {
            if (steps == SpeedSteps.DCC14 && speed > 14)
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between 0 and 14 for DCC14 speed steps.");
            if (steps == SpeedSteps.DCC28 && speed > 28)
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between 0 and 28 for DCC28 speed steps.");
            if (steps == SpeedSteps.DCC128 && speed > 126)
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between 0 and 126 for DCC128 speed steps.");

            SpeedSteps = steps;
            Direction = direction;
            Speed = speed;
            EmergencyStop = emergencyStop;
        }

        public static byte GetEmergencyStop(LocoDirection direction) => (byte)(direction + 1);

        public static byte GetStop(LocoDirection direction) => (byte)direction;

        public byte GetByte()
        {
            byte output = (byte)Direction;

            if (EmergencyStop)
                return (byte)(output + 1);
            if (Speed == 0)
                return output;

            if (SpeedSteps == SpeedSteps.DCC28)
            {
                byte even = (byte)((Speed & 1 ^ 1) << 4);
                return (byte)(output + even + (Speed + 1 >> 1) + 1);
            }
            else
            {
                return (byte)(output + Speed + 1);
            }
        }

        public static LocoSpeed Parse(SpeedSteps steps, LocoDirection direction, byte speed)
        {
            speed = (byte)(speed & 0x7F); // 0111_1111
            bool emergencyStop;
            if (steps == SpeedSteps.DCC28)
                emergencyStop = (speed & 0xF) == 1;
            else
                emergencyStop = speed == 1;

            byte parseSpeed()
            {
                if (speed == 0)
                    return 0;

                if (steps != SpeedSteps.DCC28)
                    return (byte)(speed - 1);

                if ((speed & 0xF) == 0) // 1111
                    return 0;

                byte even = (byte)((speed & 0b10000) >> 4);
                return (byte)(((speed & 0b1111) << 1) - 3 + even);
            }

            return new LocoSpeed(steps, direction, parseSpeed(), emergencyStop);
        }
    }
}
