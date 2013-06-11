using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

using System.Drawing;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace ProUpgradeEditor.Common
{
    public class GuitarChordConfig
    {
        public int[] Frets { get; internal set; }
        public int[] Channels { get; internal set; }
        public bool IsSlide { get; internal set; }
        public bool IsSlideReverse { get; internal set; }
        public bool IsHammeron { get; internal set; }
        public ChordStrum StrumMode { get; internal set; }

        public GuitarChordConfig()
        {
            Frets = Utility.Null6.ToArray();
            Channels = Utility.Null6.ToArray();
            IsSlide = false;
            IsSlideReverse = false;
            IsHammeron = false;
            StrumMode = ChordStrum.Normal;
        }

        public GuitarChordConfig(int[] frets = null, int[] channels = null, bool isSlide = false, bool isSlideReverse = false, bool isHammeron = false, ChordStrum strumMode = ChordStrum.Normal)
            : this()
        {
            if (frets != null)
            {
                Frets = frets.ToArray();
            }

            if (channels != null)
            {
                Channels = channels.ToArray();

                if (Frets != null)
                {
                    for (int x = 0; x < Frets.Count(); x++)
                    {
                        if (Frets[x].IsNotNull() && Channels[x].IsNull())
                        {
                            Channels[x] = Utility.ChannelDefault;
                        }
                        else if (Frets[x].IsNull() && Channels[x].IsNotNull())
                        {
                            Channels[x] = Int32.MinValue;
                        }
                    }
                }

                if (Channels.Any(x => x == Utility.ChannelX))
                {
                    for (int x = 0; x < Channels.Count(); x++)
                    {
                        if (Channels[x].IsNotNull())
                        {
                            Channels[x] = Utility.ChannelX;
                        }
                    }
                }
            }
            IsSlide = isSlide | isSlideReverse;
            IsSlideReverse = isSlideReverse;
            IsHammeron = isHammeron;
            StrumMode = strumMode;
        }
    }
}