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

        MidiEventPair _eventPair;
        public MidiEventPair EventPair
        {
            get
            {
                return _eventPair;
            }

        }

        public MidiEventProps(GuitarMessageList owner = null)
        {
            resetProps(owner);
        }

        private void resetProps(GuitarMessageList owner = null)
        {
            Owner = owner;
            Data1 = Int32.MinValue;
            Data2 = Int32.MinValue;
            Channel = Int32.MinValue;
            Command = ChannelCommand.Invalid;
            Text = string.Empty;
            TickPair = TickPair.NullValue;
            TimePair = TimePair.NullValue;
            _eventPair = new MidiEventPair(owner);
        }

        public MidiEventProps CloneToMemory(GuitarMessageList owner)
        {
            var ret = new MidiEventProps(owner);
            ret.setEventPair(EventPair.CloneToMemory(owner));
            ret.Owner = Owner;
            ret.Data1 = Data1;
            ret.Data2 = Data2;
            ret.Channel = Channel;
            ret.Command = Command;
            ret.Text = Text;
            ret.TickPair = new TickPair(TickPair);
            ret.TimePair = new TimePair(TimePair);
            return ret;
        }

        public MidiEventProps(GuitarMessageList owner, MidiEvent down, MidiEvent up)
            : this(owner, new MidiEventPair(owner, down, up))
        {

        }
        public MidiEventProps(GuitarMessageList owner, MidiEventPair pair)
            : this(owner)
        {
            setEventPair(pair);
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
            resetProps(Owner);
            setEventPair(pair);
        }

        private void setEventPair(MidiEventPair pair)
        {
            _eventPair = new MidiEventPair(pair);

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
                    Text = _eventPair.Down.MetaMessage.Text;
                }
            }

            this.TickPair = new TickPair((_eventPair.Down == null ? Int32.MinValue : _eventPair.Down.AbsoluteTicks),
                                     (_eventPair.Up == null ? Int32.MinValue : _eventPair.Up.AbsoluteTicks));
        }
    }
}