using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarMultiStringTremelo : GuitarModifier
    {
        public GuitarMultiStringTremelo(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : base(track, downEvent, upEvent, GuitarModifierType.MultiStringTremelo) { }

        public static GuitarMultiStringTremelo CreateMultiStringTremelo(GuitarTrack track, int downTick, int upTick)
        {
            MidiEvent downEvent, upEvent;
            Utility.CreateMessage(track, Utility.MultiStringTremeloData1, 100, Utility.ChannelDefault, downTick, upTick, out downEvent, out upEvent);

            var ret = new GuitarMultiStringTremelo(track, downEvent, upEvent);

            track.Messages.Add(ret);

            return ret;
        }

    }
}
