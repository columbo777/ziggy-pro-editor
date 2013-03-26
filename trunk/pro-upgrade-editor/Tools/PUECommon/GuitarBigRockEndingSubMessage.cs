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
}