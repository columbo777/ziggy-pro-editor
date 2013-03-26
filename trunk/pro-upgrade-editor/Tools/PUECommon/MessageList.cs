using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class MessageList : IEnumerable<GuitarMessage>
    {
        List<GuitarMessage> list;
        TrackEditor owner;
        public MessageList(TrackEditor owner)
        {
            list = new List<GuitarMessage>();
            this.owner = owner;
        }

        public int Count { get { return list.Count; } }
        public virtual void Add(GuitarMessage msg)
        {
            list.Add(msg);
        }

        public virtual void Remove(GuitarMessage msg)
        {
            list.Remove(msg);
        }

        public virtual void Insert(int idx, GuitarMessage msg)
        {
            list.Insert(idx, msg);
        }

        public virtual IEnumerator<GuitarMessage> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}