using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum AdjustOption
    {
        NoAdjust = (0),
        AllowGrow = (1 << 0),
        AllowShrink = (1 << 1),
        AllowShift = (1 << 2),
        AllowAny = (AllowGrow | AllowShrink | AllowShift),
    }
}