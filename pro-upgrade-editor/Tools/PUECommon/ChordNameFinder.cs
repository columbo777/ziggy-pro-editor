using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{



    public static class ChordNameFinder
    {

        class ChordNameListMeta
        {
            public int[] Notes { get; internal set; }
            public string Description { get; internal set; }

            public ChordNameListMeta(int[] n, string d)
            {
                this.Notes = n;
                this.Description = d;
            }
        }

        public static IEnumerable<ChordNameMeta> GetChordNames(IEnumerable<ToneNameEnum> tones)
        {
            var ret = new List<ChordNameMeta>();

            if (tones != null)
            {
                tones = tones.Where(x => x != ToneNameEnum.NotSet).ToArray();
                if (tones.Count() > 2)
                {
                    var ecTones = tones.Select(x => GetECToneName(x)).ToArray();
                    ret.AddRange(EC_CalculateChordSymbols(ecTones, false));
                }
            }

            return ret;
        }

        public static ChordNameMeta GetChordName(IEnumerable<ToneNameEnum> tones)
        {
            ChordNameMeta ret = null;

            if (tones != null)
            {
                tones = tones.Where(x => x != ToneNameEnum.NotSet).ToArray();
                if (tones.Count() > 2)
                {
                    var ecTones = tones.Select(x => GetECToneName(x)).ToArray();

                    ret = EC_CalculateChordSymbols(ecTones, true).FirstOrDefault();
                }
            }

            return ret;
        }

        static ChordNameMeta EC_ShowChord(string chord)
        {
            ChordNameMeta ret = null;

            if (chord.IsNotEmpty())
            {
                ret = new ChordNameMeta();

                ret.RootNote = chord[0].ToString();

                for (var j = 1; j < chord.Length; j++)
                {
                    var c = chord[j];
                    switch (c)
                    {
                        case '>':
                            ret.IsSuper = true;
                            break;
                        case '#':
                        case 'b':
                            {
                                if (c == 'b')
                                {
                                    ret.IsFlat = true;
                                }
                                else
                                {
                                    ret.IsSharp = true;
                                }
                            }
                            break;
                        default:
                            {
                                if (ret.IsSuper)
                                {
                                    ret.Super += c;
                                }
                                else
                                {
                                    ret.Number += c;
                                }
                            }
                            break;
                    }
                }
            }
            return ret;
        }

        static IEnumerable<ChordNameMeta> EC_CalculateChordSymbols(ECToneNameEnum[] tones, bool firstOnly)
        {
            var ret = new List<ChordNameMeta>();
            if (tones.IsEmpty())
            {
                return ret;
            }
            var distinctTones = tones.Distinct().OrderBy(x => x.ToInt()).ToList();
            if (distinctTones.Count() == 1)
            {
                ret.Add(new ChordNameMeta() { ToneName = ChordNameFinder.GetToneName(distinctTones.First()) });
                return ret;
            }
            else if (distinctTones.Count() == 2)
            {
                distinctTones.Add(distinctTones.First());
            }

            var n = distinctTones.Count();
            if (n < 3 || n > 5)
            {
                return ret;
            }

            var ecNotes = distinctTones.Select(x => EC_NotesDir[((int)x)]).ToArray();

            for (var i = 0; i < EC_Chords.Length; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    var x = 0;
                    if (n == EC_Chords[i].Notes.Length)
                    {
                        for (var k = 0; k < EC_Chords[i].Notes.Length; k++)
                        {
                            var tone = ((EC_Chords[i].Notes[k] + j - 1) % 12);

                            for (var l = 0; l < n; l++)
                            {
                                if (tone == (int)tones[l])
                                {
                                    x++;
                                }
                            }
                        }
                    }
                    if (x == EC_Chords[i].Notes.Length)
                    {
                        var chord = EC_ShowChord(EC_NotesDir[j] + EC_Chords[i].Description);
                        if (chord != null)
                        {
                            ret.Add(chord);

                            if (firstOnly)
                                break;
                        }
                    }
                }
            }
            return ret;
        }


        public enum ECToneNameEnum
        {
            C = 0,
            Cs,
            D,
            Ds,
            E,
            F,
            Fs,
            G,
            Gs,
            A,
            As,
            B,
        }


        static ECToneNameEnum GetECToneName(ToneNameEnum tone)
        {
            return (ECToneNameEnum)((((int)tone) + 4) % 12);
        }

        static ToneNameEnum GetToneName(ECToneNameEnum tone)
        {
            return (ToneNameEnum)((((int)tone) + 8) % 12);
        }

        static readonly string[] EC_NotesUp = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        static readonly string[] EC_NotesDn = new[] { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        static string[] EC_NotesDir { get { return EC_NotesUp; } }

        static readonly ChordNameListMeta[] EC_Chords =
            new[]
        {
            new ChordNameListMeta(new[]{1,5,8}, ""),
            new ChordNameListMeta(new[]{1,4,8}, "m"),
            new ChordNameListMeta(new[]{1,5,9}, ">+(5)"),
            new ChordNameListMeta(new[]{1,4,7}, ">0"),
            new ChordNameListMeta(new[]{1,5,7}, ">-5"),
            new ChordNameListMeta(new[]{1,4,9}, "m>+(5)"),
            new ChordNameListMeta(new[]{3,5,8}, ">add9"),
            new ChordNameListMeta(new[]{3,5,8}, ">sus2"),
            new ChordNameListMeta(new[]{3,4,8}, "m>add9"),
            new ChordNameListMeta(new[]{3,4,8}, "m>sus2"),
            new ChordNameListMeta(new[]{4,5,11}, "(7)>+9"),
            new ChordNameListMeta(new[]{1,6,8}, ">sus(4)"),
            new ChordNameListMeta(new[]{1,6,8}, "m>sus(4)"),
            new ChordNameListMeta(new[]{5,10,11}, "13"),
            new ChordNameListMeta(new[]{5,9,11}, "-13"),
            new ChordNameListMeta(new[]{1,5,8,11}, "7"),
            new ChordNameListMeta(new[]{3,5,8,11}, "9"),
            new ChordNameListMeta(new[]{1,5,8,10}, "6"),
            new ChordNameListMeta(new[]{1,4,8,11}, "m7"),
            new ChordNameListMeta(new[]{1,4,8,10}, "m6"),
            new ChordNameListMeta(new[]{3,4,8,11}, "m9"),
            new ChordNameListMeta(new[]{1,5,8,12}, "maj7"),
            new ChordNameListMeta(new[]{1,4,8,12}, "m(maj7)"),
            new ChordNameListMeta(new[]{3,5,8,12}, "maj9"),
            new ChordNameListMeta(new[]{3,4,8,12}, "m(maj9)"),
            new ChordNameListMeta(new[]{2,5,8,11}, "(7)>-9"),
            new ChordNameListMeta(new[]{3,5,8,10}, "6.9"),
            new ChordNameListMeta(new[]{3,4,8,10}, "m6.9"),
            new ChordNameListMeta(new[]{1,6,8,11}, "7>sus(4)"),
            new ChordNameListMeta(new[]{1,6,8,11}, "m7>sus(4)"),
            new ChordNameListMeta(new[]{1,5,9,11}, "7>+(5)"),
            new ChordNameListMeta(new[]{1,5,7,11}, "7>-5"),
            new ChordNameListMeta(new[]{1,4,7,11}, "m7>-5"),
            new ChordNameListMeta(new[]{3,5,9,11}, "9>+5"),
            new ChordNameListMeta(new[]{3,5,9,11}, ">-13.9"),
            new ChordNameListMeta(new[]{3,5,7,11}, "9>-5"),
            new ChordNameListMeta(new[]{2,5,9,11}, "(7)>-9+5"),
            new ChordNameListMeta(new[]{2,5,7,11}, "(7)>-9-5"),
            new ChordNameListMeta(new[]{2,5,7,11}, ">+11-9"),
            new ChordNameListMeta(new[]{4,5,9,11}, "(7)>+9+5"),
            new ChordNameListMeta(new[]{4,5,7,11}, "(7)>+9-5"),
            new ChordNameListMeta(new[]{4,5,7,11}, ">+11+9"),
            new ChordNameListMeta(new[]{3,6,8,11}, "11"),
            new ChordNameListMeta(new[]{3,5,10,11}, "13.9"),
            new ChordNameListMeta(new[]{2,5,10,11}, "13>-9"),
            new ChordNameListMeta(new[]{2,5,9,11}, ">-13-9"),
            new ChordNameListMeta(new[]{3,6,10,11}, "13.11(9)"),
            new ChordNameListMeta(new[]{2,6,10,11}, "13.11-9"),
            new ChordNameListMeta(new[]{3,7,10,11}, "13>+11(9)"),
            new ChordNameListMeta(new[]{3,5,7,8,11}, ">+11"),
            new ChordNameListMeta(new[]{1,5,7,10,11}, "13>-5"),
            new ChordNameListMeta(new[]{1,5,7,9,11}, ">-13-5"),
            new ChordNameListMeta(new[]{1,5,7,9,11}, ">+11+5"),
            new ChordNameListMeta(new[]{2,5,7,10,11}, "13>-9-5"),
            new ChordNameListMeta(new[]{3,5,7,9,11}, "9>+5-5")
        };

    }
}