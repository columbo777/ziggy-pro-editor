using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarSingleStringTremelo : GuitarModifier
    {
        public GuitarSingleStringTremelo(GuitarMessageList owner, TickPair ticks) :
            base(owner, ticks, GuitarModifierType.SingleStringTremelo, GuitarMessageType.GuitarSingleStringTremelo) 
        {
            this.Data1 = Utility.SingleStringTremeloData1;
            this.Data2 = 100;
            Channel = 0;
            this.SetTicks(ticks);
        }
        public GuitarSingleStringTremelo(MidiEventPair ev) :
            base(ev, GuitarModifierType.SingleStringTremelo, GuitarMessageType.GuitarSingleStringTremelo) 
        {
        }

        public static GuitarSingleStringTremelo CreateSingleStringTremelo(GuitarMessageList owner, TickPair ticks)
        {
            var ret = new GuitarSingleStringTremelo(owner, ticks);
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }
    }
}
