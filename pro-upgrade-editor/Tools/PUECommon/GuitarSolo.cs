using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarSolo : GuitarModifier
    {

        public GuitarSolo(GuitarMessageList track, MidiEvent downEvent = null, MidiEvent upEvent = null) :
            base(track, downEvent, upEvent, GuitarModifierType.Solo, GuitarMessageType.GuitarSolo) { }

        public static GuitarSolo CreateSolo(GuitarMessageList track, TickPair ticks)
        {
            if (track.Solos.Any())
            {
            }
            var ev = track.Insert(Utility.SoloData1, 100, Utility.ChannelDefault, ticks);

            var ret = new GuitarSolo(track, ev.Down, ev.Up);

            track.Add(ret);

            return ret;
        }
    }
}
