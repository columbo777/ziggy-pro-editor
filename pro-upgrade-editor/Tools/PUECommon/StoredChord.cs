using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProUpgradeEditor.Common
{
    public class StoredChord
    {
        public int[] Notes;
        public int[] NoteChannels;
        public bool IsSlide;
        public bool IsSlideRev;
        public bool IsHammeron;
        public bool IsTap;
        public bool IsXNote;
        public ChordStrum Strum;
        public int TickLength;

        public override string ToString()
        {
            string txt = Utility.StoredChordPrefix;
            for (int x = 5; x >= 0; x--)
            {
                var s = (Notes[x] == -1 ? "" : Notes[x].ToString());
                var c = (NoteChannels[x] == -1 ? "" : NoteChannels[x].ToString());
                if (string.IsNullOrEmpty(c))
                {
                    txt += (s.Length > 0 ? s : Utility.StoredChordEmptyNote) + Utility.StoredChordNoteSeparator;
                }
                else
                {
                    txt += ("(" + (s.Length > 0 ? s : Utility.StoredChordEmptyNote) + ")") + Utility.StoredChordNoteSeparator;
                }
            }
            txt += Utility.StoredChordSuffix;

            txt += IsSlide ? Utility.StoredChordSlide : "";
            txt += IsSlideRev ? Utility.StoredChordReverse : "";
            txt += IsHammeron ? Utility.StoredChordHammeron : "";
            txt += IsTap ? Utility.StoredChordTap : "";
            txt += IsXNote ? Utility.StoredChordXNote : "";

            txt += (Strum & ChordStrum.Low) > 0 ? Utility.StoredChordStrumLow : "";
            txt += (Strum & ChordStrum.Mid) > 0 ? Utility.StoredChordStrumMed : "";
            txt += (Strum & ChordStrum.High) > 0 ? Utility.StoredChordStrumHigh : "";
            txt += " " + TickLength.ToString();
            return txt;
        }
    }
}
