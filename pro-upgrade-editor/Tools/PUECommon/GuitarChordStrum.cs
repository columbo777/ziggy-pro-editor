using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarChordStrum : ChordModifier
    {
        public GuitarChordStrum(MidiEventPair pair)
            : base(pair, pair.Channel.GetChordStrumFromChannel().GetModifierType(), GuitarMessageType.GuitarChordStrum)
        {
            
        }
        public GuitarChordStrum(GuitarChord chord, ChordModifierType type)
            : base(chord, type, GuitarMessageType.GuitarChordStrum)
        {
            
        }

        public static GuitarChordStrum CreateStrum(GuitarChord chord, ChordStrum strum)
        {
            GuitarChordStrum ret = null;
            if (!chord.HasStrumMode(strum) && Utility.GetStrumData1(chord.Difficulty).IsNotNull())
            {
                ret = new GuitarChordStrum(chord, strum.GetModifierType());
                ret.IsNew = true;
                ret.CreateEvents();
            }
            return ret;
        }

        public override ChordStrum StrumMode
        {
            get { return Channel.GetChordStrumFromChannel(); }
            
        }

        public override string ToString()
        {
            return "Strum: " + StrumMode.ToString();
        }
    }
}
