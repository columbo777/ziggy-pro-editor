using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum AdjustResult
    {
        NoResult = 0,
        Success = (1 << 0),

        AdjustedDownTickLeft = (1 << 1),
        AdjustedDownTickRight = (1 << 2),

        AdjustedUpTickLeft = (1 << 3),
        AdjustedUpTickRight = (1 << 4),

        ShortResult = (1 << 5),

        Error = (1 << 6),

        AdjustedDownTick = (AdjustedDownTickLeft | AdjustedDownTickRight),
        AdjustedUpTick = (AdjustedUpTickLeft | AdjustedUpTickRight),
    }
}