using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class Data1ChannelEventList : IEnumerable<MidiEventPair>
    {
        public Data1ChannelPair Data1ChannelPair { get; internal set; }
        public List<MidiEventPair> Events { get; internal set; }

        public int Data1 { get { return Data1ChannelPair.Data1; } }
        public int Channel { get { return Data1ChannelPair.Channel; } }

        public Data1ChannelEventList(Data1ChannelPair pair, IEnumerable<MidiEventPair> events = null)
        {
            this.Data1ChannelPair = pair;

            this.Events = new List<MidiEventPair>();

            if (events != null)
            {
                Events.AddRange(events);
            }
        }
        public override string ToString()
        {
            return Data1ChannelPair.ToString() + " Events: " + Events.Count();
        }
        public IEnumerator<MidiEventPair> GetEnumerator()
        {
            return Events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}