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
    public struct TickPair : IComparable<TickPair>
    {
        public int Down;
        public int Up;

        public static TickPair NullValue { get { return new TickPair(Int32.MinValue, Int32.MinValue); } }

        public TickPair(int downTick, int upTick)
        {
            if (downTick > upTick)
            {
                upTick = downTick;
            }
            Down = downTick;
            Up = upTick;
        }

        public TickPair(TickPair ticks) : this(ticks.Down, ticks.Up) { }

        public bool IsClose(int tick)
        {
            return Utility.IsCloseTick(Down, tick) || Utility.IsCloseTick(Up, tick);
        }

        public bool IsCloseBoth(TickPair ticks)
        {
            return IsCloseDownDown(ticks) && IsCloseUpUp(ticks);
        }

        public bool IsCloseDownDown(TickPair ticks)
        {
            return Utility.IsCloseTick(Down, ticks.Down);
        }

        public bool IsCloseUpUp(TickPair ticks)
        {
            return Utility.IsCloseTick(Up, ticks.Up);
        }

        public bool IsCloseUpDown(TickPair ticks)
        {
            return Utility.IsCloseTick(Up, ticks.Down);
        }

        public bool IsCloseDownUp(TickPair ticks)
        {
            return Utility.IsCloseTick(Down, ticks.Up);
        }

        public int TickLength { get { return Up - Down; } }

        public bool IsShort { get { return HasNull || Utility.IsCloseTick(Up, Down); } }

        public bool IsZeroLength { get { return HasNull || TickLength <= 0; } }
        public bool NotZeroLength { get { return NotNull && TickLength > 0; } }

        public bool IsInvalid { get { return HasNull || Up < Down; } }
        public bool IsValid { get { return !IsInvalid; } }

        public bool IsNull { get { return Up == Int32.MinValue && Down == Int32.MinValue; } }

        public bool HasNull { get { return Up == Int32.MinValue || Down == Int32.MinValue; } }

        public bool NotNull { get { return !HasNull; } }

        public bool HasLength { get { return !HasNull && (Up > Down); } }

        public static TickPair operator -(TickPair pair, int i)
        {
            return new TickPair(pair.Down - i, pair.Up - i);
        }
        public static TickPair operator +(TickPair pair, int i)
        {
            return new TickPair(pair.Down + i, pair.Up + i);
        }
        public static bool operator !=(TickPair a, TickPair b)
        {
            return !(a.Down == b.Down && a.Up == b.Up);
        }
        public static bool operator ==(TickPair a, TickPair b)
        {
            return (a.Down == b.Down && a.Up == b.Up);
        }
        public override bool Equals(object obj)
        {
            return GetHashCode() == ((TickPair)obj).GetHashCode();
        }
        public override int GetHashCode()
        {
            //http://en.wikipedia.org/wiki/Cantor_pairing_function#Cantor_pairing_function
            //((x + y) * (x + y + 1) / 2) + y;
            return ((Down + Up) * (Down + Up + 1) / 2) + Up;
        }

        public TickPair Expand(int amount)
        {
            var d = Down - amount;
            var u = Up + amount;
            if (u < d)
            {
                d = Down;
                u = Up;
            }
            return new TickPair(d, u);
        }
        public TickPair Offset(int amount)
        {
            return new TickPair(Down + amount, Up + amount);
        }
        public TickPair Scale(double amount)
        {
            return new TickPair((Down.ToDouble() * amount).Round(), (Up.ToDouble() * amount).Round());
        }
        public TickPair ExtendTo(int up)
        {
            return new TickPair(Down, up);
        }
        public override string ToString()
        {
            return "(" + Down.ToStringEx("null") + ", " + Up.ToStringEx("null") + ")";
        }

        public int CompareTo(TickPair other)
        {
            if (Down < other.Down)
                return -1;

            if (Down > other.Down)
                return 1;

            if (Up < other.Up)
                return -1;

            if (Up > other.Up)
                return 1;

            return 0;
        }
    }
}