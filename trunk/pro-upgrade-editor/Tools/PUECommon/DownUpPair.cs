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
    public interface DownUpPair<T>
    {
        T Down { get; set; }
        T Up { get; set; }

        bool IsNull { get; }
    }
}