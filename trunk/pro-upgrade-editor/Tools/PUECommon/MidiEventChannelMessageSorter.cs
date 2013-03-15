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
    public class MidiEventChannelMessageSorter : IComparer<MidiEvent>
    {
        public int Compare(MidiEvent x, MidiEvent y)
        {
            if (x.AbsoluteTicks == y.AbsoluteTicks &&
                x.Data1 == y.Data1 &&
                x.Channel == y.Channel &&
                x.Command != y.Command)
            {
                if (x.IsOff)
                    return -1;
                else
                    return 1;
            }
            else
            {
                if (x.AbsoluteTicks < y.AbsoluteTicks)
                    return -1;
                else if (x.AbsoluteTicks > y.AbsoluteTicks)
                    return 1;
                else if (x.Data1 < y.Data1)
                    return -1;
                else if (x.Data1 > y.Data1)
                    return 1;
                else if (x.Channel < y.Channel)
                    return -1;
                else if (x.Channel > y.Channel)
                    return 1;
                else
                    return 0;
            }
        }
    }
}