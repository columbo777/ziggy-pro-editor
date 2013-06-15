using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    

    public class Data1ChannelPair : IComparable<Data1ChannelPair>, IEquatable<Data1ChannelPair>
    {
        public int Data1 { get; set; }
        public int Channel { get; set; }

        public static Data1ChannelPair NullValue { get { return new Data1ChannelPair(Int32.MinValue, Int32.MinValue); } }

        public Data1ChannelPair(int data1, int channel)
        {
            this.Data1 = data1;
            this.Channel = channel;
        }

        public override string ToString()
        {
            return "Data1: " + Data1.ToStringEx() + " Channel: " + Channel.ToStringEx();
        }

        public int CompareTo(Data1ChannelPair other)
        {
            if (Data1 < other.Data1)
                return -1;
            else if (Data1 > other.Data1)
                return 1;
            else if (Channel < other.Channel)
                return -1;
            else if (Channel > other.Channel)
                return 1;
            else
                return 0;
        }

        public override int GetHashCode()
        {
            return ((Data1 & 0x00FF) | ((Channel << 2) & 0xFF00));
        }

        public bool Equals(Data1ChannelPair other)
        {
            return CompareTo(other) == 0;
        }
    }
}