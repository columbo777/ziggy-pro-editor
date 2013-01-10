using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    
    public class GuitarSlide : ChordModifier
    {
        public GuitarSlide(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent)
            : base(track, downEvent, upEvent,
                downEvent.Channel == Utility.ChannelSlideReversed ? ChordModifierType.SlideReverse : ChordModifierType.Slide, 
                GuitarMessageType.GuitarSlide)
        {
        }

        public GuitarSlide(GuitarTrack track, bool isReversed, MidiEvent downEvent=null, MidiEvent upEvent=null)
            : base(track, downEvent, upEvent,
            isReversed ? ChordModifierType.SlideReverse : ChordModifierType.Slide, GuitarMessageType.GuitarSlide)
        {
        }

        

        public static GuitarSlide CreateSlide(GuitarTrack track, 
            TickPair ticks, bool reversed, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            GuitarSlide ret = null;
            if (ticks.IsValid)
            {
                if (difficulty.IsUnknownOrNone())
                    difficulty = track.CurrentDifficulty;

                var d1 = Utility.GetSlideData1(difficulty);
                if (d1 != -1)
                {
                    var ev = track.Insert(d1,
                        100,
                        reversed ? Utility.ChannelSlideReversed : Utility.ChannelDefault,
                        ticks);
                    
                    ret = new GuitarSlide(track, reversed, ev.Down, ev.Up);

                    track.Messages.Add(ret);
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
