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
    class VisibleTextEventContainer
    {
        List<VisibleTextEvent> Events = new List<VisibleTextEvent>();

        public void Clear() { Events.Clear(); }

        public int CountOverlapping(RectangleF rect)
        {
            RectangleF rs = rect;
            rs.Inflate(-2, -2);
            rs.Location = new PointF(rs.Location.X + 1f, rs.Location.Y + 1f);
            return Events.Where(x => x.DrawRect.IntersectsWith(rect)).Count();
        }

        public void Add(GuitarTextEvent ev, RectangleF rect)
        {
            Events.Add(new VisibleTextEvent() { DrawRect = rect, Event = ev });
            Events.Sort();
        }
    }
}