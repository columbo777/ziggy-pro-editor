using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarBigRockEndingSubMessage : GuitarMessage
    {
        GuitarBigRockEnding owner;

        public GuitarBigRockEndingSubMessage(GuitarBigRockEnding owner, MidiEventPair events)
            : base(owner.Owner, events, GuitarMessageType.GuitarBigRockEndingSubMessage)
        {
            this.owner = owner;
        }

    }

    public class GuitarBigRockEnding : GuitarModifier
    {
        List<GuitarBigRockEndingSubMessage> events;

        public GuitarBigRockEnding(GuitarMessageList track, IEnumerable<MidiEventPair> events) :
            base(track, null, null, GuitarModifierType.BigRockEnding, GuitarMessageType.GuitarBigRockEnding)
        {
            this.events = new List<GuitarBigRockEndingSubMessage>();

            var ev = events.Select(x => new GuitarBigRockEndingSubMessage(this, x));
            ev.ForEach(x => AddEvent(x));
        }

        public void AddEvent(GuitarBigRockEndingSubMessage msg)
        {
            if (msg != null)
            {
                if (events.Any(x => x.Data1 == msg.Data1))
                {
                    events.Where(x => x.Data1 == msg.Data1).ToList().ForEach(x =>
                    {
                        Owner.Remove(x);
                    });
                }
                events.Add(msg);
            }
        }


        public override int UpTick
        {
            get
            {
                if (base.UpTick.IsNull() && events.Any())
                {
                    return events.GetMaxTick();
                }
                else
                {
                    return base.UpTick;
                }
            }

        }
        public override int DownTick
        {
            get
            {
                if (base.DownTick.IsNull() && events.Any())
                {
                    return events.GetMinTick();
                }
                else
                {
                    return base.DownTick;
                }
            }
        }

        public override bool HasDownEvent
        {
            get
            {
                return events.Any(x => x.HasDownEvent);
            }
        }

        public override bool HasUpEvent
        {
            get
            {
                return events.Any(x => x.HasUpEvent);
            }
        }

        public override void RemoveEvents()
        {
            events.ForEach(x => Owner.Remove(x));
        }

        public override void CreateEvents()
        {
            events.ForEach(x => x.CreateEvents());
            if (Owner != null)
            {
                Owner.Add(this);
            }
        }

        public override void SetTicks(TickPair ticks)
        {
            base.SetTicks(ticks);

            events.ForEach(x => x.SetTicks(ticks));

        }
        public override void UpdateEvents()
        {
            RemoveEvents();
            CreateEvents();
        }


        public IEnumerable<GuitarBigRockEndingSubMessage> Events { get { return events; } }

        public bool IsValid { get { return Utility.BigRockEndingData1.Where(x => Events.Any(e => e.Data1 == x)).Count() == (Owner.Owner.IsPro ? 6 : 5); } }

        public static GuitarBigRockEnding CreateBigRockEnding(GuitarMessageList track, TickPair ticks)
        {
            track.BigRockEndings.GetBetweenTick(ticks).ToList().ForEach(x => track.Remove(x));

            var ret = new GuitarBigRockEnding(track, Utility.BigRockEndingData1.Select(x =>
                new MidiEventPair(track, track.Insert(x, 100, 0, ticks))));

            track.Add(ret);

            return ret;
        }
    }
}
