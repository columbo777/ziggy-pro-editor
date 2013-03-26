using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    
    
    public abstract class SpecializedMessageList<T> : IEnumerable<T> where T : GuitarMessage
    {
        public static implicit operator MessageList(SpecializedMessageList<T> list)
        {
            return list.messageList;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return messageList.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return messageList.GetEnumerator();
        }
        public abstract GuitarMessageType MessageType { get; }

        MessageList messageList;


        public SpecializedMessageList(TrackEditor owner)
        {
            messageList = new MessageList(owner);
        }


        public void Add(GuitarMessage item)
        {
            if (item != null)
            {
                item.IsDeleted = false;

                var idx = GetIndexFromTick(item.TickPair);
                if (idx == -1 || idx >= this.Count())
                {
                    messageList.Add(item);
                }
                else
                {
                    messageList.Insert(idx, item);
                }
                
            }
        }


        
        public override string ToString()
        {
            return this.GetType().DeclaringType.Name.Replace("Guitar", "") + " " + this.Count();
        }

        public virtual int GetIndexFromTick(TickPair itemTick)
        {
            if (!this.Any())
            {
                return 0;
            }
            else
            {
                if (itemTick.Down <= this.First().DownTick)
                {
                    return 0;
                }
                else if (itemTick.Down >= this.Last().DownTick)
                {
                    return -1;
                }
                else
                {
                    var idx=-1;
                    var lastMatch = messageList.Where((x, i) => (x.TickPair == itemTick) && (idx = i) == i).ToList();
                    if (lastMatch.Any())
                    {
                        return idx;
                    }
                }
            }
            return -1;
        }


    }
}
