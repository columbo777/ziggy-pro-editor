using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordNoteListSorter : IComparer<GuitarNote>
    {
        public int Compare(GuitarNote x, GuitarNote y)
        {
            return x.NoteString < y.NoteString ? -1 : x.NoteString > y.NoteString ? 1 : 0;
        }
    }
    public class GuitarChordNoteList : IEnumerable<GuitarNote>
    {
        List<GuitarNote> notes;
        GuitarTrack track;

        TickPair ticks;

        public int[] FretArray
        {
            get
            {
                var ret = Utility.Null6;
                notes.ForEach(x => ret[x.NoteString] = x.NoteFretDown);
                return ret;
            }
        }
        public int[] ChannelArray
        {
            get
            {
                var ret = Utility.Null6;
                notes.ForEach(x => ret[x.NoteString] = x.Channel);
                return ret;
            }
        }
        public int[] FretArrayZero
        {
            get
            {
                return FretArray.Select(x => x.IsNull() == false ? 0 : x).ToArray();
            }
        }
        public int[] ChannelArrayZero
        {
            get
            {
                return ChannelArray.Select(x => x.IsNull() == false ? 0 : x).ToArray();
            }
        }

        public int[] GetFretsAtStringOffset(int offset)
        {
            var ret = Utility.Null6;
            notes.ForEach(x => 
            {
                var idx = x.NoteString + offset;
                if (idx >= 0 && idx <= 5)
                {
                    ret[idx] = x.NoteFretDown;
                }
            });
            return ret;
        }
        public int[] GetChannelsAtStringOffset(int offset)
        {
            var ret = Utility.Null6;
            notes.ForEach(x =>
            {
                var idx = x.NoteString + offset;
                if (idx >= 0 && idx <= 5)
                {
                    ret[idx] = x.Channel;
                }
            });
            return ret;
        }
        public GuitarChordNoteList(GuitarTrack track)
        {
            notes = new List<GuitarNote>();
            this.track = track;
            ticks = TickPair.NullValue;
        }

        public void Remove(GuitarNote note)
        {
            track.Remove(note);
            notes.Remove(note);
            if (!notes.Any())
            {
                ticks = TickPair.NullValue;
            }
        }

        public void SetNotes(IEnumerable<GuitarNote> notes)
        {
            Clear();
            notes.ForEach(x=> internalAddNote(x));
        }
        public void Clear()
        {
            track.Messages.Notes.RemoveRange(notes);
            notes.Clear();
        }

        void internalAddNote(GuitarNote n)
        {
            ticks.Down = n.DownTick;
            ticks.Up = n.UpTick;

            if (notes.Any(x => x.IsXNote != n.IsXNote))
            {
                n.Channel = notes.First().Channel;
            }
            notes.Add(n);
        }
        public GuitarNote this[int noteString]
        {
            get 
            {
                if (noteString < 0 || noteString > 5)
                    return null;

                return notes.SingleOrDefault(x => x.NoteString == noteString); 
            }
        }

        public int GetMinTick() { return ticks.Down; }
        public int GetMaxTick() { return ticks.Up; }
        public TickPair GetTickPair() { return ticks; }

        public IEnumerator<GuitarNote> GetEnumerator()
        {
            return notes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class GuitarChord : GuitarMessage
    {

        public static GuitarChord CreateChord(GuitarTrack track,
            GuitarDifficulty difficulty,
            TickPair ticks,
            int[] frets, int[] channels,
            bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode = ChordStrum.Normal)
        {
            GuitarChord ret = null;

            ret = GetChord(track, difficulty, ticks, frets, channels, isSlide, isSlideReverse, isHammeron, strumMode);

            if (ret != null)
            {
                ret.CreateEvents();
            }
            return ret;
        }
        public static GuitarChord GetChord(GuitarTrack track, 
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
                        GuitarNote.GetNote(track, 
                            difficulty, ticks, x, fret,
                            channel == Utility.ChannelTap, 
                            channel == Utility.ChannelArpeggio, 
                            channel == Utility.ChannelX));
                }
            }
            if (!notes.Any())
            {
            }
            if (notes.Any())
            {
                ret = new GuitarChord(track, notes, false);
                
                if (isSlide || isSlideReverse)
                {
                    ret.Modifiers.Add(GuitarSlide.GetModifier(track, ticks,
                        isSlideReverse ? ChordModifierType.SlideReverse : ChordModifierType.Slide,GuitarMessageType.GuitarSlide));
                }
                if (isHammeron)
                    ret.Modifiers.Add(GuitarHammeron.GetModifier(track, ticks, ChordModifierType.Hammeron, GuitarMessageType.GuitarHammeron));

                if (strumMode.HasFlag(ChordStrum.High))
                {
                    ret.Modifiers.Add(GuitarChordStrum.GetModifier(track, ticks, ChordModifierType.ChordStrumHigh, GuitarMessageType.GuitarChordStrum));
                }
                if (strumMode.HasFlag(ChordStrum.Mid))
                {
                    ret.Modifiers.Add(GuitarChordStrum.GetModifier(track, ticks, ChordModifierType.ChordStrumMed, GuitarMessageType.GuitarChordStrum));
                }
                if (strumMode.HasFlag(ChordStrum.Low))
                {
                    ret.Modifiers.Add(GuitarChordStrum.GetModifier(track, ticks, ChordModifierType.ChordStrumLow, GuitarMessageType.GuitarChordStrum));
                }

            }

            return ret;
        }

        public GuitarChord(GuitarTrack track, IEnumerable<GuitarNote> notes, bool findModifiers)
            : base(track, null, null, GuitarMessageType.GuitarChord)
        {
            Notes = new GuitarChordNoteList(track);
            Modifiers = new List<ChordModifier>();

            if (notes != null)
            {
                Notes.SetNotes(notes);
            }

            if(findModifiers)
            {
                Modifiers.AddRange(
                        track.Messages.Hammerons.Where(x =>
                            x.DownTick < UpTick && x.UpTick > DownTick).ToList());

                Modifiers.AddRange(
                    track.Messages.Slides.Where(x =>
                        x.DownTick < UpTick && x.UpTick > DownTick).ToList());

                Modifiers.AddRange(
                    track.Messages.ChordStrums.Where(x =>
                        x.DownTick < UpTick && x.UpTick > DownTick).ToList());
            }
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
        public bool IsSlide { get { return Modifiers.Any(x => x.ModifierType == ChordModifierType.Slide ||
            x.ModifierType == ChordModifierType.SlideReverse); } }
        public bool IsSlideReversed { get { return Modifiers.Any(x => x.ModifierType == ChordModifierType.SlideReverse); } }

        public ChordStrum StrumMode
        {
            get
            {
                var isHigh = Modifiers.Any(x=> x.ModifierType == ChordModifierType.ChordStrumHigh);
                var isMed = Modifiers.Any(x=> x.ModifierType == ChordModifierType.ChordStrumMed);
                var isLow = Modifiers.Any(x=> x.ModifierType == ChordModifierType.ChordStrumLow);
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
                if(items.Any())
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
                    ret = n.Data1.GetData1Difficulty(OwnerTrack.IsPro);
                }
                return ret;
            }
        }

        public GuitarChord CloneAtTime(
            GuitarTrack track,
            TickPair ticks, 
            int stringOffset=int.MinValue)
        {
            return GuitarChord.CreateChord(track, track.CurrentDifficulty,
                track.Owner.SnapLeftRightTicks(ticks),
                Notes.GetFretsAtStringOffset(stringOffset.GetIfNull(0)),
                Notes.GetChannelsAtStringOffset(stringOffset.GetIfNull(0)),
                IsSlide, IsSlideReversed, IsHammeron, StrumMode);
        }

        public override void CreateEvents()
        {
            SnapEvents();

            OwnerTrack.Remove(OwnerTrack.Messages.Chords.GetBetweenTick(TickPair).ToList());

            Notes.ForEach(x => x.CreateEvents());
            Modifiers.ForEach(x => x.CreateEvents());

            if (OwnerTrack != null)
            {
                OwnerTrack.Messages.Add(this);
            }
        }

        public override bool HasDownEvent
        {
            get
            {
                return Notes.Any(x => x.HasDownEvent) || Modifiers.Any(x=> x.HasDownEvent);
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

        public GuitarChord CloneToMemory(GuitarTrack track)
        {
            var ret = new GuitarChord(track, Notes.Select(x => x.CloneToMemory(track)).ToList(), false);

            ret.Modifiers.AddRange(
                Modifiers.Select(x=> 
                    ChordModifier.GetModifier(track, x.TickPair, x.ModifierType, x.MessageType)).ToList());
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
            RemoveModifiers(ChordModifierType.Hammeron);
        }

        public void AddStrum(ChordStrum strum)
        {
            if (strum != ChordStrum.Normal)
            {
                if (Utility.GetStrumData1(OwnerTrack.CurrentDifficulty) != -1)
                {
                    Modifiers.Add(GuitarChordStrum.CreateStrum(OwnerTrack, OwnerTrack.CurrentDifficulty, strum, this.TickPair));
                }
            }
        }


        public void AddSlide(bool reversed)
        {
            AddSlide(reversed, this.TickPair);
        }

        public void AddSlide(bool reversed, TickPair ticks)
        {
            Modifiers.Add(GuitarSlide.CreateSlide(OwnerTrack, ticks, reversed));
        }

        void RemoveModifiers(ChordModifierType type)
        {
            foreach (var m in Modifiers.Where(x => x.ModifierType == type).ToList())
            {
                OwnerTrack.Remove(m);
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

        public void AddHammeron( TickPair ticks)
        {
            if (Utility.GetHammeronData1(Difficulty) != -1)
            {
                Modifiers.Add(GuitarHammeron.CreateHammeron(OwnerTrack, ticks));

            }
        }

        public void RemoveNotes()
        {
            Notes.ToList().ForEach(n => OwnerTrack.Remove(n));
        }

        public void RemoveNote(GuitarNote note)
        {
            OwnerTrack.Remove(note);
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
