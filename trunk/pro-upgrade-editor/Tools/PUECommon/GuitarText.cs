using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarTextEvent : GuitarMessage
    {
        public GuitarTrainerMetaEventType TrainerType
        {
            get
            {
                return Text.GetGuitarTrainerMetaEventType();
            }
        }

        

        public static GuitarTextEvent GetTextEvent(GuitarMessageList track, MidiEvent ev)
        {
            if (ev != null)
            {
                return new GuitarTextEvent(track, ev.AbsoluteTicks, ev.MetaMessage.Text, ev, ev.MetaMessage.Text.GetGuitarTrainerMetaEventType());
            }
            else
            {
                return new GuitarTextEvent(track, Int32.MinValue, string.Empty, null, GuitarTrainerMetaEventType.Unknown);
            }
        }
        public static GuitarTextEvent GetTextEvent(GuitarMessageList track, int tick, string text)
        {
            return new GuitarTextEvent(track, tick, text, null, text.GetGuitarTrainerMetaEventType());
        }
        public static GuitarTextEvent CreateTextEvent(GuitarMessageList track, int absoluteTicks, string text)
        {
            var ret = new GuitarTextEvent(track, absoluteTicks, text,
                track.Insert(absoluteTicks, MetaTextBuilder.Create(MetaType.Text, text)),
                text.GetGuitarTrainerMetaEventType());
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }

        private GuitarTextEvent(GuitarMessageList track, int tick, string text, MidiEvent midiEvent, GuitarTrainerMetaEventType type) :
            base(track, midiEvent, null, GuitarMessageType.GuitarTextEvent)
        {
            this.Text = text;
            SetDownTick(tick);
        }


        public override string ToString()
        {
            return Text;
        }


        public bool IsTrainerEvent
        {
            get { return TrainerType != GuitarTrainerMetaEventType.Unknown; }
        }

        public override void CreateEvents()
        {
            SetDownEvent(Owner.Insert(DownTick, MetaTextBuilder.Create(MetaType.Text, Text)));

            AddToList();
        }


    }

}
