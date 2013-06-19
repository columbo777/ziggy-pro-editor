using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarHandPosition : GuitarMessage
    {
        public GuitarHandPosition(GuitarMessageList owner, TickPair pair, int noteFret)
            : base(owner, pair, GuitarMessageType.GuitarHandPosition)
        {
            this.Data1 = Utility.HandPositionData1;
            NoteFretDown = noteFret;
            Channel = 0;
            this.SetTicks(pair);
        }

        public GuitarHandPosition( MidiEventPair pair)
            : base(pair, GuitarMessageType.GuitarHandPosition)
        {
            this.Data1 = pair.Data1;
            Channel = 0;
            NoteFretDown = pair.Data2 - 100;
        }

        public static GuitarHandPosition CreateEvent(GuitarMessageList owner, TickPair ticks, int noteFret)
        {
            var ret = new GuitarHandPosition(owner, ticks, noteFret);
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
            return base.ToString() + " Fret: " + NoteFretDown.ToStringEx();
        }
    }


    
}
