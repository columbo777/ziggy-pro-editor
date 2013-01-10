using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{

    public enum GuitarModifierType
    {
        Invalid=0,
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
            switch (type)
            {
                case ChordModifierType.Hammeron:
                    {
                        ret.AddRange(Utility.AllHammeronData1);
                    }
                    break;
                case ChordModifierType.Slide:
                    {
                        ret.AddRange(Utility.AllSlideData1);
                    }
                    break;
                case ChordModifierType.SlideReverse:
                    {
                        ret.AddRange(Utility.AllSlideData1);
                    }
                    break;
                case ChordModifierType.ChordStrumLow:
                case ChordModifierType.ChordStrumMed:
                case ChordModifierType.ChordStrumHigh:
                    {
                        ret.AddRange(Utility.AllStrumData1);
                    }
                    break;
            }
            return ret;
        }
        public static IEnumerable<int> GetData1ForModifierType(this GuitarModifierType type, 
            GuitarDifficulty difficulty=GuitarDifficulty.All, bool isPro=true)
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

        public ChordModifier(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent, ChordModifierType type, GuitarMessageType gt) :
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

        

        public static ChordModifier GetModifier(GuitarTrack track, TickPair ticks,
            ChordModifierType type, GuitarMessageType gt, GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            var ret = new ChordModifier(track, null, null, type, gt);
            ret.SetTicks(ticks);

            return ret;
        }

        public static ChordModifier CreateModifier(GuitarTrack track, TickPair ticks,
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
                        ret = GuitarHammeron.CreateHammeron(track, ticks);
                    }
                    break;
                case ChordModifierType.Slide:
                    {
                        ret = GuitarSlide.CreateSlide(track, ticks, false);
                    }
                    break;
                case ChordModifierType.SlideReverse:
                    {
                        ret = GuitarSlide.CreateSlide(track, ticks, true);
                    }
                    break;
            }

            return ret;
        }
    }
    public class GuitarModifier : GuitarMessage
    {
        public GuitarModifierType ModifierType;

        public GuitarModifier(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent, GuitarModifierType type, GuitarMessageType mt) : 
            base(track, new MidiEventPair(track, downEvent, upEvent), mt)
        {
            this.ModifierType = type;
        }


        public static GuitarModifier GetModifier(GuitarTrack track, TickPair ticks,
            GuitarModifierType type, GuitarMessageType mt,
            GuitarDifficulty difficulty = GuitarDifficulty.Expert)
        {
            var ret = new GuitarModifier(track, null, null, type, mt);
            ret.SetTicks(ticks);
            
            return ret;
        }

        public static GuitarModifier CreateModifier(GuitarTrack track, TickPair ticks,
            GuitarModifierType type, bool createMidi, GuitarDifficulty difficulty=GuitarDifficulty.Expert)
        {
            GuitarModifier ret = null;

            switch(type)
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
                        ret = GuitarMultiStringTremelo.CreateMultiStringTremelo(track,ticks);
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
