using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Diagnostics;

namespace ProUpgradeEditor.DataLayer
{



    public class GuitarTrack
    {
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Sequence Sequence
        {
            get { return sequence; }
            set 
            {
                dirtyItems |= DirtyItem.All;
                this.sequence = value;
            }
        }



        public GuitarTrack(Sequence sequence, Track t,
            bool isPro, GuitarDifficulty difficulty)
        {
            Messages = new GuitarMessageList();
           
            this.sequence = sequence;
            
            this.IsPro = isPro;
            
            if (isPro && t != null)
            {
                Loaded = this.Load6(t, difficulty);
            }
            else if(!isPro && t != null)
            {
                Loaded = this.Load5(t, difficulty);
            }
            if (t != null)
            {
                t.Dirty = false;
            }
            Messages.SortTicks();
            this.dirtyItems = DirtyItem.None;
        }

        public void Remove(IEnumerable<GuitarModifier> mess)
        {
            Remove(mess.Cast<GuitarMessage>());
        }

        public void Remove(IEnumerable<GuitarMessage> mess)
        {
            if (mess != null)
            {
                foreach (var m in mess)
                {
                    Remove(m);
                }
            }
        }

        public void Remove(GuitarMessage message)
        {
            if (message == null)
                return;

            if (!message.IsDeleted)
            {
                if (message is GuitarChord)
                {
                    (message as GuitarChord).RemoveSubMessages();
                }
                if (message.DownEvent != null)
                {
                    Remove(message.DownEvent);
                }
                if (message.UpEvent != null)
                {
                    Remove(message.UpEvent);
                }

                if (chanEvents.Contains(message))
                    chanEvents.Remove(message);

                if (eventsAll.Contains(message))
                {
                    eventsAll.Remove(message);
                }
                
                if (Messages.Contains(message))
                {
                    Messages.Remove(message);
                }

                message.IsDeleted = true;
            }
            else
            {
                Debug.WriteLine("Removing Deleted message");
            }
            
        }


        public void Remove( MidiEvent ev)
        {
            if (ev != null && !ev.Deleted)
            {
                gtrack.Remove(ev);
            }
        }


        public GuitarChord GetChord(
            int[] frets,
            int[] fretChannels,
            GuitarDifficulty difficulty,
            int downTick, int upTick,
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode)
        {
            var ret = new GuitarChord(this);
            var lowE = Utility.GetStringLowE(difficulty);

            for (int x = 0; x < frets.Length; x++)
            {
                var fret = frets[x];
                var channel = fretChannels[x];

                if (!fret.IsNull())
                {
                    ret.Notes[x] = GuitarNote.GetNote(this, difficulty, downTick, upTick, x, fret,
                        channel == Utility.ChannelTap, channel == Utility.ChannelArpeggio, channel == Utility.ChannelX);
                }
            }

            
            ret.IsSlide = isSlide;
            ret.IsSlideReversed = isSlideReverse;
            ret.IsHammeron = isHammeron;
            ret.StrumMode = strumMode;

            return ret;
        }

        public GuitarChord CreateChord(
            int[] frets,
            int[] fretChannels, 
            GuitarDifficulty difficulty,
            int downTick, int upTick, 
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode)
        {
            var ret = new GuitarChord(this);
            var lowE = Utility.GetStringLowE(difficulty);

            Messages.Chords.GetChordsAtTick(downTick, upTick).ToList().ForEach(x => Remove(x));

            for (int x = 0; x < frets.Length; x++)
            {
                var fret = frets[x];
                var channel = fretChannels[x];

                if (!fret.IsNull())
                {
                    ret.Notes[x] = GuitarNote.CreateNote(this, difficulty, downTick, upTick, x, fret, 
                        channel==Utility.ChannelTap, channel == Utility.ChannelArpeggio, channel == Utility.ChannelX);
                }
            }
            if (ret.HasNotes)
            {
                if (isSlide)
                {
                    ret.AddSlide(isSlideReverse);
                }
                if (isHammeron)
                {
                    ret.AddHammeron();
                }
                if (strumMode != ChordStrum.Normal)
                {
                    ret.AddStrum(strumMode);
                }

                Messages.Add(ret);
            }
            else
            {
                ret = null;
            }
            return ret;
        }

        public MidiEvent Insert(int position, IMidiMessage ev)
        {
            return gtrack.Insert(position, ev);
        }

        public Track GetTrack() { return this.gtrack; }

        public int TimeToTick(double time)
        {
            var t = Messages.Tempos.LastOrDefault(x => x.StartTime <= time);
            if (t == null)
            {
                if (time < Messages.Tempos.First().StartTime)
                {
                    t = Messages.Tempos.First();
                }
                else
                {
                    t = Messages.Tempos.Last();
                }
            }

            if (time < t.StartTime)
                time = t.StartTime;

            var over = time - t.StartTime;
            if(over > 0)
            {
                return t.DownTick + TimeToTickRel(over, t.Tempo);
            }
            else{
                return t.DownTick;
            }
        }

        
        public Track TempoTrack
        {
            get 
            {
                return FindTempoTrack();
            }
        }
        public Track FindTempoTrack()
        {
            if (sequence == null)
                return null;

            return sequence.FirstOrDefault(x => x.Tempo.Any() || x.TimeSig.Any());
        }

        public void RemoveTrack(Track t)
        {
            if (sequence == null || t == null)
                return;

            if (sequence.Contains(t))
            {
                this.sequence.Remove(t);
                this.dirtyItems |= DirtyItem.Track;
            }
        }

        public void AddTempoTrack(Track t)
        {
            if(t == null || t.Name == null)
                return;

            try
            {
                dirtyItems |= DirtyItem.Track;
                if (this.sequence.Tracks.Length > 0)
                {
                    var t0 = this.sequence.Tracks[0];

                    if (t0.ChanMessages.Any() == false &&
                        (t0.Tempo.Any() || t0.TimeSig.Any()))
                    {
                        this.sequence.Remove(t0);
                    }
                }
                this.sequence.AddTempo(t);
                
            }
            catch { }
        }

        
        void GetTempo()
        {
            if (TempoTrack == null || !TempoTrack.Tempo.Any())
            {
                HasInvalidTempo = true;

                int dummyTempo = Utility.DummyTempo;
                Messages.Add(new GuitarTempo(this, null));
               
            }
            else
            {
                foreach (var ev in TempoTrack.Tempo)
                {
                    var gt = new GuitarTempo(this, ev);
                    Messages.Add(gt);
                }
               
            }
        }

        void BuildTempo()
        {
            if (Messages.Tempos == null)
                HasInvalidTempo = true;

            if (Messages.Tempos != null)
            {
                
                double currentTime = 0;
                int nextTick = 0;
                for (int x = 0; x < Messages.Tempos.Count(); x++)
                {
                    var bp = Messages.Tempos[x];

                    bp.SetStartTime(currentTime);

                    nextTick = Messages.Tempos.Count() > x + 1 ? Messages.Tempos[x + 1].DownTick : gtrack.Length;

                    currentTime += ((nextTick - bp.DownTick)  * SecondsPerTick(bp.Tempo));
                    bp.SetUpEvent(null, nextTick);
                    bp.SetEndTime(currentTime);
                }
            }
        }



        public GuitarTimeSignature TimeSignatureFromTicks(int tick)
        {
            return Messages.TimeSignatures.LastOrDefault(x => x.DownTick <= tick);
        }


        void GetTimeSignature()
        {
            var tempoTrack = TempoTrack;

            if (tempoTrack == null || tempoTrack.TimeSig.Any()==false)
            {
                HasInvalidTempo = true;

                Messages.Add(new GuitarTimeSignature(this, null));
               
            }
            else
            {
            
                var ret = new List<GuitarTimeSignature>();
                HasInvalidTempo = false;
            
                foreach(var ev in tempoTrack.TimeSig)
                {
                    Messages.Add(new GuitarTimeSignature(this, ev));
                }
            
            }
        }


        public double TotalSongTime
        {
            get
            {
                return TickToTime(gtrack.Length);
            }
        }

        public double SecondsPerBeat(int tick, double noteLength)
        {
            var ts = TimeSignatureFromTicks(tick);
            var to = GetTempo(tick);

            double microsecondsPerQuarterNote = (double)to.Tempo;

            var secondsPerQuarterNote = microsecondsPerQuarterNote / 1000000.0;
            
            return (secondsPerQuarterNote / ((ts.Denominator / 4.0))) * noteLength;
        }

        public int TempoFromTick(int tick)
        {
            return GetTempo(tick).Tempo;
        }

        public double SecondsPerBeatQuarterNote(int tick)
        {
            var ts = TimeSignatureFromTicks(tick);
            var secondsPerQuarterNote = SecondsPerQuarterNote(TempoFromTick(tick));

            return  secondsPerQuarterNote / ( ts.Denominator / 4.0 );
        }
        
        public double SecondsPerBar(int tick)
        {
            var ts = TimeSignatureFromTicks(tick);
            var secondsPerQuarterNote = SecondsPerQuarterNote(TempoFromTick(tick));
            
            return ts.Numerator * secondsPerQuarterNote;
        }

       

        public double TickToTime(int absTick)
        {
            GuitarTempo tempo = null;
            if (absTick < 0)
            {
                tempo = Messages.Tempos.FirstOrDefault();
            }
            else
            {
                tempo = Messages.Tempos.LastOrDefault(x => x.DownTick <= absTick);
            }
            var ticks = absTick - tempo.DownTick;

            return tempo.StartTime + ticks* SecondsPerTick(tempo.Tempo);
        }

        public double TickToTimeRel(int ticks, int tempo)
        {
            return ((double)ticks) * SecondsPerTick(tempo);
        }

        public int TimeToTickRel(double time, int tempo)
        {
            return (int)(time / SecondsPerTick(tempo));
        }

        public double SecondsPerTick(int tempo)
        {
            double ticksPerQuarterNote = TicksPerQuarterNote();
            var secondsPerQuarterNote = SecondsPerQuarterNote(tempo);

            var secondsPerTick = secondsPerQuarterNote / ticksPerQuarterNote;
            return secondsPerTick;
        }

        public double SecondsPerQuarterNote(int tempo)
        {
            return (double)tempo / 1000000.0;
            
        }

        public double TicksPerQuarterNote()
        {
            return (double)sequence.Division;
        }

        public double QuarterNotesPerSecond(int tempo)
        {
            return TicksPerQuarterNote() / SecondsPerTick(tempo);
        }

        public GuitarTempo GetTempo(int tick)
        {
            return Messages.Tempos.GetAtDownTick(tick);
        }

        public GuitarTimeSignature GetTimeSignature(int tick)
        {
            return TimeSignatureFromTicks(tick);
        }


        [Flags()]
        enum DirtyItem
        {
            None = 0,
            Track = 1 << 0,
            Message = 1 << 1,
            Difficulty = 1 << 2,
            
            
            All = Track | Difficulty | Message,
        }

        DirtyItem dirtyItems = DirtyItem.All;
        public bool Dirty
        {
            get
            {
                return dirtyItems != DirtyItem.None;
            }
        }

        bool Load5(Track guitarTrack, GuitarDifficulty difficulty)
        {
            bool ret = true;

            

            if (guitarTrack != null && 
                gtrack != null &&
                gtrack != guitarTrack)
            {
                Debug.WriteLine("Load5 - Track Dirty");
                this.dirtyItems |= DirtyItem.Track;
                gtrack = guitarTrack;
                _name = guitarTrack.Name;
            }
            else if (guitarTrack != null && gtrack != null && guitarTrack == gtrack)
            {
                if (gtrack.Dirty)
                {
                    Debug.WriteLine("Load5 - Dirtying Messages");
                    this.dirtyItems |= DirtyItem.Message;
                    gtrack = guitarTrack;
                    _name = guitarTrack.Name;
                }
            }
            else
            {
                this.gtrack = guitarTrack;
                this.dirtyItems |= DirtyItem.Track;
                if (this.gtrack != null)
                {
                    this._name = this.gtrack.Name;
                }
                else
                {
                    this._name = "";
                }
            }


            if (this.CurrentDifficulty != difficulty)
            {
                Debug.WriteLine("Load5 - Difficulty Changed");
                this.CurrentDifficulty = difficulty;
                this.dirtyItems |= DirtyItem.Difficulty;
            }

            if (Dirty)
            {
                if ((dirtyItems & DirtyItem.Track) == DirtyItem.Track &&
                    guitarTrack != null)
                {
                    this._name = guitarTrack.Name;
                }
                else
                {
                    this._name = "";
                }

                ret = LoadData5();

                if (gtrack != null)
                {
                    gtrack.Dirty = false;
                }
                this.dirtyItems = DirtyItem.None;
                
            }
            return ret;
        }

        bool LoadData5()
        {
            bool ret = false;
            try
            {
                HasInvalidTempo = false;

                GetMidiEvents(true);

                if (gtrack == null)
                    return false;
                
                gtrack.Dirty = false;
                
                BuildMessages5();
                dirtyItems = DirtyItem.None;
                ret = true;
            }
            catch { ret = false; }
            
            return ret;
        }

        void BuildMessages5()
        {
            bool trackDirty = ((this.dirtyItems & DirtyItem.Track) == DirtyItem.Track);
            bool messagesDirty = ((this.dirtyItems & DirtyItem.Message) == DirtyItem.Message);
            bool difficultyDirty = ((this.dirtyItems & DirtyItem.Difficulty) == DirtyItem.Difficulty);
            bool refreshing = !trackDirty && messagesDirty;

            if (!trackDirty && !messagesDirty && !difficultyDirty)
                return;

            if (!refreshing)
            {
                Messages = new GuitarMessageList();
                CompileTempo5();
            }

            var downNotes = new NoteDownEventManager(false);

            var notes = new List<GuitarNote>();
            foreach (var ev_atTick in ChanEvents.GroupBy(x => x.AbsoluteTicks))
            {
                foreach (var ev in ev_atTick.Where(x=> x.Command == ChannelCommand.NoteOff))
                {
                    
                    var downNote = downNotes.SetNoteUp(ev.AbsoluteTicks, GetNoteFromMessage(ev, false), ev);
                    if (downNote != null)
                    {
                        ev.SetUpEvent(ev.MidiEvent);
                        downNote.SetUpEvent(ev.MidiEvent);
                    }
                    else
                    {
                        Debug.WriteLine("BuildMessages5 No note on 5 - " + ev.ToString());
                    }

                }
                foreach (var ev in ev_atTick.Where(x=>x.Command == ChannelCommand.NoteOn))
                {

                    var note = GetNoteFromMessage(ev, false);
                    if (downNotes.SetNoteDown(note))
                    {
                        notes.Add(note);
                    }
                    else
                    {
                        Debug.WriteLine("BuildMessages5 Unable to SetNoteDown - " + note.ToString());
                    }
                    
                } 
            }

            
            GuitarChord lastChord = null;
            foreach(var note in notes.Where(x=> x.NoteString != -1))
            {
                var c = ProcessG5NoteToChord(lastChord, note);
                if (c == null)
                {
                    if (note.DownTick < lastChord.UpTick)
                    {
                        note.DownTick = lastChord.UpTick;
                        c = ProcessG5NoteToChord(lastChord, note);
                    }
                }
                else
                {
                    lastChord = c;
                }
            }

            LoadModifiers5();
        }

        private GuitarChord ProcessG5NoteToChord(GuitarChord lastChord, GuitarNote note)
        {
            if (lastChord == null ||
                note.DownTick >= lastChord.UpTick)
            {
                if (lastChord != null &&
                    note.DownTick < lastChord.UpTick)
                {
                    int delta = lastChord.UpTick - note.DownTick;
                    note.DownTick = note.DownTick + delta;

                }
                var chord = new GuitarChord(this);

                chord.Notes[note.NoteString] = note;

                chord.UpEvent = note.UpEvent;
                chord.DownEvent = note.DownEvent;

                chord.UpTick = note.UpTick;
                chord.DownTick = note.DownTick;


                if ((chord.UpTick - chord.DownTick) >= 0)
                {
                    Messages.Add(chord);
                    lastChord = chord;
                }


            }
            else if (lastChord != null)
            {
                if (lastChord.Notes[note.NoteString] == null)
                {
                    lastChord.Notes[note.NoteString] = note;

                    lastChord.DownTick = lastChord.DownTick < note.DownTick ?
                        lastChord.DownTick : note.DownTick;
                    lastChord.UpTick = lastChord.UpTick > note.UpTick ?
                       lastChord.UpTick : note.UpTick;
                }
                else
                {
                    //Debug.WriteLine("hack");
                    return null;
                }
            }
            return lastChord;
        }

        private static GuitarNote GetDownNote5(List<GuitarNote> notes, GuitarMessage ev, int noteString, int evAT)
        {
            var n = notes.LastOrDefault(k => k.NoteString == noteString &&
                k.DownTick < evAT &&
                k.Data1 == ev.Data1 &&
                k.UpEvent == null);
            return n;
        }

        private void LoadModifiers5()
        {
            int currmod = -1;

            foreach(var ev in ChanEvents)
            {
                if(ev.Data1 == Utility.SoloData1 ||
                    ev.Data1 == Utility.SoloData1_G5)
                {
                    if (ev.Command == ChannelCommand.NoteOn)
                    {
                        Messages.Add(new GuitarSolo(this, ev.MidiEvent, null));
                    }
                    else
                    {
                        Messages.LastSolo.SetUpEvent(ev.MidiEvent);
                    }
                }
                else if (ev.Data1 == Utility.PowerupData1)
                {
                    if (ev.Command == ChannelCommand.NoteOn )
                    {
                        Messages.Add(new GuitarPowerup(this, ev.MidiEvent, null));
                    }
                    else
                    {
                        Messages.LastPowerup.SetUpEvent(ev.MidiEvent);
                    }
                }
                else if (Utility.BigRockEndingData1.Contains(ev.Data1) && ev.AbsoluteTicks > currmod)
                {
                    if (ev.Command == ChannelCommand.NoteOn)
                    {
                        Messages.Add(new GuitarBigRockEnding(this, ev.MidiEvent, null));
                    }
                    else
                    {
                        Messages.LastBigRockEnding.SetUpEvent(ev.MidiEvent);
                    }
                }
            }
            
        }

        private void CompileTempo5()
        {
            GetTimeSignature();

            GetTempo();

            BuildTempo();

            
        }



        bool Load6(Track guitarTrack, GuitarDifficulty difficulty)
        {
            bool ret = true;

            if (gtrack == null ||
                (gtrack != null && guitarTrack == null) ||
                (gtrack != null && guitarTrack != null && gtrack != guitarTrack))
            {
                this.gtrack = guitarTrack;
                this.dirtyItems |= DirtyItem.Track;
                if (guitarTrack != null)
                {
                    this._name = guitarTrack.Name;
                }
                else
                {
                    this._name = "";
                }
            }

            if (guitarTrack != null && !Dirty)
            {
                this.dirtyItems |= DirtyItem.Track;
            }

            if (this.CurrentDifficulty != difficulty)
            {
                this.CurrentDifficulty = difficulty;
                this.dirtyItems |= DirtyItem.Difficulty;
            }

            if (Dirty)
            {
                if ((dirtyItems & DirtyItem.Track) == DirtyItem.Track)
                {
                    this._name = guitarTrack.Name;
                }

                ret = LoadData6(true);

                this.dirtyItems = DirtyItem.None;
            }
            
            return ret;
        }

        bool LoadData6(bool includeDifficultyAll)
        {
            bool ret = false;
            try
            {
                if (dirtyItems.HasFlag(DirtyItem.Track) || dirtyItems.HasFlag(DirtyItem.Difficulty))
                {
                    HasInvalidTempo = false;
                }

                ret = GetMidiEvents(includeDifficultyAll);

                if (ret)
                {
                    ret = LoadEvents6();
                }
                
            }
            catch (Exception)
            {
                ret = false;

                Debug.WriteLine("failed..");

            }
            return ret;
        }

        Sequence sequence;
        GuitarMessageList internalMessages;
        Track gtrack;
        string _name;



        public GuitarMessageList Messages
        {
            get
            {
                return internalMessages;
            }
            set
            {
                internalMessages = value;
            }
        }

        List<GMessage> eventsAll = new List<GMessage>();
        List<GuitarMessage> chanEvents;
        public bool HasInvalidTempo = true;

        public bool IsPro = false;

        public GuitarDifficulty CurrentDifficulty { get; internal set; }

        public bool Loaded = false;

        List<MidiEvent> DirtyTrackChanMessages { get { return gtrack.ChanMessages.Where(x => x.Dirty).ToList(); } }
        List<GuitarMessage> DirtyMessages { get { return Messages.Where(x => x is GuitarMessage && x.IsDirty).Cast<GuitarMessage>().ToList(); } }
        List<GuitarMessage> NewMessages { get { return Messages.Where(x => x is GuitarMessage && x.IsNew).Cast<GuitarMessage>().ToList(); } }
        List<GuitarMessage> UpdatedMessages { get { return Messages.Where(x => x is GuitarMessage && x.IsUpdated).Cast<GuitarMessage>().ToList(); } }
        List<GuitarMessage> DeletedMessages { get { return Messages.Where(x => x is GuitarMessage && x.IsDeleted).Cast<GuitarMessage>().ToList(); } }

        List<GuitarMessage> DeletedChanEventsAll { get { return ChanEventsAll.Where(x => x.IsDeleted).ToList(); } }
        List<GuitarMessage> UpdatedChanEventsAll { get { return ChanEventsAll.Where(x => x.IsUpdated).ToList(); } }
        List<GuitarMessage> NewChanEventsAll { get { return ChanEventsAll.Where(x => x.IsNew).ToList(); } }
        List<GuitarMessage> DirtyChanEventsAll { get { return ChanEventsAll.Where(x => x.IsDirty).ToList(); } }


        List<GMessage> DeletedEventsAll { get { return EventsAll.Where(x => x.IsDeleted).ToList(); } }
        List<GMessage> UpdatedEventsAll { get { return EventsAll.Where(x => x.IsUpdated).ToList(); } }
        List<GMessage> NewEventsAll { get { return EventsAll.Where(x => x.IsNew).ToList(); } }
        List<GMessage> DirtyEventsAll { get { return EventsAll.Where(x => x.IsDirty).ToList(); } }


        List<GMetaMessage> DeletedMetaEventsAll { get { return MetaEventsAll.Where(x => x.IsDeleted).ToList(); } }
        List<GMetaMessage> UpdatedMetaEventsAll { get { return MetaEventsAll.Where(x => x.IsUpdated).ToList(); } }
        List<GMetaMessage> NewMetaEventsAll { get { return MetaEventsAll.Where(x => x.IsNew).ToList(); } }
        List<GMetaMessage> DirtyMetaEventsAll { get { return MetaEventsAll.Where(x => x.IsDirty).ToList(); } }

        

        public IEnumerable<GMessage> EventsAll
        {
            get { return eventsAll; }
        }

        public IEnumerable<GuitarMessage> ChanEventsAll
        {
            get
            {
                return eventsAll.Where(x => x is GuitarMessage).Cast<GuitarMessage>();
            }
        }

        public IEnumerable<GMetaMessage> MetaEventsAll
        {
            get
            {
                return eventsAll.Where(x => x is GMetaMessage).Cast<GMetaMessage>();
            }
        }

        public IEnumerable<GMetaMessage> TextEvents
        {
            get
            {
                return MetaEventsAll.Where(x => x.MidiEvent.MetaMessage.MetaType == MetaType.Text);
            }
        }

        public List<MidiEvent> DirtyTrackEvents
        {
            get
            {
                return gtrack != null ? gtrack.Events.Where(x => x.Dirty).ToList() : new List<MidiEvent>();
            }
        }


        public IEnumerable<GuitarMessage> ChanEvents
        {
            get
            {
                return chanEvents;
            }
        }
        bool LoadEvents6()
        {
            bool ret = true;
            try
            {
                
                
                
                bool trackDirty = this.dirtyItems.HasFlag(DirtyItem.Track);
                bool messagesDirty = this.dirtyItems.HasFlag(DirtyItem.Message);
                bool difficultyDirty = this.dirtyItems.HasFlag(DirtyItem.Difficulty);

                bool refreshing = !trackDirty && messagesDirty;
                if (difficultyDirty)
                    refreshing = false;

                if (refreshing)
                {
                   
                }
                else
                {
                    Messages = new GuitarMessageList();
                    CompileTempo6();

                    Messages.AddRange(ChanEvents.Where(x => x.Data1 == Utility.HandPositionData1));

                    LoadModifiers6(refreshing, ChanEvents);

                    var modifiers = LoadNoteModifiers6(ChanEvents);
                    ret = LoadNotes6(refreshing, ChanEvents, modifiers);

                }


            }
            catch { ret = false; }
            return ret;
        }

        public class NoteDownEventManager
        {
            bool isPro = false;
            public NoteDownEventManager(bool isPro)
            {
                this.isPro = isPro;
            }

            public class DownNote
            {
                public GuitarNote Note{ get; set; }
                public bool IsDown { get; set; }
            }

            List<DownNote> downEvents = new List<DownNote>();

            public bool IsNoteDown(GuitarNote note)
            {
                var n = GetNote(note);

                return n != null && n.IsDown == true;
            }

            DownNote GetNote(GuitarNote note)
            {
                return downEvents.SingleOrDefault(x =>
                    isPro ? (
                             x.Note.Data1 == note.Data1 &&
                             x.Note.NoteString == note.NoteString && 
                             x.Note.Channel == note.Channel) : 
                            (x.Note.Data1 == note.Data1 && x.Note.Channel == note.Channel));
            }

            public bool SetNoteDown(GuitarNote note)
            {
                bool ret = true;
                
                var dn = GetNote(note);
                if (dn == null)
                {
                    downEvents.Add(new DownNote() { IsDown = true, Note = note });
                }
                else
                {

                    if (dn.IsDown == true)
                    {
                        ret = false;
                    }
                    else
                    {
                        dn.Note = note;
                        dn.IsDown = true;
                    }
                }
                return ret;
            }

            public GuitarNote SetNoteUp(int absTick, GuitarNote note, GuitarMessage message)
            {
                
                var dn = GetNote(note);
                if (dn == null || dn.IsDown == false)
                {
                    //Debug.WriteLine("No note down -  New note: " + note.ToString());
                    return null;
                }
                else
                {
                    dn.Note.UpTick = absTick;
                    dn.Note.UpEvent = message.MidiEvent;
                    dn.IsDown = false;
                    return dn.Note;
                }
            
            }
        }

        

        public void AddTrainer(GuitarTrainer tr)
        {
            if (tr.TrainerIndex.IsNull())
            {
                tr.TrainerIndex = Messages.TextEvents.Count(x => x.Type == 
                    GuitarTrainerMetaEventType.BeginProGuitar || 
                    x.Type == GuitarTrainerMetaEventType.BeginProBass);

                var mb = new MetaTextBuilder();
                mb.Type = MetaType.Text;
                mb.Text = tr.StartText;
                mb.Build();
                Insert(tr.Start.AbsoluteTicks, mb.Result);

                if (tr.Loopable)
                {
                    mb.Text = tr.NormText;
                    mb.Build();
                    Insert(tr.Norm.AbsoluteTicks, mb.Result);
                }
                mb.Text = tr.EndText;
                mb.Build();
                Insert(tr.End.AbsoluteTicks, mb.Result);
            }
            
        }


        private bool LoadNotes6(bool refreshing, IEnumerable<GuitarMessage> events, IEnumerable<GuitarModifier> modifiers)
        {
            bool ret = true;

            var downNotes = new NoteDownEventManager(true);

            var currentChord = new GuitarChord(this);

            foreach (var ev_atTick in events.Where(x=>
                    x.IsDeleted==false && 
                    x.NoteString != -1 && 
                    x.MidiEvent != null &&
                    x.MidiEvent.ChannelMessage != null &&
                    x.Difficulty.HasFlag(CurrentDifficulty)).
                GroupBy(x => x.DownTick).ToList())
            {
                foreach (var ev in ev_atTick.Where(x=> 
                    x.IsDeleted==false &&
                    x.Command == ChannelCommand.NoteOff).ToList())
                {
                    var downNote = downNotes.SetNoteUp(ev.DownTick, GetNoteFromMessage(ev, true), ev);
                    if (downNote != null)
                    {
                        downNote.SetUpEvent(ev.DownEvent);
                    }
                    else
                    {
                       // Debug.WriteLine("No note on");
                        //Remove(ev);
                    }
                }

                foreach (var ev in ev_atTick.Where(x => 
                    x.IsDeleted==false &&
                    x.Command == ChannelCommand.NoteOn).ToList())
                {
                    if (ev.Data2 < 100 || ev.Data2 > 123)
                    {
                        
                       // ev.Data2 = 100;
                    }
                    var note = GetNoteFromMessage(ev, true);
                    note.SetDownEvent(ev.DownEvent);

                    if (downNotes.SetNoteDown(note))
                    {
                        if (currentChord.IsDownEventClose(note))
                        {
                            currentChord.Notes[note.NoteString] = note;
                        }
                        else
                        {
                            currentChord = new GuitarChord(this);
                            currentChord.Notes[note.NoteString] = note;
                            Messages.Add(currentChord);

                            var chordModifiers = modifiers.GetBetweenTick(currentChord.DownTick, currentChord.DownTick + Utility.NoteCloseWidth);

                            foreach (var mod in chordModifiers.ToList())
                            {
                                if (Utility.AllSlideData1.Contains(mod.Data1))
                                {
                                    currentChord.IsSlide = true;
                                    if (mod.Channel == Utility.ChannelSlideReversed)
                                        currentChord.IsSlideReversed = true;
                                }
                                else if (Utility.AllHammeronData1.Contains(mod.Data1))
                                {
                                    currentChord.IsHammeron = true;
                                }
                                else if (Utility.AllStrumData1.Contains(mod.Data1))
                                {
                                    currentChord.StrumMode = ChordStrum.Normal;

                                    var strumMode = ChordStrum.Normal;

                                    if (mod.Channel == Utility.ChannelStrumHigh)
                                    {
                                        strumMode |= ChordStrum.High;
                                    }
                                    else if (mod.Channel == Utility.ChannelStrumMid)
                                    {
                                        strumMode |= ChordStrum.Mid;
                                    }
                                    else if (mod.Channel == Utility.ChannelStrumLow)
                                    {
                                        strumMode |= ChordStrum.Low;
                                    }

                                    currentChord.StrumMode = strumMode;
                                }
                            }
                        }
                    }
                    else
                    {
                       
                        //Debug.WriteLine("No note off");
                    }
                }
            }
            return ret;
        }

        public GuitarNote GetNoteFromMessage(GuitarMessage ev, bool isPro)
        {
            return new GuitarNote(this, ev.DownEvent, ev.UpEvent);
        }



        private void LoadModifiers6(bool refreshing, IEnumerable<GuitarMessage> events)
        {
            foreach (var ev in events.Where(x =>
                Utility.AllArpeggioData1.Contains(x.Data1) ||
                x.Data1 == Utility.PowerupData1 ||
                x.Data1 == Utility.SoloData1 ||
                x.Data1 == Utility.MultiStringTremeloData1 ||
                x.Data1 == Utility.SingleStringTremeloData1 ||
                Utility.BigRockEndingData1.Contains(x.Data1)).ToList())
            {
                if (Utility.AllArpeggioData1.Contains(ev.Data1))
                {
                    LoadArpeggio6(refreshing, ev.MidiEvent);
                    continue;
                }
                else if (ev.Data1 == Utility.PowerupData1)
                {
                    LoadPowerup6(refreshing, ev.MidiEvent);
                    continue;
                }
                else if (ev.Data1 == Utility.SoloData1)
                {
                    LoadSolo6(refreshing, ev.MidiEvent);
                    continue;
                }
                else if (ev.Data1 == Utility.MultiStringTremeloData1)
                {
                    LoadMultiStringTremelo(refreshing, ev.MidiEvent);
                    continue;
                }
                else if (ev.Data1 == Utility.SingleStringTremeloData1)
                {
                    LoadSingleStringTremelo6(refreshing, ev.MidiEvent);
                    continue;
                }
                else if (Utility.BigRockEndingData1.Contains(ev.Data1))
                {
                    LoadBigRockEnding6(refreshing, ev.MidiEvent);
                    continue;
                }
            }
        }

        private IEnumerable<GuitarModifier> LoadNoteModifiers6(IEnumerable<GuitarMessage> events)
        {
            var ret = new List<GuitarModifier>();
            var slideData1 = Utility.GetSlideData1(CurrentDifficulty);
            var hammeronData1 = Utility.GetHammeronData1(CurrentDifficulty);
            var strumData1 = Utility.GetStrumData1(CurrentDifficulty);

            foreach (var ev in events.Where(x=>
                slideData1 == x.Data1 ||
                hammeronData1 == x.Data1 ||
                strumData1 == x.Data1))
            {
                if (ev.Command == ChannelCommand.NoteOn)
                {
                    ret.Add(new GuitarModifier(this, ev.MidiEvent, null, GuitarModifierType.NoteModifier));
                }
                else
                {
                    var lm = ret.Where(i =>
                        i.UpEvent == null &&
                        i.Data1 == ev.Data1 &&
                        i.Channel == ev.Channel &&
                        i.DownTick < ev.DownTick).LastOrDefault();

                    if (lm != null)
                    {
                        lm.SetUpEvent(ev.MidiEvent);
                    }
                }
            }
            return ret;
        }

        private void LoadBigRockEnding6(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarBigRockEnding(this, ev, null));
            }
            else
            {
                var v = Messages.BigRockEndings.LastOrDefault();
                if (v != null)
                {
                    v.SetUpEvent(ev);
                }
            }
        }

        public static string GuitarTrackName17
        {
            get
            {
                return "PART REAL_GUITAR";
            }
        }

        public static string GuitarTrackName22
        {
            get
            {
                return "PART REAL_GUITAR_22";
            }
        }
        public static string BassTrackName17
        {
            get
            {
                return "PART REAL_BASS";
            }
        }

        public static string BassTrackName22
        {
            get
            {
                return "PART REAL_BASS_22";
            }
        }
        public static string[] GuitarTrackNames6
        {
            get
            {
                return new string[] { GuitarTrackName17, GuitarTrackName22 };
            }
        }
        public static string[] TrackNames22
        {
            get
            {
                return new string[] { GuitarTrackName22, BassTrackName22 };
            }
        }
        public static string[] BassTrackNames6
        {
            get
            {
                return new string[] { BassTrackName17, BassTrackName22 };
            }
        }

        public static string[] TrackNames6
        {
            get { return new string[] { GuitarTrackName17, GuitarTrackName22, BassTrackName17, BassTrackName22 }; }
        }

        public static string GuitarTrackName5
        {
            get { return "PART GUITAR"; }
        }

        public static string BassTrackName5
        {
            get { return "PART BASS"; }
        }

        public static string[] TrackNames5
        {
            get { return new string[] { GuitarTrackName5, BassTrackName5 }; }
        }

        public static bool IsGuitarTrackName(string t)
        {
            return GuitarTrackName5.Contains(t) || GuitarTrackNames6.Contains(t);
        }
        public static bool IsBassTrackName(string t)
        {
            return BassTrackName5.Contains(t) || BassTrackNames6.Contains(t);
        }
        public static bool IsTrackName22(string t)
        {
            return TrackNames22.Contains(t);
        }
        private void LoadSingleStringTremelo6(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarSingleStringTremelo(this, ev, null));
            }
            else
            {
                var v = Messages.SingleStringTremelos.LastOrDefault();

                if (v != null)
                {
                    v.SetUpEvent(ev);
                }
            }
        }

        private void LoadMultiStringTremelo(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarMultiStringTremelo(this, ev, null));
            }
            else
            {
                var v = Messages.MultiStringTremelos.LastOrDefault();
                if (v != null)
                {
                    v.SetUpEvent(ev);
                }
            }
        }


        private void LoadSolo6(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarSolo(this, ev, null));
            }
            else
            {
                var v = Messages.Solos.LastOrDefault();
                if (v != null)
                {
                    v.SetUpEvent(ev);
                }
            }
        }


        private void LoadPowerup6(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarPowerup(this, ev, null));
            }
            else
            {
                if (Messages.Powerups.Any())
                {
                    Messages.LastPowerup.SetUpEvent(ev);
                }
            }
        }

        private void LoadArpeggio6(bool refreshing, MidiEvent ev)
        {
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn && ev.ChannelMessage.Data2 != 0)
            {
                Messages.Add(new GuitarArpeggio(this, ev, null));
            }
            else
            {
                if (Messages.Arpeggios.Any())
                {
                    var v = Messages.Arpeggios.LastOrDefault();
                    if (v != null)
                    {
                        v.SetUpEvent(ev);
                    }
                }
            }
        }

        private void CompileTempo6()
        {
            GetTimeSignature();

            GetTempo();

            BuildTempo();

            
        }
       

        public GuitarTrack SetTrack(Track t, GuitarDifficulty diff, bool includeAll)
        {
            Debug.WriteLine(t.Name + " " + diff);
            if (t == null || gtrack == null)
            {
                gtrack = null;
                _name = "";
                dirtyItems |= DirtyItem.Track;
            }
            else if(gtrack != t)
            {
                dirtyItems |= DirtyItem.Track;
                _name = t.Name;
            }
            else if(t.Dirty == true)
            {
                dirtyItems |= DirtyItem.Message;
            }
            if (this.CurrentDifficulty != diff)
            {
                dirtyItems |= DirtyItem.Difficulty;
            }


            if (sequence != null && sequence.Dirty) 
            {
                var tt = FindTempoTrack();
                if (tt != null)
                {
                    if (tt.Dirty)
                    {
                        tt.Dirty = false;
                        foreach (var tk in sequence)
                        {
                            if (tk != tt)
                            {
                                //tk.Dirty = true;
                            }
                        }
                        //dirtyItems |= DirtyItem.Track;
                    }
                }
            }

            if (Dirty || gtrack.ChanMessages.Any(x=> x.Dirty))
            {
                this.CurrentDifficulty = diff;
                gtrack = t;
                _name = (t!=null ? t.Name : "");
                if (IsPro)
                {
                    LoadData6(includeAll);
                }
                else
                {
                    LoadData5();
                }
            }
            if (gtrack != null)
            {
                gtrack.Dirty = false;
            }

            sequence.Dirty = false;

            
            DirtyMessages.ForEach(x => x.IsDirty = false);
            DirtyEventsAll.ForEach(x => x.IsDirty = false);
            DirtyTrackEvents.ForEach(x => x.Dirty = false);
            this.dirtyItems = DirtyItem.None;
            
            return this;
        }


        

        public GuitarTrainer GetTrainerByIndex(GuitarTrainerType type, int index)
        {
            return Messages.Trainers.Where(x=> x.TrainerType == type && x.TrainerIndex == index).FirstOrDefault();
        }

        void LoadTrainers()
        {
            try
            {
                var trainers = new List<GuitarTrainer>();
                foreach (var meta in TextEvents)
                {
                    var text = meta.Text;
                    
                    var eventType = text.GetGuitarTrainerMetaEventType();
                    if (eventType == GuitarTrainerMetaEventType.BeginProGuitar)
                    {
                        var tr = new GuitarTrainer(this, GuitarTrainerType.ProGuitar);
                        tr.SetStart(meta.DownEvent);
                        
                        if (!tr.TrainerIndex.IsNull())
                        {
                            var et = GetTrainerByIndex(GuitarTrainerType.ProGuitar, tr.TrainerIndex);
                            if (et == null)
                            {
                                trainers.Add(tr);
                            }
                            else
                            {
                                et.SetStart(meta.DownEvent);
                            }
                        }
                    }
                    if (eventType == GuitarTrainerMetaEventType.BeginProBass)
                    {
                        var tr = new GuitarTrainer(this, GuitarTrainerType.ProBass);
                        tr.SetStart(meta.DownEvent);

                        if (!tr.TrainerIndex.IsNull())
                        {
                            var et = GetTrainerByIndex(GuitarTrainerType.ProBass, tr.TrainerIndex);
                            if (et == null)
                            {
                                trainers.Add(tr);
                            }
                            else
                            {
                                et.SetStart(meta.DownEvent);
                            }
                        }
                    }
                    if (eventType == GuitarTrainerMetaEventType.ProGuitarNorm)
                    {
                        int tick = meta.DownTick;
                        int index = text.GetTrainerEventIndex();
                        if (!index.IsNull())
                        {
                            var et = trainers.Where(x => x.TrainerIndex == index && x.TrainerType == GuitarTrainerType.ProGuitar).FirstOrDefault();
                            if (et != null)
                            {
                                et.SetNorm(meta.DownEvent);
                            }
                        }
                    }
                    if (eventType == GuitarTrainerMetaEventType.ProBassNorm)
                    {
                        int tick = meta.DownTick;
                        int index = text.GetTrainerEventIndex();
                        if (!index.IsNull())
                        {
                            var et = trainers.Where(x => x.TrainerIndex == index && x.TrainerType == GuitarTrainerType.ProBass).FirstOrDefault();
                            if (et != null)
                            {
                                et.SetNorm(meta.DownEvent);
                            }
                        }
                    }
                    if (eventType == GuitarTrainerMetaEventType.EndProGuitar)
                    {
                        int tick = meta.DownTick;
                        int index = text.GetTrainerEventIndex();
                        if (!index.IsNull())
                        {
                            var et = trainers.Where(x => x.TrainerIndex == index && x.TrainerType == GuitarTrainerType.ProGuitar).FirstOrDefault();
                            if (et != null)
                            {
                                et.SetEnd(meta.DownEvent);
                            }
                        }
                    }
                    if (eventType == GuitarTrainerMetaEventType.EndProBass)
                    {
                        int tick = meta.DownTick;
                        int index = text.GetTrainerEventIndex();
                        if (!index.IsNull())
                        {
                            var et = trainers.Where(x => x.TrainerIndex == index && x.TrainerType == GuitarTrainerType.ProBass).FirstOrDefault();
                            if (et != null)
                            {
                                et.SetEnd(meta.DownEvent);
                            }
                        }
                    }
                }

                foreach (var tr in trainers.Where(x=> x.Start.IsTrainerEvent &&
                    x.End.IsTrainerEvent).OrderBy(x=> x.TrainerIndex))
                {
                    Messages.Add(tr);
                }
            }
            catch { }
        }

        public void RefreshTrainers()
        {
            try
            {

                var ret = new List<GuitarTrainer>();

                
            }
            catch { }
        }




        private bool GetMidiEvents(bool includeDifficultyAll)
        {
            bool ret = true;

            try
            {
                bool trackDirty = dirtyItems.HasFlag(DirtyItem.Track);
                bool messagesDirty = dirtyItems.HasFlag(DirtyItem.Message);
                if (messagesDirty && !trackDirty)
                {
                    var nd = gtrack.Events.Count(x => x.Dirty);
                    if (nd > 200)
                    {
                        dirtyItems |= DirtyItem.Track;
                        trackDirty = true;
                    }
                }

                if ((dirtyItems & DirtyItem.Track) == DirtyItem.Track)
                {
                    var diff = (includeDifficultyAll ? GuitarDifficulty.All : 0) | CurrentDifficulty;
                    chanEvents = GetMessages(false, diff);
                }
                else if ((dirtyItems & DirtyItem.Message) == DirtyItem.Message)
                {
                    var diff = (includeDifficultyAll ? GuitarDifficulty.All : 0) | CurrentDifficulty;
                    chanEvents = GetMessages(true, diff);
                }
                else if ((dirtyItems & DirtyItem.Difficulty) == DirtyItem.Difficulty)
                {
                    var diff = (includeDifficultyAll ? GuitarDifficulty.All : 0) | CurrentDifficulty;
                    chanEvents = GetMessages(false, diff);
                }
            }
            catch { ret = false; }
            return ret;
        }


        public List<GuitarMessage> GetMessages(bool refreshing, GuitarDifficulty diff)
        {
            if (refreshing)
            {
                DeletedMessages.ForEach(x => Messages.Remove(x));
            }
            else
            {
                eventsAll.Clear();

                eventsAll.AddRange(gtrack.ChanMessages.Select(x => new GuitarMessage(this, x)));
                
                eventsAll.AddRange(gtrack.Meta.Select(x => new GMetaMessage(this, x)));

                eventsAll = eventsAll.SortTicks().ToList();
            }

            return ChanEventsAll.Where(x => diff.HasFlag(x.Difficulty) ).SortTicks().ToList();
        }

        public List<GuitarChord> GetChordsByDifficulty(GuitarDifficulty difficulties)
        {
            var ret = new List<GuitarChord>();

            foreach (var difficulty in Utility.EasyMediumHardExpert)
            {
                if (difficulties.HasFlag(difficulty))
                {
                    SetTrack(this.gtrack, difficulty, true);

                    foreach (var gc in Messages.Chords)
                    {
                        ret.Add(gc);
                    }
                }
            }

            return ret.SortTicks().ToList();
        }

        

        public bool CreateHandPositionEvents(GenDiffConfig config)
        {
            try
            {
                var allChords = GetChordsByDifficulty(GuitarDifficulty.EasyMediumHardExpert);

                bool hasAbove14 = allChords.Any(x => x.HighestFret > 14);

                Remove(ChanEventsAll.Where(x => x.Data1 == Utility.HandPositionData1).ToList());

                MidiEvent downEvent, upEvent;
                Utility.CreateMessage(this, Utility.HandPositionData1, 100, 0, 20, 40, out downEvent, out upEvent);

                if (hasAbove14)
                {
                    bool isGuitar = IsGuitarTrackName(Name);

                    var expert = allChords.Where(x => x.Difficulty == GuitarDifficulty.Expert).ToList();

                    
                    if (isGuitar)
                    {
                        if (config.EnableProGuitarHard == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            Load6(gtrack, GuitarDifficulty.Hard);
                            foreach (var hn in hard)
                            {
                                if (expert.GetChordsByDownTick(hn.DownTick).Any() &&
                                    !expert.GetChordsByDownTick(hn.DownTick - 1).Any())
                                {
                                    hn.DownTick -= 1;
                                    hn.UpTick -= 1;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Hard);
                        }
                        if (config.EnableProGuitarMedium == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            Load6(gtrack, GuitarDifficulty.Medium);
                            foreach (var hn in med)
                            {
                                if ((hard.GetChordsByDownTick(hn.DownTick).Any() ||
                                    expert.GetChordsByDownTick(hn.DownTick).Any()) &&
                                    !(expert.GetChordsByDownTick(hn.DownTick - 2).Any() ||
                                      hard.GetChordsByDownTick(hn.DownTick - 2).Any()))
                                {
                                    hn.DownTick -= 2;
                                    hn.UpTick -= 2;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Medium);
                        }
                        if (config.EnableProGuitarEasy == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            var easy = allChords.Where(x => x.Difficulty == GuitarDifficulty.Easy).ToList();
                            Load6(gtrack, GuitarDifficulty.Easy);
                            foreach (var hn in easy)
                            {
                                if ((med.GetChordsByDownTick(hn.DownTick).Any() ||
                                    hard.GetChordsByDownTick(hn.DownTick).Any() ||
                                    expert.GetChordsByDownTick(hn.DownTick).Any()) &&
                                    !(expert.GetChordsByDownTick(hn.DownTick - 3).Any() ||
                                      hard.GetChordsByDownTick(hn.DownTick - 3).Any() ||
                                      med.GetChordsByDownTick(hn.DownTick - 3).Any()))
                                {
                                    hn.DownTick -= 3;
                                    hn.UpTick -= 3;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Easy);
                        }
                    }
                    else
                    {
                        if (config.EnableProBassHard == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            Load6(gtrack, GuitarDifficulty.Hard);
                            foreach (var hn in hard)
                            {
                                if (expert.GetChordsByDownTick(hn.DownTick).Any() &&
                                    !expert.GetChordsByDownTick(hn.DownTick - 1).Any())
                                {
                                    hn.DownTick -= 1;
                                    hn.UpTick -= 1;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Hard);
                        }
                        if (config.EnableProBassMedium == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            Load6(gtrack, GuitarDifficulty.Medium);
                            foreach (var hn in med)
                            {
                                if ((hard.GetChordsByDownTick(hn.DownTick).Any() ||
                                    expert.GetChordsByDownTick(hn.DownTick).Any()) &&
                                    !(expert.GetChordsByDownTick(hn.DownTick - 2).Any() ||
                                      hard.GetChordsByDownTick(hn.DownTick - 2).Any()))
                                {
                                    hn.DownTick -= 2;
                                    hn.UpTick -= 2;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Medium);
                        }
                        if (config.EnableProBassEasy == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            var easy = allChords.Where(x => x.Difficulty == GuitarDifficulty.Easy).ToList();
                            Load6(gtrack, GuitarDifficulty.Easy);
                            foreach (var hn in easy)
                            {
                                if ((med.GetChordsByDownTick(hn.DownTick).Any() ||
                                    hard.GetChordsByDownTick(hn.DownTick).Any() ||
                                    expert.GetChordsByDownTick(hn.DownTick).Any()) &&
                                    !(expert.GetChordsByDownTick(hn.DownTick - 3).Any() ||
                                      hard.GetChordsByDownTick(hn.DownTick - 3).Any() ||
                                      med.GetChordsByDownTick(hn.DownTick - 3).Any()))
                                {
                                    hn.DownTick -= 3;
                                    hn.UpTick -= 3;
                                    hn.UpdateChordProperties();
                                }
                            }
                            Load6(gtrack, GuitarDifficulty.Easy);
                        }
                    }

                    allChords = GetChordsByDifficulty(GuitarDifficulty.EasyMediumHardExpert);
                    
                    int last108Fret = -1;
                    int last108Tick = 40;

                    foreach (var me in allChords)
                    {
                        int fret = me.LowestNonZeroFret;

                        if (fret > 17)
                        {
                            fret = 17;
                        }
                        if (fret < 0)
                            fret = 0;

                        if (fret != last108Fret)
                        {
                            var down = me.DownTick;
                            var up = me.DownTick;

                            if (down > last108Tick)
                            {
                                last108Fret = fret;

                                last108Tick = up;

                                //MidiEvent downEvent, upEvent;
                                Utility.CreateMessage(this, Utility.HandPositionData1,
                                    100 + fret, 0, down, up, out downEvent, out upEvent);
                                
                            }
                        }
                    }
                }
             
                return true;
            }
            catch {
                return false;
            }
        }


        public List<GuitarMessage> GetMessagesAtTick(int downTick, int upTick)
        {
            return Messages.GetBetweenTick(downTick, upTick).ToList();
        }

        public List<GuitarMessage> GetMessagesAtTime(double minTime, double maxTime)
        {
            return Messages.ToList().SortTicks().GetBetweenTime(minTime, maxTime).ToList();
        }

        public List<GuitarChord> GetChordsAtTime(double minTime, double maxTime)
        {
            return Messages.Chords.GetChordsAtTime(minTime, maxTime).ToList();
        }

        public List<GuitarChord> GetChordsAtTick(int minTick, int maxTick)
        {
            return Messages.Chords.GetChordsAtTick(minTick, maxTick).ToList();
        }
    }
}
