using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarSlide : ChordModifier
    {
        public GuitarSlide(GuitarMessageList list, MidiEvent downEvent, MidiEvent upEvent)
            : base(list, downEvent, upEvent,
                downEvent.Channel == Utility.ChannelSlideReversed ? ChordModifierType.SlideReverse : ChordModifierType.Slide,
                GuitarMessageType.GuitarSlide)
        {
        }

        public GuitarSlide(GuitarMessageList list, bool isReversed, MidiEvent downEvent = null, MidiEvent upEvent = null)
            : base(list, downEvent, upEvent,
            isReversed ? ChordModifierType.SlideReverse : ChordModifierType.Slide, GuitarMessageType.GuitarSlide)
        {
        }



        public static GuitarSlide CreateSlide(GuitarMessageList list,
            TickPair ticks, bool reversed, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            GuitarSlide ret = null;
            if (ticks.IsValid)
            {
                if (difficulty.IsUnknownOrNone())
                    difficulty = list.Owner.CurrentDifficulty;

                var d1 = Utility.GetSlideData1(difficulty);
                if (d1 != -1)
                {
                    var ev = list.Insert(d1,
                        100,
                        reversed ? Utility.ChannelSlideReversed : Utility.ChannelDefault,
                        ticks);

                    ret = new GuitarSlide(list, reversed, ev.Down, ev.Up);

                    list.Add(ret);
                }
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
        public bool IsReversed
        {
            get { return ModifierType == ChordModifierType.SlideReverse; }
            set
            {
                if (value != IsReversed)
                {
                    ModifierType = (value == true ?
                        ChordModifierType.SlideReverse : ChordModifierType.Slide);
                }
            }
        }

        public override string ToString()
        {
            return "Slide: " + (IsReversed ? "Reverse" : "Normal");
        }
    }
}
