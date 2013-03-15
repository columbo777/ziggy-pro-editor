﻿using System;
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

    public static class PUEExtensions
    {

        public static double ToDouble(this int i) { return (double)i; }
        public static double Round(this double d, int decimals) { return Math.Round(d, decimals); }
        public static int Round(this double d) { return (int)Math.Round(d); }
        public static int Ceiling(this double d) { return (int)Math.Ceiling(d); }
        public static int Floor(this double d) { return (int)Math.Floor(d); }

        public static MidiEvent GetInsertedClone(this MidiEvent ev, GuitarMessageList owner)
        {
            return owner.Insert(ev.AbsoluteTicks, ev.Clone());
        }

        public static void IfAny<T>(this T o, Action<T> func, Action<T> elseFunc = null)
            where T : IEnumerable<MidiEvent>
        {
            if (o != null && o.Any())
            {
                func(o);
            }
            else
            {
                elseFunc(o);
            }
        }



        public static IEnumerable<T> MakeEnumerable<T>(this T o)
        {
            return new[] { o };
        }
        public static void IfIsType<T2>(this object o, Action<T2> func) where T2 : class
        {
            if (o is T2)
                func(o as T2);
        }

        public static void IfObjectNotNull<T>(this T o, Action<T> func, Action<T> elseFunc = null)
        {
            if (o != null)
                func(o);
            else
                if (elseFunc != null)
                    elseFunc(o);
        }
        public static TRet GetIfNotNull<T, TRet>(this T o, Func<T, TRet> func, Func<T, TRet> elseFunc = null)
            where TRet : class
        {
            return o != null ? func(o) : elseFunc != null ? elseFunc(o) : null;
        }

        public static T TryGetValue<T>(Func<T> func) where T : class
        {
            T ret = null;
            try { ret = func(); }
            catch { }
            return ret;
        }



        public static T TryExec<T>(Func<T> func)
        {
            T ret = default(T);
            try { ret = func(); }
            catch { }
            return ret;
        }
        public static void TryExec(Action func)
        {
            try { func(); }
            catch { }
        }
        public static bool EndsWithEx(this string str, string val, bool ignoreCase = true)
        {
            return (str ?? "").EndsWith(val ?? "", StringComparison.InvariantCultureIgnoreCase);
        }
        public static bool StartsWithEx(this string str, string val, bool ignoreCase = true)
        {
            return (str ?? "").StartsWith(val ?? "", StringComparison.InvariantCultureIgnoreCase);
        }
        public static FileType GetMidiFileType(this string localMidiFile)
        {
            var ret = FileType.Unknown;
            try
            {
                var seq = new Sequence(FileType.Unknown, localMidiFile);
                if (seq.Tracks.Any(x => x.Name.IsProTrackName()))
                {
                    ret = FileType.Pro;
                }
                else
                {
                    ret = FileType.Guitar5;
                }
            }
            catch { }
            return ret;
        }

        public static FileType FindFileType(this Sequence seq)
        {
            FileType ret = FileType.Unknown;
            try
            {
                if (seq != null && seq.Tracks != null)
                {
                    ret = seq.Tracks.Any(x => x.Name.IsProTrackName()) ? FileType.Pro :
                            seq.Tracks.Any(x => x.Name.IsGuitarTrackName5() || x.Name.IsBassTrackName5()) ? FileType.Guitar5 :
                        FileType.Unknown;
                }
            }
            catch { }
            return ret;
        }
        public static Sequence LoadSequence(this byte[] data)
        {
            Sequence ret = null;
            try
            {
                ret = new Sequence(FileType.Unknown);
                using (var ms = new MemoryStream(data))
                {
                    ret.Load(ms);
                }
                ret.FileType = ret.FindFileType();
            }
            catch { }
            return ret;
        }
        public static bool IsMidiFileName(this string fileName) { return fileName.EndsWithEx(".mid") || fileName.EndsWithEx(".midi"); }

        public static Sequence LoadSequenceFile(this string localFileName)
        {
            Sequence ret = null;
            try
            {
                if (localFileName.FileExists())
                {
                    ret = new Sequence(FileType.Unknown, localFileName);
                    ret.FileType = ret.FindFileType();
                }
            }
            catch { }
            return ret;
        }
        public static DateTime GetFileModifiedTime(this string fileName)
        {
            return fileName.FileExists() ? fileName.GetFileInfo().LastWriteTime : DateTime.MinValue;
        }
        public static DateTime GetFileCreationTime(this string fileName)
        {
            return fileName.FileExists() ? fileName.GetFileInfo().CreationTime : DateTime.MinValue;
        }
        public static DateTime GetFileAccessedTime(this string fileName)
        {
            return fileName.FileExists() ? fileName.GetFileInfo().LastAccessTime : DateTime.MinValue;
        }
        public static bool FileExists(this string str)
        {
            return TryExec(() => { return !str.IsEmpty() && File.Exists(str); });
        }
        public static FileInfo GetFileInfo(this string fileName)
        {
            return TryGetValue(() => new FileInfo(fileName));
        }
        public static bool IsReadOnlyFile(this string fileName)
        {
            return TryExec(() =>
            {
                return fileName.FileExists() && fileName.GetFileInfo().Attributes.HasFlag(FileAttributes.ReadOnly);
            });
        }
        public static void MakeWritableFile(this string fileName)
        {
            TryExec(() =>
            {
                if (fileName.IsReadOnlyFile())
                    fileName.GetFileInfo().Attributes ^= FileAttributes.ReadOnly;
            });
        }
        public static bool WriteFileBytes(this string fileName, byte[] contents)
        {
            if (!fileName.IsEmpty())
            {
                fileName.GetFolderName().CreateFolderIfNotExists();
            }
            return TryExec(() =>
            {
                var ret = false;
                try
                {
                    if (fileName.IsReadOnlyFile())
                        fileName.MakeWritableFile();

                    File.WriteAllBytes(fileName, contents);
                    ret = true;
                }
                catch
                {
                }
                return ret;
            });
        }
        public static byte[] ReadFileBytes(this string fileName)
        {
            byte[] ret = null;
            try
            {
                ret = File.ReadAllBytes(fileName);
            }
            catch { }
            return ret;
        }
        public static string AppendIfMissing(this string str, string strEnd)
        {
            if (!str.IsEmpty())
            {
                if (!str.EndsWith(strEnd))
                    str += strEnd;
            }
            return str;
        }

        public static int IndexOfClosing(this string str, char opening, char closing, int currentIndex)
        {
            int ret = -1;
            if (!str.IsEmpty() && currentIndex < str.Length - 1 && currentIndex >= 0 && str[currentIndex] == opening)
            {
                int count = 1;
                var chars = str.ToArray();
                var len = chars.Length;
                while (count > 0)
                {
                    currentIndex++;

                    if (chars[currentIndex] == closing)
                    {
                        count--;
                        if (count == 0)
                        {
                            ret = currentIndex;
                            break;
                        }
                    }
                    else if (chars[currentIndex] == opening)
                    {
                        count++;
                    }

                    if (currentIndex >= len - 1)
                    {
                        ret = currentIndex;
                        break;
                    }
                }
            }
            return ret;
        }


        public static string GetBetween(this string str, char first, char last)
        {
            var ret = string.Empty;
            if (!str.IsEmpty())
            {
                var start = str.IndexOf(first);
                var end = str.IndexOf(last, start + 1);
                if (start != -1 && end != -1)
                {
                    var len = end - start;
                    if (len > 0)
                    {
                        ret = str.Substring(start, len);
                    }
                }
            }
            return ret;
        }

        public static string GetBetween(this string str, char[] first, char[] last)
        {
            var ret = string.Empty;
            if (!str.IsEmpty())
            {
                var start = str.IndexOfAny(first);
                var end = str.IndexOfAny(last, start + 1);
                if (start != -1 && end != -1)
                {
                    var len = end - start;
                    if (len > 0)
                    {
                        ret = str.Substring(start, len);
                    }
                }
            }
            return ret;
        }
        public static DataPair<int> GetBetweenIndex(this string str, char[] first, char[] last)
        {
            DataPair<int> ret = null;
            if (!str.IsEmpty())
            {
                var start = str.IndexOfAny(first);
                var end = str.IndexOfAny(last, start + 1);
                if (start != -1 && end != -1)
                {
                    var len = end - start;
                    if (len > 0)
                    {
                        ret = new DataPair<int>(start, end);
                    }
                }
            }
            return ret;
        }

        public static string AppendSlashIfMissing(this string str)
        {
            return str.AppendIfMissing("\\");
        }
        public static bool FolderExists(this string str)
        {
            return TryExec(() =>
            {
                if (!str.IsEmpty())
                {
                    var di = new DirectoryInfo(str.AppendSlashIfMissing());
                    return di.Exists;
                }
                else
                {
                    return false;
                }
            });
        }
        public static DirectoryInfo GetDirectoryInfo(this string str)
        {
            return TryGetValue(delegate()
            {
                return new DirectoryInfo(str.AppendSlashIfMissing());
            });
        }
        public static string GetFolderName(this string str)
        {
            return TryGetValue(delegate()
            {
                if (str.IsEmpty())
                {
                    return "";
                }
                else
                {
                    return str.GetFileInfo().GetIfNotNull((fi) => fi.Exists && fi.Attributes.HasFlag(FileAttributes.Directory) ? str.AppendSlashIfMissing() : Path.GetDirectoryName(str).AppendSlashIfMissing());
                }
            });
        }
        public static string GetParentFolder(this string folder)
        {
            return TryGetValue(() =>
            {
                return folder.GetFolderName().GetDirectoryInfo().Parent.GetIfNotNull((di) => di.FullName.AppendSlashIfMissing());
            });
        }
        public static Track GetTempoTrack(this Sequence seq)
        {
            Track ret = null;
            if (seq != null)
            {
                ret = seq.Tracks.Where(x => x.IsTempo()).FirstOrDefault();
            }
            return ret;
        }
        public static string CreateFolderIfNotExists(this string str)
        {
            return TryExec(() =>
            {
                str.AppendSlashIfMissing().IfNotEmpty(x =>
                {
                    if (!x.FolderExists())
                        Directory.CreateDirectory(x);
                    str = x;
                });
                return str;
            });
        }
        public static bool IsFileNotFolder(this string str)
        {
            return TryExec(() => { return str.IsEmpty() == false && (str.FolderExists() == false && str.FileExists() == true); });
        }
        public static bool IsFolderNotFile(this string str)
        {
            return TryExec(() => { return str.IsEmpty() == false && (str.FolderExists() == true && str.FileExists() == false); });
        }
        public static string GetFileNameWithoutExtension(this string str)
        {
            return TryGetValue(delegate() { return str.IsEmpty() ? "" : Path.GetFileNameWithoutExtension(str); });
        }
        public static string GetFileName(this string str)
        {
            return TryGetValue(delegate() { return str.IsEmpty() ? "" : Path.GetFileName(str); });
        }
        public static void OutputDebug(this string str) { Debug.WriteLine(str ?? ""); }
        public static IEnumerable<string> GetFilesInFolder(this string folder, bool recursive = true, string searchPattern = "*")
        {
            var ret = new List<string>();
            TryExec(() =>
            {

                foreach (var str in searchPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ret.AddRange(
                        Directory.EnumerateFiles(folder.AppendSlashIfMissing(),
                        str,
                        recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(x => x.IsFileNotFolder()));
                }
            });
            return ret;
        }


        public static T2 CastObject<T2>(this object o)
        {
            try
            {
                if (o.IsType<T2>())
                {
                    return (T2)(dynamic)o;
                }
                else if (o.IsType<IConvertible>())
                {
                    try
                    {
                        return (T2)((IConvertible)o).ToType(typeof(T2), CultureInfo.CurrentCulture);
                    }
                    catch { }
                }
            }
            catch { }
            return default(T2);
        }

        public static bool IsType<T2>(this Type type)
        {
            return type == typeof(T2);
        }

        public static bool IsType<T2>(this object o)
        {
            return o is T2;
        }

        public static T2 ConvertTo<T2>(this object o)
        {
            return ConvertTo<T2>(o, default(T2));
        }

        public static T2 ConvertTo<T2>(this object o, T2 nullValue)
        {
            T2 ret = default(T2);

            if (o != null)
            {
                try
                {
                    var t2type = typeof(T2);

                    if (o.IsType<T2>())
                    {
                        ret = o.CastObject<T2>();
                    }
                    else if (o.IsType<IConvertible>())
                    {
                        ret = o.CastObject<IConvertible>().ToType(t2type, CultureInfo.CurrentCulture).CastObject<T2>();
                    }
                    else if (t2type.IsType<string>())
                    {
                        ret = o.ToString().CastObject<T2>();
                    }
                    else
                    {
                        ("Unknown Type: " + typeof(T2).Name).OutputDebug();
                    }
                }
                catch
                {
                }
            }


            return ret;
        }

        public static IEnumerable<string> GetSubFolders(this string folder, bool recursive = true, string searchPattern = "*")
        {
            var ret = new List<string>();
            TryExec(() =>
            {
                foreach (var str in searchPattern.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ret.AddRange(
                        Directory.EnumerateDirectories(folder.AppendSlashIfMissing(),
                        str,
                        recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(x => x.IsFolderNotFile()));
                }
            });
            return ret;
        }
        public static string PathCombine(this string path1, string path2)
        {
            return Path.Combine(path1 ?? "", path2 ?? "");
        }

        public static Data1ChannelPair GetData1ChannelPair(this MidiEvent ev)
        {
            return new Data1ChannelPair(ev.Data1, ev.Channel);
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

        public static bool IsFileTypeG5(this Sequence seq) { return seq.FileType == FileType.Guitar5; }
        public static bool IsFileTypeG5(this Track t) { return t.FileType == FileType.Guitar5; }

        public static bool IsFileTypeUnknown(this Sequence seq) { return seq.FileType == FileType.Unknown; }
        public static bool IsFileTypeUnknown(this Track t) { return t.FileType == FileType.Unknown; }

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
        public static bool IsNotEmpty(this string str) { return !str.IsEmpty(); }

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

        public static bool EqualsEx(this string str, string str2, bool ignoreCase = true)
        {
            return string.Compare(str ?? "", str2 ?? "", ignoreCase) == 0;
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
            return ret ?? "";
        }

        public static GuitarMessage ToGuitarMessage(this MidiEvent ev, GuitarMessageList track)
        {
            return new GuitarMessage(track, ev, null, GuitarMessageType.Unknown);
        }

        public static bool IsCloseScreenPoint(this int i, int x)
        {
            return (int)Math.Abs(i - x) <= Utility.GridSnapDistance;
        }

        public static bool IsCloseTick(this int i, int x)
        {
            return (int)Math.Abs(i - x) <= Utility.TickCloseWidth;
        }

        public static int DistSq(this int i, int x)
        {
            return (int)Math.Abs(i - x);
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
            return Utility.PowerupData1 == ev.Data1;
        }
        public static bool IsSoloEvent(this MidiEvent ev)
        {
            return ev.Data1.IsSolo(ev.Owner.IsFileTypePro());
        }
        public static bool IsMultiStringTremeloEvent(this MidiEvent ev)
        {
            return Utility.MultiStringTremeloData1 == ev.Data1;
        }

        public static bool IsSingleStringTremeloEvent(this MidiEvent ev)
        {
            return Utility.SingleStringTremeloData1 == ev.Data1;
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
        public static string GetIfEmpty(this string str, string other)
        {
            return str.IsEmpty() ? other : str;
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

        public static bool IsVocalTrackName5(this string str)
        {
            return str.EqualsEx(GuitarTrack.VocalTrackName5);
        }

        public static bool HasVocalTrack(this Sequence seq)
        {
            return seq.GetVocalTrack() != null;
        }

        public static Track GetVocalTrack(this Sequence seq)
        {
            return seq.FirstOrDefault(x => x.Name.IsVocalTrackName5());
        }

        public static Track GetPrimaryTrack(this Sequence seq, bool preferGuitar17 = true)
        {
            Track ret = null;
            if (preferGuitar17)
            {
                ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName17());
                if (ret == null)
                    ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName22());
                if (ret == null)
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
        public static Track GetPrimaryTrackG5(this Sequence seq)
        {
            Track ret = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName5());
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
            track.Remove(ret);
            return ret;
        }

        public static Sequence ConvertToPro(this Sequence seq5, bool onlyGBTempo = true)
        {
            var seq = new Sequence(FileType.Pro, seq5.Division);

            if (onlyGBTempo)
            {
                var tempo = seq5.Tracks.FirstOrDefault();
                if (tempo != null)
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

            if (t.Name.Equals("tempo", StringComparison.OrdinalIgnoreCase))
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

                    var isPro = t5.IsFileTypePro();
                    foreach (var x in t5.ChanMessages.Where(x => targetDifficulty.HasFlag(x.Data1.GetData1Difficulty(isPro))).ToList())
                    {
                        var proCM = x.ChannelMessage.ConvertToPro(targetDifficulty);
                        if (proCM != null)
                        {
                            ret.Insert(x.AbsoluteTicks, proCM);
                        }
                    };

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

                    foreach (var cm in t5.ChanMessages)
                    {
                        var proCM = cm.ChannelMessage.ConvertToPro(targetDifficulty);
                        if (proCM != null)
                        {
                            ret.Insert(cm.AbsoluteTicks, proCM);
                        }
                    };

                    foreach (var meta in t5.Meta.Where(meta => meta.IsTextEvent()).ToList())
                    {
                        ret.Insert(meta.AbsoluteTicks, meta.Clone());
                    };
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
            else if (t.IsFileTypePro())
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertDifficultyPro(destDifficulty)));
            else
                tmess.ForEach(x => ret.Insert(x.AbsoluteTicks, x.ChannelMessage.ConvertDifficultyG5(destDifficulty)));

            return ret;
        }

        public static Track ConvertToG5(this Track t6, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            var ret = new Track(FileType.Guitar5, t6.Name);

            t6.ChanMessages.ForEach(cm =>
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
        public static IEnumerable<GuitarDifficulty> GetDifficulties(this GuitarDifficulty difficulty)
        {
            if (difficulty.IsAll())
                yield return GuitarDifficulty.All;
            if (difficulty.IsExpert())
                yield return GuitarDifficulty.Expert;
            if (difficulty.IsHard())
                yield return GuitarDifficulty.Hard;
            if (difficulty.IsMedium())
                yield return GuitarDifficulty.Medium;
            if (difficulty.IsEasy())
                yield return GuitarDifficulty.Easy;
        }

        public static MetaMessage CloneMeta(this MetaMessage mess)
        {
            return new MetaMessage(mess.MetaType, mess.GetBytes());
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

        public static bool IsGuitarChord(this GuitarMessage x)
        {
            return x is GuitarChord;
        }

        public static bool IsChordSubItem(this GuitarMessage x)
        {
            return x is GuitarNote || x is GuitarSlide || x is GuitarHammeron || x is GuitarChordStrum;
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
                    else
                    {
                        cm.ToString().OutputDebug();
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
            return isPro ? (Utility.SoloData1 == data1) : (Utility.ExpertSoloData1_G5 == data1 || data1 == Utility.SoloData1);
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
            return str.IsEmpty() == false && (GuitarTrack.GuitarTrackName17 == str || GuitarTrack.BassTrackName17 == str);
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
            return str.IsEmpty() ? false : GuitarTrack.GuitarTrackName5.Contains(str);
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
            return str.IsEmpty() ? false : GuitarTrack.BassTrackName5.Contains(str);
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
            return diff.HasFlag(GuitarDifficulty.Unknown) || (diff.IsEasyMediumHardExpert() == false && diff.IsAll() == false);
        }
        public static bool IsUnknownOrNone(this GuitarDifficulty diff)
        {
            return diff.IsNone() || diff.IsUnknown();
        }
        public static bool IsNone(this GuitarDifficulty diff)
        {
            return diff == GuitarDifficulty.None;
        }
        public static GuitarMessageType GetGuitarMessageType(this ChordModifierType type)
        {
            var ret = GuitarMessageType.Unknown;
            switch (type)
            {
                case ChordModifierType.ChordStrumHigh:
                case ChordModifierType.ChordStrumMed:
                case ChordModifierType.ChordStrumLow:
                    ret = GuitarMessageType.GuitarChordStrum;
                    break;
                case ChordModifierType.Hammeron:
                    ret = GuitarMessageType.GuitarHammeron;
                    break;
                case ChordModifierType.Slide:
                case ChordModifierType.SlideReverse:
                    ret = GuitarMessageType.GuitarSlide;
                    break;
            }
            return ret;
        }

        public static ChordModifierType GetModifierType(this ChordStrum strum)
        {
            var ret = ChordModifierType.Invalid;
            if (strum == ChordStrum.High)
                ret = ChordModifierType.ChordStrumHigh;
            if (strum == ChordStrum.Mid)
                ret = ChordModifierType.ChordStrumMed;
            if (strum == ChordStrum.Low)
                ret = ChordModifierType.ChordStrumLow;
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


        public static IEnumerable<MidiEvent> GetEvents(this GuitarTrack tr)
        {
            return tr.GetTrack().Events;
        }


        public static IEnumerable<MidiEvent> GetDifficulty(this GuitarTrack track, GuitarDifficulty diff)
        {
            return track.GetEvents().GetDifficulty(track.IsPro, diff).ToList();
        }

        public static IEnumerable<MidiEvent> GetDifficulty(this IEnumerable<MidiEvent> list, bool isPro, GuitarDifficulty diff)
        {
            return list.Where(x => diff.HasFlag(x.Data1.GetData1Difficulty(isPro))).ToList();
        }

        public static T SingleByDownTick<T>(this SpecializedMessageList<T> list, int downTick) where T : GuitarMessage
        {
            downTick = downTick < 0 ? 0 : downTick;
            var ret = list.SingleOrDefault(x => x.DownTick <= downTick && x.UpTick > downTick);
            if (ret == null)
            {
                ret = list.LastOrDefault();
            }
            return ret;
        }

        public static IEnumerable<T> GetBetweenTick<T>(this IEnumerable<T> list, TickPair ticks) where T : GuitarMessage
        {
            return list.Where(x => x.DownTick < ticks.Up && x.UpTick > ticks.Down);
        }

        public static IEnumerable<T> GetBetweenTime<T>(this IEnumerable<T> list, TimePair time) where T : GuitarMessage
        {
            return list.Where(x => x.StartTime < time.Up && x.EndTime > time.Down);
        }

        public static bool IsTickInsideAny<T>(this IEnumerable<T> list, int tick) where T : GuitarMessage
        {
            return list.Any(x => x.DownTick < tick && x.UpTick > tick);
        }
        public static bool AnyBetweenTick<T>(this IEnumerable<T> list, TickPair ticks) where T : GuitarMessage
        {
            return list.Any(x => x.DownTick < ticks.Up && x.UpTick > ticks.Down);
        }
        public static int CountBetweenTick<T>(this IEnumerable<T> list, TickPair ticks) where T : GuitarMessage
        {
            return list.Count(x => x.DownTick < ticks.Up && x.UpTick > ticks.Down);
        }
        public static TickPair GetTickPair<T>(this IEnumerable<T> list) where T : GuitarMessage
        {
            return new TickPair(list.GetMinTick(), list.GetMaxTick());
        }
        public static TimePair GetTimePair<T>(this IEnumerable<T> list) where T : GuitarMessage
        {
            return new TimePair(list.Min(x => x.StartTime), list.Max(x => x.EndTime));
        }

        public static bool AnyBetweenTick(this IEnumerable<MidiEvent> list, TickPair ticks)
        {
            return list.Any(x => x.AbsoluteTicks < ticks.Up && x.AbsoluteTicks > ticks.Down);
        }

        public static bool HasDownTickAtTick(this IEnumerable<GuitarChord> list, int tick)
        {
            return list.Any(x => x.DownTick == tick);
        }

        public static int GetMinTick<T>(this IEnumerable<T> list, int valueIfNull = Int32.MinValue) where T : GuitarMessage
        {
            if (list.Any())
            {
                return list.Min(x => x.DownTick);
            }
            else
            {
                return valueIfNull;
            }
        }

        public static int GetMaxTick<T>(this IEnumerable<T> list, int valueIfNull = Int32.MinValue) where T : GuitarMessage
        {
            if (list.Any())
            {
                return list.Max(x => x.UpTick);
            }
            else
            {
                return valueIfNull;
            }
        }
        public static int GetMaxDownTick<T>(this IEnumerable<T> list, int valueIfNull = Int32.MinValue) where T : GuitarMessage
        {
            if (list.Any())
            {
                return list.Max(x => x.DownTick);
            }
            else
            {
                return valueIfNull;
            }
        }
        [CompilerGenerated()]
        public static void ForEach<T>(this IEnumerable<T> tlist, Action<T> func)
        {
            foreach (var l in tlist) { func(l); }
        }
        [CompilerGenerated()]
        public static void ForEach<T>(this IEnumerable<T> tlist, Action<T, int> func)
        {
            var arr = tlist.ToArray();
            for (int x = 0; x < arr.Length; x++)
            {
                func(arr[x], x);
            }
        }
        [CompilerGenerated()]
        public static void For<T>(this IEnumerable<T> tlist, Action<T, int> func)
        {
            for (int x = 0; x < tlist.Count(); x++)
            {
                func(tlist.ElementAt(x), x);
            }
        }

        public static IEnumerable<T> GetMessages<T>(this IEnumerable<T> list, GuitarMessageType type) where T : GuitarMessage
        {
            return list.Where(x => x.MessageType == type).ToList();
        }



        public static IEnumerable<GuitarMessage> GetEventsByData1(this IEnumerable<GuitarMessage> list, int data1)
        {
            return list.Where(x => x.Data1 == data1).ToList();
        }

        public static IEnumerable<MidiEvent> GetEventsByData1(this IEnumerable<MidiEvent> list, int data1)
        {
            return list.Where(x => x.Data1 == data1).ToList();
        }

        public static IEnumerable<KeyValuePair<Data1ChannelPair, IEnumerable<MidiEvent>>> GroupByData1Channel(
            this IEnumerable<MidiEvent> list, IEnumerable<int> data1)
        {
            var comparer = new Data1ChannelPairEqualityComparer();

            var full = list.Where(x => data1.Contains(x.Data1)).ToList();

            var keys = full.Select(x => x.GetData1ChannelPair()).Distinct(comparer).ToList();

            var ret = new List<KeyValuePair<Data1ChannelPair, List<MidiEvent>>>();

            foreach (var item in full)
            {
                var p = item.GetData1ChannelPair();
                var cntList = ret.Where(x => x.Key.CompareTo(p) == 0);
                if (!cntList.Any())
                {
                    var cnt = new KeyValuePair<Data1ChannelPair, List<MidiEvent>>(p, new List<MidiEvent>());
                    cnt.Value.Add(item);
                    ret.Add(cnt);
                }
                else
                {
                    cntList.Single().Value.Add(item);
                }
            }

            return ret.Select(x =>
                new KeyValuePair<Data1ChannelPair, IEnumerable<MidiEvent>>(x.Key,
                    x.Value.OrderBy(i => i, new MidiEventTickCommandComparer()).ToList())).ToList();

        }


        public static IEnumerable<IEnumerable<MidiEventPair>> GroupMidiEventPairByCloseTick(this IEnumerable<MidiEventPair> list, int closeValue)
        {
            return list.GroupBy(x => x.DownTick, x => x, new TickCloseComparer(closeValue)).ToList();
        }
        public static IEnumerable<IEnumerable<T>> GroupByCloseTick<T>(this IEnumerable<T> list, int closeValue) where T : MidiEvent
        {
            return list.GroupBy(x => x.AbsoluteTicks, x => x, new TickCloseComparer(closeValue)).ToList();
        }
        public static IEnumerable<IEnumerable<T>> GroupByCloseTick<T>(this IEnumerable<T> list) where T : GuitarMessage
        {
            return list.GroupBy(x => x.DownTick, x => x, new TickCloseComparer(Utility.TickCloseWidth)).ToList();
        }
        public static IEnumerable<IEnumerable<T>> GroupByCloseTick<T>(this SpecializedMessageList<T> list) where T : GuitarMessage
        {
            return list.GroupBy(x => x.DownTick, x => x, new TickCloseComparer(Utility.TickCloseWidth)).ToList();
        }
        public static IEnumerable<MidiEvent> GetEventsByData1(this IDictionary<Data1ChannelPair, IEnumerable<MidiEvent>> dic, IEnumerable<int> data1)
        {
            var ret = new List<MidiEvent>();

            foreach (var k in dic.Keys.Where(x => data1.Contains(x.Data1)))
            {
                ret.AddRange(dic[k]);
            }

            return ret;
        }

        public static IEnumerable<MidiEventPair> GetEventPairs(this IEnumerable<KeyValuePair<Data1ChannelPair, IEnumerable<MidiEvent>>> dic,
            GuitarMessageList owner, int data1)
        {
            var ret = new List<MidiEventPair>();
            try
            {
                foreach (var k in dic.Where(k => data1 == k.Key.Data1))
                {
                    ret.AddRange(k.Value.GetEventPairsFromData1List(owner));
                }

                ret.Sort(new MidiEventPairInterlacingSorter());
            }
            catch { }
            return ret.ToList();
        }


        public static IEnumerable<MidiEventPair> GetEventPairs(this IEnumerable<KeyValuePair<Data1ChannelPair, IEnumerable<MidiEvent>>> dic,
            GuitarMessageList owner, IEnumerable<int> data1)
        {
            var ret = new List<MidiEventPair>();
            try
            {
                foreach (var k in dic.Where(k => data1.Contains(k.Key.Data1)))
                {
                    ret.AddRange(k.Value.GetEventPairsFromData1List(owner));
                }

                ret.Sort(new MidiEventPairInterlacingSorter());
            }
            catch { }
            return ret.ToList();
        }

        static MidiEvent ReplaceEvent(GuitarMessageList owner, MidiEvent ev, int tick, ChannelCommand cmd)
        {
            var ticks = ev.AbsoluteTicks;
            var cb = new ChannelMessageBuilder(ev.ChannelMessage);
            cb.Command = cmd;
            if (cmd == ChannelCommand.NoteOn)
            {
                if (cb.Data2 < 100)
                    cb.Data2 = 100;
            }
            else
            {
                cb.Data2 = 0;
            }

            cb.Build();
            owner.Remove(ev);
            return owner.Insert(tick, cb.Result);
        }

        public static IEnumerable<MidiEventPair> GetEventPairsFromData1List(this IEnumerable<MidiEvent> items, GuitarMessageList owner)
        {
            var ret = new List<MidiEventPair>();
            try
            {
                if (!items.Any())
                    return ret;

                int minWidth = Utility.NoteCloseWidth;
                var d1 = items.First().Data1;
                if (d1 == Utility.HandPositionData1)
                    minWidth = 1;

                var closeGroups = items.GroupByCloseTick(minWidth);
                int numGroups = closeGroups.Count();

                int lastOnTick = Int32.MinValue;
                int lastOffTick = Int32.MinValue;

                if (d1 == Utility.HandPositionData1)
                {

                    if (numGroups >= 2)
                    {
                        if (numGroups == 2 && closeGroups.First().Count() == 1 && closeGroups.Last().Count() == 1)
                        {
                            ret.Add(new MidiEventPair(owner, closeGroups.First().First(), closeGroups.Last().First()));
                            return ret;
                        }

                        IEnumerable<IEnumerable<MidiEvent>> remaining = null;

                        if (closeGroups.ElementAt(0).Count() == 1 && closeGroups.ElementAt(1).Count() == 1)
                        {
                            ret.Add(new MidiEventPair(owner, closeGroups.ElementAt(0).First(), closeGroups.ElementAt(1).First()));
                            remaining = closeGroups.Where((e, i) => i > 1);
                        }
                        else
                        {
                            remaining = closeGroups;
                        }

                        var good = remaining.Where(x => x.Count() == 2 && x.First().IsOn && x.Last().IsOff).ToList();
                        if (good.Any())
                        {
                            ret.AddRange(good.Select(x => new MidiEventPair(owner, x.First(), x.Last())));
                            if (good.Count == remaining.Count())
                                return ret;
                        }

                        var bad = remaining.Where(x => x.Count() != 2 || (x.Count() == 2 && !x.First().IsOn || !x.Last().IsOff)).ToList();
                        if (bad.Any())
                        {
                            owner.Owner.GuitarTrack.Remove(bad.SelectMany(x => x));
                        }
                    }
                    else
                    {
                        owner.Remove(closeGroups.SelectMany(x => x));
                    }

                }
                else
                {
                    for (int i = 0; i < closeGroups.Count(); )
                    {
                        var tickGroup1 = closeGroups.ElementAt(i);

                        var notesOn1 = tickGroup1.Where(x => x.IsOn);
                        var notesOff1 = tickGroup1.Where(x => x.IsOff);



                        if (!notesOn1.Any())
                        {
                            if (notesOff1.Any())
                            {
                                if (lastOffTick.IsNull())
                                {
                                    owner.Remove(notesOff1);
                                }
                                else if (notesOff1.Count() > 1)
                                {
                                }
                                else if (notesOff1.First().AbsoluteTicks > lastOffTick)
                                {
                                    owner.Remove(notesOff1);
                                }
                            }
                            i++;
                        }
                        else
                        {
                            var tickGroup2 = closeGroups.ElementAtOrDefault(i + 1);
                            if (tickGroup2 == null)
                            {
                                owner.Remove(notesOn1);

                                if (lastOffTick.IsNull())
                                {
                                    owner.Remove(notesOff1);
                                }

                                break;
                            }
                            var notesOn2 = tickGroup2.Where(x => x.IsOn);
                            var notesOff2 = tickGroup2.Where(x => x.IsOff);

                            if (notesOn1.Any() && notesOff2.Any())
                            {
                                var on = notesOn1.First();
                                var off = notesOff2.First();

                                if (notesOff1.Any() && lastOffTick.IsNull())
                                {
                                    owner.Remove(notesOff1);
                                }

                                lastOnTick = on.AbsoluteTicks;
                                lastOffTick = off.AbsoluteTicks;

                                ret.Add(new MidiEventPair(owner, on, off));

                                var del1 = notesOn1.Where(x => x != on);
                                if (del1.Any())
                                    owner.Remove(del1);

                                var del2 = notesOff2.Where(x => x != off);
                                if (del2.Any())
                                    owner.Remove(del2);

                                i++;
                            }
                            else
                            {
                                if (d1 == 102 || d1 == 103)
                                {
                                    owner.Remove(notesOn1);
                                    i++;
                                }
                                else
                                {
                                    var on = notesOn1.LastOrDefault();
                                    var ev = owner.Insert(notesOn2.First().AbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOff, on.Data1, 0, on.Channel));

                                    ret.Add(new MidiEventPair(owner, on, ev));
                                    i++;
                                }
                            }
                        }
                    }
                }
                /*
                int i;
            

                int minWidth = Utility.NoteCloseWidth;

                var d1 = sorted[0].Data1;

                if (d1 == Utility.HandPositionData1)
                    minWidth = 1;

                for (i = 0; i < sorted.Length - 1; )
                {
                    var p1 = sorted[i];
                    var p2 = sorted[i + 1];

                    var delta = Math.Abs(p1.AbsoluteTicks - p2.AbsoluteTicks);
                    if (delta < minWidth)
                    {
                        if (p1.IsOn)
                        {
                            swap(sorted, i, i + 1);
                            swapEvents(ref p1, ref p2);

                            owner.Remove(p1);
                            i++;
                            continue;
                        }
                        else if (p1.Command == p2.Command)
                        {
                            owner.Remove(p1);
                            owner.Remove(p2);
                            i += 2;
                            continue;
                        }
                        else
                        {
                            owner.Remove(p1);
                            i++;
                            continue;
                        }
                    
                    }

                    if (p1.Command == p2.Command)
                    {
                    
                        owner.Remove(p1);
                        i++;
                        continue;
                    }
                    if (p1.IsOff && p2.IsOn)
                    {
                        swap(sorted, i, i + 1);
                        swapEvents(ref p1, ref p2);
                    }

                
                

                    if (p2.IsOn && 
                        sorted[i+2].IsOff &&
                        sorted[i+1].AbsoluteTicks == sorted[i+2].AbsoluteTicks)
                    {
                        swapEvents(ref sorted[i + 1], ref sorted[i + 2]);
                        p2 = sorted[i + 1];
                    }

                    if (p2.AbsoluteTicks == p1.AbsoluteTicks && p2.Command == p1.Command)
                    {
                        owner.Remove(p2);
                        sorted[i + 1] = sorted[i];
                        i++;
                        continue;
                    }
                    else if (sorted.Count(x => x.IsOn) == 1)
                    {
                        var noteOn = sorted.FirstOrDefault(x=> x.IsOn);
                        var noteOff = sorted.LastOrDefault(x=> x.IsOff && x.AbsoluteTicks > noteOn.AbsoluteTicks);

                        var min = sorted.Min(x => x.AbsoluteTicks);
                        var max = sorted.Max(x => x.AbsoluteTicks);
                        if (max != min)
                        {
                            owner.Remove(sorted.Where(x => x != noteOn && x != noteOff));
                            ret.Clear();
                            ret.Add(new MidiEventPair(owner, noteOn, noteOff));

                            return ret;
                        }
                    }

                    if (p1.IsOn && 
                        p2.IsOff &&
                        p2.AbsoluteTicks > p1.AbsoluteTicks)
                    {
                        ret.Add(new MidiEventPair(owner, p1, p2));
                    }
                
                    i += 2;
                }

                while (i < sorted.Length - 1)
                {
                    owner.Remove(sorted[i]);
                    i++;
                }
                 */
            }
            catch { }
            return ret;
        }

        private static void swap<T>(T[] array, int i1, int i2)
        {
            var v = array[i1];
            array[i1] = array[i2];
            array[i2] = v;
        }

        private static void swapEvents(ref MidiEvent p1, ref MidiEvent p2)
        {
            var v = p1;
            p1 = p2;
            p2 = v;
        }

        public static IEnumerable<MidiEventPair> GetBetweenTicks(this IEnumerable<MidiEventPair> list, TickPair ticks)
        {
            return list.Where(x =>
                x.Down.AbsoluteTicks < ticks.Up &&
                x.Up.AbsoluteTicks > ticks.Down).ToList();
        }

        public static IEnumerable<T> SortTicks<T>(this IEnumerable<T> list, bool ascending = true) where T : GuitarMessage
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


        public static bool ToBool(this string str, bool nullValue = false)
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

        public static int ToInt(this string str, int nullValue = Int32.MinValue)
        {
            int ret = nullValue;
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

        public static string SubstringEx(this string str, int start, int len = -1)
        {
            var ret = string.Empty;
            try
            {
                if (str.IsNotEmpty())
                {
                    if (start > str.Length - 1)
                        return ret;

                    if (start + len > str.Length - 1)
                        len = -1;
                    if (len < 0)
                        len = -1;

                    if (len == -1)
                        ret = str.Substring(start);
                    else
                        ret = str.Substring(start, len);
                }
            }
            catch { }
            return ret;
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
            if (ret.Contains('.'))
                ret = ret.TrimEnd('0').TrimEnd('.');
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
