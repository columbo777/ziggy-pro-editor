using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;
using ProUpgradeEditor.Common;

namespace EditorResources.Components
{
    public class PETrackDifficulty
    {
        public PEMidiTrack MidiTrack;
        public GuitarDifficulty Difficulty;

        public PETrackDifficulty(PEMidiTrack track, GuitarDifficulty diff)
        {
            this.MidiTrack = track;
            this.Difficulty = diff;
        }
    }
}