using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarSlide : ChordModifier
    {
        public override bool IsSlide
        {
            get
            {
                return true;
            }
        }

        
        public GuitarSlide(GuitarChord chord, bool isReversed)
            : base(chord,
            isReversed ?  ChordModifierType.SlideReverse : ChordModifierType.Slide, GuitarMessageType.GuitarSlide)
        {
            
        }
        public GuitarSlide(MidiEventPair pair)
            : base(pair, pair.Channel == Utility.ChannelSlideReversed  ? ChordModifierType.SlideReverse : ChordModifierType.Slide, 
            GuitarMessageType.GuitarSlide)
        {
        }
        public static GuitarSlide CreateSlide(GuitarChord chord, bool reversed)
        {
            GuitarSlide ret = null;
            if (!chord.HasSlide && Utility.GetSlideData1(chord.Difficulty).IsNotNull())
            {
                ret = new GuitarSlide(chord, reversed);
                ret.IsNew = true;
                ret.CreateEvents();
            }
            return ret;
        }

        public override int Channel
        {
            get
            {
                return IsReversed ? Utility.ChannelSlideReversed : Utility.ChannelDefault;
            }
            set
            {
                if (value != this.Channel)
                {
                    base.Channel = value;
                }
            }
        }

        public override bool IsReversed
        {
            get { return ModifierType == ChordModifierType.SlideReverse; }
            
        }

        public override string ToString()
        {
            return "Slide: " + (IsReversed ? "Reverse" : "Normal");
        }
    }
}
