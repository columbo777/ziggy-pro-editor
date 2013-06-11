using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarNote : GuitarMessage
    {
        
        internal GuitarChordNoteList OwnerList { get; set; }
        internal GuitarChord OwnerChord { get; set; }


        public GuitarNote(GuitarMessageList owner, TickPair ticks)
            : base(owner, ticks, GuitarMessageType.GuitarNote)
        {
            Data2 = Utility.Data2Default;
            Channel = Utility.ChannelDefault;
            SetTicks(ticks);
        }

        public override void AddToList()
        {
            IsDeleted = false;
            
            IsNew = false;
        }

        public override void RemoveFromList()
        {
            Owner.Remove(this);
            IsDeleted = true;
        }

        public override void DeleteAll()
        {
            base.DeleteAll();
        }

        public override void RemoveEvents()
        {
            if (UpEvent != null && Owner != null)
            {
                Owner.Remove(UpEvent);
            }
            if (DownEvent != null && Owner != null)
            {
                Owner.Remove(DownEvent);
            }
        }

        public GuitarNote(MidiEventPair pair)
            : base(pair, GuitarMessageType.GuitarNote)
        {
            Data1 = pair.Data1;
            Data2 = pair.Data2;
            Channel = pair.Channel;
        }

        public static GuitarNote GetNote(GuitarMessageList owner, GuitarDifficulty diff,
            TickPair ticks, int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = new GuitarNote(owner, ticks);
            ret.Data1 = Utility.GetNoteData1(noteString, diff, ret.IsPro);
            ret.NoteFretDown = noteFret;
            
            if (isX)
            {
                ret.Channel = Utility.ChannelX;
            }
            else if (isArpeggio)
            {
                ret.Channel = Utility.ChannelArpeggio;
            }
            else if (isTap)
            {
                ret.Channel = Utility.ChannelTap;
            }
            
            return ret;
        }

        public virtual int NoteString
        {
            get
            {
                var ret = Utility.GetNoteString(Data1, IsPro);
                
                return ret;
            }
            set
            {
                Data1 = Utility.GetNoteData1(value, Difficulty, IsPro);
            }
        }

        public GuitarNote CloneToMemory(GuitarMessageList owner)
        {
            return GetNote(owner, Difficulty, this.TickPair, NoteString, NoteFretDown, IsTapNote, IsArpeggioNote, IsXNote);
        }


        public static GuitarNote CreateNote(GuitarMessageList owner, GuitarDifficulty diff, TickPair ticks,
            int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = GetNote(owner, diff, ticks, noteString, noteFret, isTap, isArpeggio, isX);

            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }

        public int NoteFretDown
        {
            get
            {
                return Data2 - 100;
            }
            set
            {
                Data2 = value + 100;
            }
        }

        public bool IsTapNote
        {
            get 
            { 
                return Channel == Utility.ChannelTap; 
            }
        }

        public bool IsArpeggioNote 
        { 
            get 
            { 
                return Channel == Utility.ChannelArpeggio; 
            } 
        }

        public bool IsXNote
        {
            get 
            { 
                return Channel == Utility.ChannelX; 
            }
        }

        public override string ToString()
        {
            return base.ToString() +
                " [" + NoteString.ToStringEx() + "]->" + NoteFretDown.ToStringEx() + " " + 
                (this.IsArpeggioNote ? "(helper)" : "") +
                (this.IsXNote ? "(x)" : "") + 
                (this.IsTapNote ? "(tap)":"");
        }
    }

}
