using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public enum ChordModifierType
    {
        None,
        ChordStrumLow,
        ChordStrumMed,
        ChordStrumHigh,
        Slide,
        SlideReverse,
        Hammeron,
    }
}