using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;


namespace ProUpgradeEditor.UI
{
    public class MidiInputListItem
    {
        public int index;
        public MidiInCaps Caps;
        public override string ToString()
        {
            return Caps.name;
        }
    }
}