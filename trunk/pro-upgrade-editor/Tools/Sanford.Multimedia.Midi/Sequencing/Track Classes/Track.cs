using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sanford.Multimedia.Midi
{

    public sealed partial class Track
    {

        string name;

        MidiEvent getNameEvent()
        {
            if (Count > 0)
            {
                return Meta.FirstOrDefault(x => x.MetaMessage.MetaType == MetaType.TrackName);
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

        public void ReplaceEvent(MidiEvent evOld, MidiEvent evNew)
        {
            if (evOld.Previous != null)
            {
                evNew.Previous = evOld.Previous;
                evOld.Previous.Next = evNew;
            }
            else
            {
                evNew.Previous = null;
                head = evNew;
            }

            if (evOld.Next != null)
            {
                evNew.Next = evOld.Next;
                evNew.Next.Previous = evNew;
            }
            else
            {
                evNew.Next = null;
                tail = evNew;

                
            }

            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            endOfTrackMidiEvent.Previous = tail;

            evOld.Next = evOld.Previous = null;

        }


        // The number of MidiEvents in the Track. Will always be at least 1
        // because the Track will always have an end of track message.
        private int count = 1;

        // The number of ticks to offset the end of track message.
        private int endOfTrackOffset = 0;

        // The first MidiEvent in the Track.
        private MidiEvent head = null;

        // The last MidiEvent in the Track, not including the end of track
        // message.
        private MidiEvent tail = null;

        // The end of track MIDI event.
        private MidiEvent endOfTrackMidiEvent;

        //

        // Construction

        public Track(FileType fileType, string name=null)
        {
            this.FileType = fileType;
            endOfTrackMidiEvent = new MidiEvent(this, Length, MetaMessage.EndOfTrackMessage);
            this.Name = name;
            
        }

        public override string ToString()
        {
            return Name;
        }

        Sequence sequence;
        public Sequence Sequence
        {
            get { return sequence; }
            internal set { sequence = value; }
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
                return Events.Where(x => x.MessageType == MessageType.Meta).ToList();
            }
        }

        
        public IEnumerable<MidiEvent> TimeSig
        {
            get
            {
                return Meta.Where(x => x.MetaMessage.MetaType == MetaType.TimeSignature).ToList();
            }
        }


        public IEnumerable<MidiEvent> Tempo
        {
            get
            {
                return Meta.Where(x => x.MetaMessage.MetaType == MetaType.Tempo).ToList();
            }
        }

        
        public IEnumerable<MidiEvent> ChanMessages
        {
            get
            {
                return Events.Where(x =>
                    x.MessageType == MessageType.Channel &&
                    (x.ChannelMessage.Command == ChannelCommand.NoteOn || x.ChannelMessage.Command == ChannelCommand.NoteOff)).ToList();
            }
        }



        
        public MidiEvent Insert(int position, IMidiMessage message)
        {
            var newMidiEvent = new MidiEvent(this, position, message);


            if (head == null)
            {
                head = newMidiEvent;
                tail = newMidiEvent;
            }
            else if (position >= tail.AbsoluteTicks)
            {
                newMidiEvent.Previous = tail;
                tail.Next = newMidiEvent;
                tail = newMidiEvent;
                
            }
            else
            {
                var current = head;

                while (current.AbsoluteTicks < position)
                {
                    current = current.Next;
                }

                newMidiEvent.Next = current;
                newMidiEvent.Previous = current.Previous;

                if (current.Previous != null)
                {
                    current.Previous.Next = newMidiEvent;
                }
                else
                {
                    head = newMidiEvent;
                }

                current.Previous = newMidiEvent;
            }
            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            endOfTrackMidiEvent.Previous = tail;
            count++;

            return newMidiEvent;
        }

        bool dirty = false;
        public bool Dirty
        {
            get
            {
                return this.dirty;
            }
            set
            {
                this.dirty = value;
            }
        }


        public void Clear()
        {
            head = tail = null;
            count = 1;
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
                return;
            
            this.dirty = true;

            ev.Deleted = true;
            
            

            if (ev.Previous != null)
            {
                ev.Previous.Next = ev.Next;
            }
            else
            {
                head = head.Next;
            }

            if (ev.Next != null)
            {
                ev.Next.Previous = ev.Previous;
            }
            else
            {
                tail = tail.Previous;

                endOfTrackMidiEvent.SetAbsoluteTicks(Length);
                endOfTrackMidiEvent.Previous = tail;
            }

            ev.Next = ev.Previous = null;
            count--;
        }
     

        public void Move(MidiEvent e, int newPosition)
        {
            if (e == null)
                return;

            MidiEvent previous = e.Previous;
            MidiEvent next = e.Next;

            if(e.Previous != null && e.Previous.AbsoluteTicks > newPosition)
            {
                e.Previous.Next = e.Next;

                if(e.Next != null)
                {
                    e.Next.Previous = e.Previous;
                }

                while(previous != null && previous.AbsoluteTicks > newPosition)
                {
                    next = previous;
                    previous = previous.Previous;
                }                
            }
            else if(e.Next != null && e.Next.AbsoluteTicks < newPosition)
            {
                e.Next.Previous = e.Previous;

                if(e.Previous != null)
                {
                    e.Previous.Next = e.Next;
                }

                while(next != null && next.AbsoluteTicks < newPosition)
                {
                    previous = next;
                    next = next.Next;
                }
            }

            if(previous != null)
            {
                previous.Next = e;
            }

            if(next != null)
            {
                next.Previous = e;
            }

            e.Previous = previous;
            e.Next = next;
            e.SetAbsoluteTicks(newPosition);

            if(newPosition < head.AbsoluteTicks)
            {
                head = e;
            }

            if(newPosition > tail.AbsoluteTicks)
            {
                tail = e;                
            }

            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            endOfTrackMidiEvent.Previous = tail;
        }

        public int Count
        {
            get
            {
                return count;
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

                if (tail != null)
                {
                    length += tail.AbsoluteTicks;
                }

                return length + 1;
            }
        }

        public FileType FileType
        {
            get;
            set;
        }

        public void Merge(IEnumerable<MidiEvent> trk)
        {
            var trkCount = trk.Count();

            if (trk == this.Events)
            {
                return;
            }
            else if (trkCount==1)
            {
                return;
            }

            count += trkCount;

            MidiEvent a = head;
            
            MidiEvent current = null;

            var v = trk.ToArray();
            int i = 0;

            MidiEvent b = v[i];
            if (a != null && a.AbsoluteTicks <= b.AbsoluteTicks)
            {
                current = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                a = a.Next;
            }
            else
            {
                current = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                if (i < v.Length - 1)
                {
                    b = v[++i];
                }
                else b = null;
            }

            head = current;

            while (a != null && b != null)
            {
                while (a != null && a.AbsoluteTicks <= b.AbsoluteTicks)
                {
                    current.Next = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                    current.Next.Previous = current;
                    current = current.Next;
                    a = a.Next;
                }

                if (a != null)
                {
                    while (b != null && b.AbsoluteTicks <= a.AbsoluteTicks)
                    {
                        current.Next = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                        current.Next.Previous = current;
                        current = current.Next;
                        if (i < v.Length - 1)
                        {
                            b = v[++i];
                        }
                        else b = null;
                    }
                }
            }

            while (a != null)
            {
                current.Next = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                current.Next.Previous = current;
                current = current.Next;
                a = a.Next;
            }

            while (b != null)
            {
                current.Next = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                current.Next.Previous = current;
                current = current.Next;
                if (i < v.Length - 1)
                {
                    b = v[++i];
                }
                else b = null;
            }

            tail = current;

            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            endOfTrackMidiEvent.Previous = tail;

            safeTrackName();
            
        }

        void safeTrackName()
        {
            var tn = Meta.Where(x => x.MetaMessage.MetaType == MetaType.TrackName).ToArray();
            if (tn.Length > 1)
            {
                for (int x = 1; x < tn.Length; x++)
                {
                    Remove(tn[x]);
                }
            }
        }
        public void Merge(Track trk)
        {
            if (trk == this)
            {
                return;
            }
            else if (trk.Count == 1)
            {
                return;
            }

            count += trk.Count - 1;

            MidiEvent a = head;
            MidiEvent b = trk.head;
            MidiEvent current = null;


            if (a != null && a.AbsoluteTicks <= b.AbsoluteTicks)
            {
                current = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                a = a.Next;
            }
            else
            {
                current = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                b = b.Next;
            }

            head = current;

            while (a != null && b != null)
            {
                while (a != null && a.AbsoluteTicks <= b.AbsoluteTicks)
                {
                    current.Next = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                    current.Next.Previous = current;
                    current = current.Next;
                    a = a.Next;
                }

                if (a != null)
                {
                    while (b != null && b.AbsoluteTicks <= a.AbsoluteTicks)
                    {
                        current.Next = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                        current.Next.Previous = current;
                        current = current.Next;
                        b = b.Next;
                    }
                }
            }

            while (a != null)
            {
                current.Next = new MidiEvent(this, a.AbsoluteTicks, a.MidiMessage);
                current.Next.Previous = current;
                current = current.Next;
                a = a.Next;
            }

            while (b != null)
            {
                current.Next = new MidiEvent(this, b.AbsoluteTicks, b.MidiMessage);
                current.Next.Previous = current;
                current = current.Next;
                b = b.Next;
            }

            tail = current;

            endOfTrackMidiEvent.SetAbsoluteTicks(Length);
            endOfTrackMidiEvent.Previous = tail;

            safeTrackName();
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

    }

}
