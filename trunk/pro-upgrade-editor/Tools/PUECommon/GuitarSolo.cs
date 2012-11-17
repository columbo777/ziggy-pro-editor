using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarSolo : GuitarModifier
    {

        public GuitarSolo(GuitarTrack track, MidiEvent downEvent = null, MidiEvent upEvent = null) : base(track, downEvent, upEvent, GuitarModifierType.Solo) { }

        public static GuitarSolo CreateSolo(GuitarTrack track, int downTick, int upTick)
        {
            
            MidiEvent downEvent, upEvent;
            Utility.CreateMessage(track, Utility.SoloData1, 100, Utility.ChannelDefault, downTick, upTick, out downEvent, out upEvent);

            var ret = new GuitarSolo(track, downEvent, upEvent);

            track.Messages.Add(ret);

            return ret;
        }
    }
}
