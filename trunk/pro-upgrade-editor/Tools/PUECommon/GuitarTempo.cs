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
        public GuitarTempo(GuitarMessageList owner, MidiEvent ev)
            : base(owner, ev, null, GuitarMessageType.GuitarTempo)
        {
            if (ev == null)
            {
                this.Tempo = Utility.DummyTempo;
            }
            else
            {
                var cb = new TempoChangeBuilder((MetaMessage)ev.Clone());
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
                    _cacheStartTime = Owner.Tempos.Where(x => x.AbsoluteTicks < AbsoluteTicks).Sum(x => x.TimeLength);
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
            get { return TickLengthDouble / Owner.Owner.GuitarTrack.SequenceDivision; }
        }
        public double NumWholeNotes
        {
            get { return NumQuarterNotes * 4.0; }
        }
        public double NumOneTwentyEigthNotes
        {
            get { return NumQuarterNotes * 32.0; }
        }
        public double NumSixtyFourthNotes
        {
            get { return NumQuarterNotes * 16.0; }
        }
        public double NumThirtySecondNotes
        {
            get { return NumQuarterNotes * 8.0; }
        }
        public double NumSixteenthNotes
        {
            get { return NumQuarterNotes * 4.0; }
        }
        public double NumEigthNotes
        {
            get { return NumQuarterNotes * 2.0; }
        }
        public double GetTicksPerBeat(TimeUnit unit)
        {
            var scale = 1.0 / (double)unit;
            return TicksPerWholeNote * scale;
        }

        public double TicksPerOneTwentyEigthNote
        {
            get { return TicksPerSixtyFourth / 2.0; }
        }
        public double TicksPerSixtyFourth
        {
            get { return TicksPerThirtySecondNote / 2.0; }
        }
        public double TicksPerThirtySecondNote
        {
            get { return TicksPerSixteenthNote / 2.0; }
        }
        public double TicksPerSixteenthNote
        {
            get { return TicksPerEigthNote / 2.0; }
        }
        public double TicksPerEigthNote
        {
            get { return TicksPerQuarterNote / 2.0; }
        }
        public double TicksPerQuarterNote
        {
            get { return TicksPerSecond / QuarterNotesPerSecond; }
        }

        public double TicksPerWholeNote
        {
            get { return TicksPerQuarterNote * 4.0; }
        }

        public double GetSecondsPerBeat(TimeUnit unit)
        {
            var scale = 1.0 / (double)unit;
            return SecondsPerQuarterNote * scale;
        }

        public double SecondsPerEigthNote
        {
            get { return SecondsPerQuarterNote / 2.0; }
        }

        public double SecondsPerSixteenthNote
        {
            get { return SecondsPerEigthNote / 2.0; }
        }
        public double SecondsPerThirtySecondNote
        {
            get { return SecondsPerSixteenthNote / 2.0; }
        }
        public double SecondsPerQuarterNote
        {
            get { return 1.0 / QuarterNotesPerSecond; }
        }

        public double SecondsPerHalfNote
        {
            get { return SecondsPerQuarterNote * 2.0; }
        }

        public double SecondsPerWholeNote
        {
            get { return SecondsPerHalfNote * 2.0; }
        }
        public double WholeNotesPerSecond
        {
            get { return QuarterNotesPerSecond / 4.0; }
        }
        public double QuarterNotesPerSecond
        {
            get { return 1000000.0 / Tempo; }
        }

        public double TicksPerSecond
        {
            get { return 1000000.0 / (Tempo / Owner.Owner.GuitarTrack.SequenceDivision); }
        }

        public double SecondsPerTick
        {
            get { return 1.0 / TicksPerSecond; }
        }

        public static GuitarTempo GetTempo(GuitarTrack track, double tempo)
        {
            var ret = new GuitarTempo(track.Messages, null);
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
            return "Tempo: " + DownTick + " - " + UpTick + " - BPM: " + (int)(QuarterNotesPerSecond * 60).Round(0);
        }

    }


    

}
