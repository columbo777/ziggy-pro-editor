using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProUpgradeEditor.DataLayer;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarHandPosition : GuitarMessage
    {
        public GuitarHandPosition(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent)
            : base(track, downEvent, upEvent)
        {
        }

        public static GuitarHandPosition CreateEvent(GuitarTrack track, int downTick, int upTick, int noteFret)
        {
            MidiEvent downEvent, upEvent;
            Utility.CreateMessage(track, Utility.HandPositionData1, 100+noteFret, 0, downTick, upTick, out downEvent, out upEvent);
            var ret = new GuitarHandPosition(track, downEvent, upEvent);
            track.Messages.Add(ret);
            return ret;
        }

        public static GuitarHandPosition FromEvent(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent)
        {
            return new GuitarHandPosition(track, downEvent, upEvent);
        }

        public int NoteFretDown
        {
            get 
            {
                return Data2 - 100; 
            }
            set { Data2 = 100 + value; }
        }

        public override string ToString()
        {
            return "Fret: " + NoteFretDown.ToStringEx();
        }
    }
}
