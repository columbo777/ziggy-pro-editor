﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public class GuitarModifier : GuitarMessage
    {
        public GuitarModifierType ModifierType;

        public GuitarModifier(GuitarMessageList owner, TickPair ticks, GuitarModifierType type, GuitarMessageType mt) :
            base(owner, ticks, mt)
        {
            this.ModifierType = type;
        }

        public GuitarModifier(MidiEventPair ev, GuitarModifierType type, GuitarMessageType mt) :
            base(ev, mt)
        {
            this.ModifierType = type;
        }

    }
}
