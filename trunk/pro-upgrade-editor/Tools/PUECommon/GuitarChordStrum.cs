using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProUpgradeEditor.DataLayer;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    
    public class GuitarChordStrum : GuitarModifier
    {
        public GuitarChordStrum(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent, GuitarModifierType type)
            : base(track, downEvent, upEvent, type)
        {
        }

        public static GuitarChordStrum CreateStrum(GuitarTrack track, GuitarDifficulty difficulty, ChordStrum strum, int downTick, int upTick)
        {
            GuitarChordStrum ret = null;
            var d1 = Utility.GetStrumData1(difficulty);
            if (d1 != -1)
            {
                MidiEvent downEvent, upEvent;
                Utility.CreateMessage(track, d1, 100, strum.GetChannelFromChordStrum(), downTick, upTick, out downEvent, out upEvent);

                ret = new GuitarChordStrum(track, downEvent, upEvent, strum.GetModifierType());

            }
            return ret;
        }

        public ChordStrum StrumMode
        {
            get { return Channel.GetChordStrumFromChannel(); }
            set { Channel = value.GetChannelFromChordStrum(); }
        }

        public override string ToString()
        {
            return "Strum: " + StrumMode.ToString();
        }
    }
}
