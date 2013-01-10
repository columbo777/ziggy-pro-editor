using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarBigRockEnding : GuitarModifier
    {
        public GuitarBigRockEnding(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : 
            base(track, downEvent, upEvent, GuitarModifierType.BigRockEnding, GuitarMessageType.GuitarBigRockEnding) { }


        public static GuitarBigRockEnding CreateBigRockEnding(GuitarTrack track, TickPair ticks)
        {
            GuitarBigRockEnding ret = null;
            bool first = true;
            foreach (var bre in Utility.BigRockEndingData1)
            {
                var ev = track.Insert(bre, 100, Utility.ChannelDefault, ticks);

                if (first)
                {
                    first = false;

                    ret = new GuitarBigRockEnding(track, ev.Down, ev.Up);
                    track.Messages.Add(ret);
                }
            }
            
            return ret;
        }
    }
}
