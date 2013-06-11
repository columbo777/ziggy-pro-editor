using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarPowerup : GuitarModifier
    {
        public GuitarPowerup(GuitarMessageList owner, TickPair pair)
            : base(owner, pair, GuitarModifierType.Powerup, GuitarMessageType.GuitarPowerup)
        {
            this.Data1 = Utility.PowerupData1;
            this.Data2 = 100;
            Channel = 0;
            this.SetTicks(pair);
        }
        public GuitarPowerup(MidiEventPair ev) :
            base(ev, GuitarModifierType.Powerup, GuitarMessageType.GuitarPowerup) 
        {
            this.Data1 = Utility.PowerupData1;
            this.Data2 = 100;
            Channel = 0;
            this.SetTicks(ev.TickPair);
        }

        public static GuitarPowerup CreatePowerup(GuitarMessageList owner, TickPair ticks)
        {
            var ret = new GuitarPowerup(owner, ticks);
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }
    }
}
