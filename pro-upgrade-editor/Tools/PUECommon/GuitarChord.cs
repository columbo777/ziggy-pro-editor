using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Drawing;
using System.Diagnostics;

namespace ProUpgradeEditor.DataLayer
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

        public GuitarChordNoteList(GuitarTrack track)
        {
            notes = new List<GuitarNote>();
            this.track = track;
        }

        public void Remove(GuitarNote note)
        {
            track.Remove(note);
            notes.Remove(note);
        }

        public void SetNotes(IEnumerable<GuitarNote> notes)
        {
            Clear();
            this.notes.AddRange(notes);
        }
        public void Clear()
        {
            track.Remove(notes);
            notes.Clear();
        }

        public GuitarNote this[int noteString]
        {
            get { return notes.SingleOrDefault(x => x.NoteString == noteString); }
            set
            {
                var existing = notes.SingleOrDefault(x => x.NoteString == noteString);
                if (existing != null)
                {
                    notes.Remove(existing);
                }
                if (value != null)
                {
                    notes.Add(value);
                }
            }
        }

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
        public static GuitarChord GetChord(GuitarTrack track, GuitarDifficulty difficulty,
            int downTick, int upTick,
            int[] frets, int[] channels, bool isSlide, bool isSlideReverse, bool isHammeron, ChordStrum strumMode)
        {

            var ret = new GuitarChord(track);
            var lowE = Utility.GetStringLowE(difficulty);

            for (int x = 0; x < frets.Length; x++)
            {
                var fret = frets[x];
                var channel = channels[x];

                if (!fret.IsNull())
                {
                    ret.Notes[x] = GuitarNote.GetNote(track, difficulty, downTick, upTick, x, fret,
                        channel == Utility.ChannelTap, channel == Utility.ChannelArpeggio, channel == Utility.ChannelX);
                }
            }
            if (ret.HasNotes)
            {
                ret.IsSlide = isSlide;
                ret.IsSlideReversed = isSlideReverse;
                ret.IsHammeron = isHammeron;
                ret.StrumMode = strumMode;
            }
            else
            {
                ret = null;
            }
            return ret;
        }
        public GuitarChord(GuitarTrack track)
            : base(track, null, null)
        {
            Notes = new GuitarChordNoteList(track);
            Modifiers = new List<GuitarModifier>();

            StrumMode = ChordStrum.Normal;
            IsHammeron = false;
            IsSlide = false;
            IsSlideReversed = false;
            downTick = Int32.MinValue;
            upTick = Int32.MinValue;

            if (downEvent != null && upEvent != null)
            {
                var gn = new GuitarNote(track, downEvent, upEvent);
                Notes[gn.NoteString] = gn;
                downTick = downEvent.AbsoluteTicks;
                upTick = upEvent.AbsoluteTicks;
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
                var ret = new int[6];
                for (int x = 0; x < 6; x++)
                {
                    ret[x] = Notes[x] != null ? Notes[x].Channel : Int32.MinValue;
                }
                return ret;
            }
        }
        public List<GuitarModifier> Modifiers;
        public bool IsHammeron;
        public bool IsSlide;
        public bool IsSlideReversed;
        public ChordStrum StrumMode;

        public override double StartTime
        {
            get
            {
                if (DownTick.IsNull())
                {
                    return Int32.MinValue;
                }
                return base.StartTime;
            }
        }

        public override double EndTime
        {
            get
            {
                if (UpTick.IsNull())
                {
                    return Int32.MinValue;
                }
                return base.EndTime;
            }
        }
        public override int DownTick
        {
            get
            {
                if (downTick.IsNull())
                {
                    downTick = Notes.GetMinTick();
                }
                return downTick;
            }
            set
            {
                if (downTick != value)
                {
                    downTick = value;
                    IsUpdated = true;

                    foreach (var n in Notes)
                    {
                        if (n.DownTick != value)
                        {
                            n.DownTick = value;
                        }
                    }
                    foreach (var m in Modifiers)
                    {
                        if (m.DownTick != value)
                        {
                            m.DownTick = value;
                        }
                    }
                }
            }
        }

        public override int UpTick
        {
            get
            {
                if (upTick.IsNull())
                {
                    if (Notes.Any())
                    {
                        return Notes.Max(x => x.UpTick);
                    }
                    else
                    {
                        return Int32.MinValue;
                    }
                }
                else
                {
                    return upTick;
                }
            }
            set
            {
                if (upTick != value)
                {
                    upTick = value;

                    IsUpdated = true;

                    foreach (var n in Notes)
                    {
                        if (n.UpTick != value)
                        {
                            n.UpTick = value;
                        }
                    }
                    foreach (var m in Modifiers)
                    {
                        if (m.UpTick != value)
                        {
                            m.UpTick = value;
                        }
                    }
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
                int ret = int.MaxValue;
                foreach (var n in Notes)
                {
                   
                    if (n.NoteFretDown < ret)
                    {
                        ret = n.NoteFretDown;
                    }
                    
                }
                if (ret == int.MaxValue)
                    ret = 0;
                return ret;
            }
        }
        public int LowestNonZeroFret
        {
            get
            {
                int ret = int.MaxValue;
                foreach (var n in Notes)
                {
                    
                    if (n.NoteFretDown != 0)
                    {
                        if (n.NoteFretDown < ret)
                        {

                            ret = n.NoteFretDown;
                        }
                    }
                    
                }
                if (ret == int.MaxValue)
                    ret = 0;
                return ret;
            }
        }
        public int HighestFret
        {
            get
            {
                int ret = int.MinValue;
                foreach (var n in Notes)
                {
                    if (n.NoteFretDown > ret)
                    {
                        ret = n.NoteFretDown;
                    }
                }
                if (ret == int.MinValue)
                    ret = 0;
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
            int downTick, int upTick, int stringOffset=int.MinValue, int minDownTick=Int32.MinValue)
        {
            stringOffset = stringOffset.IsNull() ? 0 : stringOffset;
            var cb = new ChannelMessageBuilder();

            RemoveChordsAtTick(track, ref downTick, ref upTick, true);
            
            if (downTick < minDownTick)
            {
                downTick = minDownTick;
            }

            if (upTick - downTick < Utility.NoteCloseWidth)
                upTick = downTick + Utility.NoteCloseWidth;

            var nc = new GuitarChord(track);

            for (int x = 0; x < 6; x++)
            {
                var note = Notes[x];

                if (note != null)
                {
                    if (note.NoteString + stringOffset > 5 || note.NoteString + stringOffset < 0)
                    {
                        Notes[x] = null;
                    }
                    else
                    {
                        nc.Notes[note.NoteString + stringOffset] = note.Clone(track, track.CurrentDifficulty, downTick, upTick, stringOffset);
                    }
                }
            }

            if (nc.HasNotes)
            {
                if (IsSlide)
                {
                    nc.AddSlide(IsSlideReversed);
                }

                if (IsHammeron)
                {
                    nc.AddHammeron();
                }

                if (StrumMode != ChordStrum.Normal)
                {
                    nc.AddStrum(this.StrumMode);
                }
                
                OwnerTrack.Messages.Add(nc);
            }
            else
            {
                nc = null;
            }
            
            return nc;
        }

        private void RemoveChordsAtTick(GuitarTrack track, ref int downTick, ref int upTick, bool removeThis=true)
        {
            foreach (var ncn in track.GetChordsAtTick(downTick,
                                    upTick).ToArray())
            {
                if (removeThis == false && ncn == this)
                    continue;
                if (ncn.IsDeleted == false)
                {
                    if (Utility.IsCloseTick(ncn.DownTick, upTick))
                    {
                        upTick = ncn.DownTick;
                    }
                    else if (Utility.IsCloseTick(ncn.UpTick, downTick))
                    {
                        downTick = ncn.UpTick;
                    }
                    else
                    {
                        OwnerTrack.Remove(ncn);
                    }
                }
            }

            if (upTick < downTick - Utility.NoteCloseWidth)
            {
                upTick = downTick + Utility.NoteCloseWidth;
            }
        }


        public bool HasStrum
        {
            get
            {
                return Modifiers.Any(x => Utility.AllStrumData1.Contains(x.Data1));
            }
        }

        public bool HasSlide
        {
            get
            {
                return Modifiers.Any(x => Utility.AllSlideData1.Contains(x.Data1));
            }
        }

        public bool HasSlideReversed
        {
            get
            {
                return Modifiers.Any(x => Utility.AllSlideData1.Contains(x.Data1) && x.Channel == Utility.ChannelSlideReversed);
            }
        }

        public bool HasHammeron
        {
            get
            {
                return Modifiers.Any(x => Utility.AllHammeronData1.Contains(x.Data1));
            }
        }

        public void RemoveSlide()
        {
            RemoveModifiers(Utility.AllSlideData1);
        }


        public void RemoveStrum()
        {
            RemoveModifiers(Utility.AllStrumData1);
        }

        public void AddStrum(ChordStrum strum)
        {
            this.StrumMode = strum;
            if (Utility.GetStrumData1(OwnerTrack.CurrentDifficulty) != -1)
            {
                if (this.StrumMode != ChordStrum.Normal)
                {
                    AddStrum( this.DownTick, this.UpTick);
                }
            }
        }

        public void AddStrum( int downTick, int upTick)
        {
            var diff = OwnerTrack.CurrentDifficulty;
            if (Utility.GetStrumData1(OwnerTrack.CurrentDifficulty) != -1)
            {
                if (this.StrumMode != ChordStrum.Normal)
                {
                    
                    if (this.StrumMode.HasFlag(ChordStrum.Low))
                    {
                        var st = GuitarModifier.CreateModifier(OwnerTrack, DownTick, UpTick, GuitarModifierType.ChordStrumLow, diff);
                        if (st != null)
                            Modifiers.Add(st);
                    }
                    if ((this.StrumMode & ChordStrum.Mid) > 0)
                    {
                        var st = GuitarModifier.CreateModifier(OwnerTrack, DownTick, UpTick, GuitarModifierType.ChordStrumMed, diff);
                        if (st != null)
                            Modifiers.Add(st);
                    }
                    if ((this.StrumMode & ChordStrum.High) > 0)
                    {
                        var st = GuitarModifier.CreateModifier(OwnerTrack, DownTick, UpTick, GuitarModifierType.ChordStrumHigh, diff);
                        if (st != null)
                            Modifiers.Add(st);
                    }
                }
            }
        }

        public bool UpdateChordProperties()
        {
            bool ret = true;
            try
            {
                if (downTick.IsNull())
                {
                    downTick = DownTick;
                }
                if (upTick.IsNull())
                {
                    upTick = UpTick;
                }

                RemoveChordsAtTick(OwnerTrack, ref downTick, ref upTick, false);

                RemoveSubMessages();

                if (Notes.Any())
                {
                    Notes.SetNotes(Notes.Select(x => x.Clone(OwnerTrack, Difficulty, downTick, upTick)).ToList());

                    if (IsHammeron)
                    {
                        AddHammeron();
                    }
                    if (IsSlide)
                    {
                        AddSlide(IsSlideReversed);
                    }
                    if (StrumMode != ChordStrum.Normal)
                    {
                        AddStrum(StrumMode);
                    }
                }
                else
                {
                    OwnerTrack.Remove(this);
                }
            }
            catch { ret = false; }
            return ret;
        }
        public void AddSlide(bool reversed)
        {
            AddSlide(reversed, this.DownTick, this.UpTick);
        }

        public void AddSlide(bool reversed, int downTick, int upTick)
        {
            MidiEvent downEvent, upEvent;

            Utility.CreateMessage(OwnerTrack, Utility.GetSlideData1(OwnerTrack.CurrentDifficulty), 100,
                reversed ? Utility.ChannelSlideReversed : Utility.ChannelDefault, 
                downTick, upTick, out downEvent, out upEvent);

            Modifiers.Add(new GuitarModifier(OwnerTrack, downEvent, upEvent, GuitarModifierType.NoteModifier));
            
            IsSlide = true;
            IsSlideReversed = reversed;
        }

        void RemoveModifiers(int[] modifierData1)
        {
            foreach (var m in Modifiers.Where(x => Utility.AllHammeronData1.Contains(x.Data1)).ToList())
            {
                OwnerTrack.Remove(m);
                Modifiers.Remove(m);
            }
        }

        public void RemoveHammeron()
        {
            RemoveModifiers(Utility.AllHammeronData1);
        }

        public void AddHammeron()
        {
            AddHammeron( this.DownTick, this.UpTick);
        }

        public void AddHammeron( int downTick, int upTick)
        {
            if (Utility.GetHammeronData1(Difficulty) != -1)
            {
                MidiEvent downEvent, upEvent;

                Utility.CreateMessage(OwnerTrack, Utility.GetHammeronData1(Difficulty), 100,
                    0, downTick, upTick, out downEvent, out upEvent);

                Modifiers.Add(new GuitarModifier(OwnerTrack, downEvent, upEvent, GuitarModifierType.NoteModifier));

                IsHammeron = true;
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
