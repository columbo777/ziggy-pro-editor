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
            Data1 = Utility.GetHammeronData1(chord.Difficulty);
            Data2 = Utility.Data2Default;
            Channel = Utility.ChannelDefault;
            SetTicks(chord.TickPair);
        }
        public GuitarHammeron(MidiEventPair pair)
            : base(pair, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron)
        {
            Data1 = Utility.GetHammeronData1(pair.Data1.GetData1Difficulty(IsPro));
            Data2 = pair.Data2;
            Channel = pair.Channel;
            SetTicks(pair.TickPair);
        }
        public static GuitarHammeron CreateHammeron(GuitarChord chord)
        {
            GuitarHammeron ret = null;
            
            if (!chord.HasHammeron && Utility.GetHammeronData1(chord.Difficulty).IsNotNull())
            {
                ret = new GuitarHammeron(chord);
                ret.IsNew = true;
                ret.SetTicks(chord.TickPair);
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
