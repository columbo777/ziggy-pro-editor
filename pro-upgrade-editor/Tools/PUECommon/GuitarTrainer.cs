using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarTrainer : GuitarMessage
    {
        public GuitarTrainer(GuitarMessageList owner, TickPair ticks, GuitarTrainerType type, GuitarTextEvent start, GuitarTextEvent end, GuitarTextEvent norm)
            : base(owner, ticks, GuitarMessageType.GuitarTrainer)
        {
            this.TrainerType = type;

            Start = start;
            End = end;
            Norm = norm;

            this.TrainerIndex = ParseTrainerIndex();

            this.Loopable = norm != null;

            SetTicks(ticks);
        }

        public GuitarTrainer(GuitarMessageList owner, TickPair ticks, GuitarTrainerType type, bool loopable, int index = Int32.MinValue)
            : base(owner, ticks, GuitarMessageType.GuitarTrainer)
        {
            this.TrainerType = type;
            this.TrainerIndex = index.IsNull() ? (owner.Trainers.Where(x => x.TrainerType == type).Count() + 1) : index;
            this.Loopable = loopable;

            Start = new GuitarTextEvent(owner, ticks.Down, GetStartText(TrainerType, TrainerIndex));
            End = new GuitarTextEvent(owner, ticks.Up, GetEndText(TrainerType, TrainerIndex));

            if (Loopable)
            {
                Norm = new GuitarTextEvent(owner, GetNormTick(ticks), GetNormText(TrainerType, TrainerIndex, Loopable));
            }

            SetTicks(ticks);
        }


        public GuitarTextEvent Start { get; internal set; }
        public GuitarTextEvent End { get; internal set; }
        public GuitarTextEvent Norm { get; internal set; }

        public GuitarTrainerType TrainerType { get; internal set; }
        public int TrainerIndex { get; internal set; }
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

        int ParseTrainerIndex()
        {
            var ret = Int32.MinValue;
            var mt = Start.Text;
            if (!mt.IsEmpty() && mt.Contains(']') && mt.Contains('_'))
            {
                var li = mt.LastIndexOf('_');
                var li2 = mt.LastIndexOf(']');
                var idx = mt.Substring(li + 1, li2 - (li + 1));
                ret = idx.ToInt();
            }
            return ret;
        }


        public static string GetStartText(GuitarTrainerType type, int index)
        {
            if (type == GuitarTrainerType.ProGuitar)
            {
                return Utility.TextEventBeginTag +
                    Utility.SongTrainerBeginPGText + " " +
                    Utility.SongTrainerPGText + index.ToStringEx() +
                    Utility.TextEventEndTag;
            }
            else if (type == GuitarTrainerType.ProBass)
            {
                return Utility.TextEventBeginTag +
                   Utility.SongTrainerBeginPBText + " " +
                   Utility.SongTrainerPBText + index.ToStringEx() +
                   Utility.TextEventEndTag;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetEndText(GuitarTrainerType type, int index)
        {
            if (type == GuitarTrainerType.ProGuitar)
            {
                return Utility.TextEventBeginTag +
                   Utility.SongTrainerEndPGText + " " +
                   Utility.SongTrainerPGText + index.ToStringEx() +
                   Utility.TextEventEndTag;
            }
            else if (type == GuitarTrainerType.ProBass)
            {
                return Utility.TextEventBeginTag +
                   Utility.SongTrainerEndPBText + " " +
                   Utility.SongTrainerPBText + index.ToStringEx() +
                   Utility.TextEventEndTag;
            }
            return string.Empty;
        }

        public static string GetNormText(GuitarTrainerType type, int index, bool loopable)
        {
            if (loopable)
            {
                if (type == GuitarTrainerType.ProGuitar)
                {
                    return Utility.TextEventBeginTag +
                       Utility.SongTrainerNormPGText + " " +
                       Utility.SongTrainerPGText + index.ToStringEx() +
                       Utility.TextEventEndTag;
                }
                else if (type == GuitarTrainerType.ProBass)
                {
                    return Utility.TextEventBeginTag +
                       Utility.SongTrainerNormPBText + " " +
                       Utility.SongTrainerPBText + index.ToStringEx() +
                       Utility.TextEventEndTag;
                }
            }
            return string.Empty;
        }

        public string StartText
        {
            get
            {
                return GetStartText(TrainerType, TrainerIndex);
            }
        }
        public string NormText
        {
            get
            {
                return GetNormText(TrainerType, TrainerIndex, Loopable);
            }
        }
        public string EndText
        {
            get
            {
                return GetEndText(TrainerType, TrainerIndex);
            }
        }

        public void SetStart(GuitarTextEvent ev)
        {
            Start = ev;
        }

        public void SetEnd(GuitarTextEvent ev)
        {
            End = ev;
        }

        public void SetNorm(GuitarTextEvent ev)
        {
            Norm = ev;
            if (ev == null)
            {
                Loopable = false;
            }
            else
            {
                Loopable = true;
            }
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
            Start.IfObjectNotNull(n => n.SetDownTick(ticks.Down));
            End.IfObjectNotNull(n => n.SetDownTick(ticks.Up));
            Norm.IfObjectNotNull(n => n.SetDownTick(GetNormTick(ticks)));

            base.SetTicks(ticks);
        }


        public override void CreateEvents()
        {
            if (TrainerIndex.IsNull())
            {
                TrainerIndex = Owner.Trainers.Where(g => g.TrainerType == this.TrainerType).Count() + 1;
            }

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
                if (Norm != null)
                {
                    Norm.SetDownTick(GetNormTick(TickPair));
                    Norm.Text = NormText;
                    Norm.CreateEvents();
                }
            }

            base.CreateEvents();
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
                if (Norm != null)
                {
                    Norm.DeleteAll();
                    Norm = null;
                }
            }
            base.UpdateEvents();
        }

        public override void RemoveEvents()
        {
            Start.IfObjectNotNull(n => n.RemoveEvents());
            End.IfObjectNotNull(n => n.RemoveEvents());
            Norm.IfObjectNotNull(n => n.RemoveEvents());
            base.RemoveEvents();
        }

        public override void AddToList()
        {
            Start.IfObjectNotNull(n => n.AddToList());
            End.IfObjectNotNull(n => n.AddToList());
            Norm.IfObjectNotNull(n => n.AddToList());

            base.AddToList();
        }

        public override void DeleteAll()
        {
            Start.IfObjectNotNull(n => n.DeleteAll());
            Start = null;
            End.IfObjectNotNull(n => n.DeleteAll());
            End = null;
            Norm.IfObjectNotNull(n => n.DeleteAll());
            Norm = null;

            base.DeleteAll();
        }

        public override void RemoveFromList()
        {
            Start.IfObjectNotNull(n => n.RemoveFromList());
            End.IfObjectNotNull(n => n.RemoveFromList());
            Norm.IfObjectNotNull(n => n.RemoveFromList());

            base.RemoveFromList();
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
