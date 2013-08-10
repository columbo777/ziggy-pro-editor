using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum ChordQueryOption
    {
        Default = (0),
        IncludeEndingOnMin = (1 << 0),
        IncludeStartingOnMax = (1 << 1),
    }
}