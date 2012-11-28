using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarNote : GuitarMessage
    {

        public GuitarNote(GuitarTrack track, MidiEvent downEvent = null, MidiEvent upEvent = null)
            : base(track, downEvent)
        {
            SetUpEvent(upEvent);
        }

        public GuitarNote Clone(GuitarTrack destTrack, GuitarDifficulty difficulty, int minTick, int maxTick, int stringOffset = Int32.MinValue)
        {
            stringOffset = stringOffset.IsNull() ? 0 : stringOffset;

            return GuitarNote.CreateNote(destTrack, difficulty, minTick, maxTick, NoteString+stringOffset, NoteFretDown, IsTapNote, IsArpeggioNote, IsXNote);
        }

        public static GuitarNote GetNote(GuitarTrack track, GuitarDifficulty diff, int downTick, int upTick, int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
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
            else
            {
                ret.Channel = Utility.ChannelDefault;
            }

            ret.downTick = downTick;
            ret.upTick = upTick;

            return ret;
        }

        public static GuitarNote CreateNote(GuitarTrack track, GuitarDifficulty diff,int downTick, int upTick, int noteString, int noteFret, bool isTap, bool isArpeggio, bool isX)
        {
            var ret = GetNote(track, diff, downTick, upTick, noteString, noteFret, isTap, isArpeggio, isX);


            var cb = new ChannelMessageBuilder();
            cb.Data1 = ret.Data1;
            cb.Data2 = ret.Data2;
            cb.MidiChannel = ret.Channel;
            cb.Command = ChannelCommand.NoteOn;

            cb.Build();

            var downEvent = track.Insert(downTick, cb.Result);

            cb.Command = ChannelCommand.NoteOff;
            cb.Data2 = 0;
            cb.Build();

            var upEvent = track.Insert(upTick, cb.Result);

            ret.SetDownEvent(downEvent);
            ret.SetUpEvent(upEvent);

            
            ret.IsNew = true;
            return ret;
        }

        public int NoteFretDown { get { return Data2-100; } set { Data2 = value + 100; } }

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
