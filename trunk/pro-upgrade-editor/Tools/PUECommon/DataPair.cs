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
    public class DataPair<T>
    {
        public T A { get; set; }
        public T B { get; set; }
        public DataPair(T a, T b) { this.A = a; this.B = b; }
    }
}