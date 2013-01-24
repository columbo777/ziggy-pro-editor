using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProUpgradeEditor.Common
{

    public enum InstrumentDifficulty
    {
        INVALID = -1,
        NOPART = 0,
        Warmup,
        Apprentice,
        Solid,
        Moderate,
        Challenging,
        Nightmare,
        Impossible,
    }

    public class InstrumentDifficultyUtil
    {

        public static string MapDifficulty(InstrumentDifficulty diff)
        {
            if (diff == InstrumentDifficulty.NOPART ||
                diff == InstrumentDifficulty.INVALID)
                return Utility.DTADifficultyNoPart;
            if (diff == InstrumentDifficulty.Warmup)
                return Utility.DTADifficultyWarmup;
            if (diff == InstrumentDifficulty.Apprentice)
                return Utility.DTADifficultyApprentice;
            if (diff == InstrumentDifficulty.Solid)
                return Utility.DTADifficultySolid;
            if (diff == InstrumentDifficulty.Moderate)
                return Utility.DTADifficultyModerate;
            if (diff == InstrumentDifficulty.Challenging)
                return Utility.DTADifficultyChallenging;
            if (diff == InstrumentDifficulty.Nightmare)
                return Utility.DTADifficultyNightmare;
            //if(diff == InstrumentDifficulty.Impossible)
            return Utility.DTADifficultyImpossible;
        }
        public static InstrumentDifficulty MapDifficulty(string diff)
        {
            if (string.IsNullOrEmpty(diff))
                return InstrumentDifficulty.NOPART;

            int iDiff = diff.ToInt(0);

            if (iDiff == 0)
            {
                return InstrumentDifficulty.NOPART;
            }
            if (iDiff < 199)
            {
                return InstrumentDifficulty.Warmup;
            }
            if (iDiff < 220)
            {
                return InstrumentDifficulty.Apprentice;
            }
            if (iDiff < 268)
            {
                return InstrumentDifficulty.Solid;
            }
            if (iDiff < 373)
            {
                return InstrumentDifficulty.Moderate;
            }
            if (iDiff < 440)
            {
                return InstrumentDifficulty.Challenging;
            }
            if (iDiff < 456)
            {
                return InstrumentDifficulty.Nightmare;
            }
            return InstrumentDifficulty.Impossible;
        }

        public static string GetDifficulty(byte[] upgradesdta, string match)
        {
            string ret = string.Empty;
            using (var sr = new StreamReader(new MemoryStream(upgradesdta)))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Trim(' ', '(', ')');
                    var c = line.IndexOf(';');
                    if (c != -1)
                    {
                        line = line.Substring(0, c).Trim(' ', '(', ')');
                    }
                    if (line.StartsWith(match, StringComparison.OrdinalIgnoreCase))
                    {
                        var diff = line.Substring(match.Length).Trim();
                        return diff;
                    }
                }
            }
            return ret;
        }

        public static InstrumentDifficulty DTAGetGuitarDifficulty(byte[] upgrades)
        {
            return MapDifficulty(GetDifficulty(upgrades, "real_guitar "));
        }

        public static InstrumentDifficulty DTAGetBassDifficulty(byte[] upgrades)
        {
            return MapDifficulty(GetDifficulty(upgrades, "real_bass "));
        }
    }
}
