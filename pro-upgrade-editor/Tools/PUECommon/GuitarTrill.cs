using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarSingleStringTremelo : GuitarModifier
    {
        public GuitarSingleStringTremelo(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : base(track, downEvent, upEvent, GuitarModifierType.SingleStringTremelo) { }

        public static GuitarSingleStringTremelo CreateSingleStringTremelo(GuitarTrack track, int downTick, int upTick)
        {
            MidiEvent downEvent, upEvent;
            Utility.CreateMessage(track, Utility.SingleStringTremeloData1, 100, Utility.ChannelDefault, downTick, upTick, out downEvent, out upEvent);

            var ret = new GuitarSingleStringTremelo(track, downEvent, upEvent);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
