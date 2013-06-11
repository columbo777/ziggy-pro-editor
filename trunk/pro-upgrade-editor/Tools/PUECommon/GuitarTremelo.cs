using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarMultiStringTremelo : GuitarModifier
    {
        public GuitarMultiStringTremelo(GuitarMessageList owner, TickPair pair)
            : base(owner, pair, GuitarModifierType.MultiStringTremelo, GuitarMessageType.GuitarMultiStringTremelo)
        {
            this.Data1 = Utility.MultiStringTremeloData1;
            this.Data2 = 100;
            Channel = 0;
            this.SetTicks(pair);
        }
        public GuitarMultiStringTremelo(MidiEventPair ev) :
            base(ev, GuitarModifierType.MultiStringTremelo, GuitarMessageType.GuitarMultiStringTremelo) 
        { 
        }

        public static GuitarMultiStringTremelo CreateMultiStringTremelo(GuitarMessageList owner, TickPair ticks)
        {
            var ret = new GuitarMultiStringTremelo(owner, ticks);
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }

    }
}
