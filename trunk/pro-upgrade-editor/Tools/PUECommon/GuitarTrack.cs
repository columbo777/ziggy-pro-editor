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
            get { return gtrack == null ? "" : (gtrack.Name??""); }
        }

        public Sequence Sequence
        {
            get { return gtrack != null ? gtrack.Sequence : null; }
        }

        public bool IsLoaded
        {
            get
            {
                return gtrack != null && Messages != null;
            }
        }

        public void Close()
        {
            gtrack = null;
            Messages = null;
        }

        public GuitarTrack(bool isPro)
        {
            this.IsPro = isPro;
            
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
                if (message is GuitarTrainer)
                {
                    (message as GuitarTrainer).RemoveSubMessages();
                }
                if (message.DownEvent != null)
                {
                    Remove(message.DownEvent);
                    message.DownEvent = null;
                }
                if (message.UpEvent != null)
                {
                    Remove(message.UpEvent);
                    message.UpEvent = null;
                }
                
                if (Messages.Contains(message))
                {
                    Messages.Remove(message);
                }

                message.IsDeleted = true;
            }
            else
            {
                
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
            return GuitarChord.GetChord(this, difficulty,
                downTick, upTick, frets, fretChannels, isSlide, isSlideReverse, isHammeron, strumMode);
        }

        public GuitarChord CreateChord(
            int[] frets,
            int[] fretChannels, 
            GuitarDifficulty difficulty,
            int downTick, int upTick, 
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode)
        {
            var ret = GuitarChord.GetChord(this, difficulty,
                downTick, upTick, frets, fretChannels, isSlide, isSlideReverse, isHammeron, strumMode);

            if (ret.HasNotes)
            {
                foreach (var n in ret.Notes)
                {
                    var cb = new ChannelMessageBuilder();
                    cb.Command = ChannelCommand.NoteOn;
                    cb.Data1 = n.Data1;
                    cb.Data2 = n.Data2;
                    cb.MidiChannel = n.Channel;
                    cb.Build();
                    n.DownEvent = gtrack.Insert(downTick, cb.Result);

                    cb.Command = ChannelCommand.NoteOff;
                    cb.Data1 = n.Data1;
                    cb.Data2 = 0;
                    cb.MidiChannel = n.Channel;
                    cb.Build();
                    n.UpEvent = gtrack.Insert(upTick, cb.Result);
                    
                }
                Messages.Add(ret);

                ret.UpdateChordProperties();
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

        public void Insert(int data1, int data2, int channel, int downTick, int upTick)
        {
            var cb = new ChannelMessageBuilder();
            cb.Data1 = data1;
            cb.Data2 = data2;
            cb.MidiChannel = channel;
            cb.Command = ChannelCommand.NoteOn;
            cb.Build();
            Insert(downTick, cb.Result);
            cb.Command = ChannelCommand.NoteOff;
            cb.Data2 = 0;
            cb.Build();
            Insert(upTick, cb.Result);
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
            return Sequence.FirstOrDefault(x => x.Tempo.Any() || x.TimeSig.Any());
        }

        public void RemoveTrack(Track t)
        {
            if (Sequence.Contains(t))
            {
                Sequence.Remove(t);
            }
            this.dirtyItems |= DirtyItem.All;
        }

        public void AddTrack(Track t)
        {
            if (!Sequence.Contains(t))
            {
                this.Sequence.Add(t);
            }
            this.dirtyItems |= DirtyItem.All;
        }

        public void AddTempoTrack(Track t)
        {
            if(t == null || t.Name == null)
                return;

            try
            {
                if (!Sequence.Contains(t))
                {
                    Sequence.AddTempo(t);
                }
            }
            catch { }

            dirtyItems |= DirtyItem.All;
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
            return (double)(Sequence == null ? 480 : Sequence.Division);
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
                
            }
            else if (guitarTrack != null && gtrack != null && guitarTrack == gtrack)
            {
                if (gtrack.Dirty)
                {
                    Debug.WriteLine("Load5 - Dirtying Messages");
                    this.dirtyItems |= DirtyItem.Message;
                    gtrack = guitarTrack;
                    
                }
            }
            else
            {
                this.gtrack = guitarTrack;
                this.dirtyItems |= DirtyItem.Track;
            }


            if (this.CurrentDifficulty != difficulty)
            {
                this.CurrentDifficulty = difficulty;
            }

            if (Dirty)
            {
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
                Messages = new GuitarMessageList(this);
                CompileTempo5();
            }

            var downNotes = new NoteDownEventManager(false);

            var notes = new List<GuitarNote>();
            foreach (var ev_atTick in gtrack.GetChanMessagesByDifficulty(CurrentDifficulty).Select(x=> new GuitarMessage(this,x)).GroupBy(x => x.AbsoluteTicks))
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

            gtrack.Meta.Where(x => x.IsTextEvent()).ForEach(x => AddTextEventMessage(x.ToGuitarMessage(this)));

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

            foreach(var ev in gtrack.ChanMessages)
            {
                if(ev.Data1 == Utility.SoloData1 ||
                    Utility.GetSoloData1_G5(CurrentDifficulty) == ev.Data1)
                {
                    if (ev.Command == ChannelCommand.NoteOn)
                    {
                        Messages.Add(new GuitarSolo(this, ev, null));
                    }
                    else
                    {
                        Messages.LastSolo.SetUpEvent(ev);
                    }
                }
                else if (ev.Data1 == Utility.PowerupData1)
                {
                    if (ev.Command == ChannelCommand.NoteOn )
                    {
                        Messages.Add(new GuitarPowerup(this, ev, null));
                    }
                    else
                    {
                        Messages.LastPowerup.SetUpEvent(ev);
                    }
                }
                else if (Utility.BigRockEndingData1.Contains(ev.Data1) && ev.AbsoluteTicks > currmod)
                {
                    if (ev.Command == ChannelCommand.NoteOn)
                    {
                        Messages.Add(new GuitarBigRockEnding(this, ev, null));
                    }
                    else
                    {
                        Messages.LastBigRockEnding.SetUpEvent(ev);
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

            if (gtrack != guitarTrack)
            {
                this.gtrack = guitarTrack;
                this.dirtyItems |= DirtyItem.Track;
            }

            if (gtrack != null && gtrack.Dirty)
            {
                this.dirtyItems |= DirtyItem.Track;
            }

            if (this.CurrentDifficulty != difficulty)
            {
                this.dirtyItems = DirtyItem.Difficulty;

                this.CurrentDifficulty = difficulty;
            }

            if (Dirty)
            {
                ret = LoadData6();
            }

            return ret;
        }

        bool LoadData6()
        {
            bool ret = false;
            try
            {
                if (gtrack == null)
                {
                    Messages = new GuitarMessageList(this);
                    return ret;
                }
                if (dirtyItems.HasFlag(DirtyItem.Track))
                {
                    HasInvalidTempo = false;
                }

                ret = LoadEvents6();

                
            }
            catch (Exception)
            {
                ret = false;
            }

            this.dirtyItems = DirtyItem.None;

            return ret;
        }

        GuitarMessageList internalMessages;
        Track gtrack;
       
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

        public bool HasInvalidTempo = true;

        public bool IsPro = false;

        GuitarDifficulty currentDifficulty = GuitarDifficulty.Expert;
        public GuitarDifficulty CurrentDifficulty 
        {
            get { return currentDifficulty; }
            internal set 
            {
                if (currentDifficulty != value || Dirty)
                {
                    currentDifficulty = value;
                    dirtyItems |= DirtyItem.Difficulty;
                    if (IsPro)
                    {
                        LoadData6();
                    }
                    else
                    {
                        LoadData5();
                    }
                }
            }
        }

        
        public List<MidiEvent> DirtyTrackEvents
        {
            get
            {
                return gtrack != null ? gtrack.Events.Where(x => x.Dirty).ToList() : new List<MidiEvent>();
            }
        }


        bool LoadEvents6()
        {
            bool ret = true;
            try
            {                
                
                Messages = new GuitarMessageList(this);


                CompileTempo6();

                gtrack.ChanMessages.Where(x=> x.IsHandPositionEvent()).ToGuitarMessage(this).GetMessagePairs().ForEach(x =>
                    {
                        Messages.Add(GuitarHandPosition.FromEvent(this, x.Down.MidiEvent, x.Up.MidiEvent));
                    });

                gtrack.Meta.Where(x=> x.IsTextEvent() && x.MetaMessage.Text.GetGuitarTrainerMetaEventType()==GuitarTrainerMetaEventType.Unknown)
                    .ForEach(x => AddTextEventMessage(x.ToGuitarMessage(this)));
                LoadTrainers();

                LoadModifiers6(false, gtrack.ChanMessages.Where(x=> x.IsProModifier()).ToGuitarMessage(this));

                var modifiers = LoadNoteModifiers6(gtrack.ChanMessages.Where(x=> x.IsProNoteModifier(CurrentDifficulty)).ToGuitarMessage(this));

                ret = LoadNotes6(false, gtrack.GetChanMessagesByDifficulty(CurrentDifficulty).ToGuitarMessage(this), modifiers);

                Messages.Chords.Where(x => x.Notes.Any(n => n.DownTick != x.DownTick || n.UpTick != x.UpTick)).ToList().ForEach(x =>
                    x.UpdateChordProperties());

                Messages.Chords.Where(x => x.Notes.Any(n => n.IsXNote) && x.Notes.Any(n=>!n.IsXNote)).ToList().ForEach(x =>
                    x.UpdateChordProperties());

                this.dirtyItems = DirtyItem.None;
                
            }
            catch { ret = false; }
            return ret;
        }

        private void AddTextEventMessage(GuitarMessage x)
        {
            Messages.Add(GuitarTextEvent.GetTextEvent(this, x.MidiEvent));
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
                var n = GetDownNote(note);

                return n != null && n.IsDown == true;
            }

            DownNote GetDownNote(GuitarNote note)
            {
                return downEvents.SingleOrDefault(x => x.IsDown &&
                    isPro ? (x.Note.Data1 == note.Data1 && x.Note.NoteString == note.NoteString) : 
                            (x.Note.Data1 == note.Data1 && x.Note.Channel == note.Channel));
            }
            DownNote GetUpNote(GuitarNote note)
            {
                return downEvents.SingleOrDefault(x => x.IsDown==false &&
                    isPro ? (x.Note.Data1 == note.Data1 && x.Note.NoteString == note.NoteString) :
                            (x.Note.Data1 == note.Data1 && x.Note.Channel == note.Channel));
            }

            public bool SetNoteDown(GuitarNote note)
            {
                bool ret = true;
                
                var dn = GetUpNote(note);
                if (dn == null)
                {
                    var curDown = GetDownNote(note);
                    if (curDown == null)
                    {
                        downEvents.Add(new DownNote() { IsDown = true, Note = note });
                    }
                    else
                    {
                        ret = false;
                    }
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
                
                var dn = GetDownNote(note);
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
                tr.TrainerIndex = Messages.Trainers.Count(x => x.TrainerType == tr.TrainerType)+1;

                var mb = new MetaTextBuilder();
                mb.Type = MetaType.Text;
                mb.Text = tr.StartText;
                mb.Build();
                tr.Start.MidiEvent = Insert(tr.Start.AbsoluteTicks, mb.Result);

                if (tr.Loopable)
                {
                    mb.Text = tr.NormText;
                    mb.Build();
                    tr.Norm.MidiEvent = Insert(tr.Norm.AbsoluteTicks, mb.Result);
                    
                }
                mb.Text = tr.EndText;
                mb.Build();
                tr.End.MidiEvent = Insert(tr.End.AbsoluteTicks, mb.Result);
                
                Messages.Add(tr);
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
                        gtrack.Remove(ev.MidiEvent);
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
                            if (currentChord.Notes[note.NoteString] == null)
                            {
                                currentChord.Notes[note.NoteString] = note;
                            }
                            else
                            {
                                gtrack.Remove(note.MidiEvent);
                            }
                        }
                        else
                        {
                            currentChord = new GuitarChord(this);
                            currentChord.Notes[note.NoteString] = note;
                            Messages.Add(currentChord);

                            var chordModifiers = modifiers.GetBetweenTick(currentChord.DownTick, currentChord.DownTick + Utility.NoteCloseWidth);
                            if (chordModifiers.Any())
                            {
                                currentChord.Modifiers.AddRange(chordModifiers.Cast<GuitarModifier>().ToArray());

                                foreach (var mod in chordModifiers)
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
                    }
                    else
                    {
                        gtrack.Remove(ev.MidiEvent);
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
            foreach (var ev in events)
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
            foreach(var ev in events)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
            if (ev.ChannelMessage.Command == ChannelCommand.NoteOn)
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
       

        public GuitarTrack SetTrack(Track t, GuitarDifficulty diff)
        {
            if (t == null || gtrack == null)
            {
                gtrack = null;
                dirtyItems |= DirtyItem.Track;
            }
            else if(gtrack != t)
            {
                dirtyItems |= DirtyItem.Track;
            }
            else if (this.CurrentDifficulty != diff)
            {
                dirtyItems |= DirtyItem.Difficulty;
            }

            if (Dirty || gtrack.ChanMessages.Any(x=> x.Dirty))
            {
                gtrack = t;
                this.CurrentDifficulty = diff;
                
            }
            
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
                foreach (var meta in Messages.TextEvents.Where(x=> x.TrainerType != GuitarTrainerMetaEventType.Unknown))
                {
                    var eventType = meta.TrainerType;
                    int index = meta.Text.GetTrainerEventIndex();

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

                foreach (var tr in trainers.Where(x=> x.Valid).OrderBy(x=> x.TrainerIndex))
                {
                    Messages.Add(tr);
                }
            }
            catch { }
        }

        public void RefreshTrainers()
        {
            LoadTrainers();
        }



        public List<GuitarChord> GetChordsByDifficulty(GuitarDifficulty difficulties)
        {
            var ret = new List<GuitarChord>();

            var diff = this.CurrentDifficulty;
            foreach (var difficulty in Utility.EasyMediumHardExpert)
            {
                if (difficulties.HasFlag(difficulty))
                {
                    this.CurrentDifficulty = difficulty;

                    ret.AddRange(Messages.Chords.ToArray());
                }
            }
            this.CurrentDifficulty = diff;
            return ret.SortTicks().ToList();
        }

        

        public bool CreateHandPositionEvents(GenDiffConfig config)
        {
            try
            {
                var currDiff = CurrentDifficulty;

                var allChords = GetChordsByDifficulty(GuitarDifficulty.EasyMediumHardExpert);

                bool hasAbove14 = allChords.Any(x => x.HighestFret > 14);

                Messages.HandPositions.ToList().ForEach(x => Remove(x));

                GuitarHandPosition.CreateEvent(this, 20, 40, 0);
                
                if (hasAbove14)
                {
                    bool isGuitar = IsGuitarTrackName(Name);

                    var expert = allChords.Where(x => x.Difficulty.IsExpert()).ToList();

                    if (isGuitar)
                    {
                        if (config.EnableProGuitarHard == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty.IsHard()).ToList();
                            CurrentDifficulty = GuitarDifficulty.Hard;
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
                            
                        }
                        if (config.EnableProGuitarMedium == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            CurrentDifficulty = GuitarDifficulty.Medium;
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
                            
                        }
                        if (config.EnableProGuitarEasy == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            var easy = allChords.Where(x => x.Difficulty == GuitarDifficulty.Easy).ToList();
                            CurrentDifficulty = GuitarDifficulty.Easy;
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
                            
                        }
                    }
                    else
                    {
                        if (config.EnableProBassHard == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            CurrentDifficulty = GuitarDifficulty.Hard;
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
                            
                        }
                        if (config.EnableProBassMedium == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            CurrentDifficulty = GuitarDifficulty.Medium;
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
                            
                        }
                        if (config.EnableProBassEasy == false)
                        {
                            var hard = allChords.Where(x => x.Difficulty == GuitarDifficulty.Hard).ToList();
                            var med = allChords.Where(x => x.Difficulty == GuitarDifficulty.Medium).ToList();
                            var easy = allChords.Where(x => x.Difficulty == GuitarDifficulty.Easy).ToList();
                            CurrentDifficulty = GuitarDifficulty.Easy;
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
                        }
                    }

                    CurrentDifficulty = GuitarDifficulty.Expert;
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
                            var up = me.DownTick+1;

                            if (down >= last108Tick)
                            {
                                last108Fret = fret;

                                last108Tick = up;

                                GuitarHandPosition.CreateEvent(this, down, up, fret);
                            }
                        }
                    }

                    CurrentDifficulty = currDiff;
                }
             
                return true;
            }
            catch {
                return false;
            }
        }


        public IEnumerable<GuitarMessage> GetMessagesAtTick(int downTick, int upTick)
        {
            return Messages.GetBetweenTick(downTick, upTick);
        }

        public IEnumerable<GuitarChord> GetChordsAtTime(double minTime, double maxTime)
        {
            return Messages.Chords.GetChordsAtTime(minTime, maxTime);
        }

        public IEnumerable<GuitarChord> GetChordsAtTick(int minTick, int maxTick)
        {
            return Messages.Chords.GetChordsAtTick(minTick, maxTick);
        }
    }
}
