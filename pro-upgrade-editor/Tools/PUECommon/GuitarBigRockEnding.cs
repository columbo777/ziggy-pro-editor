using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarBigRockEnding : GuitarModifier
    {
        public GuitarBigRockEnding(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : base(track, downEvent, upEvent, GuitarModifierType.BigRockEnding) { }


        public static GuitarBigRockEnding CreateBigRockEnding(GuitarTrack track, int downTick, int upTick)
        {
            GuitarBigRockEnding ret = null;
            bool first = true;
            foreach (var bre in Utility.BigRockEndingData1)
            {
                MidiEvent downEvent, upEvent;
                Utility.CreateMessage(track, bre, 100, Utility.ChannelDefault, downTick, upTick, out downEvent, out upEvent);

                if (first)
                {
                    first = false;

                    ret = new GuitarBigRockEnding(track, downEvent, upEvent);
                    track.Messages.Add(ret);
                }
            }
            
            return ret;
        }
    }
}
