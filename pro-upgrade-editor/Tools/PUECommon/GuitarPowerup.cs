using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarPowerup : GuitarModifier
    {
        public GuitarPowerup(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : 
            base(track, downEvent, upEvent, GuitarModifierType.Powerup, GuitarMessageType.GuitarPowerup) { }


        public static GuitarPowerup CreatePowerup(GuitarTrack track, TickPair ticks)
        {
            var ev = track.Insert(Utility.PowerupData1, 100, Utility.ChannelDefault, ticks);

            var ret = new GuitarPowerup(track, ev.Down, ev.Up);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
