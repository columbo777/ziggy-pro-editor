using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarTrainer : GuitarMessage
    {
        GuitarTextEvent start, end, norm;


        public GuitarTextEvent Start { get { return start; } set { start = value; } }
        public GuitarTextEvent End { get { return end; } set { end = value; } }
        public GuitarTextEvent Norm
        {
            get
            {
                return norm;
            }
            set
            {
                norm = value;
                if (value == null)
                {
                    Loopable = false;
                }
                else
                {
                    Loopable = norm.Text.GetGuitarTrainerMetaEventType().IsTrainerNorm();
                }
            }
        }

        int trainerIndex = int.MinValue;
        GuitarTrainerType trainerType;

        public bool Loopable { get; set; }

        public bool Valid
        {
            get
            {
                return Start != null && Start.IsTrainerEvent && End != null && End.IsTrainerEvent &&
                    Start.Text.GetGuitarTrainerMetaEventType().IsTrainerBegin() &&
                    End.Text.GetGuitarTrainerMetaEventType().IsTrainerEnd();
            }
        }
        public GuitarTrainerType TrainerType
        {
            get { return trainerType; }
        }

        public int TrainerIndex
        {
            get
            {
                if (trainerIndex.IsNull() && start != null)
                {
                    var mt = start.Text;
                    if (!mt.IsEmpty() && mt.Contains(']') && mt.Contains('_'))
                    {
                        var li = mt.LastIndexOf('_');
                        var li2 = mt.LastIndexOf(']');
                        var idx = mt.Substring(li + 1, li2 - (li + 1));
                        trainerIndex = idx.ToInt();
                    }
                }
                return trainerIndex;
            }
            set
            {
                trainerIndex = value;
            }
        }

        public string StartText
        {
            get
            {

                if (TrainerType == GuitarTrainerType.ProGuitar)
                {
                    return Utility.TextEventBeginTag +
                        Utility.SongTrainerBeginPGText + " " +
                        Utility.SongTrainerPGText + TrainerIndex.ToStringEx() +
                        Utility.TextEventEndTag;
                }
                else if (TrainerType == GuitarTrainerType.ProBass)
                {
                    return Utility.TextEventBeginTag +
                       Utility.SongTrainerBeginPBText + " " +
                       Utility.SongTrainerPBText + TrainerIndex.ToStringEx() +
                       Utility.TextEventEndTag;
                }
                return string.Empty;
            }
        }
        public string NormText
        {
            get
            {
                if (Loopable)
                {
                    if (TrainerType == GuitarTrainerType.ProGuitar)
                    {
                        return Utility.TextEventBeginTag +
                           Utility.SongTrainerNormPGText + " " +
                           Utility.SongTrainerPGText + TrainerIndex.ToStringEx() +
                           Utility.TextEventEndTag;
                    }
                    else if (TrainerType == GuitarTrainerType.ProBass)
                    {
                        return Utility.TextEventBeginTag +
                           Utility.SongTrainerNormPBText + " " +
                           Utility.SongTrainerPBText + TrainerIndex.ToStringEx() +
                           Utility.TextEventEndTag;
                    }
                }
                return string.Empty;
            }
        }
        public string EndText
        {
            get
            {
                if (TrainerType == GuitarTrainerType.ProGuitar)
                {
                    return Utility.TextEventBeginTag +
                       Utility.SongTrainerEndPGText + " " +
                       Utility.SongTrainerPGText + TrainerIndex.ToStringEx() +
                       Utility.TextEventEndTag;
                }
                else if (TrainerType == GuitarTrainerType.ProBass)
                {
                    return Utility.TextEventBeginTag +
                       Utility.SongTrainerEndPBText + " " +
                       Utility.SongTrainerPBText + TrainerIndex.ToStringEx() +
                       Utility.TextEventEndTag;
                }
                return string.Empty;
            }
        }

        public void SetStart(MidiEvent ev)
        {
            var t = ev.MetaMessage.Text.GetGuitarTrainerMetaEventType();
            start = GuitarTextEvent.GetTextEvent(Owner, ev);
            trainerIndex = StartText.GetTrainerEventIndex();
        }

        public void SetEnd(MidiEvent ev)
        {
            end = GuitarTextEvent.GetTextEvent(Owner, ev);
        }

        public void SetNorm(MidiEvent ev)
        {
            if (ev != null)
            {
                norm.SetDownEvent(ev);
            }
        }

        public GuitarTrainer(GuitarMessageList track, GuitarTrainerType type)
            : base(track, null, null, GuitarMessageType.GuitarTrainer)
        {
            Start = GuitarTextEvent.GetTextEvent(track, null);
            End = GuitarTextEvent.GetTextEvent(track, null);
            Norm = GuitarTextEvent.GetTextEvent(track, null);

            this.trainerType = type;
            Loopable = false;
        }

        public override int DownTick
        {
            get
            {
                return Start.AbsoluteTicks;
            }

        }

        public override int UpTick
        {
            get
            {
                return End.AbsoluteTicks;
            }
        }

        public override void SetTicks(TickPair ticks)
        {
            if (ticks.IsValid)
            {
                Start.SetDownTick(ticks.Down);
                End.SetDownTick(ticks.Up);

                if (Loopable)
                {
                    Norm.SetDownTick(GetNormTick(ticks));
                }
            }
        }


        public override void CreateEvents()
        {

            TrainerIndex = Owner.Trainers.Where(g => g.TrainerType == this.TrainerType).Count() + 1;

            if (Start != null)
            {
                Start.SetDownTick(TickPair.Down);
                Start.Text = StartText;
                Start.CreateEvents();
                
            }
            if (End != null)
            {
                End.SetDownTick(TickPair.Up);
                End.Text = EndText;
                End.CreateEvents();
               
            }

            if (this.Loopable)
            {
                Norm.SetDownTick(GetNormTick(TickPair));
                Norm.Text = NormText;
                Norm.CreateEvents();
                
            }
            AddToList();
        }

        public override void UpdateEvents()
        {
            if (Start != null)
            {
                Start.SetDownTick(TickPair.Down);
                Start.Text = StartText;
                Start.UpdateEvents();
            }
            if (End != null)
            {
                End.SetDownTick(TickPair.Up);
                End.Text = EndText;
                End.UpdateEvents();
            }

            if (this.Loopable)
            {
                Norm.SetDownTick(GetNormTick(TickPair));
                Norm.Text = NormText;
                Norm.UpdateEvents();
            }
            else
            {
                if (Norm != null && Norm.MidiEvent != null)
                {
                    Owner.Remove(Norm);
                    Norm.SetMidiEvent(null);
                }
            }
        }

        public override void RemoveEvents()
        {
            Start.Owner.Remove(Start);
            Start.RemoveEvents();
            if (Norm != null && Norm.Owner != null)
            {
                Norm.Owner.Remove(Norm);
            }
            Norm.RemoveEvents();
            End.Owner.Remove(End);
            End.RemoveEvents();
        }


        int GetNormTick(TickPair ticks)
        {
            return ticks.Down + (int)((ticks.TickLength) * Utility.SongTrainerNormOffset);
        }


        public override string ToString()
        {
            string ret = string.Empty;
            if (TrainerType == GuitarTrainerType.ProGuitar)
            {
                ret = "Pro Guitar - ";
            }
            else if (TrainerType == GuitarTrainerType.ProBass)
            {
                ret = "Pro Bass - ";
            }
            else
            {
                ret = "Unknown - ";
            }
            ret += (TrainerIndex.IsNull() ? " [new] " : "[" + TrainerIndex.ToStringEx() + "] ") + Start.DownTick.ToStringEx() + " - " + End.DownTick.ToStringEx();
            return ret;
        }


    }

    public enum GuitarTrainerType
    {
        ProGuitar = 1,
        ProBass = 2,
        Unknown = 3,
    }

    public enum GuitarTrainerMetaEventType
    {
        BeginProGuitar = 1,
        ProGuitarNorm,
        EndProGuitar,
        BeginProBass,
        ProBassNorm,
        EndProBass,
        Unknown,
    }
}
