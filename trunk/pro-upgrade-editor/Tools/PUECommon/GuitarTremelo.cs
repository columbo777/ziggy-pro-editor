using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarMultiStringTremelo : GuitarModifier
    {
        public GuitarMultiStringTremelo(GuitarMessageList track, MidiEvent downEvent, MidiEvent upEvent) :
            base(track, downEvent, upEvent, GuitarModifierType.MultiStringTremelo, GuitarMessageType.GuitarMultiStringTremelo) { }
        public GuitarMultiStringTremelo(MidiEventPair ev) :
            base(ev, GuitarModifierType.MultiStringTremelo, GuitarMessageType.GuitarMultiStringTremelo) { }

        public static GuitarMultiStringTremelo CreateMultiStringTremelo(GuitarMessageList track, TickPair ticks)
        {
            var ev = track.Insert(Utility.MultiStringTremeloData1, 100, 0, ticks);

            var ret = new GuitarMultiStringTremelo(track, ev.Down, ev.Up);

            track.Add(ret);

            return ret;
        }

    }
}
