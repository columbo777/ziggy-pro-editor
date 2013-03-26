using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    

    public class GuitarBigRockEnding : GuitarModifier
    {
        List<GuitarBigRockEndingSubMessage> childEvents;

        public GuitarBigRockEnding(GuitarMessageList track, IEnumerable<MidiEventPair> eventList) :
            base(track, null, null, GuitarModifierType.BigRockEnding, GuitarMessageType.GuitarBigRockEnding)
        {
            this.childEvents = new List<GuitarBigRockEndingSubMessage>();
            SetTicks(eventList.GetTickPair());
            this.childEvents.AddRange( eventList.Select(x => new GuitarBigRockEndingSubMessage(this, x)));
        }

        public override void RemoveEvents()
        {
            childEvents.ToList().ForEach(x => x.RemoveEvents());

            base.RemoveEvents();
        }

        public override void CreateEvents()
        {
            childEvents.ToList().ForEach(x => x.CreateEvents());

            base.CreateEvents();
        }

        public override void SetTicks(TickPair ticks)
        {
            base.SetTicks(ticks);

            childEvents.ToList().ForEach(x => x.SetTicks(ticks));

        }
        public override void UpdateEvents()
        {
            childEvents.ToList().ForEach(x => x.UpdateEvents());
        }

        public override void DeleteAll()
        {
            childEvents.ToList().ForEach(x => x.DeleteAll());
            childEvents.Clear();

            base.DeleteAll();
        }

        public IEnumerable<GuitarBigRockEndingSubMessage> Events { get { return childEvents; } }


        public static GuitarBigRockEnding CreateBigRockEnding(GuitarMessageList list, TickPair ticks)
        {
            var data1 = Utility.GetBigRockEndingData1(list.Owner.IsPro);
            var events = new List<MidiEventPair>();
            foreach(var d1 in data1)
            {
                events.Add(list.Insert(d1, 100, Utility.ChannelDefault, ticks));
            }
            var ret = new GuitarBigRockEnding(list, events);
            
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }
    }
}
