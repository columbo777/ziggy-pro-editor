using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{
    
    public class GuitarMessageList : IEnumerable<GuitarMessage>
    {

        protected IEnumerable<GuitarMessageType> GetAllMessageTypes()
        {
            return new GuitarMessageType[]
            {
                GuitarMessageType.GuitarHandPosition,
                GuitarMessageType.GuitarChordName,
                GuitarMessageType.GuitarTextEvent,
                GuitarMessageType.GuitarTrainer,
                GuitarMessageType.GuitarChord,
                GuitarMessageType.GuitarChordStrum,
                GuitarMessageType.GuitarNote,
                GuitarMessageType.GuitarPowerup,
                GuitarMessageType.GuitarSolo,
                GuitarMessageType.GuitarTempo,
                GuitarMessageType.GuitarTimeSignature,
                GuitarMessageType.GuitarArpeggio,
                GuitarMessageType.GuitarBigRockEnding,
            
                GuitarMessageType.GuitarSingleStringTremelo,
                GuitarMessageType.GuitarMultiStringTremelo,
                GuitarMessageType.GuitarSlide,
                GuitarMessageType.GuitarHammeron,
            };
        }

        public IEnumerable<GuitarMessage> GetByDifficulty(GuitarDifficulty diff)
        {
            var ret = new List<GuitarMessage>();
            
            foreach (var messageType in GetAllMessageTypes())
            {
                switch (messageType)
                {
                    case GuitarMessageType.GuitarHandPosition:
                        ret.AddRange(HandPositions.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;
                    case GuitarMessageType.GuitarChordName:
                        ret.AddRange(ChordNames.Where(x => diff.HasFlag(x.Difficulty)));
                        break;
                    case GuitarMessageType.GuitarTextEvent:
                        ret.AddRange(TextEvents.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarTrainer:
                        ret.AddRange(Trainers.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;
                    case GuitarMessageType.GuitarChordStrum:
                        ret.AddRange(ChordStrums.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;
                    case GuitarMessageType.GuitarChord:
                        ret.AddRange(Chords.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarNote:
                        ret.AddRange(Notes.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarPowerup:
                        ret.AddRange(Powerups.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarSolo:
                        ret.AddRange(Solos.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarTempo:
                        ret.AddRange(Tempos.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarTimeSignature:
                        ret.AddRange(TimeSignatures.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarArpeggio:
                        ret.AddRange(Arpeggios.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarBigRockEnding:
                        ret.AddRange(BigRockEndings.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;
                    case GuitarMessageType.GuitarBigRockEndingSubMessage:
                        
                        break;
                    case GuitarMessageType.GuitarSingleStringTremelo:
                        ret.AddRange(SingleStringTremelos.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarMultiStringTremelo:
                        ret.AddRange(MultiStringTremelos.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarSlide:
                        ret.AddRange(Slides.Where(x=> diff.HasFlag(x.Difficulty)));
                        break;

                    case GuitarMessageType.GuitarHammeron:
                        ret.AddRange(Hammerons.Where(x => diff.HasFlag(x.Difficulty)));
                        break;
                    default:
                        ("unknown message: " + messageType).OutputDebug();
                        break;
                }
            }

            return ret;
        }

        public ChordNameList ChordNames;
        public GuitarHandPositionList HandPositions;
        public GuitarTextEventList TextEvents;
        public GuitarTrainerList Trainers;

        public GuitarPowerupList Powerups;
        public GuitarSoloList Solos;
        public GuitarTempoList Tempos;
        public GuitarTimeSignatureList TimeSignatures;
        public GuitarArpeggioList Arpeggios;
        public GuitarBigRockEndingList BigRockEndings;
        public GuitarSingleStringTremeloList SingleStringTremelos;
        public GuitarMultiStringTremeloList MultiStringTremelos;

        public GuitarChordList Chords;
        public GuitarNoteList Notes;
        public GuitarSlideList Slides;
        public GuitarHammeronList Hammerons;
        public ChordStrumList ChordStrums;

        TrackEditor owner;
        public TrackEditor Owner { get { return owner; } }

        public bool IsPro { get { return owner.IsPro; } }

        public GuitarMessageList(TrackEditor owner)
        {
            this.owner = owner;
            ChordNames = new ChordNameList(owner);
            HandPositions = new GuitarHandPositionList(owner);
            TextEvents = new GuitarTextEventList(owner);
            Trainers = new GuitarTrainerList(owner);
            Chords = new GuitarChordList(owner);
            Notes = new GuitarNoteList(owner);

            Tempos = new GuitarTempoList(owner);
            TimeSignatures = new GuitarTimeSignatureList(owner);

            Arpeggios = new GuitarArpeggioList(owner);
            BigRockEndings = new GuitarBigRockEndingList(owner);
            SingleStringTremelos = new GuitarSingleStringTremeloList(owner);
            MultiStringTremelos = new GuitarMultiStringTremeloList(owner);

            Powerups = new GuitarPowerupList(owner);
            Solos = new GuitarSoloList(owner);

            Slides = new GuitarSlideList(owner);
            Hammerons = new GuitarHammeronList(owner);
            ChordStrums = new ChordStrumList(owner);
        }

        public IEnumerable<GuitarModifier> GetModifiersByType(GuitarModifierType type)
        {
            var ret = new List<GuitarModifier>();
            switch (type)
            {
                case GuitarModifierType.Arpeggio:
                    ret.AddRange(Arpeggios.Cast<GuitarModifier>());
                    break;
                case GuitarModifierType.BigRockEnding:
                    ret.AddRange(BigRockEndings.Cast<GuitarModifier>());
                    break;
                case GuitarModifierType.Powerup:
                    ret.AddRange(Powerups.Cast<GuitarModifier>());
                    break;
                case GuitarModifierType.Solo:
                    ret.AddRange(Solos.Cast<GuitarModifier>());
                    break;
                case GuitarModifierType.MultiStringTremelo:
                    ret.AddRange(MultiStringTremelos.Cast<GuitarModifier>());
                    break;
                case GuitarModifierType.SingleStringTremelo:
                    ret.AddRange(SingleStringTremelos.Cast<GuitarModifier>());
                    break;

            }

            return ret.ToList();
        }

        public IEnumerable<ChordModifier> GetChordModifiersByType(ChordModifierType type)
        {
            var ret = new List<ChordModifier>();
            switch (type)
            {
                case ChordModifierType.Slide:
                    ret.AddRange(Slides);
                    break;
                case ChordModifierType.SlideReverse:
                    ret.AddRange(Slides.Where(x => x.IsReversed));
                    break;
                case ChordModifierType.Hammeron:
                    ret.AddRange(Hammerons);
                    break;
                case ChordModifierType.ChordStrumLow:
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode.HasFlag(ChordStrum.Low)));
                    break;
                case ChordModifierType.ChordStrumMed:
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode.HasFlag(ChordStrum.Mid)));
                    break;
                case ChordModifierType.ChordStrumHigh:
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode.HasFlag(ChordStrum.High)));
                    break;
            }
            return ret;
        }

        public IEnumerable<GuitarModifier> GetModifiersBetweenTick(TickPair pair)
        {
            var ret = new List<GuitarModifier>();
            
            ret.AddRange(Arpeggios.GetBetweenTick(pair));
            ret.AddRange(BigRockEndings.GetBetweenTick(pair));
            ret.AddRange(Powerups.GetBetweenTick(pair));
            ret.AddRange(Solos.GetBetweenTick(pair));
            ret.AddRange(MultiStringTremelos.GetBetweenTick(pair));
            ret.AddRange(SingleStringTremelos.GetBetweenTick(pair));

            return ret.ToList();
        }



        public AdjustResult AdjustChordTicks(GuitarChord chord, TickPair newTicks, AdjustOption option)
        {
            AdjustResult result;
            var updatedTicks = GetAdjustChordTicks(newTicks, option, out result, chord);

            if (!result.HasFlag(AdjustResult.Error) && updatedTicks.IsNull == false)
            {
                chord.SetTicks(updatedTicks);
                chord.UpdateEvents();
            }
            return result;
        }

        public TickPair GetAdjustChordTicks(TickPair newTicks, AdjustOption option, out AdjustResult result, GuitarChord chord = null)
        {
            result = AdjustResult.NoResult;

            var existingTicks = new TickPair(newTicks.Down, newTicks.Up);

            var queryTicks = new TickPair(newTicks.Down, newTicks.Up);
            if (option.HasFlag(AdjustOption.AllowGrow) || option.HasFlag(AdjustOption.AllowShift))
            {
                queryTicks = new TickPair(newTicks.Down, newTicks.Up);
            }

            var between = Chords.GetBetweenTick(queryTicks);

            if (chord != null)
            {
                between = between.Where(x => x != chord);
            }

            if (!between.Any())
            {
                result = AdjustResult.Success;
                return newTicks;
            }
            else
            {
                var closeToBegin = between.Where(x => x.TickPair.IsCloseUpDown(newTicks));
                var closeToEnd = between.Where(x => x.TickPair.IsCloseDownUp(newTicks));
                var closeChords = closeToBegin.Concat(closeToEnd).Distinct();

                if (!closeChords.Any())
                {
                    result = AdjustResult.Success;
                    return newTicks;
                }
                else
                {
                    TickPair updatedTicks = new TickPair(newTicks.Down, newTicks.Up);

                    var min = closeToBegin.GetMaxTick(updatedTicks.Down);
                    var max = closeToEnd.GetMinTick(updatedTicks.Up);

                    var updatedLen = updatedTicks.Up - updatedTicks.Down;

                    var space = max - min;

                    if (space <= 0)
                    {
                        result = AdjustResult.Error;
                        return TickPair.NullValue;
                    }

                    if (updatedLen <= 0)
                    {
                        result = AdjustResult.Error;
                        return TickPair.NullValue;
                    }

                    if (updatedLen == space)
                    {
                        result = AdjustResult.Success;
                        return updatedTicks;
                    }
                    if ((updatedLen < space && option.HasFlag(AdjustOption.AllowGrow)) ||
                        (updatedLen > space && option.HasFlag(AdjustOption.AllowShrink)))
                    {
                        if (updatedTicks.Down < min)
                        {
                            result |= AdjustResult.AdjustedDownTickRight;
                        }
                        else if (updatedTicks.Down > min)
                        {
                            result |= AdjustResult.AdjustedDownTickLeft;
                        }
                        updatedTicks.Down = min;

                        if (updatedTicks.Up < max)
                        {
                            result |= AdjustResult.AdjustedUpTickRight;
                        }
                        else if (updatedTicks.Up > max)
                        {
                            result |= AdjustResult.AdjustedUpTickLeft;
                        }
                        updatedTicks.Up = max;

                        updatedLen = updatedTicks.Up - updatedTicks.Down;

                        if (updatedLen <= 0)
                        {
                            result = AdjustResult.Error;
                            return TickPair.NullValue;
                        }
                        else
                        {
                            if (updatedLen < Utility.NoteCloseWidth)
                            {
                                result |= AdjustResult.ShortResult;
                            }
                            result |= AdjustResult.Success;
                            return updatedTicks;
                        }

                    }
                    else if (updatedLen < space && option.HasFlag(AdjustOption.AllowShift))
                    {
                        if (updatedTicks.Down < min)
                        {
                            updatedTicks.Down = min;
                            updatedTicks.Up = min + updatedLen;
                            result |= AdjustResult.AdjustedDownTickRight | AdjustResult.AdjustedUpTickRight;
                        }
                        else
                        {
                            updatedTicks.Up = max;
                            updatedTicks.Down = max - updatedLen;
                            result |= AdjustResult.AdjustedDownTickLeft | AdjustResult.AdjustedUpTickLeft;
                        }

                        updatedLen = updatedTicks.Up - updatedTicks.Down;

                        if (updatedLen <= 0)
                        {
                            result = AdjustResult.Error;
                            return TickPair.NullValue;
                        }
                        else
                        {
                            if (updatedLen < Utility.NoteCloseWidth)
                            {
                                result |= AdjustResult.ShortResult;
                            }
                            result |= AdjustResult.Success;

                            return updatedTicks;
                        }
                    }
                    else
                    {
                        result = AdjustResult.Error;
                        return TickPair.NullValue;
                    }
                }
            }
        }

        public void AddRange<T>(IEnumerable<T> mess) where T : GuitarMessage
        {
            if (mess != null)
            {
                foreach (var x in mess)
                {
                    Add(x);
                }
            }
        }


        public virtual void Add<T>(T mess) where T : GuitarMessage
        {
            if (mess != null)
            {

                switch (mess.MessageType)
                {
                    case GuitarMessageType.GuitarHandPosition:
                        HandPositions.Add(mess);
                        break;
                    case GuitarMessageType.GuitarChordName:
                        ChordNames.Add(mess);
                        break;
                    case GuitarMessageType.GuitarTextEvent:
                        TextEvents.Add(mess);
                        break;

                    case GuitarMessageType.GuitarTrainer:
                        Trainers.Add(mess);
                        break;
                    case GuitarMessageType.GuitarChordStrum:
                        ChordStrums.Add(mess);
                        break;
                    case GuitarMessageType.GuitarChord:
                        Chords.Add(mess);
                        break;

                    case GuitarMessageType.GuitarNote:
                        Notes.Add(mess);
                        break;

                    case GuitarMessageType.GuitarPowerup:
                        Powerups.Add(mess);
                        break;

                    case GuitarMessageType.GuitarSolo:
                        Solos.Add(mess);
                        break;

                    case GuitarMessageType.GuitarTempo:
                        Tempos.Add(mess);
                        break;

                    case GuitarMessageType.GuitarTimeSignature:
                        TimeSignatures.Add(mess);
                        break;

                    case GuitarMessageType.GuitarArpeggio:
                        Arpeggios.Add(mess);
                        break;

                    case GuitarMessageType.GuitarBigRockEnding:
                        BigRockEndings.Add(mess);
                        break;
                    case GuitarMessageType.GuitarBigRockEndingSubMessage:
                        break;
                    case GuitarMessageType.GuitarSingleStringTremelo:
                        SingleStringTremelos.Add(mess);
                        break;

                    case GuitarMessageType.GuitarMultiStringTremelo:
                        MultiStringTremelos.Add(mess);
                        break;

                    case GuitarMessageType.GuitarSlide:
                        Slides.Add(mess);
                        break;

                    case GuitarMessageType.GuitarHammeron:
                        Hammerons.Add(mess);
                        break;
                    default:
                        ("unknown message: " + mess.MessageType).OutputDebug();
                        break;
                }
                
            }
        }

        public MidiEvent Insert(int absoluteTicks, IMidiMessage ev)
        {
            return owner.GuitarTrack.Insert(absoluteTicks, ev);
        }

        public MidiEventPair Insert(int data1, int data2, int channel, TickPair ticks)
        {
            return owner.GuitarTrack.Insert(data1, data2, channel, ticks);
        }

        public void Remove(GuitarMessage mess)
        {
            internalRemove(mess);
        }
        public void Remove(MidiEventPair pair)
        {
            owner.GuitarTrack.Remove(pair);
        }
        public void Remove(IEnumerable<GuitarMessage> mess)
        {
            foreach (var m in mess.ToList())
                internalRemove(m);
        }
        internal void internalRemove<T>(T mess) where T : GuitarMessage
        {
            if (mess != null)
            {

                switch (mess.MessageType)
                {
                    case GuitarMessageType.GuitarHandPosition:
                        HandPositions.Remove(mess);
                        break;
                    case GuitarMessageType.GuitarChordName:
                        ChordNames.Remove(mess);
                        break;
                    case GuitarMessageType.GuitarTextEvent:
                        TextEvents.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarTrainer:
                        Trainers.Remove(mess);
                        break;
                    case GuitarMessageType.GuitarChordStrum:
                        ChordStrums.Remove(mess);
                        break;
                    case GuitarMessageType.GuitarChord:
                        Chords.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarNote:
                        Notes.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarPowerup:
                        Powerups.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarSolo:
                        Solos.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarTempo:
                        Tempos.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarTimeSignature:
                        TimeSignatures.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarArpeggio:
                        Arpeggios.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarBigRockEnding:
                        BigRockEndings.Remove(mess);
                        break;
                    case GuitarMessageType.GuitarBigRockEndingSubMessage:
                        break;
                    case GuitarMessageType.GuitarSingleStringTremelo:
                        SingleStringTremelos.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarMultiStringTremelo:
                        MultiStringTremelos.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarSlide:
                        Slides.Remove(mess);
                        break;

                    case GuitarMessageType.GuitarHammeron:
                        Hammerons.Remove(mess);
                        break;
                    default:
                        ("unknown message: " + mess.MessageType).OutputDebug();
                        break;
                }
            }
        }

        public IEnumerator<T> GetMessagesByType<T>(IEnumerable<GuitarMessageType> types) where T : GuitarMessage
        {
            foreach (var messageType in types)
            {
                switch (messageType)
                {
                    case GuitarMessageType.GuitarHandPosition:
                        {
                            var enumer = HandPositions.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;
                    case GuitarMessageType.GuitarChordName:
                        {
                            var enumer = ChordNames.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;
                    case GuitarMessageType.GuitarTextEvent:
                        {
                            var enumer = TextEvents.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarTrainer:
                        {
                            var enumer = Trainers.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;
                    case GuitarMessageType.GuitarChordStrum:
                        {
                            var enumer = ChordStrums.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;
                    case GuitarMessageType.GuitarChord:
                        {
                            var enumer = Chords.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarNote:
                        {
                            var enumer = Notes.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarPowerup:
                        {
                            var enumer = Powerups.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarSolo:
                        {
                            var enumer = Solos.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarTempo:
                        {
                            var enumer = Tempos.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarTimeSignature:
                        {
                            var enumer = TimeSignatures.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarArpeggio:
                        {
                            var enumer = Arpeggios.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarBigRockEnding:
                        {
                            var enumer = BigRockEndings.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarBigRockEndingSubMessage:
                        
                        break;

                    case GuitarMessageType.GuitarSingleStringTremelo:
                        {
                            var enumer = SingleStringTremelos.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarMultiStringTremelo:
                        {
                            var enumer = MultiStringTremelos.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarSlide:
                        {
                            var enumer = Slides.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;

                    case GuitarMessageType.GuitarHammeron:
                        {
                            var enumer = Hammerons.GetEnumerator();
                            while (enumer.MoveNext())
                            {
                                yield return enumer.Current as T;
                            }
                        }
                        break;
                    default:
                        ("unknown message: " + messageType).OutputDebug();
                        break;
                }
            }
        }

        public IEnumerator<GuitarMessage> GetEnumerator()
        {
            return GetMessagesByType<GuitarMessage>(GetAllMessageTypes());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
