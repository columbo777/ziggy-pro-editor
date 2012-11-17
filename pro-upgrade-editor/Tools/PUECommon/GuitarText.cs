using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{

    public class GuitarTextEvent : GuitarMessage
    {
        string text;
        
        GuitarTrainerMetaEventType type;

        public GuitarTrainerMetaEventType Type { get { return type; } set { type = value; } }

        public GuitarTextEvent(GuitarTrack track, int tick, string text, MidiEvent midiEvent, GuitarTrainerMetaEventType type) : base(track, midiEvent)
        {
            this.text = text;
            this.type = type;
            downTick = tick;
        }

        public string Text 
        { 
            get 
            {
                return (DownEvent == null ? text : DownEvent.ToString()); 
            } 
            set 
            { 
                text = value; 
            } 
        }
        
        public override string ToString()
        {
            return Text;
        }


        public bool IsTrainerEvent
        {
            get { return Type != GuitarTrainerMetaEventType.Unknown; }
        }


    }

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
                return Start != null && Start.Type != GuitarTrainerMetaEventType.Unknown &&
                    End != null && End.Type != GuitarTrainerMetaEventType.Unknown;
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
                if(TrainerType == GuitarTrainerType.ProGuitar)
                {
                    return Utility.TextEventBeginTag +
                        Utility.SongTrainerBeginPGText + " " +
                        Utility.SongTrainerPGText +  TrainerIndex.ToStringEx() + 
                        Utility.TextEventEndTag;
                }
                else if(TrainerType == GuitarTrainerType.ProBass)
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
            start = new GuitarTextEvent(OwnerTrack, ev.AbsoluteTicks, ev.MetaMessage.Text, ev, t);
            trainerIndex = StartText.GetTrainerEventIndex();
        }

        public void SetEnd(MidiEvent ev)
        {
            end = new GuitarTextEvent(OwnerTrack, ev.AbsoluteTicks, ev.MetaMessage.Text, ev, ev.MetaMessage.Text.GetGuitarTrainerMetaEventType());
        }

        public void SetNorm(MidiEvent ev)
        {
            if (ev != null)
            {
                norm.SetDownEvent(ev);
                norm.Type = ev.MetaMessage.Text.GetGuitarTrainerMetaEventType();
            }
        }

        public GuitarTrainer(GuitarTrack track, GuitarTrainerType type)
            : base(track, null)
        {
            Start = new GuitarTextEvent(track, int.MinValue, "", null, GuitarTrainerMetaEventType.Unknown);
            End = new GuitarTextEvent(track, int.MinValue, "", null, GuitarTrainerMetaEventType.Unknown);
            Norm = new GuitarTextEvent(track, int.MinValue, "", null, GuitarTrainerMetaEventType.Unknown);

            this.trainerType = type;
            Loopable = false;

        }

        public void SetTicks( int start, int end, bool loopable)
        {
            
            if (end > start)
            {
                Start.Text = StartText;
                Start.DownTick = start;

                End.Text = EndText;
                End.DownTick = end;

                Loopable = loopable;
                if (loopable)
                {
                    Norm.Text = NormText;
                    Norm.DownTick = start + (int)((end - start) * Utility.SongTrainerNormOffset);
                }
                else
                {
                    if (Norm.DownEvent != null)
                    {
                        if (OwnerTrack != null)
                        {
                            OwnerTrack.Remove(Norm.DownEvent);
                            Norm.DownEvent = null;
                        }
                    }
                }

            }
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
        ProGuitar=1,
        ProBass=2,
        Unknown=3,
    }

    public enum GuitarTrainerMetaEventType
    {
        BeginProGuitar=1,
        ProGuitarNorm,
        EndProGuitar,
        BeginProBass,
        ProBassNorm,
        EndProBass,
        Unknown,
    }
}
