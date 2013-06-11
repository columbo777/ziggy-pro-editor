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

        public GuitarBigRockEndingSubMessage(GuitarBigRockEnding owner, TickPair ticks, int data1)
            : base(owner.Owner, ticks, GuitarMessageType.GuitarBigRockEndingSubMessage)
        {
            this.Data1 = data1;
            this.owner = owner;
            this.SetTicks(ticks);
        }

        public GuitarBigRockEndingSubMessage(GuitarBigRockEnding owner, MidiEventPair events)
            : base(events, GuitarMessageType.GuitarBigRockEndingSubMessage)
        {
            this.owner = owner;
            this.Data1 = events.Data1;
        }

        public override void RemoveFromList()
        {
            owner.RemoveSubMessage(this);
            base.RemoveFromList();
        }

        public override void AddToList()
        {
            base.AddToList();
            owner.AddSubMessage(this);
        }
    }
}