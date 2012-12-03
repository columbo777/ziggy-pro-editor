﻿using System;
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
using System.Linq.Expressions;
using X360;


namespace ProUpgradeEditor.UI
{

    partial class MainForm
    {
        

        string FileNameG5
        {
            get { return EditorG5.LoadedFileName; }
            set
            {
                EditorG5.LoadedFileName = value;
                if (string.IsNullOrEmpty(value))
                {
                    labelCurrentLoadedG5.Text = "No 5 Button Guitar File Loaded";
                }
                else
                {
                    labelCurrentLoadedG5.Text = "Editing 5 Button Midi File: " + value;
                }
            }
        }


        string FileNamePro
        {
            get
            {
                return EditorPro.LoadedFileName;
            }
            set
            {
                EditorPro.LoadedFileName = value;
                if (string.IsNullOrEmpty(value))
                {
                    labelCurrentLoadedG6.Text = "No Pro Midi File Loaded";
                }
                else
                {
                    labelCurrentLoadedG6.Text = "Editing Pro Midi File: " + value;
                }
            }
        }


        SongCacheItem SelectedSong
        {
            get { return SongList.SelectedSong; }
            set
            {
                SongList.SelectedSong = value;
                if (SongList.SelectedSong == null)
                {
                    this.Text = "Ziggys Pro Guitar Editor - No Song Loaded";
                }
                else
                {
                    this.Text = "Ziggys Pro Guitar Editor - " + SongList.SelectedSong.Description;
                }
            }
        }

        



        void LoadLastFile(object sciToLoad)
        {
            try
            {
                if (sciToLoad != null)
                {
                   
                    var sc = sciToLoad as SongCacheItem;
                    
                    if (OpenSongCacheItem(sc))
                    {
                        listBoxSongLibrary.SelectedItem = sc;
                    }
                }
            }
            catch { }
        }

        

        public void DoFormLoad()
        {
            
            comboProGDifficulty.SelectedIndex = 0;
            comboProBDifficulty.SelectedIndex = 0;

            textBoxZoom.Text = Utility.timeScalar.ToString();

            AddListedModifiers();


            LoadSettingConfiguration();
            
        }

        void PostLoadSettings()
        {
            CreatePropertiesGrid();

            var freq = settings.GetValueInt("textBoxMidiScrollFreq", 5);
            timerMidiPlayback.Interval = freq;
            textBoxMidiScrollFreq.Text = freq.ToString();

            RefreshMidiOutputList();

            comboMidiInstrument.Items.Clear();
            var names = Enum.GetNames(typeof(GeneralMidiInstrument));
            foreach (var n in names)
            {
                comboMidiInstrument.Items.Add(n);
            }

            comboMidiInstrument.SelectedIndex = settings.GetValueInt("MidiDeviceInstrument", 0);

            UpdateMidiDevice();
            UpdateMidiInstrument(true);

            


            if (checkBoxMidiInputStartup.Checked && checkBoxEnableMidiInput.Checked)
            {
                ConnectMidiDevice();
            }

            UpdateEditorProperties();
        }

        public DialogResult AskToSave(MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel)
        {
            return MessageBox.Show("Save Changes?", "Save changes", buttons);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!DesignMode)
            {
                try
                {

                    if (EditorPro.NumBackups > 0)
                    {
                        var mbr = AskToSave();
                        if (mbr == DialogResult.Yes)
                        {
                            SaveSongCacheItem(SelectedSong, false);
                        }
                        else if (mbr == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }

                    var itm = comboMidiDevice.SelectedItem as MidiOutputListItem;
                    if (itm != null)
                    {
                        settings.SetValue("MidiDeviceIndex", itm.index);
                        settings.SetValue("MidiDeviceDesc", itm.Caps.name);
                    }
                    settings.SetValue("MidiDeviceInstrument", comboMidiInstrument.SelectedIndex);

                    SaveSettingConfiguration();



                    mp3Player.Cleanup();

                }
                catch { }

                try
                {
                    DisconnectMidiDevice();

                    ShutDownMidiDevice();
                }
                catch { }

                lock (timerLocker)
                {
                    foreach (var t in threadTimers)
                    {
                        t.timer.Change(1000, 2000);
                        t.timer.Dispose();
                        t.timer = null;
                    }
                    threadTimers.Clear();
                }
            }
          
            base.OnFormClosing(e);
        }


        public string ShowOpenFileDlg(string caption, string defaultFolder, string startupFolder, bool mustExist = true)
        {
            if (!checkUseDefaultFolders.Checked && string.IsNullOrEmpty(startupFolder))
            {
                var folder = settings.GetValue("OPEN_FILE_" + caption, "");
                if (!string.IsNullOrEmpty(folder))
                {
                    if (folder.IsFileNotFolder())
                        folder = folder.GetFolderName();
                    startupFolder = folder.AppendSlashIfMissing();
                }
            }

            var dlg = new OpenFileDialog();
            dlg.Title = caption;
            dlg.AutoUpgradeEnabled = true;
            if (startupFolder.FileExists())
            {
                dlg.FileName = startupFolder.GetFileName();
                startupFolder = startupFolder.GetFolderName();
            }
            if (startupFolder.IsEmpty() && checkUseDefaultFolders.Checked)
            {
                dlg.InitialDirectory = defaultFolder.AppendSlashIfMissing();
            }
            else if (!startupFolder.IsEmpty())
            {
                dlg.InitialDirectory = startupFolder.GetFolderName().AppendSlashIfMissing();
            }
            else
            {
                dlg.InitialDirectory = settings.GetValue("OPEN_FILE_" + caption, "").AppendSlashIfMissing();
            }
            dlg.CheckFileExists = mustExist;
            dlg.CheckPathExists = mustExist;
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                var folderName = fileName.GetFolderName();
                if (mustExist == false && !folderName.FolderExists())
                {
                    folderName.CreateFolderIfNotExists();
                }
                settings.SetValue("OPEN_FILE_" + caption, folderName);

                return fileName;
            }
            return string.Empty;
        }

        public string ShowSaveFileDlg(string caption, string startupFolder, string fileName)
        {
            if (!checkUseDefaultFolders.Checked && startupFolder.IsEmpty())
            {
                var folder = settings.GetValue("SAVE_FILE_" + caption, "");
                if (!folder.IsEmpty())
                {
                    startupFolder = folder.AppendSlashIfMissing();
                }
            }
            var dlg = new SaveFileDialog();
            dlg.Title = caption;
            dlg.AutoUpgradeEnabled = true;
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = false;
            
            dlg.ValidateNames = true;

            if (checkUseDefaultFolders.Checked && string.IsNullOrEmpty(startupFolder))
            {
                dlg.InitialDirectory = DefaultConFileLocation.AppendSlashIfMissing();
                dlg.FileName = fileName.GetFileName();
            }
            else
            {
                if (fileName.Any(x => x == '\\'))
                {
                    dlg.InitialDirectory = fileName.GetFolderName().CreateFolderIfNotExists();
                    dlg.FileName = fileName.GetFileName();
                }
                else
                {
                    dlg.InitialDirectory = startupFolder.AppendSlashIfMissing();
                    dlg.FileName = fileName.GetFileName();
                }
            }
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlg.FileName.GetFolderName().CreateFolderIfNotExists();

                settings.SetValue("SAVE_FILE_" + caption, dlg.FileName.GetFolderName());

                return dlg.FileName;
            }
            return string.Empty;
        }


        string ShowSelectFolderDlg(string caption, string startupFolder, string alternateStartupFolder)
        {

            string ret = string.Empty;

            //var bf = new BrowseForFolder();
            var bf = new FolderBrowserDialog();
            
            var folder = alternateStartupFolder;
            if (checkUseDefaultFolders.Checked)
            {
                folder = startupFolder;
            }
            if (string.IsNullOrEmpty(folder))
            {
                folder = settings.GetValue("SELECT_FOLDER_" + caption);
            }

           
            bf.SelectedPath = checkUseDefaultFolders.Checked ? startupFolder : alternateStartupFolder;
            bf.ShowNewFolderButton = true;
            bf.Description = caption;

            var res = bf.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK)
            {
                ret = bf.SelectedPath.AppendSlashIfMissing();
               
                settings.SetValue("SELECT_FOLDER_" + caption, ret);
            
            }

            return ret;
        }

        public bool ShowOpenMidi5()
        {
            bool ret = false;
            try
            {

                string file = ShowOpenFileDlg("Open Rock Band 3 Midi File",
                    DefaultMidiFileLocationG5, "");

                if (!string.IsNullOrEmpty(file))
                {
                    if (SelectedSong != null)
                    {
                        SetSelectedSongItem(null);
                    }
                    ret = EditorG5.LoadMidi5(file, ReadFileBytes(file), false);

                    ReloadTracks();
                    RefreshTracks();
                }
            }
            catch { }
            return ret;
        }

        

        public void SetEditorDifficulty(GuitarDifficulty diff)
        {
            radioDifficultyMedium.Checked = false;
            radioDifficultyHard.Checked = false;
            radioDifficultyExpert.Checked = false;
            radioDifficultyEasy.Checked = false;

            radioNoteEditDifficultyExpert.Checked = false;
            radioNoteEditDifficultyHard.Checked = false;
            radioNoteEditDifficultyMedium.Checked = false;
            radioNoteEditDifficultyEasy.Checked = false;

            if (diff == GuitarDifficulty.Easy)
            {
                radioDifficultyEasy.Checked = true;
                radioNoteEditDifficultyEasy.Checked = true;
            }
            else if (diff == GuitarDifficulty.Medium)
            {
                radioDifficultyMedium.Checked = true;
                radioNoteEditDifficultyMedium.Checked = true;
            }
            else if (diff == GuitarDifficulty.Hard)
            {
                radioDifficultyHard.Checked = true;
                radioNoteEditDifficultyHard.Checked = true;
            }
            else if (diff == GuitarDifficulty.Expert)
            {
                radioDifficultyExpert.Checked = true;
                radioNoteEditDifficultyExpert.Checked = true;
            }

            
            if (EditorPro.CurrentDifficulty != diff)
            {
                EditorPro.CurrentDifficulty = diff;
            }
            if (EditorG5.CurrentDifficulty != diff)
            {
                EditorG5.CurrentDifficulty = diff;
            }

            RefreshTracks();
        }
        




        void RefreshTrackList(TrackEditor ed)
        {
            try
            {
                if (ed == EditorPro)
                {
                    midiTrackEditorPro.SetTrack(ed.MidiTrack, ed.CurrentDifficulty);
                    if (this.ProGuitarTrack != null && this.ProGuitarTrack.HasInvalidTempo)
                    {
                        this.buttonInitTempo.Image = ProResources.x;
                        this.buttonInitTempo.ImageAlign = ContentAlignment.MiddleLeft;
                    }
                    else
                    {
                        this.buttonInitTempo.Image = null;
                    }
                    midiTrackEditorPro.Invalidate();
                }
                else
                {
                    midiTrackEditorG5.SetTrack(ed.MidiTrack, ed.CurrentDifficulty);
                    midiTrackEditorG5.Invalidate();
                }
            }
            catch { }
        }

        public void RefreshTracks6()
        {
            RefreshTrackList(EditorPro);
            midiTrackEditorPro.Refresh();
            
        }
        public void RefreshTracks5()
        {
            RefreshTrackList(EditorG5);
            midiTrackEditorG5.Refresh();
        }

        public bool SaveProFile(string fileName, bool silent)
        {
            try
            {
                if (fileName.IsEmpty() || !fileName.FileExists())
                {
                    try
                    {
                        ShowSaveFileDlg("Save Rock Band 3 Pro Midi File", DefaultMidiFileLocationPro,
                            fileName).IfNotEmpty(file =>
                        {
                            EditorPro.SaveTrack(file);
                            EditorPro.LoadMidi17(file, ReadFileBytes(file), false);
                        });
                    }
                    catch { }
                }
                else
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        EditorPro.SaveTrack(fileName);
                    }
                }
                return true;
            }
            catch { }
            return false;
        }


        public bool hasEasyMedHardEvent6()
        {
            try
            {
                return EditorPro.Tracks.Any(x =>
                    x.ChanMessages.Any(cm => cm.ChannelMessage.Data1.GetData1Difficulty(true).IsEasyMediumHard()));
                
            }
            catch { }
            return false;
        }
        public Track SelectedMidiTrack6
        {
            get
            {
                Track ret = null;
                midiTrackEditorPro.SelectedTrack.IfObjectNotNull(x=> ret = x.Track);
                return ret;
            }
        }
        public Track SelectedMidiTrack5
        {
            get
            {
                Track ret = null;
                midiTrackEditorG5.SelectedTrack.IfObjectNotNull(x => ret = x.Track);
                return ret;

            }
        }

        public bool InitializeFrom5Tar()
        {
            bool ret = false;
            try
            {
                if (!EditorG5.IsLoaded)
                    return false;

                Sequence seq = EditorG5.Sequence.ConvertToPro();
                ret = EditorPro.SetTrack6(seq, seq.GetPrimaryTrack(), GuitarDifficulty.Expert);

                EditorG5.SetTrack(EditorG5.GetGuitar5MidiTrack(), GuitarDifficulty.Expert);
            }
            catch { ret = false; }

            return ret;
        }


        public Track BuildTempo(GuitarTrack trackSource)
        {
            return TrackEditor.CopyTrack(trackSource.TempoTrack, "Tempo");
        }

        
        public Point GetMousePosition()
        {
            var nx = EditorPro.PointToClient(MousePosition);

            if (EditorPro.GridSnap)
            {
                nx = EditorPro.GetGridSnapPointFromMouse();
            }
            
            return nx;
        }

        int createItemStartNoteEnd = -1;


        public bool OnTrackEditorProClick(TrackEditor editor, MouseEventArgs e)
        {
            if (DesignMode)
                return false;
            try
            {
                var gc = editor.GetChordFromPoint(e.Location);

                if (editor.CreationState == TrackEditor.EditorCreationState.CreatingNote)
                {
                    if (DoCreateChordFromScreenPoint(editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.SelectingChordStartOffset)
                {
                    if (DoSelectNodeStartFromScreenPoint(editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.SelectingChordEndOffset)
                {
                    if (DoSelelectNoteEndFromScreenPoint(editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CopyingPattern)
                {
                    if (gc != null && DoCopyPatternSelection(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingSolo)
                {
                    if (gc != null && DoSoloCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingPowerup)
                {
                    if (gc != null && DoPowerupCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingMultiTremelo)
                {
                    if (gc != null && DoMultiStringTremeloCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingSingleTremelo)
                {
                    if (gc != null && DoSingleStringTremeloCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingArpeggio)
                {
                    if (gc != null && DoArpeggioCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingProGuitarTrainer)
                {
                    if (gc != null && DoTrainerCreation(gc, editor, e))
                        return true;
                }
                else if (editor.CreationState == TrackEditor.EditorCreationState.CreatingProBassTrainer)
                {
                    if (gc != null && DoTrainerCreation(gc, editor, e))
                        return true;
                }
            }
            catch { }

            return false;
        }

        public bool DoSelectNodeStartFromScreenPoint(TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                
                if (button5.Text == "Cancel")
                {
                    ret = true;

                    var nx = GetMousePosition().X;

                    if (SelectedChord != null)
                    {
                        var selectedStartTime = ProGuitarTrack.TimeToTick(
                            Utility.ScaleTimeDown(EditorPro.HScrollValue + nx));
                        if (selectedStartTime < SelectedChord.UpTick)
                        {
                            SelectedChord.DownTick = selectedStartTime;
                            EditorPro.BackupSequence();
                            UpdateChordProperties(SelectedChord, true, SelectNextEnum.ForceKeepSelection);
                        }
                    }
                    ReloadTracks(SelectNextEnum.ForceKeepSelection);
                    EditorPro.SetStatusIdle();

                }
            }
            catch { }
            return ret;
        }

        public bool DoSelelectNoteEndFromScreenPoint(TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {

                if (button8.Text == "Cancel")
                {
                    ret = true;
                    var nx = GetMousePosition().X;

                    if (SelectedChord != null)
                    {
                        var selectedEndTime = ProGuitarTrack.TimeToTick(Utility.ScaleTimeDown(EditorPro.HScrollValue + nx));
                        if (selectedEndTime > SelectedChord.DownTick)
                        {
                            SelectedChord.UpTick = selectedEndTime;
                            EditorPro.BackupSequence();
                            UpdateChordProperties(SelectedChord, true, SelectNextEnum.ForceKeepSelection);
                        }
                    }
                    ReloadTracks(SelectNextEnum.ForceKeepSelection);
                    EditorPro.SetStatusIdle();
                }
            }
            catch { }

            return ret;
        }

        public bool DoCreateChordFromScreenPoint( TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
               
                    ret = true;
                    var mouseP = GetMousePosition();


                    if (label17.Text == "Select Start")
                    {
                        createItemStartNoteEnd = ProGuitarTrack.TimeToTick(Utility.ScaleTimeDown(EditorPro.HScrollValue + mouseP.X));
                        label17.Text = "Select End Note " + createItemStartNoteEnd.ToString();
                    }
                    else if (label17.Text.StartsWith("Select End Note "))
                    {
                        EditorPro.BackupSequence();
                        var mouseTick = ProGuitarTrack.TimeToTick(Utility.ScaleTimeDown(EditorPro.HScrollValue + mouseP.X));

                        if (createItemStartNoteEnd < mouseTick)
                        {
                            EditorPro.SetStatusIdle();

                            var cb = new ChannelMessageBuilder();

                            foreach (var c
                                in ProGuitarTrack.GetChordsAtTick(createItemStartNoteEnd, mouseTick))
                            {
                                if (Utility.IsCloseTick(createItemStartNoteEnd, c.UpTick))
                                {
                                    createItemStartNoteEnd = c.UpTick;
                                }
                                else if (Utility.IsCloseTick(mouseTick, c.DownTick))
                                {
                                    mouseTick = c.DownTick;
                                }
                                else
                                {
                                    if (checkBoxAllowOverwriteChord.Checked == false)
                                    {
                                        MessageBox.Show("Note Over-write not enabled");

                                        return ret;
                                    }
                                    ProGuitarTrack.Remove(c);
                                }
                            }

                            if (checkBoxUseCurrentChord.Checked)
                            {
                                ProGuitarTrack.CreateChord(
                                    ScreenFrets, ScreenChannels,
                                    ProGuitarTrack.CurrentDifficulty,
                                    createItemStartNoteEnd, mouseTick,
                                    checkIsSlide.Checked, checkIsSlideReversed.Checked, checkIsHammeron.Checked, GetChordStrumFromScreen());
                            }
                            else
                            {
                                var y = 5 - (mouseP.Y - editor.TopLineOffset) / editor.LineSpacing;

                                var frets = new int[6]{ Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue};
                                var channels = new int[6]{ Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue};
                                frets[y] = textBoxPlaceNoteFret.Text.ToInt(0);
                                channels[y] = Utility.ChannelDefault;

                                ProGuitarTrack.CreateChord(
                                   frets, channels, ProGuitarTrack.CurrentDifficulty, 
                                            createItemStartNoteEnd, mouseTick,
                                            false, false, false, ChordStrum.Normal);
                                
                            }

                            ReloadTracks(SelectNextEnum.ForceKeepSelection);
                        }

                    }
                
                
            }
            catch { }
            return ret;
        }

        public bool DoCopyPatternSelection(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;

            try
            {
                if (statusItem != null && statusItem == label20)
                {
                    ret = true;
                    if (label20.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label20.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label20.Text.StartsWith("Select End Note "))
                    {
                        int second = label20.Text.IndexOf(" ", 16);
                        if (second == -1)
                        {
                            label20.Text += " " + gc.UpTick.ToStringEx();
                        }
                        else
                        {
                            int time = label20.Text.Substring(16, second - 16).ToInt();

                            label20.Text = "Select End Note " + time.ToString() + " " + gc.UpTick.ToStringEx();
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool DoSoloCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == label11)
                {
                    ret = true;
                    if (label11.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label11.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label11.Text.StartsWith("Select End Note "))
                    {
                        EditorPro.BackupSequence();

                        int time = label11.Text.Substring(16).ToInt();
                        if (gc.UpTick > time)
                        {
                            
                            var overlap = ProGuitarTrack.Messages.Solos.GetBetweenTick(time, gc.UpTick).Where(
                                x => x.Data1 == Utility.SoloData1).ToArray();

                            if (overlap.Length > 0)
                            {
                                MessageBox.Show("Overlapping");
                            }
                            else
                            {
                                GuitarSolo.CreateSolo(ProGuitarTrack, time, gc.UpTick);
                            }
                            EditorPro.SetStatusIdle();

                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool DoPowerupCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == label10)
                {
                    ret = true;
                    if (label10.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label10.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label10.Text.StartsWith("Select End Note "))
                    {
                        EditorPro.BackupSequence();
                        int time = (label10.Text.Substring(16)).ToInt();
                        if (gc.UpTick > time)
                        {
                            
                            var overlap = ProGuitarTrack.Messages.Powerups.GetBetweenTick(time, gc.UpTick).Where(
                                x => x.Data1 == Utility.PowerupData1).ToArray();

                            if (overlap.Length > 0)
                            {
                                MessageBox.Show("Overlapping");
                            }
                            else
                            {
                                GuitarPowerup.CreatePowerup(ProGuitarTrack, time, gc.UpTick);
                                
                            }
                            EditorPro.SetStatusIdle();

                            ReloadTracks(SelectNextEnum.ForceKeepSelection);
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool DoMultiStringTremeloCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == label16)
                {
                    ret = true;
                    if (label16.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label16.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label16.Text.StartsWith("Select End Note "))
                    {
                        EditorPro.BackupSequence();
                        int time = (label16.Text.Substring(16)).ToInt();
                        if (gc.UpTick > time)
                        {
                            var g6Track = ProGuitarTrack;

                            var overlap = ProGuitarTrack.Messages.SingleStringTremelos.GetBetweenTick(time, gc.UpTick).Where(
                                x => x.Data1 == Utility.MultiStringTremeloData1).ToArray();

                            if (overlap.Length > 0)
                            {
                                MessageBox.Show("Overlapping");
                            }
                            else
                            {
                                MidiEvent downEvent, upEvent;
                                Utility.CreateMessage(g6Track,
                                    Utility.MultiStringTremeloData1, 100, 0, time, gc.UpTick, out downEvent, out upEvent);

                                ProGuitarTrack.Messages.Add(new GuitarMultiStringTremelo(ProGuitarTrack, downEvent, upEvent));
                            }

                            EditorPro.SetStatusIdle();

                            ReloadTracks(SelectNextEnum.ForceKeepSelection);
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool DoSingleStringTremeloCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == label18)
                {
                    ret = true;
                    if (label18.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label18.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label18.Text.StartsWith("Select End Note "))
                    {
                        EditorPro.BackupSequence();
                        int time = (label18.Text.Substring(16)).ToInt();
                        if (gc.UpTick > time)
                        {
                            var g6Track = ProGuitarTrack;

                            var overlap = ProGuitarTrack.Messages.SingleStringTremelos.GetBetweenTick(time, gc.UpTick).Where(
                                x => x.Data1 == Utility.SingleStringTremeloData1).ToArray();

                            if (overlap.Length > 0)
                            {
                                MessageBox.Show("Overlapping");
                            }
                            else
                            {
                                MidiEvent downEvent, upEvent;
                                Utility.CreateMessage(g6Track,
                                    Utility.SingleStringTremeloData1, 100, 0, time, gc.UpTick, out downEvent, out upEvent);

                                ProGuitarTrack.Messages.Add(new GuitarSingleStringTremelo(ProGuitarTrack, downEvent, upEvent));
                            }
                            EditorPro.SetStatusIdle();

                            ReloadTracks(SelectNextEnum.ForceKeepSelection);
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool DoArpeggioCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == label13)
                {
                    ret = true;
                    if (label13.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        label13.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (label13.Text.StartsWith("Select End Note "))
                    {
                        int time = (label13.Text.Substring(16)).ToInt();
                        if (gc.UpTick > time)
                        {
                            CreateArpeggio(time, createItemStartNoteEnd, gc.UpTick, checkBoxCreateArpeggioHelperNotes.Checked);
                        }

                    }
                }
            }
            catch 
            {
                UndoLast();
            }
            return ret;
        }

        public bool DoTrainerCreation(GuitarChord gc, TrackEditor editor, MouseEventArgs e)
        {
            bool ret = false;
            try
            {
                if (statusItem != null && statusItem == labelProGuitarTrainerStatus)
                {
                    ret = true;
                    if (statusItem.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        statusItem.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (statusItem.Text.StartsWith("Select End Note "))
                    {
                        int downTick = (statusItem.Text.Substring(16)).ToInt();
                        if (gc.UpTick > downTick)
                        {
                            CreateTrainer(GuitarTrainerType.ProGuitar, downTick, gc.UpTick, checkTrainerLoopableProGuitar.Checked);
                        }
                    }
                }
                if (statusItem != null && statusItem == labelProBassTrainerStatus)
                {
                    ret = true;
                    if (statusItem.Text == "Select Start Note")
                    {
                        createItemStartNoteEnd = gc.UpTick;
                        statusItem.Text = "Select End Note " + gc.DownTick.ToStringEx();
                    }
                    else if (statusItem.Text.StartsWith("Select End Note "))
                    {
                        int downTick = (statusItem.Text.Substring(16)).ToInt();
                        if (gc.UpTick > downTick )
                        {
                            CreateTrainer(GuitarTrainerType.ProBass, downTick, gc.UpTick, checkTrainerLoopableProBass.Checked);
                            
                        }
                    }
                }

                
            }
            catch
            {
                
            }
            return ret;
        }

        public void CreateTrainer(GuitarTrainerType type, int downTick, int upTick, bool loopable)
        {
            try
            {
                var gt = new GuitarTrainer(ProGuitarTrack, type);
                
                gt.SetTicks(downTick, upTick, loopable);
                
                ProGuitarTrack.AddTrainer(gt);

                EditorPro.SetStatusIdle();
                RefreshTrainers();
            }
            catch { }
        }

        public void CreateArpeggio(int downTick, int firstUpTick, int upTick, bool createHelpers)
        {
            try
            {
                if (ProGuitarTrack == null)
                    return;
                
                if (upTick > firstUpTick)
                {

                    EditorPro.SetStatusIdle();

                    
                    var arpData1 = Utility.GetArpeggioData1(EditorPro.CurrentDifficulty);
                    if (!arpData1.IsNull())
                    {
                        EditorPro.BackupSequence();

                        var overlap = ProGuitarTrack.Messages.Arpeggios.GetBetweenTick(downTick, upTick).Where(
                            x => x.Data1 == arpData1).ToArray();

                        if (overlap.Length > 0)
                        {
                            MessageBox.Show("Overlapping Arpeggio");
                            return;
                        }

                        GuitarArpeggio.CreateArpeggio(ProGuitarTrack, EditorPro.CurrentDifficulty, downTick, upTick);
                        
                        if (createHelpers)
                        {
                            var msgs = ProGuitarTrack.Messages.Chords.GetBetweenTick(
                                downTick, upTick).Cast<GuitarChord>();
                            bool hasGhost = false;
                            int[] ghostNotes = new int[6] { -1, -1, -1, -1, -1, -1 };
                            foreach (var msg in msgs)
                            {
                                foreach(var note in msg.Notes)
                                {
                                    if (note.DownTick >= firstUpTick)
                                    {
                                        if (ghostNotes[note.NoteString] == -1)
                                        {
                                            hasGhost = true;
                                            ghostNotes[note.NoteString] = note.NoteFretDown;
                                        }
                                    }
                                    else
                                    {
                                        ghostNotes[note.NoteString] = -2;
                                    }
                                    
                                }
                            }
                            if (hasGhost)
                            {
                                var gc = EditorPro.GetStaleChord(downTick, firstUpTick, false);
                                if (gc != null)
                                {
                                    SetSelectedChord(gc, false, true);
                                    
                                    for (int x = 0; x < 6; x++)
                                    {
                                        if (ghostNotes[x] >= 0)
                                        {
                                            NoteChannelBoxes[5-x].Text = Utility.ChannelArpeggio.ToStringEx();
                                            GetHoldBoxes()[5-x].Text = ghostNotes[x].ToStringEx();
                                            //Utility.CreateMessage(ProGuitarTrack,Utility.GetStringLowE(GetDifficulty()) + x,100 + ghostNotes[x],Utility.ChannelArpeggio,downTick, firstUpTick);
                                        }
                                    }
                                    PlaceNote(SelectNextEnum.ForceKeepSelection);
                                    
                                }
                            }
                        }
                    }
                    EditorPro.SetStatusIdle();
                    ReloadTracks(SelectNextEnum.ForceKeepSelection);

                }
            }
            catch 
            { 
                EditorPro.RestoreBackup();
                MessageBox.Show("Error creating arpeggio");
            }
        }

        public void SetChordToScreen(GuitarChord gc, bool allowKeepSelection = true, bool ignoreKeepSelection=false)
        {
            var keepSel = (allowKeepSelection && checkKeepSelection.Checked);

            if (gc != null && (keepSel==false || ignoreKeepSelection))
            {
                
                    
                var hb = GetHoldBoxes();
                hb.ForEach(x => x.Text = "");
                NoteChannelBoxes.ForEach(x => x.Text = "");
                foreach(var n in gc.Notes)
                {
                    hb[5 - n.NoteString].Text = n.NoteFretDown.ToStringEx();
                    NoteChannelBoxes[5 - n.NoteString].Text = n.Channel.ToStringEx();
                }
                


                var sb = GetChordStartBox();
                var eb = GetChordEndBox();

                
                sb.Text = gc.DownTick.ToStringEx();
                eb.Text = gc.UpTick.ToStringEx();

                
                checkIsSlide.Checked = gc.IsSlide;
                checkIsSlideReversed.Checked = gc.IsSlideReversed;

                checkIsHammeron.Checked = gc.IsHammeron;

                checkIsTap.Checked = gc.IsTap;

                checkIsX.Checked = gc.IsXNote;

                checkStrumHigh.Checked = (gc.StrumMode & ChordStrum.High) > 0;
                checkStrumMid.Checked = (gc.StrumMode & ChordStrum.Mid) > 0;
                checkStrumLow.Checked = (gc.StrumMode & ChordStrum.Low) > 0;
            }
            else if (gc != null)
            {
                var sb = GetChordStartBox();
                var eb = GetChordEndBox();

                sb.Text = gc.DownTick.ToStringEx();
                eb.Text = gc.UpTick.ToStringEx();
            }

            int iStart = GetChordStartBox().Text.ToInt();
            int iEnd = GetChordEndBox().Text.ToInt();
            if (!iStart.IsNull() && !iEnd.IsNull())
            {
                textBox19.Text = (iEnd - iStart).ToStringEx();
            }
            
            CheckQuickEditFocus();
        }

        public ChordStrum GetChordStrumFromScreen()
        {
            ChordStrum cs = ChordStrum.Normal;
            if (checkStrumHigh.Checked)
            {
                cs |= ChordStrum.High;
            }
            if (checkStrumMid.Checked)
            {
                cs |= ChordStrum.Mid;
            }
            if (checkStrumLow.Checked)
            {
                cs |= ChordStrum.Low;
            }
            return cs;
        }

        public GuitarChord GetChordFromScreen()
        {
            return ProGuitarTrack.GetChord(ScreenFrets, ScreenChannels, ProGuitarTrack.CurrentDifficulty,
                GetChordStartTick(), GetChordEndTick(), checkIsSlide.Checked, checkIsSlideReversed.Checked, checkIsHammeron.Checked,
                GetChordStrumFromScreen());
        }

        public void ScrollToSelection()
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                EditorPro.ScrollToSelection();
            }));
            EditorPro.Invalidate();
            EditorG5.Invalidate();
        }

        public void SetSelectedChord(GuitarChord gc, bool clicked, bool ignoreKeepSelection=false)
        {
            bool wasSelected = gc == null ? false : gc.Selected;

            EditorPro.ClearSelection();
        
            if(gc != null)
            {
                
                gc.Selected = true;
                SetChordToScreen(gc, clicked || checkKeepSelection.Checked == true, ignoreKeepSelection || wasSelected);
                
                if (checkScrollToSelection.Checked && !clicked)
                {
                    ScrollToSelection();
                }
                
            }
            EditorPro.Invalidate();
        }

        void CheckQuickEditFocus()
        {
            if (checkKBQuickEdit.Checked && tabContainerMain.SelectedTab == tabNoteEditor)
            {
                foreach (var tb in GetHoldBoxes())
                {
                    if (tb.Text.Length > 0)
                    {
                        tb.Focus();
                        tb.SelectAll();
                        break;
                    }
                }
            }
            else
            {
                if (tabContainerMain.SelectedTab == tabNoteEditor)
                {
                    bool hasFocused = false;
                    foreach (var tb in GetHoldBoxes())
                    {
                        if (tb.Focused && tb.Text.Length == 0)
                        {
                            hasFocused = true;
                            break;
                        }
                    }
                    if (hasFocused)
                    {
                        foreach (var tb in GetHoldBoxes())
                        {
                            if (tb.Text.Length > 0 && tb.Focused)
                            {
                                tb.SelectAll();
                                break;
                            }
                        }
                    }
                }
            }
        }


        TextBox[] GetNoteBoxes()
        {
            return new TextBox[] { textBox2, textBox3, textBox4, textBox5, textBox6, textBox7 };
        }

        TextBox[] GetHoldBoxes()
        {
            return new TextBox[] { textBox8, textBox9, textBox10, textBox11, textBox12, textBox13 };
        }

        public int[] ScreenFrets
        {
            get
            {
                var ret = new int[6];
                var hb = GetHoldBoxes();
                for (int x = 0; x < 6; x++)
                {
                    ret[x] = hb[5 - x].Text.ToInt();
                }
                return ret;
            }
            set
            {
                var hb = GetHoldBoxes();
                for (int x = 0; x < 6; x++)
                {
                    hb[5 - x].Text = value[x].ToStringEx();
                }
            }
        }

        public int[] ScreenChannels
        {
            get
            {
                var ret = new int[6];
                var cb = NoteChannelBoxes;

                for (int x = 0; x < 6; x++)
                {
                    if (!ScreenFrets[x].IsNull())
                    {
                        ret[x] = cb[5 - x].Text.ToInt();
                    }
                    else
                    {
                        ret[x] = Int32.MinValue;
                    }
                }

                bool isXNote = false;
                for (int x = 0; x < 6; x++)
                {
                    if (!ret[x].IsNull())
                    {
                        ret[x] = ret[x].IsNull() ? 0 : ret[x];
                        if (ret[x] == Utility.ChannelX)
                        {
                            isXNote = true;
                        }
                    }
                }
                if (isXNote)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        var chan = ret[x];
                        if (!chan.IsNull())
                        {
                            ret[x] = Utility.ChannelX;
                        }
                    }
                }

                return ret;
            }
            set
            {
                var cb = NoteChannelBoxes;

                for (int x = 0; x < 6; x++)
                {
                    cb[5 - x].Text = value[x].ToStringEx();
                }
            }
        }

        TextBox GetChordStartBox()
        {
            return textBox14;
        }

        int GetChordStartTick()
        {
            return GetChordStartBox().Text.ToInt();
        }

        TextBox[] NoteChannelBoxes
        {
            get
            {
                return new TextBox[] { textBox31, textBox32, textBox33, textBox34, textBox35, textBox36 };
            }
        }

        TextBox GetChordEndBox()
        {
            return textBox20;
        }
        int GetChordEndTick()
        {
            return GetChordEndBox().Text.ToInt();
        }

        public void PlaceHeldNotesIntoHoldBoxes()
        {
            resetCount = 99;
            TextBox[] tbNote = GetNoteBoxes();
            TextBox[] tbHold = GetHoldBoxes();

            for (int x = 0; x < tbNote.Length; x++)
            {
                tbHold[x].Text = tbNote[x].Text;
            }
        }

        

        public void ClearNoteBoxes()
        {
            foreach (var tb in GetNoteBoxes())
            {
                tb.Text = "";
            }
            resetCount = resetTime;
        }
        public void ClearHoldBoxes()
        {
            foreach (var tb in GetHoldBoxes())
            {
                tb.Text = "";
            }
            resetCount = resetTime;
        }


        

        public bool HasInvalidStrumSelected()
        {

            if (Utility.WarnOnInvalidStrum != 0)
            {
                var hb = GetHoldBoxes();

                string wrn = "Warning - \"{0}\"\r\nSelect Abort to cancel adding\r\nRetry to add the note\r\nIgnore to not see this warning any more.";

                bool lowAlert = false;
                bool midAlert = false;
                bool highAlert = false;

                lowAlert = (checkStrumLow.Checked && hb[4].Text.Length == 0 && hb[5].Text.Length == 0);
                midAlert = (checkStrumMid.Checked && hb[2].Text.Length == 0 && hb[3].Text.Length == 0);
                highAlert = (checkStrumHigh.Checked && hb[0].Text.Length == 0 && hb[1].Text.Length == 0);

                if (lowAlert || midAlert || highAlert)
                {
                    string msg = string.Empty;
                    if (lowAlert)
                    {
                        msg = string.Format(wrn, "low strum marker with no low notes (Low E or A)");
                    }
                    else if (midAlert)
                    {
                        msg = string.Format(wrn, "Mid strum marker with no Mid notes (D or G)");
                    }
                    else if (highAlert)
                    {
                        msg = string.Format(wrn, "High strum marker with no High notes (B or High E)");
                    }

                    DialogResult result = MessageBox.Show(msg, "", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                    if (result == DialogResult.Abort)
                    {
                        return true;
                    }
                    else if (result == DialogResult.Ignore)
                    {
                        Utility.WarnOnInvalidStrum = 0;
                    }
                }
            }
            return false;
        }

        

        bool placingNote = false;
        public bool PlaceNote(SelectNextEnum selectNextEnum)
        {
            bool ret = false;
            
            if (placingNote)
            {
                return ret;
            }
            placingNote = true;
                
            try
            {
                EditorPro.BackupSequence();

                foreach (var sc in EditorPro.SelectedChords.ToList())
                {
                    int noteStart = GetChordStartTick();
                    int noteEnd = GetChordEndTick();

                    if (noteStart.IsNull() || noteEnd.IsNull())
                        break;
                    
                    AddChordFromScreen(ProGuitarTrack,
                        sc, ref noteStart, ref noteEnd);
                }

                HandleSelectNext(selectNextEnum);
                
                ret = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
            placingNote = false;
            return ret;
        }

        private void HandleSelectNext(SelectNextEnum selectNextEnum=SelectNextEnum.ForceKeepSelection, int lastTick=Int32.MinValue)
        {

            if (selectNextEnum == SelectNextEnum.ForceSelectNext ||
                (selectNextEnum == SelectNextEnum.UseConfiguration &&
                checkBoxAutoSelectNext.Checked))
            {
                if (lastTick.IsNull())
                    lastTick = GetChordEndTick();
                if (lastTick.IsNull() == false)
                {
                    SelectNextChord(lastTick);
                }
            }

            if (checkScrollToSelection.Checked && selectNextEnum != SelectNextEnum.ForceKeepSelection)
            {
                ScrollToSelection();
            }


            if (checkBoxClearAfterNote.Checked)
            {
                ClearHoldBoxes();
            }

            CheckQuickEditFocus();
        }

        private void AddChordFromScreen(GuitarTrack gt, GuitarChord sc, ref int noteStart, ref int noteEnd)
        {
            gt.Remove(sc);
            
            noteStart = sc.DownTick;
            noteEnd = sc.UpTick;
            var ch = GetChordFromScreen();
            if (ch != null)
            {
                var gc = ProGuitarTrack.CreateChord(ch.NoteFrets, ch.NoteChannels, ProGuitarTrack.CurrentDifficulty, noteStart, noteEnd, ch.IsSlide, ch.IsSlideReversed, ch.IsHammeron, ch.StrumMode);
                if (gc != null)
                {
                    gc.Selected = true;
                }
            }
        }

        public bool SelectNextChord(int lastUpTick = Int32.MinValue)
        {
            bool ret = false;
            try
            {
                if (EditorPro.IsLoaded == false)
                    return ret;

                if (lastUpTick.IsNull())
                {
                    if (SelectedChord == null)
                    {
                        SetSelectedChord(EditorPro.Messages.FirstChord, false);
                        ScrollToSelection();
                    }
                    else
                    {
                        SetSelectedChord(EditorPro.GetNextChord(SelectedChord), false);
                    }
                }
                else
                {
                    var sel = EditorPro.Messages.Chords.FirstOrDefault(x => x.DownTick >= lastUpTick);
                    if (sel != null)
                    {
                        SetSelectedChord(sel, false);
                    }
                }
                
                EditorPro.Invalidate();
                CheckQuickEditFocus();

                ret = EditorPro.SelectedChord != null;
            }
            catch { }
            return ret;
        }



        public void buttonSet108Events(object sender, EventArgs e)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                var genConfig = new GenDiffConfig(SelectedSong,
                            true, checkGenDiffCopyGuitarToBass.Checked,
                            checkBoxInitSelectedDifficultyOnly.Checked,
                            checkBoxInitSelectedTrackOnly.Checked,
                            true);

                if (!Set108Events(genConfig))
                {
                    MessageBox.Show("Failed");
                }
            });
        }

        public bool ClearChordNames()
        {
            bool ret = false;
            try
            {
                ProGuitarTrack.GetTrack().ChanMessages.Where(x => 
                    Utility.ChordNameEvents.Contains(x.Data1)).ToList().ForEach(x => ProGuitarTrack.Remove(x));

                ProGuitarTrack.Insert(Utility.ChordNameHiddenData1, 100, 0, 20, 21); 

                ret = ReloadTrack();
            }
            catch { }

            return ret;
        }

        public bool Set108Events(GenDiffConfig config)
        {
            bool ret = false;

            if (!EditorPro.IsLoaded)
                return ret;


            if (!config.Generate108Events)
                return true;

            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {
                    foreach (var t in EditorPro.GetTracks(GuitarTrack.TrackNames6))
                    {
                        if (EditorPro.SetTrack(t))
                        {
                            EditorPro.GuitarTrack.CreateHandPositionEvents(config);
                        }
                    }
                    ret = true;
                }
                catch { ret = false; }
            });

            return ret;
        }



        public bool UpdateChordProperties(GuitarChord gc, bool reload, SelectNextEnum selectNextEnum)
        {
            bool ret = true;
            try
            {
                ret = gc.UpdateChordProperties();

                HandleSelectNext(selectNextEnum);
                ReloadTracks();
            }
            catch { ret = false; }
            return ret;
        }


        public void UpdateSelectedChordProperties(SelectNextEnum selectNextEnum)
        {
            try
            {
                var selectedChord = EditorPro.SelectedChord;
                if (selectedChord != null)
                {
                    var st = GetChordStartTick();
                    var ed = GetChordEndTick();
                    if (ed.IsNull() || st.IsNull())
                        return;

                    foreach (var c in ProGuitarTrack.GetChordsAtTick(st, ed).ToList())
                    {
                        if (Utility.IsCloseTick(st, c.UpTick))
                        {
                            st = c.UpTick;
                            if (c.TickLength < Utility.NoteCloseWidth)
                            {
                                ProGuitarTrack.Remove(c);
                            }
                        }
                        else if (Utility.IsCloseTick(ed, c.DownTick))
                        {
                            ed = c.DownTick;
                        }
                        else
                        {
                            ProGuitarTrack.Remove(c);
                        }
                    }

                    if(ScreenFrets.Any(x=> x.IsNull()==false))
                    {
                        ProGuitarTrack.CreateChord(
                            ScreenFrets, ScreenChannels,
                            EditorPro.CurrentDifficulty,
                            st, ed,
                            checkIsSlide.Checked, 
                            checkIsSlideReversed.Checked, 
                            checkIsHammeron.Checked, 
                            GetChordStrumFromScreen());
                            
                        HandleSelectNext(selectNextEnum);
                        CheckQuickEditFocus();
                    }
                    else
                    {
                        selectedChord.Selected = false;
                        selectedChord.IsDeleted = true;
                        
                        SelectNextChord(selectedChord.UpTick);
                        
                        HandleSelectNext(SelectNextEnum.ForceSelectNext);
                    }
                    
                }
            }
            catch { }
        }


        public bool CopyTrack(string fromTrack, string toTrack)
        {
            try
            {
                var sourceTrack = TrackEditor.GetTrack(EditorPro.Sequence, fromTrack);
                if (sourceTrack == null)
                    return true;

                var destTrack = TrackEditor.GetTrack(EditorPro.Sequence, toTrack);
                if (destTrack != null)
                {
                    EditorPro.GuitarTrack.RemoveTrack(destTrack);
                }

                var t6 = TrackEditor.CopyTrack(sourceTrack, toTrack);

                EditorPro.AddTrack(t6);

                ReloadTracks();

                RefreshTracks6();
                return true;
            }
            catch { }
            return false;
        }

        Label statusItem;
        public void SetStatus(string s, Label item)
        {
            
            statusItem = item;
            toolStripCreateStatus.Text = s;
        }


        class stringObject
        {
            public object Obj;
            public string Name;

            public override string ToString()
            {
                return Name;
            }
        }


        public bool ReloadTracks(SelectNextEnum selectNextEnum = SelectNextEnum.ForceKeepSelection)
        {
            return ReloadTrack(selectNextEnum, true);

            
        }
        public bool ReloadTrackPro(SelectNextEnum selectNextEnum = SelectNextEnum.ForceKeepSelection)
        {
            return ReloadTrack(selectNextEnum, false);
        }
        public bool ReloadTrack5(SelectNextEnum selectNext = SelectNextEnum.ForceKeepSelection)
        {
            return ReloadTrack(selectNext, true);
        }

        bool reloading = false;
        public bool ReloadTrack(SelectNextEnum selectNextEnum = SelectNextEnum.ForceKeepSelection, bool reloadG5=false)
        {
            bool ret = true;

            while (reloading)
            {
                Application.DoEvents();
            }

            if (!reloading)
            {
                reloading = true;

                bool selectNext = false;
                if(selectNextEnum == SelectNextEnum.UseConfiguration)
                {
                    selectNext = checkBoxAutoSelectNext.Checked;
                }
                else if(selectNextEnum == SelectNextEnum.ForceSelectNext)
                {
                    selectNext = true;
                }
                try
                {
                    var selectedChords = EditorPro.SelectedChords;
                    var nextChord = EditorPro.GetNextChord(EditorPro.SelectedChord);

                    if (ProGuitarTrack != null)
                    {
                        EditorPro.ReloadTrack();
                        EditorG5.ReloadTrack();

                        RefreshModifierListBoxes();
                        UpdateControlsForDifficulty(EditorPro.CurrentDifficulty);
                    }

                    if (selectNext)
                    {
                        if (selectNextEnum == SelectNextEnum.UseConfiguration &&
                            checkKeepSelection.Checked)
                        {
                            var c = GetChordFromScreen();
                            SetSelectedChord(EditorPro.GetStaleChord(nextChord, true), true);
                            SetChordToScreen(c, false);
                        }
                        else
                        {
                            SetSelectedChord(EditorPro.GetStaleChord(nextChord, true), true);
                        }
                    }
                    else
                    {
                        ReloadStaleChords(selectedChords);
                    }
                }
                catch { ret = false; }

                reloading = false;
            }

            RefreshTracks();
            EditorPro.Invalidate();
            EditorG5.Invalidate();

            CheckQuickEditFocus();
            return ret;
        }

        public void ReloadStaleChords(List<GuitarChord> selectedChords)
        {
            if (selectedChords != null)
            {
                var old = new List<GuitarChord>();
                foreach (var sc in selectedChords)
                {
                    var stale = EditorPro.GetStaleChord(sc, false);
                    if (stale != null)
                    {
                        if (!old.Contains(stale))
                        {
                            old.Add(stale);
                        }
                    }
                }
                EditorPro.ClearSelection();
                foreach (var gc in old)
                {
                    gc.Selected = true;
                }
            }
        }

        private void UpdateControlsForDifficulty(GuitarDifficulty difficulty)
        {
            if (difficulty == GuitarDifficulty.Expert)
            {
                
                groupBoxStrumMarkers.Enabled = true;

                groupBoxSolo.Enabled = true;
                groupBoxPowerup.Enabled = true;
                groupBoxArpeggio.Enabled = true;
                groupBoxMultiStringTremelo.Enabled = true;
                groupBoxSingleStringTremelo.Enabled = true;
            }
            if (difficulty == GuitarDifficulty.Hard)
            {
                
                groupBoxStrumMarkers.Enabled = true;

                groupBoxSolo.Enabled = false;
                groupBoxPowerup.Enabled = false;
                groupBoxArpeggio.Enabled = true;
                groupBoxMultiStringTremelo.Enabled = false;
                groupBoxSingleStringTremelo.Enabled = false;
            }
            if (difficulty == GuitarDifficulty.Medium)
            {
                
                ClearStrumMarkers();
                groupBoxStrumMarkers.Enabled = false;

                groupBoxSolo.Enabled = false;
                groupBoxPowerup.Enabled = false;
                groupBoxArpeggio.Enabled = false;
                groupBoxMultiStringTremelo.Enabled = false;
                groupBoxSingleStringTremelo.Enabled = false;
            }
            if (difficulty == GuitarDifficulty.Easy)
            {
                
                ClearStrumMarkers();
                groupBoxStrumMarkers.Enabled = false;

                groupBoxSolo.Enabled = false;
                groupBoxPowerup.Enabled = false;
                groupBoxArpeggio.Enabled = false;
                groupBoxMultiStringTremelo.Enabled = false;
                groupBoxSingleStringTremelo.Enabled = false;
            }
        }

        

        public bool EditorsValid
        {
            get
            {
                return EditorPro.IsLoaded && EditorG5.IsLoaded;
            }
        }

        public bool CopyBigRockEnding()
        {
            var ret = true;

            if(!EditorsValid)
                return ret;
            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {

                    var bre = Utility.BigRockEndingData1;

                    var brEvent = EditorG5.Messages.BigRockEndings.Where(me => bre.Contains(me.Data1));
                    if (brEvent.Any())
                    {
                        EditorPro.Tracks.Where(x=> x.Name.IsProTrackName()).ForEach(x=>
                        {
                            if (EditorPro.SetTrack6(x, GuitarDifficulty.Expert))
                            {
                                CopyBRE(bre, brEvent);
                            }
                        });
                    }
                }
                catch { ret = false; }
            });
            return ret;
        }

        private void CopyBRE(int[] bre, IEnumerable<GuitarMessage> brEvent)
        {
            EditorPro.GuitarTrack.Remove(EditorPro.Messages.BigRockEndings.Where(
                me => bre.Contains(me.Data1)));

            GuitarBigRockEnding.CreateBigRockEnding(ProGuitarTrack, brEvent.GetMinTick(), brEvent.GetMaxTick());

            EditorPro.Invalidate();
        }

        delegate bool cmpChordFunc(GuitarChord a, GuitarChord b);
        delegate bool cmpNoteFunc(GuitarNote a, GuitarNote b);
        delegate bool matchFunc(GuitarChord[] a, GuitarChord[] b);

        bool getPatternMatchTicks(out int startTick, out int endTick)
        {
            startTick = int.MinValue;
            endTick = int.MinValue;
            bool ret = false;
            try
            {
                int firstTime = -1;
                int secondTime = -1;
                int sidx = -1;
                if (label20.Text.Length > 16)
                {
                    sidx = label20.Text.IndexOf(" ", 16);
                }
                if (sidx != -1)
                {
                    firstTime = label20.Text.Substring(16, sidx - 16).ToInt();
                    secondTime = label20.Text.Substring(sidx + 1).ToInt();
                }
                else
                {
                    if (EditorPro.SelectedChords.Count > 0)
                    {
                        firstTime = EditorPro.SelectedChords[0].DownTick;
                        secondTime = EditorPro.SelectedChords[
                            EditorPro.SelectedChords.Count - 1].UpTick;
                    }
                    else
                    {
                        return ret;
                    }
                }
                if (firstTime != -1 && secondTime != -1)
                {
                    startTick = firstTime;
                    endTick = secondTime;
                    ret = startTick.IsNull() == false && endTick.IsNull() == false;
                }
            }
            catch { }
            return ret;
        }

        class MatchingCopyPattern
        {
            public int StartTick;
            public int EndTick;
            public GuitarChord[] OriginalChords6;
            public double DeltaTimeStart;
            
            public List<GuitarChord[]> Matches;

            public MatchingCopyPattern()
            {
                Matches = new List<GuitarChord[]>();
                

            }
        }

        void dotest(GuitarChord ccc)
        {
            if (ccc.DownTick == 57600)
            {
            }
        }
        MatchingCopyPattern internalFindMatchingCopyPattern(CopyPatternPreset replaceConfig, bool findPrev)
        {
            var ret = new MatchingCopyPattern();


            bool matchG5Lengths = replaceConfig.MatchLengths5;
            bool matchG6Lengths = replaceConfig.MatchLengths6;
            bool matchSpacing = replaceConfig.MatchSpacing;
            bool matchBeat = replaceConfig.MatchBeat;

            bool replaceFirstMatchOnly = replaceConfig.FirstMatchOnly;
            int startTick, endTick;
            if (!getPatternMatchTicks(out startTick, out endTick))
                return ret;
            
            if (endTick <= startTick)
            {
                EditorPro.SetStatusIdle();
                
                return ret;
            }

            var track5 = EditorG5.GuitarTrack;
            var track6 = EditorPro.GuitarTrack;

            var chords5 = track5.Messages.Chords.ToArray();
            var nchords6 = track6.Messages.Chords.ToArray();

            var matchChords5 = chords5.GetChordsAtTick(startTick, endTick).ToArray();
            if (matchChords5.Length == 0)
                return ret;

            var oChords6 = nchords6.GetChordsAtTick(startTick, endTick).ToArray();

            if (oChords6.Length == 0)
                return ret;

            if (replaceConfig.ForwardOnly && !findPrev)
            {
                chords5 = chords5.Where(x => x.DownTick >= endTick).ToArray();
                nchords6 = nchords6.Where(x => x.DownTick >= endTick).ToArray();
            }

            
            int minMatchTick5 = matchChords5.GetMinTick();


            int minMatchTick6 = oChords6.GetMinTick();
            int maxMatchTick6 = oChords6.GetMaxTick();


            
            ret.DeltaTimeStart = track6.TickToTime(minMatchTick6) -
                track5.TickToTime(minMatchTick5);



            if (matchChords5.Any() == false)
                return ret;

            int numMatchChords5 = matchChords5.Count();
            int numMatchChords6 = oChords6.Count();


            ret.OriginalChords6 = oChords6;
            ret.StartTick = startTick;
            ret.EndTick = endTick;

            var checkStrings = new cmpNoteFunc(
                (n1, n2) =>
                {
                    return n1.NoteString != n2.NoteString;
                });
            var checkData1 = new cmpNoteFunc(
                (n1, n2) =>
                {
                    return n1.NoteFretDown != n2.NoteFretDown;
                });
            var checkLength = new cmpNoteFunc(
                (n1, n2) =>
                {
                    return Utility.IsCloseTick( n1.TickLength , n2.TickLength)==false;
                });

            var checkNull = new cmpNoteFunc((a, b) =>
            {
                return ((a == null) || (b == null));
            });

            var testBeat = new cmpChordFunc((co, cn) =>
            {
                var ob = track5.SecondsPerBeatQuarterNote(co.DownTick);
                var nb = track5.SecondsPerBeatQuarterNote(cn.DownTick);

                var diff = Math.Abs(ob - nb);
                if (diff > 0.1)
                {
                    return false;
                }

                return true;
            });
            var chordsEqual = new cmpChordFunc(
                (a, b) =>
                {
                    if (matchBeat && testBeat(a, b) == false)
                        return false;

                    for (int x = 0; x < 6; x++)
                    {
                        var an = a.Notes[x];
                        var bn = b.Notes[x];

                        if (an == null && bn == null)
                            continue;

                        if (checkNull(an, bn) ||
                            checkData1(an, bn) ||
                            checkStrings(an, bn) ||
                            (matchG5Lengths && checkLength(an, bn)))
                        {
                            return false;
                        }
                    }
                    return true;
                });

            var testSpacing = new matchFunc((mfmatch, mfmca) =>
            {
                for (int y = 0; y < mfmca.Length - 1; y++)
                {
                    var nc = mfmatch[y];
                    var oc = mfmca[y];

                    var timeTillNext5 =
                        track5.TickToTime(mfmca[y + 1].DownTick) -
                        track5.TickToTime(oc.UpTick);

                    var timeTillNext52 =
                        track5.TickToTime(mfmatch[y + 1].DownTick) -
                        track5.TickToTime(nc.UpTick);

                    var diff = Math.Abs(timeTillNext52 - timeTillNext5);
                    double spb = track5.SecondsPerBeat(oc.DownTick, GridScalar128) * 0.5;
                    if (diff > spb)
                    {
                        return false;
                    }
                }
                return true;
            });


            var mca = matchChords5.ToArray();
            var match = new GuitarChord[mca.Length];
            var matches = new List<GuitarChord[]>();
            for (int ci = 0; ci < chords5.Length - mca.Length; ci++)
            {
                var ccc = chords5[ci];
                dotest(ccc);
                for (int mc = 0; mc < mca.Length; mc++)
                {
                    var cc = chords5[ci + mc];

                    if (cc.DownTick < mca[mca.Length-1].UpTick &&
                        cc.UpTick > mca[0].DownTick)
                    {
                        break;
                    }

                    if (chordsEqual(cc, mca[mc]))
                    {
                        match[mc] = cc;
                        if (mc == mca.Length - 1)
                        {
                            if (matchSpacing)
                            {
                                if (testSpacing(match, mca) == true)
                                {
                                    matches.Add(match.ToArray());
                                    ci += mca.Length - 1;
                                }
                            }
                            else
                            {
                                matches.Add(match.ToArray());
                                ci += mca.Length - 1;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var goodMatches = new List<GuitarChord[]>();
            foreach (var m in matches)
            {
                
                if (replaceConfig.MatchLengths6)
                {
                    int minTick = m.GetMinTick();
                    int maxTick = m.GetMaxTick();

                    var c6 = track6.GetChordsAtTick(minTick, maxTick).ToArray();
                    if (c6.Length == oChords6.Length)
                    {
                        bool ok = true;
                        for (int y = 0; y < c6.Length; y++)
                        {
                            var dt1 = c6[y].DownTick;
                            var dt2 = oChords6[y].DownTick;
                            
                            double d1 = track6.TickToTimeRel(c6[y].TickLength, track6.GetTempo(dt1).Tempo);
                            double d2 = track6.TickToTimeRel(oChords6[y].TickLength, track6.GetTempo(dt1).Tempo);

                            double diff = Math.Abs(d1 - d2);

                            var spbq1 = track6.SecondsPerBeatQuarterNote(dt1);


                            if (!Utility.IsCloseTick(
                                    c6[y].TickLength, oChords6[y].TickLength))
                            {
                                var b = false;
                                if (diff > 0)
                                {
                                    double val = spbq1 / diff;
                                    if (val > 10)
                                    {
                                        b = true;
                                    }
                                }
                                if (!b)
                                {
                                    ok = false;
                                    continue;
                                }
                            }
                        }
                        if (!ok)
                            continue;
                    }
                }

                goodMatches.Add(m);
            }
            ret.Matches = goodMatches;
            return ret;
        }

        MatchingCopyPattern FindMatchingCopyPattern(bool findNext, bool findPrev, bool findFirst)
        {
            MatchingCopyPattern ret = null;

            try
            {

                var replaceConfig = GetNewCopyPatternPresetFromScreen();

                
                ret = internalFindMatchingCopyPattern(replaceConfig, findPrev);

                if (findNext || findPrev)
                {

                    var matches = ret.Matches.ToArray();

                    ret.Matches.Clear();

                    var track6 = EditorPro.GuitarTrack;

                    var mat = matches.ToArray();
                    for (int xz = 0; xz < mat.Length; xz++)
                    {
                        var i = mat[xz].ToList();
                        i.Sort((x, y) => x.DownTick > y.DownTick ? 1 : x.DownTick < y.DownTick ? -1 : 0);
                        matches[xz] = i.ToArray();
                    }

                    if (findNext)
                    {
                        var next = matches.Where(n => n.GetMinTick() > SelectedChord.DownTick);
                        if (next != null && next.Any())
                        {
                            if (replaceConfig.FirstMatchOnly)
                            {
                                ret.Matches.Add(next.FirstOrDefault());
                            }
                            else
                            {
                                foreach (var itm in next)
                                {
                                    ret.Matches.Add(itm);
                                }
                            }

                        }
                    }
                    else if (findPrev)
                    {
                        var next = matches.Where(n => n.GetMinTick() < SelectedChord.DownTick);
                        if (next != null && next.Any())
                        {
                            next.Reverse();

                            if (replaceConfig.FirstMatchOnly)
                            {
                                ret.Matches.Add(next.LastOrDefault());
                            }
                            else
                            {
                                foreach (var itm in next)
                                {
                                    ret.Matches.Add(itm);
                                }
                            }

                        }
                    }
                }
            
                
            }
            catch { }

            return ret;
        }

        bool DoReplace()
        {
            bool ret = true;

            try
            {
                var match = FindMatchingCopyPattern(false, false, false);
                if (match != null)
                {
                    var track6 = EditorPro.GuitarTrack;
                    int minTick = int.MinValue;

                    int totmat = 0;
                    foreach (var im in match.Matches)
                    {
                        totmat += im.Length;
                    }
                    if (totmat > 100)
                    {

                        var mb = MessageBox.Show(string.Format("There are currently {0} matching chords, Continue?", totmat),
                            "Warning", MessageBoxButtons.YesNo);
                        if (mb == System.Windows.Forms.DialogResult.No)
                            return true;
                    }
                    EditorPro.BackupSequence();
                    int numReplaced=0;
                    foreach (var m in match.Matches)
                    {
                        int trep;
                        minTick = ReplaceNotes(track6, match.OriginalChords6, match.DeltaTimeStart,
                                minTick, m,
                                m.GetMinTick(), m.GetMaxTick(), out trep);
                        
                        
                        numReplaced += trep;
                        
                    }

                    if (numReplaced == 0)
                    {
                        MessageBox.Show("No Matches found");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Replaced {0} Matches", numReplaced));
                    }

                    EditorPro.ClearSelection();
                    EditorPro.GuitarTrack.GetChordsAtTick(match.OriginalChords6.GetMinTick(), match.OriginalChords6.GetMaxTick()).ToList().ForEach(x => x.Selected = true);
                }
                
            }
            catch { ret = false; }
            EditorPro.Invalidate();
            return ret;
        }


        void ReplaceMatch()
        {
            
            if (!EditorsValid)
            {
                EditorPro.SetStatusIdle();
                return;
            }
            try
            {

                if (DoReplace())
                {
                    HandleSelectNext();
                }

            }
            catch
            {
                UndoLast();
            }
            
            EditorPro.SetStatusIdle();
        }

        private int ReplaceNotes(GuitarTrack track6, GuitarChord[] oChords6, 
            double deltaTimeStart, 
            int minTime, GuitarChord[] m, 
            int startTick, int endTick, out int numReplaced)
        {
            numReplaced = 0;
            

            var replaceConfig = GetNewCopyPatternPresetFromScreen();


            if (replaceConfig.KeepLengths &&
                m.Length == oChords6.Length)
            {

                if (replaceConfig.RemoveExisting)
                {
                    track6.Remove(EditorPro.GuitarTrack.GetChordsAtTick(
                        m.GetMinTick() + Utility.NoteCloseWidth,
                        m.GetMaxTick() - Utility.NoteCloseWidth));
                }


                int iochord6 = 0;
                foreach (var oC in oChords6)
                {
                    var nn = oC.CloneAtTime(
                        track6,
                        m[iochord6].DownTick, 
                        m[iochord6].UpTick);
                    if (nn != null)
                    {
                        numReplaced++;
                        iochord6++;

                        nn.Selected = true;
                    }
                }
            }
            else
            {
                GuitarChord prev = null;
                var currentTime = track6.TickToTime(startTick);
                int currentTick = track6.TimeToTick(currentTime);


                foreach (var oC in oChords6)
                {
                    int space = 0;
                    if(prev != null)
                    {
                        space = oC.DownTick - prev.UpTick;
                    }

                    int newStartTick;
                    int newEndTick;
                    CalcStartEndTick(oC.DownTick, oC.UpTick, space, currentTick, currentTime, deltaTimeStart,
                        out newStartTick, out newEndTick);

                    if (!(newStartTick < oC.UpTick && newEndTick > oC.DownTick))
                    {

                        if (newStartTick >= minTime)
                        {

                            if (replaceConfig.RemoveExisting)
                            {
                                ProGuitarTrack.Remove( ProGuitarTrack.GetChordsAtTick(
                                    m.GetMinTick() + Utility.NoteCloseWidth,
                                    m.GetMaxTick() - Utility.NoteCloseWidth));
                            }
                            var nn = oC.CloneAtTime(track6,newStartTick, newEndTick);
                            if(nn != null)
                            {
                                endTick = nn.UpTick;
                                numReplaced++;
                                nn.Selected = true;
                            }
                            minTime = endTick;
                            deltaTimeStart = 0;
                        }
                       
                    }
                    prev = oC;
                    currentTime = track6.TickToTime(minTime);
                }
            }
            ReloadTracks();
            return minTime;
        }

        void CalcStartEndTick(int oDownTick, int oUpTick, int spaceTick,
            int currentTick, double currentTime, double deltaTimeStart,
            out int newStartTick, out int newEndTick)
        {
            var track6 = EditorPro.GuitarTrack;

            var onoteStart = track6.TickToTime(oDownTick);
            var onoteEnd = track6.TickToTime(oUpTick);

           
            var secs = track6.SecondsPerBeatQuarterNote(
                oDownTick);

            var secs2 = track6.SecondsPerBeatQuarterNote(
                currentTick);


            var deltaTime = track6.TickToTimeRel(spaceTick, track6.GetTempo(currentTick).Tempo);
            //*secs2;
            currentTime += deltaTime;
            currentTick = track6.TimeToTick(currentTime);
            double deltaBeatStart = secs2 / secs;

            var st = currentTime +
                deltaTimeStart * deltaBeatStart;
            newStartTick = track6.TimeToTick(st);

            var onoteLenTime = onoteEnd - onoteStart;

            var et = st +
                (onoteLenTime * secs2 / secs);
            newEndTick = track6.TimeToTick(et);
        }

        public bool CombineNextNote()
        {

            try
            {
                var sn = SelectedChord;
                if (sn != null)
                {
                    var nn = EditorPro.GetNextChord(sn);

                    if (nn != null)
                    {
                        sn.UpTick = nn.UpTick;

                        UpdateChordProperties(sn, true, SelectNextEnum.ForceKeepSelection);
                    }
                }
                EditorPro.Invalidate();
                return true;
            }
            catch { }
            return false;
        }


        public void AddSlideHOPO()
        {
            try
            {
                var clist = EditorPro.SelectedChords;
                foreach (var c in clist)
                {
                    var t = ProGuitarTrack;

                    c.RemoveSlide();
                    c.AddSlide( false);

                    var c2 = EditorPro.GetNextChord(c);

                    c2.RemoveHammeron();
                    c2.AddHammeron();
                }
                EditorPro.Invalidate();
            }
            catch { }
        }

        public void AddSlideNote()
        {
            try
            {

                foreach (var c in EditorPro.SelectedChords)
                {
                    var t = ProGuitarTrack;

                    c.RemoveSlide();
                    c.AddSlide( false);

                }

                EditorPro.Invalidate();
            }
            catch { }
        }

        public void AddHOPONote()
        {
            try
            {
                foreach (var c in EditorPro.SelectedChords)
                {
                    var t = ProGuitarTrack;

                    c.RemoveHammeron();
                    c.AddHammeron();
                }

                EditorPro.Invalidate();
            }
            catch { }
        }


        void ClearStrumMarkers()
        {
            checkStrumHigh.Checked = false;
            checkStrumMid.Checked = false;
            checkStrumLow.Checked = false;
        }


        public bool CopySolosFromG5(bool alltracks)
        {
            bool ret = true;
            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {
                    var ot = EditorPro.MidiTrack;
                    if (!alltracks)
                    {
                        CopySoloDataForCurrentTrack();
                    }
                    else
                    {
                        foreach (var tr in EditorPro.Tracks)
                        {
                            if (tr.Name.IsGuitarTrackName())
                            {
                                EditorG5.SetTrack5(EditorG5.GetGuitar5MidiTrack());
                                EditorPro.SetTrack6(tr);
                                CopySoloDataForCurrentTrack();
                            }
                            else if (tr.Name.IsBassTrackName())
                            {
                                EditorG5.SetTrack5(EditorG5.GetGuitar5BassMidiTrack());
                                EditorPro.SetTrack6(tr);
                                CopySoloDataForCurrentTrack();
                            }
                        }
                    }
                }
                catch
                {
                    ret = false;
                }
            });
            return ret;
        }

        public bool CopySoloDataForCurrentTrack()
        {
            bool ret = true;
            try
            {
                ProGuitarTrack.Messages.Solos.ToList().ForEach(x => ProGuitarTrack.Remove(x));

                EditorG5.Messages.Solos.ForEach(x =>
                    {
                        GuitarSolo.CreateSolo(ProGuitarTrack, x.DownTick, x.UpTick);
                    });
            }
            catch 
            {
                ret = false;
            }
            return ret;
        }

        public bool CopyPowerupDataForCurrentTrack()
        {
            bool ret = true;
            try
            {
                ProGuitarTrack.Messages.Powerups.ToList().ForEach(x => ProGuitarTrack.Remove(x));

                EditorG5.Messages.Powerups.ForEach(x =>
                    GuitarPowerup.CreatePowerup(ProGuitarTrack, x.DownTick, x.UpTick));

                
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public void StoreScreenChord()
        {
            var sc = GetStoredChordFromScreen();

            listBoxStoredChords.Items.Add(sc);
            listBoxStoredChords.SelectedItem = sc;
        }

        
        public StoredChord GetStoredChordFromScreen()
        {
            var sc = new StoredChord();
            sc.Notes = new int[6];
            sc.NoteChannels = new int[6];
            var hb = GetHoldBoxes();
            
            for (int x = 0; x < hb.Length; x++)
            {
                var note = hb[x].Text;
                int iNote = note.ToInt(-1);
                
                sc.Notes[x] = iNote;
                

                if (sc.Notes[x] != -1)
                {
                    sc.NoteChannels[x] = NoteChannelBoxes[x].Text.ToInt(-1);
                }
                else
                {
                    sc.NoteChannels[x] = -1;
                }
            }
            sc.IsSlide = checkIsSlide.Checked;
            sc.IsSlideRev = checkIsSlideReversed.Checked;
            sc.IsHammeron = checkIsHammeron.Checked;
            sc.IsTap = checkIsTap.Checked;
            sc.IsXNote = checkIsX.Checked;

            sc.Strum = ChordStrum.Normal;
            if (checkStrumHigh.Checked)
            {
                sc.Strum |= ChordStrum.High;
            }
            if (checkStrumMid.Checked)
            {
                sc.Strum |= ChordStrum.Mid;
            }
            if (checkStrumLow.Checked)
            {
                sc.Strum |= ChordStrum.Low;
            }
            sc.TickLength = textBox19.Text.ToInt(Utility.MinimumNoteWidth + 5);
            
            return sc;
        }



        public void UpdateScreenChord()
        {
            if (listBoxStoredChords.SelectedItem != null)
            {
                var sc = GetStoredChordFromScreen();

                listBoxStoredChords.Items[listBoxStoredChords.SelectedIndex] = sc;
            }
        }


        StoredChord GetSelectedStoredChord()
        {
            return listBoxStoredChords.SelectedItem as StoredChord;
        }


        public void SetStoredChordToEditor(StoredChord sc)
        {
            if (sc == null)
                return;

            var hb = GetHoldBoxes();
            
            for (int x = 0; x < hb.Length; x++)
            {
                var n = sc.Notes[x];
                if (n == -1)
                    hb[x].Text = "";
                else
                    hb[x].Text = n.ToString();

                var ch = sc.NoteChannels[x];
                if (ch != -1)
                {
                    NoteChannelBoxes[x].Text = ch.ToString();
                }
                else if (hb[x].Text.Length > 0)
                {
                    if (ch != -1)
                    {
                        NoteChannelBoxes[x].Text = ch.ToString();
                    }
                    else
                    {
                        NoteChannelBoxes[x].Text = "0";
                    }
                }
            }
            checkIsSlide.Checked = sc.IsSlide;
            checkIsSlideReversed.Checked = sc.IsSlideRev;
            checkIsHammeron.Checked = sc.IsHammeron;
            checkIsTap.Checked = sc.IsTap;
            checkIsX.Checked = sc.IsXNote;


            checkStrumHigh.Checked = (sc.Strum & ChordStrum.High) > 0;
            checkStrumMid.Checked = (sc.Strum & ChordStrum.Mid) > 0;
            checkStrumLow.Checked = (sc.Strum & ChordStrum.Low) > 0;

            GetChordEndBox().Text = "";

            textBox19.Text = sc.TickLength.ToStringEx();
            int sp = GetChordStartBox().Text.ToInt();

            if (sp.IsNull()==false)
            {
                GetChordEndBox().Text = (sp + sc.TickLength).ToStringEx();
            }
            
            var gc = GetChordFromScreen();
            if (gc != null)
            {
                EditorPro.SetChordToClipboard(gc);
            }
        }


        public void ExtendToNextNote()
        {
            try
            {
                var clist = EditorPro.SelectedChords;
                foreach (var sn in clist)
                {
                    var nn = EditorPro.GetNextChord(sn);

                    if (nn != null)
                    {
                        sn.UpTick = nn.DownTick;
                        sn.UpdateChordProperties();
                    }
                }
                EditorPro.Invalidate();
            }
            catch { }
        }


        public void SplitNote()
        {

            try
            {
                foreach(var sn in EditorPro.SelectedChords)
                {
                    var start = sn.DownTick;
                    var end = sn.UpTick;

                    if (end != -1 && start != -1)
                    {
                        var len = (end - start);

                        var nend = (start + len / 2 - 1);

                        sn.UpTick = nend;

                        UpdateChordProperties(sn, false, SelectNextEnum.ForceKeepSelection);

                        sn.CloneAtTime(EditorPro.GuitarTrack,
                            nend, end);
                    }
                }

                EditorPro.Invalidate();
            }
            catch { }
        }



        void CreateArpeggioFromSelection()
        {
            try
            {
                if (EditorPro.SelectedChords.Count > 1)
                {
                    CreateArpeggio(EditorPro.SelectedChords.GetMinTick(),
                        EditorPro.SelectedChords[0].UpTick,
                        EditorPro.SelectedChords.GetMaxTick(),
                        checkBoxCreateArpeggioHelperNotes.Checked);
                }
            }
            catch { }
        }

        bool HandleKeys(KeyEventArgs e)
        {
            if (EditorPro.IsLoaded == false)
            {
                return false;
            }

            if (tabContainerMain.SelectedTab != null &&
                tabContainerMain.SelectedTab == tabModifierEditor)
            {
                if (e.KeyValue == 189)//minus
                {
                    ZoomOut();
                    return true;
                }
                if (e.KeyValue == 187)//plus
                {
                    ZoomIn();
                    return true;
                }

                if (e.KeyCode == Keys.S && !e.Control)
                {
                    SoloCreateStart();
                    return true;
                }
                else if (e.KeyCode == Keys.S && e.Control)
                {
                    return false;
                }
                else if (e.KeyCode == Keys.P)
                {
                    PowerupCreateStart();
                    return true;
                }
                
                else if (e.KeyCode == Keys.A)
                {
                    if (EditorPro.SelectedChords.Count > 1)
                    {
                        CreateArpeggioFromSelection();
                    }
                    else
                    {
                        ArpeggioCreateStart();
                    }
                    return true;
                }
                else if (e.KeyCode == Keys.H)
                {
                    checkBoxCreateArpeggioHelperNotes.Checked = !checkBoxCreateArpeggioHelperNotes.Checked;
                    return true;
                }
                else if (e.KeyCode == Keys.M)
                {
                    BeginMultiStringTremeloCreate();
                    return true;
                }
                else if (e.KeyCode == Keys.T)
                {
                    BeginSingleStringTremeloCreate();
                    return true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    EditorPro.SetStatusIdle();
                    return true;
                }
                if (e.KeyCode == Keys.Z && e.Control && !e.Shift)
                {
                    UndoLast(true);

                    return true;
                }
            }
            else if (IsTabEventsActive)
            {
                
            }
            else if (tabContainerMain.SelectedTab != null &&
                tabContainerMain.SelectedTab == tabNoteEditor )
            {
                if (textBox19.ContainsFocus || textBox20.ContainsFocus || textBox14.ContainsFocus || textBox31.ContainsFocus ||textBox32.ContainsFocus ||textBox33.ContainsFocus || textBox34.ContainsFocus || textBox35.ContainsFocus || textBox36.ContainsFocus
                    || textBoxPlaceNoteFret.ContainsFocus || comboNoteEditorCopyPatternPreset.ContainsFocus)
                {
                    if (comboNoteEditorCopyPatternPreset.ContainsFocus)
                        return false;

                    if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 || e.KeyCode == Keys.D3 ||
                        e.KeyCode == Keys.D4 || e.KeyCode == Keys.D5 || e.KeyCode == Keys.D6 ||
                        e.KeyCode == Keys.D7 || e.KeyCode == Keys.D8 || e.KeyCode == Keys.D9 ||
                        e.KeyCode == Keys.D0 || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
                    {
                        return false;
                    }
                }

                else if (e.KeyCode == Keys.Delete)
                {
                    ClearHoldBoxes();
                    PlaceNote(SelectNextEnum.UseConfiguration);
                    return true;
                }
                else if (e.KeyValue == 189)//minus
                {
                    ZoomOut();
                    return true;
                }
                else if (e.KeyValue == 187)//plus
                {
                    ZoomIn();
                    return true;
                }
                else if (e.KeyCode == Keys.A && e.Control)
                {
                    
                    EditorPro.ClearSelection();
                    EditorPro.Messages.Chords.All(x => x.Selected = true);
                    EditorPro.Invalidate();
                    return true;
                }
                else if (e.KeyCode == Keys.W)
                {
                    EditorPro.BackupSequence();
                    AddSlideNote();
                    return true;
                }
                else if (e.KeyCode == Keys.Q)
                {
                    EditorPro.BackupSequence();
                    AddHOPONote();
                    return true;
                }
                else if (e.KeyCode == Keys.E)
                {
                    EditorPro.BackupSequence();
                    AddSlideHOPO();
                    return true;
                }
                else if (e.KeyCode == Keys.H)
                {
                    checkIsHammeron.Checked = checkIsHammeron.Checked ? false : true;
                    return true;
                }
                else if (e.KeyCode == Keys.S && !e.Control)
                {
                    checkIsSlide.Checked = checkIsSlide.Checked ? false : true;
                    return true;
                }
                else if (e.KeyCode == Keys.R)
                {
                    checkIsSlideReversed.Checked = checkIsSlideReversed.Checked ? false : true;
                    return true;
                }
                else if (e.KeyCode == Keys.X && e.Control)
                {
                    EditorPro.BackupSequence();
                    EditorPro.SetSelectedToClipboard();
                    EditorPro.RemoveSelectedNotes();

                    return true;
                }
                else if (e.KeyCode == Keys.X)
                {
                    checkIsX.Checked = checkIsX.Checked ? false : true;

                    return true;
                }
                else if (e.KeyCode == Keys.C && e.Control)
                {
                    EditorPro.SetSelectedToClipboard();

                    return true;
                }
                else if (e.KeyCode == Keys.C)
                {
                    ClearHoldBoxes();
                    ClearNoteBoxes();
                    

                    return true;
                }
                else if (e.KeyCode == Keys.V && e.Control)
                {
                    if (EditorPro.CopyChords.Count() > 0)
                    {
                        if (EditorPro.CurrentSelectionState != TrackEditor.EditorSelectionState.PastingNotes &&
                            EditorPro.CurrentSelectionState != TrackEditor.EditorSelectionState.MovingNotes)
                        {
                            EditorPro.CurrentSelectionState = TrackEditor.EditorSelectionState.PastingNotes;
                        }
                        else
                        {
                            EditorPro.PasteCopyBuffer(EditorPro.CurrentPastePoint);
                            
                        }
                    }
                    
                    EditorPro.Invalidate();
                    return true;
                }
                else if (e.KeyCode == Keys.V)
                {
                    
                    PlaceNote(SelectNextEnum.UseConfiguration);

                    return true;
                }
                else if (e.KeyCode == Keys.K)
                {
                    checkKeepSelection.Checked = checkKeepSelection.Checked ? false : true;

                    return true;
                }
                else if (e.KeyCode == Keys.T)
                {
                    checkIsTap.Checked = checkIsTap.Checked ? false : true;

                    return true;
                }
                
                else if (e.KeyCode == Keys.N)
                {
                    SelectNextChord();

                    return true;
                }
                else if (e.KeyCode == Keys.B)
                {
                    SelectPreviousChord();
                    return true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (EditorPro.CurrentSelectionState == TrackEditor.EditorSelectionState.MovingNotes)
                    {
                        UndoLast();
                    }
                    EditorPro.SetStatusIdle();
                    return true;
                }
                else if (e.KeyCode == Keys.P)
                {

                    if (label20.Text == "Idle")
                    {
                        BeginCopyPattern();
                    }
                    else
                    {
                        
                        ReplaceMatch();
                    }

                    return true;
                }

                if (e.KeyCode == Keys.Right)
                {
                    if (e.Shift == true)
                        button35_Click(null, null);
                    else
                        button37_Click(null, null);
                    var hb = GetHoldBoxes();
                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (hb[x].Focused)
                        {
                            hb[x].SelectAll();
                            break;
                        }
                    }

                    return true;
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (e.Shift == true)
                        button36_Click(null, null);
                    else
                        button38_Click(null, null);
                    var hb = GetHoldBoxes();
                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (hb[x].Focused)
                        {
                            hb[x].SelectAll();
                            break;
                        }
                    }

                    return true;
                }
                if (e.KeyCode == Keys.Up && e.Alt)
                {
                    var n = listBoxStoredChords.SelectedIndex;
                    if (n == -1 && listBoxStoredChords.Items.Count > 0)
                    {
                        listBoxStoredChords.SelectedIndex = 0;
                    }
                    else if (n > 0)
                    {
                        listBoxStoredChords.SelectedIndex = n - 1;
                    }
                    else if (n == 0)
                    {
                        listBoxStoredChords.SelectedIndex = -1;
                        listBoxStoredChords.SelectedIndex = 0;
                    }
                    return true;
                }
                if (e.KeyCode == Keys.Up && !e.Alt)
                {
                    var hb = GetHoldBoxes();

                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (x > 0 && hb[x].Focused)
                        {
                            hb[x - 1].Focus();
                            hb[x - 1].SelectAll();
                            break;
                        }
                    }
                    if (!e.Control)
                    {
                        button33_Click(null, null);
                    }

                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (hb[x].Focused)
                        {
                            hb[x].SelectAll();
                            break;
                        }
                    }

                    return true;
                }
                if (e.KeyCode == Keys.Down && e.Alt)
                {
                    var n = listBoxStoredChords.SelectedIndex;
                    if (n == -1 && listBoxStoredChords.Items.Count > 0)
                    {
                        listBoxStoredChords.SelectedIndex = 0;
                    }
                    else if (n != -1)
                    {
                        if (n < listBoxStoredChords.Items.Count - 1)
                        {
                            listBoxStoredChords.SelectedIndex = n + 1;
                        }
                        else
                        {
                            listBoxStoredChords.SelectedIndex = -1;
                            listBoxStoredChords.SelectedIndex = n;
                        }
                    }
                    return true;
                }
                if (e.KeyCode == Keys.Down && !e.Alt)
                {
                    var hb = GetHoldBoxes();
                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (x < hb.Length - 1 && hb[x].Focused)
                        {
                            hb[x + 1].Focus();
                            hb[x + 1].SelectAll();
                            break;
                        }
                    }
                    if (!e.Control)
                    {
                        button34_Click(null, null);
                    }

                    for (int x = 0; x < hb.Length; x++)
                    {
                        if (hb[x].Focused)
                        {
                            hb[x].SelectAll();
                            break;
                        }
                    }

                    return true;
                }
                if (e.KeyCode == Keys.U)
                {
                    checkStrumHigh.Checked = checkStrumHigh.Checked ? false : true;

                    return true;
                }
                if (e.KeyCode == Keys.I)
                {
                    checkStrumMid.Checked = checkStrumMid.Checked ? false : true;

                    return true;
                }

                if (e.KeyCode == Keys.L)
                {
                    checkStrumLow.Checked = checkStrumLow.Checked ? false : true;

                    return true;
                }
                if (e.KeyCode == Keys.Y)
                {
                    buttonStrumNone_Click(null, null);

                    return true;
                }
                if (e.KeyCode == Keys.J)
                {
                    checkBoxClearAfterNote.Checked = checkBoxClearAfterNote.Checked ? false : true;

                    return true;
                }
                if (e.KeyCode == Keys.G)
                {
                    StoreScreenChord();

                    return true;
                }
                if (e.KeyCode == Keys.Z && e.Control && !e.Shift)
                {
                    UndoLast(true);

                    return true;
                }
                if (e.KeyCode == Keys.Z && e.Control && e.Shift)
                {
                    EditorPro.RedoBackup();

                    return true;
                }
                if (e.KeyCode == Keys.D1 ||
                    e.KeyCode == Keys.D2 ||
                    e.KeyCode == Keys.D3 ||
                    e.KeyCode == Keys.D4 ||
                    e.KeyCode == Keys.D5 ||
                    e.KeyCode == Keys.D6 ||
                    e.KeyCode == Keys.D7 ||
                    e.KeyCode == Keys.D8 ||
                    e.KeyCode == Keys.D9 ||
                    e.KeyCode == Keys.D0)
                {
                    var hb = GetHoldBoxes();
                    TextBox focusBox = null;
                    foreach (var b in hb)
                    {
                        if (b.Focused)
                        {
                            focusBox = b;
                            break;
                        }
                    }
                    if (focusBox == null)
                    {
                        foreach (var b in hb)
                        {
                            if (b.Text.Length > 0)
                            {
                                focusBox = b;
                                break;
                            }
                        }
                        if (focusBox == null)
                        {
                            if (SelectedChord != null && SelectedChord.HasNotes)
                            {
                                
                                for (int x = 0; x < SelectedChord.Notes.Count(); x++)
                                {
                                    if (SelectedChord.Notes[x] != null)
                                    {
                                        focusBox = hb[5-x];
                                        break;
                                    }
                                }
                                
                            }
                            
                            if(focusBox == null)
                            {
                                focusBox = hb[0];
                            }
                        }
                        focusBox.Focus();
                        focusBox.SelectAll();

                    }


                    var t = focusBox.Text;
                    if (t.ToInt().IsNull() && focusBox.Text.Length > 0)
                    {
                        focusBox.Text = "";
                        return true;
                    }
                    else if(focusBox.Text.Length == 0)
                    {
                        focusBox.Text = ((char)e.KeyValue).ToString();
                        focusBox.SelectionStart = 1;
                        return true;
                    }
                    
                }
            }
            return false;
        }


        

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!DesignMode)
            {
                KeyEventArgs k = new KeyEventArgs(keyData);
                if (HandleKeys(k) == true)
                {
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

       


        public bool SelectPreviousChord()
        {
            bool ret = false;
            try
            {
                if (EditorPro.IsLoaded == false)
                    return ret;

                if (EditorPro.SelectedChord == null)
                {
                    SetSelectedChord(EditorPro.Messages.LastChord, false);
                    ScrollToSelection();
                }
                else
                {

                    SetSelectedChord(EditorPro.GetPreviousChord(SelectedChord), false);
                    
                }

                EditorPro.Invalidate();
                CheckQuickEditFocus();

                ret = SelectedChord != null;
            }
            catch{}

            return ret;
        }

        public TextBox[] HoldBoxes
        {
            get
            {
                return GetHoldBoxes();
            }

        }
        void CheckNoteEntry(int hbIndex)
        {
            var i = HoldBoxes[hbIndex].Text.ToInt();
            if (i.IsNull() || i < 0 || i > 23)
            {
                NoteChannelBoxes[hbIndex].Text = "";    
                HoldBoxes[hbIndex].Text = "";
            }

        }

        public void onChangeString6(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(0);
        }

        public void onChangeString5(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(1);
        }

        public void onChangeString4(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(2);
        }

        public void onChangeString3(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(3);
        }

        public void onChangeString2(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(4);
        }

        public void onChangeString1(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNoteEntry(5);
        }

        
        public void UndoLast(bool allowRedo = false)
        {
            try
            {
                if (allowRedo &&
                    EditorPro.NumBackups > 0)
                {
                    EditorPro.CreateRedoBackup();
                }
                EditorPro.RestoreBackup();
            }
            catch
            {
                EditorPro.RestoreBackup();
            }
        }


        public void RemoveAllArpegios()
        {
            try
            {

                if (!EditorPro.IsLoaded)
                    return;

                if (Utility.GetArpeggioData1(EditorPro.CurrentDifficulty).IsNull())
                    return;


                foreach (var a in ProGuitarTrack.Messages.Arpeggios.ToList())
                {
                    foreach (var ch in ProGuitarTrack.GetChordsAtTick(a.DownTick, a.UpTick).Where(x => x.Notes.Any(n => n.IsArpeggioNote)).ToList())
                    {
                        foreach (var n in ch.Notes.Where(x=> x.IsArpeggioNote).ToList())
                        {
                            ch.RemoveNote(n);
                        }
                        if (!ch.HasNotes)
                        {
                            ProGuitarTrack.Remove(ch);
                        }
                    }
                    ProGuitarTrack.Remove(a);
                }
                RefreshModifierListBoxes();
            }
            catch { }
        }


        public void RemoveAllStrum()
        {
            try
            {
                var gt = ProGuitarTrack;
                if (gt == null)
                    return;

                if (gt.Messages == null)
                    return;

                foreach (var c in gt.Messages.Chords)
                {
                    if (c.HasStrum)
                    {
                        c.RemoveStrum();
                    }
                }

                
            }
            catch { }
        }


        public void CopyG5Tempo()
        {
            try
            {
                if (!EditorsValid)
                    return;
                EditorPro.BackupSequence();
                var gt2 = ProGuitarTrack;
                var gt1 = EditorG5.GuitarTrack;
                if (gt2 != null && gt1 != null)
                {
                    var gt2Tempo = BuildTempo(gt1);
                    if (gt2Tempo != null)
                    {
                        var tempo = gt2.TempoTrack;

                        if (tempo != null)
                        {
                            gt2.RemoveTrack(tempo);
                        }

                        gt2.AddTempoTrack(gt2Tempo);
                    }
                }
            }
            catch { }
        }



        public bool CopyDifficulties()
        {
            try
            {

                if (EditorPro.IsLoaded == false)
                    return false;

                var selDiff = EditorPro.SelectedTrackDifficulty;

                var selectedDiffOnly = checkBoxInitSelectedDifficultyOnly.Checked;
                var diff = EditorPro.CurrentDifficulty;
                
                
                var copyDiffs = GuitarDifficulty.Unknown;

                if (selectedDiffOnly)
                {
                    if (diff == GuitarDifficulty.Expert)
                    {
                        copyDiffs = GuitarDifficulty.EasyMediumHard;
                    }
                    else if (diff == GuitarDifficulty.Hard)
                    {
                        copyDiffs = GuitarDifficulty.EasyMedium;
                    }
                    else if (diff == GuitarDifficulty.Medium)
                    {
                        copyDiffs = GuitarDifficulty.Easy;
                    }
                }
                else
                {
                    copyDiffs = GuitarDifficulty.EasyMediumHard;
                }

                if (copyDiffs.IsUnknown())
                    return true;

                EditorPro.GuitarTrack.GetTrack().RemoveDifficulty(copyDiffs);
                
                if (copyDiffs.HasFlag(GuitarDifficulty.Hard))
                {
                    ProGuitarTrack.GetDifficulty(GuitarDifficulty.Expert).ForEach(x =>
                        ProGuitarTrack.Insert(x.AbsoluteTicks, x.ConvertDifficultyPro(GuitarDifficulty.Hard)));
                }
                if (copyDiffs.HasFlag(GuitarDifficulty.Medium))
                {
                    ProGuitarTrack.GetDifficulty(GuitarDifficulty.Hard).ForEach(x =>
                        ProGuitarTrack.Insert(x.AbsoluteTicks, x.ConvertDifficultyPro(GuitarDifficulty.Medium)));
                }
                if (copyDiffs.HasFlag(GuitarDifficulty.Easy))
                {
                    ProGuitarTrack.GetDifficulty(GuitarDifficulty.Medium).ForEach(x =>
                        ProGuitarTrack.Insert(x.AbsoluteTicks, x.ConvertDifficultyPro(GuitarDifficulty.Easy)));
                }

                EditorPro.SelectedTrackDifficulty = selDiff;
            }
            catch { return false; }
            return true;
        }


        



        public string DTAGetSongShortName(byte[] upgradesdta)
        {
            string ret = string.Empty;
            using (var sr = new StreamReader(new MemoryStream(upgradesdta)))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Trim(' ', '(', ')', '\'');
                    var c = line.IndexOf(';');
                    if (c != -1)
                    {
                        line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                    }
                    if (line.Length == 0)
                        continue;

                    ret = line;
                    break;
                }
            }
            return ret;
        }

        public string DTAGetSongID(byte[] upgradesdta)
        {
            string ret = string.Empty;
            using (var sr = new StreamReader(new MemoryStream(upgradesdta)))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Trim(' ', '(', ')', '\'');

                    if (line.Length == 0)
                        continue;

                    var c = line.IndexOf(';');
                    if (c != -1)
                    {
                        line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                    }
                    string match = "song_id";
                    if (line.StartsWith(match, StringComparison.OrdinalIgnoreCase))
                    {
                        return line.Substring(match.Length).Trim();
                    }
                }
            }
            return ret;
        }
        public string DTAGetProFileName(byte[] upgradesdta)
        {
            string ret = string.Empty;
            using (var sr = new StreamReader(new MemoryStream(upgradesdta)))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Trim(' ', '(', ')', '\'');

                    if (line.Length == 0)
                        continue;

                    var c = line.IndexOf(';');
                    if (c != -1)
                    {
                        line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                    }
                    string match = "midi_file";
                    if (line.StartsWith(match, StringComparison.OrdinalIgnoreCase))
                    {
                        return line.Substring(match.Length).Trim();
                    }
                }
            }
            return ret;
        }

        public bool AddPackageTreeNodes(TreeNodeCollection treeNode, PackageFolder folder)
        {
            bool ret = true;
            try
            {
                TreeNodeCollection nc = treeNode;
                if (string.IsNullOrEmpty(folder.Name) == false)
                {
                    var n = nc.Add(folder.Name);
                    n.Tag = folder;
                    nc = n.Nodes;
                    n.ImageKey = "XPFolder.gif";
                    n.SelectedImageKey = "OpenFolder.gif";
                    
                }


                foreach (PackageFile f in folder.Files)
                {
                    var fn = nc.Add(f.Name);
                    fn.Tag = f;



                    var ext = Path.GetExtension(f.Name);
                    if (ext.EndsWith("dta", StringComparison.OrdinalIgnoreCase) ||
                        ext.EndsWith("mid", StringComparison.OrdinalIgnoreCase) ||
                        ext.EndsWith("midi", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ext.EndsWith("dta", StringComparison.OrdinalIgnoreCase))
                        {
                            fn.ImageKey = "file_extension_dll.png";
                            fn.SelectedImageKey = "file_extension_dll.png";
                        }
                        else
                        {
                            fn.ImageKey = "document-music-playlist.png";
                            fn.SelectedImageKey = "document-music-playlist.png";
                        }

                        var b = f.Data;
                        Debug.WriteLine(f.Name + " : " + b.Length.ToString());
                    }
                    else
                    {
                        fn.ImageKey = "file_extension_chm.png";
                        fn.SelectedImageKey = "file_extension_chm.png";
                    }
                }

                foreach (PackageFolder f in folder.Folders)
                {
                    if (!AddPackageTreeNodes(nc, f))
                        ret = false;
                }
            }
            catch { ret = false; }
            return ret;
        }


        public Package OpenProCONFile(SongCacheItem sc, bool silent)
        {

            if (string.IsNullOrEmpty(sc.G6ConFile))
            {
                return null;
            }
            var ret = Package.Load(sc.G6ConFile);
            if (ret == null)
                return ret;

            var upgrades = ret.GetFile("songs_upgrades", "upgrades.dta");
            if (upgrades == null)
            {
                if (!silent)
                    MessageBox.Show("upgrades.dta file missing");
                return null;
            }

            PackageFile proMid = GetProMidFromPackage(ret);

            if (proMid == null)
            {
                if (!silent)
                    MessageBox.Show("Invalid pro midi file in upgrades.dta");
            }

            return ret;
        }



        public bool LoadPackageIntoTree(Package package)
        {
            bool ret = false;
            
            treePackageContents.SuspendLayout();
            treePackageContents.Nodes.Clear();
            treePackageContents.Tag = package;
            try
            {
                ret = AddPackageTreeNodes(treePackageContents.Nodes, package.RootFolder);
            }
            catch { }
            treePackageContents.ExpandAll();
            treePackageContents.ResumeLayout();

            return ret;
        }

        public byte[] GetUpgradesDTAFromPackage(string fileName)
        {
            return GetUpgradesDTAFromPackage(Package.Load(fileName));
        }

        public byte[] GetUpgradesDTAFromPackage(Package pk)
        {
            if (pk == null)
                return null;

            try
            {
                return pk.GetFile("songs_upgrades", "upgrades.dta").Data;
            }
            catch
            {
                return null;
            }
        }
        public PackageFile GetProMidFromPackage(Package pk)
        {
            if (pk == null)
                return null;

            PackageFile upgradeDTA = null;
            try
            {
                upgradeDTA = pk.GetFile("songs_upgrades", "upgrades.dta");
            }
            catch
            {
                return null;
            }

            string proName = DTAGetProFileName(upgradeDTA.Data);

            if (string.IsNullOrEmpty(proName))
            {
                return null;
            }
            else
            {
                proName = proName.Trim('"');

                var s = proName.Split('/');

                if (s == null || s.Length != 2 || s[0] != "songs_upgrades")
                {
                    return null;
                }
                else
                {
                    var proMid = pk.GetFile("songs_upgrades", s[1]);

                    if (proMid == null)
                    {
                        return null;
                    }
                    else
                    {
                        return proMid;
                    }
                }
            }

        }




        public bool TryWriteFile(string fileName, byte[] data)
        {
            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                File.WriteAllBytes(fileName, data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveProCONFile(SongCacheItem sc,  bool silent, bool batch, bool saveAs=false)
        {
            bool ret = false;
            if (sc == null)
                return ret;
            try
            {

                string fileName = saveAs ? "" : sc.G6ConFile;

                if (string.IsNullOrEmpty(fileName))
                {
                    var f = GetShortFileNameFromG5(sc);
                    if (!string.IsNullOrEmpty(f))
                    {
                        f += Utility.DefaultCONFileExtension;
                    }
                    fileName = ShowSaveFileDlg("Save CON Package",
                        DefaultConFileLocation,
                        Path.Combine(DefaultConFileLocation, f));

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        sc.G6ConFile = fileName;
                        textBoxSongLibConFile.Text = sc.G6ConFile;
                    }
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    byte[] proTrack = null;

                    

                    using (var ms = EditorPro.Sequence.Save())
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var bw = new BinaryReader(ms))
                        {
                            proTrack = bw.ReadBytes((int)ms.Length);
                        }
                    }
                    if (proTrack == null || proTrack.Length == 0)
                    {
                        if (!silent)
                            MessageBox.Show("Unable to retrieve pro midi file");
                        return ret;
                    }


                    byte[] existingCON = null;
                    if (File.Exists(fileName))
                    {
                        existingCON = ReadFileBytes(fileName);
                    }

                    string proName = Path.GetFileName(sc.G6FileName);

                    var con = Package.CreateRB3Pro(sc.DTASongShortName,
                       InstrumentDifficultyUtil.MapDifficulty((InstrumentDifficulty)sc.DTAGuitarDifficulty),
                        InstrumentDifficultyUtil.MapDifficulty((InstrumentDifficulty)sc.DTABassDifficulty),
                        sc.DTASongID,
                        sc.DTASongShortName,
                        proName, proTrack, existingCON);

                    if (con != null && con.Length > 0)
                    {
                        if (batch == false || (batch==true && !checkBoxSmokeTest.Checked))
                        {
                            if (!TryWriteFile(fileName, con))
                            {
                                if (!silent)
                                    MessageBox.Show("Cannot write file");
                            }
                        }

                        var pk = Package.Load(fileName);
                        if (pk == null)
                        {
                            if (!silent)
                                MessageBox.Show("Validate package failed");
                        }
                        else
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (!silent)
                            MessageBox.Show("Cannot create CON Package");
                    }
                }
            }
            catch { Debug.WriteLine("crypto error"); }
            return ret;
        }





        public bool FindDTAInformation(SongCacheItem sc)
        {
            bool ret = false;
            try
            {
                string songid, songshortname, gDiff, bDiff;
                if (FindSongID(sc, out songid, out songshortname, out gDiff, out bDiff))
                {
                    ret = true;
                    textBoxCONSongID.Text = songid;
                    sc.DTASongID = textBoxCONSongID.Text;
                    textBoxCONShortName.Text = songshortname;
                    sc.DTASongShortName = textBoxCONShortName.Text;

                    comboProGDifficulty.SelectedIndex = (int)InstrumentDifficultyUtil.MapDifficulty(gDiff);

                    comboProBDifficulty.SelectedIndex = (int)InstrumentDifficultyUtil.MapDifficulty(bDiff);


                    if (comboProGDifficulty.SelectedIndex == 0 &&
                        comboProGDifficulty.SelectedIndex == 0)
                    {
                        comboProGDifficulty.SelectedIndex = 2;
                        comboProBDifficulty.SelectedIndex = 2;
                    }

                    sc.DTAGuitarDifficulty = comboProGDifficulty.SelectedIndex;
                    sc.DTABassDifficulty = comboProBDifficulty.SelectedIndex;
                }
            }
            catch { }
            return ret;
        }

        public string GetShortFileNameFromG5(SongCacheItem sc)
        {
            string g5fileName = sc.G5FileName;
            if (string.IsNullOrEmpty(g5fileName))
            {
                return string.Empty;
            }

            if (!File.Exists(g5fileName))
                return string.Empty;

            if (!string.IsNullOrEmpty(g5fileName))
            {
                return Path.GetFileNameWithoutExtension(g5fileName);
            }
            return string.Empty;
        }

        public bool FindSongID(SongCacheItem sc, out string songID, out string songShortName,
            out string gDiff, out string bDiff)
        {
            songID = string.Empty;
            songShortName = string.Empty;
            gDiff = string.Empty;
            bDiff = string.Empty;
            bool foundDiff = false;
            bool foundSongID = false;
            try
            {
                if (string.IsNullOrEmpty(songID))
                {

                    string fileFolder = sc.G5FileName;
                    songShortName = GetShortFileNameFromG5(sc);
                    if (string.IsNullOrEmpty(fileFolder))
                    {
                        if (!string.IsNullOrEmpty(textBoxCONShortName.Text))
                        {
                            songShortName = textBoxCONShortName.Text;
                        }
                        else
                        {
                            songShortName = textBox24.Text;
                        }
                        if (!string.IsNullOrEmpty(songShortName))
                        {
                            fileFolder = sc.G6FileName;
                        }
                    }
                    

                    if (string.IsNullOrEmpty(fileFolder) ||
                        string.IsNullOrEmpty(songShortName))
                    {
                        return false;
                    }

                    var fileDirectory = Path.GetDirectoryName(fileFolder);
                    for (int x = 0; x < 3; x++)
                    {
                        var songsDTAPath = Path.Combine(fileDirectory, "songs.dta");
                        var upgradesDTAPath = Path.Combine(fileDirectory, "upgrades.dta");

                        if (File.Exists(songsDTAPath) || File.Exists(upgradesDTAPath))
                        {
                            if (File.Exists(songsDTAPath) == false)
                            {
                                songsDTAPath = upgradesDTAPath;

                                if (!ParseUpgradesDTAForSongName(upgradesDTAPath, out songShortName))
                                {
                                    break;
                                }
                            }

                            if (!foundSongID)
                            {
                                if (ParseSongsDTAForSongID(songShortName, songsDTAPath, out songID))
                                {
                                    foundSongID = true;
                                }
                            }

                            if (!foundDiff)
                            {
                                if (ParseSongsDTAForDifficulty(songShortName, songsDTAPath, out gDiff, out bDiff))
                                {
                                    foundDiff = true;
                                }
                            }

                            break;
                        }
                        try
                        {
                            var dinfo = Directory.GetParent(fileDirectory);
                            if (dinfo != null && dinfo.Exists)
                            {
                                fileDirectory = dinfo.FullName;
                            }
                        }
                        catch { break; }
                    }
                    
                }


                if (string.IsNullOrEmpty(songID) && !string.IsNullOrEmpty(songShortName))
                {
                    using (var ms = new MemoryStream(Package.GetSongIDList()))
                    {
                        using (var sr = new StreamReader(ms))
                        {
                            while (!sr.EndOfStream)
                            {
                                var line = sr.ReadLine().Trim();
                                var c = line.IndexOf(';');
                                if (c != -1)
                                {
                                    line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                                }
                                var i = line.IndexOf(" - ");
                                if (i != -1)
                                {
                                    var lineName = line.Substring(0, i).Trim();
                                    var lineID = line.Substring(i + 3).Trim();

                                    if (string.Compare(lineName, songShortName, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        songID = lineID;
                                        songShortName = lineName;
                                        foundSongID = true;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return foundSongID || foundDiff;
        }

        public static bool ParseSongsDTAForSongID(
            string songShortFileName,
            string songDTAPath, out string songID)
        {
            songID = string.Empty;
            if (File.Exists(songDTAPath))
            {
                using (var ms = new MemoryStream(ReadFileBytes(songDTAPath)))
                {
                    using (var br = new StreamReader(ms))
                    {
                        while (!br.EndOfStream)
                        {
                            var line = br.ReadLine().Trim(' ', '(', ')', '\'');
                            var c = line.IndexOf(';');
                            if (c != -1)
                            {
                                line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                            }
                            if (string.Compare(line, songShortFileName, StringComparison.OrdinalIgnoreCase) == 0)
                            {

                                while (!br.EndOfStream)
                                {
                                    var line2 = br.ReadLine().Trim(' ', '(', ')', '\'');

                                    var song_id = "song_id ";
                                    if (line2.StartsWith(song_id, StringComparison.OrdinalIgnoreCase))
                                    {
                                        line2 = line2.Substring(song_id.Length);
                                        int comment = line2.IndexOf(';');
                                        if (comment != -1)
                                        {
                                            line2 = line2.Substring(0, comment).Trim(' ', '(', ')', '\'');
                                        }

                                        int isongid;
                                        if (!string.IsNullOrEmpty(line2))
                                        {
                                            isongid = line2.ToInt();
                                            if (!isongid.IsNull())
                                            {
                                                if (isongid == 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    songID = isongid.ToString();
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return string.IsNullOrEmpty(songID) ? false : true;
        }

        public static bool ParseUpgradesDTAForSongName(string upgradesDTAPath, out string songShortName)
        {
            songShortName = string.Empty;
            if (File.Exists(upgradesDTAPath))
            {
                using (var ms = new MemoryStream(ReadFileBytes(upgradesDTAPath)))
                {
                    using (var br = new StreamReader(ms))
                    {
                        while (!br.EndOfStream)
                        {
                            var line = br.ReadLine().Trim(' ', '(', ')', '\'');
                            var c = line.IndexOf(';');
                            if (c != -1)
                            {
                                line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                            }
                            if (!string.IsNullOrEmpty(line))
                            {
                                songShortName = line;
                                break;
                            }
                        }
                    }
                }
            }
            return string.IsNullOrEmpty(songShortName) ? false : true;
        }
        public static bool ParseSongsDTAForDifficulty(
            string songShortFileName,
            string songDTAPath,
            out string guitarDiff,
            out string bassDiff)
        {
            bool foundG = false;
            bool foundB = false;

            guitarDiff = string.Empty;
            bassDiff = string.Empty;
            if (File.Exists(songDTAPath))
            {
                using (var ms = new MemoryStream(ReadFileBytes(songDTAPath)))
                {
                    using (var br = new StreamReader(ms))
                    {


                        while (!br.EndOfStream)
                        {
                            var line = br.ReadLine().Trim(' ', '(', ')', '\'');
                            var c = line.IndexOf(';');
                            if (c != -1)
                            {
                                line = line.Substring(0, c).Trim(' ', '(', ')', '\'');
                            }
                            if (string.Compare(line, songShortFileName, StringComparison.OrdinalIgnoreCase) == 0)
                            {

                                while (!br.EndOfStream)
                                {
                                    var line2 = br.ReadLine().Trim(' ', '(', ')', '\'');

                                    var gdiff = "guitar ";
                                    if (line2.StartsWith(gdiff, StringComparison.OrdinalIgnoreCase))
                                    {
                                        line2 = line2.Substring(gdiff.Length);
                                        int comment = line2.IndexOf(';');
                                        if (comment != -1)
                                        {
                                            line2 = line2.Substring(0, comment).Trim(' ', '(', ')', '\'');
                                        }


                                        if (!string.IsNullOrEmpty(line2) && !line2.Contains(' ') && !line2.Contains('('))
                                        {
                                            guitarDiff = line2;
                                            foundG = true;
                                        }
                                    }
                                    gdiff = "bass ";
                                    if (line2.StartsWith(gdiff, StringComparison.OrdinalIgnoreCase))
                                    {
                                        line2 = line2.Substring(gdiff.Length);
                                        int comment = line2.IndexOf(';');
                                        if (comment != -1)
                                        {
                                            line2 = line2.Substring(0, comment).Trim(' ', '(', ')', '\'');
                                        }


                                        if (!string.IsNullOrEmpty(line2) && !line2.Contains(' ') && !line2.Contains('('))
                                        {
                                            bassDiff = line2;
                                            foundB = true;
                                        }
                                    }
                                    if (foundG && foundB)
                                        break;
                                }
                                break;
                            }

                        }
                    }
                }
            }
            return foundG && foundB;
        }

        public void CloseSelectedSong()
        {
            SetSelectedSongItem(null);
        }

        

        public void SetSelectedSongItem(SongCacheItem sc)
        {
            if (sc != null)
            {
                SelectedSong = sc;
                
                var i = SongList.IndexOf(sc);
                SongList.SelectedSong = sc;

                settings.SetValue("lastSelectedSongItem", sc.ToString());
                label37.Text = sc.SongName;

                
                textBoxSongLibG5MidiFileName.SetValueSuspend(sc.G5FileName).ScrollToEnd();
                
                textBoxSongLibProMidiFileName.SetValueSuspend(sc.G6FileName).ScrollToEnd();
                textBoxSongLibConFile.SetValueSuspend(sc.G6ConFile).ScrollToEnd();
                textBox24.SetValueSuspend(sc.Description).ScrollToEnd();
                

                checkBoxSongLibHasBass.SetValueSuspend(sc.HasBass);
                checkBoxSongLibHasGuitar.SetValueSuspend(sc.HasGuitar);
                checkBoxSongLibCopyGuitar.SetValueSuspend(sc.CopyGuitarToBass);
                checkBoxSongLibIsComplete.SetValueSuspend(sc.IsComplete);
                checkBoxSongLibIsFinalized.SetValueSuspend(sc.IsFinalized);

                textBox37.SetValueSuspend(sc.GuitarTuning[0]);
                textBox38.SetValueSuspend(sc.GuitarTuning[1]);
                textBox39.SetValueSuspend(sc.GuitarTuning[2]);
                textBox40.SetValueSuspend(sc.GuitarTuning[3]);
                textBox41.SetValueSuspend(sc.GuitarTuning[4]);
                textBox42.SetValueSuspend(sc.GuitarTuning[5]);

                textBox43.SetValueSuspend(sc.BassTuning[0]);
                textBox44.SetValueSuspend(sc.BassTuning[1]);
                textBox45.SetValueSuspend(sc.BassTuning[2]);
                textBox46.SetValueSuspend(sc.BassTuning[3]);
                textBox47.SetValueSuspend(sc.BassTuning[4]);
                textBox48.SetValueSuspend(sc.BassTuning[5]);

                comboProBDifficulty.SetValueSuspend(sc.DTABassDifficulty);
                comboProGDifficulty.SetValueSuspend(sc.DTAGuitarDifficulty);
                textBoxCONSongID.SetValueSuspend(sc.DTASongID);
                textBoxCONShortName.SetValueSuspend(sc.DTASongShortName);

                FileNameG5 = sc.G5FileName;
                FileNamePro = sc.G6FileName;

                textBoxSongPropertiesMP3StartOffset.SetValueSuspend(sc.SongMP3PlaybackOffset.ToStringEx());
                textBoxSongPropertiesMP3Location.SetValueSuspend(sc.SongMP3Location).ScrollToEnd();

                if (mp3Player.PlayingMP3File)
                {
                    mp3Player.Stop();
                }

                this.trackBarMidiVolume.SetValueSuspend(sc.SongMidiPlaybackVolume);
                this.trackBarMP3Volume.SetValueSuspend(sc.SongMP3PlaybackVolume);

                checkBoxEnableMidiPlayback.SetValueSuspend(sc.EnableSongMidiPlayback);
                checkBoxSongPropertiesEnableMP3Playback.SetValueSuspend(sc.EnableSongMP3Playback);

                enableMidiPlayback(checkBoxEnableMidiPlayback.Checked);
                enableMP3Playback(checkBoxSongPropertiesEnableMP3Playback.Checked);

                ApplyMidiVolumeChange(sc.SongMidiPlaybackVolume);
                ApplyMP3VolumeChange(sc.SongMP3PlaybackVolume);

                checkBoxAutoGenGuitarHard.SetValueSuspend(sc.AutoGenGuitarHard);
                checkBoxAutoGenGuitarMedium.SetValueSuspend(sc.AutoGenGuitarMedium);
                checkBoxAutoGenGuitarEasy.SetValueSuspend(sc.AutoGenGuitarEasy);

                checkBoxAutoGenBassHard.SetValueSuspend(sc.AutoGenBassHard);
                checkBoxAutoGenBassMedium.SetValueSuspend(sc.AutoGenBassMedium);
                checkBoxAutoGenBassEasy.SetValueSuspend(sc.AutoGenBassEasy);

                RefreshTracks();
                
            }
            else if (SelectedSong != null)
            {
                SelectedSong = null;
                SongList.SelectedSong = null;

                PUEExtensions.TryExec(delegate()
                {
                    if (mp3Player.PlayingMP3File)
                    {
                        mp3Player.Stop();
                    }
                });
                EditorG5.Close();
                EditorPro.Close();
                listBoxSongLibrary.SelectedItems.Clear();
                
                
                label37.Text = "None Selected";
                textBoxSongLibG5MidiFileName.Text = "";

                textBoxSongLibProMidiFileName.Text = "";

                textBoxSongLibConFile.Text = "";

                textBox24.Text = "";


                checkBoxSongLibHasBass.Checked = false;
                checkBoxSongLibHasGuitar.Checked = false;
                checkBoxSongLibCopyGuitar.Checked = false;
                checkBoxSongLibIsComplete.Checked = false;
                checkBoxSongLibIsFinalized.Checked = false;

                textBox37.Text = "";
                textBox38.Text = "";
                textBox39.Text = "";
                textBox40.Text = "";
                textBox41.Text = "";
                textBox42.Text = "";

                textBox43.Text = "";
                textBox44.Text = "";
                textBox45.Text = "";
                textBox46.Text = "";
                textBox47.Text = "";
                textBox48.Text = "";

                comboProBDifficulty.SelectedIndex = 0;
                comboProGDifficulty.SelectedIndex = 0;
                textBoxCONSongID.Text = "";
                textBoxCONShortName.Text = "";

                
                
                FileNameG5 = "";
                FileNamePro = "";

                textBoxSongPropertiesMP3Location.Text = "";

                if (settings. GetValueBool("Keep Midi Playback Selection", false) == false)
                {
                    textBoxSongPropertiesMP3StartOffset.Text = "";
                    checkBoxSongPropertiesEnableMP3Playback.Checked = false;
                    checkBoxEnableMidiPlayback.Checked = true;
                    this.trackBarMidiVolume.Value = 100;
                    this.trackBarMP3Volume.Value = 100;
                }
                if (settings.GetValueBool("Keep Auto Gen Difficulty Selection", false) == false)
                {
                    checkBoxAutoGenGuitarHard.Checked = false;
                    checkBoxAutoGenGuitarMedium.Checked = false;
                    checkBoxAutoGenGuitarEasy.Checked = false;

                    checkBoxAutoGenBassHard.Checked = false;
                    checkBoxAutoGenBassMedium.Checked = false;
                    checkBoxAutoGenBassEasy.Checked = false;
                }
                RefreshTracks();
            }
        }


        public bool OpenSongCacheItem(SongCacheItem item)
        {
            bool ret = false;
            try
            {
                EditorPro.ClearSelection();
                ClearDTAFileProperties();

                if (item != null)
                {
                    SetSelectedSongItem(null);

                    SetSelectedSongItem(item);
                    if (item.G5FileName.FileExists())
                    {
                        ret = EditorG5.LoadMidi5(item.G5FileName, ReadFileBytes(item.G5FileName), true);
                    }
                    if (item.G6FileName.FileExists())
                    {
                        ret = EditorPro.LoadMidi17(item.G6FileName, ReadFileBytes(item.G6FileName), false);
                    }

                    SetEditorDifficulty(GuitarDifficulty.Expert);

                    RefreshModifierListBoxes();

                    RefreshTrainers();

                    listBoxSongLibrary.SelectedItem = item;

                    SongList.SelectedSong = listBoxSongLibrary.SelectedItem as SongCacheItem;
                    
                }
                else
                {
                    
                    SetSelectedSongItem(null);
                }
            }
            catch { }
            return ret;
        }


        public void UpdateSongCacheItem(SongCacheItem sc)
        {
            if (sc != null)
            {
                sc.G5FileName = textBoxSongLibG5MidiFileName.Text;
                sc.G6FileName = textBoxSongLibProMidiFileName.Text;
                sc.G6ConFile = textBoxSongLibConFile.Text;

                sc.Description = textBox24.Text;

                sc.HasBass = checkBoxSongLibHasBass.Checked;
                sc.HasGuitar = checkBoxSongLibHasGuitar.Checked;
                sc.CopyGuitarToBass = checkBoxSongLibCopyGuitar.Checked;
                sc.IsComplete = checkBoxSongLibIsComplete.Checked;
                sc.IsFinalized = checkBoxSongLibIsFinalized.Checked;


                sc.GuitarTuning[0] = textBox37.Text;
                sc.GuitarTuning[1] = textBox38.Text;
                sc.GuitarTuning[2] = textBox39.Text;
                sc.GuitarTuning[3] = textBox40.Text;
                sc.GuitarTuning[4] = textBox41.Text;
                sc.GuitarTuning[5] = textBox42.Text;

                sc.BassTuning[0] = textBox43.Text;
                sc.BassTuning[1] = textBox44.Text;
                sc.BassTuning[2] = textBox45.Text;
                sc.BassTuning[3] = textBox46.Text;
                sc.BassTuning[4] = textBox47.Text;
                sc.BassTuning[5] = textBox48.Text;

                sc.DTABassDifficulty = comboProBDifficulty.SelectedIndex;
                sc.DTAGuitarDifficulty = comboProGDifficulty.SelectedIndex;
                sc.DTASongID = textBoxCONSongID.Text;
                sc.DTASongShortName = textBoxCONShortName.Text;

                sc.SongMP3Location = textBoxSongPropertiesMP3Location.Text;

                sc.SongMP3PlaybackOffset = textBoxSongPropertiesMP3StartOffset.Text.ToInt();
                sc.EnableSongMP3Playback = checkBoxSongPropertiesEnableMP3Playback.Checked;

                sc.SongMidiPlaybackVolume = this.trackBarMidiVolume.Value;
                sc.SongMP3PlaybackVolume = this.trackBarMP3Volume.Value;
                sc.EnableSongMidiPlayback = this.checkBoxEnableMidiPlayback.Checked;

                enableMidiPlayback(sc.EnableSongMidiPlayback);
                enableMP3Playback(sc.EnableSongMP3Playback);

                sc.AutoGenGuitarHard = checkBoxAutoGenGuitarHard.Checked;
                sc.AutoGenGuitarMedium = checkBoxAutoGenGuitarMedium.Checked;
                sc.AutoGenGuitarEasy = checkBoxAutoGenGuitarEasy.Checked;

                sc.AutoGenBassHard = checkBoxAutoGenBassHard.Checked;
                sc.AutoGenBassMedium = checkBoxAutoGenBassMedium.Checked;
                sc.AutoGenBassEasy = checkBoxAutoGenBassEasy.Checked;

                SongList.UpdateSongCacheItem(sc);
            }
        }

        public void AddNewSongToLibrary()
        {
            ClearDTAFileProperties();

            string songName = "";


            if (!string.IsNullOrEmpty(FileNameG5))
            {
                songName = Path.GetFileNameWithoutExtension(FileNameG5);
            }

            if (string.IsNullOrEmpty(songName) && !string.IsNullOrEmpty(FileNamePro))
            {
                songName = Path.GetFileNameWithoutExtension(FileNamePro);
            }

            if (string.IsNullOrEmpty(songName))
            {
                MessageBox.Show("Cannot find song name in package");
                return;
            }

            foreach (SongCacheItem sc in SongList)
            {
                if (string.Compare(sc.SongName, songName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SongList.SelectedSong = sc;
                    OpenSongCacheItem(SongList.SelectedSong);
                    return;
                }
            }

            var i = new SongCacheItem()
            {
                G5FileName = FileNameG5,
                G6FileName = FileNamePro,
                G6ConFile = "",
                Description = songName,
                SongName = songName,
                CopyGuitarToBass = false,
                HasBass = true,
                CacheSongID = GetNextSongID(),
            };

            SongList.AddSong(i);
            
            if (OpenSongCacheItem(i))
            {
                FindDTAInformation(i);

                UpdateSongCacheItem(i);
            }
            
            RefreshUSBSongs();

            ReloadTracks();
            
            SongList.SelectedSong = i;
            OpenSongCacheItem(SongList.SelectedSong);
        }

        public void ClearDTAFileProperties()
        {
            textBoxCONSongID.Text = "";
            textBoxCONShortName.Text = "";
            comboProBDifficulty.SelectedIndex = 0;
            comboProGDifficulty.SelectedIndex = 0;
            textBox37.Text =
            textBox38.Text =
            textBox39.Text =
            textBox40.Text =
            textBox41.Text =
            textBox42.Text =

            textBox43.Text =
            textBox44.Text =
            textBox45.Text =
            textBox46.Text =
            textBox47.Text =
            textBox48.Text = "0";
        }



        public void SaveCONPackageAs()
        {
            if (SelectedSong != null)
            {
                SaveProCONFile(SelectedSong, false, false);
            }
        }


        public bool ExtractPackageContents(string localDirectory, PackageFolder folder, IEnumerable<string> filters)
        {
            var ret = true;
            try
            {
                localDirectory = localDirectory.AppendSlashIfMissing();

                var files = folder.Files.Where(file => file.Data != null && 
                    (filters == null || filters.Any(filter => file.Name.EndsWithEx(filter))));

                if (files.Any())
                {
                    localDirectory.AppendSlashIfMissing().CreateFolderIfNotExists();
                    foreach (var file in files)
                    {
                        localDirectory.PathCombine(file.Name).WriteFileBytes(file.Data);
                    }
                }
                foreach (var subFolder in folder.Folders)
                {
                    ExtractPackageContents(localDirectory.PathCombine(subFolder.Name).AppendSlashIfMissing(),
                        subFolder, filters);
                }
                
            }
            catch { ret = false; }
            return ret;
        }




        public void ExtractPackageToFolder(bool extractAll)
        {
            if (treePackageContents.SelectedNode == null && !extractAll)
                return;
            if (treePackageContents.Tag == null)
                return;


            try
            {
                bool extracted = false;
                string location = string.Empty;
                if (extractAll)
                {
                    if (treePackageContents.Nodes.Count > 0)
                    {
                        var fo = treePackageContents.Nodes[0].Tag as PackageFolder;
                        if (fo != null)
                        {
                            location = ShowSelectFolderDlg("Select location to extract", "", "");
                            if (!string.IsNullOrEmpty(location))
                            {

                                string[] filters = null;
                                if (checkBoxPackageEditExtractDTAMidOnly.Checked)
                                {
                                    filters = new string[] { ".dta", ".mid", ".midi" };
                                }

                                ExtractPackageContents(location, fo, filters);

                                extracted = true;
                            }
                        }
                    }
                }
                else
                {
                    var f = treePackageContents.SelectedNode.Tag as PackageFile;
                    if (f != null)
                    {
                        location = ShowSaveFileDlg("Save Package As", "", f.Name);
                        if (!string.IsNullOrEmpty(location))
                        {
                            extracted = TryWriteFile(location, f.Data);
                        }
                    }
                    else
                    {
                        var fo = treePackageContents.SelectedNode.Tag as PackageFolder;
                        if (fo != null)
                        {
                            location = ShowSelectFolderDlg("Extract Package to Folder", "", "");
                            if (!string.IsNullOrEmpty(location))
                            {

                                ExtractPackageContents(location, fo, null);
                                extracted = true;
                            }
                        }
                    }
                }

                if (checkBox2.Checked && extracted)
                {
                    OpenExplorer(location);
                }

            }
            catch { MessageBox.Show("Failed"); }
        }



        

        public void ShowSelectedPackage()
        {

            textBoxPackageDTAText.Text = "";
            if (treePackageContents.SelectedNode != null &&
                treePackageContents.SelectedNode.Tag != null)
            {
                var f = treePackageContents.SelectedNode.Tag as PackageFile;
                if (f != null)
                {
                    var ext = Path.GetExtension(f.Name);
                    if (string.Compare(ext, ".dta", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        
                        bool opened = false;
                        if (HasValidUSBDeviceSelection)
                            opened = SelectedUSB.Open();

                        textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);

                        if (opened)
                            SelectedUSB.Close();
                    }
                    else
                    {
                        try
                        {
                            if (f.Name.IsMidiFileName())
                            {
                                var mid = f.Data.LoadSequence();

                                var sb = new StringBuilder();

                                foreach (Track t in mid)
                                {
                                    sb.Append(t.Name.GetFileName() + " ");
                                    if (GuitarTrack.TrackNames6.Contains(t.Name))
                                    {
                                        sb.AppendLine("Pro Guitar Midi File");
                                        break;
                                    }
                                    else if (GuitarTrack.TrackNames5.Contains(t.Name))
                                    {
                                        sb.AppendLine("5 Button Guitar Midi File");
                                        break;
                                    }
                                }

                                if (sb.Length == 0)
                                {
                                    sb.AppendLine("Unknown Midi File");
                                }

                                sb.AppendLine();
                                sb.AppendLine("Tracks:");
                                foreach (Track t in mid)
                                {
                                    sb.Append("    ");
                                    sb.AppendLine(t.Name);
                                }
                                textBoxPackageDTAText.Text = sb.ToString();
                            }
                        }
                        catch { }

                        try
                        {
                            if (string.Compare(ext, ".usr", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".vnn", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                               // textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".voc", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".xvocab", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".mogg", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".png_xbox", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text =  Encoding.ASCII.GetString(f.Data);

                               // var ft = X360.Other.VariousFunctions.ReadFileType("c:\\test.dat");
                                //var img = Image.FromStream(new MemoryStream(f.Data));
                            }

                            if (string.Compare(ext, ".milo_xbox", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text =  Encoding.ASCII.GetString(f.Data);
                            }

                            if (string.Compare(ext, ".bin", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                //textBoxPackageDTAText.Text = Encoding.ASCII.GetString(f.Data);
                            }
                        }
                        catch { }
                    }

                }
            }
        }

        public void ClearBatchResults()
        {
            textBoxSongLibBatchResults.Text = "";
        }

        public void WriteBatchResult(string txt)
        {
            var sb = new StringBuilder();
            sb.AppendLine(txt);
            textBoxSongLibBatchResults.SuspendLayout();
            textBoxSongLibBatchResults.Text += sb.ToString();

            if (textBoxSongLibBatchResults.Text.Length > 0)
                textBoxSongLibBatchResults.SelectionStart = textBoxSongLibBatchResults.Text.Length - 1;
            textBoxSongLibBatchResults.SelectionLength = 1;
            textBoxSongLibBatchResults.ScrollToCaret();

            textBoxSongLibBatchResults.ResumeLayout();
        }

        public bool ExecuteBatchBuildTextEvents()
        {
            bool ret = false;
            buttonSongLibCancel.Enabled = true;
            ret = GenerateTextEvents();
            buttonSongLibCancel.Enabled = false;
            return ret;
        }

        public bool ExecuteBatchDifficulty()
        {
            bool ret = false;
            buttonSongLibCancel.Enabled = true;
            ret = GenerateCompletedDifficulties();
            buttonSongLibCancel.Enabled = false;
            return ret;
        }
        public bool ExecuteBatchCopyUSB()
        {
            bool ret = true;
            buttonSongLibCancel.Enabled = true;

            OpenSelectedUSB();
           
            try
            {
                if (SelectedUSB.Open())
                {
                    var folder = SelectedUSBFolderEntry;
                    var contents = SelectedUSBContents;
                    if (folder != null)
                    {
                        buttonSongLibCancel.Enabled = true;
                        progressBarGenCompletedDifficulty.Value = 0;

                        var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

                        progressBarGenCompletedDifficulty.Maximum = songs.Count();
                        
                        foreach (SongCacheItem item in songs)
                        {
                            Application.DoEvents();

                            WriteBatchResult("Copying to USB: " + item.ToString());
                            progressBarGenCompletedDifficulty.Value++;

                            if (buttonSongLibCancel.Enabled == false)
                            {
                                WriteBatchResult("User Cancelled");
                                ret = false;
                                break;
                            }

                            if (item.IsComplete == false && checkBoxBatchProcessIncomplete.Checked == false)
                            {
                                WriteBatchResult("Skipping Incomplete song: " + item.ToString());
                                continue;
                            }

                            CopySongToUSB(item);
                        }
                    }
                    SelectedUSB.Close();
                }
            }
            catch
            {
                ret = false;
                WriteBatchResult("Exception copying files to USB");
            }
            OpenSelectedUSB();
            buttonSongLibCancel.Enabled = false;
            return ret;
        }

        public bool CopySongToUSB(SongCacheItem item)
        {
            bool ret = false;
            try
            {
                bool opened = false;
                if (SelectedUSB == null)
                    return ret;

                if (SelectedUSB.IsOpen == false)
                {
                    OpenSelectedUSB();
                    opened = SelectedUSB.Open();
                }

                var folder = SelectedUSBFolderEntry;
                var contents = SelectedUSBContents;
                    

                if (!string.IsNullOrEmpty(item.G6ConFile))
                {
                    if (File.Exists(item.G6ConFile))
                    {
                        try
                        {
                            var fileName = Path.GetFileName(item.G6ConFile);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                bool overwrite = false;
                                if (contents != null)
                                {
                                    foreach (var c in contents.Files)
                                    {
                                        if (string.Compare(c.Name, fileName, true) == 0)
                                        {
                                            overwrite = true;
                                            break;
                                        }
                                    }
                                }

                                bool success = false;
                                if (overwrite)
                                {
                                    success = folder.AddFile(fileName, item.G6ConFile, AddType.Replace);
                                }
                                else
                                {
                                    success = folder.AddFile(fileName, item.G6ConFile, AddType.Inject);
                                }

                                if (!success)
                                {
                                    WriteBatchResult("Unable to write file to usb: " + fileName);
                                }
                                else
                                {
                                    ret = success;
                                }
                            }
                        }
                        catch
                        {
                            WriteBatchResult("Exception writing to usb");
                            ret = false;
                        }
                    }
                }
                if (opened)
                {
                    SelectedUSB.Close();
                }
            }
            catch { }
            return ret;
        }


        public bool GenerateCompletedDifficulties()
        {
            progressBarGenCompletedDifficulty.Value = 0;

            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();

            foreach (var item in songs.ToList())
            {
                Application.DoEvents();

                WriteBatchResult("Generating Difficulties: " + item.ToString());
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    return false;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);
                }
                catch { }

                if ((checkBoxBatchProcessIncomplete.Checked == false && !item.IsComplete) ||
                    (checkBoxBatchProcessFinalized.Checked == false && item.IsFinalized))
                {
                    WriteBatchResult("Skipping: " + item.ToString());
                }
                else
                {
                    WriteBatchResult("Processing: " + item.ToString());
                    try
                    {
                        if (!OpenSongCacheItem(item))
                        {
                            WriteBatchResult("Unable to open: " + item.ToString());
                            continue;
                        }

                        if (checkBoxSkipGenIfEasyNotes.Checked &&
                            hasEasyMedHardEvent6())
                        {
                            WriteBatchResult("Skipping (already calculated): " + item.ToString());
                        }
                        else
                        {
                            var config = new GenDiffConfig(item, true, checkBoxSongLibCopyGuitar.Checked,
                                false, false, true);

                            Debug.WriteLine("gen complete regen");
                            if (!GenerateDifficulties(true, config))
                            {
                                WriteBatchResult("Gen Difficulty Failed: " + item.ToString());
                                continue;
                            }

                            if (!SaveSongCacheItem(item, true))
                            {
                                WriteBatchResult("Failed Saving: " + item.ToString());
                                continue;
                            }
                        }

                    }
                    catch
                    {
                        WriteBatchResult("Error processing: " + item.ToString());
                    }
                }
            }

            WriteBatchResult("Batch Generate Difficulties Complete");

            return true;
        }


        public bool GenerateTextEvents()
        {
            progressBarGenCompletedDifficulty.Value = 0;

            var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

            progressBarGenCompletedDifficulty.Maximum = songs.Count();

            foreach (var item in songs)
            {
                Application.DoEvents();
                WriteBatchResult("Generating Text Events: " + item.ToString());
                if (buttonSongLibCancel.Enabled == false)
                {
                    WriteBatchResult("User Cancelled");
                    return false;
                }
                try
                {
                    progressBarGenCompletedDifficulty.Value = songs.IndexOf(item);
                }
                catch { }

                if ((checkBoxBatchProcessIncomplete.Checked == false && !item.IsComplete) ||
                    (checkBoxBatchProcessFinalized.Checked == false && item.IsFinalized))
                {
                    WriteBatchResult("Skipping: " + item.ToString());
                }
                else
                {
                    WriteBatchResult("Processing: " + item.ToString());
                    try
                    {
                        if (!OpenSongCacheItem(item))
                        {
                            WriteBatchResult("Unable to open: " + item.ToString());
                            continue;
                        }

                        if (checkBatchCopyTextEvents.Checked)
                        {
                            CopyTextEvents(false);

                            if (checkBatchGenerateTrainersIfNone.Checked)
                            {
                                GenerateTrainers(false);
                            }
                        }

                        if (!SaveProFile(item.G6FileName, true))
                        {
                            WriteBatchResult("Failed saving pro file: " + item.ToString());
                            continue;
                        }


                    }
                    catch
                    {
                        WriteBatchResult("Error processing: " + item.ToString());
                    }
                }
            }

            WriteBatchResult("Batch Generate Text Events Complete");

            return true;
        }

        public bool SaveSongCacheItem(SongCacheItem item, bool silent)
        {
            try
            {
                if (item != null)
                {
                    UpdateSongCacheItem(item);
                    item.IsDirty = false;
                    return SaveProFile(item.G6FileName, silent);
                }
            }
            catch { }
            return false;
        }


        public bool TryCopyFile(string sourceFile, string destFile)
        {
            try
            {
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                File.Copy(sourceFile, destFile);
                return true;
            }
            catch { return false; }
        }


        public void UpdateCONFileProperties(SongCacheItem sc)
        {
            if (sc != null)
            {
                if (SaveProCONFile(sc,  false, false))
                {
                    OpenProCONFile(sc, true);
                }
            }
        }

        public bool SavePro()
        {
            bool ret = true;
            try
            {
                SaveProMidi();
              
                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);
                }
            }
            catch { ret = false; }
            return ret;
        }

        public bool RebuildCONFileProperties(SongCacheItem sc)
        {
            bool ret = false;
            try
            {
                if (sc != null)
                {
                    SaveProMidi();

                    if (SaveProCONFile(sc,  true, false))
                    {
                        if (OpenProCONFile(sc, true) != null)
                        {
                            ret = true;
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool SaveProMidi()
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(FileNamePro) && File.Exists(FileNamePro))
            {
                ret = SaveProFile(FileNamePro, true);
            }
            return ret;
        }
        public void OpenExplorer(string location)
        {
            try
            {
                Process.Start("explorer.exe", "\"" + location + "\"");
            }
            catch { }
        }
        public void OpenNotepad(string location)
        {
            try
            {
                Process.Start("notepad.exe", "\"" + location + "\"");
            }
            catch { }
        }

        public void OpenNotepad(byte[] data)
        {
            try
            {
                var str = Encoding.ASCII.GetString(data);

                if (string.IsNullOrEmpty(str))
                    return;

                var tempFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "noteTemp.txt");
                File.WriteAllText(tempFile, str);

                var proc = Process.Start("notepad.exe", "\"" + tempFile + "\"");
                if (proc.WaitForInputIdle(10000))
                {
                    
                }
            }
            catch { }
        }

        public void OpenExplorerFolder(string location)
        {
            if (!string.IsNullOrEmpty(location))
            {
                if (Directory.Exists(location) == false && File.Exists(location) == true)
                {
                    location = Path.GetDirectoryName(location);
                }

                if (Directory.Exists(location) == true && File.Exists(location) == false)
                {
                    OpenExplorer(location);
                }
                else
                {
                    MessageBox.Show("Invalid Path");
                }
            }
            
        }

        //)
        public bool IsChordSameAsStoredChord(GuitarChord c, StoredChord sc, 
            bool noteType, bool noteFret, bool matchAll, bool matchStrum)
        {
            bool same = true;
            if (noteType)
            {
                if ((c.HasHammeron == sc.IsHammeron &&
                    c.HasSlide == sc.IsSlide &&
                    c.HasSlideReversed == sc.IsSlideRev &&
                    c.IsTap == sc.IsTap &&
                    c.IsXNote == sc.IsXNote) == false)
                {
                    same = false;
                }
            }

            if (same == true && noteFret)
            {

                int numToMatch = 0;
                var cn = new List<int>();
                for (int x = 0; x < 6; x++)
                {
                    GuitarNote n = c.Notes[x];
                    int scn = sc.Notes[5 - x];

                    if (scn != -1)
                        numToMatch++;
                    if (n == null && scn == -1)
                    {
                        continue;
                    }
                    else if (n == null && scn != -1)
                    {
                        same = false;
                    }
                    else if (n != null && scn == -1)
                    {
                        same = false;
                    }
                    else if (n != null && scn != -1 &&
                        n.NoteFretDown == scn)
                    {
                        cn.Add(x);
                    }
                    else
                    {
                        same = false;
                    }
                    
                }

                if (same == true && matchAll)
                {
                    if (cn.Count != numToMatch)
                        same = false;
                }
                else if (matchAll == false && cn.Count > 0)
                {
                    same = true;
                }
            }

            if (same == true && matchStrum)
            {
                same = (c.StrumMode == sc.Strum);
            }
            return same;

        }






        public void CheckMinimumNoteWidth()
        {
            int i = textBoxMinimumNoteWidth.Text.ToInt();
            if (!i.IsNull())
            {
                if (i >= 0)
                {
                    Utility.MinimumNoteWidth = i;
                }
            }
        }

        public void SetScrollToSelectionOffset()
        {
            var soffset = textBoxScrollToSelectionOffset.Text;
            var offset = soffset.ToInt();
            if (offset.IsNull())
            {
                textBoxScrollToSelectionOffset.Text = "300";
                offset = 300;
            }
            
            Utility.ScollToSelectionOffset = offset;
        
        }


        public void CheckNotesGridSelection()
        {
            EditorPro.ShowNotesGrid = checkViewNotesGridPro.Checked;

            EditorG5.ShowNotesGrid = checkViewNotesGrid5Button.Checked;

            if (checkViewNotesGridPro.Checked)
            {
                EditorPro.GridScalar = GetGridScalar();
            }

            if (checkViewNotesGrid5Button.Checked)
            {
                EditorG5.GridScalar = GetGridScalar();
            }
            EditorG5.Invalidate();
            EditorPro.Invalidate();
        }

        public void SetGridScalar(string scale)
        {
            double d = scale.ToDouble(1.0);
            
            if (d < 0.124 * 0.5)
                radioGrid128thNote.Checked = true;
            else if (d < 0.124)
                radioGrid64thNote.Checked = true;
            else if (d < 0.24)
                radioGrid32Note.Checked = true;
            else if (d < 0.4)
                radioGrid16Note.Checked = true;
            else if (d < 0.9)
                radioGrid8Note.Checked = true;
            else
                radioGridQuarterNote.Checked = true;
        }

        public double GetGridScalar()
        {
            if (radioGrid128thNote.Checked)
            {
                return 0.125 * 0.5 * 0.5;
            }
            if (radioGrid64thNote.Checked)
            {
                return 0.125 * 0.5;
            }
            if (radioGrid32Note.Checked)
            {
                return 0.125;
            }
            else if (radioGrid16Note.Checked)
            {
                return 0.25;
            }
            else if (radioGrid8Note.Checked)
            {
                return 0.5;
            }
            else //if (radioGridQuarterNote.Checked)
            {
                return 1.0;
            }
        }

        public double GridScalar128
        {
            get
            {
                return 0.125 * 0.5 * 0.5;
            }
        }




        

        public void UpdateEditorProperties()
        {
            int freq = textBoxMidiScrollFreq.Text.ToInt(5);
            if (freq < 5)
            {
                textBoxMidiScrollFreq.Text = "5";
                freq = 5;
            }
            if (!freq.IsNull())
            {
                timerMidiPlayback.Interval = freq;
            }

            int i = textClearHoldBox.Text.ToInt();
            if (!i.IsNull())
            {
                resetTime = i;
            }
            else
            {
                textClearHoldBox.Text = resetTime.ToString();
            }
            
            SetScrollToSelectionOffset();
            CheckMinimumNoteWidth();

            i = textBoxNoteCloseDist.Text.ToInt();
            if (!i.IsNull())
            {
                Utility.NoteCloseWidth = i;
            }
            else
            {
                textBoxNoteCloseDist.Text = Utility.NoteCloseWidth.ToString();
            }

            if (checkView5Button.Checked)
            {
                if (checkView5Button.Tag != null)
                {
                    checkView5Button.Tag = null;
                    panel5ButtonEditor.Visible = true;
                    var h = panel5ButtonEditor.Height;
                    panelProEditor.Location = new Point(panelProEditor.Location.X, panelProEditor.Location.Y + h);
                    tabContainerMain.Location = new Point(tabContainerMain.Location.X, tabContainerMain.Location.Y + h);
                    tabContainerMain.Height -= h;
                }
            }
            else
            {
                if (checkView5Button.Tag == null)
                {
                    checkView5Button.Tag = "Hidden";
                    panel5ButtonEditor.Visible = false;
                    var h = panel5ButtonEditor.Height;
                    panelProEditor.Location = new Point(panelProEditor.Location.X, panelProEditor.Location.Y - h);
                    tabContainerMain.Location = new Point(tabContainerMain.Location.X, tabContainerMain.Location.Y - h);
                    tabContainerMain.Height += h;
                }
            }

            EditorPro.Invalidate();
        }



        public void CheckNoteChannelVisibility()
        {
            if (checkBoxShowMidiChannelEdit.Checked == false)
            {

                groupBox6.Visible = false;

                if (groupBox6.Tag == null)
                {
                    groupBox6.Tag = "hidden";
                    int w = groupBox6.Width;
                    groupBox2.Location = new Point(groupBox2.Location.X + w, groupBox2.Location.Y);
                    groupBox36.Location = new Point(groupBox36.Location.X + w, groupBox36.Location.Y);
                }
            }
            else
            {
                groupBox6.Visible = true;

                if (groupBox6.Tag != null)
                {
                    groupBox6.Tag = null;
                    int w = groupBox6.Width;
                    groupBox2.Location = new Point(groupBox2.Location.X - w, groupBox2.Location.Y);
                    groupBox36.Location = new Point(groupBox36.Location.X - w, groupBox36.Location.Y);
                }
            }
        }


        public static byte[] ReadFileBytes(string fileName)
        {
            byte[] buffer = null;
            if (File.Exists(fileName))
            {
                

                using (FileStream fs = File.OpenRead(fileName))
                {
                    buffer = new byte[fs.Length];

                    fs.Read(buffer, 0, (int)fs.Length);
                }
                
            }
            return buffer;
        }

    }
}
