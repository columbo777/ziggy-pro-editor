using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    public class GuitarArpeggio : GuitarModifier
    {
        public GuitarArpeggio(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : 
            base(track, downEvent, upEvent, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio) { }


        public static GuitarArpeggio CreateArpeggio(GuitarTrack track, GuitarDifficulty difficulty, TickPair ticks)
        {
            GuitarArpeggio ret = null;
            var d1 = Utility.GetArpeggioData1(difficulty);

            if(!d1.IsNull())
            {
                var ev = track.Insert(d1, 100, 0, ticks);
                
                if (ev.IsNull==false)
                {
                    ret = new GuitarArpeggio(track, ev.Down, ev.Up);
                    track.Messages.Add(ret);
                }
            }
            return ret;
        }
    }
}
