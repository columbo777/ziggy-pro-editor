using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProUpgradeEditor.DataLayer;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{

    public class GuitarTrainer : GuitarMessage
    {
        GuitarTextEvent start, end, norm;


        public GuitarTextEvent Start { get { return start; } set { start = value; } }
        public GuitarTextEvent End { get { return end; } set { end = value; } }
        public GuitarTextEvent Norm { get { return norm; } set { norm = value; } }

        int trainerIndex = int.MinValue;
        GuitarTrainerType trainerType;

        public bool Loopable { get; set; }

        public bool Valid
        {
            get
            {
                return Start != null && Start.IsTrainerEvent && End != null && End.IsTrainerEvent;
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
            start = GuitarTextEvent.GetTextEvent(OwnerTrack, ev);
            trainerIndex = StartText.GetTrainerEventIndex();
        }

        public void SetEnd(MidiEvent ev)
        {
            end = GuitarTextEvent.GetTextEvent(OwnerTrack, ev);
        }

        public void SetNorm(MidiEvent ev)
        {
            if (ev != null)
            {
                norm.SetDownEvent(ev);
            }
        }

        public GuitarTrainer(GuitarTrack track, GuitarTrainerType type)
            : base(track, null)
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
            set
            {
                Start.AbsoluteTicks = value;

            }
        }

        public override int UpTick
        {
            get
            {
                return End.AbsoluteTicks;
            }
            set
            {
                End.AbsoluteTicks = value;
            }
        }

        public void SetTicks(int start, int end, bool loopable)
        {

            if (end > start)
            {
                
                Start.Text = StartText;
                Start.DownTick = start;

                End.Text = EndText;
                End.DownTick = end;

                Loopable = loopable;
                if (Loopable)
                {
                    Norm.Text = NormText;
                    UpdateNormTick(start, end);
                }
                else
                {
                    if (Norm.DownEvent != null && OwnerTrack != null)
                    {
                        OwnerTrack.Remove(Norm);
                        Norm.DownEvent = null;
                    }
                }

            }
        }

        public void UpdateTicks(int start, int end, bool loopable)
        {
            if (end > start)
            {
                this.Loopable = loopable;
                this.Start.AbsoluteTicks = start;
                this.End.AbsoluteTicks = end;
                if (this.Loopable)
                {
                    UpdateNormTick(start, end);
                }
                else
                {
                    if (Norm.MidiEvent != null && OwnerTrack != null)
                    {
                        OwnerTrack.Remove(Norm);
                        Norm.MidiEvent = null;
                    }
                }
            }
        }



        public void RemoveSubMessages()
        {   
            if(OwnerTrack != null && !IsDeleted)
            {
                if (Start.MidiEvent != null)
                {
                    OwnerTrack.Remove(Start);
                    Start.MidiEvent = null;
                }
                
                if (Norm.MidiEvent != null)
                {
                    OwnerTrack.Remove(Norm);
                    Norm.MidiEvent = null;
                }
                if (End.MidiEvent != null)
                {
                    OwnerTrack.Remove(End);
                    End.MidiEvent = null;
                }

            }
        }

        private void UpdateNormTick(int start, int end)
        {
            var normTick = start + (int)((end - start) * Utility.SongTrainerNormOffset);
            if (Norm.MidiEvent == null)
            {
                Norm = GuitarTextEvent.CreateTextEvent(OwnerTrack, normTick, NormText);
            }
            Norm.AbsoluteTicks = normTick;
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
