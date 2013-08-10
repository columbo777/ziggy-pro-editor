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
        public GuitarChordNoteList Notes { get; internal set; }
        public List<ChordModifier> Modifiers { get; internal set; }

        public GuitarChordNameList ChordNameEvents { get; internal set; }

        public GuitarChord(GuitarMessageList owner, TickPair pair, GuitarDifficulty difficulty, IEnumerable<GuitarNote> notes)
            : base(owner, pair, GuitarMessageType.GuitarChord)
        {
            Notes = new GuitarChordNoteList(this);
            Modifiers = new List<ChordModifier>();
            ChordNameEvents = new GuitarChordNameList(this);

            Notes.SetNotes(notes);
            SetTicks(pair);

            chordConfig = new GuitarChordConfig()
            {
                Frets = this.NoteFrets.ToArray(),
                Channels = this.NoteChannels.ToArray(),
                IsHammeron = this.HasHammeron,
                IsSlide = this.HasSlide,
                IsSlideReverse = this.HasSlideReversed,
                StrumMode = this.StrumMode,
                
            };

        }

        public GuitarChord(MidiEventPair pair)
            : base(pair, GuitarMessageType.GuitarChord)
        {
            Notes = new GuitarChordNoteList(this);
            Modifiers = new List<ChordModifier>();
            ChordNameEvents = new GuitarChordNameList(this);

            chordConfig = new GuitarChordConfig()
            {
                Frets = this.NoteFrets.ToArray(),
                Channels = this.NoteChannels.ToArray(),
                IsHammeron = this.HasHammeron,
                IsSlide = this.HasSlide,
                IsSlideReverse = this.HasSlideReversed,
                StrumMode = this.StrumMode,

            };
        }

        public bool UpTwelveFrets()
        {
            var ret = false;
            if (HighestFret <= 10)
            {
                ret = true;
                foreach (var note in Notes.ToList())
                {
                    note.NoteFretDown += 12;
                }
            }
            UpdateEvents();
            return ret;
        }

        public IEnumerable<ChordNameMeta> GetTunedChordNames(int[] tuning, bool allMatches = true)
        {
            var ret = new List<ChordNameMeta>();

            var tunedNotes = GetTunedNoteScale(tuning).ToList().Where(x=> x != ToneNameEnum.NotSet).ToList();

            if (tunedNotes.Any())
            {
                if (tunedNotes.Count() == 1)
                {
                    var item = new ChordNameMeta();
                    item.ToneName = tunedNotes.Single();
                    ret.Add(item);
                }
                else
                {
                    if (tunedNotes.Count() == 2)
                    {
                        tunedNotes.Add(tunedNotes.First());
                    }
                    if (allMatches)
                    {
                        var items = ChordNameFinder.GetChordNames(tunedNotes);
                        if (items == null)
                        {
                            var item = new ChordNameMeta();
                            item.ToneName = tunedNotes.Single();
                            ret.Add(item);
                        }
                        else
                        {
                            ret.AddRange(items);
                        }
                    }
                    else
                    {
                        var item = ChordNameFinder.GetChordName(tunedNotes);
                        if (item != null)
                        {
                            ret.Add(item);
                        }
                        else
                        {
                            item = new ChordNameMeta();
                            item.ToneName = tunedNotes.Single();
                            ret.Add(item);
                        }
                    }
                }
            }
            return ret;
        }

        public ChordNameMeta GetTunedChordName(int[] tuning)
        {
            return GetTunedChordNames(tuning, false).FirstOrDefault();
        }

        

        public ToneNameEnum GetRootNoteTone(int[] tuning)
        {
            var ret = ToneNameEnum.NotSet;
            GetTunedChordName(tuning).IfNotNull(x=> ret = x.ToneName);
            return ret;
        }

        public void DownString()
        {
            foreach (var n in Notes.ToList())
            {
                if (n.NoteString == 0)
                {
                    RemoveNote(n);
                }
                else
                {
                    n.NoteString--;

                    if (n.NoteString == 3)
                    {
                        n.NoteFretDown += 4;
                    }
                    else
                    {
                        n.NoteFretDown += 5;
                    }
                    if (n.NoteFretDown > 23)
                        n.NoteFretDown = 23;
                }
            }
            if (!Notes.Any())
            {
                DeleteAll();
            }
            else
            {
                UpdateEvents();
            }
        }

        public bool CanMoveDownString
        {
            get
            {
                var ret = true;
                if (Notes.Any(x => x.NoteString == 0))
                {
                    ret = false;
                }
                else
                {

                    foreach (var n in Notes.ToList())
                    {
                        if (n.NoteString != 0)
                        {
                            var noteString = n.NoteString - 1;
                            var noteFretDown = n.NoteFretDown;

                            if (noteString == 3)
                            {
                                noteFretDown += 4;
                            }
                            else
                            {
                                noteFretDown += 5;
                            }
                            if (noteFretDown > 23)
                            {
                                ret = false;
                                break;
                            }
                        }
                    }
                }
                return ret;
            }
        }
        public bool CanMoveUpString
        {
            get
            {
                var ret = true;
                if (Notes.Any(x => x.NoteString == 5))
                {
                    ret = false;
                }
                else
                {

                    foreach (var n in Notes.ToList())
                    {
                        var noteString = n.NoteString;
                        var noteFret = n.NoteFretDown;

                        if (noteString != 5)
                        {
                            noteString++;

                            if (noteString == 4)
                            {
                                noteFret -= 4;
                            }
                            else if (noteString == 5)
                            {
                                noteFret -= 5;
                            }
                            else
                            {
                                noteFret -= 5;
                            }
                            if (noteFret < 0)
                            {
                                ret = false;
                                break;
                            }
                        }
                    }
                }
                return ret;
            }
        }

        public void UpString()
        {
            foreach (var n in Notes.ToList())
            {
                var noteString = n.NoteString;
                var noteFret = n.NoteFretDown;

                if (noteString != 5)
                {
                    noteString++;

                    if (noteString == 4)
                    {
                        noteFret -= 4;
                    }
                    else if (noteString == 5)
                    {
                        noteFret -= 5;
                    }
                    else
                    {
                        noteFret -= 5;
                    }
                    if (noteFret < 0)
                        noteFret = 0;

                    n.NoteFretDown = noteFret;
                    n.NoteString = noteString;

                }
                else
                {
                    RemoveNote(n);
                }
            }

            if (!Notes.Any())
            {
                DeleteAll();
            }
            else
            {
                UpdateEvents();
            }
        }
        public bool CanMoveDownTwelveFrets
        {
            get
            {
                return LowestNonZeroFret >= 12;
            }
        }
        public bool CanMoveUpTwelveFrets
        {
            get
            {
                return HighestFret <= 10;
            }
        }
        public bool DownTwelveFrets()
        {
            var ret = false;
            if (LowestNonZeroFret >= 12)
            {
                ret = true;
                foreach (var n in Notes.ToList())
                {
                    if (n.NoteFretDown >= 12)
                    {
                        n.NoteFretDown -= 12;
                    }
                }
            }
            UpdateEvents();
            return ret;
        }

        public bool CanMoveDownOctave
        {
            get
            {
                var ret = true;
                
                foreach (var n in Notes.ToList())
                {
                    int noteString = n.NoteString - 2;
                    int noteFret = n.NoteFretDown;

                    if (noteString == 3 || noteString == 2)
                    {
                        noteFret -= 3;
                    }
                    else
                    {
                        noteFret -= 2;
                    }
                    if (noteFret < 0 || noteString < 0)
                    {
                        ret = false;
                        break;
                    }
                }
                
                return ret;
            }
        }

        public void DownOctave()
        {
            foreach (var n in Notes.ToList())
            {
                int noteString = n.NoteString - 2;
                int noteFret = n.NoteFretDown;
                if (noteString == 3 || noteString == 2)
                {
                    noteFret -= 3;
                }
                else
                {
                    noteFret -= 2;
                }
                if (noteFret < 0)
                {
                    noteFret = 0;
                }

                if (noteString < 0)
                {
                    RemoveNote(n);
                }
                else
                {
                    n.NoteFretDown = noteFret;
                    n.NoteString = noteString;
                }
            }
            if (!Notes.Any())
            {
                DeleteAll();
            }
            else
            {
                UpdateEvents();
            }
        }

        public bool CanMoveUpOctave
        {
            get
            {
                var ret = true;

                foreach (var n in Notes.ToList())
                {
                    int noteString = n.NoteString;
                    int noteFretDown = n.NoteFretDown;

                    if (noteString < 4)
                    {
                        noteString += 2;

                        if (noteString >= 4)
                        {
                            noteFretDown += 3;
                        }
                        else
                        {
                            noteFretDown += 2;
                        }
                        if (noteFretDown > 22)
                        {
                            ret = false;
                            break;
                        }
                    }
                    else
                    {
                        ret = false;
                        break;
                    }
                }
                return ret;
            }
        }

        public void UpOctave()
        {
            foreach (var n in Notes.ToList())
            {
                int noteString = n.NoteString;
                int noteFretDown = n.NoteFretDown;
                if (noteString < 4)
                {
                    noteString += 2;

                    if (noteString >= 4)
                    {
                        noteFretDown += 3;
                    }
                    else
                    {
                        noteFretDown += 2;
                    }
                    if (noteFretDown > 22)
                        noteFretDown = 22;

                    n.NoteFretDown = noteFretDown;
                    n.NoteString = noteString;

                }
                else
                {
                    RemoveNote(n);
                }
            }
            if (!Notes.Any())
            {
                DeleteAll();
            }
            else
            {
                UpdateEvents();
            }
        }

        GuitarChordConfig chordConfig;

        public static GuitarChord CreateChord(GuitarMessageList owner, GuitarDifficulty difficulty, TickPair ticks,
           GuitarChordConfig config)
        {
            
            GuitarChord ret = null;

            ret = GetChord(owner, difficulty, ticks, config);

            if (ret != null)
            {
                ret.IsNew = true;
                ret.CreateEvents();
            }
            return ret;
        }

        public static GuitarChord GetChord(GuitarMessageList owner, 
            GuitarDifficulty difficulty, 
            IEnumerable<GuitarNote> notes, 
            bool findModifiers = true)
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

                    var tickPair = notes.GetTickPairSmallest();

                    if (!tickPair.IsValid)
                    {
                        Debug.WriteLine("short chord");
                        return ret;
                    }

                    var unfit = notes.Where(x => x.TickPair != tickPair);
                    if (unfit.Any())
                    {
                        unfit.ForEach(x => x.SetTicks(tickPair));
                    }

                    ret = new GuitarChord(owner, tickPair, difficulty, notes.ToList());

                    if (findModifiers)
                    {
                        ret.Modifiers.AddRange(owner.Hammerons.Where(x => x.Chord == null).GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(owner.Slides.Where(x => x.Chord == null).GetBetweenTick(ret.TickPair).ToList());
                        ret.Modifiers.AddRange(owner.ChordStrums.Where(x => x.Chord == null).GetBetweenTick(ret.TickPair).ToList());

                        ret.ChordNameEvents.SetNames(owner.ChordNames.GetAtTick(ret.AbsoluteTicks).ToList());

                        var mods = ret.Modifiers.Where(x => x.TickPair != ret.TickPair).ToList();
                        foreach (var mod in mods)
                        {
                            mod.SetTicks(ret.TickPair);
                        }
                        mods.ForEach(x => x.Chord = ret);
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine("GetChord: " + ex.Message); }
            return ret;
        }



        public static GuitarChord GetChord(GuitarMessageList owner,
            GuitarDifficulty difficulty,
            TickPair ticks,
            GuitarChordConfig config)
        {
            GuitarChord ret = null;
            
            var lowE = Utility.GetStringLowE(difficulty);

            var notes = new List<GuitarNote>();

            for (int x = 0; x < config.Frets.Length; x++)
            {
                var fret = config.Frets[x];
                var channel = config.Channels[x];

                if (!fret.IsNull() && fret >= 0 && fret <= 23)
                {
                    var note = GuitarNote.GetNote(owner,
                            difficulty, ticks, x, fret,
                            channel == Utility.ChannelTap,
                            channel == Utility.ChannelArpeggio,
                            channel == Utility.ChannelX);

                    if (note != null && note.NoteFretDown.IsNotNull() && note.NoteString.IsNotNull())
                    {
                        notes.Add(note);
                    }
                    else
                    {
                    }
                }
            }

            if (notes.Any())
            {
                ret = new GuitarChord(owner, ticks, difficulty, notes);
                ret.chordConfig = config.Clone();

                if (ret != null)
                {
                    if (config.IsSlide || config.IsSlideReverse)
                    {
                        ret.AddSlide(config.IsSlideReverse, false);
                    }

                    if (config.IsHammeron)
                        ret.AddHammeron(false);

                    ret.AddStrum(config.StrumMode, false);

                }
            }

            return ret;
        }




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

        public ToneNameEnum[] NoteScale
        {
            get
            {
                var ret = new[] { ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet };

                Notes.ForEach(x => ret[x.NoteString] = x.NoteScale);

                return ret;
            }
        }
        public ToneNameEnum[] GetTunedNoteScale(int[] tuning)
        {
            var ret = new[] { ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet, ToneNameEnum.NotSet };

            Notes.ForEach(x => ret[x.NoteString] = x.GetTunedNoteScale(tuning));

            return ret;
            
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

        public override TickPair TickPair
        {
            get
            {
                return base.TickPair;
            }
            set
            {
                base.TickPair = value;
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

        public int LowestString
        {
            get
            {
                var ret = Int32.MinValue;
                if (Notes.Any())
                {
                    ret = Notes.Min(n => n.NoteString);
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


        public GuitarChord CloneAtTime(
            GuitarMessageList owner,
            TickPair ticks,
            int stringOffset = int.MinValue)
        {
            return GuitarChord.CreateChord(owner, owner.Owner.CurrentDifficulty,
                owner.Owner.SnapLeftRightTicks(ticks, new SnapConfig(true, true, true)),
                new GuitarChordConfig(Notes.GetFretsAtStringOffset(stringOffset.GetIfNull(0)),
                    Notes.GetChannelsAtStringOffset(stringOffset.GetIfNull(0)),
                    HasSlide, HasSlideReversed, HasHammeron, StrumMode,
                        RootNoteConfig.GetIfNotNull(x=> x.Clone())));
        }

        public GuitarChordRootNoteConfig RootNoteConfig
        {
            get
            {
                return chordConfig.GetIfNotNull(x=> x.RootNoteConfig);
            }
        }

        public override void DeleteAll()
        {
            if (!IsDeleted)
            {
                Selected = false;

                var tp = this.TickPair;

                Notes.ToList().ForEach(x => x.DeleteAll());
                Modifiers.ToList().ForEach(x => x.DeleteAll());

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
            if (Owner != null)
            {
                var between = Owner.Chords.GetBetweenTick(TickPair).ToList();
                between.ForEach(x => x.DeleteAll());
            }

            if (!IsNew)
            {
                IsNew = IsDeleted;
            }
            if (Notes.Any(x => x.IsXNote))
            {
                Notes.ToList().ForEach(x => x.Channel = Utility.ChannelX);
            }
            Notes.ForEach(x =>
            {
                x.CreateEvents();
            });
            Modifiers.ForEach(x =>
            {
                x.CreateEvents();
            });

            

            if (IsNew)
            {
                AddToList();
            }
        }

        public override void AddToList()
        {
            Notes.ForEach(x => x.AddToList());
            Modifiers.ToList().ForEach(x => x.AddToList());

            base.AddToList();
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
            if (Notes != null)
            {
                Notes.ForEach(x => x.SetTicks(ticks));
            }
            if (Modifiers != null)
            {
                Modifiers.ForEach(x => x.SetTicks(ticks));
            }

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

        public GuitarChord CloneToMemory(GuitarMessageList owner, GuitarDifficulty difficulty)
        {
            GuitarChord ret = null;
            var notes = Notes.Select(x => x.CloneToMemory(owner)).Where(x => x != null).ToList();
            if (notes.Any())
            {
                ret = GuitarChord.GetChord(owner, difficulty, notes, false);
                if (ret != null)
                {
                    if (HasSlide)
                    {
                        ret.AddSlide(HasSlideReversed, false);
                    }
                    if (HasStrum)
                    {
                        ret.AddStrum(StrumMode, false);
                    }
                    if (HasHammeron)
                    {
                        ret.AddHammeron(false);
                    }
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
                ret = Modifiers.Any(x => x.ModifierType == ChordModifierType.ChordStrumHigh);
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

        public void AddStrum(ChordStrum strum, bool createEvents)
        {
            if (strum != ChordStrum.Normal && !HasStrum)
            {
                if (strum.HasFlag(ChordStrum.High))
                {
                    var gs = new GuitarChordStrum(this, ChordModifierType.ChordStrumHigh);

                    gs.IsNew = true;

                    Modifiers.Add(gs);

                    if (createEvents)
                    {
                        gs.CreateEvents();
                    }

                }
                if (strum.HasFlag(ChordStrum.Mid))
                {
                    var gs = new GuitarChordStrum(this, ChordModifierType.ChordStrumMed);

                    gs.IsNew = true;

                    Modifiers.Add(gs);

                    if (createEvents)
                    {
                        gs.CreateEvents();
                    }
                }
                if (strum.HasFlag(ChordStrum.Low))
                {
                    var gs = new GuitarChordStrum(this, ChordModifierType.ChordStrumLow);

                    gs.IsNew = true;

                    Modifiers.Add(gs);

                    if (createEvents)
                    {
                        gs.CreateEvents();
                    }
                }
            }
        }


        public void AddSlide(bool reversed, bool createEvents)
        {
            if (!HasSlide)
            {
                var slide = new GuitarSlide(this, reversed);
                slide.IsNew = true;

                Modifiers.Add(slide);

                if (createEvents)
                {
                    slide.CreateEvents();
                }
            }
        }

        public override GuitarDifficulty Difficulty
        {
            get
            {
                var diff = Owner.Owner.CurrentDifficulty;
                if (Notes.Any())
                {
                    diff = Notes.FirstOrDefault().Difficulty;
                }
                return diff;
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

        public void AddHammeron(bool createEvents)
        {
            if (!HasHammeron)
            {
                var ho = new GuitarHammeron(this);
                Modifiers.Add(ho);

                ho.IsNew = true;

                if (createEvents)
                {
                    ho.CreateEvents();
                }
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
            var ret = base.ToString() + " ";

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
            ret.RootNoteConfig = RootNoteConfig.GetIfNotNull(x => x.Clone());
            GetTunedChordNames(null).ForEach(x =>
                ret.Names.Add(x));
            return ret;
        }
    }
}
