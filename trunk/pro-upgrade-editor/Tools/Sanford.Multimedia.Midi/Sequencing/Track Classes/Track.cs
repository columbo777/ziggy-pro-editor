using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanford.Multimedia.Midi
{

    public sealed partial class Track : IEnumerable<MidiEvent>
    {
        List<MidiEvent> eventList;
        string name;

        public Track(FileType fileType, string name = null)
        {
            eventList = new List<MidiEvent>();

            this.FileType = fileType;
            endOfTrackMidiEvent = new MidiEvent(this, Length, MetaMessage.EndOfTrackMessage);
            this.Name = name;
            Dirty = true;
        }

        MidiEvent getNameEvent()
        {
            if (Count > 0)
            {
                return Meta.FirstOrDefault(x => x.MetaType == MetaType.TrackName);
            }

            return null;
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    var tn = getNameEvent();
                    if (tn != null)
                    {
                        name = tn.ToString();
                        return name ?? "";
                    }
                }
                else { return name ?? ""; }
                return "(no name)";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    name = value ?? "";

                    var tn = getNameEvent();

                    if (tn != null)
                    {
                        Remove(tn);
                    }

                    var mb = new MetaTextBuilder(MetaType.TrackName, name);
                    mb.Build();

                    Insert(0, mb.Result);

                }
            }
        }


        // The number of ticks to offset the end of track message.
        private int endOfTrackOffset = 0;


        // The end of track MIDI event.
        private MidiEvent endOfTrackMidiEvent;

        public override string ToString()
        {
            return Name;
        }

        public Sequence Sequence
        {
            get;
            internal set;
        }

        public IEnumerable<MidiEvent> Events
        {
            get
            {
                return Iterator();
            }

        }


        public IEnumerable<MidiEvent> Meta
        {
            get
            {
                return Events.Where(x => x.MessageType == MessageType.Meta);
            }
        }


        public IEnumerable<MidiEvent> TimeSig
        {
            get
            {
                return Meta.Where(x => x.MetaType == MetaType.TimeSignature);
            }
        }


        public IEnumerable<MidiEvent> Tempo
        {
            get
            {
                return Meta.Where(x => x.MetaType == MetaType.Tempo);
            }
        }


        public IEnumerable<MidiEvent> ChanMessages
        {
            get
            {
                return Events.Where(x => x.MessageType == MessageType.Channel);
            }
        }

        void InsertAt(int index, MidiEvent message)
        {
            eventList.Insert(index, message);
        }
        void AddEvent(MidiEvent message)
        {
            if (message.MessageType == MessageType.Meta && message.MetaType == MetaType.TrackName)
            {
                eventList.Insert(0, message);
            }
            else
            {
                eventList.Add(message);
            }
        }

        public MidiEvent Insert(int position, IMidiMessage message)
        {
            
            var newMidiEvent = new MidiEvent(this, position, message);
            if (newMidiEvent.Command == ChannelCommand.NoteOn && newMidiEvent.Data2 == 0)
            {
                newMidiEvent.SetChanMessageData(ChannelMessage.PackCommand(newMidiEvent.MessageData, ChannelCommand.NoteOff));
            }

            var isAtEnd = position >= Length - 1;
            var isChanMessage = newMidiEvent.MessageType == MessageType.Channel;

            if (isAtEnd)
            {
                AddEvent(newMidiEvent);
            }
            else
            {
                var after = eventList.SkipWhile(x => x.AbsoluteTicks < position);

                if (!after.Any())
                {
                    AddEvent(newMidiEvent);
                }
                else
                {
                    var eq = after.TakeWhile(x => x.AbsoluteTicks == position).ToList();
                    if (!eq.Any())
                    {
                        InsertAt(eventList.IndexOf(after.First()), newMidiEvent);
                    }
                    else
                    {
                        if (message.MessageType == MessageType.Channel)
                        {
                            var eqChan = eq.Where(x => x.MessageType == MessageType.Channel);
                            if (!eqChan.Any())
                            {
                                InsertAt(eventList.IndexOf(eq.First()), newMidiEvent);
                            }
                            else
                            {
                                if (newMidiEvent.Command == ChannelCommand.NoteOn)
                                {
                                    InsertAt(eventList.IndexOf(eqChan.Last()) + 1, newMidiEvent);
                                }
                                else
                                {
                                    InsertAt(eventList.IndexOf(eqChan.First()), newMidiEvent);
                                }
                            }
                        }
                        else
                        {
                            InsertAt(eventList.IndexOf(eq.First()), newMidiEvent);
                        }
                    }
                }
            }
            
            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            Dirty = true;
            return newMidiEvent;
        }


        public bool Dirty
        {
            get;
            set;
        }


        public void Clear()
        {
            Dirty = true;
            eventList.Clear();
            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
           
        }

        public void Remove(IEnumerable<MidiEvent> ev)
        {
            
            foreach (var e in ev)
            {
                Remove(e);
            }
        }
        public void Remove(MidiEvent ev)
        {
            
            if (ev == null)
                return;
            if (ev == endOfTrackMidiEvent)
            {
                Debug.WriteLine("deleting end event");
                return;
            }

            if (ev.Deleted)
            {
                Debug.WriteLine("deleting already deleted event");
                return;
            }

            if (ev.Owner != this)
            {
                Debug.WriteLine("wrong track for event");
                return;
            }

            Dirty = true;

            ev.Deleted = true;
            eventList.Remove(ev);
            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            
        }



        public int Count
        {
            get
            {
                return eventList.Count;
            }
        }

        /// <summary>
        /// Gets the length of the Track in ticks.
        /// </summary>
        public int Length
        {
            get
            {
                int length = EndOfTrackOffset;

                if (eventList.Any())
                {
                    length += eventList.Last().AbsoluteTicks;
                }

                return length + 1;
            }
        }

        public FileType FileType
        {
            get;
            internal set;
        }

        public void Merge(Track trk)
        {
            trk.ChanMessages.ToList().ForEach(x => this.Insert(x.AbsoluteTicks, x.Clone()));
            trk.Meta.Where(x=> x.MetaType != MetaType.TrackName && x.MetaType != MetaType.EndOfTrack).
                ToList().ForEach(x => this.Insert(x.AbsoluteTicks, x.Clone()));
            
        }
        /// <summary>
        /// Gets or sets the end of track meta message position offset.
        /// </summary>
        public int EndOfTrackOffset
        {
            get
            {
                return endOfTrackOffset;
            }
            set
            {
                endOfTrackOffset = value;
                endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            }
        }


        public IEnumerator<MidiEvent> GetEnumerator()
        {
            return Iterator().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
