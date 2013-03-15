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
    public class DifficultyButton : PETrackDifficulty
    {
        public Button Button;

        public DifficultyButton(PETrackDifficulty trackDiff, Button button)
            : base(trackDiff.MidiTrack, trackDiff.Difficulty)
        {
            this.Button = button;
        }
    }
}