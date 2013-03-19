using System;
using System.Text;

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
        int data1;
        int data2;
        public int Data1
        {
            get
            {
                return data1;
                
            }
        }
        public int Data2
        {
            get
            {
                return data2;
                
            }
        }
        int channelCommand;
        public ChannelCommand Command
        {
            get
            {
                return (ChannelCommand)channelCommand;
            }
        }

        public bool IsOn { get { return channelCommand == (int)ChannelCommand.NoteOn; } }
        public bool IsOff { get { return channelCommand == (int)ChannelCommand.NoteOff; } }

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
            var ret = new StringBuilder(64);

            if (message != null)
            {
                if (MessageType == MessageType.Meta)
                {
                    var meta = message as MetaMessage;
                    if (meta.MetaType == Midi.MetaType.TrackName)
                    {
                        ret.Append(meta.Text);
                    }
                    else
                    {
                        ret.Append(AbsoluteTicks.ToString());
                        ret.Append(" - ");
                        ret.Append(this.MetaMessage.ToString());
                    }
                }
                else if (MessageType == Midi.MessageType.Channel)
                {
                    int msg = (message as ChannelMessage).Message;
                    ret.Append(AbsoluteTicks.ToString());
                    ret.Append(" cmd: ");
                    ret.Append(ChannelMessage.UnpackCommand(msg));

                    ret.Append(" chan: ");
                    ret.Append(ChannelMessage.UnpackMidiChannel(msg));

                    ret.Append(" d1: ");
                    ret.Append(ChannelMessage.UnpackData1(msg));

                    ret.Append(" d2: ");
                    ret.Append(ChannelMessage.UnpackData2(msg));

                    ret.Append(" - stat: ");
                    ret.Append(ChannelMessage.UnpackStatus(msg));
                }
                else if (message is ShortMessage)
                {
                    var msg = (message as ShortMessage).Message;
                    ret.Append(AbsoluteTicks.ToString());
                    ret.Append(" - d1: ");
                    ret.Append(ShortMessage.UnpackData1(msg));

                    ret.Append(" - d2: ");
                    ret.Append(ShortMessage.UnpackData2(msg));

                    ret.Append(" - stat: ");
                    ret.Append(ShortMessage.UnpackStatus(msg));
                }
                else
                {
                    ret.Append(AbsoluteTicks.ToString());
                    ret.Append(this.MessageType.ToString());
                }
            }
            return ret.ToString();
        }

        public MidiEvent(Track owner, int absoluteTicks, IMidiMessage message)
        {
            this.owner = owner;
            this.absoluteTicks = absoluteTicks;
            this.message = message;
            deleted = false;

            data2 = data1 = Int32.MinValue;
            channelCommand = 0;

            if (message is ChannelMessage)
            {
                int msg = (message as ChannelMessage).Message;
                channelCommand = (int)ChannelMessage.UnpackCommand(msg);
                data1 = ChannelMessage.UnpackData1(msg);
                data2 = ChannelMessage.UnpackData2(msg);
            }
            else if (message is ShortMessage)
            {
                var msg = (message as ShortMessage).Message;
                data1 = ShortMessage.UnpackData1(msg);
                data2 = ShortMessage.UnpackData2(msg);
            }
        }


        public IMidiMessage Clone()
        {
            IMidiMessage ret = null;

            if (this.MessageType == MessageType.Channel)
            {
                ret = new ChannelMessage(ChannelMessage.Message);
            }
            else if (this.MessageType == MessageType.Meta)
            {
                ret = new MetaMessage(this.MetaType, this.MetaMessage.GetBytes());
            }
            else if (this.MessageType == MessageType.SystemCommon)
            {
                ret = new SysCommonMessage((this.MidiMessage as SysCommonMessage).Message);
            }
            else if (this.MessageType == MessageType.SystemExclusive)
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

                if (Previous != null)
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
                        else
                            return 0;
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
