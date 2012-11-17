using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.DataLayer;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor
{

    public class GuitarMessageComparer : IEqualityComparer<GuitarMessage>
    {
        public bool Equals(GuitarMessage x, GuitarMessage y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(GuitarMessage message)
        {
            return message.GetHashCode();
        }
    }


    public static class PUEExtensions
    {
        public static bool IsEasyMediumHard(this GuitarDifficulty diff)
        {
            return diff.IsEasy() || diff.IsMedium() || diff.IsHard();
        }
        public static bool IsEasy(this GuitarDifficulty diff)
        {
            return (diff & GuitarDifficulty.Easy) == GuitarDifficulty.Easy;
        }
        public static bool IsMedium(this GuitarDifficulty diff)
        {
            return (diff & GuitarDifficulty.Medium) == GuitarDifficulty.Medium;
        }
        public static bool IsHard(this GuitarDifficulty diff)
        {
            return (diff & GuitarDifficulty.Hard) == GuitarDifficulty.Hard;
        }
        public static bool IsExpert(this GuitarDifficulty diff)
        {
            return (diff & GuitarDifficulty.Expert) == GuitarDifficulty.Expert;
        }
        public static bool IsExpertAll(this GuitarDifficulty diff)
        {
            return diff.HasFlag(GuitarDifficulty.Expert) || diff.HasFlag(GuitarDifficulty.All);
        }
        public static bool IsAll(this GuitarDifficulty diff)
        {
            return (diff & GuitarDifficulty.All) == GuitarDifficulty.All;
        }
        public static bool IsUnknown(this GuitarDifficulty diff)
        {
            return diff == GuitarDifficulty.Unknown;
        }

        public static bool IsNone(this GuitarDifficulty diff)
        {
            return diff == GuitarDifficulty.None;
        }

        public static GuitarModifierType GetModifierType(this ChordStrum strum)
        {
            var ret = GuitarModifierType.NoteModifier;
            if (strum == ChordStrum.High)
                ret = GuitarModifierType.ChordStrumHigh;
            if (strum == ChordStrum.Mid)
                ret = GuitarModifierType.ChordStrumMed;
            if (strum == ChordStrum.Low)
                ret = GuitarModifierType.ChordStrumLow;
            return ret;
        }

        public static int GetChannelFromChordStrum(this ChordStrum strum)
        {
            int ret = Utility.ChannelDefault;
            if (strum == ChordStrum.High)
            {
                ret = Utility.ChannelStrumHigh;
            }
            else if (strum == ChordStrum.Mid)
            {
                ret = Utility.ChannelStrumMid;
            }
            else if (strum == ChordStrum.Low)
            {
                ret = Utility.ChannelStrumLow;
            }
            return ret;
        }

        public static ChordStrum GetChordStrumFromChannel(this int i)
        {
            var ret = ChordStrum.Normal;
            if (i == Utility.ChannelStrumHigh)
            {
                ret = ChordStrum.High;
            }
            else if (i == Utility.ChannelStrumMid)
            {
                ret = ChordStrum.Mid;
            }
            else if (i == Utility.ChannelStrumLow)
            {
                ret = ChordStrum.Low;
            }
            return ret;
        }

        public static GuitarDifficulty GetData1Difficulty(this int i, bool isPro)
        {
            return Utility.GetDifficulty(i, isPro);
        }

        public static GuitarTempo GetAtDownTick(this IEnumerable<GuitarTempo> list, int downTick)
        {
            var ret = list.Where(x => x.DownTick <= downTick).LastOrDefault();
            if (ret == null)
            {
                if (list.First().DownTick > downTick)
                {
                    ret = list.First();
                }
                else
                {
                    ret = list.Last();
                }
            }
            return ret;
        }


        public static IEnumerable<GMessage> GetBetweenTick(this IEnumerable<GMessage> list, int downTick, int upTick)
        {
            return list.Where(x => x.DownTick < upTick && x.DownTick >= downTick && x.IsDeleted == false);
        }
        public static IEnumerable<GuitarMessage> GetBetweenTick(this IEnumerable<GuitarMessage> list, int downTick, int upTick)
        {
            return list.Where(x => x.DownTick < upTick && x.UpTick > downTick && x.IsDeleted == false);
        }
        public static IEnumerable<GuitarMessage> GetMessagesByDownTick(this IEnumerable<GuitarMessage> list, int downTick)
        {
            return list.Where(x => x.DownTick == downTick && x.IsDeleted == false);
        }
        
        public static IEnumerable<GuitarMessage> GetBetweenTime(this IEnumerable<GuitarMessage> list, double start, double end)
        {
            return list.Where(x => x.StartTime < end && x.EndTime > start && x.IsDeleted == false);
        }

        public static IEnumerable<GuitarChord> GetChordsAtTime(this IEnumerable<GuitarChord> list, double start, double end)
        {
            return list.Where(x => x.StartTime < end && x.EndTime > start && x.IsDeleted == false);
        }

        public static IEnumerable<GuitarChord> GetChordsByDownTick(this IEnumerable<GuitarChord> list, int downTick)
        {
            return list.Where(x => x.DownTick == downTick && x.IsDeleted == false);
        }

        public static IEnumerable<GuitarChord> GetChordsAtTick(this IEnumerable<GuitarChord> list, int downTick, int upTick)
        {
            return list.Where(x => x.DownTick < upTick && x.UpTick > downTick && x.IsDeleted == false);
        }

        public static int GetMinTick(this IEnumerable<GuitarMessage> list)
        {
            var items = list.Where(x => x != null);
            if (items.Any())
            {
                return items.Min(x => x.DownTick);
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public static int GetMaxTick(this IEnumerable<GuitarMessage> list)
        {
            var items = list.Where(x => x != null);
            if (items.Any())
            {
                return items.Max(x => x.UpTick);
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public static IEnumerable<T> GetMessages<T>(this IEnumerable<GuitarMessage> list)
        {
            return list.Where(x => x is T).Cast<T>();
        }


        public static IEnumerable<GuitarChord> SortTicks(this IEnumerable<GuitarChord> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x => x.DownTick);

            }
            else
            {
                return list.OrderByDescending(x => x.DownTick);
            }
        }
        public static IEnumerable<GuitarMessage> SortTicks(this IEnumerable<GuitarMessage> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x => x.DownTick);

            }
            else
            {
                return list.OrderByDescending(x => x.DownTick);
            }
        }

        public static IEnumerable<GMessage> SortTicks(this IEnumerable<GMessage> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x => x.DownTick);

            }
            else
            {
                return list.OrderByDescending(x => x.DownTick);
            }
        }
        
        public static int Max(this int value, int value2)
        {
            return value > value2 ? value : value2;
        }
        public static int Min(this int value, int value2)
        {
            return value < value2 ? value : value2;
        }

        public static Control SetValueSuspend(this Control control, object value)
        {
            var enabled = control.Enabled;
            control.Enabled = false;
            if (control is TextBox)
            {
                (control as TextBox).Text = (value ?? "").ToString();
            }
            else if (control is CheckBox)
            {
                (control as CheckBox).Checked = (value ?? "").ToString().ToBool();
            }
            else if (control is ComboBox)
            {
                (control as ComboBox).SelectedIndex = (value ?? "").ToString().ToInt(-1);
            }
            else if (control is TrackBar)
            {
                (control as TrackBar).Value = (value ?? "").ToString().ToInt(0);
            }
            control.Enabled = enabled;
            return control;
        }

        public static Control ScrollToEnd(this Control control)
        {
            if (control is TextBoxBase)
            {
                var tb = control as TextBoxBase;
                if (tb.Text.Length > 0)
                    tb.SelectionStart = tb.Text.Length - 1;
                tb.ScrollToCaret();
            }
            return control;
        }

        public static byte[] GetBytes(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }


        public static bool ToBool(this string str, bool nullValue=false)
        {
            var ret = nullValue;
            if (bool.TryParse(str, out ret))
            {
                return ret;
            }
            else
            {
                return nullValue;
            }
        }

        public static int ToInt(this string str, int nullValue= Int32.MinValue)
        {
            int ret=nullValue;
            if (int.TryParse(str, out ret))
            {
                return ret;
            }
            else
            {
                return nullValue;
            }
        }

        public static double ToDouble(this string str, double nullValue = double.MinValue)
        {
            double ret = nullValue;
            if (double.TryParse(str, out ret))
            {
                return ret;
            }
            else
            {
                return nullValue;
            }
        }

        public static DateTime ToDateTime(this string str)
        {
            var ret = DateTime.MinValue;
            if (DateTime.TryParse(str, out ret) == false)
                ret = DateTime.MinValue;
            return ret;
        }
        public static DateTime ToDateTime(this string str, DateTime nullValue)
        {
            var ret = nullValue;
            if (DateTime.TryParse(str, out ret) == false)
                ret = nullValue;
            return ret;
        }
        public static bool IsInt(this string str)
        {
            return str.ToInt().IsNull() == false;
        }
        public static bool IsDouble(this string str)
        {
            return str.ToDouble().IsNull() == false;
        }
        public static bool IsDateTime(this string str)
        {
            return str.ToDateTime().IsNull() == false;
        }

        public static int GetTrainerEventIndex(this string text)
        {
            int ret = int.MinValue;

            if (!string.IsNullOrEmpty(text))
            {
                var lu = text.LastIndexOf('_');
                var eb = text.LastIndexOf(']');
                if (lu != -1 && eb != -1 && eb > lu && eb - lu > 1)
                {
                    ret = text.Substring(lu + 1, eb - (lu + 1)).ToInt();
                }
            }

            return ret;
        }

        public static GuitarTrainerMetaEventType GetGuitarTrainerMetaEventType(this string text)
        {
            var ret = GuitarTrainerMetaEventType.Unknown;

            if (text.IsNull())
                return ret;


            if (!(text.StartsWith(Utility.TextEventBeginTag) &&
                text.EndsWith(Utility.TextEventEndTag)))
            {
                return ret;
            }

            text = text.Trim(new char[] { Utility.TextEventBeginTag.First(), Utility.TextEventEndTag.First() });

            var items = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (items != null && items.Length == 2)
            {
                if (items[0] == Utility.SongTrainerBeginPGText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPGText))
                    {
                        ret = GuitarTrainerMetaEventType.BeginProGuitar;
                    }
                }
                else if (items[0] == Utility.SongTrainerNormPGText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPGText))
                    {
                        ret = GuitarTrainerMetaEventType.ProGuitarNorm;
                    }
                }
                else if (items[0] == Utility.SongTrainerEndPGText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPGText))
                    {
                        ret = GuitarTrainerMetaEventType.EndProGuitar;
                    }
                }
                else if (items[0] == Utility.SongTrainerBeginPBText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPBText))
                    {
                        ret = GuitarTrainerMetaEventType.BeginProBass;
                    }
                }
                else if (items[0] == Utility.SongTrainerNormPBText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPBText))
                    {
                        ret = GuitarTrainerMetaEventType.ProBassNorm;
                    }
                }
                else if (items[0] == Utility.SongTrainerEndPBText)
                {
                    if (items[1].StartsWith(Utility.SongTrainerPBText))
                    {
                        ret = GuitarTrainerMetaEventType.EndProBass;
                    }
                }
            }

            return ret;
            
        
        }

        public static bool IsNull(this DateTime dt)
        {
            return dt == DateTime.MinValue;
        }
        public static bool IsNull(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        

        public static string ToStringEx(this DateTime dt, string fmt = "MM-dd-yyyy")
        {
            return dt.IsNull() ? "" : dt.ToString(fmt);
        }

        public static bool IsNull(this double d, double nullValue = double.MinValue)
        {
            return d == nullValue;
        }

        public static bool IsNull(this int d, int nullValue = Int32.MinValue)
        {
            return d == nullValue;
        }

        public static string ToStringEx(this int d, string nullValue = "")
        {
            return d.IsNull() ? nullValue : d.ToString();
        }

        public static string ToStringEx(this double d, uint decimals = 3, string nullValue = "")
        {
            string fmt = "";
            if (decimals > 0)
            {
                for (int x = 0; x < decimals; x++)
                    fmt += "0";
                fmt = "0." + fmt;
            }
            else
            {
                fmt = "0";
            }
            return d.IsNull() ? nullValue : d.ToString(fmt);
        }



        public static Point GetTopLeft(this Rectangle rect)
        {
            return new Point(rect.Left, rect.Top);
        }
        public static Point GetTopRight(this Rectangle rect)
        {
            return new Point(rect.Right, rect.Top);
        }
        public static Point GetBottomLeft(this Rectangle rect)
        {
            return new Point(rect.Left, rect.Bottom);
        }
        public static Point GetBottomRight(this Rectangle rect)
        {
            return new Point(rect.Right, rect.Bottom);
        }



        public static int DistanceSq(this Point a, Point b)
        {
            return (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static float Distance(this Point a, Point b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static Point ClosestPointOnRectangle(this Point v, Rectangle rect)
        {
            return new Point(v.X < rect.Left ? rect.Left : v.X > rect.Right ? rect.Right : v.X,
                v.Y < rect.Top ? rect.Top : v.Y > rect.Bottom ? rect.Bottom : v.Y);
        }

        public static float DistanceSq(this Point v, Rectangle rect)
        {
            return v.ClosestPointOnRectangle(rect).DistanceSq(v);
        }

        public static float Distance(this Point v, Rectangle rect)
        {
            return v.ClosestPointOnRectangle(rect).Distance(v);
        }
        

    }

}
