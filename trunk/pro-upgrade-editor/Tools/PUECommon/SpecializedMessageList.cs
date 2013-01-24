using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{


    public interface ISpecializedList : IEnumerable
    {
        void Add(GuitarMessage mess);

        void Remove(GuitarMessage mess);
        IEnumerable<GuitarMessage> List { get; }
    }

    public class SpecializedMessageList<T> : ISpecializedList, IEnumerable<T> where T : GuitarMessage
    {
        protected TrackEditor owner;

        protected List<T> itemList;

        public override string ToString()
        {
            return this.GetType().DeclaringType.Name.Replace("Guitar", "") + "\n" +
                this.SelectMany(x => x.ToString() + "\n");
        }

        public SpecializedMessageList(TrackEditor owner)
        {
            this.owner = owner;
            itemList = new List<T>();
        }

        public virtual T ElementAt(int index)
        {
            if (index >= itemList.Count)
                index = itemList.Count - 1;
            return itemList[index];
        }


        public virtual void RemoveRange(IEnumerable<T> mess)
        {
            foreach (var gm in mess.ToList())
            {
                this.internalRemove(gm);
            }
        }

        public virtual void AddRange(IEnumerable<T> mess)
        {
            foreach (var gm in mess.ToList())
            {
                this.internalInsert(gm as T);
            }
        }

        public virtual IEnumerable<GuitarMessage> List
        {
            get { return itemList; }
        }

        SpecializedMessageList()
        {

        }

        public virtual int GetIndexFromTick(int itemTick)
        {
            if (!itemList.Any())
            {
                return 0;
            }
            else
            {
                if (itemTick <= this.First().DownTick)
                {
                    return 0;
                }
                else if (itemTick >= this.Last().DownTick)
                {
                    return -1;
                }
                else
                {
                    return itemList.FindLastIndex(x => x.DownTick < itemTick) + 1;
                }
            }
        }

        public virtual int GetIndexFromTime(double time)
        {
            if (!itemList.Any())
            {
                return 0;
            }
            else
            {
                if (time <= this.First().StartTime)
                {
                    return 0;
                }
                else if (time >= this.Last().StartTime)
                {
                    return this.itemList.Count - 1;
                }
                else
                {
                    return itemList.FindLastIndex(x => x.StartTime < time) + 1;
                }
            }
        }

        protected virtual void internalInsert(T item)
        {
            if (item != null)
            {
                item.IsDeleted = false;

                var idx = GetIndexFromTick(item.AbsoluteTicks);
                if (idx == -1 || idx >= this.itemList.Count)
                {
                    itemList.Add(item);
                }
                else
                {
                    itemList.Insert(idx, item);
                }
            }
        }

        protected virtual void internalInsertRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                internalInsert(item);
            }
        }

        protected virtual void internalRemove(T item)
        {
            itemList.Remove(item);
        }

        public virtual void Clear()
        {
            itemList.Clear();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return itemList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return itemList.GetEnumerator();
        }

        public virtual void Add(GuitarMessage mess)
        {
            var item = mess as T;
            if (item != null)
            {
                internalInsert(item);
            }
        }

        public virtual void Remove(GuitarMessage mess)
        {
            var item = mess as T;
            if (item != null)
            {
                internalRemove(item);
            }
        }

    }
}
