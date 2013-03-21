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
            MidiEvent current = head;

            while (current != null)
            {
                if (current.MidiMessage != null)
                {
                    yield return current;
                }
                current = current.Next;
            }

            current = endOfTrackMidiEvent;

            yield return current;
        }
        public IEnumerable<MidiEvent> Iterator()
        {
            MidiEvent current = head;

            while (current != null)
            {
                if (current.MidiMessage != null)
                {
                    yield return current;
                }
                current = current.Next;
            }

        }

        public IEnumerable<int> DispatcherIterator(MessageDispatcher dispatcher)
        {
            IEnumerator<MidiEvent> enumerator = Iterator().GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current.AbsoluteTicks;

                dispatcher.Dispatch(enumerator.Current.MidiMessage);
            }
        }

        public IEnumerable<int> TickIterator(int startPosition,
            ChannelChaser chaser, MessageDispatcher dispatcher)
        {


            IEnumerator<MidiEvent> enumerator = Iterator().GetEnumerator();

            bool notFinished = enumerator.MoveNext();
            IMidiMessage message;

            while (notFinished && enumerator.Current.AbsoluteTicks < startPosition)
            {
                message = enumerator.Current.MidiMessage;

                if (message.MessageType == MessageType.Channel)
                {
                    chaser.Process((ChannelMessage)message);
                }
                else if (message.MessageType == MessageType.Meta)
                {
                    dispatcher.Dispatch(message);
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
                    dispatcher.Dispatch(enumerator.Current.MidiMessage);

                    notFinished = enumerator.MoveNext();
                }

                ticks++;
            }
        }

        #endregion
    }
}
