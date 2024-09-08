namespace Z21lib.Enums
{
    public enum HardwareType : uint
    {
        UNKNOWN = 0,
        /// <summary>
        /// Black Z21 (hardware variant from 2012)
        /// </summary>
        Z21_OLD = 0x00000200,
        /// <summary>
        /// Black Z21 (hardware variant from 2013)
        /// </summary>
        Z21_NEW = 0x00000201,
        /// <summary>
        /// SmartRail (from 2012)
        /// </summary>
        SMARTRAIL = 0x00000202,
        /// <summary>
        /// "White Z21" starter set variant (from 2013)
        /// </summary>
        Z21_SMALL = 0x00000203,
        /// <summary>
        /// "Z21 start" starter set variant (from 2016)
        /// </summary>
        Z21_START = 0x00000204,
        /// <summary>
        /// 10806 "Z21 Single Booster" (zLink)
        /// </summary>
        SINGLE_BOOSTER = 0x00000205,
        /// <summary>
        /// 10807 "Z21 Dual Booster" (zLink)
        /// </summary>
        DUAL_BOOSTER = 0x00000206,
        /// <summary>
        /// 10870 "Z21 XL Series (from 2020)
        /// </summary>
        Z21_XL = 0x00000211,
        /// <summary>
        /// 10869 "Z21 XL Booster" (from 2021, zLink)
        /// </summary>
        Z21_XL_BOOSTER = 0x00000212,
        /// <summary>
        /// 10836 "Z21 SwitchDecoder" (zLink)
        /// </summary>
        Z21_SWITCH_DECODER = 0x00000301,
        /// <summary>
        /// 10836 "Z21 SignalDecoder" (zLink)
        /// </summary>
        Z21_SIGNAL_DECODER = 0x00000302
    }
}
