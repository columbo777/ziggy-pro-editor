using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace ProUpgradeEditor.Common
{
    [Serializable()]
    public class SerializedChordNote
    {
        public int String { get; set; }
        public int Fret { get; set; }
        public int Channel { get; set; }
    }

}