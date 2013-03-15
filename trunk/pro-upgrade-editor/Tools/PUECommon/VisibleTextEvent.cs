using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using NAudio.Wave;

namespace ProUpgradeEditor.Common
{
    class VisibleTextEvent : IComparable<VisibleTextEvent>
    {
        public GuitarTextEvent Event { get; set; }
        public RectangleF DrawRect { get; set; }

        public int CompareTo(VisibleTextEvent other)
        {
            var ret = Event.AbsoluteTicks < other.Event.AbsoluteTicks ? -1 :
                Event.AbsoluteTicks > other.Event.AbsoluteTicks ? 1 : 0;

            if (ret == 0)
            {
                ret = string.Compare(Event.Text, other.Event.Text);
            }
            return ret;
        }
    }
}