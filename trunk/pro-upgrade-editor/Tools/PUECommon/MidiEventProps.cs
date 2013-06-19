using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class MidiEventProps
    {
        public GuitarMessageList Owner { get; set; }

        public int Data2 { get; set; }

        public int Data1 { get; set; }

        public int Channel { get; set; }

        public ChannelCommand Command { get; set; }

        public string Text { get; set; }

        public TickPair TickPair { get; set; }

        public TimePair TimePair { get; set; }

        public bool IsPro { get; set; }

        MidiEventPair _eventPair;
        public MidiEventPair EventPair
        {
            get
            {
                return _eventPair;
            }

        }

        public MidiEventProps(GuitarMessageList owner, TickPair ticks)
        {
            resetProps(owner);
            TickPair = ticks;
        }
        public MidiEventProps(MidiEventPair pair, GuitarMessageType type)
        {
            if (type == GuitarMessageType.GuitarTempo)
            {
                setEventPair(pair, false);
                TickPair = pair.TickPair;
            }
            else
            {
                setEventPair(pair);
                TickPair = pair.TickPair;
            }
        }

        public MidiEventProps(MidiEventPair pair)
        {
            setEventPair(pair);
            TickPair = pair.TickPair;
        }

        private void resetProps(GuitarMessageList owner)
        {
            Owner = owner;
            IsPro = (owner == null ? true : owner.IsPro);
            Data1 = Int32.MinValue;
            Data2 = Int32.MinValue;
            Channel = Int32.MinValue;
            Command = ChannelCommand.Invalid;
            Text = string.Empty;
            TickPair = TickPair.NullValue;
            TimePair = TimePair.NullValue;

            _eventPair = new MidiEventPair(owner);
        }


        public void SetDownEvent(MidiEvent ev)
        {
            _eventPair.Down = ev;
        }

        public void SetUpEvent(MidiEvent ev)
        {
            _eventPair.Up = ev;
        }

        public void SetUpdatedEventPair(MidiEventPair pair)
        {
            setEventPair(pair);
        }
        public void SetUpdatedEventPair(MidiEvent ev)
        {
            setEventPair(new MidiEventPair(Owner, ev));
        }

        private void setEventPair(MidiEventPair pair, bool calcTime=true)
        {
            resetProps(pair.Owner);

            _eventPair = pair;

            if (_eventPair.Down != null)
            {
                if (_eventPair.Down.IsChannelEvent())
                {
                    Data1 = _eventPair.Down.Data1;
                    Data2 = _eventPair.Down.Data2;
                    Channel = _eventPair.Down.Channel;
                    Command = _eventPair.Down.Command;
                }

                if (_eventPair.Down.IsTextEvent())
                {
                    Text = _eventPair.Down.Text;
                }
            }

            this.TickPair = new TickPair((_eventPair.Down == null ? Int32.MinValue : _eventPair.Down.AbsoluteTicks),
                                     (_eventPair.Up == null ? Int32.MinValue : _eventPair.Up.AbsoluteTicks));
            if (calcTime)
            {
                if (TickPair.IsValid)
                {
                    this.TimePair = new TimePair(Owner.Owner.GuitarTrack.TickToTime(TickPair));
                }
                else if (TickPair.Down.IsNotNull())
                {
                    var time = Owner.Owner.GuitarTrack.TickToTime(TickPair.Down);
                    this.TimePair = new TimePair(time, time);
                }
            }
        }
    }
}