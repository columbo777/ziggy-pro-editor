using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum ChordQueryOption
    {
        Default = (0),
        IncludeEndingOnMin = (1 << 0),
        IncludeStartingOnMax = (1 << 1),
    }

    [Flags()]
    public enum AdjustResult
    {
        NoResult = 0,
        Success = (1 << 0),

        AdjustedDownTickLeft = (1 << 1),
        AdjustedDownTickRight = (1 << 2),

        AdjustedUpTickLeft = (1 << 3),
        AdjustedUpTickRight = (1 << 4),

        ShortResult = (1 << 5),

        Error = (1 << 6),

        AdjustedDownTick = (AdjustedDownTickLeft | AdjustedDownTickRight),
        AdjustedUpTick = (AdjustedUpTickLeft | AdjustedUpTickRight),
    }

    [Flags()]
    public enum AdjustOption
    {
        NoAdjust = (0),
        AllowGrow = (1 << 0),
        AllowShrink = (1 << 1),
        AllowShift = (1 << 2),
        AllowAny = (AllowGrow | AllowShrink | AllowShift),
    }

    public class GuitarMessageList
    {


        protected IEnumerable<GuitarMessageType> GetAllMessageTypes()
        {
            return new GuitarMessageType[]
            {
                GuitarMessageType.GuitarHandPosition,
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
                var list = GetMessageListForType(messageType);
                if(list != null)
                {
                    foreach (var item in list)
                    {
                        if (diff.HasFlag(item.Difficulty))
                        {
                            ret.Add(item);
                        }
                    }
                }
            }

            return ret;
        }

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


        public GuitarMessageList(TrackEditor owner)
        {
            this.owner = owner;
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

        public MessageList GetMessageListForType(GuitarMessageType type)
        {
            MessageList ret = null;
            switch (type)
            {
                case GuitarMessageType.GuitarHandPosition:
                    ret = HandPositions;
                    break;

                case GuitarMessageType.GuitarTextEvent:
                    ret = TextEvents;
                    break;

                case GuitarMessageType.GuitarTrainer:
                    ret = Trainers;
                    break;
                case GuitarMessageType.GuitarChordStrum:
                    ret = ChordStrums;
                    break;
                case GuitarMessageType.GuitarChord:
                    ret = Chords;
                    break;

                case GuitarMessageType.GuitarNote:
                    ret = Notes;
                    break;

                case GuitarMessageType.GuitarPowerup:
                    ret = Powerups;
                    break;

                case GuitarMessageType.GuitarSolo:
                    ret = Solos;
                    break;

                case GuitarMessageType.GuitarTempo:
                    ret = Tempos;
                    break;

                case GuitarMessageType.GuitarTimeSignature:
                    ret = TimeSignatures;
                    break;

                case GuitarMessageType.GuitarArpeggio:
                    ret = Arpeggios;
                    break;

                case GuitarMessageType.GuitarBigRockEnding:
                    ret = BigRockEndings;
                    break;

                case GuitarMessageType.GuitarSingleStringTremelo:
                    ret = SingleStringTremelos;
                    break;

                case GuitarMessageType.GuitarMultiStringTremelo:
                    ret = MultiStringTremelos;
                    break;

                case GuitarMessageType.GuitarSlide:
                    ret = Slides;
                    break;

                case GuitarMessageType.GuitarHammeron:
                    ret = Hammerons;
                    break;
            }
            return ret;
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


        public void Add<T>(T mess) where T : GuitarMessage
        {
            if (mess != null)
            {
                
                var list = GetMessageListForType(mess.MessageType);
                if (list != null)
                {
                    if (!list.Contains(mess))
                    {
                        list.Add(mess);
                    }
                    else
                    {
                        Debug.WriteLine("re-adding to list");
                    }
                    mess.IsDeleted = false;
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

        public void Remove(MidiEvent mess)
        {
            owner.GuitarTrack.Remove(mess);
        }
        public void Remove(IEnumerable<GuitarMessage> mess)
        {
            foreach (var m in mess.ToList())
                internalRemove(m);
        }
        public void Remove(IEnumerable<MidiEvent> mess)
        {
            owner.GuitarTrack.Remove(mess);
        }
        public void Remove(IEnumerable<MidiEventPair> mess)
        {
            owner.GuitarTrack.Remove(mess);
        }
        internal void internalRemove<T>(T mess) where T : GuitarMessage
        {
            if (mess != null)
            {
                GetMessageListForType(mess.MessageType).Remove(mess);
            }
        }


    }
}
