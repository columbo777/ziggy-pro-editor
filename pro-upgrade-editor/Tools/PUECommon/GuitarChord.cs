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

    

    

    

    public class GuitarChord : GuitarMessage
    {
        public IEnumerable<GuitarNote> HighStrumNotes
        {
            get
            {
                var ret = new List<GuitarNote>();
                if (NoteFrets[4].IsNotNull())
                    ret.Add(Notes[4]);
                if (NoteFrets[5].IsNotNull())
                    ret.Add(Notes[5]);
                
                return ret;
            }
        }
        public IEnumerable<GuitarNote> MidStrumNotes
        {
            get
            {
                var ret = new List<GuitarNote>();
                if (NoteFrets[2].IsNotNull())
                    ret.Add(Notes[2]);
                if (NoteFrets[3].IsNotNull())
                    ret.Add(Notes[3]);
                
                return ret;
            }
        }
        public IEnumerable<GuitarNote> LowStrumNotes
        {
            get
            {
                var ret = new List<GuitarNote>();
                if (NoteFrets[0].IsNotNull())
                    ret.Add(Notes[0]);
                if (NoteFrets[1].IsNotNull())
                    ret.Add(Notes[1]);
                
                return ret;
            }
        }
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
                ret.IsNew = true;
                ret.Modifiers.ForEach(x => x.IsNew = true);
                ret.CreateEvents();
            }
            return ret;
        }

        public static GuitarChord GetChord(GuitarMessageList list, IEnumerable<GuitarNote> notes, bool findModifiers = true)
        {
            GuitarChord ret = null;
            try
            {
                if (notes != null && notes.Any())
                {
                    if (notes.Any(x => x.IsDeleted))
                    {
                        Debug.WriteLine("getting deleted note");
                    }
                    ret = new GuitarChord(list);
                    ret.Notes.SetNotes(notes.ToList());

                    if (findModifiers)
                    {
                        ret.Modifiers.AddRange(list.Hammerons.GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(list.Slides.GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(list.ChordStrums.GetBetweenTick(ret.TickPair).ToList());
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine("GetChord: " + ex.Message); }
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

            bool isXNote = channels.Any(x => x == Utility.ChannelX);

            for (int x = 0; x < frets.Length; x++)
            {
                var fret = frets[x];
                var channel = channels[x];

                if (!fret.IsNull() && fret >= 0 && fret <= 23)
                {
                    var note = GuitarNote.GetNote(list,
                            difficulty, ticks, x, fret,
                            !isXNote && (channel == Utility.ChannelTap),
                            !isXNote && (channel == Utility.ChannelArpeggio),
                            isXNote);

                    if(note != null)
                        notes.Add(note);
                }
            }

            if (notes.Any())
            {
                ret = new GuitarChord(list);
                ret.Notes.SetNotes(notes);
                if (ret != null)
                {
                    if (isSlide || isSlideReverse)
                    {
                        ret.AddSlide(isSlideReverse);
                    }

                    if (isHammeron)
                        ret.AddHammeron();

                    ret.AddStrum(strumMode);
                    
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

        public GuitarChord NextChord
        {
            get
            {
                GuitarChord ret = null;
                if (Owner != null)
                {
                    var chords = Owner.Chords;
                    if (chords.Any() && chords.Contains(this))
                    {
                        var idx = chords.IndexOf(this);
                        if (idx != -1)
                        {
                            ret = chords.ElementAtOrDefault(idx + 1);
                        }
                    }
                }
                return ret;
            }
        }
        public GuitarChord PrevChord
        {
            get
            {
                GuitarChord ret = null;

                var chords = Owner.Chords.ToList();
                if (chords.Any() && chords.Contains(this))
                {
                    var idx = chords.IndexOf(this);
                    if (idx != -1 && idx != 0)
                    {
                        ret = chords.ElementAtOrDefault(idx - 1);
                    }
                }
                return ret;
            }
        }

        public int[] NoteFrets
        {
            get
            {
                var ret = Utility.Null6.ToArray();

                Notes.ForEach(n => ret[n.NoteString] = n.NoteFretDown);

                return ret;
            }
        }

        public int[] NoteChannels
        {
            get
            {
                var ret = Utility.Null6.ToArray();

                Notes.ForEach(n => ret[n.NoteString] = n.Channel);

                return ret;
            }
        }

        public bool IsPureArpeggioHelper
        {
            get { return Notes.All(x => x.Channel == Utility.ChannelArpeggio); }
        }


        public List<ChordModifier> Modifiers;

        public ChordStrum StrumMode
        {
            get
            {
                return (HasStrumMode(ChordStrum.High) ? ChordStrum.High : ChordStrum.Normal) |
                    (HasStrumMode(ChordStrum.Mid) ? ChordStrum.Mid : ChordStrum.Normal) |
                    (HasStrumMode(ChordStrum.Low) ? ChordStrum.Low : ChordStrum.Normal);
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
                var items = Notes.Where(x => x.NoteFretDown > 0).ToList();
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
                HasSlide, HasSlideReversed, HasHammeron, StrumMode);
        }

        public override void DeleteAll()
        {
            if (!IsDeleted)
            {
                Selected = false;

                var tp = this.TickPair;

                Notes.ToList().ForEach(x => x.DeleteAll());
                Modifiers.ToList().ForEach(x => x.DeleteAll());

                Notes.Clear();
                Modifiers.Clear();

                this.SetTicks(tp);

                RemoveFromList();

                IsDeleted = true;
            }
        }

        public override void RemoveFromList()
        {
            Notes.ToList().ForEach(x => x.RemoveFromList());
            Modifiers.ToList().ForEach(x => x.RemoveFromList());
            
            base.RemoveFromList();
        }
        public override void CreateEvents()
        {
            Owner.Chords.GetBetweenTick(TickPair).Where(x=> x != this).ToList().ForEach(x => x.DeleteAll());

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
            GuitarChord ret = null;
            var notes = Notes.Select(x => x.CloneToMemory(list)).Where(x=> x != null).ToList();
            if (notes.Any())
            {
                ret = GuitarChord.GetChord(list, notes, false);
                if (ret != null)
                {
                    if (HasSlide)
                        ret.AddSlide(HasSlideReversed);
                    if (HasStrum)
                        ret.AddStrum(StrumMode);
                    if (HasHammeron)
                        ret.AddHammeron();
                    
                }
            }
            return ret;
        }

        public bool HasLowStrum
        {
            get { return HasStrumMode(ChordStrum.Low); }
        }

        public bool HasMidStrum
        {
            get { return HasStrumMode(ChordStrum.Mid); }
        }
        public bool HasHighStrum
        {
            get { return HasStrumMode(ChordStrum.High); }
        }
        public bool HasStrum
        {
            get { return Modifiers.Any(x => x.ModifierType.GetGuitarMessageType() == GuitarMessageType.GuitarChordStrum); }
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
            if (HasSlide)
            {
                RemoveModifier(ChordModifierType.Slide);
                RemoveModifier(ChordModifierType.SlideReverse);
            }
        }


        public void RemoveStrum()
        {
            if (HasStrum)
            {
                RemoveModifier(ChordModifierType.ChordStrumHigh);
                RemoveModifier(ChordModifierType.ChordStrumMed);
                RemoveModifier(ChordModifierType.ChordStrumLow);
            }
        }

        public bool HasStrumMode(ChordStrum strum)
        {
            var ret = false;
            if (strum.HasFlag(ChordStrum.High))
            {
                ret = Modifiers.Any(x=> x.ModifierType == ChordModifierType.ChordStrumHigh);
            }
            else if (strum.HasFlag(ChordStrum.Mid))
            {
                ret = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumMed);
            }
            else if (strum.HasFlag(ChordStrum.Low))
            {
                ret = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumLow);
            }
            return ret;
        }

        public void AddStrum(ChordStrum strum)
        {
            if (strum != ChordStrum.Normal && !HasStrum)
            {
                if (strum.HasFlag(ChordStrum.High))
                {
                    GuitarChordStrum.CreateStrum(this, ChordStrum.High).IfObjectNotNull(o => Modifiers.Add(o));
                }
                if (strum.HasFlag(ChordStrum.Mid))
                {
                    GuitarChordStrum.CreateStrum(this, ChordStrum.Mid).IfObjectNotNull(o => Modifiers.Add(o));
                }
                if (strum.HasFlag(ChordStrum.Low))
                {
                    GuitarChordStrum.CreateStrum(this, ChordStrum.Low).IfObjectNotNull(o => Modifiers.Add(o));
                }
            }
        }


        public void AddSlide(bool reversed)
        {
            if (!HasSlide)
            {
                GuitarSlide.CreateSlide(this, reversed).IfObjectNotNull(s => Modifiers.Add(s));
            }
        }


        void RemoveModifier(ChordModifierType type)
        {
            foreach (var m in Modifiers.Where(x => x.ModifierType == type).ToList())
            {
                m.DeleteAll();
            }
        }

        public void RemoveHammeron()
        {
            RemoveModifier(ChordModifierType.Hammeron);
        }

        public void AddHammeron()
        {
            if (!HasHammeron)
            {
                GuitarHammeron.CreateHammeron(this).IfObjectNotNull(o => Modifiers.Add(o));
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

        public override string ToString()
        {
            var ret = base.ToString()+" ";

            if (HasSlide)
            {
                if (HasSlideReversed)
                {
                    ret += "(srev)";
                }
                else
                {
                    ret += "(s)";
                }
            }
            if (HasHammeron)
            {
                ret += "(h)";
            }
            if (HasStrum)
            {
                ret += "(strum)";
            }
            ret += " notes: ";
            for (int x = 0; x < 6; x++)
            {
                ret += NoteFrets[x].ToStringEx("_") + ",";
            }
            ret += " chans:";
            for (int x = 0; x < 6; x++)
            {
                ret += NoteChannels[x].ToStringEx("_") + ",";
            }
            return ret;
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
                return Notes.Any(x => x.IsTapNote);
            }
        }
        public bool IsXNote
        {
            get
            {
                return Notes.Any(x => x.IsXNote);
            }
        }
    }
}
