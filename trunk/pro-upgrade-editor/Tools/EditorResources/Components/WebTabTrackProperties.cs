using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ProUpgradeEditor.Common;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;

namespace EditorResources.Components
{

    public class WebTabTrackProperties
    {
        public WebTabTrackProperties(Track track, TrackEditor editorPro)
        {
            this.Track = track;

            this.Scale = 1.0;
            this.Offset = 3.0;
            this.Import = true;

            Events = new CopyChordList(editorPro);
            editorPro.SetTrack(track, GuitarDifficulty.Expert);
            
            Events.AddRange(editorPro.Messages.Chords.ToList());

        }
        public Track Track;

        public CopyChordList Events;

        public double Scale;
        public bool Import;
        public double Offset;
    }
}