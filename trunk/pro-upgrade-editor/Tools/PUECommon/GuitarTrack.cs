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

                message.RemoveFromList();
            }
        }

        public void Remove(MidiEventPair ev)
        {
            if (ev.Down != null && !ev.Down.Deleted)
            {
                midiTrack.Remove(ev.Down);
            }
            if (ev.Up != null && !ev.Up.Deleted)
            {
                midiTrack.Remove(ev.Up);
            }
        }

        public void Remove(MidiEvent ev)
        {
            midiTrack.Remove(ev);
        }

        public void Remove(IEnumerable<MidiEvent> ev)
        {
            foreach (var e in ev)
            {
                Remove(e);
            }
        }
        public void Remove(IEnumerable<MidiEventPair> ev)
        {
            foreach (var e in ev)
            {
                if (e.HasDown)
                    Remove(e.Down);
                if (e.HasUp)
                    Remove(e.Up);
            }
        }

        public MidiEvent Insert(int absoluteTicks, IMidiMessage ev)
        {
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

        public IEnumerable<GuitarTempo> GetTempoMessages(GuitarMessageList owner, Track tempoTrack)
        {
            var ret = new List<GuitarTempo>();
            try
            {
                var tempos = internalGetTempo(owner, tempoTrack);

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

        IEnumerable<GuitarTimeSignature> GetTimeSignaturesFromTrack(GuitarMessageList owner, Track tempoTrack)
        {
            var ret = new List<GuitarTimeSignature>();
            if (tempoTrack != null && tempoTrack.TimeSig.Any())
            {
                ret.AddRange(tempoTrack.TimeSig.Select(t => new GuitarTimeSignature(owner, t)));
            }
            else
            {
                ret.Add(GuitarTimeSignature.GetDefaultTimeSignature(owner));
            }
            return ret;
        }


        public double TotalSongTime
        {
            get
            {
                return TickToTime(midiTrack.Events.Last().AbsoluteTicks);
            }
        }
        public int TotalSongTicks
        {
            get
            {
                return midiTrack.Events.Last().AbsoluteTicks;
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

                if (track.Name.IsGuitarTrackName6())
                {
                    var bassTrainers = track.Meta.Where(x => x.Text.IsTrainerBass()).ToList();
                    var guitarTrainers = track.Meta.Where(x => x.Text.IsTrainerGuitar()).ToList();

                    if (bassTrainers.Any())
                    {
                        bassTrainers.ForEach(x => x.Owner.Remove(x));
                    }
                }

                if (track.Name.IsBassTrackName6())
                {
                    var bassTrainers = track.Meta.Where(x => x.Text.IsTrainerBass()).ToList();

                    var guitarTrainers = track.Meta.Where(x => x.Text.IsTrainerGuitar()).ToList();
                    if (guitarTrainers.Any())
                    {
                        guitarTrainers.ForEach(x => x.Owner.Remove(x));
                    }
                }

                if (track.Name.IsBassTrackName6() || track.Name.IsGuitarTrackName6())
                {
                    var distinctData1 = track.ChanMessages.Select(x => x.Data1).Distinct().ToList();


                    foreach (var data1 in distinctData1)
                    {
                        var msgs = track.ChanMessages.Where(x => x.Data1 == data1).GroupByMatchingTicks().ToList();

                        var expectingOn = true;
                        int onTick = Int32.MinValue;
                        int offTick = Int32.MinValue;

                        var invalidEvents = new List<MidiEvent>();
                        foreach (var item in msgs)
                        {

                            var expected = item.FirstOrDefault(x => x.IsOn == expectingOn);
                            if (expected == null)
                            {
                                track.Remove(item);
                            }
                            else
                            {
                                if (item.Count() == 1)
                                {
                                    if (expectingOn)
                                    {
                                        onTick = expected.AbsoluteTicks;
                                    }
                                    else
                                    {
                                        offTick = expected.AbsoluteTicks;
                                        if (offTick > onTick)
                                        {
                                        }
                                        else
                                        {
                                        }
                                    }
                                    expectingOn = !expectingOn;

                                }
                                else
                                {
                                    var offItem = item.FirstOrDefault(x => x.IsOff);
                                    var onItem = item.FirstOrDefault(x => x.IsOn);

                                    if (expectingOn == false)
                                    {
                                        if (onItem != null && offItem != null && item.IndexOf(offItem) < item.IndexOf(onItem))
                                        {
                                        }
                                        else
                                        {
                                            track.Remove(item.Where(x => x != offItem && x != onItem));
                                            if (onItem != null)
                                            {
                                                var c1 = offItem.Clone();
                                                var c2 = onItem.Clone();
                                                track.Remove(offItem);
                                                track.Remove(onItem);
                                                var new1 = track.Insert(offItem.AbsoluteTicks, c1);
                                                var new2 = track.Insert(offItem.AbsoluteTicks, c2);
                                            }
                                            else
                                            {
                                                expectingOn = !expectingOn;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        track.Remove(item.Where(x => x != onItem));
                                        expectingOn = !expectingOn;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch { }
        }


        void BuildMessages5()
        {
            GetMessages(CurrentDifficulty);

            NoteGrid = new NoteGrid(owner);

        }

        GuitarBigRockEnding GetBigRockEnding(GuitarMessageList owner, IEnumerable<IGrouping<Data1ChannelPair, MidiEvent>> events)
        {
            GuitarBigRockEnding ret = null;
            var data1List = Utility.GetBigRockEndingData1(owner.IsPro);

            var breEvents = events.GetEventPairs(owner, data1List).ToList();

            if (breEvents != null && breEvents.Any())
            {
                var ticks = breEvents.GetTickPair();

                if (breEvents.Count() != data1List.Count())
                {
                    breEvents.ToList().ForEach(x => owner.Remove(x));

                    ret = GuitarBigRockEnding.CreateBigRockEnding(owner, ticks);
                }
                else
                {
                    ret = new GuitarBigRockEnding(owner, ticks, breEvents);
                    ret.AddToList();
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
                    currentDifficulty = value;
                    RebuildEvents();
                }
            }
        }


        public void GetMessages(GuitarDifficulty difficulty, bool includeDifficultyAll = true)
        {
            if (midiTrack.Dirty)
            {
                checkForInvalidNotes(midiTrack);
            }
            Messages = new GuitarMessageList(owner);
            var ret = Messages;
            var tempoTrack = GetTempoTrack();
            ret.AddRange(GetTempoMessages(ret, tempoTrack));
            ret.AddRange(GetTimeSigMessages(ret, tempoTrack));
            try
            {
                if (difficulty.IsAll())
                    difficulty = difficulty ^ GuitarDifficulty.All;

                var validData1List = Utility.GetKnownData1ForDifficulty(IsPro,
                    includeDifficultyAll ? GuitarDifficulty.All | difficulty : difficulty).ToList();

                var events = midiTrack.ChanMessages.GroupByData1Channel(validData1List).ToList();

                if (IsPro)
                {

                    events.GetEventPairs(ret, Utility.AllArpeggioData1).Select(x => new GuitarArpeggio(x)).ToList().ForEach(x => x.AddToList());

                    events.GetEventPairs(ret, Utility.SoloData1.MakeEnumerable()).Select(x => new GuitarSolo(x)).ToList().ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.PowerupData1.MakeEnumerable()).Select(x => new GuitarPowerup(x)).ToList().ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.MultiStringTremeloData1.MakeEnumerable()).Select(x => new GuitarMultiStringTremelo(x)).ToList().ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.SingleStringTremeloData1.MakeEnumerable()).Select(x => new GuitarSingleStringTremelo(x)).ToList().ForEach(x => x.AddToList());

                    GetBigRockEnding(ret, events);

                    events.GetEventPairs(ret, Utility.AllSlideData1).Select(x => new GuitarSlide(x)).ToList().ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.AllHammeronData1).Select(x => new GuitarHammeron(x)).ToList().ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.AllStrumData1).Select(x => new GuitarChordStrum(x)).ToList().ForEach(x => x.AddToList());

                    var notes = events.GetEventPairs(ret, Utility.GetStringsForDifficulty6(difficulty)).Select(x => new GuitarNote(x)).ToList();
                    notes.ForEach(x => x.AddToList());
                    if (notes.Any())
                    {
                        var closeNotes = notes.GroupByCloseTick().ToList();
                        var chordNotes = closeNotes.Select(n => GuitarChord.GetChord(ret, difficulty, n, true)).Where(x => x != null).ToList();
                        chordNotes.ForEach(x => x.AddToList());
                    }

                    var textEvents = midiTrack.Meta.Where(x => x.IsTextEvent()).Select(x => new GuitarTextEvent(ret, x)).ToList();
                    textEvents.ForEach(x => x.AddToList());

                    LoadTrainers(ret, ret.TextEvents).ToList().ForEach(x => x.AddToList());

                    events.GetEventPairs(ret, Utility.HandPositionData1.MakeEnumerable()).Select(x => new GuitarHandPosition(x)).ToList().ForEach(x => x.AddToList());
                }
                else
                {
                    events.GetEventPairs(ret, new[] { Utility.ExpertSoloData1_G5, Utility.SoloData1 }).ToList().Select(x => new GuitarSolo(x)).ForEach(x => x.AddToList());
                    events.GetEventPairs(ret, Utility.PowerupData1.MakeEnumerable()).Select(x => new GuitarPowerup(x)).ForEach(x => x.AddToList());

                    var notes = events.GetEventPairs(ret, Utility.GetStringsForDifficulty5(difficulty)).Select(x => new GuitarNote(x)).ToList();
                    notes.ForEach(x => x.AddToList());

                    if (notes.Any())
                    {
                        var closeNotes = notes.GroupByCloseTick().ToList();
                        var chordNotes = closeNotes.Select(x => GuitarChord.GetChord(ret, difficulty, x, false)).Where(x => x != null).ToList();
                        chordNotes.ForEach(x => x.AddToList());
                    }

                    var textEvents = midiTrack.Meta.Where(x => x.IsTextEvent()).ToList().Select(x => new GuitarTextEvent(ret, x)).ToList();
                    textEvents.ForEach(x => x.AddToList());

                    GetBigRockEnding(ret, events);
                }

            }
            catch { }

        }

        bool LoadEvents6()
        {
            bool ret = true;
            try
            {
                GetMessages(CurrentDifficulty);
                NoteGrid = new NoteGrid(owner);

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

        public GuitarTrack SetTrack(Track selectedTrack, GuitarDifficulty diff)
        {
            if (selectedTrack == null || midiTrack == null)
            {
                midiTrack = null;
                dirtyItems |= DirtyItem.Track;
            }
            if (selectedTrack != null)
            {
                if (midiTrack == selectedTrack && !midiTrack.Dirty && currentDifficulty == diff && dirtyItems == DirtyItem.None)
                {
                }
                else
                {
                    midiTrack = selectedTrack;
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
                    selectedTrack.Dirty = false;
                }
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

        public IEnumerable<GuitarTrainer> LoadTrainers(GuitarMessageList owner, IEnumerable<GuitarTextEvent> textEventList)
        {
            var ret = new List<GuitarTrainer>();

            try
            {
                var trainerEvents = textEventList.Where(v => v.IsTrainerEvent).ToList();
                var validTrainerEvents = trainerEvents.Where(x => x.Text.GetTrainerEventIndex().IsNotNull());

                var invalidTrainerEvents = trainerEvents.Where(v => v.Text.GetTrainerEventIndex().IsNull());
                invalidTrainerEvents.ToList().ForEach(x => x.DeleteAll());



                var removeEvents = new List<GuitarMessage>();

                var groups = validTrainerEvents.ToList().GroupBy(v => v.TrainerType.IsTrainerGuitar());

                foreach (var group in groups)
                {
                    var indexGroups = group.OrderBy(v => v.Text.GetTrainerEventIndex()).GroupBy(v => v.Text.GetTrainerEventIndex());

                    foreach (var trainer in indexGroups.ToList())
                    {
                        var index = trainer.Key;

                        var begin = trainer.Where(v => v.TrainerType.IsTrainerBegin());
                        var end = trainer.Where(v => v.TrainerType.IsTrainerEnd());
                        var norm = trainer.Where(v => v.TrainerType.IsTrainerNorm());

                        if (begin.Any() && end.Any())
                        {
                            var trainerType = group.Key == true ? GuitarTrainerType.ProGuitar : GuitarTrainerType.ProBass;

                            var startEv = begin.First();
                            var endEv = end.First();
                            var normEv = norm.FirstOrDefault();

                            var gt = new GuitarTrainer(owner, new TickPair(startEv.AbsoluteTicks, endEv.AbsoluteTicks), trainerType, startEv, endEv, normEv);

                            ret.Add(gt);

                            removeEvents.AddRange(begin.Skip(1).ToList());
                            removeEvents.AddRange(end.Skip(1).ToList());

                            if (norm.Any())
                            {
                                removeEvents.AddRange(norm.Skip(1).ToList());
                            }
                        }
                        else
                        {
                            removeEvents.AddRange(trainer.ToList());
                        }
                    }
                }
                removeEvents.ToList().ForEach(x => x.DeleteAll());
            }
            catch { }


            return ret.Where(v => v.IsDeleted == false).ToList();
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
                Remove(Messages.HandPositions.ToList());

                var x108 = Generate108(config);

                var sorted = x108.ToList().OrderBy(x => x.Ticks.Down).ToList();

                for (int x = 0; x < sorted.Count; x++)
                {
                    var item = sorted[x];

                    GuitarHandPosition.CreateEvent(Messages, item.Ticks, item.Fret);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }



        private IEnumerable<GuitarHandPositionMeta> Generate108(GenDiffConfig config)
        {
            var ret = new List<GuitarHandPositionMeta>();

            var sorted = Messages.Chords.OrderBy(x => x.DownTick).ToList();

            ret.Add(new GuitarHandPositionMeta()
            {
                Fret = 0,
                Ticks = new TickPair(Utility.HandPositionMarkerFirstBeginOffset, Utility.HandPositionMarkerFirstEndOffset),
                IsChord = false,
            });


            if (sorted.Any())
            {
                foreach (var chord in sorted)
                {
                    int lowestNonZero = chord.LowestNonZeroFret;
                    var highestFret = chord.HighestFret;

                    ret.Add(new GuitarHandPositionMeta()
                    {
                        Ticks = chord.TickPair,
                        Fret = lowestNonZero,
                        IsChord = chord.NoteFrets.Count() > 1
                    });
                }

                var retArray = ret.ToArray();

                ret = new List<GuitarHandPositionMeta>();

                ret.Add(retArray.First());

                int lastFret = retArray.First().Fret;
                bool lastIsChord = false;

                for (int x = 1; x < retArray.Length; x++)
                {
                    var cur = retArray[x];
                    if ((cur.Fret != lastFret) || cur.IsChord || (cur.IsChord != lastIsChord))
                    {
                        ret.Add(cur);
                        lastFret = cur.Fret;
                        lastIsChord = cur.IsChord;
                    }
                }

            }
            return ret;
        }

    }

}
