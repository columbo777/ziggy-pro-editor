using System;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Sanford.Multimedia.Midi
{
    public class MidiEventComparer : IComparer<MidiEvent>
    {
        public int Compare(MidiEvent x, MidiEvent y)
        {
            if (x == null && y == null)
                return 0;
            return x.CompareTo(y);
        }
    }
    
    public class MidiEvent : IComparable<MidiEvent>
    {
        public int AbsoluteTicks { get; internal set; }


        public int Data1 { get; internal set; }
        public int Data2 { get; internal set; }
        public ChannelCommand Command { get; internal set; }
        public int Channel { get; internal set; }

        public bool IsOn { get { return Command == ChannelCommand.NoteOn; } }
        public bool IsOff { get { return Command == ChannelCommand.NoteOff; } }

        public MessageType MessageType { get; internal set; }
        public MetaType MetaType { get; internal set; }

        public string Text { get; internal set; }

        public bool Deleted { get; internal set; }

        public override string ToString()
        {
            return Text;
        }

        public int MessageData { get; internal set; }

        byte[] byteData;
        SysRealtimeType realtimeType;

        public void SetChanMessageData(int data)
        {
            MessageData = data;
            Command = ChannelMessage.UnpackCommand(MessageData);
            Channel = ChannelMessage.UnpackMidiChannel(MessageData);
            Data1 = ChannelMessage.UnpackData1(MessageData);
            Data2 = ChannelMessage.UnpackData2(MessageData);
            Text = GetText();
        }

        public MidiEvent(int absoluteTicks, IMidiMessage message)
        {
            MessageType = message.MessageType;
            MessageData = Int32.MinValue;
            byteData = null;

            this.AbsoluteTicks = absoluteTicks;

            Deleted = false;

            Data2 = Data1 = Int32.MinValue;
            MetaType = Midi.MetaType.Unknown;
            Command = ChannelCommand.Invalid;

            if (MessageType == Midi.MessageType.Channel)
            {
                SetChanMessageData((message as ChannelMessage).Message);
            }
            else if (MessageType == Midi.MessageType.Meta)
            {
                var msg = (message as MetaMessage);
                this.byteData = msg.GetBytes();
                this.MetaType = msg.MetaType;
                Text = msg.Text ?? "";
            }
            else if (MessageType == Midi.MessageType.SystemRealtime)
            {
                this.realtimeType = (message as SysRealtimeMessage).SysRealtimeType;
                Text = GetText();
            }
            else if (MessageType == Midi.MessageType.SystemExclusive)
            {
                var sysEx = message as Midi.SysExMessage;

                this.MessageData = sysEx.Status;

                byteData = message.GetBytes();
                Text = GetText();
            }
            else
            {
                MessageData = (message as ShortMessage).Message;
                Data1 = ShortMessage.UnpackData1(MessageData);
                Data2 = ShortMessage.UnpackData2(MessageData);
                byteData = message.GetBytes();
                Text = GetText();
            }

        }

        string GetText()
        {
            var ret = new StringBuilder();
            if (MessageType == Midi.MessageType.Channel)
            {

                ret.Append(AbsoluteTicks.ToString());

                ret.Append("[");
                ret.Append(Channel);
                ret.Append("]");

                ret.Append("[");
                ret.Append(Data1);

                ret.Append(",");
                ret.Append(Data2);
                ret.Append("]");

                ret.Append(" ");
                ret.Append(IsOn ? "on" : "off");

            }
            else
            {
                ret.Append(AbsoluteTicks.ToString());
                ret.Append(" ");
                ret.Append(this.MessageType.ToString());
            }

            return ret.ToString();
        }

        public IMidiMessage Clone()
        {
            IMidiMessage ret = null;

            if (this.MessageType == MessageType.Channel)
            {
                ret = new ChannelMessage(MessageData);
            }
            else if (this.MessageType == MessageType.Meta)
            {
                ret = new MetaMessage(this.MetaType, byteData);
            }
            else if (this.MessageType == MessageType.SystemCommon)
            {
                ret = new SysCommonMessage(MessageData);
            }
            else if (this.MessageType == MessageType.SystemExclusive)
            {
                ret = new SysExMessage(byteData);
            }
            else if (this.MessageType == MessageType.SystemRealtime)
            {
                ret = SysRealtimeMessage.FromType(realtimeType);
                ;
            }
            return ret;
        }



        internal void SetAbsoluteTicks(int absoluteTicks)
        {
            this.AbsoluteTicks = absoluteTicks;
        }


        public int CompareTo(MidiEvent other)
        {
            if (AbsoluteTicks < other.AbsoluteTicks)
                return -1;
            if (AbsoluteTicks > other.AbsoluteTicks)
                return 1;

            if (MessageType == other.MessageType && MessageType == Midi.MessageType.Channel)
            {
                if (Command != other.Command)
                {
                    if (Command == ChannelCommand.NoteOff)
                        return -1;
                    if (Command == ChannelCommand.NoteOn)
                        return 1;
                }
            }
            return 0;
        }
    }
}
