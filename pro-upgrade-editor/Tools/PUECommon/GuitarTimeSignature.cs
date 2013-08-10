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
        public static GuitarTimeSignature GetDefaultTimeSignature(GuitarMessageList owner)
        {
            return new GuitarTimeSignature(owner, null);
        }

        public double Numerator = 4;
        public double Denominator = 4;
        public double ClocksPerMetronomeClick = 24;
        public double ThirtySecondNotesPerQuarterNote = 8;

        public override string ToString()
        {
            return "TimeSig: " + DownTick + " - " + UpTick + " - " + (int)Numerator + "/" + (int)Denominator;
        }

        public static GuitarTimeSignature GetTimeSignature(GuitarMessageList owner,
            int startTick = 0,
            int numerator = 4,
            int denominator = 4,
            int clocksPerMetronomeClick = 24,
            int thirtySecondNotesPerQuarterNote = 8)
        {
            var ret = new GuitarTimeSignature(owner, null);
            ret.Numerator = numerator;
            ret.Denominator = denominator;
            ret.ClocksPerMetronomeClick = clocksPerMetronomeClick;
            ret.ThirtySecondNotesPerQuarterNote = thirtySecondNotesPerQuarterNote;
            ret.SetDownTick(startTick);

            return ret;
        }

        public static MetaMessage BuildMessage(int numerator = 4, int denominator = 4, int clocksPerMetronomeClick = 24, int thirtySecondNotesPerQuarterNote = 8)
        {
            var sb = new TimeSignatureBuilder();
            sb.Numerator = (byte)numerator;
            sb.Denominator = (byte)denominator;
            sb.ClocksPerMetronomeClick = (byte)clocksPerMetronomeClick;
            sb.ThirtySecondNotesPerQuarterNote = (byte)thirtySecondNotesPerQuarterNote;
            sb.Build();
            return sb.Result;
        }

        public static GuitarTimeSignature CreateTimeSignature(
            GuitarMessageList owner,
            int startTick = 0,
            int numerator = 4,
            int denominator = 4,
            int clocksPerMetronomeClick = 24,
            int thirtySecondNotesPerQuarterNote = 8)
        {
            var ret = GetTimeSignature(owner, startTick,
                numerator, denominator, clocksPerMetronomeClick, thirtySecondNotesPerQuarterNote);

            ret.IsNew = true;
            ret.CreateEvents();

            return ret;
        }

        public GuitarTimeSignature(GuitarMessageList owner, MidiEvent ev)
            : base(owner, ev, null, GuitarMessageType.GuitarTimeSignature)
        {

            if (ev == null)
            {
                SetDownTick(0);
            }
            else
            {
                SetDownEvent(ev);

                var builder = new TimeSignatureBuilder((MetaMessage)ev.Clone());

                Numerator = builder.Numerator;
                Denominator = builder.Denominator;
                ClocksPerMetronomeClick = builder.ClocksPerMetronomeClick;

                ThirtySecondNotesPerQuarterNote = builder.ThirtySecondNotesPerQuarterNote;
            }
        }
    }
}