using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarHandPosition : GuitarMessage
    {
        public GuitarHandPosition(GuitarMessageList track, TickPair pair, int noteFret)
            : base(track, new MidiEventProps(track), GuitarMessageType.GuitarHandPosition)
        {
            this.Data1 = Utility.HandPositionData1;
            this.Data2 = 100 + noteFret;
            Channel = 0;
            this.SetTicks(pair);
        }
        public GuitarHandPosition( MidiEventPair pair)
            : base(pair.Owner, pair, GuitarMessageType.GuitarHandPosition)
        {
            
        }

        public static GuitarHandPosition CreateEvent(GuitarMessageList track, TickPair ticks, int noteFret)
        {
            var ret = new GuitarHandPosition(track, ticks, noteFret);
            ret.IsNew = true;
            ret.CreateEvents();
            
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
