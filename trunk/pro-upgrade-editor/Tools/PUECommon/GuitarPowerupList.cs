using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarPowerupList : SpecializedMessageList<GuitarPowerup>
    {

        public GuitarPowerupList(TrackEditor owner) : base(owner) { }
        public override GuitarMessageType MessageType
        {
            get { return GuitarMessageType.GuitarPowerup; }
        }
    }
}