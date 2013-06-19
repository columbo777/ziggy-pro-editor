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
                        if (!item.G6FileName.IsEmpty())
                        {
                            if (File.Exists(item.G6FileName))
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
                        if (item.G5FileName.FileExists())
                        {
                            var destFile = destFolder.PathCombine(item.G5FileName.GetFileName());

                            if (!TryCopyFile(item.G5FileName, destFile))
                            {
                                WriteBatchResult("Unable to overwrite: " + item.ToString());
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
            ExecAndRestoreTrackDifficulty(() =>
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
                            if (proTrack.Name.IsGuitarTrackName6())
                            {
                                genGuitar = true;
                                selectedTrackGuitar = true;
                            }
                            else if (proTrack.Name.IsBassTrackName6())
                            {
                                genBass = true;
                                selectedTrackBass = true;
                            }
                        }
                        else
                        {
                            genGuitar = true;
                            genBass = config.CopyGuitarToBass == false;
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
                            if (selectedTrackGuitar && genGuitar)
                            {
                                if (!guitarDifficulties.IsUnknownOrNone())
                                {
                                    GenerateDifficultiesForTrack(EditorG5.GuitarTrack.GetTrack(), EditorPro.GuitarTrack.GetTrack(), guitarDifficulties, config);
                                }
                            }
                            else if (selectedTrackBass && genBass)
                            {
                                if (!bassDifficulties.IsUnknownOrNone())
                                {
                                    GenerateDifficultiesForTrack(EditorG5.GuitarTrack.GetTrack(), EditorPro.GuitarTrack.GetTrack(), bassDifficulties, config);
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

                            if (config.CopyGuitarToBass)
                            {
                                if (EditorPro.GetTrackGuitar17() != null)
                                {
                                    CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17);
                                }
                                if (EditorPro.GetTrackGuitar22() != null)
                                {
                                    CopyTrack(GuitarTrack.GuitarTrackName22, GuitarTrack.BassTrackName22);
                                }
                            }
                            else if (genBass)
                            {
                                var gt5 = EditorG5.GetGuitar5BassMidiTrack();

                                var gt6 = EditorPro.GetTrackBass17();
                                if (gt5 != null && gt6 != null)
                                {
                                    GenerateDifficultiesForTrack(gt5, gt6, bassDifficulties, config);
                                }
                                
                                gt6 = EditorPro.GetTrackBass22();
                                if (gt5 != null && gt6 != null)
                                {
                                    GenerateDifficultiesForTrack(gt5, gt6, bassDifficulties, config);
                                }
                            }
                        }

                        if (config.Generate108Events)
                        {
                            Set108Events(config);
                        }

                        ret = true;
                    }
                    catch { ret = false; }
                });
            return ret;
        }


        private void GenerateDifficultiesForTrack(
            Track trackG5,
            Track trackG6,
            GuitarDifficulty guitarDifficulties,
            GenDiffConfig config)
        {
           
            if (guitarDifficulties.IsUnknownOrNone())
                return;

            var diffs = guitarDifficulties.GetDifficulties().Where(x => x.IsEasyMediumHard());

            EditorPro.SetTrack(trackG6, GuitarDifficulty.Expert);
            EditorG5.SetTrack(trackG5, GuitarDifficulty.Expert);

            var expertChords = EditorPro.Messages.Chords.ToList();

            foreach (var diff in diffs)
            {
                var sourceDifficulty =
                    diff == GuitarDifficulty.Easy ? GuitarDifficulty.Medium :
                    diff == GuitarDifficulty.Medium ? GuitarDifficulty.Hard :
                    GuitarDifficulty.Expert;

                EditorPro.CurrentDifficulty = sourceDifficulty;

                var sourceChords = EditorPro.Messages.Chords.ToList();
                var sourceArpeggio = EditorPro.Messages.Arpeggios.ToList();

                EditorPro.CurrentDifficulty = diff;
                EditorG5.CurrentDifficulty = diff;

                EditorPro.Messages.Chords.ToList().ForEach(x => x.DeleteAll());

                EditorPro.Messages.Arpeggios.Where(x => x.Difficulty == diff).ToList().ForEach(x => x.DeleteAll());


                GenDiffNotes(expertChords, sourceChords, sourceArpeggio, diff, config);

            }

        }

        private void GenDiffNotes(
            IEnumerable<GuitarChord> expertChords,
            IEnumerable<GuitarChord> sourceChords,
            IEnumerable<GuitarArpeggio> sourceArpeggio,
            GuitarDifficulty targetDifficulty, GenDiffConfig config)
        {
            var chords = sourceChords.Where(x => x.IsPureArpeggioHelper == false).ToList();
            CreateChords(expertChords, targetDifficulty, chords);

            if (targetDifficulty == GuitarDifficulty.Hard)
            {
                var owner = EditorPro.Messages;
                foreach (var arp in sourceArpeggio.Where(x => owner.Chords.AnyBetweenTick(x.TickPair)))
                {
                    GuitarArpeggio.CreateArpeggio(owner, arp.TickPair, targetDifficulty);
                }
            }
        }

        private void CreateChords(
            IEnumerable<GuitarChord> expertChords,
            GuitarDifficulty targetDifficulty, 
            IEnumerable<GuitarChord> sourceChords)
        {

            GuitarChord lastChord = null;

            foreach (var sc in EditorG5.Messages.Chords.ToList())
            {
                if (lastChord != null && sc.StartTime < lastChord.EndTime)
                    continue;

                var c = CreateChordAtDifficulty(expertChords, sourceChords, targetDifficulty, sc);
                if (c != null)
                {
                    lastChord = c;
                }

            }
        }

        private GuitarChord CreateChordAtDifficulty(
            IEnumerable<GuitarChord> expertChords, 
            IEnumerable<GuitarChord> sourceChords,
            GuitarDifficulty targetDifficulty, GuitarChord sc)
        {
            GuitarChord ret = null;

            var g6c = sourceChords.SingleBetweenTick(sc.TickPair);
            if (g6c == null)
            {
                var last = EditorPro.Messages.Chords.LastOrDefault();
                if (last != null)
                {
                    if (sc.TickPair.Down - Utility.TickCloseWidth > last.UpTick)
                    {
                        var tickPair = sc.TickPair;
                        g6c = sourceChords.SingleBetweenTick(tickPair.Offset(-Utility.TickCloseWidth));
                        if (g6c == null)
                        {
                            tickPair = sc.TickPair;
                            g6c = sourceChords.SingleBetweenTick(tickPair.Offset(Utility.TickCloseWidth));

                        }
                    }
                }
                
            }

            if (g6c == null)
            {
                g6c = expertChords.SingleBetweenTick(sc.TickPair);
            }

            if (g6c != null)
            {

                var frets = Utility.Null6;
                var channels = Utility.Null6;
                int noteCount = 0;

                var notesOverZero = g6c.Notes.Where(x => x.NoteFretDown > 0);

                if (targetDifficulty == GuitarDifficulty.Easy)
                {
                    if (notesOverZero.Any())
                    {
                        var note = notesOverZero.OrderBy(x => x.NoteString).First();
                        frets[note.NoteString] = note.NoteFretDown;
                        channels[note.NoteString] = note.Channel;
                        noteCount = 1;
                    }
                    else
                    {
                        var note = g6c.Notes.OrderBy(x => x.NoteString).First();
                        frets[note.NoteString] = note.NoteFretDown;
                        channels[note.NoteString] = note.Channel;
                        noteCount = 1;
                    }
                }
                else if (targetDifficulty == GuitarDifficulty.Medium)
                {
                    if (notesOverZero.Count() > 1)
                    {
                        var noz = notesOverZero.OrderBy(x => x.NoteString).ToArray();
                        var note1 = noz[0];
                        frets[note1.NoteString] = note1.NoteFretDown;
                        channels[note1.NoteString] = note1.Channel;

                        var note2 = noz[1];
                        frets[note2.NoteString] = note2.NoteFretDown;
                        channels[note2.NoteString] = note2.Channel;
                        noteCount = 2;
                    }
                    else if (notesOverZero.Count() > 0)
                    {
                        var note1 = notesOverZero.OrderBy(x => x.NoteString).First();
                        frets[note1.NoteString] = note1.NoteFretDown;
                        channels[note1.NoteString] = note1.Channel;
                        noteCount = 1;
                    }
                    else
                    {
                        var note = g6c.Notes.OrderBy(x => x.NoteString).First();
                        frets[note.NoteString] = note.NoteFretDown;
                        channels[note.NoteString] = note.Channel;
                        noteCount = 1;
                    }
                }
                else if (targetDifficulty == GuitarDifficulty.Hard)
                {
                    foreach (var note in g6c.Notes)
                    {
                        frets[note.NoteString] = note.NoteFretDown;
                        channels[note.NoteString] = note.Channel;
                        noteCount++;
                    }
                }

                if (noteCount > 0)
                {
                    ret = GuitarChord.CreateChord(
                        EditorPro.Messages,
                        targetDifficulty,
                        sc.TickPair,
                        new GuitarChordConfig(frets, channels, 
                            g6c.HasSlide, 
                            g6c.HasSlideReversed, 
                            g6c.HasHammeron, 
                            g6c.StrumMode,
                            null));

                }
            }
            return ret;
        }

        public void RefreshTracks()
        {
            UpdateControlsForDifficulty(EditorPro.CurrentDifficulty);
            RefreshTracks5();
            RefreshTracks6();
            EditorPro.Invalidate();
            EditorG5.Invalidate();
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
                        var songIDList = LoadDTAFile(upgradeDTA.Data);
                        if (songIDList.Any())
                        {
                            songIDList.GetSongIDs().FirstOrDefault().GetIfNotNull(x => dtaSongID = x.Value);
                        }
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

                                                fileErrors.AddRange(VerifySongData(proName, sq, item));

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

        List<string> VerifySongData(string midiTrackName, Sequence midiSequence, SongCacheItem songCacheItem)
        {

            var fileErrors = new List<string>();
            try
            {
                if (midiSequence.FirstOrDefault() == null)
                {
                    fileErrors.Add("No Tracks");
                }
                else
                {
                    var proTracks = midiSequence.Skip(1).ToList();
                    foreach (var track in proTracks)
                    {
                        if (track.Name != null && track.Name == "(no name)")
                        {
                            fileErrors.Add("******** Missing Track Name: " + (track.Name ?? ""));
                        }
                        else if (track.Name.IsProTrackName() == false)
                        {
                            fileErrors.Add("Invalid Track Name: " + (track.Name ?? ""));
                        }
                    }
                }
                foreach (var track in midiSequence)
                {
                    if (track.Name.IsEmpty())
                    {
                        fileErrors.Add("Null track name");
                    }
                    else if (track.Name.IsProTrackName() && midiSequence.IndexOf(track) == 0)
                    {
                        fileErrors.Add("Tempo must be first track");
                    }
                }

                var tracks = midiSequence.GetGuitarBassTracks().ToList();
                bool foundGuitarTrack = tracks.Any(x => x.Name.IsGuitarTrackName());
                bool foundBassTrack = tracks.Any(x => x.Name.IsBassTrackName());
                bool foundGuitar22 = tracks.Any(x => x.Name.IsGuitarTrackName22());
                bool foundBass22 = tracks.Any(x => x.Name.IsBassTrackName22());
                bool foundGuitar17 = tracks.Any(x => x.Name.IsGuitarTrackName17());
                bool foundBass17 = tracks.Any(x => x.Name.IsBassTrackName17());

                if (foundGuitarTrack == false)
                {
                    fileErrors.Add("No pro guitar track in midi file: " + midiTrackName);
                }
                if (foundBassTrack == false && (songCacheItem != null && songCacheItem.HasBass == true))
                {
                    fileErrors.Add("No pro bass track in midi file: " + midiTrackName);
                }

                if (foundGuitar22 == true && foundGuitar17 == false)
                {
                    fileErrors.Add("No 17 fret pro guitar track in file: " + midiTrackName);
                }
                if (foundBass22 == true && foundBass17 == false)
                {
                    fileErrors.Add("No 17 fret pro bass track in file: " + midiTrackName);
                }


                foreach (var currentTrack in tracks)
                {

                    var currentTrackName = currentTrack.Name ?? "(no name)";

                    if (!EditorPro.SetTrack6(midiSequence, currentTrack, GuitarDifficulty.Expert))
                    {
                        fileErrors.Add(currentTrackName + " Failed to set track ");
                        continue;
                    }


                    var metaNames = currentTrack.Meta.Where(x => x.MetaType == MetaType.TrackName).ToList();

                    if (!metaNames.Any())
                    {
                        fileErrors.Add("Contains Invalid track with no name");
                    }
                    else if (metaNames.Count() > 1)
                    {
                        fileErrors.Add(metaNames.First().Text + " Contains Extra track names:");
                        foreach (var en in metaNames)
                        {
                            fileErrors.Add(en.Text);
                        }
                    }

                    foreach (var diff in Utility.GetDifficultyIter())
                    {

                        EditorPro.CurrentDifficulty = diff;

                        var chords = EditorPro.Messages.Chords.ToList();
                        if (!chords.Any())
                        {
                            fileErrors.Add(currentTrackName + " No Chords Created !! " + diff);
                            continue;
                        }
                        if (diff.IsExpert())
                        {

                            if (EditorG5.IsLoaded)
                            {
                                var copyGuitarToBass = (songCacheItem != null && songCacheItem.CopyGuitarToBass == true);
                                if (currentTrackName.IsBassTrackName() && !copyGuitarToBass &&
                                    EditorG5.GetTrack(GuitarTrack.BassTrackName5) != null)
                                {
                                    EditorG5.SetTrack(GuitarTrack.BassTrackName5, diff);
                                }
                                else if (EditorG5.GetTrack(GuitarTrack.GuitarTrackName5) != null)
                                {
                                    EditorG5.SetTrack(GuitarTrack.GuitarTrackName5, diff);
                                }

                                if (EditorG5.Messages.BigRockEndings.Any() &&
                                    !EditorPro.Messages.BigRockEndings.Any())
                                {
                                    fileErrors.Add(currentTrackName + " Big Rock Ending in 5 button but not pro");
                                }

                                var solo5 = EditorG5.Messages.Solos.ToList();
                                var solo6 = EditorPro.Messages.Solos.ToList();

                                var missing5 = solo5.Where(x => !solo6.Any(sx => sx.TickPair == x.TickPair)).ToList();
                                if (missing5.Any())
                                {
                                    fileErrors.Add(currentTrackName + " Not all solos created ");
                                    fileErrors.AddRange(missing5.Select(m => m.TickPair.ToString()));
                                }

                                var power5 = EditorG5.Messages.Powerups.ToList();
                                var power6 = EditorPro.Messages.Powerups.ToList();

                                var missingpower5 = power5.Where(x => !power6.Any(sx => sx.TickPair == x.TickPair)).ToList();
                                if (missingpower5.Any())
                                {
                                    fileErrors.Add(currentTrackName + " Not all powerups snapped ");
                                    fileErrors.Add("Editor G5");
                                    fileErrors.AddRange(power5.Select(m => "    " + (missingpower5.Contains(m) ? " Missing->" : "") + m.TickPair.ToString()));
                                    fileErrors.Add("Editor G6");
                                    fileErrors.AddRange(power6.Select(m => "    " + m.TickPair.ToString()));
                                }


                            }


                            var handPositions = EditorPro.Messages.HandPositions.ToList();
                            if (!handPositions.Any())
                            {
                                fileErrors.Add(currentTrackName + " does not have hand position events (108) " + diff);
                            }
                            else
                            {
                                if (chords.First().DownTick < handPositions.First().DownTick)
                                {
                                    fileErrors.Add("108 hand position event occurs too late on track: " + (currentTrackName) + " " + diff);
                                }
                            }

                            if (chords.First().DownTick < 10)
                            {
                                fileErrors.Add("first chord is too soon in track: " + (currentTrackName) + " " + diff);
                            }

                            var tempotrack = EditorPro.GuitarTrack.GetTempoTrack();
                            if (tempotrack == null)
                            {
                                fileErrors.Add(currentTrackName + " no tempo track found");
                            }


                            if (!EditorPro.Messages.TimeSignatures.Any())
                            {
                                fileErrors.Add(currentTrackName + " Time signature missing from tempo track.");
                            }

                            if (!EditorPro.Messages.Tempos.Any())
                            {
                                fileErrors.Add(currentTrackName + " Tempo missing from tempo track.");
                            }
                        }

                        if (songCacheItem != null)
                        {
                            if (songCacheItem.CopyGuitarToBass == false)
                            {
                                if (currentTrackName.IsBassTrackName())
                                {
                                    
                                    var notesOver4 = chords.Where(c => c.Notes.Any(x=> x.NoteString > 3)).ToList();
                                    if (notesOver4.Any())
                                    {
                                        fileErrors.Add(currentTrackName + " " + notesOver4.Count + " Chords using more than 4 strings on bass: " + diff);
                                        fileErrors.AddRange(notesOver4.Select(m => m.ToString()));
                                    }
                                }
                            }
                        }

                        var noteAligns = chords.Where(x => x.Notes.NotesAligned == false).ToList();
                        if (noteAligns.Any())
                        {
                            fileErrors.Add(currentTrackName + " " + noteAligns.Count + " Note times not snapped: " + diff);
                            fileErrors.AddRange(noteAligns.Select(m => m.ToString()));
                        }

                        var modAligns = chords.Where(chord => chord.Modifiers.Any(modifier => modifier.TickPair != chord.TickPair)).ToList();
                        if (modAligns.Any())
                        {

                            fileErrors.Add(currentTrackName + " " + modAligns.Count + " Modifier times not snapped: " + diff);
                            fileErrors.AddRange(modAligns.Select(m => m.ToString() + " " + m.TickPair.ToString()));
                        }

                        var overlaps = chords.Take(chords.Count - 1).Where(x => x.UpTick > x.NextChord.DownTick).ToList();

                        if (overlaps.Any())
                        {
                            fileErrors.Add(currentTrackName + " " + overlaps.Count + " Overlapping notes found: " + diff);
                            fileErrors.AddRange(overlaps.Select(m => m.ToString()));
                        }


                        if (Utility.GetArpeggioData1(diff).IsNotNull())
                        {
                            var arpeggios = EditorPro.Messages.Arpeggios.ToList();
                            var arpeggioMissingChords = arpeggios.Where(x => chords.AnyBetweenTick(x.TickPair) == false).ToList();
                            if (arpeggioMissingChords.Any())
                            {
                                fileErrors.Add(currentTrackName + " " + arpeggioMissingChords.Count + " Arpeggios found with no chords: " + diff);
                                fileErrors.AddRange(arpeggioMissingChords.Select(m => m.ToString()));
                            }
                        }


                        if (diff.IsExpert())
                        {
                            var sstrem = EditorPro.Messages.SingleStringTremelos.ToList();
                            var modWithNoNotes = sstrem.Where(x => chords.AnyBetweenTick(x.TickPair) == false).ToList();

                            if (modWithNoNotes.Any())
                            {
                                fileErrors.Add(currentTrackName + " " + modWithNoNotes.Count + " Single string tremelo with no notes: " + diff);
                                fileErrors.AddRange(modWithNoNotes.Select(m => m.ToString()));
                            }
                        }

                        if (diff.IsExpert())
                        {
                            var trem = EditorPro.Messages.MultiStringTremelos.ToList();
                            var modWithNoNotes = trem.Where(x => chords.AnyBetweenTick(x.TickPair) == false).ToList();
                            if (modWithNoNotes.Any())
                            {
                                fileErrors.Add(currentTrackName + " " + modWithNoNotes.Count + " Multi string tremelo with no notes: " + diff);
                                fileErrors.AddRange(modWithNoNotes.Select(m => m.ToString()));
                            }
                        }

                        {
                            var powerups = EditorPro.Messages.Powerups.ToList();
                            var modWithNoNotes = powerups.Where(x => chords.AnyBetweenTick(x.TickPair) == false).ToList();

                            if (modWithNoNotes.Any())
                            {
                                fileErrors.Add(currentTrackName + " " + modWithNoNotes.Count + " Powerup with no notes: " + diff);
                                fileErrors.AddRange(modWithNoNotes.Select(m => m.ToString()));
                            }
                        }

                        var over17 = chords.Where(x => x.HighestFret > 17);
                        if (currentTrackName.IsProTrackName17() && over17.Any())
                        {
                            fileErrors.Add(currentTrackName + " Notes above 17 on Mustang chart: " + diff);
                            fileErrors.AddRange(over17.Select(m => m.ToString()));
                        }
                        var chordMix = chords.Where(x => x.Notes.Any(n => n.IsXNote) && x.Notes.Any(n => n.IsXNote) == false);
                        if (chordMix.Any())
                        {
                            fileErrors.Add(currentTrackName + " Mix of mute and non mute: " + diff);
                            fileErrors.AddRange(chordMix.Select(m => m.ToString()));
                        }

                        var numZero = chords.Count(x => x.HighestFret == 0);
                        var numNonZero = chords.Count(x => x.HighestFret != 0);

                        if (numNonZero == 0)
                        {
                            fileErrors.Add(currentTrackName + " No notes set: " + diff);
                        }

                        var shortChords = chords.Where(x => x.TickLength < 8);
                        if (shortChords.Any())
                        {
                            fileErrors.Add(currentTrackName + " invalid chord length found: " + diff);

                            shortChords.ForEach(x => fileErrors.Add("  " + x.ToString()));

                        }

                        var invStrum = chords.Where(x =>
                            (x.HasHighStrum && !x.HighStrumNotes.Any()) ||
                            (x.HasMidStrum && !x.MidStrumNotes.Any()) ||
                            (x.HasLowStrum && !x.LowStrumNotes.Any())
                            ).ToList();

                        if (invStrum.Any())
                        {
                            fileErrors.Add(currentTrackName + " " + diff + " strum marker missing notes");
                            fileErrors.AddRange(invStrum.Select(m => m.ToString()));
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
