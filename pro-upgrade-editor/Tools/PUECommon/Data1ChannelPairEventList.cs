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
    public class Data1ChannelPairEventList : List<MidiEvent>
    {
        public Data1ChannelPair Data1ChannelPair { get; internal set; }
        public Data1ChannelPairEventList(int data1, int channel)
        {
            this.Data1ChannelPair = new Data1ChannelPair(data1, channel);
        }
    }
}