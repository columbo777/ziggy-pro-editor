using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    
    


    public class GuitarTrack
    {
        
        public string Name
        {
            get { return midiTrack == null ? "" : (midiTrack.Name??""); }
        }

        public Sequence Sequence
        {
            get { return midiTrack != null ? midiTrack.Sequence : null; }
        }

        public bool IsLoaded
        {
            get
            {
                return midiTrack != null && Messages != null;
            }
        }

        public void Close()
        {
            midiTrack = null;
            Messages = null;
        }

        public TrackEditor Owner { get { return owner; } }
        TrackEditor owner;

        public GuitarTrack(TrackEditor owner, bool isPro)
        {
            this.owner = owner;
            this.IsPro = isPro;
            
            this.dirtyItems = DirtyItem.None;
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
                message.RemoveEvents();

                Messages.Remove(message);

                message.IsDeleted = true;
            }
        }


        public void Remove( MidiEvent ev)
        {
            if (ev != null && !ev.Deleted)
            {
                midiTrack.Remove(ev);
            }
        }

        public void Remove(IEnumerable<MidiEvent> ev)
        {
            foreach (var e in ev)
            {
                Remove(e); 
            }
        }


        public MidiEvent Insert(int absoluteTicks, IMidiMessage ev)
        {
            ev.IfIsType<MetaMessage>(evMeta =>
            {
                if (midiTrack.Meta.Any(x => x.AbsoluteTicks == absoluteTicks &&
                    evMeta.MetaType == x.MetaType))
                {
                    Debug.WriteLine("Possible duplicate");
                }
            });
            
            return midiTrack.Insert(absoluteTicks, ev);
        }

        public MidiEventPair Insert(int data1, int data2, int channel, TickPair ticks)
        {
            if(!ticks.IsValid)
                return new MidiEventPair();
            
            var down = Insert(ticks.Down, new ChannelMessage(ChannelCommand.NoteOn, data1, data2, channel));
            var up = Insert(ticks.Up, new ChannelMessage(ChannelCommand.NoteOff, data1, 0, channel));
            
            return new MidiEventPair(this, down, up);
        }

        public Track GetTrack() { return this.midiTrack; }


        
        public Track GetTempoTrack()
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

        
        IEnumerable<GuitarTempo> GetTempoFromTrack(Track tempoTrack)
        {
            var ret = new List<GuitarTempo>();
            if(tempoTrack != null && tempoTrack.Tempo.Any())
            {
                ret.AddRange( 
                    tempoTrack.Tempo.Select(x => 
                        new GuitarTempo(this, x)));
            }
            else
            {
                ret.Add(GuitarTempo.GetTempo(this, Utility.DummyTempo));
            }
            return ret;
        }


        bool BuildTempo(Track tempoTrack)
        {
            
            var ret = false;
            try
            {
                var timeSigs = GetTimeSignaturesFromTrack(tempoTrack);
                var tempos = GetTempoFromTrack(tempoTrack);

                int nextTick = 0;

                tempos.For((t, i) =>
                {
                    tempos.ElementAtOrDefault(i + 1).IfObjectNotNull(next =>
                    {
                        nextTick = next.AbsoluteTicks;
                    },
                    Else =>
                    {
                        nextTick = Sequence.GetLength();
                    });

                    t.SetUpTick(nextTick);
                });

                nextTick = 0;
                timeSigs.For((t, i) =>
                {
                    timeSigs.ElementAtOrDefault(i + 1).IfObjectNotNull(next =>
                    {
                        nextTick = next.AbsoluteTicks;
                    },
                    Else =>
                    {
                        nextTick = Sequence.GetLength();
                    });

                    t.SetUpTick(nextTick);
                });

                Messages.AddRange(timeSigs);
                Messages.AddRange(tempos);

                ret = timeSigs.Any() && tempos.Any();

                NoteGrid = new NoteGrid(owner);
            }
            catch { }
            return ret;
        }

        public GridPoint GetCloseGridPointToTick(int tick)
        {
            return NoteGrid.Points.Where(p => p.Tick.IsCloseTick(tick)).OrderByDescending(x=> Math.Abs(tick-x.Tick)).FirstOrDefault();
        }

        public GridPoint GetCloseGridPointToScreenPoint(int screenPoint)
        {
            return NoteGrid.Points.Where(p => p.ScreenPoint.IsCloseScreenPoint(screenPoint)).OrderByDescending(x => Math.Abs(screenPoint - x.ScreenPoint)).FirstOrDefault();
        }
        public GuitarTimeSignature GetTimeSignatureFromTick(int tick)
        {
            return Messages.TimeSignatures.LastOrDefault(x => x.DownTick <= tick);
        }

        public GuitarTimeSignature GetTimeSignatureFromTime(double time)
        {
            return Messages.TimeSignatures.LastOrDefault(x => x.StartTime <= time);
        }

        IEnumerable<GuitarTimeSignature> GetTimeSignaturesFromTrack(Track tempoTrack)
        {
            var ret = new List<GuitarTimeSignature>();
            if (tempoTrack != null && tempoTrack.TimeSig.Any())
            {
                ret.AddRange(tempoTrack.TimeSig.Select(t => new GuitarTimeSignature(this, t)));
            }
            else
            {
                ret.Add(GuitarTimeSignature.GetDefaultTimeSignature(this));
            }
            return ret;
        }


        public double TotalSongTime
        {
            get
            {
                var time = Messages.Tempos.Sum(x => x.TickLength * x.SecondsPerTick);
                return TickToTime(midiTrack.Length);
            }
        }



        public TickPair TimeToTick(TimePair times)
        {
            return new TickPair(TimeToTick(times.Down), TimeToTick(times.Up));
        }

        public TimePair TickToTime(TickPair ticks)
        {
            return new TimePair(TickToTime(ticks.Down), TickToTime(ticks.Up));
        }

        public double TickToTime(int absTick)
        {
            if (absTick <= 0)
                return 0;
            
            var t = GetTempo(absTick);

            var over = absTick - t.DownTick;

            return t.StartTime + t.SecondsPerTick * over;
        }

        public int TimeToTick(double absTime)
        {
            
            var tempos = Messages.Tempos;

            var firstTempo = tempos.First();

            if (absTime < firstTempo.EndTime)
            {
                if (absTime < 0)
                    absTime = 0;

                return firstTempo.DownTick + (firstTempo.TicksPerSecond * (absTime - firstTempo.StartTime)).Round();
            }
            else 
            {
                var lastTempo = tempos.Last();

                if (absTime >= lastTempo.StartTime)
                {
                    return lastTempo.DownTick + (lastTempo.TicksPerSecond * (absTime - lastTempo.StartTime)).Round();
                }
                else
                {
                    double time = 0;
                    
                    foreach(var tempo in tempos)
                    {
                        var tempoLen = (tempo.SecondsPerTick*tempo.TickLength);
                        if ((time + tempoLen) > absTime)
                        {
                            return tempo.DownTick + (tempo.TicksPerSecond * (absTime - tempo.StartTime)).Round();
                        }
                        else
                        {
                            time += tempoLen;
                        }
                    }


                    return lastTempo.DownTick + (lastTempo.TicksPerSecond * (absTime - lastTempo.StartTime)).Round();
                }
            }
        }

        public NoteGrid NoteGrid;

        public void RebuildEvents()
        {
            if (IsPro)
            {
                LoadEvents6();
            }
            else
            {
                BuildMessages5();
            }
        }

        
        public GuitarTempo GetTempoFromTime(double time)
        {
            return Messages.Tempos.GetTempo(TimeToTick(time));
        }

        public GuitarTempo GetTempo(int tick)
        {
            return Messages.Tempos.GetTempo(tick);
        }

        public GuitarTimeSignature GetTimeSignature(int tick)
        {
            return GetTimeSignatureFromTick(tick);
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

        void checkForInvalidNotes()
        {
            var notesOnData2Zero = midiTrack.ChanMessages.Where(x => x.Data2 == 0 && x.Command == ChannelCommand.NoteOn).ToList();
            if (notesOnData2Zero.Any())
            {
                midiTrack.Remove(notesOnData2Zero);

                foreach (var x in notesOnData2Zero)
                {
                    midiTrack.Insert(x.AbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOff, x.Data1, 0, x.Channel));
                }
            }
        }
        void BuildMessages5()
        {
            Messages = new GuitarMessageList(owner);

            if (!IsLoaded)
                return;

            checkForInvalidNotes();

            BuildTempo(GetTempoTrack());
            
            var events = midiTrack.ChanMessages.GroupByData1Channel(Utility.GetKnownData1ForDifficulty(IsPro,CurrentDifficulty|GuitarDifficulty.All)).ToList();

            Messages.AddRange(events.GetEventPairs(this, new[]{Utility.ExpertSoloData1_G5, Utility.SoloData1}).ToList().Select(x => new GuitarSolo(this, x.Down, x.Up)));

            Messages.AddRange(events.GetEventPairs(this, Utility.PowerupData1).Select(x => new GuitarPowerup(this, x.Down, x.Up)));

            Messages.AddRange(events.GetEventPairs(this, Utility.BigRockEndingData1[0]).Select(x => new GuitarBigRockEnding(this, x.Down, x.Up)));

            Messages.AddRange(events.GetEventPairs(this, Utility.GetStringsForDifficulty5(CurrentDifficulty)).Select(x => new GuitarNote(this, x.Down, x.Up)));
            
            Messages.AddRange(Messages.Notes.GroupByCloseTick().ToList().Select(x => new GuitarChord(this, x, false)));

            Messages.AddRange(midiTrack.Meta.Where(x => x.IsTextEvent() && x.MetaMessage.Text.GetGuitarTrainerMetaEventType() == GuitarTrainerMetaEventType.Unknown)
                .ToList().Select(x => GuitarTextEvent.GetTextEvent(this, x)));
        }



        public bool ViewLyrics { get; set; }

        
        Track midiTrack;

        public GuitarMessageList Messages
        {
            get;
            internal set;
        }

        public bool HasInvalidTempo = true;

        public bool IsPro = false;

        GuitarDifficulty currentDifficulty = GuitarDifficulty.Expert;

        public GuitarDifficulty CurrentDifficulty 
        {
            get 
            { 
                return currentDifficulty; 
            }
            internal set
            {
                if (currentDifficulty != value)
                {
                    dirtyItems |= DirtyItem.Difficulty;

                    RebuildEvents();
                }
            }
        }

        public GuitarMessage CreateMessageByType(GuitarMessageType type, MidiEvent down, MidiEvent up=null)
        {
            GuitarMessage ret = null;
            switch (type)
            {
                case GuitarMessageType.GuitarHandPosition:
                    ret = new GuitarHandPosition(this, down, up);
                    break;
                case GuitarMessageType.GuitarTextEvent:
                    ret = GuitarTextEvent.GetTextEvent(this, down);
                    break;
                case GuitarMessageType.GuitarTrainer:
                    ret = new GuitarTrainer(this, GuitarTrainerType.Unknown);
                    break;
                case GuitarMessageType.GuitarChord:
                    ret = new GuitarChord(this, Messages.Notes.Where(x=> x.DownTick == down.AbsoluteTicks), true);
                    break;
                case GuitarMessageType.GuitarChordStrum:
                    ret = new GuitarChordStrum(this, down, up);
                    break;
                case GuitarMessageType.GuitarNote:
                    ret = new GuitarNote(this, down, up);
                    break;
                case GuitarMessageType.GuitarPowerup:
                    ret = new GuitarPowerup(this, down, up);
                    break;
                case GuitarMessageType.GuitarSolo:
                    ret = new GuitarSolo(this, down, up);
                    break;
                case GuitarMessageType.GuitarTempo:
                    ret = new GuitarTempo(this, down);
                    break;
                case GuitarMessageType.GuitarTimeSignature:
                    ret = new GuitarTimeSignature(this, down);
                    break;
                case GuitarMessageType.GuitarArpeggio:
                    ret = new GuitarArpeggio(this, down, up);
                    break;
                case GuitarMessageType.GuitarBigRockEnding:
                    ret = new GuitarBigRockEnding(this, down, up);
                    break;
                case GuitarMessageType.GuitarSingleStringTremelo:
                    ret = new GuitarSingleStringTremelo(this, down, up);
                    break;
                case GuitarMessageType.GuitarMultiStringTremelo:
                    ret = new GuitarMultiStringTremelo(this, down, up);
                    break;
                case GuitarMessageType.GuitarSlide:
                    ret = new GuitarSlide(this, down, up);
                    break;
                case GuitarMessageType.GuitarHammeron:
                    ret = new GuitarHammeron(this, down, up);
                    break;
            }
            return ret;
        }

        bool LoadEvents6()
        {
            bool ret = true;
            try
            {
                Messages = new GuitarMessageList(owner);

                if (!IsLoaded)
                    return false;

                checkForInvalidNotes();

                BuildTempo(GetTempoTrack());

                
                var events = midiTrack.ChanMessages.GroupByData1Channel(
                    Utility.GetKnownData1ForDifficulty(IsPro, CurrentDifficulty | GuitarDifficulty.All));

                Messages.AddRange(events.GetEventPairs(this, Utility.HandPositionData1).Select(x => new GuitarHandPosition(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllArpeggioData1).Select(x => new GuitarArpeggio(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.SoloData1).Select(x => new GuitarSolo(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.PowerupData1).Select(x => new GuitarPowerup(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.MultiStringTremeloData1).Select(x => new GuitarMultiStringTremelo(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.SingleStringTremeloData1).Select(x => new GuitarSingleStringTremelo(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.BigRockEndingData1[0]).Select(x => new GuitarBigRockEnding(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllSlideData1).Select(x => new GuitarSlide(this, x.Channel == Utility.ChannelSlideReversed, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllHammeronData1).Select(x => new GuitarHammeron(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllStrumData1).Select(x => new GuitarChordStrum(this, x.Down, x.Up, x.Channel.GetChordStrumFromChannel().GetModifierType())));
                Messages.AddRange(events.GetEventPairs(this, Utility.GetStringsForDifficulty6(CurrentDifficulty)).Select(x => new GuitarNote(this, x.Down, x.Up)));
                Messages.AddRange(Messages.Notes.GroupByCloseTick().ToList().Select(chordNotes => new GuitarChord(this, chordNotes, true)).ToList());
                Messages.AddRange(midiTrack.Meta.Where(x=> x.IsTextEvent() && x.MetaMessage.Text.GetGuitarTrainerMetaEventType() == GuitarTrainerMetaEventType.Unknown)
                    .Select(x => GuitarTextEvent.GetTextEvent(this, x)));

                LoadTrainers();
                
                ret=true;

                this.dirtyItems = DirtyItem.None;
                
            }
            catch { ret = false; }
            return ret;
        }

        

        public void AddTrainer(GuitarTrainer tr)
        {
            if (tr.TrainerIndex.IsNull())
            {
                tr.TrainerIndex = Messages.Trainers.Count(x => x.TrainerType == tr.TrainerType)+1;

                tr.Start.SetDownEvent(Insert(tr.Start.AbsoluteTicks, new MetaMessage(MetaType.Text, tr.StartText)));

                if (tr.Loopable)
                {
                    tr.Norm.SetDownEvent(Insert(tr.Norm.AbsoluteTicks, new MetaMessage(MetaType.Text, tr.NormText)));
                }
                
                tr.End.SetDownEvent(Insert(tr.End.AbsoluteTicks, new MetaMessage(MetaType.Text, tr.EndText)));
                Messages.Add(tr);
            }
        }


        public static string VocalTrackName5
        {
            get
            {
                return "PART VOCALS";
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

        public GuitarTrack SetTrack(Track t, GuitarDifficulty diff)
        {
            if (t == null || midiTrack == null)
            {
                midiTrack = null;
                dirtyItems |= DirtyItem.Track;
            }
            if (t != null)
            {
                midiTrack = t;
                currentDifficulty = diff;
                
                if (midiTrack == null || midiTrack.Sequence == null)
                {
                    this.SequenceDivision = 480;
                }
                else
                {
                    this.SequenceDivision = midiTrack.Sequence.Division;
                }

                RebuildEvents();

                this.dirtyItems = DirtyItem.None;
            }
            return this;
        }

        public double SequenceDivision
        {
            get;
            set;
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


        public IEnumerable<GuitarChord> GetChordsByDifficulty(
            GuitarDifficulty singleDifficulty)
        {
            CurrentDifficulty = singleDifficulty;
            return Messages.Chords.ToList();
        }

        public bool CreateHandPositionEvents(GenDiffConfig config)
        {
            try
            {
                var currDiff = CurrentDifficulty;

                var ex = GetChordsByDifficulty(GuitarDifficulty.Expert);
                var ha = GetChordsByDifficulty(GuitarDifficulty.Hard);
                var me = GetChordsByDifficulty(GuitarDifficulty.Medium);
                var ea = GetChordsByDifficulty(GuitarDifficulty.Easy);

                bool hasAbove14 = 
                    ex.Any(adc=> adc.HighestFret > 14) ||
                    ha.Any(adc => adc.HighestFret > 14) ||
                    me.Any(adc => adc.HighestFret > 14) ||
                    ea.Any(adc => adc.HighestFret > 14);

                CurrentDifficulty = GuitarDifficulty.Expert;

                Remove(Messages.HandPositions.ToList());

                GuitarHandPosition.CreateEvent(this, new TickPair(20,40), 0);
                
                if (hasAbove14)
                {
                    bool isGuitar = IsGuitarTrackName(Name);

                    if (isGuitar)
                    {
                        if (config.EnableProGuitarHard == false)
                        {
                            foreach (var hn in Messages.Chords)
                            {
                                if (ex.HasDownTickAtTick(hn.DownTick) &&
                                    !ex.HasDownTickAtTick(hn.DownTick - 1))
                                {
                                    hn.SetTicks(hn.TickPair-1);
                                    hn.UpdateEvents();
                                }
                            }
                            
                        }
                        if (config.EnableProGuitarMedium == false)
                        {
                            foreach (var hn in me)
                            {
                                if ((ha.HasDownTickAtTick(hn.DownTick) ||
                                    ex.HasDownTickAtTick(hn.DownTick)) &&
                                    !(ex.HasDownTickAtTick(hn.DownTick - 2) ||
                                      ha.HasDownTickAtTick(hn.DownTick - 2)))
                                {
                                    hn.SetTicks(hn.TickPair-2);
                                    hn.UpdateEvents();
                                }
                            }
                            
                        }
                        if (config.EnableProGuitarEasy == false)
                        {
                            foreach (var hn in ea)
                            {
                                if ((me.HasDownTickAtTick(hn.DownTick) ||
                                    ha.HasDownTickAtTick(hn.DownTick) ||
                                    ex.HasDownTickAtTick(hn.DownTick)) &&
                                    !(ex.HasDownTickAtTick(hn.DownTick - 3) ||
                                      ha.HasDownTickAtTick(hn.DownTick - 3) ||
                                      me.HasDownTickAtTick(hn.DownTick - 3)))
                                {
                                    hn.SetTicks(hn.TickPair-3);
                                    hn.UpdateEvents();
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        if (config.EnableProBassHard == false)
                        {
                            foreach (var hn in ha)
                            {
                                if (ex.HasDownTickAtTick(hn.DownTick) &&
                                    !ex.HasDownTickAtTick(hn.DownTick - 1))
                                {
                                    hn.SetTicks(hn.TickPair-1);
                                    hn.UpdateEvents();
                                }
                            }
                            
                        }
                        if (config.EnableProBassMedium == false)
                        {
                            foreach (var hn in me)
                            {
                                if ((ha.HasDownTickAtTick(hn.DownTick) ||
                                    ex.HasDownTickAtTick(hn.DownTick)) &&
                                    !(ex.HasDownTickAtTick(hn.DownTick - 2) ||
                                      ha.HasDownTickAtTick(hn.DownTick - 2)))
                                {
                                    hn.SetTicks(hn.TickPair-2);
                                    hn.UpdateEvents();
                                }
                            }
                            
                        }
                        if (config.EnableProBassEasy == false)
                        {
                            foreach (var hn in ea)
                            {
                                if ((me.HasDownTickAtTick(hn.DownTick) ||
                                    ha.HasDownTickAtTick(hn.DownTick) ||
                                    ex.HasDownTickAtTick(hn.DownTick)) &&
                                    !(ex.HasDownTickAtTick(hn.DownTick - 3) ||
                                      ha.HasDownTickAtTick(hn.DownTick - 3) ||
                                      me.HasDownTickAtTick(hn.DownTick - 3)))
                                {
                                    hn.SetTicks(hn.TickPair-3);
                                    hn.UpdateEvents();
                                }
                            }
                        }
                    }

                    
                    
                    CurrentDifficulty = GuitarDifficulty.Expert;
                    Generate108();

                    CurrentDifficulty = GuitarDifficulty.Hard;
                    Generate108();

                    CurrentDifficulty = GuitarDifficulty.Medium;
                    Generate108();

                    CurrentDifficulty = GuitarDifficulty.Easy;
                    Generate108();

                }
             
                return true;
            }
            catch {
                return false;
            }
        }

        private void Generate108()
        {

            int last108Fret = -1;
            int last108Tick = 40;

            foreach (var hp in Messages.Chords.ToList())
            {
                int fret = hp.LowestNonZeroFret;
                var hiFret = hp.HighestFret;

                if (fret >= 17)
                {
                    fret = 17;
                }
                if (hiFret <= 14)
                    fret = 0;

                if (fret != last108Fret)
                {
                    var down = hp.DownTick;
                    var up = hp.DownTick + 1;

                    if (down >= last108Tick)
                    {
                        last108Fret = fret;

                        last108Tick = up;

                        if (!Messages.HandPositions.Any(x => x.DownTick == down))
                        {
                            GuitarHandPosition.CreateEvent(this, new TickPair(down, up), fret);
                        }
                    }
                }
            }
        }

    }
}
