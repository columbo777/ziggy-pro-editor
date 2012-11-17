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

            progressBarGenCompletedDifficulty.Maximum = songs.Count;


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

            progressBarGenCompletedDifficulty.Maximum = songs.Count;

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

            progressBarGenCompletedDifficulty.Maximum = songs.Count;


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

            progressBarGenCompletedDifficulty.Maximum = songs.Count;

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
            try
            {
                Track originalG5Track = null, originalProTrack = null;

                var g5Track = EditorG5.GuitarTrack;
                var proTrack = EditorPro.GuitarTrack;

                if (proTrack == null)
                    return false;

                originalG5Track = g5Track.GetTrack();
                originalProTrack = proTrack.GetTrack();

                var currentDifficulty = GetEditorDifficulty();

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
                    guitarDifficulties = currentDifficulty;
                    bassDifficulties = currentDifficulty;
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
                        if (guitarDifficulties != GuitarDifficulty.None)
                        {
                            GenerateDifficultiesForTrack(originalG5Track, originalProTrack, currentDifficulty, guitarDifficulties);
                        }
                    }
                    else if (selectedTrackBass)
                    {
                        if (bassDifficulties != GuitarDifficulty.None)
                        {
                            GenerateDifficultiesForTrack(originalG5Track, originalProTrack, currentDifficulty, bassDifficulties);
                        }
                    }
                }
                else
                {
                    if (genGuitar && guitarDifficulties != GuitarDifficulty.None)
                    {
                        var gt5 = EditorG5.GetGuitar5MidiTrack();
                        var gt6 = EditorPro.GetTrackGuitar17();

                        if (gt5 != null && gt6 != null)
                        {
                            GenerateDifficultiesForTrack(gt5, gt6, currentDifficulty, guitarDifficulties);
                        }

                        gt6 = EditorPro.GetTrackGuitar22();
                        if (gt5 != null && gt6 != null)
                        {
                            GenerateDifficultiesForTrack(gt5, gt6, currentDifficulty, guitarDifficulties);
                        }
                    }

                    if (genBass && bassDifficulties != GuitarDifficulty.None)
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
                                GenerateDifficultiesForTrack(gt5, gt6, currentDifficulty, bassDifficulties);
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
                                GenerateDifficultiesForTrack(gt5, gt6, currentDifficulty, bassDifficulties);
                            }
                        }
                    }
                }

                ReloadTracks();

                if (config.Generate108Events)
                {
                    Set108Events(config);
                }

                SetEditorDifficulty(currentDifficulty);
                EditorG5.SetTrack5(originalG5Track, currentDifficulty);
                EditorPro.SetTrack6(originalProTrack, currentDifficulty);
                ReloadTracks();
            }
            catch { return false; }

            RefreshTracks();

            return true;
        }

        private void GenerateDifficultiesForTrack(Track originalG5Track, Track originalProTrack, GuitarDifficulty currentDifficulty, GuitarDifficulty guitarDifficulties)
        {
            if (guitarDifficulties.IsExpert())
            {
                guitarDifficulties ^= GuitarDifficulty.Expert;
            }

            if (guitarDifficulties == GuitarDifficulty.None || guitarDifficulties == GuitarDifficulty.Unknown)
                return;

            SetEditorDifficulty(currentDifficulty);
            RemoveEventsByDifficulty6(guitarDifficulties);

            if (guitarDifficulties.IsHard())
            {
                SetEditorDifficulty(GuitarDifficulty.Hard);
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Hard);
            }

            if (guitarDifficulties.IsMedium())
            {
                SetEditorDifficulty(GuitarDifficulty.Medium);
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Medium);
            }

            if (guitarDifficulties.IsEasy())
            {
                SetEditorDifficulty(GuitarDifficulty.Easy);
                GenDiffNotes(originalG5Track, originalProTrack, GuitarDifficulty.Easy);
            }

            SetEditorDifficulty(currentDifficulty);
        }

        private void GenDiffNotes(Track tG5, Track tG6, GuitarDifficulty diff)
        {


            SetEditorDifficulty(GuitarDifficulty.Expert);
            EditorG5.SetTrack5(tG5, GuitarDifficulty.Expert);
            EditorPro.SetTrack6(tG6, GuitarDifficulty.Expert);


            var expertArpeggios = EditorPro.GuitarTrack.Messages.Arpeggios;
            var xchords = EditorPro.GuitarTrack.Messages.Chords.ToList();

            SetEditorDifficulty(diff);
            EditorPro.SetTrack6(tG6, diff);


            EditorG5.SetTrack5(tG5, diff);
            var g5c = EditorG5.GuitarTrack.Messages.Chords;

            var g6e = Utility.GetStringLowE(diff);
            var g6ex = Utility.GetStringLowE(GuitarDifficulty.Expert);


            int lastUpTick = int.MinValue;
            int currUpTick = int.MinValue;


            foreach (var ch in xchords)
            {
                var g5notes = g5c.GetBetweenTick(ch.DownTick, ch.UpTick);
                if (g5notes.Any())
                {

                    int currDownTick = ch.DownTick;
                    currUpTick = ch.UpTick;
                    if (diff == GuitarDifficulty.Easy || diff == GuitarDifficulty.Medium)
                    {
                        currDownTick = g5notes.GetMinTick();
                        currUpTick = g5notes.GetMaxTick();
                    }


                    if (ch.DownTick >= lastUpTick)
                    {
                        lastUpTick = currUpTick;
                        int notesCreated = 0;
                        var frets = new int[6] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue };
                        var channels = new int[6] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue };

                        for (int x = 0; x < 6; x++)
                        {
                            var n = ch.Notes[x];
                            if (n != null)
                            {
                                if ((n.IsArpeggioNote && diff == GuitarDifficulty.Hard) || n.IsArpeggioNote == false)
                                {
                                    frets[x] = n.NoteFretDown;
                                    channels[x] = n.Channel;

                                    notesCreated++;

                                    if (diff == GuitarDifficulty.Easy)
                                        break;
                                    if (diff == GuitarDifficulty.Medium && notesCreated > 1)
                                        break;
                                }
                            }
                        }
                        if (notesCreated > 0)
                        {
                            ProGuitarTrack.CreateChord(frets, channels, diff, currDownTick, currUpTick, Utility.GetSlideData1(diff) != -1 ? ch.IsSlide : false,
                                Utility.GetSlideData1(diff) != -1 ? ch.IsSlideReversed : false,
                                Utility.GetHammeronData1(diff) != -1 ? ch.IsHammeron : false,
                                Utility.GetStrumData1(diff) != -1 ? ch.StrumMode : ChordStrum.Normal);
                        }

                    }
                }
            }


            if (diff == GuitarDifficulty.Hard)
            {
                foreach (var arp in expertArpeggios)
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

                progressBarGenCompletedDifficulty.Maximum = songs.Count;

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

                            if (!SaveProFile(item.G6FileName, true))
                            {
                                WriteBatchResult("Failed saving pro file: " + item.ToString());
                                continue;
                            }

                            if (!SaveProCONFile(item, false, true))
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

                progressBarGenCompletedDifficulty.Maximum = songs.Count;


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
                //var resultFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                //     "BatchErrors.txt");

                //File.WriteAllText(resultFile, textBoxSongLibBatchResults.Text);

                OpenNotepad(textBoxSongLibBatchResults.Text.GetBytes());

            }
            catch { }
        }

        public bool CheckConPackage(Package pk, ref List<string> fileErrors)
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
                }



                if (upgradeDTA == null)
                {
                    fileErrors.Add("Cannot find DTA file in package");
                }
                else
                {
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

                        if (string.IsNullOrEmpty(dtaSongID))
                        {
                            fileErrors.Add("song id missing in upgrades.dta");
                        }
                        else
                        {
                            int i = dtaSongID.ToInt();
                            if (i.IsNull())
                            {
                                fileErrors.Add("song id invalid in upgrades.dta");
                            }
                            else if (i == 0)
                            {
                                fileErrors.Add("song id cannot be zero in upgrades.dta");
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
                                    g6B = InstrumentDifficultyUtil.DTAGetGuitarDifficulty(upgradeDTA.Data);
                                }
                                catch { }

                                if (g6B == InstrumentDifficulty.INVALID)
                                {
                                    fileErrors.Add("invalid bass difficulty in upgrades.dta");
                                }


                                var mid = pk.GetFiles("songs_upgrades", ".mid|.midi");
                                if (mid == null || mid.Length == 0)
                                {
                                    fileErrors.Add("No \".mid\" file found in package");
                                }

                                string shortName = string.Empty;
                                try
                                {
                                    shortName = DTAGetSongShortName(upgradeDTA.Data);
                                }
                                catch { }

                                if (string.IsNullOrEmpty(shortName))
                                {
                                    fileErrors.Add("Song Name Missing from DTA");
                                }
                                else
                                {

                                    string proName = DTAGetProFileName(upgradeDTA.Data);

                                    if (string.IsNullOrEmpty(proName))
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
                                                var sq = new Sequence(FileType.Pro);
                                                bool loaded = false;
                                                try
                                                {
                                                    sq.Load(new MemoryStream(proMid.Data));

                                                    loaded = sq.Tracks.Where(tn => GuitarTrack.TrackNames6.Contains(tn.Name)).Count() > 0;
                                                }
                                                catch
                                                {
                                                    fileErrors.Add("Unable to open pro midi file from package");
                                                }

                                                if (loaded)
                                                {
                                                    var errors = VerifySongData(proName, sq);
                                                    foreach (var er in errors)
                                                    {
                                                        fileErrors.Add(er);
                                                    }
                                                }




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

                CheckConPackage(data, ref fileErrors);
                /*
                if (item.HasGuitar == true &&
                    g6D == InstrumentDifficulty.NOPART)
                {
                    fileErrors.Add("Has Guitar is marked for song but difficulty is set to no part");
                }
                if (item.HasBass == true &&
                    g6B == InstrumentDifficulty.NOPART)
                {
                    fileErrors.Add("Has Bass is marked for song but difficulty is set to no part");
                }*/


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
            StringBuilder sb = new StringBuilder();
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
                    CheckConPackage(pk, ref fileErrors);
                    /*
                    if (item.HasGuitar == true &&
                        g6D == InstrumentDifficulty.NOPART)
                    {
                        fileErrors.Add("Has Guitar is marked for song but difficulty is set to no part");
                    }
                    if (item.HasBass == true &&
                        g6B == InstrumentDifficulty.NOPART)
                    {
                        fileErrors.Add("Has Bass is marked for song but difficulty is set to no part");
                    }*/

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

        List<string> VerifySongData(string proName, Sequence sq)
        {

            var fileErrors = new List<string>();
            try
            {

                for (int x = 0; x < sq.Count; x++)
                {
                    var sqt = sq[x];
                    if (string.IsNullOrEmpty(sqt.Name))
                    {
                        fileErrors.Add("Null track name");
                    }
                    else if (!GuitarTrack.TrackNames6.Contains(sqt.Name))
                    {
                        if (sqt.Tempo.Any() && x != 0)
                        {
                            fileErrors.Add("Tempo must be first track");
                        }
                    }
                    else if (GuitarTrack.TrackNames6.Contains(sqt.Name) && x == 0)
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
                    if (GuitarTrack.GuitarTrackNames6.Contains(name))
                    {
                        foundGuitarTrack = true;
                    }
                    if (GuitarTrack.BassTrackNames6.Contains(name))
                    {
                        foundBassTrack = true;
                    }
                    if (GuitarTrack.GuitarTrackName22 == name)
                    {
                        foundGuitar22 = true;
                    }
                    if (GuitarTrack.BassTrackName22 == name)
                    {
                        foundBass22 = true;
                    }
                    if (GuitarTrack.GuitarTrackName17 == name)
                    {
                        foundGuitar17 = true;
                    }
                    if (GuitarTrack.BassTrackName17 == name)
                    {
                        foundBass17 = true;
                    }
                }

                if (foundGuitarTrack == false)
                {
                    fileErrors.Add("No pro guitar track in midi file: " + proName);
                }
                if (foundBassTrack == false)
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


                foreach (Track t in sq.Where(x => GuitarTrack.TrackNames6.Contains(x.Name)))
                {
                    var trackName = t.Name;

                    SetEditorDifficulty(GuitarDifficulty.Expert);

                    var nt = TrackEditor.GetTrack(sq, trackName);

                    if (nt == null || !EditorPro.SetTrack6(sq, nt, GetEditorDifficulty()))
                    {
                        fileErrors.Add("Cannot load track: " + (trackName ?? "(no name)"));
                        continue;
                    }

                    if (EditorG5.IsLoaded)
                    {
                        if (GuitarTrack.IsGuitarTrackName(t.Name))
                        {
                            var t5 = EditorG5.GetTrack(GuitarTrack.GuitarTrackName5);
                            if (t5 != null)
                            {
                                EditorG5.SetTrack5(t5, GetEditorDifficulty());
                            }
                        }
                        else if (GuitarTrack.IsBassTrackName(t.Name))
                        {
                            var t5 = EditorG5.GetTrack(GuitarTrack.BassTrackName5);
                            if (t5 != null)
                            {
                                EditorG5.SetTrack5(t5, GetEditorDifficulty());
                            }
                        }
                    }
                    ReloadTracks();

                    var h108 = EditorPro.GuitarTrack.ChanEventsAll.Where(x =>
                        Utility.HandPositionData1 == x.Data1);

                    if (h108.Count() == 0)
                    {
                        fileErrors.Add(trackName + " does not have hand position events (108)");
                    }
                    else
                    {
                        var first = h108.First();

                        if (EditorPro.Messages.Chords != null)
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
                        SetEditorDifficulty((GuitarDifficulty)(((int)GuitarDifficulty.Easy) << tx));

                        if (!EditorPro.SetTrack6(sq, t, GetEditorDifficulty()))
                        {
                            fileErrors.Add(trackName + " Failed loading difficulty: " + GetEditorDifficulty().ToString());

                            continue;
                        }


                        if (EditorPro.GuitarTrack.Messages.Chords.Count() == 0)
                        {
                            fileErrors.Add(trackName + " difficulty not created: " + GetEditorDifficulty().ToString());
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
                                fileErrors.Add(trackName + " " + overlappingChords.ToString() + " Overlapping notes found: " + GetEditorDifficulty().ToString());
                            }
                            if (nonSnappedNotes > 0)
                            {
                                fileErrors.Add(trackName + " " + nonSnappedNotes.ToString() + " Note times not matching chord times: " + GetEditorDifficulty().ToString());
                            }

                            bool arpeggioMissingChords = false;
                            bool missingHelperNotes = false;
                            foreach (var mod in EditorPro.GuitarTrack.Messages.Arpeggios)
                            {
                                var chords = EditorPro.GuitarTrack.GetChordsAtTick(
                                    mod.DownTick, mod.UpTick).ToList();

                                if (chords.Count == 0)
                                {
                                    arpeggioMissingChords = true;
                                }

                                if (chords.Count > 0)
                                {
                                    bool hasHelper = false;
                                    var firstChord = chords[0];
                                    foreach (var n in firstChord.Notes.Where(x => x != null))
                                    {
                                        if (n.IsArpeggioNote)
                                        {
                                            hasHelper = true;
                                            break;
                                        }
                                    }
                                    if (hasHelper == false)
                                    {
                                        missingHelperNotes = true;
                                    }
                                }
                            }

                            if (arpeggioMissingChords)
                            {
                                fileErrors.Add(trackName + " Arpeggio found with no chords: " + GetEditorDifficulty().ToString());
                            }
                            if (missingHelperNotes)
                            {
                                fileErrors.Add(trackName + " Arpeggio missing helper notes: " + GetEditorDifficulty().ToString());
                            }

                            bool modWithNoNotes = false;
                            foreach (var mod in EditorPro.GuitarTrack.Messages.SingleStringTremelos)
                            {
                                var chords = EditorPro.GuitarTrack.GetChordsAtTick(
                                    mod.DownTick, mod.UpTick).ToList();

                                if (chords.Count == 0)
                                {
                                    modWithNoNotes = true;
                                }
                                break;
                            }
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Single string tremelo with no notes: " + GetEditorDifficulty().ToString());
                            }

                            modWithNoNotes = false;
                            foreach (var mod in EditorPro.GuitarTrack.Messages.MultiStringTremelos)
                            {
                                var chords = EditorPro.GuitarTrack.GetChordsAtTick(
                                    mod.DownTick, mod.UpTick).ToList();

                                if (chords.Count == 0)
                                {
                                    modWithNoNotes = true;
                                }
                                break;
                            }
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Multi string tremelo with no notes: " + GetEditorDifficulty().ToString());
                            }

                            modWithNoNotes = false;
                            foreach (var mod in EditorPro.GuitarTrack.Messages.Powerups)
                            {
                                var chords = EditorPro.GuitarTrack.GetChordsAtTick(
                                    mod.DownTick, mod.UpTick).ToList();

                                if (chords.Count == 0)
                                {
                                    modWithNoNotes = true;
                                }
                                break;
                            }
                            if (modWithNoNotes)
                            {
                                fileErrors.Add(trackName + " Powerup with no notes: " + GetEditorDifficulty().ToString());
                            }

                            var hammerons = EditorPro.GuitarTrack.ChanEvents.Where(x =>
                                Utility.AllHammeronData1.Contains(x.Data1)).ToList();

                            if (hammerons.Count > 0 &&
                                (GetEditorDifficulty() == GuitarDifficulty.Easy ||
                                    GetEditorDifficulty() == GuitarDifficulty.Medium))
                            {
                                fileErrors.Add(trackName + " " +
                                    " Hammerons cannot be on easy or medium: " + GetEditorDifficulty().ToString());
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
                                        if (noteFret > 17 &&
                                            (GuitarTrack.GuitarTrackName17.Contains(EditorPro.GuitarTrack.Name) ||
                                            GuitarTrack.BassTrackName17.Contains(EditorPro.GuitarTrack.Name)))
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
                                if (gc.UpTick - gc.DownTick < Utility.NoteCloseWidth)
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

                            if (!GuitarTrack.IsTrackName22(t.Name))
                            {
                                if (hasOver17)
                                {
                                    fileErrors.Add(trackName + " Notes above 17 on Mustang chart: " + GetEditorDifficulty().ToString());
                                }
                            }
                            if (hasMixedXNotes)
                            {
                                fileErrors.Add(trackName + " Chord(s) found with mixture of mute and non mute notes: " + GetEditorDifficulty().ToString());
                            }

                            if (((numZeroInARow > numNotes / 2) ||
                                (numZero == numNotes) && numNotes > 0))
                            {
                                if (numZero == numNotes)
                                {
                                    fileErrors.Add(trackName + " All Zero notes for difficulty: " + GetEditorDifficulty().ToString());
                                }
                                else
                                {
                                    fileErrors.Add(trackName + " A lot of missing notes for difficulty: " + GetEditorDifficulty().ToString());
                                }
                            }
                            else if (numNotes == 0)
                            {
                                fileErrors.Add(trackName + " No notes set: " + GetEditorDifficulty().ToString());
                            }

                            if (hasShortChord)
                            {
                                fileErrors.Add(trackName + " invalid chord length found: " + GetEditorDifficulty().ToString());
                            }

                            if (EditorG5.IsLoaded)
                            {
                                if (EditorG5.Messages.BigRockEndings.Count() > 0 &&
                                    EditorPro.Messages.BigRockEndings.Count() == 0)
                                {
                                    fileErrors.Add(trackName + " Big Rock Ending in 5 button but not pro");
                                }

                                if (EditorG5.Messages.Solos.Count() >
                                    EditorPro.Messages.Solos.Count())
                                {
                                    fileErrors.Add(trackName + " Not all solos created for difficulty: " + GetEditorDifficulty().ToString());
                                }
                            }
                        }

                    }//end for each difficulty 

                    bool foundName = false;
                    string name = "";
                    var metas = t.Meta;
                    MidiEvent nameEvent = null;
                    var extraNames = new List<MidiEvent>();
                    foreach (var meta in metas)
                    {
                        var m = meta.MidiMessage as MetaMessage;
                        if (m.MetaType == MetaType.TrackName)
                        {
                            if (foundName)
                            {
                                extraNames.Add(meta);
                            }
                            else
                            {
                                foundName = true;
                                nameEvent = meta;
                                name = Encoding.ASCII.GetString(m.GetBytes());
                            }
                        }
                    }
                    if (!foundName)
                    {
                        fileErrors.Add("Contains Invalid track with no name");
                    }
                    else
                    {
                        if (extraNames.Count > 0)
                        {
                            fileErrors.Add(name + " Contains Extra track names:");
                            foreach (var en in extraNames)
                            {
                                fileErrors.Add("Extra Track Name: " + en);
                            }
                        }
                    }


                    for (int tx = 0; tx < 2; tx++)
                    {
                        if (!EditorPro.SetTrack6(sq, t, tx == 0 ? GuitarDifficulty.Expert : GuitarDifficulty.Hard))
                        {
                            fileErrors.Add(trackName + " Failed loading Expert");
                        }
                        else
                        {

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
                    }
                }//end for each track



                SetEditorDifficulty(GuitarDifficulty.Expert);
                EditorPro.SetTrack6(EditorPro.GetGuitar6MidiTrack(), GetEditorDifficulty());
                EditorG5.SetTrack5(EditorG5.GetGuitar5MidiTrack(), GetEditorDifficulty());
                ReloadTracks();
            }
            catch (Exception ex)
            {
                fileErrors.Add("Exception while checking file: " + ex.Message);
            }
            return fileErrors;
        }
    }
}
