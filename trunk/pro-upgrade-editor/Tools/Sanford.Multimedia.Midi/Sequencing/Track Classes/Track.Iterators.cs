using System;
using System.Collections.Generic;
using System.Threading;

namespace Sanford.Multimedia.Midi
{
    public sealed partial class Track
    {
        #region Iterators
        public IEnumerable<MidiEvent> AllIterator()
        {
            foreach (var ev in eventList)
            {
                yield return ev;
            }
            endOfTrackMidiEvent.SetAbsoluteTicks(Length);

            yield return endOfTrackMidiEvent;
        }
        public IEnumerable<MidiEvent> Iterator()
        {
            return eventList;
        }

        public IEnumerable<int> DispatcherIterator(MessageDispatcher dispatcher)
        {
            var enumerator = Iterator().GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current.AbsoluteTicks;

                dispatcher.Dispatch(enumerator.Current.Clone());
            }
        }

        public IEnumerable<int> TickIterator(int startPosition,
            ChannelChaser chaser, MessageDispatcher dispatcher)
        {
            var enumerator = Iterator().GetEnumerator();

            bool notFinished = enumerator.MoveNext();
            
            while (notFinished && enumerator.Current.AbsoluteTicks < startPosition)
            {
                var cur = enumerator.Current;

                if (cur.MessageType == MessageType.Channel)
                {
                    chaser.Process((ChannelMessage)cur.Clone());
                }
                else if (cur.MessageType == MessageType.Meta)
                {
                    dispatcher.Dispatch(cur.Clone());
                }

                notFinished = enumerator.MoveNext();
            }

            chaser.Chase();

            int ticks = startPosition;

            while (notFinished)
            {
                while (ticks < enumerator.Current.AbsoluteTicks)
                {
                    yield return ticks;

                    ticks++;
                }

                yield return ticks;

                while (notFinished && enumerator.Current.AbsoluteTicks == ticks)
                {
                    dispatcher.Dispatch(enumerator.Current.Clone());

                    notFinished = enumerator.MoveNext();
                }

                ticks++;
            }
        }

        #endregion
    }
}
