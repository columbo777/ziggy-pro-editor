using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.DataLayer
{
    public enum MessageType
    {
        Unknown,
        GuitarHandPosition,
        GuitarTextEvent,
        GuitarTrainer,
        GuitarChord,
        GuitarPowerup,
        GuitarSolo,
        GuitarTempo,
        GuitarTimeSignature,
        GuitarArpeggio,
        GuitarBigRockEnding,
        GuitarSingleStringTremelo,
        GuitarMultiStringTremelo,
    }

    public class GMessage : DeletableEntity, IComparable<GMessage>
    {
        public MessageType MessageType
        {
            get
            {
                if (this is GuitarHandPosition)
                    return MessageType.GuitarHandPosition;
                else if (this is GuitarTextEvent)
                    return MessageType.GuitarTextEvent;
                else if (this is GuitarTrainer)
                    return MessageType.GuitarTrainer;
                else if (this is GuitarChord)
                    return MessageType.GuitarChord;
                else if (this is GuitarPowerup)
                    return MessageType.GuitarPowerup;
                else if (this is GuitarSolo)
                    return MessageType.GuitarSolo;
                else if (this is GuitarTempo)
                    return MessageType.GuitarTempo;
                else if (this is GuitarTimeSignature)
                    return MessageType.GuitarTimeSignature;
                else if (this is GuitarArpeggio)
                    return MessageType.GuitarArpeggio;
                else if (this is GuitarBigRockEnding)
                    return MessageType.GuitarBigRockEnding;
                else if (this is GuitarSingleStringTremelo)
                    return MessageType.GuitarSingleStringTremelo;
                else if (this is GuitarMultiStringTremelo)
                    return MessageType.GuitarMultiStringTremelo;
                else
                    return MessageType.Unknown;
            }
        }
        GuitarTrack ownerTrack;
        public GuitarTrack OwnerTrack
        {
            get{ return ownerTrack; }
        }

        public GMessage(GuitarTrack track, MidiEvent downEvent)
        {
            
            this.ownerTrack = track;
            
            this.downEvent = downEvent;
            
            if (downEvent != null)
            {
                this.downTick = downEvent.AbsoluteTicks;
            }
            else
            {
                this.downTick = Int32.MinValue;
            }
        }

        protected bool selected;

        public virtual bool Selected { get { return selected; } set { selected = value; } }

        protected MidiEvent downEvent;
        public virtual MidiEvent DownEvent
        {
            get
            {
                return downEvent;
            }
            set
            {
                downEvent = value;
                if (value != null)
                {
                    downTick = value.AbsoluteTicks;
                }
                this.IsUpdated = true;
            }
        }

        public MidiEvent MidiEvent
        {
            get { return DownEvent; }
            set 
            { 
                DownEvent = value; 
            }
        }

        public int AbsoluteTicks
        {
            get { return DownTick; }
            set { DownTick = value; }
        }

        protected int downTick;
        public virtual int DownTick
        {
            get
            {
                return downTick;
            }
            set
            {
                downTick = value;
                this.IsUpdated = true;
            }
        }

        public void SetDownEvent(MidiEvent ev)
        {
            this.downEvent = ev;
            if (ev != null)
            {
                this.downTick = ev.AbsoluteTicks;
            }
        }
        public virtual double StartTime 
        { 
            get 
            { 
                return OwnerTrack.TickToTime(DownTick);
            } 
        }

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty;
            }
            set
            {
                base.IsDirty = value;
            }
        }

        public virtual int CompareTo(GMessage other)
        {
            if (DownTick > other.DownTick)
            {
                return 1;
            }
            else if (DownTick < other.DownTick)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }



    public class GuitarMessage : GMessage
    {
        protected string text;
        public virtual string Text
        {
            get
            {
                return ( MidiEvent == null ? text : MidiEvent.ToString()); 
            }
            set
            {
                text = value;
                this.IsUpdated = true;
            }
        }
        public virtual T Clone<T>(GuitarTrack destTrack, int minTick, int maxTick) where T : GuitarMessage, new()
        {
            var cb = new ChannelMessageBuilder(this.downEvent.ChannelMessage);
            cb.Command = ChannelCommand.NoteOn;
            cb.Build();

            var downEvent = destTrack.Insert(minTick, cb.Result);

            cb.Command = ChannelCommand.NoteOff;
            cb.Build();
            var upEvent = destTrack.Insert(maxTick, cb.Result);

            T ret = new T();
            ret.SetDownEvent(downEvent);
            ret.SetUpEvent(upEvent);

            return ret;
        }

        public GuitarMessage(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent)
            : this(track, downEvent)
        {
            SetUpEvent(upEvent);
        }

        
        public GuitarMessage(GuitarTrack track, MidiEvent ev) : base(track, ev)
        {
            this.upTick = int.MinValue;

            SetDownEvent(ev);

            if (ev != null && ev.ChannelMessage != null)
            {
                data1 = ev.ChannelMessage.Data1;
                data2 = ev.ChannelMessage.Data2;

                if (downEvent != null && downEvent.ChannelMessage != null)
                {
                    command = (downEvent.ChannelMessage.Command == ChannelCommand.NoteOff || Data2 == 0) ? ChannelCommand.NoteOff : ChannelCommand.NoteOn;
                    channel = downEvent.ChannelMessage.MidiChannel;
                }
                else
                {
                    command = 0;
                    channel = 0;
                }
            }
            else
            {
                data1 = -1;
                data2 = -1;
                command = 0;
                channel = 0;
            }
        }

        public override double StartTime
        {
            get
            {
                return base.StartTime;
            }
        }

        public virtual double EndTime 
        { 
            get 
            { 
                return OwnerTrack.TickToTime(UpTick); 
            } 
        }

        protected int data1;
        public virtual int Data1
        {
            get
            {
                return data1;
            }
            set
            {
                data1 = value;
                this.IsUpdated = true;
            }
        }

        protected int data2;
        public virtual int Data2
        {
            get
            {
                return data2;
            }
            set
            {
                data2 = value;
                this.IsUpdated = true;
            }
        }

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
                    Data1 = Utility.GetStringLowE(Utility.GetStringDifficulty6(data1)) + value;
                }
                else
                {
                    Data1 = Utility.GetStringLowE5(Utility.GetStringDifficulty5(data1)) + value;
                }

                base.IsUpdated = true;
            }
        }

        
        ChannelCommand command;
        public virtual ChannelCommand Command
        {
            get
            {
                return (ChannelCommand)command;
            }
            set
            {
                command = value;
                IsUpdated = true;
            }
        }


        public virtual GuitarMessage ConvertDifficulty(GuitarDifficulty toDifficulty)
        {
            GuitarMessage ret = null;

            var d1 = Utility.GetModifierData1ForDifficulty(Data1, Difficulty, toDifficulty);
            if (d1 != -1)
            {
                ret = new GuitarMessage(OwnerTrack, null);
                ret.Data1 = d1;
                ret.Data2 = Data2;
                ret.Channel = Channel;
                ret.Command = Command;
                ret.DownTick = DownTick;
                
            }
            else
            {
                var ns = Utility.GetNoteString(Data1);
                if (ns != -1)
                {
                    ret = new GuitarMessage(OwnerTrack, null);
                    ret.Data1 = Utility.GetStringLowE(toDifficulty) + ns;
                    ret.Data2 = Data2;
                    ret.Channel = Channel;
                    ret.Command = Command;
                    ret.DownTick = DownTick;
                }
            }

            return ret;
        }
        

        int channel;
        public virtual int Channel
        {
            get
            {
                return channel;
            }
            set
            {
                channel = value;
                IsUpdated = true;
            }
        }

        public virtual GuitarDifficulty Difficulty
        {
            get
            {
                return Utility.GetDifficulty(Data1, OwnerTrack.IsPro);
            }
        }

        protected MidiEvent upEvent;
        public virtual MidiEvent UpEvent
        {
            get
            {
                return upEvent;
            }
            set
            {
                upEvent = value;
                if (value != null)
                {
                    upTick = value.AbsoluteTicks;
                }
                this.IsUpdated = true;
            }
        }
        protected int upTick;
        public virtual int UpTick
        {
            get
            {
                if (upTick.IsNull())
                    upTick = DownTick+1;

                return upTick;
            }
            set
            {
                upTick = value;
                this.IsUpdated = true;
            }
        }

        public override int DownTick
        {
            get
            {
                return base.DownTick;
            }
            set
            {
                base.DownTick = value;
            }
        }
        
        public void SetUpEvent(MidiEvent ev, int ticks = int.MinValue)
        {
            this.upEvent = ev;
            if (ticks.IsNull() && ev != null)
            {
                this.upTick = ev.AbsoluteTicks;
            }
            else
            {
                this.upTick = ticks;
            }
        }

        protected bool deleted;
       

        public override int GetHashCode()
        {
            int ret = (DownTick << 16) + DownTick;
          
            return ret;
        }

        public override string ToString()
        {
            return "Down: " + DownTick.ToString() + " Up: " + UpTick.ToString() + 
                " Deleted: " + this.IsDeleted + " Dirty: " + this.IsDirty;
        }


        public virtual ChannelMessage DownChannelMessage { get { return DownEvent.MidiMessage as ChannelMessage; } }
        public virtual ChannelMessage UpChannelMessage { get { return UpEvent.MidiMessage as ChannelMessage; } }

        public virtual int TickLength { get { return UpTick - DownTick; } }

        public virtual double TimeLength
        {
            get { return EndTime - StartTime; }
        }

        public virtual bool IsDownEventClose(GuitarMessage m2)
        {
            if (DownTick.IsNull() || m2.DownTick.IsNull())
                return false;

            var v = (int)Math.Abs(DownTick - m2.DownTick);
            return v <= Utility.NoteCloseWidth;
        }

        public virtual bool IsUpEventClose(GuitarMessage m2)
        {
            var v = Math.Abs(UpTick - m2.UpTick);
            return v <= Utility.NoteCloseWidth;
        }

        public virtual bool IsBefore(GuitarMessage obj)
        {
            return (DownTick - obj.DownTick) < 0;
        }

        public virtual bool IsAfter(GuitarMessage obj)
        {
            return (DownTick - obj.DownTick) > 0;
        }

        
    }


}
