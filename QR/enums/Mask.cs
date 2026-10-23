using System;
using System.Collections.Generic;
using System.Text;

namespace QR
{
    public enum Mask : byte
    {
        /// <summary>
        /// ■■■
        /// </summary>
        M_000 = 0,
        /// <summary>
        /// ■■_
        /// </summary>
        M_001 = 1,
        /// <summary>
        /// ■_■
        /// </summary>
        M_010 = 2,
        /// <summary>
        /// __■
        /// </summary>
        M_011 = 3,
        /// <summary>
        /// _■■
        /// </summary>
        M_100 = 4,
        /// <summary>
        /// _■_
        /// </summary>
        M_101 = 5,
        /// <summary>
        /// __■
        /// </summary>
        M_110 = 6,
        /// <summary>
        /// ___
        /// </summary>
        M_111 = 7
    }
}
