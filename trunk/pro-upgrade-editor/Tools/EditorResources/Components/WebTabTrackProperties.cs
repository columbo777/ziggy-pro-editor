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

namespace EditorResources.Components
{

    public class AbsoluteMidiEvent
    {
        public int Tick;
        public IMidiMessage Message;

        public AbsoluteMidiEvent Clone(){ return new AbsoluteMidiEvent(){ Tick=this.Tick, Message = Message } ; }
    }

    public class WebTabTrackProperties
    {
        public WebTabTrackProperties(Track track)
        {
            this.Track = track;
            
            this.Scale = 1.0;
            this.Offset = 3.0;
            this.Import = true;
            
            Events = new List<AbsoluteMidiEvent>();
            Events.AddRange(track.ChanMessages.Select(x => new AbsoluteMidiEvent() { Message = x.Clone(), Tick = x.AbsoluteTicks }));

        }
        public Track Track;

        public List<AbsoluteMidiEvent> Events;

        public double Scale;
        public bool Import;
        public double Offset;
    }
}