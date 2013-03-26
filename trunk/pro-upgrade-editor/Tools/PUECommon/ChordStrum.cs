using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum ChordStrum
    {
        Normal = 0,
        Low = 1,
        Mid = 2,
        High = 4,
    }
}