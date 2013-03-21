using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace ProUpgradeEditor.Common
{

    [Serializable()]
    public class SerializedChordNote
    {
        public int String { get; set; }
        public int Fret { get; set; }
        public int Channel { get; set; }
    }

    [Serializable()]
    public class SerializedChordModifier
    {
        public int Type { get; set; }
    }

    [Serializable()]
    public class SerializedChord
    {

        public override string ToString()
        {
            return Serialize();
        }

        public string Serialize()
        {
            var ser = new XmlSerializer(typeof(SerializedChord));
            var sb = new StringBuilder();
            using (var tr = new StringWriter(sb))
            {
                ser.Serialize(tr, this);
            }
            return sb.ToString();
        }

        public static SerializedChord Deserialize(string str)
        {
            SerializedChord ret = null;
            var ser = new XmlSerializer(typeof(SerializedChord));

            using (var tr = new StringReader(str))
            {
                ret = (SerializedChord)ser.Deserialize(tr);
            }
            return ret;
        }

        public int TickLength { get; set; }
        public double TimeLength { get; set; }

        public List<SerializedChordNote> Notes { get; set; }
        public List<SerializedChordModifier> Modifiers { get; set; }

        public SerializedChord() 
        { 
            Notes = new List<SerializedChordNote>();
            Modifiers = new List<SerializedChordModifier>();
        }

        public GuitarChord Deserialize(GuitarMessageList owner, GuitarDifficulty diff, TickPair ticks)
        {
            int[] frets;
            int[] channels;
            ChordStrum chordStrum;
            bool isSlide;
            bool isSlideReverse;
            bool isHammeron;
            GetProperties(out frets, out channels, out chordStrum, out isSlide, out isSlideReverse, out isHammeron);

            var ret = GuitarChord.CreateChord(owner, diff, ticks, frets, channels, 
                isSlide,
                isSlideReverse,
                isHammeron,
                chordStrum);

            return ret;
        }

        public void GetProperties(out int[] frets, out int[] channels, out ChordStrum chordStrum, out bool isSlide, out bool isSlideReverse, out bool isHammeron)
        {
            frets = Utility.Null6.ToArray();
            channels = Utility.Null6.ToArray();
            foreach (var n in Notes)
            {
                frets[n.String] = n.Fret;
                channels[n.String] = n.Channel;
            }
            var ct = Modifiers.Select(x => (ChordModifierType)x.Type).ToArray();

            chordStrum = ChordStrum.Normal;
            if (ct.Any(x => x == ChordModifierType.ChordStrumHigh))
            {
                chordStrum |= ChordStrum.High;
            }
            if (ct.Any(x => x == ChordModifierType.ChordStrumMed))
            {
                chordStrum |= ChordStrum.Mid;
            }
            if (ct.Any(x => x == ChordModifierType.ChordStrumLow))
            {
                chordStrum |= ChordStrum.Low;
            }
            isSlide = ct.Any(x => x == ChordModifierType.SlideReverse || x == ChordModifierType.Slide);
            isSlideReverse = ct.Any(x => x == ChordModifierType.SlideReverse);
            isHammeron = ct.Any(x => x == ChordModifierType.Hammeron);
        }
    }

    public class GuitarChord : GuitarMessage
    {
        public SerializedChord Serialize()
        {
            var ret = new SerializedChord();

            ret.TickLength = TickLength;
            ret.TimeLength = TimeLength;

            ret.Notes.AddRange(Notes.Select(x => new SerializedChordNote() { Fret = x.NoteFretDown, String = x.NoteString, Channel = x.Channel }));
            ret.Modifiers.AddRange(Modifiers.Select(x => new SerializedChordModifier() { Type = (int)x.ModifierType }));

            return ret;
        }


        public static GuitarChord CreateChord(GuitarMessageList list,
            GuitarDifficulty difficulty,
            TickPair ticks,
            int[] frets, int[] channels,
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode = ChordStrum.Normal)
        {
            GuitarChord ret = null;

            ret = GetChord(list, difficulty, ticks, frets, channels, isSlide, isSlideReverse, isHammeron, strumMode);

            if (ret != null)
            {
                list.Chords.GetBetweenTick(ret.TickPair).ToList().ForEach(x => x.DeleteAll());

                var newMods = ret.Modifiers.Select(x => ChordModifier.CreateModifier(list, ticks, x.ModifierType, true, difficulty)).Where(x=> x != null).ToList();
                ret.Modifiers.Clear();
                ret.Modifiers.AddRange(newMods);
                ret.IsNew = true;
                ret.CreateEvents();
            }
            return ret;
        }

        public static GuitarChord GetChord(GuitarMessageList list, IEnumerable<GuitarNote> notes, bool findModifiers = true)
        {
            GuitarChord ret = null;
            try
            {
                if (notes != null)
                {
                    var dupes = notes.GroupBy(x => x.NoteString).Where(x => x.Count() > 1);
                    foreach (var noteset in dupes)
                    {
                        if (noteset.Count() == 2 && noteset.Count(x => x.Channel != 0) == 1)
                        {
                            list.Remove(noteset.Where(x => x.Channel == 0).ToList());
                        }
                    }
                    foreach (var note in notes.Where(x => x.TickLength < Utility.TickCloseWidth).ToList())
                    {
                        var tempo = list.Owner.GuitarTrack.GetTempo(note.DownTick);
                        if (list.Notes.IsTickInsideAny(note.UpTick + tempo.TicksPerOneTwentyEigthNote.Round()))
                        {
                            list.Remove(note);
                        }
                    }

                }

                if (notes.Any(x => !x.IsDeleted))
                {
                    ret = new GuitarChord(list);
                    ret.Notes.SetNotes(notes.Where(x => !x.IsDeleted));

                    if (findModifiers)
                    {
                        ret.Modifiers.AddRange(list.Hammerons.GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(list.Slides.GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(list.ChordStrums.GetBetweenTick(ret.TickPair).ToList());
                    }
                }
            }
            catch { }
            return ret;
        }
        public static GuitarChord GetChord(GuitarMessageList list,
            GuitarDifficulty difficulty,
            TickPair ticks,
            int[] frets, int[] channels,
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode)
        {
            GuitarChord ret = null;

            var lowE = Utility.GetStringLowE(difficulty);

            var notes = new List<GuitarNote>();

            for (int x = 0; x < frets.Length; x++)
            {
                var fret = frets[x];
                var channel = channels[x];

                if (!fret.IsNull() && fret >= 0 && fret <= 23)
                {
                    notes.Add(
                        GuitarNote.GetNote(list,
                            difficulty, ticks, x, fret,
                            channel == Utility.ChannelTap,
                            channel == Utility.ChannelArpeggio,
                            channel == Utility.ChannelX));
                }
            }

            if (notes.Any())
            {
                ret = GetChord(list, notes, false);
                if (ret != null)
                {
                    if (isSlide || isSlideReverse)
                    {
                        ret.Modifiers.Add(GuitarSlide.GetModifier(list, ticks,
                            isSlideReverse ? ChordModifierType.SlideReverse : ChordModifierType.Slide, GuitarMessageType.GuitarSlide));
                    }
                    if (isHammeron)
                        ret.Modifiers.Add(GuitarHammeron.GetModifier(list, ticks, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron));

                    if (strumMode.HasFlag(ChordStrum.High))
                    {
                        ret.Modifiers.Add(GuitarChordStrum.GetModifier(list, ticks, ChordModifierType.ChordStrumHigh, GuitarMessageType.GuitarChordStrum));
                    }
                    if (strumMode.HasFlag(ChordStrum.Mid))
                    {
                        ret.Modifiers.Add(GuitarChordStrum.GetModifier(list, ticks, ChordModifierType.ChordStrumMed, GuitarMessageType.GuitarChordStrum));
                    }
                    if (strumMode.HasFlag(ChordStrum.Low))
                    {
                        ret.Modifiers.Add(GuitarChordStrum.GetModifier(list, ticks, ChordModifierType.ChordStrumLow, GuitarMessageType.GuitarChordStrum));
                    }
                }
            }

            return ret;
        }

        public GuitarChord(GuitarMessageList track)
            : base(track, null, null, GuitarMessageType.GuitarChord)
        {
            Notes = new GuitarChordNoteList(track);
            Modifiers = new List<ChordModifier>();
        }

        public GuitarChordNoteList Notes;

        public int[] NoteFrets
        {
            get
            {
                var ret = new int[6];
                for (int x = 0; x < 6; x++)
                {
                    ret[x] = Notes[x] != null ? Notes[x].NoteFretDown : Int32.MinValue;
                }
                return ret;
            }
        }

        public int[] NoteChannels
        {
            get
            {
                var ret = Utility.Null6;
                Notes.ForEach(n => ret[n.NoteString] = n.Channel);
                return ret;
            }
        }

        public bool IsPureArpeggioHelper
        {
            get { return Notes.All(x => x.Channel == Utility.ChannelArpeggio); }
        }


        public List<ChordModifier> Modifiers;

        public bool IsHammeron { get { return Modifiers.Any(x => x.ModifierType == ChordModifierType.Hammeron); } }
        public bool IsSlide
        {
            get
            {
                return Modifiers.Any(x => x.ModifierType == ChordModifierType.Slide ||
                    x.ModifierType == ChordModifierType.SlideReverse);
            }
        }
        public bool IsSlideReversed { get { return Modifiers.Any(x => x.ModifierType == ChordModifierType.SlideReverse); } }

        public ChordStrum StrumMode
        {
            get
            {
                var isHigh = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumHigh);
                var isMed = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumMed);
                var isLow = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumLow);
                return (isHigh ? ChordStrum.High : ChordStrum.Normal) |
                    (isMed ? ChordStrum.Mid : ChordStrum.Normal) |
                    (isLow ? ChordStrum.Low : ChordStrum.Normal);
            }
        }


        public override int UpTick
        {
            get
            {
                if (base.UpTick.IsNull() && Notes.Any())
                {
                    return Notes.GetMaxTick();
                }
                else
                {
                    return base.UpTick;
                }
            }

        }
        public override int DownTick
        {
            get
            {
                if (base.DownTick.IsNull() && Notes.Any())
                {
                    return Notes.GetMinTick();
                }
                else
                {
                    return base.DownTick;
                }
            }
        }


        public bool HasNotes
        {
            get
            {
                return Notes.Any(x => x != null);
            }
        }

        public int LowestFret
        {
            get
            {
                var ret = 0;
                if (Notes.Any())
                {
                    ret = Notes.Min(n => n.NoteFretDown);
                }
                return ret;
            }
        }

        public int LowestNonZeroFret
        {
            get
            {
                var ret = 0;
                var items = Notes.Where(x => x.NoteFretDown > 0);
                if (items.Any())
                    ret = items.Min(x => x.NoteFretDown);
                return ret;
            }
        }

        public int HighestFret
        {
            get
            {
                int ret = 0;
                if (Notes.Any())
                {
                    ret = Notes.Max(x => x.NoteFretDown);
                }
                return ret;
            }
        }


        public override GuitarDifficulty Difficulty
        {
            get
            {
                var ret = GuitarDifficulty.Unknown;
                var n = Notes.FirstOrDefault();
                if (n != null)
                {
                    ret = n.Data1.GetData1Difficulty(Owner.Owner.IsPro);
                }
                return ret;
            }
        }

        public GuitarChord CloneAtTime(
            GuitarMessageList list,
            TickPair ticks,
            int stringOffset = int.MinValue)
        {
            return GuitarChord.CreateChord(list, list.Owner.CurrentDifficulty,
                list.Owner.SnapLeftRightTicks(ticks, new SnapConfig(true, true, true)),
                Notes.GetFretsAtStringOffset(stringOffset.GetIfNull(0)),
                Notes.GetChannelsAtStringOffset(stringOffset.GetIfNull(0)),
                IsSlide, IsSlideReversed, IsHammeron, StrumMode);
        }

        public override void DeleteAll()
        {
            Notes.ToList().ForEach(x => x.DeleteAll());
            Modifiers.ToList().ForEach(x => x.DeleteAll());

            Notes.Clear();
            Modifiers.Clear();

            base.DeleteAll();
        }

        public override void RemoveFromList()
        {
            Notes.ToList().ForEach(x => x.RemoveFromList());
            Modifiers.ToList().ForEach(x => x.RemoveFromList());
            
            base.RemoveFromList();
        }
        public override void CreateEvents()
        {
            if (Notes.Any(x => x.IsXNote))
            {
                Notes.ToList().ForEach(x => x.Channel = Utility.ChannelX);
            }
            Notes.ForEach(x => x.CreateEvents());
            Modifiers.ForEach(x => x.CreateEvents());
            AddToList();
        }

        public override bool HasDownEvent
        {
            get
            {
                return Notes.Any(x => x.HasDownEvent) || Modifiers.Any(x => x.HasDownEvent);
            }
        }

        public override bool HasUpEvent
        {
            get
            {
                return Notes.Any(x => x.HasUpEvent) || Modifiers.Any(x => x.HasUpEvent);
            }
        }

        public override void SetTicks(TickPair ticks)
        {
            base.SetTicks(ticks);

            Notes.ForEach(x => x.SetTicks(ticks));
            Modifiers.ForEach(x => x.SetTicks(ticks));

        }
        public override void UpdateEvents()
        {
            RemoveEvents();
            CreateEvents();
        }

        public override void RemoveEvents()
        {
            Notes.ForEach(x => x.RemoveEvents());
            Modifiers.ForEach(x => x.RemoveEvents());
            
        }

        public GuitarChord CloneToMemory(GuitarMessageList list)
        {
            var ret = GuitarChord.GetChord(list, Notes.Select(x => x.CloneToMemory(list)).ToList(), false);
            if (ret != null)
            {
                ret.Modifiers.AddRange(
                    Modifiers.Select(x =>
                        ChordModifier.GetModifier(list, x.TickPair, x.ModifierType, x.MessageType)).ToList());
            }
            return ret;
        }


        public bool HasStrum
        {
            get
            {
                return Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumHigh ||
                    x.ModifierType == ChordModifierType.ChordStrumMed ||
                    x.ModifierType == ChordModifierType.ChordStrumLow);
            }
        }

        public bool HasSlide
        {
            get
            {
                return Modifiers.Any(x => x.ModifierType == ChordModifierType.Slide || x.ModifierType == ChordModifierType.SlideReverse);
            }
        }

        public bool HasSlideReversed
        {
            get
            {
                return Modifiers.Any(x => x.ModifierType == ChordModifierType.SlideReverse);
            }
        }

        public bool HasHammeron
        {
            get
            {
                return Modifiers.Any(x => x.ModifierType == ChordModifierType.Hammeron);
            }
        }

        public void RemoveSlide()
        {
            RemoveModifiers(ChordModifierType.Slide);
            RemoveModifiers(ChordModifierType.SlideReverse);
        }


        public void RemoveStrum()
        {
            RemoveModifiers(ChordModifierType.ChordStrumHigh);
            RemoveModifiers(ChordModifierType.ChordStrumMed);
            RemoveModifiers(ChordModifierType.ChordStrumLow);
        }

        public void AddStrum(ChordStrum strum)
        {
            if (strum != ChordStrum.Normal)
            {
                if (Utility.GetStrumData1(this.Difficulty) != -1)
                {
                    Modifiers.Add(GuitarChordStrum.CreateStrum(Owner, this.Difficulty, strum, this.TickPair));
                }
            }
        }


        public void AddSlide(bool reversed)
        {
            AddSlide(reversed, this.TickPair);
        }

        public void AddSlide(bool reversed, TickPair ticks)
        {
            Modifiers.Add(GuitarSlide.CreateSlide(Owner, ticks, reversed));
        }

        void RemoveModifiers(ChordModifierType type)
        {
            foreach (var m in Modifiers.Where(x => x.ModifierType == type).ToList())
            {
                m.DeleteAll();
                
                Modifiers.Remove(m);
            }
        }

        public void RemoveHammeron()
        {
            RemoveModifiers(ChordModifierType.Hammeron);
        }

        public void AddHammeron()
        {
            AddHammeron(this.TickPair);
        }

        public void AddHammeron(TickPair ticks)
        {
            if (Utility.GetHammeronData1(Difficulty) != -1)
            {
                Modifiers.Add(GuitarHammeron.CreateHammeron(Owner, ticks));
            }
        }

        public void RemoveNotes()
        {
            Notes.ToList().ForEach(n => n.DeleteAll());
        }

        public void RemoveNote(GuitarNote note)
        {
            note.DeleteAll();
            
            Notes.Remove(note);
        }


        public void RemoveSubMessages()
        {
            RemoveHammeron();
            RemoveSlide();
            RemoveStrum();
            RemoveNotes();
        }

        public bool IsTap
        {
            get
            {
                for (int x = 0; x < 6; x++)
                {
                    if (Notes[x] != null && Notes[x].IsTapNote)
                        return true;
                }
                return false;
            }
        }
        public bool IsXNote
        {
            get
            {
                for (int x = 0; x < 6; x++)
                {
                    if (Notes[x] != null && Notes[x].IsXNote)
                        return true;
                }
                return false;
            }
        }
    }
}
