using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace ProUpgradeEditor.Common
{
    public class GuitarArpeggio : GuitarModifier
    {
        public GuitarArpeggio(MidiEventPair ev) : base(ev, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio)
        {
            Data1 = ev.Data1;
            
            SetTicks(ev.TickPair);
        }

        public GuitarArpeggio(GuitarMessageList owner, TickPair ticks, GuitarDifficulty difficulty)
            : base(owner, ticks, GuitarModifierType.Arpeggio, GuitarMessageType.GuitarArpeggio)
        {
            Data1 = Utility.GetArpeggioData1(difficulty);
        }

        public static GuitarArpeggio CreateArpeggio(GuitarMessageList owner, TickPair ticks, GuitarDifficulty difficulty)
        {
            GuitarArpeggio ret = null;
            if (Utility.GetArpeggioData1(difficulty).IsNotNull())
            {
                ret = new GuitarArpeggio(owner, ticks, difficulty);
                if (ret != null)
                {
                    ret.IsNew = true;
                    ret.CreateEvents();
                }
            }
            return ret;
        }
    }
}
