using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    public class TrackDifficulty
    {
        public string Track { get; internal set; }
        public GuitarDifficulty Difficulty { get; internal set; }

        public TrackDifficulty(string track, GuitarDifficulty difficulty)
        {
            this.Track = track;
            this.Difficulty = difficulty;
        }
    }
}
