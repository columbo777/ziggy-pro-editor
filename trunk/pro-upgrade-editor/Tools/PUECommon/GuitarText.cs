using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
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

        public static GuitarTextEvent GetTextEvent(GuitarTrack track, MidiEvent ev)
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
        public static GuitarTextEvent GetTextEvent(GuitarTrack track, int tick, string text)
        {
            return new GuitarTextEvent(track, tick, text, null, text.GetGuitarTrainerMetaEventType());
        }
        public static GuitarTextEvent CreateTextEvent(GuitarTrack track, int absoluteTicks, string text)
        {
            var ret = new GuitarTextEvent(track, absoluteTicks, text, 
                track.Insert(absoluteTicks, MetaTextBuilder.Create(MetaType.Text, text)), 
                text.GetGuitarTrainerMetaEventType());
            track.Messages.Add(ret);
            return ret;
        }

        private GuitarTextEvent(GuitarTrack track, int tick, string text, MidiEvent midiEvent, GuitarTrainerMetaEventType type) : base(track, midiEvent)
        {
            this.text = text;
            
            downTick = tick;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.MidiEvent != null && OwnerTrack != null)
                {
                    OwnerTrack.Remove(this.MidiEvent);
                    
                    this.MidiEvent = OwnerTrack.Insert(this.AbsoluteTicks, MetaTextBuilder.Create(MetaType.Text, value));
                    this.text = value;
                }
                else
                {
                    base.Text = value;
                }
            }
        }
        
        public override string ToString()
        {
            return Text;
        }


        public bool IsTrainerEvent
        {
            get { return TrainerType != GuitarTrainerMetaEventType.Unknown; }
        }


    }

}
