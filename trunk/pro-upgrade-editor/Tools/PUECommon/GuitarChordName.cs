using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordName : GuitarMessage
    {

        internal GuitarChordNameList OwnerList { get; set; }
        internal GuitarChord OwnerChord { get; set; }


        public override void AddToList()
        {
            IsDeleted = false;

            IsNew = false;
        }

        public override void RemoveFromList()
        {
            if (Owner != null)
            {
                Owner.Remove(this);
            }
            IsDeleted = true;
        }

        public override void DeleteAll()
        {
            base.DeleteAll();
        }


        public GuitarChordName(GuitarMessageList owner, TickPair pair, ChordNameMeta meta, bool hidden)
            : base(owner, pair, GuitarMessageType.GuitarChordName)
        {
            
            this.Data1 = meta.ToneName.ToToneNameData1().ToInt();
            this.Meta = meta;
            this.ChordNameHidden = hidden;
            Channel = 0;
        }

        public GuitarChordName(MidiEventPair pair)
            : base(pair, GuitarMessageType.GuitarChordName)
        {
            this.Data1 = pair.Data1;
            Channel = 0;
        }

        public bool ChordNameHidden
        {
            get;
            set;
        }

        public ChordNameMeta Meta
        {
            get;
            internal set;
        }

        public static GuitarChordName CreateEvent(GuitarMessageList owner, TickPair ticks, ChordNameMeta meta, bool chordNameHidden)
        {
            var ret = new GuitarChordName(owner, ticks, meta, chordNameHidden);
            ret.IsNew = true;
            ret.CreateEvents();
            return ret;
        }

        public override string ToString()
        {
            return base.ToString() + " : " + Meta.ToStringEx();
        }
    }
}