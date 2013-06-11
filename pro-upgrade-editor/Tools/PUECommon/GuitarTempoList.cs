using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

using System.Collections;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class GuitarTempoList : SpecializedMessageList<GuitarTempo>
    {

        public GuitarTempoList(TrackEditor owner)
            : base(owner)
        {

        }
        public override GuitarMessageType MessageType
        {
            get { return GuitarMessageType.GuitarTempo; }
        }

        public override IEnumerator<GuitarTempo> GetEnumerator()
        {
            return base.GetEnumerator();
        }
        public GuitarTempo GetTempo(int tick)
        {
            var downTick = tick < 0 ? 0 : tick;
            var ret = this.SingleOrDefault(x => x.DownTick <= downTick && x.UpTick > downTick);
            if (ret == null)
            {
                ret = this.LastOrDefault();
            }
            return ret;
        }

    }
}