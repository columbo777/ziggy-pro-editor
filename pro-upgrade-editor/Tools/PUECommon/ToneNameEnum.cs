using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public enum ToneNameEnum
    {
        NotSet = -1,
        E = 0,
        F = 1,
        Gb = 2,
        G = 3,
        Ab = 4,
        A = 5,
        Bb = 6,
        B = 7,
        C = 8,
        Db = 9,
        D = 10,
        Eb = 11,
        NumTones = 12,
    }

    public static class ToneExtension
    {

        public static ToneNameData1 ToToneNameData1(this ToneNameEnum tone)
        {
            if (tone == ToneNameEnum.NotSet)
            {
                return ToneNameData1.NotSet;
            }
            return (ToneNameData1)(ToneNameData1.BaseValue.ToInt() + tone.ToInt());
        }

        public static ToneNameEnum ToToneName(this ToneNameData1 tone)
        {
            return (ToneNameEnum)(tone.ToInt() - ToneNameData1.BaseValue.ToInt());
        }

        public static bool IsNotNull(this ToneNameEnum tone)
        {
            return tone != ToneNameEnum.NotSet;
        }
        public static bool IsNull(this ToneNameEnum tone)
        {
            return tone == ToneNameEnum.NotSet;
        }

        public static bool IsFlat(this ToneNameEnum tone)
        {
            return tone == ToneNameEnum.Gb || tone == ToneNameEnum.Ab || tone == ToneNameEnum.Bb || tone == ToneNameEnum.Db || tone == ToneNameEnum.Eb;
        }

        public static ToneNameEnum NextSemiNote(this ToneNameEnum tone)
        {
            var next = tone.ToInt() + 1;
            if (next > ToneNameEnum.Eb.ToInt())
            {
                next = ToneNameEnum.E.ToInt();
            }
            return (ToneNameEnum)next;
        }

        public static ToneNameEnum NextWholeNote(this ToneNameEnum tone)
        {
            var ret = tone.NextSemiNote();
            if (ret.IsFlat())
            {
                ret = ret.NextSemiNote();
            }
            return ret;
        }

        public static ToneNameEnum PreviousSemiNote(this ToneNameEnum tone)
        {
            var prev = tone.ToInt() - 1;
            if (prev < 0)
                prev = ToneNameEnum.Eb.ToInt();
            return (ToneNameEnum)prev;
        }

        public static ToneNameEnum PreviousWholeNote(this ToneNameEnum tone)
        {
            var ret = tone.PreviousSemiNote();
            if (ret.IsFlat())
            {
                ret = ret.PreviousSemiNote();
            }
            return ret;
        }

        public static string GetToneLetter(this ToneNameEnum tone, bool sharp = false)
        {
            var ret = string.Empty;
            if (tone.IsNotNull())
            {
                if (sharp && tone.IsFlat())
                {
                    ret = tone.PreviousSemiNote().GetToneLetter();
                }
                else
                {
                    tone.ToStringEx().IfNotEmpty(x => ret = x.First().ToString());
                }
            }
            return ret;
        }

        public static string ToStringEx(this ToneNameEnum tone, bool sharp = false)
        {
            var ret = string.Empty;
            if (tone != ToneNameEnum.NotSet)
            {
                if (sharp)
                {
                    if (tone == ToneNameEnum.Gb)
                    {
                        ret = "F#";
                    }
                    else if (tone == ToneNameEnum.Ab)
                    {
                        ret = "G#";
                    }
                    else if (tone == ToneNameEnum.Ab)
                    {
                        ret = "G#";
                    }
                    ret = tone.ToString();
                }
                else
                {
                    ret = tone.ToString();
                }
            }
            return ret;
        }
    }
}