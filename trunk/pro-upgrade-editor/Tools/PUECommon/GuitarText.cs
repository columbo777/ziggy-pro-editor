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

        public GuitarTextEvent(GuitarMessageList owner, int tick, string text) :
            base(owner, new TickPair(tick, Int32.MinValue), GuitarMessageType.GuitarTextEvent)
        {
            this.Text = text;
        }

        public GuitarTextEvent(GuitarMessageList list, MidiEvent ev) :
            base(list, ev, null, GuitarMessageType.GuitarTextEvent)
        {
            this.Text = ev.Text;
        }
        
        public static GuitarTextEvent CreateTextEvent(GuitarMessageList owner, int absoluteTicks, string text)
        {
            var ret = new GuitarTextEvent(owner, absoluteTicks, text);
            ret.IsNew = true;
            ret.CreateEvents();
            return ret;
        }

        public override void AddToList()
        {
            IsDeleted = false;
            
            IsNew = false;

            Owner.Add(this);
        }

        public override void RemoveFromList()
        {
            Owner.Remove(this);
            IsDeleted = true;
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
            props.SetUpdatedEventPair(Owner.Insert(DownTick, MetaTextBuilder.Create(MetaType.Text, Text)));

            if (IsNew)
            {
                AddToList();
            }
        }


    }

}
