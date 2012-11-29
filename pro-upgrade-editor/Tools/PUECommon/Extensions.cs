using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.DataLayer;
using ProUpgradeEditor.Common;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;

namespace ProUpgradeEditor
{

    public class DownUpPair<T>
    {
        public T Down { get; set; }
        public T Up { get;set;}
        public DownUpPair(T down, T up)
        {
            this.Down = down;
            this.Up = up;
        }
    }

    public class GuitarMessagePair : DownUpPair<GuitarMessage> 
    {
        public GuitarMessagePair() : this(null,null) { }
        public GuitarMessagePair(GuitarMessage down) : this(down, null){ }
        public GuitarMessagePair(GuitarMessage down, GuitarMessage up) : base(down,up) { }
    }
    public class MidiEventPair : DownUpPair<MidiEvent>
    {
        public MidiEventPair() : this(null, null) { }
        public MidiEventPair(MidiEvent down) : this(down, null) { }
        public MidiEventPair(MidiEvent down, MidiEvent up) : base(down, up) { }
    }

    
    public class GuitarMessageSorter : IComparer<GuitarMessage>
    {
        public int Compare(GuitarMessage x, GuitarMessage y)
        {
            var ret = 0;

            if (x.AbsoluteTicks < y.AbsoluteTicks)
                ret = -1;
            else if (x.AbsoluteTicks > y.AbsoluteTicks)
                ret = 1;
            else
            {
                if (x.IsChannelEvent() && y.IsChannelEvent())
                {
                    if (x.Command == ChannelCommand.NoteOff && y.Command == ChannelCommand.NoteOn)
                        ret = -1;
                    else if (x.Command == ChannelCommand.NoteOn && y.Command == ChannelCommand.NoteOff)
                        ret = 1;
                    else if ((int)x.Difficulty < (int)y.Difficulty)
                        return -1;
                    else if ((int)x.Difficulty > (int)y.Difficulty)
                        return 1;
                }
            }
            
            return ret;
        }
    }


    public static class PUEExtensions
    {
        public static void IfObjectNotNull<T>(this T o, Action<T> func, Action<T> Else = null)
        {
            if (o != null)
            {
                func(o);
            }
            else
            {
                if (Else != null)
                    Else(o);
            }
        }

        public static bool IsMetaEvent(this GuitarMessage ev)
        {
            return ev.MidiEvent.IsMetaEvent();
        }

        public static bool IsTextEvent(this GuitarMessage ev)
        {
            return ev.MidiEvent.IsTextEvent();
        }

        public static bool IsChannelEvent(this GuitarMessage ev)
        {
            return ev.MidiEvent.IsChannelEvent();
        }
        public static bool IsTempoEvent(this MidiEvent ev)
        {
            return ev.IsMetaEvent() && ev.MetaMessage.MetaType == MetaType.Tempo;
        }
        public static bool IsTimeSigEvent(this MidiEvent ev)
        {
            return ev.IsMetaEvent() && ev.MetaMessage.MetaType == MetaType.TimeSignature;
        }
        public static bool IsTempoTimesigEvent(this MidiEvent ev)
        {
            return ev.IsTempoEvent() || ev.IsTimeSigEvent();
        }
        public static int ToInt(this double d)
        {
            var ret = Int32.MinValue;
            try { ret = (int)Math.Round(d); }
            catch { ret = Int32.MinValue; }
            return ret;
        }
        public static bool IsMetaEvent(this MidiEvent ev)
        {
            return ev != null && ev.MetaMessage != null;
        }

        public static void SetSelectedItem<T>(this ListBox list, T item) where T : class
        {
            if (item != null && list.Items.ToEnumerable<T>().Any(x => x == item))
            {
                list.SelectedItem = item;
            }
            else
            {
                list.SelectedItem = null;
            }
        }

        public static IEnumerable<Ret> ToEnumerable<Ret>(this IEnumerable obj) where Ret : class
        {
            var enumer = obj.GetEnumerator();
            while (enumer.MoveNext())
            {
                yield return enumer.Current as Ret;
            }
        }
        public static bool IsTextEvent(this MidiEvent ev)
        {
            return ev != null && ev.MetaMessage != null && (ev.MetaMessage.MetaType != MetaType.EndOfTrack &&
                ev.MetaMessage.MetaType != MetaType.TrackName);
        }

        public static bool IsChannelEvent(this MidiEvent ev)
        {
            return ev != null && ev.ChannelMessage != null;
        }

        public static bool IsFileTypePro(this Sequence seq) { return seq.FileType == FileType.Pro; }
        public static bool IsFileTypePro(this Track t) { return t.FileType == FileType.Pro; }


        public static IEnumerable<MidiEvent> GetChanMessagesByDifficulty(this Track t, GuitarDifficulty diff) 
        {
            return t.ChanMessages.Where(x => 
                diff.HasFlag(x.Data1.GetData1Difficulty(t.IsFileTypePro()))
                ).ToList();
        }

        public static bool HasDropObject<T>(this DragEventArgs args) where T : class
        {
            return args.GetDropObject<T>() != null;
        }

        public static T GetDropObject<T>(this DragEventArgs args) where T : class
        {
            T ret = null;
            if (args.Data.GetDataPresent(typeof(T)))
            {
                ret = args.Data.GetData(typeof(T)) as T;
            }
            return ret;
        }

        public static bool IsEmpty(this string str) { return string.IsNullOrEmpty(str); }

        public static void IfNotNull(this double d, Action<double> func, Action<double> Else = null)
        {
            if (!d.IsNull())
            {
                func(d);
            }
            else { if (Else != null) { Else(d); } }
        }

        public static void IfNotNull(this DateTime d, Action<DateTime> func, Action<DateTime> Else = null)
        {
            if (!d.IsNull())
            {
                func(d);
            }
            else { if (Else != null) { Else(d); } }
        }
        
        public static void IfNotEmpty(this string d, Action<string> func, Action<string> Else = null)
        {
            if (!string.IsNullOrEmpty(d))
            {
                func(d);
            }
            else { if (Else != null) { Else(d); } }
        }
        public static string IfNotEmpty(this string d, Func<string, string> func, Func<string, string> Else = null)
        {
            var ret = d;
            if (!string.IsNullOrEmpty(d))
            {
                ret = func(d);
            }
            else { if (Else != null) { ret = Else(d); } }
            return ret;
        }

        public static IEnumerable<GuitarMessage> ToGuitarMessage(this IEnumerable<MidiEvent> list, GuitarTrack track)
        {
            return list.Select(x => new GuitarMessage(track, x)).ToList();
        }
        public static GuitarMessage ToGuitarMessage(this MidiEvent ev, GuitarTrack track)
        {
            return new GuitarMessage(track, ev);
        }
        public static bool IsBetween(this int i, int lowValue, int highValue, bool includingValue = true)
        {
            if (lowValue > highValue)
            {
                var v = highValue;
                highValue = lowValue;
                lowValue = v;
            }
            if (includingValue)
            {
                return i >= lowValue && i <= highValue;
            }
            else
            {
                return i > lowValue && i < highValue;
            }
        }

        public static bool IsProNoteModifier(this MidiEvent ev, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            if (ev.Data1.IsNull())
                return false;

            if (difficulty.IsUnknown())
            {
                return Utility.AllSlideData1.Contains(ev.Data1) ||
                    Utility.AllHammeronData1.Contains(ev.Data1) ||
                    Utility.AllStrumData1.Contains(ev.Data1);
            }
            else
            {
                return (ev.Data1 == Utility.GetSlideData1(difficulty)) ||
                    (ev.Data1 == Utility.GetHammeronData1(difficulty)) ||
                    (ev.Data1 == Utility.GetStrumData1(difficulty));
            }
        }

        public static bool IsArpeggioEvent(this MidiEvent ev)
        {
            return Utility.AllArpeggioData1.Contains(ev.Data1);
        }
        public static bool IsPowerupEvent(this MidiEvent ev)
        {
            return Utility.PowerupData1==ev.Data1;
        }
        public static bool IsSoloEvent(this MidiEvent ev)
        {
            return ev.Data1.IsSolo(ev.Owner.IsFileTypePro());
        }
        public static bool IsMultiStringTremeloEvent(this MidiEvent ev)
        {
            return Utility.MultiStringTremeloData1==ev.Data1;
        }

        public static bool IsSingleStringTremeloEvent(this MidiEvent ev)
        {
            return Utility.SingleStringTremeloData1==ev.Data1;
        }

        public static bool IsBigRockEnding(this MidiEvent ev)
        {
            return Utility.BigRockEndingData1.Contains(ev.Data1);
        }
        public static bool IsProModifier(this MidiEvent x)
        {
            var ret = false;
            var d1 = x.Data1;
            if (!d1.IsNull())
            {
                ret = Utility.AllArpeggioData1.Contains(d1) ||
                    d1 == Utility.PowerupData1 ||
                    d1 == Utility.SoloData1 ||
                    d1 == Utility.MultiStringTremeloData1 ||
                    d1 == Utility.SingleStringTremeloData1 ||
                    Utility.BigRockEndingData1.Contains(d1);
            }
            return ret;
        }
        
        public static bool IsHandPositionEvent(this MidiEvent ev) { return ev.Data1 == Utility.HandPositionData1; }

        public static string GetIfEmpty(this string str, Func<string> other)
        {
            var ret = string.Empty;
            try
            {
                if (str.IsEmpty())
                {
                    return other();
                }
                else
                {
                    return str;
                }
            }
            catch { }
            return ret;
        }
        public static DateTime GetIfNull(this DateTime dt, Func<DateTime> other)
        {
            if (dt.IsNull())
            {
                return other();
            }
            else
            {
                return dt;
            }
        }
        public static void IfNotNull(this int d, Action<int> func, Action<int> Else = null)
        {
            if (!d.IsNull())
            {
                func(d);
            }
            else { if (Else != null) { Else(d); } }
        }

        public static int GetIfNull(this int i, int value, int nullValue = int.MinValue) 
        {
            return i.IsNull(nullValue) ? value : i;
        }

        public static Track GetPrimaryTrack(this Sequence seq, bool preferGuitar17=true)
        {
            Track ret = null;
            if (preferGuitar17)
            {
                ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName17());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName22());
                if(ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsBassTrackName17());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsBassTrackName22());
            }
            else
            {
                ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName22());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName17());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsBassTrackName22());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsBassTrackName17());
            }

            if (ret == null)
                ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName5());
            if (ret == null)
                ret = seq.FirstOrDefault(x => x.Name.IsBassTrackName5());
            if (ret == null)
                ret = seq.LastOrDefault();
            return ret;
        }

        public static IEnumerable<Track> GetGuitarBassTracks(this IEnumerable<Track> list)
        {
            return list.Where(x => x.Name.IsGuitarTrackName() || x.Name.IsBassTrackName());
        }

        public static IEnumerable<MidiEvent> RemoveDifficulty(this Track track, GuitarDifficulty diff)
        {
            var ret = track.ChanMessages.Where(x =>
                diff.HasFlag(x.ChannelMessage.Data1.GetData1Difficulty(track.IsFileTypePro()))).ToList();
            ret.ForEach(x => track.Remove(x));
            return ret;
        }

        public static Sequence ConvertToPro(this Sequence seq5, bool onlyGBTempo=true)
        {
            var seq = new Sequence(FileType.Pro, seq5.Division);

            if (onlyGBTempo)
            {
                var tempo = seq5.Tracks.FirstOrDefault();
                if(tempo != null)
                    seq.AddTempo(tempo.Clone());

                seq5.GetGuitarBassTracks().ForEach(x => seq.Add(x.ConvertToPro()));
            }
            else
            {
                seq5.ForEach(x => seq.Add(x.ConvertToPro()));
            }
            return seq;
        }

        public static int IndexOf<T>(this IEnumerable<T> list, T item)
        {
            var ret = Int32.MinValue;
            var l = list.ToList();
            if (l.Contains(item))
            {
                ret = l.IndexOf(item);
            }
            return ret;
        }


        public static int GetTrackIndex(this Track t)
        {
            return t.Sequence.IndexOf(t);
        }
        public static bool HasTrack(this Sequence seq, Track t) { return seq.IndexOf(t).IsNull() == false; }

        public static bool IsTempo(this Track t) 
        {
            if (t.Sequence.HasTrack(t) && t.Sequence.IndexOf(t) == 0)
                return true;

            if(t.Name.Equals("tempo", StringComparison.OrdinalIgnoreCase))
                return true;

            return t.ChanMessages.Any() == false && (t.Tempo.Any() || t.TimeSig.Any());
        }

        public static Track ConvertToPro(this Track t5, GuitarDifficulty targetDifficulty = GuitarDifficulty.Unknown)
        {
            var ret = new Track(FileType.Pro, t5.Name.GetProTrackNameFromG5());
            
            if ((t5.Name.IsGuitarTrackName5() || t5.Name.IsBassTrackName5()) && t5.ChanMessages.Any())
            {
                if (!targetDifficulty.IsUnknown())
                {

                    t5.ChanMessages.Where(x => targetDifficulty.HasFlag(x.Data1.GetData1Difficulty(t5.IsFileTypePro()))).ForEach(x =>
                        {
                            var proCM = x.ChannelMessage.ConvertToPro(targetDifficulty);
                            if (proCM != null)
                            {
                                ret.Insert(x.AbsoluteTicks, proCM);
                            }
                        });

                    if (targetDifficulty.IsExpertAll())
                    {
                        t5.Meta.Where(meta => meta.IsTextEvent()).ForEach(meta =>
                        {
                            ret.Insert(meta.AbsoluteTicks, meta.MetaMessage.CloneMeta());
                        });
                    }
                }
                else
                {

                    t5.ChanMessages.ForEach(cm =>
                    {
                        var proCM = cm.ChannelMessage.ConvertToPro(targetDifficulty);
                        if (proCM != null)
                        {
                            ret.Insert(cm.AbsoluteTicks, proCM);
                        }
                    });

                    t5.Meta.Where(meta => meta.IsTextEvent()).ForEach(meta =>
                    {
                        ret.Insert(meta.AbsoluteTicks, meta.Clone());
                    });
                }
            }
            else
            {
                ret.Merge(t5);
            }
            return ret;
        }


        public static Track Clone(this Track t, FileType targetType = FileType.Unknown)
        {
            Track ret = new Track(targetType == FileType.Unknown ? t.FileType : targetType, t.Name);
            if (t.FileType != ret.FileType && t.FileType == FileType.Pro)
                ret.Merge(t.ConvertToG5());
            else if (t.FileType != ret.FileType && t.FileType == FileType.Guitar5)
                ret.Merge(t.ConvertToPro());
            else
                ret.Merge(t);
            return ret;
        }

        public static IEnumerable<MidiEvent> GetChanMessagesNotInDifficulty(this Track t, GuitarDifficulty diff)
        {
            return t.ChanMessages.Where(x => 
                !diff.HasFlag(x.ChannelMessage.Data1.GetData1Difficulty(t.IsFileTypePro())));
        }

        public static Track CloneDifficulty(this Track t, GuitarDifficulty sourceDifficulty, GuitarDifficulty destDifficulty, FileType type = FileType.Unknown)
        {
            Track ret = new Track(type == FileType.Unknown ? t.FileType : type, t.Name);
            
            var tmess = t.GetChanMessagesByDifficulty(sourceDifficulty);

            if (t.IsFileTypePro() && type == FileType.Guitar5)
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertToG5(destDifficulty)));
            else if (t.IsFileTypePro() == false && type == FileType.Pro)
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertToPro(destDifficulty)));
            else if(t.IsFileTypePro())
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertDifficultyPro(destDifficulty)));
            else
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertDifficultyG5(destDifficulty)));

            return ret;
        }

        public static Track ConvertToG5(this Track t6, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            var ret = new Track(FileType.Guitar5, t6.Name);

            t6.ChanMessages.ForEach(cm=>
            {
                cm.ChannelMessage.ConvertToG5(difficulty).IfObjectNotNull(cm5 =>
                    ret.Insert(cm.AbsoluteTicks, cm5));
            });

            t6.Meta.Where(meta => meta.IsTextEvent()).ForEach(meta =>
            {
                ret.Insert(meta.AbsoluteTicks, meta.Clone());
            });

            return ret;
        }
        public static int GetStringLowEPro(this GuitarDifficulty difficulty)
        {
            return Utility.GetStringLowE(difficulty);
        }

        public static int GetStringLowE5(this GuitarDifficulty difficulty)
        {
            return Utility.GetStringLowE5(difficulty);
        }

        public static MetaMessage CloneMeta(this MetaMessage mess)
        {
            var mb = new MetaTextBuilder(mess.MetaType);
            mb.Text = mess.Text;
            mb.Build();
            return mb.Result;
        }

        public static ChannelMessage ConvertDifficultyPro(this MidiEvent ev, 
            GuitarDifficulty targetDifficulty)
        {
            return ev.ChannelMessage.ConvertDifficultyPro(targetDifficulty);
        }
        
        public static ChannelMessage ConvertDifficultyPro(this ChannelMessage cm, 
            GuitarDifficulty targetDifficulty)
        {
            ChannelMessage ret = null;
            var cb = new ChannelMessageBuilder(cm);

            var noteString = cm.Data1.GetNoteString6();
            if (noteString != -1)
            {
                cb.Data2 = cm.Data2;
                cb.Data1 = targetDifficulty.GetStringLowEPro() + noteString;
                cb.MidiChannel = cm.MidiChannel;
                cb.Command = cm.Command;
                cb.Build();

                ret = cb.Result;
            }
            else
            {
                var modData1 = Utility.GetModifierData1ForDifficulty(cm.Data1, cm.Data1.GetData1Difficulty(true), targetDifficulty);
                if (modData1 != -1)
                {
                    cb.Data2 = cm.Data2;
                    cb.Data1 = modData1;
                    cb.MidiChannel = cm.MidiChannel;
                    cb.Command = cm.Command;
                    cb.Build();

                    ret = cb.Result;
                }
                else
                {
                    if (targetDifficulty.IsUnknown() || 
                        targetDifficulty.IsAll())
                    {
                        if (cm.Data1.IsSoloExpert(false))
                        {
                            cb.Data2 = cm.Data2;
                            cb.Data1 = Utility.SoloData1;
                            cb.MidiChannel = cm.MidiChannel;
                            cb.Command = cm.Command;
                            cb.Build();

                            ret = cb.Result;
                        }
                        else if (cm.Data1.IsPowerup() || cm.Data1.IsBigRockEnding())
                        {
                            cb.Data2 = cm.Data2;
                            cb.Data1 = cm.Data1;
                            cb.MidiChannel = cm.MidiChannel;
                            cb.Command = cm.Command;
                            cb.Build();

                            ret = cb.Result;
                        }
                    }
                }
                
            }
            return ret;
        }

        public static ChannelMessage ConvertDifficultyG5(this ChannelMessage cm, GuitarDifficulty targetDifficulty)
        {
            ChannelMessage ret = null;
            var cb = new ChannelMessageBuilder(cm);

            var noteString = cm.Data1.GetNoteString5();
            if (noteString != -1)
            {
                cb.Data2 = cm.Command.GetData2();
                cb.Command = cm.Command;
                cb.Data1 = targetDifficulty.GetStringLowE5() + noteString;
                cb.MidiChannel = Utility.ChannelDefault;
                
                cb.Build();

                ret = cb.Result;
            }
            
            return ret;
        }

        public static ChannelMessage ConvertToPro(this ChannelMessage cm, GuitarDifficulty targetDifficulty = GuitarDifficulty.Unknown)
        {
            ChannelMessage ret = null;
            var cb = new ChannelMessageBuilder(cm);
            
            if (targetDifficulty.IsUnknown())
                targetDifficulty = cm.Data1.GetData1Difficulty(false);
            
            var noteString = cm.Data1.GetNoteString5();
            if (noteString != -1)
            {
                cb.Data2 = cm.Command.GetData2();
                cb.Command = cm.Command;
                cb.Data1 = targetDifficulty.GetStringLowEPro() + noteString;
                cb.MidiChannel = Utility.ChannelDefault;
                
                cb.Build();

                ret = cb.Result;
            }
            else
            {
                var modData1 = Utility.GetModifierData1ForDifficulty(cm.Data1,
                    cm.Data1.GetData1Difficulty(false), targetDifficulty);
                if (modData1 != -1)
                {
                    cb.Data2 = cm.Command.GetData2();
                    cb.Command = cm.Command;
                    cb.Data1 = modData1;
                    cb.MidiChannel = Utility.ChannelDefault;
                    
                    cb.Build();

                    ret = cb.Result;
                }
                else if (targetDifficulty.IsEasy())
                {
                }
                else if (targetDifficulty.IsMedium())
                {
                }
                else if (targetDifficulty.IsHard())
                {
                }
                else if (targetDifficulty.IsUnknown() || targetDifficulty.IsAll())
                {
                    if (cm.Data1.IsSoloExpert(false))
                    {
                        cb.Data2 = cm.Command.GetData2();
                        cb.Command = cm.Command;
                        cb.Data1 = Utility.SoloData1;
                        cb.MidiChannel = Utility.ChannelDefault;

                        cb.Build();

                        ret = cb.Result;
                    }
                    else if (cm.Data1.IsPowerup() || cm.Data1.IsBigRockEnding())
                    {
                        cb.Data2 = cm.Command.GetData2();
                        cb.Command = cm.Command;
                        cb.Data1 = cm.Data1;
                        cb.MidiChannel = Utility.ChannelDefault;

                        cb.Build();

                        ret = cb.Result;
                    }
                }
            }
            return ret;
        }
        public static bool IsBigRockEnding(this int data1)
        {
            return Utility.BigRockEndingData1.Contains(data1);
        }
        public static ChannelMessage ConvertToG5(this ChannelMessage cm, GuitarDifficulty targetDifficulty = GuitarDifficulty.Unknown)
        {
            ChannelMessage ret = null;
            var cb = new ChannelMessageBuilder(cm);

            if (targetDifficulty.IsUnknown())
                targetDifficulty = cm.Data1.GetData1Difficulty(true);

            if (cm.Data1.IsProStringData1())
            {
                if (cm.Data1.GetNoteString6() == 5)
                    cb.MidiChannel = 1;

                cb.Data2 = cm.Command.GetData2();
                cb.Data1 = targetDifficulty.GetStringLowE5() + cm.Data1.GetNoteString6();
                cb.MidiChannel = Utility.ChannelDefault;
                cb.Command = cm.Command;
                cb.Build();

                ret = cb.Result;
            }
            else
            {
                if (targetDifficulty != GuitarDifficulty.Unknown)
                {
                    if (cm.Data1.IsSoloExpert(true))
                    {
                        cb.Data2 = cm.Command.GetData2();
                        cb.Data1 = Utility.GetSoloData1_G5(targetDifficulty);
                        cb.MidiChannel = Utility.ChannelDefault;
                        cb.Command = cm.Command;
                        cb.Build();
                        ret = cb.Result;
                    }
                    else if (cm.Data1.IsPowerup())
                    {
                        cb.Data2 = cm.Command.GetData2();
                        cb.Data1 = Utility.PowerupData1;
                        cb.MidiChannel = Utility.ChannelDefault;
                        cb.Command = cm.Command;
                        cb.Build();
                        ret = cb.Result;
                    }
                }
            }
            return ret;
        }



        public static bool IsProStringData1(this int data1)
        {
            return Utility.IsData1String6(data1);
        }

        public static int GetData2(this ChannelCommand cm)
        {
            return cm == ChannelCommand.NoteOn ? 100 : 0;
        }

        public static bool IsPowerup(this int data1)
        {
            return Utility.PowerupData1 == data1;
        } 

        public static bool IsSolo(this int data1, bool isPro)
        {
            return isPro ? (Utility.SoloData1 == data1) : (Utility.SoloData1_G5.Contains(data1) || data1 == Utility.SoloData1);
        }
        public static bool IsSoloExpert(this int data1, bool isPro)
        {
            return isPro ? (Utility.SoloData1 == data1) : (Utility.ExpertSoloData1_G5==data1 || data1 == Utility.SoloData1);
        }
        public static int GetNoteString6(this int data1)
        {
            return Utility.GetNoteString(data1);
        }
        public static int GetNoteString5(this int data1)
        {
            return Utility.GetNoteString5(data1);
        }
        public static bool IsNoteString5(this int data1)
        {
            return data1.GetNoteString5() != -1;
        }

        public static bool IsNoteString6(this int data1)
        {
            return data1.GetNoteString6() != -1;
        }
        public static bool IsProTrackName(this string str)
        {
            return GuitarTrack.TrackNames6.Contains(str);
        }
        public static bool IsProTrackName17(this string str)
        {
            return str.IsEmpty()==false && (GuitarTrack.GuitarTrackName17 == str || GuitarTrack.BassTrackName17 == str);
        }
        public static bool IsProTrackName22(this string str)
        {
            return str.IsEmpty() == false && (GuitarTrack.GuitarTrackName22 == str || GuitarTrack.BassTrackName22 == str);
        }
        public static bool IsGuitarTrackName17(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.GuitarTrackName17 == str ? true : false;
        }
        public static bool IsGuitarTrackName22(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.GuitarTrackName22 == str ? true : false;
        }
        public static bool IsBassTrackName17(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.BassTrackName17 == str ? true : false;
        }
        public static bool IsBassTrackName22(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.BassTrackName22 == str ? true : false;
        }
        public static bool IsGuitarTrackName5(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.IsGuitarTrackName(str) && GuitarTrack.IsTrackName22(str)==false;
        }
        public static bool IsGuitarTrackName6(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.GuitarTrackNames6.Contains(str);
        }
        public static bool IsBassTrackName6(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.BassTrackNames6.Contains(str);
        }
        public static bool IsBassTrackName5(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.IsBassTrackName(str) && GuitarTrack.IsTrackName22(str) == false;
        }
        public static bool IsGuitarTrackName(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.IsGuitarTrackName(str);
        }
        public static bool IsBassTrackName(this string str)
        {
            return str.IsEmpty() ? false : GuitarTrack.IsBassTrackName(str);
        }
        public static string GetProTrackNameFromG5(this string g5TrackName, bool getTrack17 = true)
        {
            var ret = g5TrackName;
            if (GuitarTrack.IsGuitarTrackName(g5TrackName))
            {
                ret = getTrack17 ? GuitarTrack.GuitarTrackName17 : GuitarTrack.GuitarTrackName22;
            }
            else if (GuitarTrack.IsBassTrackName(g5TrackName))
            {
                ret = getTrack17 ? GuitarTrack.BassTrackName17 : GuitarTrack.BassTrackName22;
            }
            return ret;
        }
        public static string GetG5TrackNameFromPro(this string proTrackName)
        {
            var ret = proTrackName;
            if (proTrackName.IsGuitarTrackName())
            {
                ret = GuitarTrack.GuitarTrackName5;
            }
            else if (proTrackName.IsBassTrackName())
            {
                ret = GuitarTrack.BassTrackName5;
            }
            return ret;
        }
        public static bool IsEasyMediumHardExpert(this GuitarDifficulty diff)
        {
            return diff.IsEasy() || diff.IsMedium() || diff.IsHard() || diff.IsExpert();
        }
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

        public static bool IsHandPositionMessage(this GuitarMessage mess)
        {
            return mess.Data1 == Utility.HandPositionData1;
        }


        public static GuitarDifficulty GetData1Difficulty(this int i, bool isPro)
        {
            return Utility.GetDifficulty(i, isPro);
        }

        public static GuitarTempo GetAtDownTick(this IEnumerable<GuitarTempo> list, int downTick)
        {
            var ret = list.LastOrDefault(x => x.DownTick <= downTick);
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

        public static IEnumerable<MidiEvent> GetEvents(this GuitarTrack tr)
        {
            return tr.GetTrack().Events;
        }

        public static IEnumerable<MidiEvent> GetDifficulty(this GuitarTrack track, GuitarDifficulty diff)
        {
            return track.GetEvents().GetDifficulty(track.IsPro, diff);
        }

        public static IEnumerable<MidiEvent> GetDifficulty(this IEnumerable<MidiEvent> list, bool isPro, GuitarDifficulty diff)
        {
            return list.Where(x => diff.HasFlag(x.Data1.GetData1Difficulty(isPro)));
        }
        public static IEnumerable<MidiEvent> GetBetweenTick(this IEnumerable<MidiEvent> list, int downTick, int upTick)
        {
            return list.Where(x => x.AbsoluteTicks < upTick && x.AbsoluteTicks >= downTick);
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

        [CompilerGenerated()]
        public static void ForEach<T>(this IEnumerable<T> tlist, Action<T> func) 
        {
            foreach (var l in tlist) { func(l); }
        }
        
        public static IEnumerable<T> GetMessages<T>(this IEnumerable<GuitarMessage> list)
        {
            return list.Where(x => x is T).Cast<T>();
        }

        

        public static IEnumerable<GuitarMessage> GetEventsByData1(this IEnumerable<GuitarMessage> list, int data1, bool sort=true)
        {
            return list.Where(x => x.Data1 == data1).SortTicks();
        }

        public static IEnumerable<GuitarMessagePair> GetMessagePairs(this IEnumerable<GuitarMessage> list)
        {
            var ret = new List<GuitarMessagePair>();
            
            for (int x = 0; x < list.Count()-1; x += 2)
            {
                ret.Add(new GuitarMessagePair(list.ElementAt(x), list.ElementAt(x + 1)));
            }
            return ret;
        }
        public static IEnumerable<MidiEventPair> GetMidiEventPairs(this IEnumerable<MidiEvent> list)
        {
            var ret = new List<MidiEventPair>();

            foreach (var data1 in list.GroupBy(x => x.Data1))
            {
                for (int x = 0; x < data1.Count() - 1; x += 2)
                {
                    ret.Add(new MidiEventPair(data1.ElementAt(x), data1.ElementAt(x + 1)));
                }
            }
            return ret;
        }
        public static IEnumerable<GuitarHandPosition> SortTicks(this IEnumerable<GuitarHandPosition> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x=> x, new GuitarMessageSorter());
            }
            else
            {
                return list.OrderByDescending(x => x, new GuitarMessageSorter());
            }
        }
        public static IEnumerable<GuitarChord> SortTicks(this IEnumerable<GuitarChord> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x => x, new GuitarMessageSorter());
            }
            else
            {   
                return list.OrderByDescending(x => x, new GuitarMessageSorter());
            }
        }
        public static IEnumerable<GuitarMessage> SortTicks(this IEnumerable<GuitarMessage> list, bool ascending = true)
        {
            if (ascending)
            {
                return list.OrderBy(x => x, new GuitarMessageSorter());
            }
            else
            {
                return list.OrderByDescending(x => x, new GuitarMessageSorter());
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
            return d.IsNull() ? nullValue : d.ToString(USCulture);
        }

        public static string ToStringEx(this bool b)
        {
            return b.ToString().ToLower();
        }

        public static CultureInfo USCulture = new CultureInfo("en-US");


        public static string ToStringEx(this double d, string nullValue = "")
        {
            if (d.IsNull())
                return nullValue;

            var ret = d.ToString(USCulture);
            if(ret != "0")
                ret = ret.TrimEnd(new[] { '.', '0' });
            return ret;
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
