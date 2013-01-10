using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarHammeron : ChordModifier
    {

        public GuitarHammeron(GuitarTrack track, MidiEvent downEvent = null, MidiEvent upEvent = null)
            : base(track, downEvent, upEvent, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron)
        {
            
        }

        public static GuitarHammeron CreateHammeron(GuitarTrack track,
            TickPair ticks, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            GuitarHammeron ret = null;
            if (difficulty.IsUnknownOrNone())
                difficulty = track.CurrentDifficulty;

            var d1 = Utility.GetHammeronData1(difficulty);
            if (d1 != -1)
            {
                var ev = track.Insert(d1, 100, Utility.ChannelDefault,
                    ticks);

                ret = new GuitarHammeron(track, ev.Down, ev.Up);
            }
            track.Messages.Add(ret);
            return ret;
        }



        public override string ToString()
        {
            return "Hammeron: ";
        }
    }
}
