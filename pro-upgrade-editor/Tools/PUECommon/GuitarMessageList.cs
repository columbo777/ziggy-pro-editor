using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    [Flags()]
    public enum ChordQueryOption
    {
        Default = (0),
        IncludeEndingOnMin = (1<<0),
        IncludeStartingOnMax = (1<<1),
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


        public static IEnumerable<GuitarMessageType> AllMessageTypes
        {
            get
            {
                yield return GuitarMessageType.GuitarHandPosition;
                yield return GuitarMessageType.GuitarTextEvent;
                yield return GuitarMessageType.GuitarTrainer;
                yield return GuitarMessageType.GuitarChord;
                yield return GuitarMessageType.GuitarChordStrum;
                yield return GuitarMessageType.GuitarNote;
                yield return GuitarMessageType.GuitarPowerup;
                yield return GuitarMessageType.GuitarSolo;
                yield return GuitarMessageType.GuitarTempo;
                yield return GuitarMessageType.GuitarTimeSignature;
                yield return GuitarMessageType.GuitarArpeggio;
                yield return GuitarMessageType.GuitarBigRockEnding;
                yield return GuitarMessageType.GuitarSingleStringTremelo;
                yield return GuitarMessageType.GuitarMultiStringTremelo;
                yield return GuitarMessageType.GuitarSlide;
                yield return GuitarMessageType.GuitarHammeron;
            }
        }

        public IEnumerable<GuitarMessage> Where(Func<GuitarMessage,bool> func)
        {
            foreach (var messageType in AllMessageTypes)
            {
                var list = GetMessageListForType(messageType).List;
                foreach (var msg in list)
                {
                    if (func(msg))
                    {
                        yield return msg;
                    }
                }
            }
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

        public IEnumerable<GuitarMessage> ImportMessageRange(IEnumerable<GuitarMessage> list)
        {
            var ret = new List<GuitarMessage>();

            ret.AddRange(list.Where(x => x.IsMetaEvent()).ToList().Select(x => ImportMessage(x)).Where(x=> x != null));
            ret.AddRange(list.Where(x => x.IsChannelEvent() && !x.IsChordSubItem() && !x.IsGuitarChord()).ToList().Select(x=> ImportMessage(x)).Where(x => x != null));
            ret.AddRange(list.Where(x => x.IsChannelEvent() && x.IsChordSubItem()).ToList().Select(x => ImportMessage(x)).Where(x => x != null));
            ret.AddRange(addContainerChords(ret));

            return ret;
        }

        IEnumerable<GuitarMessage> addContainerChords(IEnumerable<GuitarMessage> list)
        {
            var ret = new List<GuitarMessage>();
            ret.AddRange(list.Where(x=> x is GuitarNote).GroupByCloseTick().ToList().Select(x=>
                new GuitarChord(owner.GuitarTrack, x.Cast<GuitarNote>(), true)).Where(x=> x != null));
            var chordList = GetMessageListForType(GuitarMessageType.GuitarChord);
            chordList.AddGuitarMessageRange(ret);
            return ret;
        }

        public GuitarMessage ImportMessage(GuitarMessage msg)
        {
            var m = convertMessage(msg);
            if (m != null)
            {
                var downEvent = owner.GuitarTrack.Insert(msg.DownTick, m.A);
                MidiEvent upEvent = null;
                if (m.B != null)
                {
                    upEvent = owner.GuitarTrack.Insert(msg.UpTick, m.B);
                }
                var newMessage = owner.GuitarTrack.CreateMessageByType(msg.MessageType, downEvent, upEvent);
                if (newMessage != null)
                {
                    GetMessageListForType(msg.MessageType).Add(newMessage);
                }
                return newMessage;
            }
            return null;
        }

        public GuitarMessage ImportEvent(GuitarMessageType type, MidiEvent down, MidiEvent up = null)
        {
            var cd = convertEvent(down);
            if(cd != null)
            {
                var downEvent = owner.GuitarTrack.Insert(down.AbsoluteTicks, cd);
                MidiEvent upEvent = null;
                if (up != null)
                {
                    var ue = convertEvent(up);
                    if (ue != null)
                    {
                        upEvent = owner.GuitarTrack.Insert(up.AbsoluteTicks, ue);
                    }
                }
                
                var newMessage = owner.GuitarTrack.CreateMessageByType(type, downEvent, upEvent);
                if (newMessage != null)
                {
                    GetMessageListForType(type).Add(newMessage);
                }
                return newMessage;
            }
            return null;
        }

        DataPair<IMidiMessage> convertMessage(GuitarMessage x)
        {
            return new DataPair<IMidiMessage>(convertEvent(x.DownEvent), convertEvent(x.UpEvent));
        }

        IMidiMessage convertEvent(MidiEvent x)
        {
            IMidiMessage ret = null;
            if (x != null)
            {
                ret = x.MidiMessage;
                if (x.IsChannelEvent())
                {
                    if (owner.IsPro && !x.Owner.IsFileTypePro())
                    {
                        ret = x.ChannelMessage.ConvertToPro(owner.CurrentDifficulty);
                    }
                    else if (!owner.IsPro && x.Owner.IsFileTypePro())
                    {
                        ret = x.ChannelMessage.ConvertToG5(owner.CurrentDifficulty);
                    }
                }
            }
            return ret;
        }


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
                    ret.AddRange(Arpeggios);
                    break;
                case GuitarModifierType.BigRockEnding:
                    ret.AddRange(BigRockEndings);
                    break;
                case GuitarModifierType.Powerup:
                    ret.AddRange(Powerups);
                    break;
                case GuitarModifierType.Solo:
                    ret.AddRange(Solos);
                    break;
                case GuitarModifierType.MultiStringTremelo:
                    ret.AddRange(MultiStringTremelos);
                    break;
                case GuitarModifierType.SingleStringTremelo:
                    ret.AddRange(SingleStringTremelos);
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
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode == ChordStrum.Low));
                    break;
                case ChordModifierType.ChordStrumMed:
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode == ChordStrum.Mid));
                    break;
                case ChordModifierType.ChordStrumHigh:
                    ret.AddRange(ChordStrums.Where(x => x.StrumMode == ChordStrum.High));
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

        public ISpecializedList GetMessageListForType(GuitarMessageType type)
        {
            ISpecializedList ret = null;
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
                var items = mess.GroupBy(m => m.MessageType).ToList();
                if (items.Count > 1)
                {
                    foreach (var item in items)
                    {
                        AddRange(item.ToList());
                    }
                }
                items.ForEach(x =>
                {
                    GetMessageListForType(x.Key).AddGuitarMessageRange(x);
                });
            }
        }


        public void Add<T>(T mess) where T : GuitarMessage
        {
            GetMessageListForType(mess.MessageType).Add(mess);
        }



        public void Remove<T>(T mess) where T : GuitarMessage
        {
            GetMessageListForType(mess.MessageType).Remove(mess);
        }




    }
}
