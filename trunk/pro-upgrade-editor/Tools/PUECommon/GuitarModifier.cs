using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public enum GuitarModifierType
    {
        Invalid = 0,
        Arpeggio,
        BigRockEnding,
        Powerup,
        Solo,
        MultiStringTremelo,
        SingleStringTremelo,
    }

    public enum ChordModifierType
    {
        Invalid = 0,
        ChordStrumLow,
        ChordStrumMed,
        ChordStrumHigh,
        Slide,
        SlideReverse,
        Hammeron,
    }


    public static class GuitarModifierTypeExtension
    {
        public static IEnumerable<int> GetData1ForChordModifierType(this ChordModifierType type,
            GuitarDifficulty difficulty = GuitarDifficulty.All, bool isPro = true)
        {
            var ret = new List<int>();
            if (difficulty.IsAll())
                difficulty = GuitarDifficulty.Expert;

            switch (type)
            {
                case ChordModifierType.Hammeron:
                    {
                        var d1 = Utility.GetHammeronData1(difficulty);

                        if (d1 != -1)
                        {
                            ret.Add(d1);
                        }
                    }
                    break;
                case ChordModifierType.Slide:
                case ChordModifierType.SlideReverse:
                    {
                        var d1 = Utility.GetSlideData1(difficulty);
                        if(d1 != -1)
                        {
                            ret.Add(d1);
                        }
                    }
                    break;
                case ChordModifierType.ChordStrumLow:
                case ChordModifierType.ChordStrumMed:
                case ChordModifierType.ChordStrumHigh:
                    {
                        var strum = Utility.GetStrumData1(difficulty);
                        if (strum != -1)
                        {
                            ret.Add(strum);
                        }
                    }
                    break;
            }
            return ret;
        }
        public static IEnumerable<int> GetData1ForModifierType(this GuitarModifierType type,
            GuitarDifficulty difficulty = GuitarDifficulty.All, bool isPro = true)
        {
            var ret = new List<int>();
            switch (type)
            {


                case GuitarModifierType.Arpeggio:
                    {
                        ret.AddRange(Utility.AllArpeggioData1);
                    }
                    break;
                case GuitarModifierType.BigRockEnding:
                    {
                        ret.AddRange(Utility.BigRockEndingData1);
                    }
                    break;
                case GuitarModifierType.Powerup:
                    {
                        ret.AddRange(Utility.PowerupData1.MakeEnumerable());
                    }
                    break;
                case GuitarModifierType.Solo:
                    {
                        if (isPro)
                        {
                            ret.AddRange(Utility.SoloData1.MakeEnumerable());
                        }
                        else
                        {
                            ret.AddRange(Utility.SoloData1.MakeEnumerable().Concat(Utility.SoloData1_G5));
                        }
                    }
                    break;
                case GuitarModifierType.MultiStringTremelo:
                    {
                        ret.AddRange(Utility.MultiStringTremeloData1.MakeEnumerable());
                    }
                    break;
                case GuitarModifierType.SingleStringTremelo:
                    {
                        ret.AddRange(Utility.SingleStringTremeloData1.MakeEnumerable());
                    }
                    break;

            }
            return ret;
        }
    }

    public class ChordModifier : GuitarMessage
    {
        public ChordModifierType ModifierType;

        public ChordModifier(MidiEventPair ev, ChordModifierType type, GuitarMessageType gt) :
            this(ev.Owner, ev.Down, ev.Up, type, gt)
        {
            Data1 = type.GetData1ForChordModifierType(ev.Owner.Owner.CurrentDifficulty, true).FirstOrDefault();
            Data2 = 100;

        }
        public ChordModifier(GuitarMessageList track, MidiEvent downEvent, MidiEvent upEvent, ChordModifierType type, GuitarMessageType gt) :
            base(track, new MidiEventPair(track, downEvent, upEvent), gt)
        {
            if (type == ChordModifierType.Invalid)
            {
                if (downEvent != null)
                {
                    if (downEvent.Channel == Utility.ChannelStrumHigh)
                        type = ChordModifierType.ChordStrumHigh;
                    if (downEvent.Channel == Utility.ChannelStrumMid)
                        type = ChordModifierType.ChordStrumMed;
                    if (downEvent.Channel == Utility.ChannelStrumLow)
                        type = ChordModifierType.ChordStrumLow;
                }
            }
            this.ModifierType = type;
        }


        public static ChordModifier GetModifier(GuitarMessageList track, TickPair ticks,
            ChordModifierType type, GuitarMessageType gt, GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            var ret = new ChordModifier(track, null, null, type, gt);
            type.GetData1ForChordModifierType(difficulty, track.Owner.IsPro).FirstOrDefault().IfNotNull(d1 => ret.Data1 = d1);
            ret.Data2 = 100;
            ret.Channel = (type == ChordModifierType.SlideReverse ? Utility.ChannelSlideReversed : 0);

            ret.SetTicks(ticks);

            return ret;
        }

        public static ChordModifier CreateModifier(GuitarMessageList track, TickPair ticks,
            ChordModifierType type, bool createMidi, GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            ChordModifier ret = null;

            switch (type)
            {
                case ChordModifierType.ChordStrumLow:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.Low, ticks);
                    }
                    break;
                case ChordModifierType.ChordStrumMed:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.Mid, ticks);
                    }
                    break;
                case ChordModifierType.ChordStrumHigh:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.High, ticks);
                    }
                    break;
                case ChordModifierType.Hammeron:
                    {
                        ret = GuitarHammeron.CreateHammeron(track, ticks, difficulty);
                    }
                    break;
                case ChordModifierType.Slide:
                    {
                        ret = GuitarSlide.CreateSlide(track, ticks, false, difficulty);
                    }
                    break;
                case ChordModifierType.SlideReverse:
                    {
                        ret = GuitarSlide.CreateSlide(track, ticks, true, difficulty);
                    }
                    break;
            }

            return ret;
        }
    }
    public class GuitarModifier : GuitarMessage
    {
        public GuitarModifierType ModifierType;

        public GuitarModifier(GuitarMessageList track, MidiEvent downEvent, MidiEvent upEvent, GuitarModifierType type, GuitarMessageType mt) :
            base(track, new MidiEventPair(track, downEvent, upEvent), mt)
        {
            this.ModifierType = type;
        }
        public GuitarModifier(MidiEventPair ev, GuitarModifierType type, GuitarMessageType mt) :
            base(ev, mt)
        {
            this.ModifierType = type;
        }

        public static GuitarModifier GetModifier(GuitarMessageList track, TickPair ticks,
            GuitarModifierType type, GuitarMessageType mt,
            GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            var ret = new GuitarModifier(track, null, null, type, mt);
            ret.SetTicks(ticks);

            return ret;
        }

        public static GuitarModifier CreateModifier(GuitarMessageList track, TickPair ticks,
            GuitarModifierType type, bool createMidi, GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            GuitarModifier ret = null;

            switch (type)
            {

                case GuitarModifierType.Arpeggio:
                    {
                        ret = GuitarArpeggio.CreateArpeggio(track, difficulty, ticks);
                    }
                    break;
                case GuitarModifierType.BigRockEnding:
                    {
                        ret = GuitarBigRockEnding.CreateBigRockEnding(track, ticks);
                    }
                    break;
                case GuitarModifierType.Powerup:
                    {
                        ret = GuitarPowerup.CreatePowerup(track, ticks);
                    }
                    break;
                case GuitarModifierType.Solo:
                    {
                        ret = GuitarSolo.CreateSolo(track, ticks);
                    }
                    break;
                case GuitarModifierType.MultiStringTremelo:
                    {
                        ret = GuitarMultiStringTremelo.CreateMultiStringTremelo(track, ticks);
                    }
                    break;
                case GuitarModifierType.SingleStringTremelo:
                    {
                        ret = GuitarSingleStringTremelo.CreateSingleStringTremelo(track, ticks);
                    }
                    break;

            }

            return ret;
        }
    }
}
