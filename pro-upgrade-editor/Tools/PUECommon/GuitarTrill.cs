using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarSingleStringTremelo : GuitarModifier
    {
        public GuitarSingleStringTremelo(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : 
            base(track, downEvent, upEvent, GuitarModifierType.SingleStringTremelo, GuitarMessageType.GuitarSingleStringTremelo) { }

        public static GuitarSingleStringTremelo CreateSingleStringTremelo(GuitarTrack track, TickPair ticks)
        {
            var ev = track.Insert(Utility.SingleStringTremeloData1, 100, Utility.ChannelDefault, ticks);

            var ret = new GuitarSingleStringTremelo(track, ev.Down, ev.Up);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
