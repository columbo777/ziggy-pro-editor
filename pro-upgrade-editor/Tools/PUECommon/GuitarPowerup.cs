using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarPowerup : GuitarModifier
    {
        public GuitarPowerup(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : base(track, downEvent, upEvent, GuitarModifierType.Powerup) { }


        public static GuitarPowerup CreatePowerup(GuitarTrack track, int downTick, int upTick)
        {
            MidiEvent downEvent, upEvent;
            Utility.CreateMessage(track, Utility.PowerupData1, 100, Utility.ChannelDefault, downTick, upTick, out downEvent, out upEvent);

            var ret = new GuitarPowerup(track, downEvent, upEvent);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
