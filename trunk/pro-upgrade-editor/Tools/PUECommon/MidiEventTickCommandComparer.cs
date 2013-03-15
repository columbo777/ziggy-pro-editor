using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class MidiEventTickCommandComparer : IComparer<MidiEvent>
    {
        public int Compare(MidiEvent x, MidiEvent y)
        {
            if (x.AbsoluteTicks < y.AbsoluteTicks)
                return -1;
            if (x.AbsoluteTicks > y.AbsoluteTicks)
                return 1;
            if (x.IsOn && y.IsOff)
                return 1;
            if (x.IsOff && y.IsOn)
                return -1;
            return 0;
        }
    }
}