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
        public GuitarNote(GuitarMessageList track, MidiEvent downEvent = null, MidiEvent upEvent = null)
            : base(track, downEvent, upEvent, GuitarMessageType.GuitarNote)
        {

        }
        public GuitarNote(MidiEventPair ev)
            : base(ev, GuitarMessageType.GuitarNote)
        {

        }

        public virtual int NoteString
        {
            get
            {
                return Owner.Owner.IsPro ?
                    Utility.GetNoteString(Data1) :
                    Utility.GetNoteString5(Data1);
            }
            set
            {
                if (Owner.Owner.IsPro)
                {
                    Data1 = Utility.GetStringLowE(Utility.GetStringDifficulty6(Data1)) + value;
                }
                else
                {
                    Data1 = Utility.GetStringLowE5(Utility.GetStringDifficulty5(Data1)) + value;
                }

                base.IsUpdated = true;
            }
        }

        

        public GuitarNote CloneToMemory(GuitarMessageList list)
        {
            return GetNote(list, Difficulty, this.TickPair, NoteString, NoteFretDown, IsTapNote, IsArpeggioNote, IsXNote);
        }
        
        public static GuitarNote GetNote(GuitarMessageList list, GuitarDifficulty diff,
            TickPair ticks, int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = new GuitarNote(list);

            if (list.Owner.IsPro)
            {
                ret.Data1 = Utility.GetStringLowE(diff) + noteString;
            }
            else
            {
                ret.Data1 = Utility.GetStringLowE5(diff) + noteString;
            }

            ret.Data2 = noteFret + 100;

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
            else
            {
                ret.Channel = Utility.ChannelDefault;
            }

            ret.SetTicks(ticks);

            return ret;
        }

        public static GuitarNote CreateNote(GuitarMessageList list, GuitarDifficulty diff, TickPair ticks,
            int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = GetNote(list, diff, ticks, noteString, noteFret, isTap, isArpeggio, isX);

            var ev = list.Insert(ret.Data1, ret.Data2, ret.Channel, ticks);

            ret.SetDownEvent(ev.Down);
            ret.SetUpEvent(ev.Up);

            list.Add(ret);

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
            get { return Channel == Utility.ChannelTap; }
        }

        public bool IsArpeggioNote { get { return Channel == Utility.ChannelArpeggio; } }

        public bool IsXNote
        {
            get { return Channel == Utility.ChannelX; }
        }

        public override string ToString()
        {
            return base.ToString() +
                " String: " + NoteString + " Arp: " + this.IsArpeggioNote.ToString() + " X: " +
                this.IsXNote + " Tap: " + this.IsTapNote;
        }
    }

}
