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
        List<T> messages { get; set; }

        public virtual TrackEditor Owner { get; internal set; }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public abstract GuitarMessageType MessageType { get; }


        public SpecializedMessageList(TrackEditor owner)
        {
            messages = new List<T>();
            this.Owner = owner;
        }

        public void Add(GuitarMessage item)
        {
            if (item != null)
            {
                var TItem = (T)item;

                TItem.IsDeleted = false;
                if (!messages.Contains(item))
                {

                    var items = messages.Where(x => x.AbsoluteTicks == TItem.AbsoluteTicks);
                    if (items.Any())
                    {
                        if (TItem.Command == ChannelCommand.NoteOff)
                        {
                            messages.Insert(messages.IndexOf(items.First()), TItem);
                        }
                        else
                        {
                            messages.Insert(messages.IndexOf(items.Last()) + 1, TItem);
                        }
                    }
                    else
                    {
                        var gt = messages.FirstOrDefault(x => x.AbsoluteTicks > TItem.AbsoluteTicks);
                        if (gt == null)
                        {
                            messages.Add(TItem);
                        }
                        else
                        {
                            messages.Insert(messages.IndexOf(gt), TItem);
                        }
                    }
                }
            }
        }


        public virtual void Remove(GuitarMessage mess)
        {
            if (mess != null)
            {
                var TMess = mess as T;
                if (TMess != null && messages.Contains(TMess))
                {
                    messages.Remove(TMess);
                    TMess.IsDeleted = true;
                }

            }
        }


        public override string ToString()
        {
            return this.GetType().DeclaringType.Name.Replace("Guitar", "") + " " + this.Count();
        }



    }
}
