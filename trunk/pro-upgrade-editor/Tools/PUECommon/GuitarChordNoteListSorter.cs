using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordNoteListSorter : IComparer<GuitarNote>
    {
        public int Compare(GuitarNote x, GuitarNote y)
        {
            return x.NoteString < y.NoteString ? -1 : x.NoteString > y.NoteString ? 1 : 0;
        }
    }
}