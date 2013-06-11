using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProUpgradeEditor.Common
{
    

    

    public class NoteGrid
    {
        public List<GridPoint> Points;
        TrackEditor editor;

        TimeUnit[] timeUnits;

        public NoteGrid(TrackEditor editor)
        {
            timeUnits = new TimeUnit[] { TimeUnit.Whole, TimeUnit.Half, TimeUnit.Quarter, TimeUnit.Eight, TimeUnit.Sixteenth, TimeUnit.ThirtySecond, TimeUnit.SixtyFourth, TimeUnit.OneTwentyEigth };

            this.editor = editor;
            Points = new List<GridPoint>();
            Build(GetTimeUnitFromGridScalar(editor.GridScalar));
        }

        public TimeUnit GetTimeUnitFromGridScalar(double scalar)
        {
            return (TimeUnit)Math.Round(1.0 / scalar);
        }

        public TimeUnit GetTimeUnitFromTimeSignature(GuitarTimeSignature timeSig)
        {
            return (TimeUnit)(128.0 / timeSig.Denominator);
        }


        public void Build(TimeUnit unit)
        {
            var totalTime = 0.0;
            var totalTick = 0.0;

            Points.Clear();

            double nextBeat = 0.0;
            int measureNumber = 0;
            double quarterCount = 0;

            foreach (var tempo in editor.Messages.Tempos.ToList())
            {
                var scale = 1.0 / ((int)unit).ToDouble();

                var tempoLength = tempo.TimeLength;

                var beatTime = tempo.SecondsPerWholeNote;
                var beatTicks = tempo.TicksPerWholeNote;

                var currentTime = totalTime;
                var currentTick = totalTick;
                var division = 1.0 / scale;

                while (currentTime.Round(4) < totalTime + tempoLength.Round(4))
                {
                    for (int d = 0; d < division.ToInt(); d++)
                    {
                        var divisionTime = beatTime / division;
                        var divisionTicks = beatTicks / division;

                        var isWhole = Math.IEEERemainder(quarterCount.Round(4), 4.0).Round(4) == 0.0;
                        quarterCount += scale * 4;

                        var p = new GridPoint()
                        {
                            Tick = currentTick.Round(),
                            Time = currentTime,
                            TimeUnit = unit,
                            IsWholeNote = isWhole,
                            MeasureNumber = measureNumber,
                        };

                        if (isWhole)
                        {
                            measureNumber++;
                        }

                        Points.Add(p);
                        nextBeat += divisionTime;
                        currentTime += divisionTime;
                        currentTick += divisionTicks;

                        if (currentTime.Round(4) >= tempoLength.Round(4))
                        {
                            var delta = currentTime - tempoLength;
                            break;
                        }
                    }
                }
                totalTime += tempoLength;
                totalTick += tempo.TicksPerSecond * tempoLength;
            }
        }
    }
}
