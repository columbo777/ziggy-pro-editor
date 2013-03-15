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
    public class MidiEventPairInterlacingSorter : IComparer<MidiEventPair>
    {
        public int Compare(MidiEventPair a, MidiEventPair b)
        {
            if (a.Down.AbsoluteTicks < b.Down.AbsoluteTicks)
                return -1;
            else if (a.Down.AbsoluteTicks > b.Down.AbsoluteTicks)
                return 1;
            else
                return 0;
        }
    }
}