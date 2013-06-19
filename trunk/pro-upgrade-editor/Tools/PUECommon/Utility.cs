using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;


namespace ProUpgradeEditor.Common
{



    public static class Utility
    {
        public static int ScollToSelectionOffset = 300;
        public static uint RockBand3TitleID = 1161890068;

        public static int[] Null6 { get { return new int[] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue }; } }

        public const int ChannelStrumLow = 15;
        public const int ChannelStrumMid = 14;
        public const int ChannelStrumHigh = 13;
        public const int ChannelSlideReversed = 11;
        public const int ChannelSlide = 0;
        public const int ChannelTap = 4;
        public const int ChannelX = 3;
        public const int ChannelArpeggio = 1;
        public const int ChannelStringBend = 2;
        public const int ChannelDefault = 0;
        public const int Data2Default = 100;

        public static ChannelMessageBuilder CMBuilder = new ChannelMessageBuilder();
        public const int SingleStringTremeloData1 = 127;
        public const int MultiStringTremeloData1 = 126;

        public static int[] BigRockEndingData1 = new int[] { 120, 121, 122, 123, 124, 125 };

        public const int PowerupData1 = 116;
        public const int SoloData1 = 115;

        public const int ExpertSoloData1_G5 = 103;
        public const int HardSoloData1_G5 = 91;
        public const int MediumSoloData1_G5 = 79;
        public const int EasySoloData1_G5 = 67;



        public static readonly int[] SoloData1_G5 = new int[] { EasySoloData1_G5, MediumSoloData1_G5, HardSoloData1_G5, ExpertSoloData1_G5 };

        public const int HandPositionData1 = 108;

        public const int ExpertStrumData1 = 105;
        public const int ExpertArpeggioData1 = 104;

        public const int ExpertSlideData1 = 103;
        public const int ExpertHammeronData1 = 102;


        public const int ExpertData1HighE = 101;
        public const int ExpertData1B = 100;
        public const int ExpertData1G = 99;
        public const int ExpertData1D = 98;
        public const int ExpertData1A = 97;
        public const int ExpertData1LowE = 96;

        public const int HardData1B5 = 88;
        public const int HardData1G5 = 87;
        public const int HardData1D5 = 86;
        public const int HardData1A5 = 85;
        public const int HardData1LowE5 = 84;

        public const int HardStrumData1 = 81;
        public const int HardArpeggioData1 = 80;
        public const int HardSlideData1 = 79;
        public const int HardHammeronData1 = 78;

        public const int HardData1HighE = 77;
        public const int HardData1B = 76;
        public const int HardData1G = 75;
        public const int HardData1D = 74;
        public const int HardData1A = 73;
        public const int HardData1LowE = 72;

        public const int MediumData1G5 = 75;
        public const int MediumData1D5 = 74;
        public const int MediumData1A5 = 73;
        public const int MediumData1LowE5 = 72;

        public const int EasyData1D5 = 62;
        public const int EasyData1A5 = 61;
        public const int EasyData1LowE5 = 60;

        public const int MediumSlideData1 = 55;
        public const int MediumHammeronData1 = 54;
        public const int MediumData1HighE = 53;
        public const int MediumData1B = 52;
        public const int MediumData1G = 51;
        public const int MediumData1D = 50;
        public const int MediumData1A = 49;
        public const int MediumData1LowE = 48;

        public const int EasySlideData1 = 31;
        public const int EasyHammeronData1 = 30;
        public const int EasyData1HighE = 29;
        public const int EasyData1B = 28;
        public const int EasyData1G = 27;
        public const int EasyData1D = 26;
        public const int EasyData1A = 25;
        public const int EasyData1LowE = 24;

        public const int ChordNameHiddenData1 = 17;
        public const int ChordNameSlashChordData1 = 16;
        public const int ChordNameEbData1 = 15;
        public const int ChordNameDData1 = 14;
        public const int ChordNameDbData1 = 13;
        public const int ChordNameCData1 = 12;
        public const int ChordNameBData1 = 11;
        public const int ChordNameBbData1 = 10;
        public const int ChordNameAData1 = 9;
        public const int ChordNameAbData1 = 8;
        public const int ChordNameGData1 = 7;
        public const int ChordNameGbData1 = 6;
        public const int ChordNameFData1 = 5;
        public const int ChordNameEData1 = 4;

        public static int[] AllSlideData1 = { ExpertSlideData1, HardSlideData1, MediumSlideData1, EasySlideData1 };
        public static int[] AllStrumData1 = { ExpertStrumData1, HardStrumData1 };
        public static int[] AllArpeggioData1 = { ExpertArpeggioData1, HardArpeggioData1 };
        public static int[] AllHammeronData1 = { ExpertHammeronData1, HardHammeronData1, MediumHammeronData1, EasyHammeronData1 };

        public static int[] AllChordModifierData1 =
        {
            ExpertSlideData1, HardSlideData1, MediumSlideData1, EasySlideData1,
            ExpertStrumData1, HardStrumData1,
            ExpertArpeggioData1, HardArpeggioData1,
            ExpertHammeronData1, HardHammeronData1, MediumHammeronData1, EasyHammeronData1,
        };

        public static string GetChordNameTextPrefix(GuitarDifficulty difficulty)
        {
            int iDiff;
            if (difficulty.IsExpertAll())
            {
                iDiff = 3;
            }
            else if (difficulty.IsHard())
            {
                iDiff = 2;
            }
            else if (difficulty.IsMedium())
            {
                iDiff = 1;
            }
            else
            {
                iDiff = 0;
            }
            return "chrd" + iDiff + " ";
        }

        public static string CreateChordNameText(GuitarDifficulty difficulty, ChordNameMeta chordName)
        {
            return "[" + GetChordNameTextPrefix(difficulty) + " " + chordName.ToStringEx().GetIfEmpty(" ") + "]";
        }

        public static string CreateChordNameText(GuitarDifficulty difficulty, string chordName)
        {
            return "[" + GetChordNameTextPrefix(difficulty) + " " + chordName.GetIfEmpty(" ") + "]";
        }

        public static GuitarDifficulty[] EasyMediumHardExpert = { GuitarDifficulty.Easy, GuitarDifficulty.Medium, GuitarDifficulty.Hard, GuitarDifficulty.Expert };

        public static bool ClearChordNames = true;

        public static bool HandPositionGenerationEnabled = true;
        public static bool HandPositionMarkerByDifficulty = false;
        public static int HandPositionMarkerFirstBeginOffset = 20;
        public static int HandPositionMarkerFirstEndOffset = 40;

        public static int HandPositionMarkerStartOffset = 0;
        public static int HandPositionMarkerEndOffset = 0;
        public static int HandPositionMarkerMaxFret = 17;
        public static int HandPositionMarkerMinFret = 15;



        public static int MinimumNoteWidth = 0;

        public static string XNoteText = "x";
        public static int XNoteTextYOffset = -1;

        public static int XNoteTextXOffset = 0;

        public static int NoteTextYOffset = 0;
        public static int NoteTextXOffset = 0;

        public static string ArpeggioHelperPrefix = "(";
        public static string ArpeggioHelperSuffix = ")";
        public static int NoteShadowOffsetLeft = 0;
        public static int NoteShadowOffsetUp = 0;
        public static int NoteShadowOffsetRight = 2;
        public static int NoteShadowOffsetDown = 2;

        public static int BarLineWidth = 1;
        public static int DrawBeat = 1;

        public static int HScrollSmallChange = 500;
        public static int HScrollLargeChange = 500;

        public static int MaxBackups = 20;

        public static string DTADifficultyNoPart = "000";
        public static string DTADifficultyWarmup = "1";
        public static string DTADifficultyApprentice = "199";
        public static string DTADifficultySolid = "220";
        public static string DTADifficultyModerate = "268";
        public static string DTADifficultyChallenging = "373";
        public static string DTADifficultyNightmare = "440";
        public static string DTADifficultyImpossible = "456";

        public static string DefaultCONFileExtension = "pro";
        public static string DefaultPROFileExtension = "_pro.mid";

        public static string StoredChordPrefix = "[";
        public static string StoredChordNoteSeparator = " - ";
        public static string StoredChordEmptyNote = "_";
        public static string StoredChordSuffix = "]  ";
        public static string StoredChordSlide = "[S]";
        public static string StoredChordReverse = "[R]";
        public static string StoredChordHammeron = "[H]";
        public static string StoredChordTap = "[T]";
        public static string StoredChordXNote = "[X]";
        public static string StoredChordStrumLow = "[SL]";
        public static string StoredChordStrumMed = "[SM]";
        public static string StoredChordStrumHigh = "[SH]";

        public static int DummyTempo = 405405;


        public static StringFormat GetStringFormatNoWrap()
        {
            return new StringFormat(StringFormat.GenericDefault.FormatFlags | StringFormatFlags.NoWrap);
        }

        public static int GetArpeggioData1(GuitarDifficulty diff)
        {
            if (diff.IsExpertAll())
            {
                return ExpertArpeggioData1;
            }
            else if (diff.IsHard())
            {
                return HardArpeggioData1;
            }

            return Int32.MinValue;
        }

        public static int GetSlideData1(GuitarDifficulty diff)
        {
            if (diff.IsExpertAll())
            {
                return ExpertSlideData1;
            }
            else if (diff.IsHard())
            {
                return HardSlideData1;
            }
            else if (diff.IsMedium())
            {
                return MediumSlideData1;
            }
            else if (diff.IsEasy())
            {
                return EasySlideData1;
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public static IEnumerable<int> GetBigRockEndingData1(bool isPro)
        {
            if (isPro)
            {
                return BigRockEndingData1.ToList();
            }
            else
            {
                return BigRockEndingData1.Skip(1).ToList();
            }
        }

        public static int GetStrumData1(GuitarDifficulty diff)
        {
            if (diff.IsExpertAll())
            {
                return ExpertStrumData1;
            }
            else if (diff.IsHard())
            {
                return HardStrumData1;
            }
            return Int32.MinValue;
        }

        public static int GetHammeronData1(GuitarDifficulty diff)
        {

            if (diff.IsExpertAll())
            {
                return ExpertHammeronData1;
            }
            else if (diff.IsHard())
            {
                return HardHammeronData1;
            }
            else if (diff.IsMedium())
            {
                return MediumHammeronData1;
            }
            else if (diff.IsEasy())
            {
                return EasyHammeronData1;
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public static int WarnOnInvalidStrum = 1;

        public static int GridSnapDistance = 4;
        public static int NoteSnapDistance = 4;

        public static int NoteCloseWidth = 8;

        public static int TickCloseWidth = 4;

        public static bool IsCloseTick(int a, int b)
        {
            return (Math.Abs(a - b) < TickCloseWidth);
        }

        public static int SelectorWidth = 4;
        public static int SelectorHeight = 8;
        public static int ShowSelectorDist = 20;
        public static int ShowSelectorEnableDist = 10;

        public static int gridSnapCursorSize = 6;
        public static int noteBoundWidth = 1;

        public static float fontHeight = 8;
        public static Font fretFont = new Font("Courier New", fontHeight);

        public const int defaultAlpha = 150;
        public static SolidBrush fretBrush = new SolidBrush(Color.Black);
        public static SolidBrush BackgroundBrush = new SolidBrush(Color.White);

        public static SolidBrush G5GreenNoteBrush = new SolidBrush(Color.FromArgb(defaultAlpha, Color.Green));
        public static SolidBrush G5RedNoteBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 255, 120, 120));
        public static SolidBrush G5YellowNoteBrush = new SolidBrush(Color.FromArgb(defaultAlpha, Color.Yellow));
        public static SolidBrush G5BlueNoteBrush = new SolidBrush(Color.FromArgb(defaultAlpha, Color.Blue));
        public static SolidBrush G5OragneNoteBrush = new SolidBrush(Color.FromArgb(defaultAlpha, Color.Orange));


        public static Pen beatPen = new Pen(Color.FromArgb(defaultAlpha, 200, 200, 200));
        public static Pen barPen = new Pen(Color.FromArgb(defaultAlpha, 120, 120, 120));
        public static Pen linePen = new Pen(Color.FromArgb(255, Color.Black));
        public static Pen linePen22 = new Pen(Color.FromArgb(255, Color.Black));
        public static Pen selectedPen = new Pen(Color.FromArgb(defaultAlpha, Color.Red));

        public static Pen slidePen = new Pen(Color.FromArgb(defaultAlpha, Color.Green), 2.0f);
        public static Pen hammerOnPen = new Pen(Color.FromArgb(defaultAlpha, Color.Blue), 2.0f);
        public static Pen noteBoundPen = new Pen(Color.FromArgb(defaultAlpha, Color.Black));

        public static SolidBrush gridSnapCursorBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 0, 0, 0));


        public static Pen rectSelectionPen = new Pen(Color.FromArgb(defaultAlpha, Color.Red), 2.0f);
        public static SolidBrush noteBGBrushShadow = new SolidBrush(Color.FromArgb(defaultAlpha, 100, 100, 100));
        public static SolidBrush noteBGBrushSel = new SolidBrush(Color.FromArgb(defaultAlpha, 255, 0, 0));
        public static SolidBrush noteSingleStringTremeloBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 120, 240, 100));
        public static SolidBrush noteMultiStringTremeloBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 140, 140, 140));

        public static SolidBrush noteBGBrush = new SolidBrush(Color.FromArgb(252, 183, 180));
        public static SolidBrush noteTapBrush = new SolidBrush(Color.FromArgb(242, 223, 220));
        public static SolidBrush noteXBrush = new SolidBrush(Color.FromArgb(152, 152, 247));
        public static SolidBrush noteArpeggioBrush = new SolidBrush(Color.FromArgb(200, 159, 253, 222));
        public static SolidBrush notePowerupBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 242, 253, 200));
        public static SolidBrush noteSoloBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 202, 203, 250));
        public static SolidBrush noteBREBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 152, 203, 250));
        public static SolidBrush noteStrumBrush = new SolidBrush(Color.FromArgb(defaultAlpha, 180, 180, 255));

        public static SolidBrush TrainerBrush = new SolidBrush(Color.FromArgb(80, 150, 100, 200));
        public static SolidBrush SelectedTrainerBrush = new SolidBrush(Color.FromArgb(120, 250, 100, 100));
        public static SolidBrush TextEventBrush = new SolidBrush(Color.FromArgb(80, 100, 150, 200));
        public static SolidBrush SelectedTextEventBrush = new SolidBrush(Color.FromArgb(120, 200, 0, 150));


        public static int[] ChordNameEvents = new int[] 
                { 
                    ChordNameHiddenData1,
                    ChordNameSlashChordData1,
                    ChordNameEbData1,
                    ChordNameDData1,
                    ChordNameDbData1,
                    ChordNameCData1,
                    ChordNameBData1,
                    ChordNameBbData1,
                    ChordNameAData1,
                    ChordNameAbData1,
                    ChordNameGData1,
                    ChordNameGbData1,
                    ChordNameFData1,
                    ChordNameEData1
                };

        public static IEnumerable<GuitarDifficulty> GetDifficultyIter(bool sortExpertToEasy = true)
        {
            var ret = new List<GuitarDifficulty>();
            ret.Add(GuitarDifficulty.Easy);
            ret.Add(GuitarDifficulty.Medium);
            ret.Add(GuitarDifficulty.Hard);
            ret.Add(GuitarDifficulty.Expert);
            if (sortExpertToEasy)
                ret.Reverse();
            return ret.ToList();
        }

        public static GuitarDifficulty GetDifficulty(int data1, bool isPro)
        {
            if (isPro)
            {
                if (ExpertData1Strings.Contains(data1))
                    return GuitarDifficulty.Expert;
                else if (HardData1Strings.Contains(data1))
                    return GuitarDifficulty.Hard;
                else if (MediumData1Strings.Contains(data1))
                    return GuitarDifficulty.Medium;
                else if (EasyData1Strings.Contains(data1))
                    return GuitarDifficulty.Easy;

                else if (data1 == HandPositionData1)
                    return GuitarDifficulty.All;

                else if (data1 == ExpertArpeggioData1)
                    return GuitarDifficulty.Expert;

                else if (data1 == ExpertSlideData1)
                    return GuitarDifficulty.Expert;

                else if (data1 == ExpertHammeronData1)
                    return GuitarDifficulty.Expert;

                else if (data1 == ExpertStrumData1)
                    return GuitarDifficulty.Expert;

                else if (data1 == HardStrumData1)
                    return GuitarDifficulty.Hard;

                else if (data1 == HardSlideData1)
                    return GuitarDifficulty.Hard;

                else if (data1 == HardArpeggioData1)
                    return GuitarDifficulty.Hard;

                else if (data1 == HardHammeronData1)
                    return GuitarDifficulty.Hard;

                else if (data1 == MediumHammeronData1)
                    return GuitarDifficulty.Medium;

                else if (data1 == EasyHammeronData1)
                    return GuitarDifficulty.Easy;

                else if (data1 == MediumSlideData1)
                    return GuitarDifficulty.Medium;

                else if (data1 == EasySlideData1)
                    return GuitarDifficulty.Easy;

                else if (data1 == PowerupData1)
                    return GuitarDifficulty.All;

                else if (data1 == SoloData1)
                    return GuitarDifficulty.All;

                else if (data1 == SingleStringTremeloData1)
                    return GuitarDifficulty.All;

                else if (data1 == MultiStringTremeloData1)
                    return GuitarDifficulty.All;
                else if (BigRockEndingData1.Contains(data1))
                    return GuitarDifficulty.All;
                else if (ChordNameEvents.Contains(data1))
                    return GuitarDifficulty.All;
            }
            else
            {
                if (ExpertData1StringsG5.Contains(data1))
                    return GuitarDifficulty.Expert;

                if (HardData1StringsG5.Contains(data1))
                    return GuitarDifficulty.Hard;

                if (MediumData1StringsG5.Contains(data1))
                    return GuitarDifficulty.Medium;

                if (EasyData1StringsG5.Contains(data1))
                    return GuitarDifficulty.Easy;

                if (data1 == ExpertSoloData1_G5 || data1 == SoloData1)
                    return GuitarDifficulty.All;

                if (data1 == PowerupData1)
                    return GuitarDifficulty.All;

                if (BigRockEndingData1.Contains(data1))
                    return GuitarDifficulty.All;
            }

            return GuitarDifficulty.Unknown;
        }

        public static int[] GetKnownData1ForDifficulty(bool isPro, GuitarDifficulty diff)
        {
            List<int> ret = new List<int>();
            for (int x = 0; x < 128; x++)
            {
                var d = x.GetData1Difficulty(isPro);
                if (d.IsAll() && diff.IsAll())
                {
                    ret.Add(x);
                }
                else if (d.IsExpert() && diff.IsExpert())
                {
                    ret.Add(x);
                }
                else if (d.IsHard() && diff.IsHard())
                {
                    ret.Add(x);
                }
                else if (d.IsMedium() && diff.IsMedium())
                {
                    ret.Add(x);
                }
                else if (d.IsEasy() && diff.IsEasy())
                {
                    ret.Add(x);
                }
            }
            return ret.ToArray();
        }

        public static int GetSoloData1_G5(GuitarDifficulty difficulty)
        {
            if (difficulty.IsUnknown() || difficulty.IsAll() || difficulty.IsExpert())
                return ExpertSoloData1_G5;
            if (difficulty.IsHard())
                return HardSoloData1_G5;
            else if (difficulty.IsMedium())
                return MediumSoloData1_G5;
            else if (difficulty.IsEasy())
                return EasySoloData1_G5;
            return ExpertSoloData1_G5;
        }

        public static int GetModifierData1ForDifficulty(
            int sourceData1,
            GuitarDifficulty sourceDifficulty,
            GuitarDifficulty targetDifficulty)
        {
            if (targetDifficulty.IsUnknown())
                targetDifficulty = sourceDifficulty;

            if (AllHammeronData1.Contains(sourceData1))
            {
                if (targetDifficulty == GuitarDifficulty.Expert)
                {
                    return ExpertHammeronData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Hard)
                {
                    return HardHammeronData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Medium)
                {
                    return MediumHammeronData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Easy)
                {
                    return EasyHammeronData1;
                }
                else
                {
                    return Int32.MinValue;
                }
            }
            else if (AllSlideData1.Contains(sourceData1))
            {
                if (targetDifficulty == GuitarDifficulty.Expert)
                {
                    return ExpertSlideData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Hard)
                {
                    return HardSlideData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Medium)
                {
                    return MediumSlideData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Easy)
                {
                    return EasySlideData1;
                }
                else
                {
                    return Int32.MinValue;
                }
            }
            else if (AllArpeggioData1.Contains(sourceData1))
            {
                if (targetDifficulty == GuitarDifficulty.Expert)
                {
                    return ExpertArpeggioData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Hard)
                {
                    return HardArpeggioData1;
                }
                else
                {
                    return Int32.MinValue;
                }
            }
            else if (AllStrumData1.Contains(sourceData1))
            {
                if (targetDifficulty == GuitarDifficulty.Expert)
                {
                    return ExpertStrumData1;
                }
                else if (targetDifficulty == GuitarDifficulty.Hard)
                {
                    return HardStrumData1;
                }
                else
                {
                    return Int32.MinValue;
                }
            }

            else
            {
                return Int32.MinValue;
            }
        }

        public static string TextEventBeginTag = "[";
        public static string TextEventEndTag = "]";
        public static string SongTrainerPGText = "song_trainer_pg_";
        public static string SongTrainerPBText = "song_trainer_pb_";
        public static string SongTrainerBeginPGText = "begin_pg";
        public static string SongTrainerBeginPBText = "begin_pb";
        public static string SongTrainerEndPGText = "end_pg";
        public static string SongTrainerEndPBText = "end_pb";
        public static string SongTrainerNormPGText = "pg_norm";
        public static string SongTrainerNormPBText = "pb_norm";
        public static double SongTrainerNormOffset = 0.25;

        public static bool IsData1String6(int data1)
        {
            return GetStringDifficulty6(data1) != GuitarDifficulty.Unknown;
        }

        public static bool IsData1String5(int data1)
        {
            return GetStringDifficulty5(data1) != GuitarDifficulty.Unknown;
        }

        public static IEnumerable<int> GetStringsForDifficulty6(GuitarDifficulty diff)
        {
            var ret = new List<int>();
            if (diff.HasFlag(GuitarDifficulty.Easy))
            {
                ret.AddRange(EasyData1Strings);
            }
            if (diff.HasFlag(GuitarDifficulty.Medium))
            {
                ret.AddRange(MediumData1Strings);
            }
            if (diff.HasFlag(GuitarDifficulty.Hard))
            {
                ret.AddRange(HardData1Strings);
            }
            if (diff.HasFlag(GuitarDifficulty.Expert))
            {
                ret.AddRange(ExpertData1Strings);
            }
            return ret;
        }

        public static IEnumerable<int> GetStringsForDifficulty5(GuitarDifficulty diff)
        {
            var ret = new List<int>();
            if (diff.HasFlag(GuitarDifficulty.Easy))
            {
                ret.AddRange(EasyData1StringsG5);
            }
            if (diff.HasFlag(GuitarDifficulty.Medium))
            {
                ret.AddRange(MediumData1StringsG5);
            }
            if (diff.HasFlag(GuitarDifficulty.Hard))
            {
                ret.AddRange(HardData1StringsG5);
            }
            if (diff.HasFlag(GuitarDifficulty.Expert))
            {
                ret.AddRange(ExpertData1StringsG5);
            }
            return ret;
        }

        public static int[] ExpertData1Strings =  { ExpertData1LowE,
            ExpertData1A, ExpertData1D, ExpertData1G, ExpertData1B, ExpertData1HighE};

        public static int[] ExpertData1StringsG5 =  { ExpertData1LowE,
            ExpertData1A, ExpertData1D, ExpertData1G, ExpertData1B};

        public static int[] HardData1Strings =  { HardData1LowE,
            HardData1A, HardData1D, HardData1G, HardData1B, HardData1HighE};

        public static int[] HardData1StringsG5 =  { HardData1LowE5,
            HardData1A5, HardData1D5, HardData1G5, HardData1B5};

        public static int[] MediumData1Strings =  { MediumData1LowE,
            MediumData1A, MediumData1D, MediumData1G, MediumData1B, MediumData1HighE};

        public static int[] MediumData1StringsG5 =  { MediumData1LowE5,
            MediumData1A5, MediumData1D5, MediumData1G5};

        public static int[] EasyData1Strings =  { EasyData1LowE,
            EasyData1A, EasyData1D, EasyData1G, EasyData1B, EasyData1HighE};

        public static int[] EasyData1StringsG5 =  { EasyData1LowE5,
            EasyData1A5, EasyData1D5};

        public static int[] AllData1Strings6 = 
        { 
            ExpertData1LowE, ExpertData1A, ExpertData1D, ExpertData1G, ExpertData1B, ExpertData1HighE, 
            HardData1LowE, HardData1A, HardData1D, HardData1G, HardData1B, HardData1HighE,
            MediumData1LowE, MediumData1A, MediumData1D, MediumData1G, MediumData1B, MediumData1HighE,
            EasyData1LowE, EasyData1A, EasyData1D, EasyData1G, EasyData1B, EasyData1HighE,
        };

        public static int[] AllData1Strings5 = 
        { 
            ExpertData1LowE, ExpertData1A, ExpertData1D, ExpertData1G, ExpertData1B, 
            HardData1LowE5, HardData1A5, HardData1D5, HardData1G5, HardData1B5,
            MediumData1LowE5, MediumData1A5, MediumData1D5, MediumData1G5,
            EasyData1LowE5, EasyData1A5, EasyData1D5,
        };

        public static int GetNoteString(int data1, bool isPro)
        {
            return isPro ? GetNoteString(data1) : GetNoteString5(data1);
        }

        public static int GetNoteString(int data1)
        {
            int ret = Int32.MinValue;

            if (ExpertData1Strings.Contains(data1))
            {
                ret = data1 - ExpertData1LowE;
            }
            else if (HardData1Strings.Contains(data1))
            {
                ret = data1 - HardData1LowE;
            }
            else if (MediumData1Strings.Contains(data1))
            {
                ret = data1 - MediumData1LowE;
            }
            else if (EasyData1Strings.Contains(data1))
            {
                ret = data1 - EasyData1LowE;
            }
            return ret;
        }

        public static int GetNoteData1(int stringOffset, GuitarDifficulty difficulty, bool isPro)
        {
            int ret = Int32.MinValue;

            if (stringOffset >= 0)
            {
                if (isPro)
                {
                    if (difficulty == GuitarDifficulty.Expert && stringOffset < ExpertData1Strings.Length)
                    {
                        ret = ExpertData1Strings[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Hard && stringOffset < HardData1Strings.Length)
                    {
                        ret = HardData1Strings[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Medium && stringOffset < MediumData1Strings.Length)
                    {
                        ret = MediumData1Strings[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Easy && stringOffset < EasyData1Strings.Length)
                    {
                        ret = EasyData1Strings[stringOffset];
                    }
                }
                else
                {
                    if (difficulty == GuitarDifficulty.Expert && stringOffset < ExpertData1StringsG5.Length)
                    {
                        ret = ExpertData1StringsG5[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Hard && stringOffset < HardData1StringsG5.Length)
                    {
                        ret = HardData1StringsG5[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Medium && stringOffset < MediumData1StringsG5.Length)
                    {
                        ret = MediumData1StringsG5[stringOffset];
                    }
                    else if (difficulty == GuitarDifficulty.Easy && stringOffset < EasyData1StringsG5.Length)
                    {
                        ret = EasyData1StringsG5[stringOffset];
                    }
                }
            }
            return ret;
        }
        public static int GetNoteString5(int data1)
        {
            int ret = Int32.MinValue;
            if (ExpertData1StringsG5.Contains(data1))
            {
                ret = data1 - ExpertData1LowE;
            }
            else if (HardData1StringsG5.Contains(data1))
            {
                ret = data1 - HardData1LowE5;
            }
            else if (MediumData1StringsG5.Contains(data1))
            {
                ret = data1 - MediumData1LowE5;
            }
            else if (EasyData1StringsG5.Contains(data1))
            {
                ret = data1 - EasyData1LowE5;
            }

            return ret;
        }

        public static int GetStringLowE5(GuitarDifficulty difficulty)
        {
            if (difficulty == GuitarDifficulty.Easy)
                return EasyData1LowE5;
            if (difficulty == GuitarDifficulty.Medium)
                return MediumData1LowE5;
            if (difficulty == GuitarDifficulty.Hard)
                return HardData1LowE5;
            return ExpertData1LowE;
        }

        public static GuitarDifficulty GetStringDifficulty5(int data1)
        {
            if (EasyData1StringsG5.Contains(data1))
                return GuitarDifficulty.Easy;
            if (MediumData1StringsG5.Contains(data1))
                return GuitarDifficulty.Medium;
            if (HardData1StringsG5.Contains(data1))
                return GuitarDifficulty.Hard;
            if (ExpertData1StringsG5.Contains(data1))
                return GuitarDifficulty.Expert;
            return GuitarDifficulty.Unknown;
        }

        public static GuitarDifficulty GetStringDifficulty6(int data1)
        {
            if (EasyData1Strings.Contains(data1))
                return GuitarDifficulty.Easy;
            if (MediumData1Strings.Contains(data1))
                return GuitarDifficulty.Medium;
            if (HardData1Strings.Contains(data1))
                return GuitarDifficulty.Hard;
            if (ExpertData1Strings.Contains(data1))
                return GuitarDifficulty.Expert;
            return GuitarDifficulty.Unknown;
        }

        public static GuitarDifficulty GetStringDifficulty(int data1, bool isPro)
        {
            return isPro ? GetStringDifficulty6(data1) : GetStringDifficulty5(data1);
        }

        public static int GetStringLowE(GuitarDifficulty difficulty, bool isPro)
        {
            return isPro ? GetStringLowE(difficulty) : GetStringLowE5(difficulty);
        }

        public static int GetStringLowE(GuitarDifficulty difficulty)
        {
            if (difficulty == GuitarDifficulty.Easy)
                return EasyData1LowE;
            if (difficulty == GuitarDifficulty.Medium)
                return MediumData1LowE;
            if (difficulty == GuitarDifficulty.Hard)
                return HardData1LowE;
            if (difficulty == GuitarDifficulty.Expert)
                return ExpertData1LowE;
            return Int32.MinValue;
        }

        public static int GetChannelFromStrum(ChordStrum cs)
        {
            int ret = 0;

            switch (cs)
            {
                case ChordStrum.High:
                    ret = ChannelStrumHigh;
                    break;
                case ChordStrum.Mid:
                    ret = ChannelStrumMid;
                    break;
                case ChordStrum.Low:
                    ret = ChannelStrumLow;
                    break;
            }
            return ret;
        }




        public static SolidBrush G5Brush(int noteString)
        {
            SolidBrush nb = null;
            if (noteString == 0)
                nb = Utility.G5GreenNoteBrush;
            if (noteString == 1)
                nb = Utility.G5RedNoteBrush;
            if (noteString == 2)
                nb = Utility.G5YellowNoteBrush;
            if (noteString == 3)
                nb = Utility.G5BlueNoteBrush;
            if (noteString == 4)
                nb = Utility.G5OragneNoteBrush;
            return nb;
        }


        public static double timeScalar = 300.0;
        public static double timeScalarZoomSpeed = 20;
        public static double timeScalarMax = 50000.0;
        public static double timeScalarMin = 0.10;

        public static double ScaleUp(double value)
        {
            return value * timeScalar;
        }

        public static double ScaleDown(double value)
        {
            return value / timeScalar;
        }

        public static double ScaleDown(int value)
        {
            return ((double)value) / timeScalar;
        }


        public static bool ShowMeasureNumbers = false;
        public static bool ShowTempos = false;
        public static bool ShowTimeSignatures = false;
        public static bool ModifyWebTabScale = false;



        public static ToneNameData1 GetNoteScaleData1(int noteString, int noteFretDown)
        {
            return (ToneNameData1)(((int)ToneNameData1.E) + GetNoteScale(noteString, noteFretDown));
        }

        public static ToneNameEnum GetNoteScale(int noteString, int noteFretDown, int[] tuning = null)
        {
            if (noteFretDown.IsNull() || noteString.IsNull())
            {
                return ToneNameEnum.NotSet;
            }

            if (tuning != null)
            {
                noteFretDown += tuning[noteString];
            }

            int chordScaleRange = ToneNameEnum.NumTones.ToInt();

            while (noteString > 0)
            {
                noteString--;
                if (noteString == 3)
                {
                    noteFretDown += 4;
                }
                else
                {
                    noteFretDown += 5;
                }

                if (tuning != null)
                {
                    noteFretDown += tuning[noteString];
                }
            }

            while (noteFretDown < 0)
            {
                noteFretDown += 12;
            }

            return (ToneNameEnum)(noteFretDown % (int)ToneNameEnum.NumTones);
        }
    }


}
