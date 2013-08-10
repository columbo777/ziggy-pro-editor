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
    public class SerializedChord
    {

        public override string ToString()
        {
            return Serialize();
        }

        public string Serialize()
        {
            var ser = new XmlSerializer(typeof(SerializedChord));
            var sb = new StringBuilder();
            using (var tr = new StringWriter(sb))
            {
                ser.Serialize(tr, this);
            }
            return sb.ToString();
        }

        public static SerializedChord Deserialize(string str)
        {
            SerializedChord ret = null;
            var ser = new XmlSerializer(typeof(SerializedChord));

            using (var tr = new StringReader(str))
            {
                ret = (SerializedChord)ser.Deserialize(tr);
            }
            return ret;
        }

        public int TickLength { get; set; }
        public double TimeLength { get; set; }

        public List<SerializedChordNote> Notes { get; set; }
        public List<SerializedChordModifier> Modifiers { get; set; }
        public List<ChordNameMeta> Names { get; set; }
        
        public GuitarChordRootNoteConfig RootNoteConfig { get; set;}

        public SerializedChord()
        {
            Notes = new List<SerializedChordNote>();
            Modifiers = new List<SerializedChordModifier>();
            RootNoteConfig = new GuitarChordRootNoteConfig();
            Names = new List<ChordNameMeta>();
        }

        public GuitarChord Deserialize(GuitarMessageList owner, GuitarDifficulty diff, TickPair ticks)
        {
            int[] frets;
            int[] channels;
            ChordStrum chordStrum;
            bool isSlide;
            bool isSlideReverse;
            bool isHammeron;
            GuitarChordRootNoteConfig rootConfig;
            GetProperties(out frets, out channels, out chordStrum, out isSlide, out isSlideReverse, out isHammeron, out rootConfig);

            var ret = GuitarChord.CreateChord(owner, diff, ticks, 
                new GuitarChordConfig(frets, channels,
                    isSlide,
                    isSlideReverse,
                    isHammeron,
                    chordStrum,
                        rootConfig));

            return ret;
        }

        public void GetProperties(out int[] frets, out int[] channels, 
            out ChordStrum chordStrum, 
            out bool isSlide, 
            out bool isSlideReverse, 
            out bool isHammeron,
            out GuitarChordRootNoteConfig rootNoteConfig)
        {
            rootNoteConfig = this.RootNoteConfig.Clone();

            frets = Utility.Null6.ToArray();
            channels = Utility.Null6.ToArray();
            foreach (var n in Notes)
            {
                frets[n.String] = n.Fret;
                channels[n.String] = n.Channel;
            }
            var ct = Modifiers.Select(x => (ChordModifierType)x.Type).ToArray();

            chordStrum = ChordStrum.Normal;
            if (ct.Any(x => x == ChordModifierType.ChordStrumHigh))
            {
                chordStrum |= ChordStrum.High;
            }
            if (ct.Any(x => x == ChordModifierType.ChordStrumMed))
            {
                chordStrum |= ChordStrum.Mid;
            }
            if (ct.Any(x => x == ChordModifierType.ChordStrumLow))
            {
                chordStrum |= ChordStrum.Low;
            }
            isSlide = ct.Any(x => x == ChordModifierType.SlideReverse || x == ChordModifierType.Slide);
            isSlideReverse = ct.Any(x => x == ChordModifierType.SlideReverse);
            isHammeron = ct.Any(x => x == ChordModifierType.Hammeron);
        }
    }
}