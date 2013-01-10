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

        bool deleted;

        public int Data1 { get { return this.ChannelMessage != null ? this.ChannelMessage.Data1 : int.MinValue; } }
        public int Data2 { get { return this.ChannelMessage != null ? this.ChannelMessage.Data2 : int.MinValue; } }
        public ChannelCommand Command 
        { 
            get 
            {
                if (this.ChannelMessage != null)
                {
                    if (owner != null && owner.FileType == FileType.Guitar5)
                    {
                        if (Data2 == 0 || Data2 == 64)
                        {
                            return ChannelCommand.NoteOff;
                        }
                        return this.ChannelMessage.Command;
                    }
                    else
                        return ChannelMessage.Command;
                }
                else
                    return 0;
            } 
        }
        public int Channel { get { return this.ChannelMessage != null ? this.ChannelMessage.MidiChannel : int.MinValue; } }

        public MessageType MessageType
        {
            get { return message == null ? MessageType.Unknown : message.MessageType; }
        }
        public MetaType MetaType
        {
            get { return MetaMessage == null ? MetaType.Unknown : MetaMessage.MetaType; }
        }

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

        public MidiEvent(Track owner, int absoluteTicks, IMidiMessage message)
        {
            this.owner = owner;
            this.absoluteTicks = absoluteTicks;
            this.message = message;
            deleted = false;
        }

        
        public IMidiMessage Clone()
        {
            IMidiMessage ret = null;

            if (this.MessageType == MessageType.Channel)
            {
                var cb = new ChannelMessageBuilder(this.ChannelMessage);
                cb.Build();
                ret = cb.Result;
            }
            else if (this.MessageType == MessageType.Meta)
            {
                var mb = new MetaTextBuilder(this.MetaMessage);
                mb.Build();
                ret = mb.Result;
            }
            else if(this.MessageType == MessageType.SystemCommon)
            {
                var sb = new SysCommonMessageBuilder(this.MidiMessage as SysCommonMessage);
                sb.Build();
                ret = sb.Result;
            }
            else if(this.MessageType == MessageType.SystemExclusive) 
            {
                ret = new SysExMessage((this.MidiMessage as SysExMessage).GetBytes());
            }
            else if (this.MessageType == MessageType.SystemRealtime)
            {
                ret = (this.MidiMessage as SysRealtimeMessage);
            }
            return ret;
        }

        internal void SetAbsoluteTicks(int absoluteTicks)
        {
            this.absoluteTicks = absoluteTicks;
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
            set 
            {
                if (absoluteTicks != value)
                {
                    absoluteTicks = value;
                    if (this.message != null && owner != null)
                    {
                        owner.Move(this, absoluteTicks);
                    }
                }
            }
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

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
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
        public int CompareTo(MidiEvent b)
        {
            if (AbsoluteTicks < b.AbsoluteTicks)
            {
                return -1;
            }
            else if (AbsoluteTicks > b.AbsoluteTicks)
            {
                return 1;
            }
            else
            {
                if (this.MessageType == MessageType.Meta &&
                    b.MessageType == MessageType.Meta)
                {
                    var cm = this.MetaMessage;
                    var bMessage = b.MetaMessage;

                    if (cm.MetaType == MetaType.TrackName)
                        return -1;
                    else if (cm.MetaType == MetaType.EndOfTrack)
                        return 1;
                    else if (bMessage.MetaType == MetaType.EndOfTrack)
                        return -1;
                    else if (bMessage.MetaType == MetaType.TrackName)
                        return 1;
                    else
                    {
                        if (this.absoluteTicks < b.absoluteTicks)
                            return -1;
                        else if (this.absoluteTicks > b.absoluteTicks)
                            return 1;
                        else return 0;
                    }
                }
                else if (this.MessageType == MessageType.Channel && 
                        b.MessageType == MessageType.Channel)
                {
                    var cm = this.ChannelMessage;
                    var bMessage = b.ChannelMessage;

                    var isOffA = cm.Command == ChannelCommand.NoteOff;
                    var isOffB = bMessage.Command == ChannelCommand.NoteOff;

                    if (isOffA && !isOffB)
                    {
                        return -1;
                    }
                    else if (!isOffA && isOffB)
                    {
                        return 1;
                    }
                    else
                    {
                        if (isOffA == isOffB)
                        {
                            if (cm.Data1 < bMessage.Data1)
                                return -1;
                            if (cm.Data1 > bMessage.Data1)
                                return 1;
                            return 0;
                        }
                        return 0;
                    }
                }
                else
                {
                   return 0;
                }
            }
        }
    }
}
