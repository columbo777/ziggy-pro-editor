using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarTimeSignature : GuitarMessage
    {
        public static GuitarTimeSignature GetDefaultTimeSignature(GuitarMessageList track)
        {
            return new GuitarTimeSignature(track, null);
        }

        public double Numerator = 4;
        public double Denominator = 4;
        public double ClocksPerMetronomeClick = 24;
        public double ThirtySecondNotesPerQuarterNote = 8;

        public override string ToString()
        {
            return "TimeSig: " + DownTick + " - " + UpTick + " - " + (int)Numerator + "/" + (int)Denominator;
        }

        public static GuitarTimeSignature GetTimeSignature(GuitarMessageList track,
            int startTick = 0,
            int numerator = 4,
            int denominator = 4,
            int clocksPerMetronomeClick = 24,
            int thirtySecondNotesPerQuarterNote = 8)
        {
            var ret = new GuitarTimeSignature(track, null);
            ret.Numerator = numerator;
            ret.Denominator = denominator;
            ret.ClocksPerMetronomeClick = clocksPerMetronomeClick;
            ret.ThirtySecondNotesPerQuarterNote = thirtySecondNotesPerQuarterNote;
            ret.SetDownTick(startTick);

            return ret;
        }

        public MetaMessage BuildMessage()
        {
            var sb = new TimeSignatureBuilder();
            sb.Numerator = (byte)Numerator;
            sb.Denominator = (byte)Denominator;
            sb.ClocksPerMetronomeClick = (byte)ClocksPerMetronomeClick;
            sb.ThirtySecondNotesPerQuarterNote = (byte)ThirtySecondNotesPerQuarterNote;
            sb.Build();
            return sb.Result;
        }

        public static GuitarTimeSignature CreateTimeSignature(
            GuitarMessageList track,
            int startTick = 0,
            int numerator = 4,
            int denominator = 4,
            int clocksPerMetronomeClick = 24,
            int thirtySecondNotesPerQuarterNote = 8)
        {
            var ret = GetTimeSignature(track, startTick,
                numerator, denominator, clocksPerMetronomeClick, thirtySecondNotesPerQuarterNote);

            ret.SetDownEvent(track.Insert(startTick, ret.BuildMessage()));

            track.Add(ret);
            return ret;
        }

        public GuitarTimeSignature(GuitarMessageList track, MidiEvent ev)
            : base(track, ev, null, GuitarMessageType.GuitarTimeSignature)
        {

            if (ev == null)
            {
                SetDownTick(0);
            }
            else
            {
                SetDownEvent(ev);

                var builder = new TimeSignatureBuilder(ev.MetaMessage);

                Numerator = builder.Numerator;
                Denominator = builder.Denominator;
                ClocksPerMetronomeClick = builder.ClocksPerMetronomeClick;

                ThirtySecondNotesPerQuarterNote = builder.ThirtySecondNotesPerQuarterNote;
            }
        }
    }
}