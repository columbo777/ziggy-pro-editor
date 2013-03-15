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
    public class TickCloseComparer : IEqualityComparer<int>
    {
        int closeWidth;
        public TickCloseComparer(int closeWidth)
        {
            this.closeWidth = closeWidth;
        }
        public bool Equals(int x, int y)
        {
            return Math.Abs(x - y) <= closeWidth;
        }

        public int GetHashCode(int obj)
        {
            return 0;
        }
    }
}