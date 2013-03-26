using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public static class GridScale
    {
        public static readonly double WholeNote = 1.0;
        public static readonly double HalfNote = 0.5;
        public static readonly double QuarterNote = 0.25;
        public static readonly double EigthNote = 0.125;
        public static readonly double SixteenthNote = 0.0625;
        public static readonly double ThirtySecondNote = 0.03125;
        public static readonly double SixtyFourthNote = 0.015625;
        public static readonly double OneTwentyEightNote = 0.0078125;

        public static TimeUnit GetTimeUnit(double scale)
        {
            if (scale == WholeNote)
                return TimeUnit.Whole;
            else if (scale == HalfNote)
                return TimeUnit.Half;
            else if (scale == QuarterNote)
                return TimeUnit.Quarter;
            else if (scale == EigthNote)
                return TimeUnit.Eight;
            else if (scale == SixteenthNote)
                return TimeUnit.Sixteenth;
            else if (scale == ThirtySecondNote)
                return TimeUnit.ThirtySecond;
            else if (scale == SixtyFourthNote)
                return TimeUnit.SixtyFourth;
            else //if (scale == OneTwentyEightNote)
                return TimeUnit.OneTwentyEigth;
        }
    }
}