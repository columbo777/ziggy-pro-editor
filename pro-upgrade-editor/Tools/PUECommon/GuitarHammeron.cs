using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarHammeron : ChordModifier
    {
        public GuitarHammeron(GuitarChord chord)
            : base(chord, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron)
        {
            
        }
        public GuitarHammeron(MidiEventPair pair)
            : base(pair, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron)
        {
            
        }
        public static GuitarHammeron CreateHammeron(GuitarChord chord)
        {
            GuitarHammeron ret = null;
            
            if (!chord.HasHammeron && Utility.GetHammeronData1(chord.Difficulty).IsNotNull())
            {
                ret = new GuitarHammeron(chord);
                ret.IsNew = true;
                ret.CreateEvents();
            }
            
            return ret;
        }



        public override string ToString()
        {
            return "Hammeron: ";
        }
    }
}
