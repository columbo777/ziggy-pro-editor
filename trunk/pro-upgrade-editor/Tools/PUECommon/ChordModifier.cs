using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    public class ChordModifier : GuitarMessage
    {
        public ChordModifierType ModifierType { get; internal set; }
        public GuitarChord Chord { get; internal set; }

        public ChordModifier(GuitarChord chord, ChordModifierType type, GuitarMessageType gt) :
            base(chord.Owner, chord.TickPair, gt)
        {
            ModifierType = type;
            Chord = chord;
        }

        public ChordModifier(MidiEventPair pair, ChordModifierType type, GuitarMessageType gt) :
            base(pair, gt)
        {
            Chord = null;
            ModifierType = type;
        }


        public override void AddToList()
        {
            
            base.AddToList();
            
            IsDeleted = false;

            IsNew = false;
        }

        public override void RemoveFromList()
        {
            Owner.Remove(this);
            IsDeleted = true;
        }

        public override void DeleteAll()
        {
            base.DeleteAll();

            if (Chord != null)
            {
                Chord.Modifiers.Remove(this);
            }
        }

        public override GuitarDifficulty Difficulty
        {
            get
            {
                return Chord == null ? EventPair.Data1.GetData1Difficulty(true) : Chord.Difficulty;
            }
        }

        public override int Data1
        {
            get
            {
                return ModifierType.GetData1ForChordModifierType(Difficulty, true).FirstOrDefault();
            }
            set
            {
                base.Data1 = value;
            }
        }

        public virtual bool IsSlide { get { return false; } }
        public virtual bool IsReversed { get { return false; } }
        public virtual ChordStrum StrumMode { get { return ChordStrum.Normal; } }

        public static ChordModifier GetModifier(GuitarChord chord, ChordModifierType type)
        {
            ChordModifier ret = null;
            switch (type)
            {
                case ChordModifierType.ChordStrumHigh:
                case ChordModifierType.ChordStrumMed:
                case ChordModifierType.ChordStrumLow:
                    if (Utility.GetStrumData1(chord.Difficulty).IsNotNull())
                    {
                        ret = new GuitarChordStrum(chord, type);
                    }
                    break;
                case ChordModifierType.Hammeron:
                    if (Utility.GetHammeronData1(chord.Difficulty).IsNotNull())
                    {
                        ret = new GuitarHammeron(chord);
                    }
                    break;
                case ChordModifierType.Slide:
                case ChordModifierType.SlideReverse:
                    if (Utility.GetSlideData1(chord.Difficulty).IsNotNull())
                    {
                        ret = new GuitarSlide(chord, type == ChordModifierType.SlideReverse);
                    }
                    break;
            }
            return ret;
        }

        public override string ToString()
        {
            return base.ToString() + ModifierType.ToString();
        }
        public static ChordModifier CreateModifier(GuitarChord chord, ChordModifierType type)
        {
            ChordModifier ret = null;

            switch (type)
            {
                case ChordModifierType.ChordStrumLow:
                    {
                        ret = GuitarChordStrum.CreateStrum(chord, ChordStrum.Low);
                    }
                    break;
                case ChordModifierType.ChordStrumMed:
                    {
                        ret = GuitarChordStrum.CreateStrum(chord, ChordStrum.Mid);
                    }
                    break;
                case ChordModifierType.ChordStrumHigh:
                    {
                        ret = GuitarChordStrum.CreateStrum(chord, ChordStrum.High);
                    }
                    break;
                case ChordModifierType.Hammeron:
                    {
                        ret = GuitarHammeron.CreateHammeron(chord);
                    }
                    break;
                case ChordModifierType.Slide:
                    {
                        ret = GuitarSlide.CreateSlide(chord, false);
                    }
                    break;
                case ChordModifierType.SlideReverse:
                    {
                        ret = GuitarSlide.CreateSlide(chord, true);
                    }
                    break;
            }

            return ret;
        }
    }
}