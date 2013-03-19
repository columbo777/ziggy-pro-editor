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
            get { return midiTrack == null ? "" : (midiTrack.Name ?? ""); }
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


        public void Remove(MidiEvent ev)
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
            if (!ticks.IsValid)
                return default(MidiEventPair);

            var down = Insert(ticks.Down, new ChannelMessage(ChannelCommand.NoteOn, data1, data2, channel));
            var up = Insert(ticks.Up, new ChannelMessage(ChannelCommand.NoteOff, data1, 0, channel));

            return new MidiEventPair(Messages, down, up);
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
            if (t == null || t.Name == null)
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


        IEnumerable<GuitarTempo> internalGetTempo(GuitarMessageList owner, Track tempoTrack)
        {
            var ret = new List<GuitarTempo>();
            if (tempoTrack != null && tempoTrack.Tempo.Any())
            {
                ret.AddRange(
                    tempoTrack.Tempo.Select(x =>
                        new GuitarTempo(owner, x)));
            }
            else
            {
                ret.Add(GuitarTempo.GetTempo(this, Utility.DummyTempo));
            }
            return ret;
        }

        public IEnumerable<GuitarTimeSignature> GetTimeSigMessages(GuitarMessageList owner, Track tempoTrack)
        {
            var ret = new List<GuitarTimeSignature>();

            try
            {
                var timeSigs = GetTimeSignaturesFromTrack(owner, tempoTrack);

                int nextTick = 0;
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
                ret.AddRange(timeSigs.ToList());
            }
            catch { }
            return ret;
        }

        public IEnumerable<GuitarTempo> GetTempoMessages(GuitarMessageList list, Track tempoTrack)
        {
            var ret = new List<GuitarTempo>();

            try
            {

                var tempos = internalGetTempo(list, tempoTrack);

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

                ret.AddRange(tempos.ToList());
            }
            catch { }
            return ret;
        }

        public GridPoint GetCloseGridPointToTick(int tick)
        {
            return NoteGrid.Points.Where(p => p.Tick.IsCloseTick(tick)).OrderByDescending(x => Math.Abs(tick - x.Tick)).FirstOrDefault();
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

        IEnumerable<GuitarTimeSignature> GetTimeSignaturesFromTrack(GuitarMessageList list, Track tempoTrack)
        {
            var ret = new List<GuitarTimeSignature>();
            if (tempoTrack != null && tempoTrack.TimeSig.Any())
            {
                ret.AddRange(tempoTrack.TimeSig.Select(t => new GuitarTimeSignature(list, t)));
            }
            else
            {
                ret.Add(GuitarTimeSignature.GetDefaultTimeSignature(list));
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

                    foreach (var tempo in tempos)
                    {
                        var tempoLen = (tempo.SecondsPerTick * tempo.TickLength);
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

        void checkForInvalidNotes(Track track)
        {
            try
            {
                var notesOnData2Zero = track.ChanMessages.Where(x => x.Data2 == 0 && x.Command == ChannelCommand.NoteOn).ToList();
                if (notesOnData2Zero.Any())
                {
                    track.Remove(notesOnData2Zero);

                    foreach (var x in notesOnData2Zero)
                    {
                        track.Insert(x.AbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOff, x.Data1, 0, x.Channel));
                    }
                }
            }
            catch { }
        }
        void BuildMessages5()
        {
            Messages = null;

            checkForInvalidNotes(midiTrack);

            Messages = GetMessages(CurrentDifficulty);

            NoteGrid = new NoteGrid(owner);

            /*var tempoTrack = GetTempoTrack();
            Messages.AddRange(GetTempoMessages(tempoTrack));
            Messages.AddRange(GetTimeSigMessages(tempoTrack));

            NoteGrid = new NoteGrid(owner);

            var events = midiTrack.ChanMessages.GroupByData1Channel(Utility.GetKnownData1ForDifficulty(IsPro, CurrentDifficulty | GuitarDifficulty.All)).ToList();

            Messages.AddRange(events.GetEventPairs(this, new[] { Utility.ExpertSoloData1_G5, Utility.SoloData1 }).ToList().Select(x => new GuitarSolo(this, x.Down, x.Up)));

            Messages.AddRange(events.GetEventPairs(this, Utility.PowerupData1).Select(x => new GuitarPowerup(this, x.Down, x.Up)));

            Messages.AddRange(events.GetEventPairs(this, Utility.GetStringsForDifficulty5(CurrentDifficulty)).Select(x => new GuitarNote(this, x.Down, x.Up)));

            Messages.AddRange(Messages.Notes.GroupByCloseTick().ToList().Select(x => GuitarChord.GetChord(this, x, false)));

            Messages.AddRange(midiTrack.Meta.Where(x => x.IsTextEvent() && x.MetaMessage.Text.GetGuitarTrainerMetaEventType() == GuitarTrainerMetaEventType.Unknown)
                .ToList().Select(x => GuitarTextEvent.GetTextEvent(this, x)));*/
        }

        IEnumerable<GuitarBigRockEnding> GetBigRockEndings(GuitarMessageList list, IEnumerable<IGrouping<Data1ChannelPair, MidiEvent>> events)
        {
            var ret = new List<GuitarBigRockEnding>();
            var breEvents = events.GetEventPairs(list, Utility.BigRockEndingData1);

            if (breEvents != null && breEvents.Any())
            {
                var breList = breEvents.GroupMidiEventPairByCloseTick(Utility.TickCloseWidth).Select(bre =>
                    new GuitarBigRockEnding(list, bre));

                foreach (var bre in breList.ToList())
                {
                    if (!bre.IsValid)
                    {
                        Remove(bre);
                    }
                    else
                    {
                        ret.Add(bre);
                    }
                }
            }
            return ret;
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

        public GuitarMessage CreateMessageByType(GuitarMessageList list, GuitarMessageType type, MidiEvent down, MidiEvent up = null)
        {
            GuitarMessage ret = null;
            switch (type)
            {
                case GuitarMessageType.GuitarHandPosition:
                    ret = new GuitarHandPosition(new MidiEventPair(list,down, up));
                    break;
                case GuitarMessageType.GuitarTextEvent:
                    ret = GuitarTextEvent.GetTextEvent(list, down);
                    break;
                case GuitarMessageType.GuitarTrainer:
                    ret = new GuitarTrainer(list, GuitarTrainerType.Unknown);
                    break;
                case GuitarMessageType.GuitarChord:
                    ret = GuitarChord.GetChord(list, list.Notes.Where(x => x.DownTick == down.AbsoluteTicks), true);
                    break;
                case GuitarMessageType.GuitarChordStrum:
                    ret = new GuitarChordStrum(list, down, up);
                    break;
                case GuitarMessageType.GuitarNote:
                    ret = new GuitarNote(list, down, up);
                    break;
                case GuitarMessageType.GuitarPowerup:
                    ret = new GuitarPowerup(list, down, up);
                    break;
                case GuitarMessageType.GuitarSolo:
                    ret = new GuitarSolo(list, down, up);
                    break;
                case GuitarMessageType.GuitarTempo:
                    ret = new GuitarTempo(list, down);
                    break;
                case GuitarMessageType.GuitarTimeSignature:
                    ret = new GuitarTimeSignature(list, down);
                    break;
                case GuitarMessageType.GuitarArpeggio:
                    ret = new GuitarArpeggio(list, down, up);
                    break;
                case GuitarMessageType.GuitarBigRockEndingSubMessage:
                case GuitarMessageType.GuitarBigRockEnding:
                    {
                        ret = list.BigRockEndings.SingleByDownTick(down.AbsoluteTicks);
                        if (ret == null)
                        {
                            ret = new GuitarBigRockEnding(list, new List<MidiEventPair>(new MidiEventPair(list, down, up).MakeEnumerable()));
                        }
                        else
                        {
                            ret.CastObject<GuitarBigRockEnding>().AddEvent(
                                new GuitarBigRockEndingSubMessage(ret.CastObject<GuitarBigRockEnding>(),
                                    new MidiEventPair(list, down, up)));
                        }
                    }
                    break;
                case GuitarMessageType.GuitarSingleStringTremelo:
                    ret = new GuitarSingleStringTremelo(list, down, up);
                    break;
                case GuitarMessageType.GuitarMultiStringTremelo:
                    ret = new GuitarMultiStringTremelo(list, down, up);
                    break;
                case GuitarMessageType.GuitarSlide:
                    ret = new GuitarSlide(new MidiEventPair(list, down, up));
                    break;
                case GuitarMessageType.GuitarHammeron:
                    ret = new GuitarHammeron(new MidiEventPair( list, down, up));
                    break;
            }
            return ret;
        }


        public GuitarMessageList GetMessages(GuitarDifficulty difficulty, bool includeDifficultyAll = true)
        {
            var ret = new GuitarMessageList(owner);

            var tempoTrack = GetTempoTrack();
            ret.AddRange(GetTempoMessages(ret, tempoTrack));
            ret.AddRange(GetTimeSigMessages(ret, tempoTrack));
            try
            {
                if (difficulty.IsAll())
                    difficulty = difficulty ^ GuitarDifficulty.All;

                var validData1List = Utility.GetKnownData1ForDifficulty(IsPro, 
                    includeDifficultyAll ? GuitarDifficulty.All | difficulty : difficulty).ToList();

                var events = midiTrack.ChanMessages.GroupByData1Channel(validData1List);

                if (IsPro)
                {
                    
                    ret.AddRange(events.GetEventPairs(ret, Utility.AllArpeggioData1).Select(x => new GuitarArpeggio(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.SoloData1).Select(x => new GuitarSolo(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.PowerupData1).Select(x => new GuitarPowerup(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.MultiStringTremeloData1).Select(x => new GuitarMultiStringTremelo(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.SingleStringTremeloData1).Select(x => new GuitarSingleStringTremelo(x)));
                    ret.AddRange(GetBigRockEndings(ret, events));
                    ret.AddRange(events.GetEventPairs(ret, Utility.AllSlideData1).Select(x => new GuitarSlide(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.AllHammeronData1).Select(x => new GuitarHammeron(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.AllStrumData1).Select(x => new GuitarChordStrum(x)));
                    var notes = events.GetEventPairs(ret, Utility.GetStringsForDifficulty6(difficulty)).Select(x => new GuitarNote(x));
                    ret.AddRange(notes);
                    if (notes.Any())
                    {
                        ret.AddRange(notes.GroupByCloseTick().ToList().Select(chordNotes => GuitarChord.GetChord(ret, chordNotes, true)).Where(x => x != null).ToList());
                    }
                    var textEvents = midiTrack.Meta.Where(x => x.IsTextEvent()).Select(x => GuitarTextEvent.GetTextEvent(ret, x));
                    ret.AddRange(textEvents);

                    ret.AddRange(LoadTrainers(ret, textEvents));
                    ret.AddRange(events.GetEventPairs(ret, Utility.HandPositionData1).Select(x => new GuitarHandPosition(x)));
                }
                else
                {
                    ret.AddRange(events.GetEventPairs(ret, new[] { Utility.ExpertSoloData1_G5, Utility.SoloData1 }).ToList().Select(x => new GuitarSolo(x)));
                    ret.AddRange(events.GetEventPairs(ret, Utility.PowerupData1).Select(x => new GuitarPowerup(x)));
                    var notes = events.GetEventPairs(ret, Utility.GetStringsForDifficulty5(difficulty)).Select(x => new GuitarNote(x));
                    ret.AddRange(notes);
                    if (notes.Any())
                    {
                        ret.AddRange(notes.GroupByCloseTick().ToList().Select(x => GuitarChord.GetChord(ret, x, false)).Where(x => x != null));
                    }
                    
                    var textEvents = midiTrack.Meta.Where(x => x.IsTextEvent()).ToList().Select(x => GuitarTextEvent.GetTextEvent(ret, x));
                    ret.AddRange(textEvents);
                }
            }
            catch { }
            return ret;
        }

        bool LoadEvents6()
        {
            bool ret = true;
            try
            {
                Messages = null;


                checkForInvalidNotes(midiTrack);

                Messages = GetMessages(CurrentDifficulty);
                NoteGrid = new NoteGrid(owner);

                /*
                var tempoTrack = GetTempoTrack();
                Messages.AddRange(GetTempoFromTrack(tempoTrack));
                Messages.AddRange(GetTimeSigMessages(tempoTrack));

                NoteGrid = new NoteGrid(owner);

                var events = midiTrack.ChanMessages.GroupByData1Channel(
                    Utility.GetKnownData1ForDifficulty(IsPro, CurrentDifficulty | GuitarDifficulty.All));

                Messages.AddRange(events.GetEventPairs(this, Utility.HandPositionData1).Select(x => new GuitarHandPosition(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllArpeggioData1).Select(x => new GuitarArpeggio(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.SoloData1).Select(x => new GuitarSolo(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.PowerupData1).Select(x => new GuitarPowerup(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.MultiStringTremeloData1).Select(x => new GuitarMultiStringTremelo(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.SingleStringTremeloData1).Select(x => new GuitarSingleStringTremelo(this, x.Down, x.Up)));
                Messages.AddRange(GetBigRockEndings(events));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllSlideData1).Select(x => new GuitarSlide(this, x.Channel == Utility.ChannelSlideReversed, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllHammeronData1).Select(x => new GuitarHammeron(this, x.Down, x.Up)));
                Messages.AddRange(events.GetEventPairs(this, Utility.AllStrumData1).Select(x => new GuitarChordStrum(this, x.Down, x.Up, x.Channel.GetChordStrumFromChannel().GetModifierType())));
                var notes = events.GetEventPairs(this, Utility.GetStringsForDifficulty6(CurrentDifficulty)).Select(x => new GuitarNote(this, x.Down, x.Up));
                Messages.AddRange(notes);
                Messages.AddRange(notes.GroupByCloseTick().ToList().Select(chordNotes => GuitarChord.GetChord(this, chordNotes, true)).ToList());
                var textEvents = midiTrack.Meta.Where(x => x.IsTextEvent() && x.MetaMessage.Text.GetGuitarTrainerMetaEventType() == GuitarTrainerMetaEventType.Unknown)
                    .Select(x => GuitarTextEvent.GetTextEvent(this, x));
                Messages.AddRange(textEvents);

                Messages.AddRange(LoadTrainers(textEvents));
                */
                ret = true;

                this.dirtyItems = DirtyItem.None;

            }
            catch { ret = false; }
            return ret;
        }



        public void AddTrainer(GuitarTrainer tr)
        {
            if (tr.TrainerIndex.IsNull())
            {
                tr.TrainerIndex = Messages.Trainers.Count(x => x.TrainerType == tr.TrainerType) + 1;

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
            return Messages.Trainers.Where(x => x.TrainerType == type && x.TrainerIndex == index).FirstOrDefault();
        }

        public IEnumerable<GuitarTrainer> LoadTrainers(GuitarMessageList list, IEnumerable<GuitarTextEvent> te)
        {
            var ret = new List<GuitarTrainer>();

            try
            {
                var textEvents = te.Where(v => v.IsTrainerEvent).ToList();

                list.Remove(textEvents.Where(v => v.Text.GetTrainerEventIndex().IsNull()).ToList());

                list.Trainers.Clear();

                var groups = textEvents.Where(v => v.IsDeleted == false).ToList().GroupBy(v => v.TrainerType.IsTrainerGuitar());
                foreach (var group in groups)
                {
                    var indexGroups = group.OrderBy(v => v.Text.GetTrainerEventIndex()).GroupBy(v => v.Text.GetTrainerEventIndex()).ToList();

                    foreach (var trainer in indexGroups)
                    {
                        var index = trainer.Key;

                        var begin = trainer.Where(v => v.TrainerType == GuitarTrainerMetaEventType.BeginProGuitar || v.TrainerType == GuitarTrainerMetaEventType.BeginProBass);
                        var end = trainer.Where(v => v.TrainerType == GuitarTrainerMetaEventType.EndProGuitar || v.TrainerType == GuitarTrainerMetaEventType.EndProBass);
                        var norm = trainer.Where(v => v.TrainerType == GuitarTrainerMetaEventType.ProGuitarNorm || v.TrainerType == GuitarTrainerMetaEventType.ProBassNorm);

                        if (begin.Any() && end.Any())
                        {
                            var trainerType = group.Key == true ? GuitarTrainerType.ProGuitar : GuitarTrainerType.ProBass;
                            
                            var gt = new GuitarTrainer(list, trainerType);
                            
                            gt.Start = begin.First();
                            gt.End = end.First();
                            if (norm.Any())
                            {
                                gt.Norm = norm.First();
                            }
                            ret.Add(gt);


                            list.Remove(begin.Skip(1));
                            list.Remove(end.Skip(1));
                            if (norm.Any())
                            {
                                list.Remove(norm.Skip(1));
                            }
                        }
                        else
                        {
                            list.Remove(trainer);
                        }
                    }
                }
            }
            catch { }

            return ret.Where(v=> v.IsDeleted==false).ToList();
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
                
                CurrentDifficulty = GuitarDifficulty.Expert;
                Remove(Messages.HandPositions.ToList());
                GuitarHandPosition.CreateEvent(Messages, 
                    new TickPair(Utility.HandPositionMarkerFirstBeginOffset, Utility.HandPositionMarkerFirstEndOffset), 0);

                if (!Utility.HandPositionMarkerByDifficulty)
                {
                    if (Messages.Chords.Any(x => x.HighestFret >= Utility.HandPositionMarkerMinFret))
                    {
                        var x108 = Generate108(config);


                        var sorted = x108.ToList().OrderBy(x => x.Ticks.Down).ToList();
                        int lastUp = -1;
                        for (int x = 0; x < sorted.Count; x++)
                        {
                            var item = sorted[x];
                            if (item.Ticks.Down >= lastUp)
                            {
                                lastUp = item.Ticks.Up;
                                GuitarHandPosition.CreateEvent(Messages, item.Ticks, item.Fret);
                            }
                        }
                        
                    }
                }
                else
                {
                    var currDiff = CurrentDifficulty;

                    var ex = GetChordsByDifficulty(GuitarDifficulty.Expert);
                    var ha = GetChordsByDifficulty(GuitarDifficulty.Hard);
                    var me = GetChordsByDifficulty(GuitarDifficulty.Medium);
                    var ea = GetChordsByDifficulty(GuitarDifficulty.Easy);

                    bool hasAboveMin =
                        ex.Any(adc => adc.HighestFret > Utility.HandPositionMarkerMinFret) ||
                        ha.Any(adc => adc.HighestFret > Utility.HandPositionMarkerMinFret) ||
                        me.Any(adc => adc.HighestFret > Utility.HandPositionMarkerMinFret) ||
                        ea.Any(adc => adc.HighestFret > Utility.HandPositionMarkerMinFret);


                    if (hasAboveMin)
                    {
                        bool isGuitar = IsGuitarTrackName(Name);

                        if (isGuitar)
                        {
                            if (config.EnableProGuitarHard == false)
                            {
                                foreach (var n in ha.ToList())
                                {
                                    if (ex.HasDownTickAtTick(n.DownTick) &&
                                        !ex.HasDownTickAtTick(n.DownTick - 1))
                                    {
                                        n.SetTicks(n.TickPair - 1);
                                        n.UpdateEvents();
                                    }
                                }

                            }
                            if (config.EnableProGuitarMedium == false)
                            {
                                foreach (var n in me.ToList())
                                {
                                    if ((ha.HasDownTickAtTick(n.DownTick) ||
                                        ex.HasDownTickAtTick(n.DownTick)) &&
                                        !(ex.HasDownTickAtTick(n.DownTick - 2) ||
                                          ha.HasDownTickAtTick(n.DownTick - 2)))
                                    {
                                        n.SetTicks(n.TickPair - 2);
                                        n.UpdateEvents();
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
                                        hn.SetTicks(hn.TickPair - 3);
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
                                        hn.SetTicks(hn.TickPair - 1);
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
                                        hn.SetTicks(hn.TickPair - 2);
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
                                        hn.SetTicks(hn.TickPair - 3);
                                        hn.UpdateEvents();
                                    }
                                }
                            }
                        }



                        CurrentDifficulty = GuitarDifficulty.Expert;
                        var x108 = Generate108(config);

                        CurrentDifficulty = GuitarDifficulty.Hard;
                        var h108 = Generate108(config);

                        CurrentDifficulty = GuitarDifficulty.Medium;
                        var m108 = Generate108(config);

                        CurrentDifficulty = GuitarDifficulty.Easy;
                        var e108 = Generate108(config);

                        CurrentDifficulty = GuitarDifficulty.Expert;

                        var items = new List<GuitarHandPositionMeta>();
                        items.AddRange(x108);
                        items.AddRange(h108);
                        items.AddRange(m108);
                        items.AddRange(e108);

                        var sorted = items.OrderBy(x => x.Ticks.Down).ToList();
                        int lastUp = -1;
                        for (int x = 0; x < sorted.Count; x++)
                        {
                            var item = sorted[x];
                            if (item.Ticks.Down >= lastUp)
                            {
                                lastUp = item.Ticks.Up;
                                GuitarHandPosition.CreateEvent(Messages, item.Ticks, item.Fret);
                            }
                        }

                        
                    }
                }

                
                CurrentDifficulty = GuitarDifficulty.Expert;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public class GuitarHandPositionMeta
        {
            public TickPair Ticks;
            public int Fret;
        }

        private IEnumerable<GuitarHandPositionMeta> Generate108(GenDiffConfig config)
        {
            var ret = new List<GuitarHandPositionMeta>();
            int last108Fret = -1;
            int last108UpTick = 40;

            foreach (var chord in Messages.Chords.ToList())
            {
                int lowestNonZero = chord.LowestNonZeroFret;
                var highestFret = chord.HighestFret;

                var noteFret = 0;
                if (highestFret >= Utility.HandPositionMarkerMinFret)
                {
                    noteFret = lowestNonZero;
                    if (noteFret > Utility.HandPositionMarkerMaxFret)
                        noteFret = Utility.HandPositionMarkerMaxFret;
                }
                
                if (noteFret != last108Fret)
                {
                    var down = chord.DownTick + Utility.HandPositionMarkerStartOffset;
                    var up = chord.UpTick + Utility.HandPositionMarkerEndOffset;

                    if (up <= down)
                        up = down + 1;

                    if (down >= last108UpTick)
                    {
                        last108Fret = noteFret;

                        last108UpTick = up;


                        ret.Add(new GuitarHandPositionMeta() { Ticks = new TickPair(down, up), Fret = noteFret });
                    }
                }
            }
            return ret;
        }

    }
}
