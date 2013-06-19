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
    [Serializable()]
    public class GuitarChordRootNoteConfig
    {
        public virtual bool UseUserChordName { get; set; }
        public virtual string UserChordName { get; set; }
        public virtual int RootNoteData1 { get; set; }
        public virtual bool HideNoteName { get; set; }
        public virtual ChordNameMeta ChordNameMeta { get; set; }

        public GuitarChordRootNoteConfig()
        {
            UseUserChordName = false;
            UserChordName = string.Empty;
            RootNoteData1 = Int32.MinValue;
            HideNoteName = false;
            ChordNameMeta = null;
        }

        public GuitarChordRootNoteConfig Clone()
        {
            return new GuitarChordRootNoteConfig()
            {
                UseUserChordName = this.UseUserChordName,
                UserChordName = this.UserChordName,
                ChordNameMeta = this.ChordNameMeta.GetIfNotNull(x=> x.Clone()),
                HideNoteName = this.HideNoteName,
                RootNoteData1 = this.RootNoteData1,
            };
        }
    }
    public class GuitarChordConfig
    {
        public int[] Frets { get; internal set; }
        public int[] Channels { get; internal set; }
        public bool IsSlide { get; internal set; }
        public bool IsSlideReverse { get; internal set; }
        public bool IsHammeron { get; internal set; }
        public ChordStrum StrumMode { get; internal set; }
        public GuitarChordRootNoteConfig RootNoteConfig { get; internal set; }

        public GuitarChordConfig Clone()
        {
            return new GuitarChordConfig()
            {
                Frets = this.Frets.ToArray(),
                Channels = this.Channels.ToArray(),
                IsSlide= this.IsSlide,
                IsSlideReverse = this.IsSlideReverse ,
                IsHammeron = this.IsHammeron,
                StrumMode = this.StrumMode,
                RootNoteConfig = this.RootNoteConfig.GetIfNotNull(x=> x.Clone()),
            };
        }
        public GuitarChordConfig()
        {
            Frets = Utility.Null6.ToArray();
            Channels = Utility.Null6.ToArray();
            IsSlide = false;
            IsSlideReverse = false;
            IsHammeron = false;
            StrumMode = ChordStrum.Normal;
            RootNoteConfig = null;
        }

        public GuitarChordConfig(int[] frets, int[] channels,
            bool isSlide, bool isSlideReverse,
            bool isHammeron,
            ChordStrum strumMode,
            GuitarChordRootNoteConfig rootNoteConfig)
            : this()
        {
            this.RootNoteConfig = rootNoteConfig;
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