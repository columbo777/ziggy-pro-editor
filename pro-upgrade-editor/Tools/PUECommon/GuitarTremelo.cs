using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarMultiStringTremelo : GuitarModifier
    {
        public GuitarMultiStringTremelo(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : 
            base(track, downEvent, upEvent, GuitarModifierType.MultiStringTremelo, GuitarMessageType.GuitarMultiStringTremelo) { }

        public static GuitarMultiStringTremelo CreateMultiStringTremelo(GuitarTrack track, TickPair ticks)
        {
            var ev = track.Insert(Utility.MultiStringTremeloData1, 100, 0, ticks);
            
            var ret = new GuitarMultiStringTremelo(track, ev.Down, ev.Up);

            track.Messages.Add(ret);

            return ret;
        }

    }
}
