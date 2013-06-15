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
            if (a.DownTick < b.DownTick)
                return -1;
            if (a.DownTick > b.DownTick)
                return 1;
            
            return 0;
        }
    }

    public class Data1ChannelEventInterlacingSorter : IComparer<MidiEventPair>
    {
        public int Compare(MidiEventPair a, MidiEventPair b)
        {
            if (a.DownTick < b.DownTick)
                return -1;
            if (a.DownTick > b.DownTick)
                return 1;

            if (a.Channel < b.Channel)
                return -1;
            if (a.Channel > b.Channel)
                return 1;

            if (a.Data1 < b.Data1)
                return -1;
            if (a.Data1 > b.Data1)
                return 1;

            return 0;
        }
    }
}