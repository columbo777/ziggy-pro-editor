using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public struct TimePair : DownUpPair<double>
    {
        double down;
        double up;

        public static TimePair NullValue { get { return new TimePair(double.MinValue, double.MinValue); } }

        public TimePair(double downTime, double upTime)
        {
            this.down = downTime;
            this.up = upTime;
        }
        public TimePair(TimePair times) : this(times.Down, times.Up) { }

        public bool IsCloseBoth(TimePair ticks)
        {
            return IsCloseDownDown(ticks) && IsCloseUpUp(ticks);
        }

        public static bool IsClose(double a, double b)
        {
            return Math.Abs(a - b) < 0.0001;
        }

        public bool IsCloseDownDown(TimePair times)
        {
            return IsClose(Down, times.Down);
        }

        public bool IsCloseUpUp(TimePair times)
        {
            return IsClose(Up, times.Up);
        }

        public bool IsCloseUpDown(TimePair times)
        {
            return IsClose(Up, times.Down);
        }

        public bool IsCloseDownUp(TimePair times)
        {
            return IsClose(Down, times.Up);
        }

        public double TimeLength
        {
            get
            {
                return Up - Down;
            }
            set
            {
                Up = Down + value;
            }
        }

        public bool IsShort { get { return HasNull || IsClose(Up, Down); } }

        public bool IsZeroLength { get { return HasNull || TimeLength <= 0.0; } }
        public bool NotZeroLength { get { return NotNull && TimeLength > 0.0; } }

        public bool IsInvalid { get { return HasNull || Up < Down; } }
        public bool IsValid { get { return !IsInvalid; } }


        public bool IsNull { get { return Up == double.MinValue && Down == double.MinValue; } }

        public bool HasNull { get { return Up == double.MinValue || Down == double.MinValue; } }

        public bool NotNull { get { return !HasNull; } }

        public bool HasLength { get { return !HasNull && (Up > Down); } }

        public static TimePair operator -(TimePair pair, double i)
        {
            return new TimePair(pair.Down - i, pair.Up - i);
        }
        public static TimePair operator +(TimePair pair, double i)
        {
            return new TimePair(pair.Down + i, pair.Up + i);
        }

        public override string ToString()
        {
            return "(" + Down.ToStringEx("null") + ", " + Up.ToStringEx("null") + ")";
        }

        public double Down
        {
            get { return this.down; }
            set { this.down = value; }
        }

        public double Up
        {
            get { return this.up; }
            set { this.up = value; }
        }
    }
}