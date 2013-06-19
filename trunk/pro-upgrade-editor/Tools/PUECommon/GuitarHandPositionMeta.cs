using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarHandPositionMeta
    {
        public GuitarChord Chord;
        public TickPair Ticks;
        public int Fret;
        public bool IsChord;
    }
}