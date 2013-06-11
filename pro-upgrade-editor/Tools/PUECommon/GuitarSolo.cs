using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarSolo : GuitarModifier
    {

        public GuitarSolo(GuitarMessageList owner, TickPair ticks) :
            base(owner, ticks, GuitarModifierType.Solo, GuitarMessageType.GuitarSolo) 
        {
            this.Data1 = Utility.SoloData1;
            this.Data2 = 100;
            Channel = 0;
            this.SetTicks(ticks);
        }

        public GuitarSolo(MidiEventPair ev) :
            base(ev, GuitarModifierType.Solo, GuitarMessageType.GuitarSolo) 
        { 
        }
        public static GuitarSolo CreateSolo(GuitarMessageList owner, TickPair ticks)
        {
            var ret = new GuitarSolo(owner, ticks);
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }
    }
}
