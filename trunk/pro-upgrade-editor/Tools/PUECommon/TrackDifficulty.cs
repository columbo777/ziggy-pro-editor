using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using ProUpgradeEditor.DataLayer;

namespace ProUpgradeEditor
{
    public class TrackDifficulty
    {
        public Track Track { get; internal set; }
        public GuitarDifficulty Difficulty { get; internal set; }

        public TrackDifficulty(Track track, GuitarDifficulty difficulty)
        {
            this.Track = track;
            this.Difficulty = difficulty;
        }
    }
}
