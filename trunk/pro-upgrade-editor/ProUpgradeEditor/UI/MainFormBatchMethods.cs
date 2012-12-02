using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.DataLayer;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ProUpgradeEditor.UI
{

    partial class MainForm
    {


        void GetCopyAllProFolder()
        {
            var sel = ShowSelectFolderDlg("Select Pro Output Path", "", "");
            if (!string.IsNullOrEmpty(sel))
            {
                textBoxCopyAllProFolder.Text = sel;
            }
        }



        private void CopyAllConToLocation()
        {
            ClearBatchResults();
            if (string.IsNullOrEmpty(textBoxCopyAllCONFolder.Text) ||
                !Directory.Exists(textBoxCopyAllCONFolder.Text))
            {
                GetPathCopyAllCON();
            }

            var destFolder = textBoxCopyAllCONFolder.Text;
            if (string.IsNullOrEmpty(destFolder))
                return;

            progressBarGenCompletedDifficulty.Value = 0;
            progressBarGenCompletedDifficulty.Maximum = SongList.Count;

            buttonSongLibCancel.Enabled = true;

            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();


            foreach (SongCacheItem item in songs)
            {


                Application.DoEvents();
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    break;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);


                    if (!item.IsComplete)
                    {
                        WriteBatchResult("Skipping: " + item.ToString());
                    }
                    else
                    {
                        WriteBatchResult("Processing: " + item.ToString());
                        if (!string.IsNullOrEmpty(item.G6ConFile))
                        {
                            if (File.Exists(item.G6ConFile))
                            {
                                if (Package.Load(item.G6ConFile) != null)
                                {
                                    var destFile = Path.Combine(destFolder, Path.GetFileName(item.G6ConFile));

                                    if (!TryCopyFile(item.G6ConFile, destFile))
                                    {
                                        WriteBatchResult("Unable to overwrite: " + item.ToString());
                                    }
                                }

                            }
                        }
                    }
                }
                catch
                {
                    WriteBatchResult("Failed Copying: " + item.ToString());
                }
            }
            buttonSongLibCancel.Enabled = false;

            WriteBatchResult("Complete");
        }


        private void GetPathCopyAllCON()
        {
            var sel = ShowSelectFolderDlg("Select CON Output Path", "", "");
            if (!string.IsNullOrEmpty(sel))
            {
                textBoxCopyAllCONFolder.Text = sel;
            }
        }



        private bool ExecuteBatchGuitarBassCopy()
        {
            buttonSongLibCancel.Enabled = true;

            progressBarGenCompletedDifficulty.Value = 0;


            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();

            foreach (SongCacheItem item in songs)
            {

                WriteBatchResult("Copying Guitar To Bass: " + item.ToString());
                Application.DoEvents();
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    return false;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);

                    if ((checkBoxBatchProcessIncomplete.Checked == false && !item.IsComplete))
                    {
                        WriteBatchResult("Skipping (Not Completed): " + item.ToString());
                    }
                    else if (checkBoxBatchProcessFinalized.Checked == false && item.IsFinalized)
                    {
                        WriteBatchResult("Skipping (Finalized): " + item.ToString());
                    }
                    else if (item.CopyGuitarToBass == false)
                    {
                        WriteBatchResult("Skipping (Song set to Not Copy Guitar to Bass): " + item.ToString());
                    }
                    else
                    {
                        WriteBatchResult("Processing: " + item.ToString());

                        if (!OpenSongCacheItem(item))
                        {
                            WriteBatchResult("Unable to open: " + item.ToString());
                            continue;
                        }

                        if (!CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17))
                        {
                            WriteBatchResult("Unable to copy REAL_GUITAR: " + item.ToString());
                            continue;
                        }
                        if (!CopyTrack(GuitarTrack.GuitarTrackName22, GuitarTrack.BassTrackName22))
                        {
                            WriteBatchResult("Unable to copy REAL_GUITAR_22: " + item.ToString());
                            continue;
                        }

                        if (checkBoxSetBassToGuitarDifficulty.Checked)
                        {
                            item.HasBass = true;
                            item.DTABassDifficulty = item.DTAGuitarDifficulty;

                            UpdateCONFileProperties(item);
                        }

                        if (!SaveSongCacheItem(item, true))
                        {
                            WriteBatchResult("Unable to save: " + item.ToString());
                            continue;
                        }
                    }
                }
                catch
                {
                    WriteBatchResult("Failed Copying Track: " + item.ToString());
                }
            }
            buttonSongLibCancel.Enabled = false;
            WriteBatchResult("Batch Guitar Bass Copy Complete");
            return true;
        }

        private void CopyAllProToLocation()
        {
            ClearBatchResults();
            if (string.IsNullOrEmpty(textBoxCopyAllProFolder.Text) ||
                !Directory.Exists(textBoxCopyAllProFolder.Text))
            {
                GetCopyAllProFolder();
            }

            var destFolder = textBoxCopyAllProFolder.Text;
            if (string.IsNullOrEmpty(destFolder))
                return;

            progressBarGenCompletedDifficulty.Value = 0;

            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();


            buttonSongLibCancel.Enabled = true;
            foreach (SongCacheItem item in songs)
            {


                Application.DoEvents();
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    break;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);


                    if (!item.IsComplete)
                    {
                        WriteBatchResult("Skipping: " + item.ToString());
                    }
                    else
                    {
                        WriteBatchResult("Processing: " + item.ToString());
                        if (!string.IsNullOrEmpty(item.G6FileName))
                        {
                            if (File.Exists(item.G6FileName))
                            {
                                //if (Package.Load(item.G6FileName) != null)
                                {
                                    var destFile = Path.Combine(destFolder, Path.GetFileName(item.G6FileName));

                                    if (!TryCopyFile(item.G6FileName, destFile))
                                    {
                                        WriteBatchResult("Unable to overwrite: " + item.ToString());
                                    }
                                }

                            }
                        }
                    }
                }
                catch
                {
                    WriteBatchResult("Failed Copying: " + item.ToString());
                }
            }
            buttonSongLibCancel.Enabled = false;

            WriteBatchResult("Complete");
        }



        void GetCopyAllG5Folder()
        {
            var sel = ShowSelectFolderDlg("Select Midi 5 Output Path", "", "");
            if (!string.IsNullOrEmpty(sel))
            {
                textBoxCopyAllG5MidiFolder.Text = sel;
            }
        }



        private void CopyAllG5MidiToLocation()
        {

            ClearBatchResults();
            if (string.IsNullOrEmpty(textBoxCopyAllG5MidiFolder.Text) ||
                !Directory.Exists(textBoxCopyAllG5MidiFolder.Text))
            {
                GetCopyAllG5Folder();
            }

            var destFolder = textBoxCopyAllG5MidiFolder.Text;
            if (string.IsNullOrEmpty(destFolder))
                return;

            progressBarGenCompletedDifficulty.Value = 0;
            progressBarGenCompletedDifficulty.Maximum = SongList.Count;

            buttonSongLibCancel.Enabled = true;

            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();

            foreach (SongCacheItem item in songs)
            {

                Application.DoEvents();
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    break;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);

                    if (!item.IsComplete)
                    {
                        WriteBatchResult("Skipping: " + item.ToString());
                    }
                    else
                    {
                        WriteBatchResult("Processing: " + item.ToString());
                        if (!string.IsNullOrEmpty(item.G5FileName))
                        {
                            if (File.Exists(item.G5FileName))
                            {
                                var destFile = Path.Combine(destFolder, Path.GetFileName(item.G5FileName));

                                if (!TryCopyFile(item.G5FileName, destFile))
                                {
                                    WriteBatchResult("Unable to overwrite: " + item.ToString());
                                }
                            }
                        }
                    }
                }
                catch
                {
                    WriteBatchResult("Failed Copying: " + item.ToString());
                }
            }
            buttonSongLibCancel.Enabled = false;

            WriteBatchResult("Complete");
        }

        private bool GenerateDifficulties(bool isBatch, GenDiffConfig config)
        {
            var ret = false;

            if (!EditorG5.IsLoaded || !EditorPro.IsLoaded)
                return false;
            
            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {
                    var g5Track = EditorG5.GuitarTrack;
                    var proTrack = EditorPro.GuitarTrack;

                    EditorPro.BackupSequence();

                    bool genGuitar = false;
                    bool genBass = false;

                    bool selectedTrackGuitar = false;
                    bool selectedTrackBass = false;

                    if (config.SelectedTrackOnly)
                    {
                        if (GuitarTrack.GuitarTrackNames6.Contains(proTrack.Name))
                        {
                            genGuitar = true;
                            selectedTrackGuitar = true;
                        }
                        else if (GuitarTrack.BassTrackNames6.Contains(proTrack.Name))
                        {
                            genBass = true;
                            selectedTrackBass = true;
                        }
                    }
                    else
                    {
                        genGuitar = true;
                        genBass = true;
                    }


                    var guitarDifficulties = GuitarDifficulty.None;
                    var bassDifficulties = GuitarDifficulty.None;

                    if (config.SelectedDifficultyOnly)
                    {
                        guitarDifficulties = EditorPro.CurrentDifficulty;
                        bassDifficulties = EditorPro.CurrentDifficulty;
                    }
                    else
                    {
                        if (config.ProcessingSong)
                        {
                            if (genGuitar)
                            {
                                if (config.EnableProGuitarHard)
                                    guitarDifficulties |= GuitarDifficulty.Hard;
                                if (config.EnableProGuitarMedium)
                                    guitarDifficulties |= GuitarDifficulty.Medium;
                                if (config.EnableProGuitarEasy)
                                    guitarDifficulties |= GuitarDifficulty.Easy;
                            }
                            if (genBass)
                            {
                                if (config.EnableProBassHard)
                                    bassDifficulties |= GuitarDifficulty.Hard;
                                if (config.EnableProBassMedium)
                                    bassDifficulties |= GuitarDifficulty.Medium;
                                if (config.EnableProBassEasy)
                                    bassDifficulties |= GuitarDifficulty.Easy;
                            }
                        }
                        else
                        {
                            if (genGuitar)
                            {
                                guitarDifficulties = GuitarDifficulty.Hard | GuitarDifficulty.Medium | GuitarDifficulty.Easy;
                            }
                            if (genBass)
                            {
                                bassDifficulties = GuitarDifficulty.Hard | GuitarDifficulty.Medium | GuitarDifficulty.Easy;
                            }
                        }
                    }

                    if (config.SelectedTrackOnly)
                    {
                        if (selectedTrackGuitar)
                        {
                            if (!guitarDifficulties.IsUnknown())
                            {
                                GenerateDifficultiesForTrack(g5Track.GetTrack(), proTrack.GetTrack(), guitarDifficulties, config);
                            }
                        }
                        else if (selectedTrackBass)
                        {
                            if (!bassDifficulties.IsUnknown())
                            {
                                GenerateDifficultiesForTrack(g5Track.GetTrack(), proTrack.GetTrack(), bassDifficulties, config);
                            }
                        }
                    }
                    else
                    {
                        if (genGuitar)
                        {
                            var gt5 = EditorG5.GetGuitar5MidiTrack();
                            var gt6 = EditorPro.GetTrackGuitar17();

                            if (gt5 != null && gt6 != null)
                            {
                                GenerateDifficultiesForTrack(gt5, gt6, guitarDifficulties, config);
                            }

                            gt6 = EditorPro.GetTrackGuitar22();
                            if (gt5 != null && gt6 != null)
                            {
                                GenerateDifficultiesForTrack(gt5, gt6, guitarDifficulties, config);
                            }
                        }

                        if (genBass)
                        {
                            var gt5 = EditorG5.GetGuitar5BassMidiTrack();
                            var gt6 = EditorPro.GetTrackBass17();


                            if (config.CopyGuitarToBass)
                            {
                                if (EditorPro.GetTrackGuitar17() != null)
                                {
                                    CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17);
                                }
                            }
                            else
                            {
                                if (gt5 != null && gt6 != null)
                                {
                                    GenerateDifficultiesForTrack(gt5, gt6, bassDifficulties, config);
                                }
                            }


                            if (config.CopyGuitarToBass)
                            {
                                if (EditorPro.GetTrackGuitar22() != null)
                                {
                                    CopyTrack(GuitarTrack.GuitarTrackName22, GuitarTrack.BassTrackName22);
                                }
                            }
                            else
                            {
                                gt6 = EditorPro.GetTrackBass22();
                                if (gt5 != null && gt6 != null)
                                {
                                    GenerateDifficultiesForTrack(gt5, gt6, bassDifficulties, config);
                                }
                            }
                        }
                    }

                    if (config.Generate108Events)
                    {
                        Set108Events(config);
                    }
                    ReloadTracks();
                }
                catch { ret = false; }

                ret = true;
            });
            return ret;
        }

        private void GenerateDifficultiesForTrack(Track originalG5Track, Track originalProTrack, GuitarDifficulty guitarDifficulties, GenDiffConfig config)
        {
            if (guitarDifficulties.IsExpert())
            {
                guitarDifficulties ^= GuitarDifficulty.Expert;
            }

            if (guitarDifficulties.IsUnknown())
                return;

            originalProTrack.RemoveDifficulty(guitarDifficulties);
            
            if (guitarDifficulties.IsHard())
            {
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Hard, config);
            }

            if (guitarDifficulties.IsMedium())
            {
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Medium, config);
            }

            if (guitarDifficulties.IsEasy())
            {
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Easy, config);
            }

        }

        private void GenDiffNotes(Track tG5, Track tG6, GuitarDifficulty diff, GenDiffConfig config)
        {
            var sourceDifficulty = 
                diff == GuitarDifficulty.Easy ? GuitarDifficulty.Medium : 
                diff == GuitarDifficulty.Medium ? GuitarDifficulty.Hard :
                GuitarDifficulty.Expert;


            EditorG5.SetTrack5(tG5, sourceDifficulty);
            EditorPro.SetTrack6(tG6, sourceDifficulty);

            var sourceArpeggio6 = EditorPro.GuitarTrack.Messages.Arpeggios.ToList();
            var sourceChords6 = EditorPro.GuitarTrack.Messages.Chords.ToList();

            EditorPro.SetTrack6(tG6, diff);
            EditorG5.SetTrack5(tG5, diff);

            var g5c = EditorG5.GuitarTrack.Messages.Chords;

            var g6e = Utility.GetStringLowE(diff);
            var g6ex = Utility.GetStringLowE(sourceDifficulty);


            int lastUpTick = int.MinValue;
            int currUpTick = int.MinValue;


            foreach (var sourceChord6 in sourceChords6)
            {
                var g5notes = g5c.GetBetweenTick(sourceChord6.DownTick, sourceChord6.UpTick);
                if (g5notes.Any())
                {
                    int currDownTick = sourceChord6.DownTick;
                    currUpTick = sourceChord6.UpTick;
                    if (diff == GuitarDifficulty.Easy || diff == GuitarDifficulty.Medium)
                    {
                        currDownTick = g5notes.GetMinTick();
                        currUpTick = g5notes.GetMaxTick();
                    }


                    if (sourceChord6.DownTick >= lastUpTick)
                    {
                        lastUpTick = currUpTick;
                        int notesCreated = 0;
                        var frets = new int[6] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue };
                        var channels = new int[6] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue };

                        foreach(var n in sourceChord6.Notes)
                        {
                            if ((n.IsArpeggioNote && diff == GuitarDifficulty.Hard) || n.IsArpeggioNote == false)
                            {
                                frets[n.NoteString] = n.NoteFretDown;
                                channels[n.NoteString] = n.Channel;

                                notesCreated++;

                                if (diff == GuitarDifficulty.Easy)
                                    break;
                                if (diff == GuitarDifficulty.Medium && notesCreated > 1)
                                    break;
                            }
                        }
                        if (notesCreated > 0)
                        {
                            ProGuitarTrack.CreateChord(frets, channels, diff, currDownTick, currUpTick, 
                                Utility.GetSlideData1(diff) != -1 ? sourceChord6.IsSlide : false,
                                Utility.GetSlideData1(diff) != -1 ? sourceChord6.IsSlideReversed : false,
                                Utility.GetHammeronData1(diff) != -1 ? sourceChord6.IsHammeron : false,
                                Utility.GetStrumData1(diff) != -1 ? sourceChord6.StrumMode : ChordStrum.Normal);
                        }

                    }
                }
            }
            
            if (diff == GuitarDifficulty.Hard)
            {
                foreach (var arp in sourceArpeggio6)
                {
                    var bet = EditorPro.GuitarTrack.Messages.Chords.GetBetweenTick(arp.DownTick, arp.UpTick);

                    if (bet.Count() > 1)
                    {
                        GuitarArpeggio.CreateArpeggio(ProGuitarTrack, diff, bet.GetMinTick(), bet.GetMaxTick());
                    }
                }
            }
        }


        private static GuitarMessage GetNextOffNote(List<GuitarMessage> g6HardNotes, GuitarMessage g5Down)
        {
            var g6hn = g6HardNotes.FirstOrDefault(o =>
                o.Command == ChannelCommand.NoteOff &&
                o.AbsoluteTicks > g5Down.DownTick);
            return g6hn;
        }

        private static IEnumerable<GuitarMessage> GetOffNotes(List<GuitarMessage> g6HardNotes, GuitarMessage g6Up)
        {
            var g6hn = g6HardNotes.Where(o => o.Command == ChannelCommand.NoteOff &&
                o.AbsoluteTicks == g6Up.AbsoluteTicks);
            return g6hn;
        }

        public void RefreshTracks()
        {
            RefreshTracks5();
            RefreshTracks6();
            
        }

        private bool ExecuteBatchRebuildCON()
        {

            try
            {
                
                buttonSongLibCancel.Enabled = true;
                progressBarGenCompletedDifficulty.Value = 0;


                var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

                progressBarGenCompletedDifficulty.Maximum = songs.Count();

                foreach (SongCacheItem item in songs)
                {
                    Application.DoEvents();

                    WriteBatchResult("Rebuilding CON Package: " + item.ToString());
                    if (buttonSongLibCancel.Enabled == false)
                    {
                        WriteBatchResult("User Cancelled");
                        return false;
                    }
                    try
                    {
                        progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);


                        if ((checkBoxBatchProcessIncomplete.Checked == false && !item.IsComplete))
                        {
                            WriteBatchResult("Skipping (Not Completed): " + item.ToString());
                        }
                        else if (checkBoxBatchProcessFinalized.Checked == false && item.IsFinalized)
                        {
                            WriteBatchResult("Skipping (Finalized): " + item.ToString());
                        }
                        else
                        {
                            WriteBatchResult("Processing: " + item.ToString());

                            if (!OpenSongCacheItem(item))
                            {
                                WriteBatchResult("Unable to open: " + item.ToString());
                                continue;
                            }


                            if (!Set108Events(new GenDiffConfig(item, false, false, false, false, true)))
                            {
                                WriteBatchResult("Set 108 Events Failed: " + item.ToString());
                                continue;
                            }

                            if (!CopyBigRockEnding())
                            {
                                WriteBatchResult("Failed Adding Big Rock Ending: " + item.ToString());
                                continue;
                            }

                            if (!checkBoxSmokeTest.Checked)
                            {
                                if (!SaveProFile(item.G6FileName, true))
                                {
                                    WriteBatchResult("Failed saving pro file: " + item.ToString());
                                    continue;
                                }
                            }

                            if (!SaveProCONFile(item, true, true))
                            {
                                WriteBatchResult("Failed Saving: " + item.ToString());
                                continue;
                            }

                        }
                    }
                    catch { WriteBatchResult("Exception while processing: " + item.ToString()); }
                }
                buttonSongLibCancel.Enabled = false;
                WriteBatchResult("Batch Rebuild CON Complete");

            }
            catch { return false; }
            return true;
        }

        private bool ExecuteBatchCheckCON()
        {

            try
            {

                buttonSongLibCancel.Enabled = true;
                progressBarGenCompletedDifficulty.Value = 0;


                var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

                progressBarGenCompletedDifficulty.Maximum = songs.Count();


                foreach (SongCacheItem item in songs)
                {
                    WriteBatchResult("Checking CON File: " + item.ToString());
                    Application.DoEvents();

                    if (buttonSongLibCancel.Enabled == false)
                    {
                        WriteBatchResult("User Cancelled");
                        return false;
                    }
                    try
                    {
                        progressBarGenCompletedDifficulty.Value = songs.IndexOf(item).GetIfNull(0);


                        if ((checkBoxBatchProcessIncomplete.Checked == false && !item.IsComplete))
                        {
                            WriteBatchResult("Skipping (Not Completed): " + item.ToString());
                        }
                        else if (checkBoxBatchProcessFinalized.Checked == false && item.IsFinalized)
                        {
                            WriteBatchResult("Skipping (Finalized): " + item.ToString());
                        }
                        else
                        {

                            if (!OpenSongCacheItem(item))
                            {
                                WriteBatchResult("Unable to open: " + item.ToString());
                                continue;
                            }

                            if (!CheckCONPackageDTA(item, true))
                            {
                                WriteBatchResult("Failed Validation: " + item.ToString());
                            }

                        }
                    }
                    catch { WriteBatchResult("Exception while processing: " + item.ToString()); }
                }
                buttonSongLibCancel.Enabled = false;
                WriteBatchResult("Batch Check CON Files Complete");

                if (checkBatchOpenWhenCompleted.Checked)
                {
                    OpenBatchResults();
                }
            }
            catch { return false; }

            return true;
        }

        private void OpenBatchResults()
        {
            try
            {
                OpenNotepad(textBoxSongLibBatchResults.Text.GetBytes());
            }
            catch { }
        }

        public bool CheckConPackage(Package pk, ref List<string> fileErrors, SongCacheItem item)
        {

            try
            {

                if (pk == null)
                {
                    fileErrors.Add("Cannot open CON File");
                    return false;
                }


                PackageFile upgradeDTA = null;
                try
                {
                    upgradeDTA = pk.GetFile("songs_upgrades", "upgrades.dta");
                }
                catch
                {
                    fileErrors.Add("error extracting upgrades.dta");
                    return false;
                }

                if (upgradeDTA == null)
                {
                    fileErrors.Add("Cannot find DTA file in package");
                    return false;
                }

                string upgradeFileData = string.Empty;
                try
                {
                    upgradeFileData = Encoding.ASCII.GetString(upgradeDTA.Data);
                }
                catch
                {
                    fileErrors.Add("error reading upgrades.dta");
                }

                if (string.IsNullOrEmpty(upgradeFileData))
                {
                    fileErrors.Add("upgrades.dta file is empty");
                }
                else
                {
                    string dtaSongID = string.Empty;
                    try
                    {
                        dtaSongID = DTAGetSongID(upgradeDTA.Data);
                    }
                    catch { }

                    if (dtaSongID.IsEmpty())
                    {
                        fileErrors.Add("song id missing in upgrades.dta");
                    }
                    else
                    {

                        var g6D = InstrumentDifficulty.INVALID;
                        try
                        {
                            g6D = InstrumentDifficultyUtil.DTAGetGuitarDifficulty(upgradeDTA.Data);
                        }
                        catch { }

                        if (g6D == InstrumentDifficulty.INVALID)
                        {
                            fileErrors.Add("invalid guitar difficulty in upgrades.dta");
                        }

                        var g6B = InstrumentDifficulty.INVALID;
                        try
                        {
                            g6B = InstrumentDifficultyUtil.DTAGetBassDifficulty(upgradeDTA.Data);
                        }
                        catch { }

                        if (g6B == InstrumentDifficulty.INVALID)
                        {
                            fileErrors.Add("invalid bass difficulty in upgrades.dta");
                        }


                        if (!pk.GetFiles("songs_upgrades", ".mid|.midi").Any())
                        {
                            fileErrors.Add("No \".mid\" file found in package");
                        }

                        string shortName = string.Empty;
                        try
                        {
                            shortName = DTAGetSongShortName(upgradeDTA.Data);
                        }
                        catch { }

                        if (shortName.IsEmpty())
                        {
                            fileErrors.Add("Song Name Missing from DTA");
                        }
                        else
                        {

                            string proName = DTAGetProFileName(upgradeDTA.Data);

                            if (proName.IsEmpty())
                            {
                                fileErrors.Add("Missing pro file name in upgrades.dta");
                            }
                            else
                            {
                                proName = proName.Trim('"');

                                var s = proName.Split('/');

                                if (s == null || s.Length != 2 || s[0] != "songs_upgrades")
                                {
                                    fileErrors.Add("Invalid path to pro mid file in upgrades.dta: " + proName);
                                }
                                else
                                {
                                    var proMid = pk.GetFile("songs_upgrades", s[1]);

                                    if (proMid == null)
                                    {
                                        fileErrors.Add("pro midi file set in upgrades.dta not found in package");
                                    }
                                    else
                                    {
                                        var sq = proMid.Data.LoadSequence();
                                        if (sq != null)
                                        {
                                            if (!sq.IsFileTypePro())
                                            {
                                                fileErrors.Add("Unable to open pro midi file from package");
                                            }
                                            else
                                            {
                                                ExecAndRestoreTrackDifficulty(delegate(List<string> fe)
                                                {
                                                    fe.AddRange(VerifySongData(proName, sq, item));
                                                }, fileErrors);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

            }
            catch { }

            return fileErrors.Count == 0;
        }

        public bool CheckCONPackageBytes(Package data, bool silent)
        {
            bool ret = false;

            List<string> fileErrors = new List<string>();
            try
            {
                CheckConPackage(data, ref fileErrors, null);
            }
            catch
            {
                if (silent)
                {
                    WriteBatchResult("Unhandled exception ");
                }
                else
                {
                    MessageBox.Show("Unhandled exception");
                }
            }

            if (fileErrors.Count > 0)
            {
                if (silent)
                {
                    WriteBatchResult("Errors ");
                }
            }
            else
            {
                ret = true;
            }
            var sb = new StringBuilder();
            foreach (string s in fileErrors)
            {
                if (silent)
                {
                    WriteBatchResult(s);
                }
                else
                {
                    sb.AppendLine(s);
                }
            }

            RefreshTracks();

            if (!silent)
            {
                if (fileErrors.Count > 0)
                {
                    OpenNotepad(Encoding.ASCII.GetBytes(sb.ToString()));
                }
                else
                {
                    MessageBox.Show("Check OK");
                }
            }

            return ret;
        }

        public bool CheckCONPackageDTA(SongCacheItem item, bool silent)
        {
            bool ret = false;

            if (item == null)
                return ret;
            List<string> fileErrors = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(item.G6FileName))
                {
                    fileErrors.Add("Guitar Midi file name missing");
                }
                if (string.IsNullOrEmpty(item.G6ConFile))
                {
                    fileErrors.Add("Missing CON File");
                }
                else
                {
                    var pk = Package.Load(ReadFileBytes(item.G6ConFile));
                    CheckConPackage(pk, ref fileErrors, item);
                   
                }
            }
            catch (Exception ex)
            {
                if (silent)
                {
                    WriteBatchResult("Unhandled exception processing: " + item.ToString());
                }
                else
                {
                    MessageBox.Show("Unhandled exception : " + ex.Message);
                }
            }

            if (fileErrors.Count > 0)
            {
                if (silent)
                {
                    WriteBatchResult("Errors in - " + item.ToString());
                }
            }
            else
            {
                ret = true;
            }
            var sb = new StringBuilder();
            foreach (string s in fileErrors)
            {
                if (silent)
                {
                    WriteBatchResult(s);
                }
                else
                {
                    sb.AppendLine(s);
                }
            }

            RefreshTracks();

            if (!silent)
            {
                if (fileErrors.Count > 0)
                {
                    OpenNotepad(Encoding.ASCII.GetBytes(sb.ToString()));
                }
                else
                {
                    MessageBox.Show("Check OK");
                }
            }

            return ret;
        }

        List<string> VerifySongData(string proName, Sequence sq, SongCacheItem item)
        {

            var fileErrors = new List<string>();
            try
            {

                foreach(var track in sq)
                {
                    if (track.Name.IsEmpty())
                    {
                        fileErrors.Add("Null track name");
                    }
                    else if (track.Name.IsProTrackName() && sq.IndexOf(track) == 0)
                    {
                        fileErrors.Add("Tempo must be first track");
                    }
                }

                bool foundGuitarTrack = false;
                bool foundBassTrack = false;
                bool foundGuitar22 = false;
                bool foundBass22 = false;
                bool foundGuitar17 = false;
                bool foundBass17 = false;

                foreach (Track t in sq)
                {
                    var name = t.Name;
                    if(name.IsGuitarTrackName6())
                    {
                        foundGuitarTrack = true;
                    }
                    else if (name.IsBassTrackName6())
                    {
                        foundBassTrack = true;
                    }
                    if (name.IsGuitarTrackName22())
                    {
                        foundGuitar22 = true;
                    }
                    if(name.IsBassTrackName22())
                    {
                        foundBass22 = true;
                    }
                    if(name.IsGuitarTrackName17())
                    {
                        foundGuitar17 = true;
                    }
                    if(name.IsBassTrackName17())
                    {
                        foundBass17 = true;
                    }
                }

                if (foundGuitarTrack == false)
                {
                    fileErrors.Add("No pro guitar track in midi file: " + proName);
                }
                if (foundBassTrack == false && (item != null && item.HasBass==true))
                {
                    fileErrors.Add("No pro bass track in midi file: " + proName);
                }

                if (foundGuitar22 == true &&
                    foundGuitar17 == false)
                {
                    fileErrors.Add("No 17 fret pro guitar track in file: " + proName);
                }
                if (foundBass22 == true &&
                    foundBass17 == false)
                {
                    fileErrors.Add("No 17 fret pro bass track in file: " + proName);
                }


                foreach (Track t in sq.Where(x => x.Name.IsProTrackName()))
                {
                    var trackName = t.Name;

                    EditorPro.SetTrack6(sq, t, GuitarDifficulty.Expert);
                    

                    var h108 = ProGuitarTrack.Messages.HandPositions;

                    if (!h108.Any())
                    {
                        fileErrors.Add(trackName + " does not have hand position events (108)");
                    }
                    else
                    {
                        var first = h108.First();

                        if (EditorPro.Messages.Chords.Any())
                        {
                            if (first.AbsoluteTicks >= EditorPro.Messages.Chords[0].DownTick)
                            {
                                fileErrors.Add("108 hand position event occurs too late on track: " + (trackName ?? "(no name)"));
                            }
                        }
                        else
                        {
                            fileErrors.Add("no chords defined for track: " + (trackName ?? "(no name)"));
                        }
                    }

                    var tempotrack = EditorPro.GuitarTrack.FindTempoTrack();
                    if (tempotrack == null)
                    {
                        fileErrors.Add(trackName + " no tempo created");
                    }
                    else
                    {
                        if (tempotrack.TimeSig.Count() == 0)
                        {
                            fileErrors.Add(trackName + " Time signature missing from tempo track.");
                        }

                        if (tempotrack.Tempo.Count() == 0)
                        {
                            fileErrors.Add(trackName + " Tempo missing from tempo track.");
                        }
                    }


                    for (int tx = 0; tx < 4; tx++)
                    {
                        EditorPro.CurrentDifficulty = (GuitarDifficulty)(((int)GuitarDifficulty.Easy) << tx);
                        

                        if (!EditorPro.GuitarTrack.Messages.Chords.Any())
                        {
                            fileErrors.Add(trackName + " difficulty not created: " + EditorPro.CurrentDifficulty.ToString());
                        }
                        else
                        {
                            int nonSnappedNotes = 0;
                            int overlappingChords = 0;
                            for (int x = 0; x < EditorPro.GuitarTrack.Messages.Chords.Length - 2; x++)
                            {
                                var c1 = EditorPro.GuitarTrack.Messages.Chords[x];
                                var c2 = EditorPro.GuitarTrack.Messages.Chords[x + 1];

                                int min1 = int.MaxValue;
                                int max1 = int.MinValue;
                                int firstMin = int.MaxValue;
                                int firstMax = int.MinValue;
                                foreach (var n in c1.Notes)
                                {
                                    if (n != null)
                                    {
                                        if (firstMin == int.MaxValue)
                                        {
                                            firstMin = n.DownTick;
                                        }
                                        else
                                        {
                                            if (n.DownTick != firstMin)
                                            {
                                                nonSnappedNotes++;
                                            }
                                        }
                                        if (firstMax == int.MinValue)
                                        {
                                            firstMax = n.UpTick;
                                        }
                                        else
                                        {
                                            if (n.UpTick != firstMax)
                                            {
                                                nonSnappedNotes++;
                                            }
                                        }
                                        if (n.DownTick < min1)
                                        {
                                            min1 = n.DownTick;
                                        }
                                        if (n.UpTick > max1)
                                        {
                                            max1 = n.UpTick;
                                        }
                                    }
                                }

                                int min2 = int.MaxValue;
                                int max2 = int.MinValue;
                                foreach (var n in c2.Notes)
                                {
                                    if (n != null)
                                    {
                                        if (n.DownTick < min2)
                                        {
                                            min2 = n.DownTick;
                                        }
                                        if (n.UpTick > max2)
                                        {
                                            max2 = n.UpTick;
                                        }
                                    }
                                }

                                if (min2 < max1)
                                {
                                    overlappingChords++;
                                }
                            }

                            if (overlappingChords > 0)
                            {
                                fileErrors.Add(trackName + " " + overlappingChords.ToString() + " Overlapping notes found: " + EditorPro.CurrentDifficulty.ToString());
                            }
                            if (nonSnappedNotes > 0)
                            {
                                fileErrors.Add(trackName + " " + nonSnappedNotes.ToString() + " Note times not matching chord times: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            bool arpeggioMissingChords = false;
                            bool missingHelperNotes = false;
                            foreach (var mod in EditorPro.GuitarTrack.Messages.Arpeggios)
                            {
                                var chords = EditorPro.GuitarTrack.GetChordsAtTick(
                                    mod.DownTick, mod.UpTick);

                                if (!chords.Any())
                                {
                                    arpeggioMissingChords = true;
                                }
                                else
                                {
                                    bool hasHelper = chords.First().Notes.Any(x=> x.IsArpeggioNote);
                                    
                                    if (hasHelper == false)
                                    {
                                        missingHelperNotes = true;
                                    }
                                }
                            }

                            if (arpeggioMissingChords)
                            {
                                fileErrors.Add(trackName + " Arpeggio found with no chords: " + EditorPro.CurrentDifficulty.ToString());
                            }
                            if (missingHelperNotes)
                            {
                                fileErrors.Add(trackName + " Arpeggio missing helper notes: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            bool modWithNoNotes =
                                EditorPro.GuitarTrack.Messages.SingleStringTremelos.Any(x =>
                                    !EditorPro.GuitarTrack.GetChordsAtTick(x.DownTick, x.UpTick).Any());
                            
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Single string tremelo with no notes: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            modWithNoNotes = EditorPro.GuitarTrack.Messages.MultiStringTremelos.Any(x =>
                                    !EditorPro.GuitarTrack.GetChordsAtTick(x.DownTick, x.UpTick).Any());
                            
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Multi string tremelo with no notes: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            modWithNoNotes = 
                                EditorPro.GuitarTrack.Messages.Powerups.Any(x =>
                                    !EditorPro.GuitarTrack.GetChordsAtTick(x.DownTick, x.UpTick).Any());
                            
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Powerup with no notes: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            bool hasNotes = false;
                            int numNotes = 0;
                            int numZero = 0;
                            int numZeroInARow = 0;
                            bool hasShortChord = false;
                            bool hasMixedXNotes = false;
                            bool hasOver17 = false;

                            foreach (var gc in EditorPro.GuitarTrack.Messages.Chords)
                            {
                                hasNotes = false;
                                int numX = 0;
                                int numNonX = 0;
                                bool allZeroNotes = true;
                                for (int x = 0; x < 6; x++)
                                {
                                    if (gc.Notes[x] == null)
                                        continue;

                                    var noteFret = gc.Notes[x].NoteFretDown;
                                    if (noteFret != -1)
                                    {
                                        hasNotes = true;

                                        if (noteFret != 0)
                                        {
                                            allZeroNotes = false;
                                        }
                                        if (noteFret > 17 && EditorPro.GuitarTrack.Name.IsProTrackName17())
                                        {
                                            hasOver17 = true;
                                        }

                                    }

                                    if (gc.Notes[x].IsXNote)
                                    {
                                        numX++;
                                    }
                                    else
                                    {
                                        numNonX++;
                                    }
                                }


                                if (numX > 0 && numNonX > 0 && numX != numNonX)
                                {
                                    hasMixedXNotes = true;
                                }
                                if (Utility.IsCloseTick( gc.UpTick, gc.DownTick))
                                {
                                    hasShortChord = true;
                                }

                                if (hasNotes)
                                {
                                    numNotes++;
                                }

                                if (allZeroNotes)
                                {
                                    numZeroInARow++;
                                    numZero++;
                                }
                                else
                                {
                                    numZeroInARow = 0;
                                }
                            }

                            if (!t.Name.IsProTrackName22())
                            {
                                if (hasOver17)
                                {
                                    fileErrors.Add(trackName + " Notes above 17 on Mustang chart: " + EditorPro.CurrentDifficulty.ToString());
                                }
                            }
                            if (hasMixedXNotes)
                            {
                                fileErrors.Add(trackName + " Chord(s) found with mixture of mute and non mute notes: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            if (((numZeroInARow > numNotes / 2) ||
                                (numZero == numNotes) && numNotes > 0))
                            {
                                if (numZero == numNotes)
                                {
                                    fileErrors.Add(trackName + " All Zero notes for difficulty: " + EditorPro.CurrentDifficulty.ToString());
                                }
                                else
                                {
                                    fileErrors.Add(trackName + " A lot of missing notes for difficulty: " + EditorPro.CurrentDifficulty.ToString());
                                }
                            }
                            else if (numNotes == 0)
                            {
                                fileErrors.Add(trackName + " No notes set: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            if (hasShortChord)
                            {
                                fileErrors.Add(trackName + " invalid chord length found: " + EditorPro.CurrentDifficulty.ToString());
                            }

                            if (EditorG5.IsLoaded)
                            {

                                if (GuitarTrack.IsBassTrackName(t.Name) && (item != null && item.HasBass == false))
                                {
                                    EditorG5.SetTrack(GuitarTrack.GuitarTrackName5, EditorPro.CurrentDifficulty);
                                }
                                else
                                {
                                    EditorG5.SetTrack(t.Name.GetG5TrackNameFromPro(), EditorPro.CurrentDifficulty);
                                }

                                if (EditorG5.IsLoaded)
                                {
                                    if (EditorG5.Messages.BigRockEndings.Any() &&
                                        !EditorPro.Messages.BigRockEndings.Any())
                                    {
                                        fileErrors.Add(trackName + " Big Rock Ending in 5 button but not pro");
                                    }

                                    if (EditorG5.CurrentDifficulty == GuitarDifficulty.Expert &&
                                        (EditorG5.Messages.Solos.Count() !=
                                        EditorPro.Messages.Solos.Count()))
                                    {
                                        fileErrors.Add(trackName + " Not all solos created for difficulty: " + EditorPro.CurrentDifficulty.ToString());
                                    }
                                    if (EditorG5.Messages.Powerups.Count() !=
                                        EditorPro.Messages.Powerups.Count())
                                    {
                                        fileErrors.Add(trackName + " Not all powerups created for difficulty: " + EditorPro.CurrentDifficulty.ToString());
                                    }
                                }
                            }
                            
                        }

                    }//end for each difficulty 

                    bool foundName = false;
                    string name = "";
                    
                    MidiEvent nameEvent = null;
                    var extraNames = new List<MidiEvent>();
                    foreach (var meta in ProGuitarTrack.GetTrack().Meta.Where(x=> x.MetaType == MetaType.TrackName))
                    {
                        if (foundName)
                        {
                            extraNames.Add(meta);
                        }
                        else
                        {
                            foundName = true;
                            nameEvent = meta;
                            name = meta.MetaMessage.Text;
                        }
                    }
                    if (!foundName)
                    {
                        fileErrors.Add("Contains Invalid track with no name");
                    }
                    else
                    {
                        if (extraNames.Any())
                        {
                            fileErrors.Add(name + " Contains Extra track names:");
                            foreach (var en in extraNames)
                            {
                                fileErrors.Add("Extra Track Name: " + en.MetaMessage.Text);
                            }
                        }
                    }


                    for (int tx = 0; tx < 2; tx++)
                    {
                        EditorPro.CurrentDifficulty = tx == 0 ? GuitarDifficulty.Expert : GuitarDifficulty.Hard;



                        foreach (var gc in EditorPro.GuitarTrack.Messages.Chords)
                        {
                            string chordDesc = string.Empty;
                            for (int x = 0; x < 6; x++)
                            {
                                if (gc.Notes[x] != null)
                                {
                                    chordDesc += "'" + gc.Notes[x].NoteFretDown.ToString() + "',";
                                }
                                else
                                {
                                    chordDesc += "' ',";
                                }
                            }
                            chordDesc = chordDesc.Trim(',');


                            if (gc.HasStrum)
                            {
                                if ((gc.StrumMode & ChordStrum.High) > 0)
                                {
                                    if (gc.Notes[4] == null && gc.Notes[5] == null)
                                    {
                                        fileErrors.Add(trackName + " High strum marker missing high notes : " + chordDesc);
                                    }
                                }
                                if ((gc.StrumMode & ChordStrum.Mid) > 0)
                                {
                                    if (gc.Notes[2] == null && gc.Notes[3] == null)
                                    {
                                        fileErrors.Add(trackName + " Mid strum marker missing Mid notes : " + chordDesc);
                                    }
                                }
                                if ((gc.StrumMode & ChordStrum.Low) > 0)
                                {
                                    if (gc.Notes[0] == null && gc.Notes[1] == null)
                                    {
                                        fileErrors.Add(trackName + " Low strum marker missing low notes : " + chordDesc);
                                    }
                                }
                            }

                        }
                    }
                }//end for each track


            }
            catch (Exception ex)
            {
                fileErrors.Add("Exception while checking file: " + ex.Message);
            }
            return fileErrors;
        }
    }
}
