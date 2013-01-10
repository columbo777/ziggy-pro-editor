using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarSolo : GuitarModifier
    {

        public GuitarSolo(GuitarTrack track, MidiEvent downEvent = null, MidiEvent upEvent = null) : 
            base(track, downEvent, upEvent, GuitarModifierType.Solo, GuitarMessageType.GuitarSolo) { }

        public static GuitarSolo CreateSolo(GuitarTrack track, TickPair ticks)
        {
            if (track.Messages.Solos.Any())
            {
            }
            var ev = track.Insert(Utility.SoloData1, 100, Utility.ChannelDefault, ticks);
            
            var ret = new GuitarSolo(track, ev.Down, ev.Up);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
