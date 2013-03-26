using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public static class GuitarModifierTypeExtension
    {
        public static IEnumerable<int> GetData1ForChordModifierType(this ChordModifierType type,
            GuitarDifficulty difficulty, bool isPro)
        {
            var ret = new List<int>();
            if (difficulty.IsAll())
                difficulty = GuitarDifficulty.Expert;

            switch (type)
            {
                case ChordModifierType.Hammeron:
                    {
                        var d1 = Utility.GetHammeronData1(difficulty);

                        if (!d1.IsNull())
                        {
                            ret.Add(d1);
                        }
                    }
                    break;
                case ChordModifierType.Slide:
                case ChordModifierType.SlideReverse:
                    {
                        var d1 = Utility.GetSlideData1(difficulty);
                        if (!d1.IsNull())
                        {
                            ret.Add(d1);
                        }
                    }
                    break;
                case ChordModifierType.ChordStrumLow:
                case ChordModifierType.ChordStrumMed:
                case ChordModifierType.ChordStrumHigh:
                    {
                        var d1 = Utility.GetStrumData1(difficulty);
                        if (!d1.IsNull())
                        {
                            ret.Add(d1);
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
}