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
        void AddGuitarMessageRange(IEnumerable<GuitarMessage> mess);
        void Remove(GuitarMessage mess);
        IEnumerable<GuitarMessage> List { get; }
    }

    public class SpecializedMessageList<T> : ISpecializedList, IEnumerable<T> where T : GuitarMessage
    {
        protected TrackEditor owner;

        List<T> itemList;

        public virtual TickPair TickRange { get; internal set; }

        public override string ToString()
        {
            return this.GetType().DeclaringType.Name.Replace("Guitar", "") + "\n" +
                this.SelectMany(x => x.ToString() + "\n");
        }

        public SpecializedMessageList(TrackEditor owner)
        {
            this.owner = owner;
            itemList = new List<T>();
            TickRange = new TickPair();
        }

        public virtual T ElementAt(int index)
        {
            if (index >= itemList.Count)
                index = itemList.Count - 1;
            return itemList[index];
        }


        public virtual void RemoveRange(IEnumerable<T> mess)
        {
            foreach (var gm in mess)
            {
                this.internalRemove(gm);
            }
            internalUpdateTickRange();
        }

        public virtual void AddGuitarMessageRange(IEnumerable<GuitarMessage> mess)
        {
            foreach (var gm in mess)
            {
                this.internalInsert(gm as T);
            }
            internalUpdateTickRange();
        }

        public virtual void AddRange(IEnumerable<T> mess)
        {
            foreach (var gm in mess)
            {
                this.internalInsert(gm as T);
            }
            internalUpdateTickRange();
        }

        public virtual IEnumerable<GuitarMessage> List
        {
            get { return itemList; }
        }

        public virtual int GetIndexFromTick(int itemTick)
        {
            int ret = 0;

            var itemCount = itemList.Count;

            if (itemCount == 0)
            {
                return 0;
            }
            else if (itemTick >= TickRange.Up)
            {
                return itemCount;
            }
            else if (itemTick <= TickRange.Down)
            {
                return 0;
            }
            else
            {
                if (itemCount < 20)
                {
                    for (int x = 0; x < itemCount; x++)
                    {
                        if (itemList[x].AbsoluteTicks >= itemTick)
                        {
                            break;
                        }
                        else
                        {
                            ret = x;
                        }
                    }
                }
                else
                {
                    int rangeMin = 0;
                    int rangeMax = itemCount - 1;
                    int rangeCount = rangeMax - rangeMin;
                    int rangeMid = rangeMin + rangeCount / 2;

                    var low = itemList.ElementAt(rangeMin).AbsoluteTicks;
                    var mid = itemList.ElementAt(rangeMid).AbsoluteTicks;
                    var high = itemList.ElementAt(rangeMax).AbsoluteTicks;

                    while (true)
                    {
                        if (itemTick.IsBetween(low, mid))
                        {
                            rangeMax = rangeMid;
                            high = mid;
                        }
                        else if (itemTick.IsBetween(mid, high))
                        {
                            rangeMin = rangeMid;
                            low = mid;
                        }
                        else if (itemTick < low)
                        {
                            return rangeMin;
                        }
                        else if (itemTick > high)
                        {
                            return rangeMax;
                        }
                        rangeCount = rangeMax - rangeMin;
                        rangeMid = rangeMin + rangeCount / 2;
                        mid = itemList.ElementAt(rangeMid).AbsoluteTicks;

                        if (rangeCount < 5)
                        {
                            ret = rangeMin;

                            for (int x = 0; x < rangeCount; x++)
                            {
                                if (itemList[rangeMin + x].AbsoluteTicks >= itemTick)
                                {
                                    break;
                                }
                                else
                                {
                                    ret = (rangeMin + x);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        public virtual int GetIndexFromTime(double itemTime)
        {
            T retItem = null;
            foreach (var item in itemList)
            {
                if (item.StartTime <= itemTime)
                    retItem = item;
                else
                    break;
            }
            return itemList.IndexOf(retItem);
        }
        protected virtual void internalInsert(T item)
        {
            if (item != null)
            {
                itemList.Insert(GetIndexFromTick(item.AbsoluteTicks), item);
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

        protected virtual void internalUpdateTickRange()
        {
            if (itemList.Any())
            {
                var f = itemList.First();
                var l = itemList.Last();

                TickRange = new TickPair(f.DownTick, l.UpTick.GetIfNull(l.AbsoluteTicks));
            }
            else
            {
                TickRange = new TickPair();
            }
        }


        public virtual void Clear()
        {
            itemList.Clear();
            internalUpdateTickRange();
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

                internalUpdateTickRange();
            }
        }

        public virtual void Remove(GuitarMessage mess)
        {
            var item = mess as T;
            if (item != null)
            {
                internalRemove(item);
                internalUpdateTickRange();
            }
        }

    }
}
