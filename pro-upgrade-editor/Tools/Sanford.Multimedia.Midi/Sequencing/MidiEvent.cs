

using System;

namespace Sanford.Multimedia.Midi
{
    public class MidiEvent : IComparable<MidiEvent>
    {
        private Track owner = null;

        private int absoluteTicks;

        private IMidiMessage message;

        private MidiEvent next = null;

        private MidiEvent previous = null;

        bool dirty;
        bool deleted;


        public override string ToString()
        {
            if (message != null)
            {
                if (message.MessageType == MessageType.Meta)
                {
                    return  ((MetaMessage)message).ToString();
                }
                var cm = message as ChannelMessage;
                if (cm != null)
                {
                    return AbsoluteTicks.ToString() + " - " + message.MessageType.ToString() + " cmd: " + cm.Command.ToString() + " d1: " + cm.Data1 + " d2: " + cm.Data2.ToString() +
                        " chan: " + cm.MidiChannel.ToString(); ;
                }
                else
                {
                    return AbsoluteTicks.ToString() + " - " +  cm.MessageType.ToString();
                }
            }
            return "";
        }

        public MidiEvent(Track owner, int absoluteTicks, IMidiMessage message, bool startDirty=true)
        {
            this.owner = owner;
            this.absoluteTicks = absoluteTicks;
            this.message = message;
            dirty = startDirty;
            deleted = false;
        }

        internal void SetAbsoluteTicks(int absoluteTicks)
        {
            this.absoluteTicks = absoluteTicks;
            dirty = true;
        }

        public Track Owner
        {
            get
            {
                return owner;
            }
        }

        public int AbsoluteTicks
        {
            get
            {
                return absoluteTicks;
            }
            internal set { absoluteTicks = value; }
        }

        public int DeltaTicks
        {
            get
            {
                int deltaTicks;

                if(Previous != null)
                {
                    deltaTicks = AbsoluteTicks - previous.AbsoluteTicks;
                }
                else
                {
                    deltaTicks = AbsoluteTicks;
                }

                return deltaTicks;
            }
        }

        public IMidiMessage MidiMessage
        {
            get
            {
                return message;
            }
        }

        public MidiEvent Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }

        public MidiEvent Previous
        {
            get
            {
                return previous;
            }
            set
            {
                previous = value;
            }
        }

        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; dirty = true; }
        }

        public ChannelMessage ChannelMessage
        {
            get
            {
                return this.MidiMessage as ChannelMessage;
            }
        }

        public MetaMessage MetaMessage
        {
            get
            {
                return this.MidiMessage as MetaMessage;
            }
        }
        public int CompareTo(MidiEvent other)
        {
            if (AbsoluteTicks < other.AbsoluteTicks)
            {
                return -1;
            }
            else if (AbsoluteTicks > other.AbsoluteTicks)
            {
                return 1;
            }
            else
            {
                if (this.MidiMessage.MessageType == MessageType.Meta &&
                    other.MidiMessage.MessageType == MessageType.Meta)
                {
                    var cm = this.MidiMessage as MetaMessage;
                    var om = other.MidiMessage as MetaMessage;

                    if (cm.MetaType == MetaType.TrackName)
                        return -1;
                    else if (cm.MetaType == MetaType.EndOfTrack)
                        return 1;
                    else if (om.MetaType == MetaType.EndOfTrack)
                        return -1;
                    else if (om.MetaType == MetaType.TrackName)
                        return 1;
                    else
                    {
                        if (this.absoluteTicks < other.absoluteTicks)
                            return -1;
                        else if (this.absoluteTicks > other.absoluteTicks)
                            return 1;
                        else return 0;
                    }
                }
                else if (this.MidiMessage.MessageType == MessageType.Channel && 
                    other.MidiMessage.MessageType == MessageType.Channel)
                {
                    var cm = this.MidiMessage as ChannelMessage;
                    var om = other.MidiMessage as ChannelMessage;

                    
                    if (cm.Command == ChannelCommand.NoteOff && om.Command == ChannelCommand.NoteOn)
                    {
                        return -1;
                    }
                    else if (cm.Command == ChannelCommand.NoteOn && om.Command == ChannelCommand.NoteOff)
                    {
                        return 1;
                    }
                    else
                    {
                        if (cm.Command == om.Command)
                        {
                            if (cm.Data1 < om.Data1)
                                return -1;
                            if (cm.Data1 > om.Data1)
                                return 1;
                            return 0;
                        }
                        return 0;
                    }
                }
                else
                {
                    if (this.absoluteTicks < other.absoluteTicks)
                        return -1;
                    else if (this.absoluteTicks > other.absoluteTicks)
                        return 1;
                    else return 0;
                }
            }
        }
    }
}
