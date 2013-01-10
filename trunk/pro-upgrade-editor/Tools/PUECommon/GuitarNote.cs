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
        public virtual int NoteString
        {
            get
            {
                return OwnerTrack.IsPro ?
                    Utility.GetNoteString(Data1) :
                    Utility.GetNoteString5(Data1);
            }
            set
            {
                if (OwnerTrack.IsPro)
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

        public GuitarNote(GuitarTrack track, MidiEvent downEvent = null, MidiEvent upEvent = null)
            : base(track, downEvent, upEvent, GuitarMessageType.GuitarNote)
        {
            
        }

        public GuitarNote CloneToMemory(GuitarTrack track)
        {
            return GetNote(track, Difficulty, this.TickPair, NoteString, NoteFretDown, IsTapNote, IsArpeggioNote, IsXNote);
        }

        public static GuitarNote GetNote(GuitarTrack track, GuitarDifficulty diff, 
            TickPair ticks, int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = new GuitarNote(track);
            if (track.IsPro)
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

        public static GuitarNote CreateNote(GuitarTrack track, GuitarDifficulty diff, TickPair ticks, 
            int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = GetNote(track, diff, ticks, noteString, noteFret, isTap, isArpeggio, isX);

            var ev = track.Insert(ret.Data1, ret.Data2, ret.Channel, ticks);

            ret.SetDownEvent(ev.Down);
            ret.SetUpEvent(ev.Up);

            track.Messages.Add(ret);

            return ret;
        }

        public int NoteFretDown 
        { 
            get 
            { 
                return Data2-100;
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
