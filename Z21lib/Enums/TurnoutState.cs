namespace Z21lib.Enums
{
    public enum TurnoutState : byte
    {
        /// <summary>
        /// Turnout not switched yet
        /// </summary>
        NotSwitched = 0x0,
        /// <summary>
        /// Turnout is in position according to switching command P=0
        /// </summary>
        SwitchedStraight = 0x1,
        /// <summary>
        /// Turnout is in position according to switching command P=1
        /// </summary>
        SwitchedReverse = 0x2,
        /// <summary>
        /// Invalid combination
        /// </summary>
        Invalid = 0x3
    }
}
