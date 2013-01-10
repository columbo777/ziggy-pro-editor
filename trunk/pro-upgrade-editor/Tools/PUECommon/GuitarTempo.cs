using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class GuitarTempo : GuitarMessage
    {
        public double Tempo;
        public GuitarTempo(GuitarTrack track, MidiEvent ev)
            : base(track, ev, null, GuitarMessageType.GuitarTempo)
        {
            if (ev == null)
            {
                this.Tempo = Utility.DummyTempo;
            }
            else
            {
                var cb = new TempoChangeBuilder(ev.MetaMessage);
                this.Tempo = cb.Tempo;
            }
        }

        public override double TimeLength
        {
            get
            {
                return SecondsPerTick * TickLengthDouble;
            }
        }

        double _cacheStartTime = double.MinValue;
        public override double StartTime
        {
            get
            {
                if (_cacheStartTime.IsNull())
                {
                    _cacheStartTime = OwnerTrack.Messages.Tempos.Where(x => x.AbsoluteTicks < AbsoluteTicks).Sum(x => x.TimeLength);
                }
                return _cacheStartTime;
            }
        }

        double _cacheEndTime = double.MinValue;
        public override double EndTime
        {
            get
            {
                if (_cacheEndTime.IsNull())
                {
                    _cacheEndTime = StartTime + TimeLength;
                }
                return _cacheEndTime;
            }
        }

        public double TickLengthDouble
        {
            get { return ((double)TickLength); }
        }

        public double NumQuarterNotes
        {
            get { return TickLengthDouble / OwnerTrack.SequenceDivision; }
        }
        
        public double NumOneTwentyEight
        {
            get { return NumQuarterNotes * 32.0; }
        }

        public double TicksPerOneTwentyEight
        {
            get { return TickLengthDouble / NumOneTwentyEight; }
        }

        public double TicksPerQuarterNote
        {
            get { return TickLengthDouble / NumQuarterNotes; }
        }

        public double SecondsPerOneTwentyEight
        {
            get { return SecondsPerQuarterNote / 32.0; }
        }

        public double SecondsPerQuarterNote
        {
            get { return 1.0 / QuarterNotesPerSecond; }
        }

        public double QuarterNotesPerSecond
        {
            get { return 1000000.0 / Tempo; }
        }

        public double TicksPerSecond
        {
            get { return 1000000.0 / (Tempo / OwnerTrack.SequenceDivision); }
        }

        public double SecondsPerTick
        {
            get { return 1.0 / TicksPerSecond; }
        }

        public static GuitarTempo GetTempo(GuitarTrack track, double tempo)
        {
            var ret = new GuitarTempo(track, null);
            ret.Tempo = tempo;
            return ret;
        }

        public MetaMessage BuildMessage()
        {
            var b = new TempoChangeBuilder();
            b.Tempo = (int)Tempo;
            b.Build();
            return b.Result;
        }

        public static GuitarTempo CreateTempo(
            GuitarTrack track,
            int startTick,
            double tempo)
        {
            var ret = GetTempo(track, tempo);

            ret.SetDownEvent(track.Insert(startTick, ret.BuildMessage()));

            track.Messages.Add(ret);
            return ret;
        }

        public override string ToString()
        {
            return AbsoluteTicks + " - Tempo: " + Tempo;
        }

    }


    public class GuitarTimeSignature : GuitarMessage
    {
        public static GuitarTimeSignature GetDefaultTimeSignature(GuitarTrack track)
        {
            return new GuitarTimeSignature(track, null);
        }

        public double Numerator = 4;
        public double Denominator = 4;
        public double ClocksPerMetronomeClick = 24;
        public double ThirtySecondNotesPerQuarterNote = 8;

        public static GuitarTimeSignature GetTimeSignature(GuitarTrack track,
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
            GuitarTrack track,
            int startTick=0,
            int numerator=4, 
            int denominator=4, 
            int clocksPerMetronomeClick=24,
            int thirtySecondNotesPerQuarterNote=8)
        {
            var ret = GetTimeSignature(track, startTick, 
                numerator, denominator, clocksPerMetronomeClick, thirtySecondNotesPerQuarterNote);

            ret.SetDownEvent(track.Insert(startTick, ret.BuildMessage()));

            track.Messages.Add(ret);
            return ret;
        }

        public GuitarTimeSignature(GuitarTrack track, MidiEvent ev)
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
