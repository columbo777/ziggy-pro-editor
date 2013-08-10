using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarNoteList : SpecializedMessageList<GuitarNote>
    {
        public GuitarNoteList(TrackEditor owner) : base(owner) { }
        public override GuitarMessageType MessageType
        {
            get { return GuitarMessageType.GuitarNote; }
        }
    }

    public class ChordNameList : SpecializedMessageList<GuitarChordName>
    {
        public ChordNameList(TrackEditor owner) : base(owner) { }
        public override GuitarMessageType MessageType
        {
            get { return GuitarMessageType.GuitarChordName; }
        }
    }
}