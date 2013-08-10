using System;
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


        public GuitarMessage()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.props = new MidiEventProps(null, TickPair.NullValue);
            this.messageType = GuitarMessageType.Unknown;
            Data1 = Int32.MinValue;
            Data2 = Utility.Data2Default;
            Channel = Utility.ChannelDefault;
            TickPair = TickPair.NullValue;
        }

        public GuitarMessage(GuitarMessageList owner, MidiEvent downEvent, MidiEvent upEvent, GuitarMessageType type)
            : this(new MidiEventPair(owner, downEvent, upEvent), type)
        {

        }
        public GuitarMessage(MidiEventPair pair, GuitarMessageType type)
            : this()
        {

            this.messageType = type;
            this.props = new MidiEventProps(pair, type);
            SetTicks(pair.TickPair);
        }

        public GuitarMessage(GuitarMessageList owner, TickPair pair, GuitarMessageType type)
        {
            this.messageType = type;
            this.props = new MidiEventProps(owner, pair);

            this.SetTicks(pair);
        }

        public virtual bool IsPro
        {
            get
            {
                return props.IsPro;
            }
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
            if (Owner != null)
            {
                Owner.Remove(EventPair);
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
            if (IsDeleted)
            {
                Debug.WriteLine("adding deleted");
            }

            if (Owner != null)
            {
                Owner.Add(this);
            }
            IsNew = false;
            IsDeleted = false;

        }

        public virtual void RemoveFromList()
        {
            if (!IsDeleted)
            {
                Owner.Remove(this);
                IsDeleted = true;
            }
            else
            {
                Debug.WriteLine("removing deleted");
            }
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
                    ("create events non basic").OutputDebug();
                    return;
                }

                if (Data2.IsNull())
                {
                    ("data2 missing").OutputDebug();
                    return;
                }
                if (Data1.IsNull())
                {
                    ("data1 missing").OutputDebug();
                    return;
                }

                if (TickPair.IsValid == false)
                {
                    ("invalid TickPair").OutputDebug();
                    return;
                }

                if (Channel.IsNull())
                {
                    ("invalid channel").OutputDebug();
                    Channel = 0;
                }
                if (Owner.IsNotNull())
                {
                    props.SetUpdatedEventPair(Owner.Insert(Data1, Data2, Channel, TickPair));
                }
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
        public virtual TickPair TickPair { get { return new TickPair(DownTick, UpTick); } set { props.TickPair = value; } }
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
                return Utility.GetDifficulty(Data1, IsPro);
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


        private GuitarMessageType GetMessageType()
        {
            var ret = messageType;
            if (ret == GuitarMessageType.Unknown)
            {
                if (this is GuitarHandPosition)
                    ret = GuitarMessageType.GuitarHandPosition;
                if (this is GuitarChordName)
                    ret = GuitarMessageType.GuitarChordName;
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
