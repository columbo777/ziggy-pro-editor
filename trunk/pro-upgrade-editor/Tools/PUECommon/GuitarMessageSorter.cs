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
    public class GuitarMessageSorter : IComparer<GuitarMessage>
    {

        public int Compare(GuitarMessage x, GuitarMessage y)
        {
            var ret = 0;

            if (x.AbsoluteTicks < y.AbsoluteTicks)
                ret = -1;
            else if (x.AbsoluteTicks > y.AbsoluteTicks)
                ret = 1;
            else
            {
                if (x.IsChannelEvent() && y.IsChannelEvent())
                {
                    if (x.IsOff && y.IsOn)
                        ret = -1;
                    else if (x.IsOn && y.IsOff)
                        ret = 1;
                }
            }
            return ret;
        }
    }
}