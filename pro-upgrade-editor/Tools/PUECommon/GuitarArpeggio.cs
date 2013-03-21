using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    public class GuitarArpeggio : GuitarModifier
    {
        public GuitarArpeggio(GuitarMessageList track, MidiEvent downEvent, MidiEvent upEvent) :
            base(track, downEvent, upEvent, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio) { }
        
        public GuitarArpeggio(MidiEventPair ev) :
            base(ev.Owner, ev.Down, ev.Up, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio) { }

        public GuitarArpeggio(GuitarMessageList track, TickPair ticks) :
            base(track, null, null, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio)
        {
            SetTicks(ticks);
        }
        public static GuitarArpeggio GetArpeggio(GuitarMessageList track, GuitarDifficulty difficulty, TickPair ticks)
        {
            GuitarArpeggio ret = null;
            var d1 = Utility.GetArpeggioData1(difficulty);

            if (!d1.IsNull())
            {
                ret = new GuitarArpeggio(track, ticks);
                ret.Data1 = d1;
                ret.Data2 = 100;
                ret.Channel = Utility.ChannelDefault;
            }
            return ret;
        }
        public static GuitarArpeggio CreateArpeggio(GuitarMessageList track, GuitarDifficulty difficulty, TickPair ticks)
        {
            var ret = GetArpeggio(track, difficulty, ticks);
            if (ret != null)
            {
                ret.IsNew = true;
                ret.CreateEvents();

            }
            return ret;
        }
    }
}
