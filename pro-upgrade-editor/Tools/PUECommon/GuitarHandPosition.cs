using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarHandPosition : GuitarMessage
    {
        public GuitarHandPosition(GuitarMessageList track, MidiEvent downEvent, MidiEvent upEvent)
            : base(track, downEvent, upEvent, GuitarMessageType.GuitarHandPosition)
        {
        }
        

        public static GuitarHandPosition CreateEvent(GuitarMessageList track, TickPair ticks, int noteFret)
        {
            var ev = track.Insert(Utility.HandPositionData1, 100 + noteFret, Utility.ChannelDefault, ticks);

            var ret = new GuitarHandPosition(track, ev.Down, ev.Up);
            track.Add(ret);
            return ret;
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
