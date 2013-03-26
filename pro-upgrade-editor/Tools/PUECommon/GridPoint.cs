using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProUpgradeEditor.Common
{
    public class GridPoint
    {
        public double Time;
        public int Tick;
        public bool IsWholeNote;
        public int MeasureNumber;

        public int ScreenPoint
        {
            get
            {
                return (int)Math.Round(Utility.ScaleUp(Time));
            }
        }
        public TimeUnit TimeUnit;

        public int IntTimeUnit { get { return (int)TimeUnit; } }
    }
}