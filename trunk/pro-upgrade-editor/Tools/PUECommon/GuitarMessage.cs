﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    

    public class GuitarMessage : DeletableEntity
    {
        protected GuitarMessageType messageType;
        protected MidiEventProps props;

        protected bool selected;


        public GuitarMessage(GuitarMessageList owner, MidiEvent downEvent, MidiEvent upEvent, GuitarMessageType type)
            : this(owner, new MidiEventPair(owner, downEvent, upEvent), type)
        {

        }
        public GuitarMessage(MidiEventPair pair, GuitarMessageType type)
            : this(pair.Owner, pair, type)
        {

        }
        public GuitarMessage(GuitarMessageList owner, MidiEventProps props, GuitarMessageType type)
        {
            this.props = props.CloneToMemory(owner);
            this.messageType = type;
        }

        public GuitarMessage(GuitarMessageList list, MidiEventPair pair, GuitarMessageType type)
        {
            this.messageType = type;
            this.props = new MidiEventProps(list, pair);
        }

        public virtual void UpdateEvents()
        {
            if (!IsBasicChannelEvent)
            {
                Debug.WriteLine("Invalid updateevents");
            }

            RemoveEvents();
            CreateEvents();
            IsUpdated = false;
        }


        public virtual void RemoveEvents()
        {
            if (!IsBasicChannelEvent)
            {
                Debug.WriteLine("Invalid removeevents");
            }


            if (UpEvent != null && Owner != null)
            {
                Owner.Remove(UpEvent);

                SetUpEvent(null);
            }
            if (DownEvent != null && Owner != null)
            {
                Owner.Remove(DownEvent);

                SetDownEvent(null);
            }

        }

        public virtual int DefaultData1
        {
            get
            {
                int ret = Int32.MinValue;
                switch (messageType)
                {
                    case GuitarMessageType.GuitarHandPosition: { if (Difficulty.IsExpert()) { ret = Utility.HandPositionData1; } }
                        break;
                    case GuitarMessageType.GuitarPowerup: { if (Difficulty.IsExpert()) ret = Utility.PowerupData1; }
                        break;
                    case GuitarMessageType.GuitarSolo: { if (Difficulty.IsExpert()) ret = Utility.SoloData1; }
                        break;
                    case GuitarMessageType.GuitarArpeggio: { ret = Utility.GetArpeggioData1(Difficulty); }
                        break;
                    case GuitarMessageType.GuitarSingleStringTremelo: { if (Difficulty.IsExpert()) ret = Utility.SingleStringTremeloData1; }
                        break;
                    case GuitarMessageType.GuitarMultiStringTremelo: { if (Difficulty.IsExpert()) ret = Utility.MultiStringTremeloData1; }
                        break;
                    case GuitarMessageType.GuitarSlide: { ret = Utility.GetSlideData1(this.Difficulty); }
                        break;
                    case GuitarMessageType.GuitarHammeron: { ret = Utility.GetHammeronData1(this.Difficulty); }
                        break;
                }
                if (ret == -1)
                    ret = Int32.MinValue;
                return ret;
            }
        }

        public virtual bool IsBasicChannelEvent
        {
            get
            {
                var ret = true;
                switch (messageType)
                {
                    case GuitarMessageType.GuitarTempo:
                    case GuitarMessageType.GuitarTimeSignature:
                    case GuitarMessageType.GuitarBigRockEnding:
                    case GuitarMessageType.GuitarBigRockEndingSubMessage:
                    case GuitarMessageType.GuitarTextEvent:
                    case GuitarMessageType.GuitarTrainer:
                    case GuitarMessageType.GuitarChord:
                    case GuitarMessageType.GuitarChordStrum:

                        ret = false;
                        break;
                }
                return ret;
            }
        }

        public virtual void AddToList()
        {
            IsDeleted = false;
            Owner.Add(this);
            IsNew = false;
        }

        public virtual void RemoveFromList()
        {
            Owner.Remove(this);
            IsDeleted = true;
        }

        public virtual void DeleteAll()
        {
            RemoveEvents();
            RemoveFromList();
        }

        public virtual void CreateEvents()
        {

            if (!HasEvents)
            {
                if (!IsBasicChannelEvent)
                {
                    Debug.WriteLine("Invalid createevents - " + this.ToString());
                }

                if (Data2.IsNull())
                {
                    Data2 = 100;
                }
                if (Data1.IsNull())
                {
                    Data1 = DefaultData1;
                }

                if (Data1.IsNull() || TickPair.IsValid == false || Owner == null)
                {
                    Debug.WriteLine("invalid properties - " + this.ToString());
                    return;
                }

                props.SetUpdatedEventPair(Owner.Insert(props.Data1, props.Data2, props.Channel, props.TickPair));
            }
            if (IsNew)
            {
                AddToList();
            }
        }

        public GuitarMessageType MessageType
        {
            get
            {
                return GetMessageType();
            }
            set
            {
                messageType = value;

            }
        }


        public GuitarMessageList Owner
        {
            get { return props.Owner; }
        }


        public virtual bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }


        public virtual MidiEvent DownEvent
        {
            get
            {
                return props.EventPair.Down;
            }
        }

        public virtual MidiEvent UpEvent
        {
            get
            {
                return props.EventPair.Up;
            }

        }

        public MidiEvent MidiEvent
        {
            get { return DownEvent; }
        }

        public int AbsoluteTicks
        {
            get { return DownTick; }
        }

        public virtual int DownTick
        {
            get
            {
                return props.TickPair.Down;
            }
        }

        public virtual int UpTick
        {
            get
            {
                return props.TickPair.Up;
            }
        }

        public virtual void SetDownTick(int tick)
        {
            props.TickPair = new TickPair(tick, props.TickPair.Up);
        }

        public virtual void SetUpTick(int tick)
        {
            props.TickPair = new TickPair(props.TickPair.Down, tick);
        }

        public virtual void SetTicks(TickPair ticks)
        {
            props.TickPair = new TickPair(ticks);
        }

        public virtual void SetDownEvent(MidiEvent ev)
        {
            props.SetDownEvent(ev);
        }


        public virtual void SnapEvents()
        {
            var newTicks = Owner.Owner.SnapLeftRightTicks(TickPair, new SnapConfig(true, true, true));
            if (newTicks.CompareTo(TickPair) != 0)
            {
                SetTicks(newTicks);
                if (HasEvents)
                {
                    UpdateEvents();
                }
            }
        }

        public virtual bool HasEvents
        {
            get { return HasDownEvent || HasUpEvent; }
        }

        public virtual bool HasDownEvent
        {
            get { return DownEvent != null && DownEvent.Deleted == false; }
        }

        public virtual bool HasUpEvent
        {
            get { return UpEvent != null && UpEvent.Deleted == false; }
        }

        public virtual void SetMidiEvent(MidiEvent ev)
        {
            SetDownEvent(ev);
        }

        public virtual void SetUpEvent(MidiEvent ev)
        {
            props.SetUpEvent(ev);
        }


        public virtual MidiEventPair EventPair { get { return props.EventPair; } }
        public virtual TickPair TickPair { get { return new TickPair(DownTick, UpTick); } }
        public virtual TimePair TimePair { get { return new TimePair(StartTime, EndTime); } }
        public virtual TickPair ScreenPointPair { get { return new TickPair(StartScreenPoint, EndScreenPoint); } }


        public virtual string Text
        {
            get
            {
                return props.Text;
            }
            set
            {
                props.Text = value;
            }
        }

        public virtual int StartScreenPoint
        {
            get
            {
                return (int)Math.Round(Utility.ScaleUp(StartTime));
            }
        }

        public virtual int EndScreenPoint
        {
            get
            {
                return (int)Math.Round(Utility.ScaleUp(EndTime));
            }
        }

        public virtual double StartTime
        {
            get
            {
                return Owner.Owner.GuitarTrack.TickToTime(DownTick);
            }
        }

        public virtual double EndTime
        {
            get
            {
                return Owner.Owner.GuitarTrack.TickToTime(UpTick);
            }
        }

        public virtual double TimeLength
        {
            get { return EndTime - StartTime; }
        }

        public virtual int Data1
        {
            get
            {
                return props.Data1;
            }
            set
            {
                props.Data1 = value;
            }
        }


        public virtual int Data2
        {
            get
            {
                return props.Data2;
            }
            set
            {
                props.Data2 = value;
            }
        }

        public bool IsOn { get { return Command == ChannelCommand.NoteOn; } }
        public bool IsOff { get { return Command == ChannelCommand.NoteOff; } }

        public virtual ChannelCommand Command
        {
            get
            {
                return props.Command;
            }
            set
            {
                props.Command = value;
            }
        }



        public virtual int Channel
        {
            get
            {
                return props.Channel;
            }
            set
            {
                props.Channel = value;
            }
        }

        public virtual GuitarDifficulty Difficulty
        {
            get
            {
                return Utility.GetDifficulty(Data1, Owner.Owner.IsPro);
            }
        }

        public override string ToString()
        {
            return TickPair.ToString() + " " + MessageType.ToString() +
                (IsDeleted ? " (Deleted)" : "") +
                (IsNew ? " (New)" : "") +
                (!HasDownEvent ? " (no down)" : "") +
                (!HasUpEvent ? " (no up)" : "");
        }


        public virtual ChannelMessage DownChannelMessage { get { return DownEvent.ChannelMessage; } }
        public virtual ChannelMessage UpChannelMessage { get { return UpEvent.ChannelMessage; } }

        public virtual int TickLength { get { return UpTick - DownTick; } }


        public virtual bool IsDownEventClose(GuitarMessage m2)
        {
            if (DownTick.IsNull() || m2.DownTick.IsNull())
                return false;

            return TickPair.IsCloseDownDown(m2.TickPair);
        }

        public virtual bool IsUpEventClose(GuitarMessage m2)
        {
            if (UpTick.IsNull() || m2.UpTick.IsNull())
                return false;

            return TickPair.IsCloseUpUp(m2.TickPair);
        }

        public override bool IsUpdated
        {
            get
            {
                return base.IsUpdated;
            }
            set
            {
                base.IsUpdated = value;
            }
        }

        public override bool IsDeleted
        {
            get
            {
                return base.IsDeleted;
            }
            set
            {
                base.IsDeleted = value;
            }
        }

        public override bool IsNew
        {
            get
            {
                return base.IsNew;
            }
            set
            {
                base.IsNew = value;
            }
        }

        private GuitarMessageType GetMessageType()
        {
            var ret = messageType;
            if (ret == GuitarMessageType.Unknown)
            {
                if (this is GuitarHandPosition)
                    ret = GuitarMessageType.GuitarHandPosition;
                else if (this is GuitarTextEvent)
                    ret = GuitarMessageType.GuitarTextEvent;
                else if (this is GuitarTrainer)
                    ret = GuitarMessageType.GuitarTrainer;
                else if (this is GuitarChord)
                    ret = GuitarMessageType.GuitarChord;
                else if (this is GuitarNote)
                    ret = GuitarMessageType.GuitarNote;
                else if (this is GuitarPowerup)
                    ret = GuitarMessageType.GuitarPowerup;
                else if (this is GuitarSolo)
                    ret = GuitarMessageType.GuitarSolo;
                else if (this is GuitarTempo)
                    ret = GuitarMessageType.GuitarTempo;
                else if (this is GuitarTimeSignature)
                    ret = GuitarMessageType.GuitarTimeSignature;
                else if (this is GuitarArpeggio)
                    ret = GuitarMessageType.GuitarArpeggio;
                else if (this is GuitarBigRockEnding)
                    ret = GuitarMessageType.GuitarBigRockEnding;
                else if (this is GuitarBigRockEndingSubMessage)
                    ret = GuitarMessageType.GuitarBigRockEndingSubMessage;
                else if (this is GuitarSingleStringTremelo)
                    ret = GuitarMessageType.GuitarSingleStringTremelo;
                else if (this is GuitarMultiStringTremelo)
                    ret = GuitarMessageType.GuitarMultiStringTremelo;
                else if (this is GuitarSlide)
                    ret = GuitarMessageType.GuitarSlide;
                else if (this is GuitarHammeron)
                    ret = GuitarMessageType.GuitarHammeron;
            }
            return ret;
        }

    }


}
