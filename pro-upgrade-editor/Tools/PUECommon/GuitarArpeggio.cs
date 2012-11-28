using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

namespace ProUpgradeEditor.DataLayer
{
    public class GuitarArpeggio : GuitarModifier
    {
        public GuitarArpeggio(GuitarTrack track, MidiEvent downEvent, MidiEvent upEvent) : base(track, downEvent, upEvent, GuitarModifierType.Arpeggio) { }


        public static GuitarArpeggio CreateArpeggio(GuitarTrack track, GuitarDifficulty difficulty, int downTick, int upTick)
        {
            GuitarArpeggio ret = null;
            var d1 = Utility.GetArpeggioData1(difficulty);

            if(!d1.IsNull())
            {
                MidiEvent downEvent, upEvent;
                Utility.CreateMessage(track, d1, 100, 0, downTick, upTick, out downEvent, out upEvent);

                ret = new GuitarArpeggio(track, downEvent, upEvent);

                track.Messages.Add(ret);
            }
            return ret;
        }
    }
}
