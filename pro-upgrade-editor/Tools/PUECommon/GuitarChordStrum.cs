﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{
    
    public class GuitarChordStrum : ChordModifier
    {
        public GuitarChordStrum(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent, ChordModifierType type = ChordModifierType.Invalid)
            : base(track, downEvent, upEvent, type, GuitarMessageType.GuitarChordStrum)
        {
        }

        public static GuitarChordStrum CreateStrum(GuitarTrack track, GuitarDifficulty difficulty, ChordStrum strum, TickPair ticks)
        {
            GuitarChordStrum ret = null;
            var d1 = Utility.GetStrumData1(difficulty);
            if (d1 != -1)
            {
                var ev = track.Insert(d1, 100, strum.GetChannelFromChordStrum(), ticks);
                ret = new GuitarChordStrum(track, ev.Down, ev.Up, strum.GetModifierType());

                track.Messages.Add(ret);
            }
            return ret;
        }

        public ChordStrum StrumMode
        {
            get { return Channel.GetChordStrumFromChannel(); }
            set { Channel = value.GetChannelFromChordStrum(); }
        }

        public override string ToString()
        {
            return "Strum: " + StrumMode.ToString();
        }
    }
}