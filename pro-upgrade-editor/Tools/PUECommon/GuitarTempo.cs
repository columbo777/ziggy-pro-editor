using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarTempo : GuitarMessage
    {
        public int Tempo;
        public GuitarTempo(GuitarTrack track, MidiEvent ev)
            : base(track, ev)
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

        public override string ToString()
        {
            return AbsoluteTicks + " - Tempo: " + Tempo;
        }

        public void SetStartTime(double startTime)
        {
            this.startTime = startTime;
        }

        public void SetEndTime(double endTime)
        {
            this.endTime = endTime;
        }

        double startTime;
        public override double StartTime
        {
            get
            {
                return startTime;
            }
        }

        double endTime;
        public override double EndTime
        {
            get
            {
                return endTime;
            }
        }
    }


    public class GuitarTimeSignature : GuitarMessage
    {
        public double Numerator = 4;
        public double Denominator = 4;
        public double ClocksPerMetronomeClick = 24;
        public double ThirtySecondNotesPerQuarterNote = 8;


        public GuitarTimeSignature(GuitarTrack track, MidiEvent ev)
            : base(track, ev)
        {
            
            if (ev == null)
            {
                AbsoluteTicks = 0;
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
