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
    public class Data1ChannelPairEqualityComparer : IEqualityComparer<Data1ChannelPair>
    {
        public bool Equals(Data1ChannelPair x, Data1ChannelPair y)
        {
            return x.Data1 == y.Data1 && x.Channel == y.Channel;
        }

        public int GetHashCode(Data1ChannelPair obj)
        {
            return ((obj.Data1 & 0x00FF) | ((obj.Channel << 2) & 0xFF00));
        }
    }

    
}