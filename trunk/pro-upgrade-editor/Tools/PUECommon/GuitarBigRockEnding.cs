using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    

    public class GuitarBigRockEnding : GuitarModifier
    {
        List<GuitarBigRockEndingSubMessage> childEvents = new List<GuitarBigRockEndingSubMessage>();
        public GuitarBigRockEnding(GuitarMessageList owner, TickPair ticks, IEnumerable<MidiEventPair> events) :
            this(owner, ticks)
        {
            Channel = 0;
            events.ToList().ForEach(ev => AddSubMessage(new GuitarBigRockEndingSubMessage(this, ev)));
            SetTicks(ticks);
        }

        public GuitarBigRockEnding(GuitarMessageList owner, TickPair ticks) : 
            base(owner,ticks,GuitarModifierType.BigRockEnding, GuitarMessageType.GuitarBigRockEnding)
        {
            Channel = 0;
            foreach (var data1 in Utility.GetBigRockEndingData1(IsPro))
            {
                childEvents.Add(new GuitarBigRockEndingSubMessage(this, ticks, data1));
            }

            SetTicks(ticks);
        }

        public void RemoveSubMessage(GuitarBigRockEndingSubMessage msg)
        {
            if (childEvents.Contains(msg))
            {
                childEvents.Remove(msg);
            }
        }

        public void AddSubMessage(GuitarBigRockEndingSubMessage msg)
        {
            if (!childEvents.Contains(msg))
            {
                childEvents.Add(msg);
            }
        }

        public override bool IsNew
        {
            get
            {
                return base.IsNew;
            }
            set
            {
                base.IsNew = value;
                foreach (var ev in childEvents)
                    ev.IsNew = value;
            }
        }

        public override void RemoveFromList()
        {
            childEvents.ToList().ForEach(x => x.RemoveFromList());
            base.RemoveFromList();
        }

        public override void RemoveEvents()
        {
            childEvents.ToList().ForEach(x => x.RemoveEvents());

            base.RemoveEvents();
        }

        public override void CreateEvents()
        {
            childEvents.ToList().ForEach(x =>
            {
                x.IsNew = this.IsNew;
                x.CreateEvents();
            });

            if (IsNew)
            {
                AddToList();
            }
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


        public static GuitarBigRockEnding CreateBigRockEnding(GuitarMessageList owner, TickPair ticks)
        {
            var ret = new GuitarBigRockEnding(owner, ticks);
            
            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }
    }
}
