using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{

    public enum GuitarModifierType
    {
        NoteModifier=0,
        Arpeggio,
        BigRockEnding,
        Powerup,
        Solo,
        MultiStringTremelo,
        SingleStringTremelo,
        ChordStrumLow,
        ChordStrumMed,
        ChordStrumHigh,
    }
    public class GuitarModifier : GuitarMessage
    {
        public GuitarModifierType ModifierType;

        public GuitarModifier(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent, GuitarModifierType type) : base(track, downEvent, upEvent)
        {
            this.ModifierType = type;
        }

        public static GuitarModifier CreateModifier(GuitarTrack track, int downTick, int upTick, GuitarModifierType type, GuitarDifficulty difficulty=GuitarDifficulty.Expert)
        {
            GuitarModifier ret = null;

            switch(type)
            {
                case GuitarModifierType.ChordStrumLow:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.Low, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.ChordStrumMed:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.Mid, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.ChordStrumHigh:
                    {
                        ret = GuitarChordStrum.CreateStrum(track, difficulty, ChordStrum.High, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.Arpeggio:
                    {
                        ret = GuitarArpeggio.CreateArpeggio(track, difficulty, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.BigRockEnding:
                    {
                        ret = GuitarBigRockEnding.CreateBigRockEnding(track, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.Powerup:
                    {
                        ret = GuitarPowerup.CreatePowerup(track, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.Solo:
                    {
                        ret = GuitarSolo.CreateSolo(track, downTick, upTick);
                    }
                    break;
                case GuitarModifierType.MultiStringTremelo:
                    {
                        ret = GuitarMultiStringTremelo.CreateMultiStringTremelo(track,downTick, upTick);
                    }
                    break;
                case GuitarModifierType.SingleStringTremelo:
                    {
                        ret = GuitarSingleStringTremelo.CreateSingleStringTremelo(track, downTick, upTick);
                    }
                    break;
            }

            return ret;
        }
    }
}
