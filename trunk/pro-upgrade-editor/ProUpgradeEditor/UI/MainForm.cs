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
using ZipLib.SharpZipLib.Zip;
using X360;
using X360.FATX;
using System.Collections;
using X360.Other;
using System.Runtime.Serialization;
using EditorResources.Components;
using NAudio.Wave;
using ZipLib.SharpZipLib.Core;


namespace ProUpgradeEditor.UI
{
    public partial class MainForm : Form
    {

        public List<TrackEditor> Editors;

        public TrackEditor EditorPro { get{ return trackEditorG6; } }
        public TrackEditor EditorG5 { get { return trackEditorG5; } }

        
        public MainForm()
        {

            InitializeComponent();

            checkBoxRenderMouseSnap.Visible = false;
            Editors = new List<TrackEditor>() { trackEditorG5, trackEditorG6 };

            tabContainerMain.SizeChanged += new EventHandler(tabContainerMain_SizeChanged);
            tabContainerMain.Resize += new EventHandler(tabContainerMain_Resize);
            tabContainerMain.AllowDrop = true;
            tabPackageEditor.AllowDrop = true;
            tabPackageEditor.DragOver += new DragEventHandler(tabPackageEditor_DragOver);
            tabPackageEditor.DragDrop += new DragEventHandler(tabPackageEditor_DragDrop);

            tabSongLibSongProperties.AllowDrop = true;
            tabSongLibSongProperties.DragOver += new DragEventHandler(tabSongLibSongProperties_DragOver);
            tabSongLibSongProperties.DragDrop += new DragEventHandler(tabSongLibSongProperties_DragDrop);

            tabSongLibraryUtility.AllowDrop = true;
            tabSongLibraryUtility.DragOver += new DragEventHandler(tabSongLibraryUtility_DragOver);
            tabSongLibraryUtility.DragDrop += new DragEventHandler(tabSongLibraryUtility_DragDrop);
            this.AllowDrop = true;
            animationTimer.Interval = 10;

            midiTrackEditorPro.RequestBackup += new EventHandler(midiTrackEditorPro_RequestBackup);
        }

        void midiTrackEditorPro_RequestBackup(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
        }

        void tabContainerMain_Resize(object sender, EventArgs e)
        {
            foreach (Control control in tabContainerMain.Controls)
            {
                control.SuspendLayout();
            }
        }

        void tabContainerMain_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control control in tabContainerMain.Controls)
            {
                control.ResumeLayout();
            }
        }

        void tabSongLibraryUtility_DragDrop(object sender, DragEventArgs e)
        {
            HandlePackageDrop(e);
        }

        void tabSongLibraryUtility_DragOver(object sender, DragEventArgs e)
        {
            HandlePackageDropDragOver(e);
        }

        
        void tabSongLibSongProperties_DragDrop(object sender, DragEventArgs e)
        {
            HandlePackageDrop(e);
        }

        void tabSongLibSongProperties_DragOver(object sender, DragEventArgs e)
        {
            HandlePackageDropDragOver(e);
        }

        void tabPackageEditor_DragDrop(object sender, DragEventArgs e)
        {
            HandlePackageDrop(e);
        }

        bool HandlePackageDropDragOver(DragEventArgs e)
        {
            return PUEExtensions.TryExec(delegate()
            {
                var ret = e.Data.GetDataPresent(DataFormats.FileDrop, false);
                if (ret) { e.Effect = DragDropEffects.All; }
                else { e.Effect = DragDropEffects.None; }
                return ret;
            });
        }
        public void HandlePackageDrop(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    string[] packageFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (var pk in packageFiles)
                    {
                        ImportFile(pk);
                    }
                }
            }
            catch { }
        }

        public void ImportFile(string fileName)
        {
            try
            {
                var proFolder = DefaultMidiFileLocationPro;
                if (!proFolder.FolderExists())
                {
                    MessageBox.Show("Default pro midi path not configured.");
                }
                else if (!fileName.IsEmpty())
                {

                    var isZipFile = fileName.EndsWithEx(".zip");

                    if (isZipFile)
                    {
                        var outputFolder = proFolder.PathCombine(
                                fileName.GetFileNameWithoutExtension()).AppendSlashIfMissing();
                        outputFolder.CreateFolderIfNotExists();

                        if(ExtractZipFile(fileName, outputFolder, null, false))
                        {
                            var midiFiles = outputFolder.GetFilesInFolder(true, "*.mid|*.midi").ToList();
                            var dtaFiles = outputFolder.GetFilesInFolder(true, "*.dta").ToList();
                            var mp3Files = outputFolder.GetFilesInFolder(true, "*.mp3").ToList();

                            importG5Midi(dtaFiles, midiFiles, mp3Files);
                        }
                    }
                    else
                    {
                        var package = Package.Load(fileName);

                        if (package != null)
                        {
                            var outputFolder = proFolder.PathCombine(
                                fileName.GetFileNameWithoutExtension()).AppendSlashIfMissing();
                            outputFolder.CreateFolderIfNotExists();

                            try
                            {
                                var dtaFiles = ExtractDTAFiles(package, outputFolder);
                                var midiFiles = ExtractMidiFiles(package, outputFolder);

                                importG5Midi(dtaFiles, midiFiles, new List<string>());
                            }
                            catch { }
                        }

                    }
                }
            }
            catch { }
        }

        private void importG5Midi(List<string> dtaFiles, List<string> midiFiles, List<string> mp3Files)
        {
            if (dtaFiles.Any() && midiFiles.Any())
            {
                var midiFilesG5 = midiFiles.Where(x => x.GetMidiFileType() == FileType.Guitar5).ToList();

                var midiFilesPro = midiFiles.Where(x => !midiFilesG5.Contains(x)).ToList()
                    .Where(x => x.GetMidiFileType() == FileType.Pro).ToList();

                foreach (var midiFileNameG5 in midiFilesG5)
                {
                    try
                    {
                        CloseSelectedSong();
                        if (OpenEditorFile(midiFileNameG5))
                        {
                            var fn = midiFileNameG5.GetFileNameWithoutExtension();
                            var pro = midiFilesPro.Where(x => x.GetFileNameWithoutExtension().StartsWithEx(fn));
                            if (!pro.Any())
                            {
                                pro = midiFilesPro.Where(x => x.GetFolderName().EqualsEx(midiFileNameG5.GetFolderName()));
                            }
                            if (!pro.Any())
                            {
                                var seq5 = midiFileNameG5.LoadSequenceFile();
                                var tempo5 = seq5.GetTempoTrack();
                                
                                if(tempo5 != null)
                                {
                                    var t5Event = tempo5.Tempo.FirstOrDefault();

                                    var cb5 = new TempoChangeBuilder(t5Event.MetaMessage);
                                    MidiEvent tempo6 = null;
                                    pro = 
                                        midiFilesPro.Where(x=> (tempo6 = x.LoadSequenceFile().GetTempoTrack().Tempo.FirstOrDefault()) != null &&
                                        (tempo6.AbsoluteTicks == t5Event.AbsoluteTicks && 
                                        (new TempoChangeBuilder(tempo6.MetaMessage).Tempo == cb5.Tempo))).ToList();
                                }
                            }
                            if (pro.Any())
                            {
                                pro.OrderByDescending(x =>
                                    x.GetFileModifiedTime()).FirstOrDefault().
                                    IfObjectNotNull(x =>
                                {
                                    var seq = x.LoadSequenceFile();
                                    EditorPro.LoadedFileName = x;
                                    EditorPro.SetTrack6(seq, seq.GetPrimaryTrack());
                                });
                            }

                            CreateSongFromOpenMidi();

                            if (mp3Files.Any())
                            {
                                textBoxSongPropertiesMP3Location.Text = mp3Files.First();
                                textBoxSongPropertiesMP3StartOffset.Text = "0";
                                
                                if (SelectedSong != null)
                                {
                                    UpdateSongCacheItem(SelectedSong);
                                    OpenSongCacheItem(SelectedSong);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public List<string> ExtractMidiFiles(Package f, string outputDir)
        {
            var ret = new List<string>();
            try
            {
                
                foreach (var midi in f.GetFilesByExtension(".mid|.midi"))
                {
                    try
                    {
                        var newFile = outputDir.PathCombine(midi.Name);
                        if (!newFile.FileExists())
                        {
                            File.WriteAllBytes(newFile, midi.Data);
                        }
                        ret.Add(newFile);
                    }
                    catch { }
                }
                
            }
            catch { }
            return ret;
        }

        private static List<string> ExtractDTAFiles(Package f, string outputDir)
        {
            var ret = new List<string>();
            try
            {   
                foreach (var dta in f.GetFilesByExtension(".dta"))
                {
                    try
                    {
                        var newFile = Path.Combine(outputDir, dta.Name);
                        if (!File.Exists(newFile))
                        {
                            File.WriteAllBytes(newFile, dta.Data);
                        }
                        ret.Add(newFile);
                    }
                    catch { }
                }
            }
            catch { }
            return ret;
        }


        void tabPackageEditor_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                trackEditorG5.Initialize(false);
                EditorPro.Initialize(true);

                trackEditorG5.EditorPro = EditorPro;
                
                EditorPro.Editor5 = trackEditorG5;

                EditorPro.OnCreationStateChanged += new TrackEditor.CreationStateChangedHandler(EditorPro_OnCreationStateChanged);

                checkBoxGridSnap.CheckedChanged += new EventHandler(checkBoxGridSnap_CheckedChanged);
                checkBoxSnapToCloseNotes.CheckedChanged += new EventHandler(checkBoxSnapToCloseNotes_CheckedChanged);
                checkSnapToCloseG5.CheckedChanged += new EventHandler(checkSnapToCloseG5_CheckedChanged);
                EditorPro.OnSelectionStateChange += new TrackEditor.SelectionStateChangeHandler(EditorPro_OnSelectionStateChange);
                EditorG5.OnSelectionStateChange += new TrackEditor.SelectionStateChangeHandler(EditorG5_OnSelectionStateChange);
                statusStrip1.Items.Add(toolStripCreateStatus);

                tabContainerMain.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
                Editors.ForEach(x => x.AddScrollHandler());

                EditorPro.OnMouseDownEvent += new TrackEditor.MouseDownEventHandler(EditorPro_OnMouseDownEvent);
                EditorG5.OnMouseDownEvent += new TrackEditor.MouseDownEventHandler(EditorG5_OnMouseDownEvent);
                DoFormLoad();

                RefreshTracks();

                InitializeUSBList();

                trackEditorG5.OnLoadTrack += new TrackEditor.LoadTrackHandler(trackEditorG5_OnLoadTrack);
                trackEditorG6.OnLoadTrack += new TrackEditor.LoadTrackHandler(trackEditorG6_OnLoadTrack);
                
                trackEditorG5.OnClose += new TrackEditor.CloseTrackHandler(trackEditorG5_OnClose);
                trackEditorG6.OnClose += new TrackEditor.CloseTrackHandler(trackEditorG6_OnClose);

                EditorPro.OnReloadTrack += new TrackEditor.ReloadTrackHandler(EditorPro_OnReloadTrack);
                EditorG5.OnReloadTrack += new TrackEditor.ReloadTrackHandler(EditorG5_OnReloadTrack);

                checkScrollToSelection.CheckedChanged += new EventHandler(checkScrollToSelection_CheckedChanged);
                EditorPro.OnSetChordToScreen += new TrackEditor.SetChordToScreenHandler(EditorPro_OnSetChordToScreen);
                listBoxUSBSongs.Columns[0].Width = listBoxUSBSongs.Width - 2;

                CheckLoadLastFile();

                
            }
        }

        void EditorPro_OnCreationStateChanged(TrackEditor editor, TrackEditor.EditorCreationState newState)
        {
            if (newState == TrackEditor.EditorCreationState.Idle)
            {
                
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingNote)
            {
                SetStatus("Creating Note", label17);
                label17.Text = "Select Start";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingMultiTremelo)
            {
                SetStatus("Creating Multi String Tremelo", label16);
                label16.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingSingleTremelo)
            {
                SetStatus("Creating Single String Tremelo", label18);
                label18.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingSolo)
            {
                SetStatus("Creating Solo", label11);
                label11.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingPowerup)
            {
                SetStatus("Creating Powerup", label10);
                label10.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingArpeggio)
            {
                SetStatus("Creating Arpeggio", label13);
                label13.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.CopyingPattern)
            {
                SetStatus("Copying Pattern", label20);
                label20.Text = "Select Start Note";
            }
            else if (newState == TrackEditor.EditorCreationState.SelectingChordStartOffset)
            {
                SetStatus(button5.Text, null);
                button5.Text = "Cancel";
            }
            else if (newState == TrackEditor.EditorCreationState.SelectingChordEndOffset)
            {
                SetStatus(button8.Text, null);
                button8.Text = "Cancel";
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingProGuitarTrainer)
            {
                labelProGuitarTrainerStatus.Text = "Select Start Note";
                SetStatus("Creating Pro Guitar Trainer", labelProGuitarTrainerStatus);
            }
            else if (newState == TrackEditor.EditorCreationState.CreatingProBassTrainer)
            {
                labelProBassTrainerStatus.Text = "Select Start Note";
                SetStatus("Creating Pro Bass Trainer", labelProBassTrainerStatus);
            }
        }

        bool EditorG5_OnMouseDownEvent(TrackEditor editor, MouseEventArgs e)
        {
            return false;
        }

        bool EditorPro_OnMouseDownEvent(TrackEditor editor, MouseEventArgs e)
        {
            return OnTrackEditorProClick(editor, e);
        }

        void EditorG5_OnReloadTrack(TrackEditor editor, SelectNextEnum selectNext)
        {
            ReloadTrack5(selectNext);
        }

        void EditorPro_OnReloadTrack(TrackEditor editor, SelectNextEnum selectNext)
        {
            ReloadTrackPro(selectNext);
        }

        void checkScrollToSelection_CheckedChanged(object sender, EventArgs e)
        {
            EditorPro.ScrollToSelectionEnabled = checkScrollToSelection.Checked;
            EditorG5.ScrollToSelectionEnabled = checkScrollToSelection.Checked;

            EditorPro.Invalidate();
            EditorG5.Invalidate();
        }

        void EditorPro_OnSetChordToScreen(TrackEditor editor, GuitarChord chord, bool ignoreKeepSelection)
        {
            SetChordToScreen(chord, checkKeepSelection.Checked, ignoreKeepSelection);
        }

        void checkSnapToCloseG5_CheckedChanged(object sender, EventArgs e)
        {
            EditorPro.NoteSnapG5 = checkSnapToCloseG5.Checked;
            EditorG5.NoteSnapG5 = checkSnapToCloseG5.Checked;

            EditorPro.Invalidate();
            EditorG5.Invalidate();
        }

        void checkBoxSnapToCloseNotes_CheckedChanged(object sender, EventArgs e)
        {
            EditorPro.NoteSnapG6 = checkBoxSnapToCloseNotes.Checked;
            EditorG5.NoteSnapG6 = checkBoxSnapToCloseNotes.Checked;

            EditorPro.Invalidate();
            EditorG5.Invalidate();
        }

        void checkBoxGridSnap_CheckedChanged(object sender, EventArgs e)
        {
            EditorPro.GridSnap = checkBoxGridSnap.Checked;
            EditorG5.GridSnap = checkBoxGridSnap.Checked;

            EditorPro.Invalidate();
            EditorG5.Invalidate();
        }

        void EditorG5_OnSelectionStateChange(TrackEditor editor, TrackEditor.EditorSelectionState oldState, TrackEditor.EditorSelectionState newState)
        {
            
        }

        void EditorPro_OnSelectionStateChange(TrackEditor editor, TrackEditor.EditorSelectionState oldState, TrackEditor.EditorSelectionState newState)
        {
            if (newState == TrackEditor.EditorSelectionState.Idle)
            {
                if (statusItem != null)
                {
                    statusItem.Text = "Idle";
                    statusItem = null;
                }
                toolStripCreateStatus.Text = "Idle";

                if (button5.Text == "Cancel")
                {
                    button5.Text = "Start Offset From Point";
                }
                if (button8.Text == "Cancel")
                {
                    button8.Text = "End Offset From Point";
                }
            }
            Invalidate();
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabContainerMain.SelectedTab != null)
                {
                    if (tabContainerMain.SelectedTab == tabPageEvents)
                    {
                        EditorPro.EditMode = TrackEditor.EEditMode.Events;
                        EditorG5.EditMode = TrackEditor.EEditMode.Events;
                    }
                    else if (tabContainerMain.SelectedTab == tabModifierEditor)
                    {
                        EditorPro.EditMode = TrackEditor.EEditMode.Modifiers;
                        EditorG5.EditMode = TrackEditor.EEditMode.Modifiers;
                    }
                    else
                    {
                        EditorPro.EditMode = TrackEditor.EEditMode.Chords;
                        EditorG5.EditMode = TrackEditor.EEditMode.Chords;
                    }
                }
                EditorG5.Invalidate();
                EditorPro.Invalidate();


            }
            catch { }
        }

        void trackEditorG6_OnClose(TrackEditor editor)
        {
            labelStatusIconEditor6.ImageKey = "music--exclamation.png";
            this.FileNamePro = "";
            
            editor.SetHScrollMaximum(0);
            
        }

        void trackEditorG5_OnClose(TrackEditor editor)
        {
            labelStatusIconEditor5.ImageKey = "music--exclamation.png";
            this.FileNameG5 = "";
            editor.SetHScrollMaximum(0);
            
        }

        void trackEditorG5_OnLoadTrack(TrackEditor editor, Sequence seq, Track t)
        {
            labelStatusIconEditor5.ImageKey = "music.png";
            FileNameG5 = editor.LoadedFileName;
            editor.visibleSelectors.Clear();
            RefreshTracks5();
            RefreshModifierListBoxes();
            this.toolStripFileName5.Text = Path.GetFileName(editor.LoadedFileName);
        }


        void trackEditorG6_OnLoadTrack(TrackEditor editor, Sequence seq, Track t)
        {
            if (!trackEditorG6.IsLoaded)
                return;

            labelStatusIconEditor6.ImageKey = "music.png";
            FileNamePro = editor.LoadedFileName;
            
            editor.visibleSelectors.Clear();

            
            RefreshTracks6();


            RefreshModifierListBoxes();
            this.toolStripFileName6.Text = Path.GetFileName(editor.LoadedFileName);
            
        }

        private void CheckLoadLastFile()
        {
            if (checkBoxLoadLastSongStartup.Checked)
            {
                var lastSelectedSongCacheItem = settings.GetValue("lastSelectedSongItem");
                SongCacheItem sciToLoad = null;
                if (!string.IsNullOrEmpty(lastSelectedSongCacheItem))
                {
                    foreach (SongCacheItem sci in SongList)
                    {
                        if (string.Compare(sci.ToString(), lastSelectedSongCacheItem, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            sciToLoad = sci;

                            break;
                        }
                    }
                }

                if (sciToLoad != null)
                {
                    LoadLastFile(sciToLoad);

                }
            }
        }


        private void button61_Click(object sender, EventArgs e)
        {
            var sel = SelectedSong;
            if (sel != null)
            {
                FindDTAInformation(sel);
            }
        }


        private void button70_Click(object sender, EventArgs e)
        {
            UpdateMidiInstrument(false);
        }

        private void button94_Click_1(object sender, EventArgs e)
        {
            PlayMidiFromSelection();
        }

        private void button95_Click_1(object sender, EventArgs e)
        {
            StopMidiPlayback();
        }

        private void button98_Click(object sender, EventArgs e)
        {
            PlayMidiFromSelection();
        }

        private void button97_Click(object sender, EventArgs e)
        {
            StopMidiPlayback();
        }


        private void button57_Click_3(object sender, EventArgs e)
        {
            UpdateEditorProperties();
        }

        private void checkBoxPlayMidiStrum_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBoxSongLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button108_Click(object sender, EventArgs e)
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingNote;


            if (checkBoxUseCurrentChord.Checked)
            {
                EditorPro.CopyChords.Clear();

                var gc = GetChordFromScreen();
                if (gc != null)
                {
                    EditorPro.CopyChords.Add(gc);
                }
            }
        }

        private void button109_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();
        }

        private void radioGrid128thNote_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void button110_Click(object sender, EventArgs e)
        {
            PreviewInstrument(comboMidiInstrument.SelectedIndex, settings.GetValueInt("MidiDeviceInstrument", 0));
        }

        private void button96_MouseDown(object sender, MouseEventArgs e)
        {
            PlayHoldBoxMidi();

        }

        private void buttonRebuildPackage_Click(object sender, EventArgs e)
        {
            RegenDifficultiesClick();
        }

        public class EditorTrackDifficulty
        {
            public PEMidiTrackEditPanel TrackPanel;
            public IEnumerable<TrackDifficulty> Difficulties;
            public TrackDifficulty SelectedTrackDifficulty;
            public EditorTrackDifficulty(PEMidiTrackEditPanel trackPanel)
            {
                this.TrackPanel = trackPanel;
                Difficulties = trackPanel.TrackDifficulties.Select(x => new TrackDifficulty(x.Track, x.Difficulty)).ToList();
                if (trackPanel.SelectedTrack != null && (trackPanel.SelectedTrack.Track != null))
                {
                    SelectedTrackDifficulty = new TrackDifficulty(trackPanel.SelectedTrack.Track, trackPanel.SelectedTrack.SelectedDifficulty);
                }
                else
                {
                    SelectedTrackDifficulty = null;
                }
            }
        }
        public DataPair<EditorTrackDifficulty> GetSelectedTrackDifficulties()
        {
            return new DataPair<EditorTrackDifficulty>(new EditorTrackDifficulty( midiTrackEditorPro), new EditorTrackDifficulty(midiTrackEditorG5));
        }
        public void RestoreTrackDifficulty(DataPair<EditorTrackDifficulty> diff)
        {
            if (diff != null)
            {
                if (diff.A != null && diff.A.SelectedTrackDifficulty != null && diff.A.SelectedTrackDifficulty.Track != null &&
                    diff.A.SelectedTrackDifficulty.Track.Sequence != null)
                {
                    
                    foreach (var item in diff.A.Difficulties.Where(x=> midiTrackEditorPro.TrackDifficulties.Any(y=> y.Track == x.Track && y.Difficulty != x.Difficulty)))
                    {
                        EditorPro.SetTrack(item.Track, item.Difficulty);
                    }
                    
                    EditorPro.SetTrack(diff.A.SelectedTrackDifficulty.Track, diff.A.SelectedTrackDifficulty.Difficulty);
                }
                if (diff.B != null && diff.B.SelectedTrackDifficulty != null && diff.B.SelectedTrackDifficulty.Track != null &&
                    diff.B.SelectedTrackDifficulty.Track.Sequence != null)
                {

                    foreach (var item in diff.B.Difficulties.Where(x => midiTrackEditorG5.TrackDifficulties.Any(y => y.Track == x.Track && y.Difficulty != x.Difficulty)))
                    {
                        EditorG5.SetTrack(item.Track, item.Difficulty);
                    }
                    
                    EditorG5.SetTrack(diff.B.SelectedTrackDifficulty.Track, diff.B.SelectedTrackDifficulty.Difficulty);
                }
            }
        }
        public void ExecAndRestoreTrackDifficulty(Action func)
        {
            var td = GetSelectedTrackDifficulties();
            try
            {
                func();
            }
            finally
            {
                RestoreTrackDifficulty(td);
            }
        }
        public void ExecAndRestoreTrackDifficulty<T>(Action<T> func, T param)
        {
            var td = GetSelectedTrackDifficulties();
            try
            {
                func(param);
            }
            finally
            {
                RestoreTrackDifficulty(td);
            }
        }
        public T ExecAndRestoreTrackDifficulty<T>(Func<T> func)
        {
            var td = GetSelectedTrackDifficulties();
            try
            {
                return func();
            }
            finally
            {
                RestoreTrackDifficulty(td);
            }
        }
        private void RegenDifficultiesClick()
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);

                    listBoxSongLibrary.SelectedItem = SelectedSong;

                    if (SelectedSong.IsDirty || EditorPro.GuitarTrack.Dirty)
                    {
                        var result = AskToSave();
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            SaveSongCacheItem(SelectedSong, false);
                        }
                        else if (result == System.Windows.Forms.DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    var config = new GenDiffConfig(SelectedSong, true, SelectedSong.CopyGuitarToBass, false, false, true);

                    if (!GenerateDifficulties(false, config))
                    {
                        MessageBox.Show("Failed generating difficulties");
                    }
                    else
                    {
                        SavePro();
                    }
                    if (!SaveProCONFile(SelectedSong,  true, false))
                    {
                        MessageBox.Show("Unable to rebuild package");
                    }
                    else
                    {
                        if (CheckCONPackageDTA(SelectedSong, true) == false)
                        {
                            MessageBox.Show("Warnings during rebuild. Run Check.");
                        }
                        else
                        {
                            MessageBox.Show("Rebuild OK");
                        }
                    }

                    ReloadTracks();
                }
            });
        }

        private void saveCONPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedSong == null)
            {
                MessageBox.Show("No Song Selected");
                return;
            }
            UpdateCONFileProperties(SelectedSong);
        }


        private void button100_Click(object sender, EventArgs e)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                CheckCONPackageDTA(SelectedSong, false);
            });
        }

        private void buttonPackageEditorOpenPackage_Click(object sender, EventArgs e)
        {
            string selCon = string.Empty;
            if (SelectedSong != null && !string.IsNullOrEmpty(SelectedSong.G6ConFile))
            {
                selCon = SelectedSong.G6ConFile;
            }
            var selPack = ShowOpenFileDlg("Select Package", DefaultConFileLocation, selCon);
            if (!string.IsNullOrEmpty(selPack))
            {
                try
                {
                    Package p = Package.Load(selPack, false);
                    if (p != null)
                    {
                        if (!LoadPackageIntoTree(p))
                        {
                            MessageBox.Show("Unable to read package");
                        }
                        textBoxPackageDTAText.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Unable to load package");
                    }
                }
                catch { }
            }
        }


        private void buttonCreateSongFromOpenMidi_Click(object sender, EventArgs e)
        {
            CreateSongFromOpenMidi();
        }

        private bool CreateSongFromOpenMidi()
        {
            
            bool ret = false;
            try
            {
                if (EditorPro.IsLoaded || EditorG5.IsLoaded)
                {
                    if (SelectedSong == null)
                    {
                        if (FileNamePro.IsEmpty())
                        {
                            if (InitializeFrom5Tar())
                            {
                                ReloadTracks();

                                if (SelectedSong == null)
                                {
                                    AddNewSongToLibrary();
                                    ret = true;
                                }

                            }
                        }
                        else
                        {
                            AddNewSongToLibrary();
                            ret = true;
                        }
                    }
                }
            }
            catch { ret = false; }
            return ret;
        }

        private void button103_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecuteBatchDifficulty();
        }

        private void button104_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecuteBatchGuitarBassCopy();
        }

        private void button105_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecAndRestoreTrackDifficulty(delegate()
            {
                ExecuteBatchRebuildCON();
            });
        }

        private void button106_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecAndRestoreTrackDifficulty(delegate()
            {
                ExecuteBatchRebuildCON();
            });
        }

        private void button107_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedSong != null && File.Exists(SelectedSong.G6ConFile))
                {
                    var p = Package.Load(SelectedSong.G6ConFile);
                    if (p != null)
                    {
                        LoadPackageIntoTree(p);
                        textBoxPackageDTAText.Text = "";
                        tabContainerMain.SelectedTab = tabPackageEditor;
                        tabControl3.SelectedTab = tabPage8;
                    }
                }
            }
            catch { }
        }

        public bool IsTabEventsActive
        {
            get
            {
                return tabContainerMain.SelectedTab == tabPageEvents;
            }
        }
        public bool IsTabModifiersActive
        {
            get
            {
                return tabContainerMain.SelectedTab == tabModifierEditor;
            }
        }
        private void button96_MouseUp(object sender, MouseEventArgs e)
        {
            StopHoldBoxMidi();
        }
        private void xBoxUSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\resources\\USBXTAFGUI_v36.exe", "");
            }
            catch { }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void testsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            try
            {
                

                EditorPro.ClearSelection();
                CloseSelectedSong();

                if (ShowOpenMidi5())
                {
                    ReloadTracks();
                    if (InitializeFrom5Tar())
                    {
                        ReloadTracks();

                        var fileName = DefaultMidiFileLocationPro.PathCombine(
                            FileNameG5.GetFileNameWithoutExtension().IfNotEmpty(x => 
                                x.PathCombine(Utility.DefaultPROFileExtension)));

                        if (SaveProFile(fileName, false))
                        {
                            var config = new GenDiffConfig(null, true, false, false, false, true);
                            if (GenerateDifficulties(false, config))
                            {
                                ReloadTracks();

                                if (SelectedSong == null)
                                {
                                    AddNewSongToLibrary();
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void checkBoxEnableMidiInput_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            CheckMidiInputVisibility();
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button111_Click(object sender, EventArgs e)
        {
            PUEExtensions.TryExec(delegate()
            {
                int iDevice = comboBoxMidiInput.SelectedIndex;

                if (iDevice != -1)
                {
                    if (inDevice != null)
                    {
                        DisconnectMidiDevice();
                    }
                    var item = comboBoxMidiInput.SelectedItem as MidiInputListItem;
                    if (item != null)
                    {
                        ConnectMidiDevice(item.index, false);
                    }
                }
            });
        }

        private void button113_Click(object sender, EventArgs e)
        {
            RefreshMidiOutputList();
        }


        private void button112_Click(object sender, EventArgs e)
        {
            RefreshMidiInputList();
        }

        private void button114_Click(object sender, EventArgs e)
        {
            PUEExtensions.TryExec(delegate()
            {
                if (inDevice != null && midiPlaybackDevice != null)
                {
                    try
                    {
                        MidiDevice.Connect(inDevice.Handle, midiPlaybackDevice.Handle);
                    }
                    catch { MessageBox.Show("unable to hook"); }
                }
            });
        }

        private void button115_Click(object sender, EventArgs e)
        {
            PUEExtensions.TryExec(delegate()
            {
                if (inDevice != null && midiPlaybackDevice != null)
                {
                    try
                    {
                        MidiDevice.Disconnect(inDevice.Handle, midiPlaybackDevice.Handle);
                    }
                    catch { }
                }
            });
        }

        private void checkView5Button_CheckedChanged(object sender, EventArgs e)
        {
                    }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenMidi5();
        }




        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileNameG5.IfNotEmpty(x=> EditorG5.SaveTrack(FileNameG5));
            }
            catch { }
        }


        private void openPro17ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowOpenFileDlg("Open Rock Band 3 Pro Midi File",
                    DefaultMidiFileLocationPro, "").IfNotEmpty(file =>
                {
                    if (SelectedSong != null)
                    {
                        CloseSelectedSong();
                    }
                    if (!EditorPro.LoadMidi17(file, ReadFileBytes(file), false))
                    {
                        MessageBox.Show("Cannot load file");
                    }
                    EditorPro.ClearBackups();
                    ReloadTracks();
                    RefreshTracks();
                });
            }
            catch { }
        }


        private void saveProToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProFile(FileNamePro, false);
        }
        

        private void buttonInitFromG5_Click(object sender, EventArgs e)
        {
            InitProFromG5();
        }

        public void InitProFromG5()
        {
            try
            {
                EditorPro.BackupSequence();
                
                if (EditorG5.IsLoaded)
                {
                    if (EditorPro.IsLoaded == false)
                    {
                        var p = EditorG5.GuitarTrack.GetTrack().Sequence.ConvertToPro();
                        EditorPro.SetTrack6(p, p.GetPrimaryTrack());
                    }
                    else
                    {
                        var proNames = EditorPro.Sequence.GetGuitarBassTracks().Select(x => x.Name).ToList();
                        proNames.ToList().ForEach(ep =>
                        {
                            
                            if (checkBoxInitSelectedTrackOnly.Checked && EditorPro.IsLoaded && ep != EditorPro.GuitarTrack.Name)
                                return;

                            EditorPro.SetTrack6(EditorPro.GetTrack(ep), EditorPro.CurrentDifficulty);
                            
                            if (ep.IsGuitarTrackName())
                            {
                                EditorG5.SetTrack5(EditorG5.GetGuitar5MidiTrack(), EditorG5.CurrentDifficulty);
                            }
                            else if (ep.IsBassTrackName())
                            {
                                EditorG5.SetTrack5(EditorG5.GetGuitar5BassMidiTrack(), EditorG5.CurrentDifficulty);
                            }

                            var g5t = EditorG5.GuitarTrack.GetTrack();

                            if (checkBoxInitSelectedDifficultyOnly.Checked)
                            {
                                var diff = EditorPro.CurrentDifficulty;
                                if (diff == GuitarDifficulty.Expert)
                                    diff |= GuitarDifficulty.All;

                                EditorPro.GuitarTrack.Remove(EditorPro.Messages.Where(x => diff.HasFlag(x.Difficulty)).ToList());

                                g5t.ConvertToPro().ChanMessages.Where(x =>
                                    {
                                        return x.ChannelMessage.Data1.GetData1Difficulty(true).HasFlag(EditorPro.CurrentDifficulty);
                                    })
                                    .ForEach(x =>
                                    {
                                        EditorPro.GuitarTrack.Insert(x.AbsoluteTicks, x.ChannelMessage);
                                    });
                                EditorPro.SetTrack6(EditorPro.GuitarTrack.GetTrack(), EditorPro.CurrentDifficulty);
                            }
                            else
                            {
                                var cur = EditorPro.GuitarTrack.GetTrack();
                                var name = cur.Name;
                                EditorPro.GuitarTrack.RemoveTrack(cur);
                                
                                if (g5t.IsTempo())
                                {
                                    EditorPro.GuitarTrack.AddTempoTrack(g5t.ConvertToPro());
                                }
                                else
                                {
                                    var newTrack = g5t.ConvertToPro();
                                    newTrack.Name = name;
                                    EditorPro.AddTrack(newTrack);
                                    EditorPro.SetTrack6(EditorPro.Sequence, newTrack);
                                }
                            }
                            
                            ReloadTracks();
                        });
                    }

                    if (SelectedSong == null)
                    {
                        AddNewSongToLibrary();
                    }
                    
                    
                }
            }
            catch 
            {
                MessageBox.Show("Error initializing");
            }
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            ConnectMidiDevice();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DisconnectMidiDevice();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ClearHoldBoxes();
            
        }

        public void ClearChannelBoxes()
        {
            foreach (var cb in NoteChannelBoxes)
            {
                cb.Text = "";
            }
        }



        private void selectNextNote_Click(object sender, EventArgs e)
        {
            SelectNextChord();
        }
        private void updateNoteProperties_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            UpdateSelectedChordProperties(SelectNextEnum.UseConfiguration);
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            RefreshSolosList();

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;


            try
            {
                if (GetSelectedModifier(GuitarModifierType.Solo) != null)
                {
                    EditorPro.ScrollToTick(GetSelectedModifier(GuitarModifierType.Solo).DownTick);
                    
                }
            }
            catch { }


            SelectedModifierChanged(GuitarModifierType.Solo);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            RemoveSelectedModifier(GuitarModifierType.Solo);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            UpdateSelectedModifier(GuitarModifierType.Solo);
        }


        private void button14_Click(object sender, EventArgs e)
        {
            SoloCreateStart();
        }


        private void SoloCreateStart()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingSolo;
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }
        private void button20_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            RemoveSelectedModifier(GuitarModifierType.Powerup);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            RefreshModifierListBox(GuitarModifierType.Powerup);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            UpdateSelectedModifier(GuitarModifierType.Powerup);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            PowerupCreateStart();
        }

        private void PowerupCreateStart()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingPowerup;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SelectedModifierChanged(GuitarModifierType.Powerup);


            try
            {
                if (GetSelectedModifier(GuitarModifierType.Powerup) != null)
                {
                    EditorPro.ScrollToTick(GetSelectedModifier(GuitarModifierType.Powerup).DownTick);
                    
                }
            }
            catch { }
            EditorPro.Invalidate();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SelectedModifierChanged(GuitarModifierType.Arpeggio);



            try
            {
                if (GetSelectedModifier(GuitarModifierType.Arpeggio) != null)
                {
                    EditorPro.ScrollToTick(GetSelectedModifier(GuitarModifierType.Arpeggio).DownTick);
                    
                }
            }
            catch { }
            EditorPro.Invalidate();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (Utility.GetArpeggioData1(EditorPro.CurrentDifficulty).IsNull())
                return;

            EditorPro.BackupSequence();
            DeleteArpeggio();

        }


        private void button26_Click(object sender, EventArgs e)
        {
            RefreshModifierListBox(GuitarModifierType.Arpeggio);
        }

        private void button23_Click(object sender, EventArgs e)
        {

            if (Utility.GetArpeggioData1(EditorPro.CurrentDifficulty).IsNull())
                return;
            EditorPro.BackupSequence();
            UpdateSelectedModifier(GuitarModifierType.Arpeggio);

        }

        private void button24_Click(object sender, EventArgs e)
        {
            ArpeggioCreateStart();
        }

        private void ArpeggioCreateStart()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingArpeggio;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }

        private void button27_Click(object sender, EventArgs e)
        {
            Utility.timeScalar = textBoxZoom.Text.ToDouble(250);
            
            ReloadTracks();

        }

        private void button29_Click(object sender, EventArgs e)
        {
            BeginCopyPattern();
        }

        private void BeginCopyPattern()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CopyingPattern;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }


        private void button31_Click(object sender, EventArgs e)
        {
            CopyBigRockEnding();
        }


        private void button30_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            ReplaceMatch();
        }
        private void button32_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            if (!CombineNextNote())
            {
                UndoLast();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            AddSlideHOPO();
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (EditorPro.CreationState == TrackEditor.EditorCreationState.SelectingChordStartOffset)
            {
                button5.Text = "Start Offset From Point";
                EditorPro.CreationState = TrackEditor.EditorCreationState.Idle;
            }
            else
            {
                EditorPro.CreationState = TrackEditor.EditorCreationState.SelectingChordStartOffset;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (EditorPro.CreationState == TrackEditor.EditorCreationState.SelectingChordEndOffset)
            {
                button8.Text = "End Offset From Point";
                EditorPro.CreationState = TrackEditor.EditorCreationState.Idle;
            }
            else
            {
                EditorPro.CreationState = TrackEditor.EditorCreationState.SelectingChordEndOffset;
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                var hb = GetHoldBoxes();
                for (int x = 0; x < 5; x++)
                {
                    hb[x].Text = hb[x + 1].Text;
                    if (checkIndentBString.Checked && x == 1)
                    {
                        int i = hb[x].Text.ToInt();
                        if (!i.IsNull())
                        {
                            i++;
                            hb[x].Text = i.ToString();
                        }
                    }
                }
                hb[5].Text = "";
            }
            catch { }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                var hb = GetHoldBoxes();
                for (int x = 5; x > 0; x--)
                {
                    hb[x].Text = hb[x - 1].Text;

                    if (checkIndentBString.Checked == true &&
                        x == 2)
                    {
                        int i = hb[x].Text.ToInt();
                        if (!i.IsNull())
                        {
                            i--;
                            hb[x].Text = i.ToString();
                        }
                    }
                }
                hb[0].Text = "";
            }
            catch { }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            var hb = GetHoldBoxes();
            for (int x = 0; x < 6; x++)
            {
                int i = hb[x].Text.ToInt();
                if (!i.IsNull())
                {
                    var v = i + 2;
                    if (v > 22)
                        v = 22;
                    hb[x].Text = v.ToString();
                }
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            var hb = GetHoldBoxes();
            for (int x = 0; x < 6; x++)
            {
                int i = hb[x].Text.ToInt();
                if (!i.IsNull())
                {
                    var v = i - 2;
                    if (v < 0)
                        v = 0;
                    hb[x].Text = v.ToString();
                }
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            var hb = GetHoldBoxes();
            for (int x = 0; x < 6; x++)
            {
                int i = hb[x].Text.ToInt();
                if (!i.IsNull())
                {
                    var v = i + 1;
                    if (v > 22)
                        v = 22;
                    hb[x].Text = v.ToString();
                }
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            var hb = GetHoldBoxes();
            for (int x = 0; x < 6; x++)
            {
                int i = hb[x].Text.ToInt();
                if (!i.IsNull())
                {
                    var v = i - 1;
                    if (v < 0)
                        v = 0;
                    hb[x].Text = v.ToString();
                }
            }
        }


        private void buttonStrumNone_Click(object sender, EventArgs e)
        {
            ClearStrumMarkers();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SelectedModifierChanged(GuitarModifierType.MultiStringTremelo);

            try
            {
                if (GetSelectedModifier(GuitarModifierType.MultiStringTremelo) != null)
                {
                    EditorPro.ScrollToTick(GetSelectedModifier(GuitarModifierType.MultiStringTremelo).DownTick);
                }
            }
            catch { }
            EditorPro.Invalidate();

        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            RefreshModifierListBox(GuitarModifierType.MultiStringTremelo);
        }

        private void button40_Click(object sender, EventArgs e)
        {
            UpdateSelectedModifier(GuitarModifierType.MultiStringTremelo);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            BeginMultiStringTremeloCreate();
        }

        private void BeginMultiStringTremeloCreate()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingMultiTremelo;
            
        }

        private void button39_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (!CopySolosFromG5(checkBoxInitSelectedTrackOnly.Checked == false))
            {
                MessageBox.Show("Failed copying solos");
            }
            EditorPro.Invalidate();
        }
        private void button42_Click(object sender, EventArgs e)
        {
            RemoveSelectedModifier(GuitarModifierType.MultiStringTremelo);

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SelectedModifierChanged(GuitarModifierType.SingleStringTremelo);


            try
            {
                if (GetSelectedModifier(GuitarModifierType.SingleStringTremelo) != null)
                {
                    EditorPro.ScrollToTick(GetSelectedModifier(GuitarModifierType.SingleStringTremelo).DownTick);
                    
                }
            }
            catch { }
            EditorPro.Invalidate();

        }

        private void button48_Click(object sender, EventArgs e)
        {
            RemoveSelectedModifier(GuitarModifierType.SingleStringTremelo);

        }

        private void button49_Click(object sender, EventArgs e)
        {
            RefreshModifierListBox(GuitarModifierType.SingleStringTremelo);

        }

        private void button46_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            UpdateSelectedModifier(GuitarModifierType.SingleStringTremelo);

        }

        private void button47_Click(object sender, EventArgs e)
        {
            BeginSingleStringTremeloCreate();
        }

        private void BeginSingleStringTremeloCreate()
        {
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingSingleTremelo;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();

        }


        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            
            SetStoredChordToEditor(GetSelectedStoredChord());
        }
        private void button51_Click(object sender, EventArgs e)
        {
            if (listBoxStoredChords.SelectedIndex != -1)
            {
                listBoxStoredChords.Items.Remove(listBoxStoredChords.SelectedItem);
                listBoxStoredChords.SelectedItem = null;
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            listBoxStoredChords.Items.Clear();
        }

        private void listBox6_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            listBox6_SelectedIndexChanged(null, null);
        }

        private void button53_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            ExtendToNextNote();
        }
        private void button54_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            SplitNote();
        }
        private void textBox20_TextChanged(object sender, EventArgs ev)
        {
            if (DesignMode)
                return;

            var s = GetChordStartTick();
            int e = GetChordEndTick();


            if (!s.IsNull() && !e.IsNull())
            {
                textBox19.SuspendLayout();
                textBox19.Text = (e - s).ToStringEx();
                textBox19.ResumeLayout();
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs ve)
        {
            if (DesignMode)
                return;

            var s = GetChordStartTick();

            var ltb = textBox19.Text;
            int lt = ltb.ToInt();
            if (!s.IsNull() && !lt.IsNull())
            {
                GetChordEndBox().Text = (s + lt).ToStringEx();
            }

        }



        private void button55_Click(object sender, EventArgs e)
        {
            SelectPreviousChord();
        }
        private void button57_Click(object sender, EventArgs e)
        {
            UndoLast();
        }
        private void button58_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            RemoveAllArpegios();

        }
        private void button60_Click(object sender, EventArgs e)
        {
            
        }
        private void EditorG5_Click(object sender, EventArgs e)
        {
            
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorPro.RestoreBackup();
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = EditorPro.NumBackups > 0;
        }


        private void radioDifficultyExpert_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SetEditorDifficulty(GuitarDifficulty.Expert);
        }

        private void radioDifficultyHard_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            SetEditorDifficulty(GuitarDifficulty.Hard);
        }

        private void radioDifficultyMedium_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            SetEditorDifficulty(GuitarDifficulty.Medium);
        }

        private void radioDifficultyEasy_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            SetEditorDifficulty(GuitarDifficulty.Easy);
        }

        private void button59_Click(object sender, EventArgs e)
        {
            try
            {
                var g6t = ProGuitarTrack;
                if (g6t == null)
                    return;

                CopyDifficulties();

                ReloadTracks();
            }
            catch
            {
                EditorPro.RestoreBackup();
            }
        }


        private void button56_Click(object sender, EventArgs e)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                var genConfig = new GenDiffConfig(SelectedSong, true,
                    checkGenDiffCopyGuitarToBass.Checked,
                    checkBoxInitSelectedDifficultyOnly.Checked,
                    checkBoxInitSelectedTrackOnly.Checked, true);

                genConfig.CopyGuitarToBass = checkGenDiffCopyGuitarToBass.Checked;

                GenerateDifficulties(false, genConfig);
            });
        }
        private void buttonCreatePackage_Click(object sender, EventArgs e)
        {
            SaveCONPackageAs();
        }
        private void button62_Click(object sender, EventArgs e)
        {
            var s = ShowSelectFolderDlg("Select default CON folder",
                DefaultConFileLocation, "");
            if (!string.IsNullOrEmpty(s))
            {
                DefaultConFileLocation = s;
            }
        }

        private void button63_Click(object sender, EventArgs e)
        {
            var s = ShowSelectFolderDlg("Select default Midi 5 folder", DefaultMidiFileLocationG5, "");
            if (s != string.Empty)
            {
                DefaultMidiFileLocationG5 = s;
            }
        }

        private void button64_Click(object sender, EventArgs e)
        {
            var s = ShowSelectFolderDlg("Select default Pro Midi folder", DefaultMidiFileLocationPro, "");
            if (s != string.Empty)
            {
                DefaultMidiFileLocationPro = s;
            }
        }

        private void button65_Click(object sender, EventArgs e)
        {
            var path = textBox21.Text;

            if (string.IsNullOrEmpty(path))
                path = textBox22.Text;

            var s = ShowOpenFileDlg("Select midi file",
                DefaultMidiFileLocationG5, path);
            if (!string.IsNullOrEmpty(s))
            {
                textBox21.Text = s;
                textBox21.ScrollToEnd();
                
                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);
                    
                    OpenSongCacheItem(SelectedSong);
                }
            }
        }

        private void button66_Click(object sender, EventArgs e)
        {
            var path = textBox22.Text;

            if (string.IsNullOrEmpty(path))
                path = textBox21.Text;

            var s = ShowOpenFileDlg("Select pro midi file",
                DefaultMidiFileLocationPro, path);
            if (!string.IsNullOrEmpty(s))
            {
                textBox22.Text = s;
                textBox22.ScrollToEnd();

                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);

                    OpenSongCacheItem(SelectedSong);
                }
            }
        }


        private void button68_Click(object sender, EventArgs e)
        {
            SongList.SelectedSong = listBoxSongLibrary.SelectedItem as SongCacheItem;
            OpenSongCacheItem(SongList.SelectedSong);
        }
        private void button67_Click(object sender, EventArgs e)
        {
            UpdateSongCacheItem(SelectedSong);
        }
        private void button69_Click(object sender, EventArgs e)
        {
            try
            {
                var sng = listBoxSongLibrary.SelectedItem as SongCacheItem;
                if (sng != null)
                {
                    if (MessageBox.Show("Confirm Song Removal: " + sng.Description, "Removing Song", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;

                    var docLibRoot = XMLUtil.GetNode(settings.XMLRoot, "docLib");
                    if (docLibRoot != null)
                    {
                        string path = string.Format("song[@CacheSongID='{0}']", sng.CacheSongID);
                        var node = XMLUtil.GetNode(docLibRoot, path);
                        if (node != null)
                        {
                            docLibRoot.RemoveChild(node);
                        }
                    }

                    SongList.RemoveSong(sng);
                }
            }
            catch { }
        }

        private void button71_Click(object sender, EventArgs e)
        {
            var fileName = textBoxSongLibConFile.Text;
            var folder = DefaultConFileLocation;

            if(fileName.IsEmpty())
            {
                if(SelectedSong != null)
                {
                    var fn = (SelectedSong.G6FileName.GetIfEmpty(SelectedSong.G5FileName) ?? "").GetFileNameWithoutExtension();
                    if (fn.EndsWithEx("_pro"))
                        fn = fn.Replace("_pro", "");
                    fileName = fn + Utility.DefaultCONFileExtension;
                }
            }
            else if(fileName.FileExists())
            {
                fileName = fileName.GetFileName();
                folder = fileName.GetFolderName();
            }
            
            ShowSaveFileDlg("Select pro CON file",
                folder,
                fileName).IfNotEmpty(newFileName =>
            {
                textBoxSongLibConFile.Text = newFileName;
                textBoxSongLibConFile.ScrollToEnd();
                
                UpdateSongCacheItem(SelectedSong);
                SaveProCONFile(SelectedSong, false, false);
            });
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            SongList.SelectedSong = listBoxSongLibrary.SelectedItem as SongCacheItem;
            OpenSongCacheItem(SongList.SelectedSong);
        }
        private void saveCONPackageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCONPackageAs();
        }


        private void button60_Click_1(object sender, EventArgs e)
        {
            ExtractPackageToFolder(true);
        }
        private void treePackageContents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowSelectedPackage();
        }
        private void button74_Click(object sender, EventArgs e)
        {
            button74.Enabled = false;
            CopyAllConToLocation();
            button74.Enabled = true;
        }
        private void button75_Click(object sender, EventArgs e)
        {
            GetPathCopyAllCON();
        }

        private void button80_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBox21.Text);
        }

        private void button79_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBox22.Text);
        }

        private void button78_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBoxSongLibConFile.Text);
        }

        private void button81_Click(object sender, EventArgs e)
        {
            ExtractPackageToFolder(false);
        }

        private void buttonSongLibCancel_Click(object sender, EventArgs e)
        {
            buttonSongLibCancel.Enabled = false;

        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        //{
        {

            if (DesignMode)
                return;

            if (checkBoxMultiSelectionSongList.Checked)
            {
                listBoxSongLibrary.SelectionMode = SelectionMode.MultiExtended;
            }
            else
            {
                listBoxSongLibrary.SelectionMode = SelectionMode.One;
            }
        }

        private void button77_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();

            ReplaceBassWithGuitar();

        }

        public void ReplaceBassWithGuitar()
        {
            try
            {
                
                CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17);
                
                CopyTrack(GuitarTrack.GuitarTrackName22, GuitarTrack.BassTrackName22);

                ReloadTracks();
            }
            catch { }
        }

        private void button83_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        public void ZoomOut()
        {
            if (!reloading)
            {
                if (Utility.timeScalar > 1)
                {
                    if (Utility.timeScalar <= 5 && Utility.timeScalar >= 2)
                    {
                        Utility.timeScalar -= 1;
                    }
                    else if (Utility.timeScalar > 5 && Utility.timeScalar < 25)
                    {
                        Utility.timeScalar -= 5;
                    }
                    else if (Utility.timeScalar >= 25)
                    {
                        Utility.timeScalar -= 15;
                    }
                    if (Utility.timeScalar < 1)
                        Utility.timeScalar = 1;

                    this.textBoxZoom.Text = Utility.timeScalar.ToString();

                    if (EditorsValid)
                    {
                        if (SelectedChord == null)
                        {
                            var vc = EditorPro.GetVisibleChords();
                            if (vc != null && vc.Any())
                            {
                                SetSelectedChord(vc.First(), true);
                            }
                        }
                    }
                    ReloadTracks();
                    ScrollToSelection();

                }
            }
        }

        private void button84_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        public void ZoomIn()
        {
            if (!reloading)
            {
                if (Utility.timeScalar < 1200)
                {
                    if (Utility.timeScalar < 5)
                    {
                        Utility.timeScalar += 1;
                    }
                    else if (Utility.timeScalar >= 5 && Utility.timeScalar < 25)
                    {
                        Utility.timeScalar += 5;
                    }
                    else if (Utility.timeScalar >= 25)
                    {
                        Utility.timeScalar += 15;
                    }
                }
                this.textBoxZoom.Text = Utility.timeScalar.ToString();

                if (EditorsValid)
                {
                    if (SelectedChord == null)
                    {
                        var vc = EditorPro.GetVisibleChords();
                        if (vc != null && vc.Any())
                        {
                            SetSelectedChord(vc.First(), true);
                        }
                    }
                }

                ReloadTracks();
                ScrollToSelection();

            }
        }

        private void button85_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            AddSlideNote();
        }

        private void button86_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            AddHOPONote();
        }

        
        GuitarChord SelectedChord
        {
            get
            {
                return EditorPro.SelectedChord;
            }
        }

        GuitarTrack ProGuitarTrack
        {
            get
            {
                return EditorPro.GuitarTrack;
            }
        }

        GuitarTrack G5Track
        {
            get
            {
                return EditorG5.GuitarTrack;
            }
        }

        private void button87_Click(object sender, EventArgs e)
        {
            click_FindNextStoredChordMatch();
        }

        private void click_FindNextStoredChordMatch()
        {
            try
            {
                label15.Text = "________";

                var sc = GetSelectedStoredChord();
                if (sc == null)
                    return;

                GuitarChord start = null;
                if (EditorPro.SelectedChords != null)
                {
                    var chord = EditorPro.SelectedChords;
                    if (chord != null)
                    {
                        start = chord.LastOrDefault();
                    }
                }

                bool includeFirst = false;
                if (start == null)
                {
                    start = EditorPro.Messages.Chords.FirstOrDefault();
                    includeFirst = true;
                }

                var result = FindNextStoredChordMatch(start, sc, includeFirst);

                if (result != null)
                {
                    label15.Text = "Found Match";

                    SetSelectedChord(result, false);
                    ScrollToSelection();
                }
                else
                {
                    label15.Text = "No Match";
                }
            }
            catch { }
        }

        public GuitarChord FindNextStoredChordMatch(GuitarChord start, StoredChord chord, bool includeFirst = false)
        {
            GuitarChord ret = null;
            if (chord == null)
                return ret;

            try
            {
                
                int startOffset = -1;
                if (start != null)
                {
                    startOffset = start.DownTick;
                }
                

                var config = GetStoredChordSearchConfigFromScreen();

                ret = ProGuitarTrack.Messages.Chords.FirstOrDefault(
                    x => (includeFirst ? x.DownTick >= startOffset : x.DownTick > startOffset) &&
                        IsChordSameAsStoredChord(x, chord,
                        config.SearchByNoteType,
                        config.SearchByNoteFret,
                        config.MatchAllFrets,
                        config.SearchByNoteStrum) == true);

                
            }
            catch { }

            return ret;
        }

        private void button88_Click(object sender, EventArgs e)
        {
            click_FindPreviousStoredChordMatch();
        }

        public class StoredChordSearchConfig
        {
            public bool SearchByNoteType;
            public bool MatchAllFrets;
            public bool SearchByNoteFret;
            public bool SearchByNoteStrum;
        }

        public StoredChordSearchConfig GetStoredChordSearchConfigFromScreen()
        {
            return new StoredChordSearchConfig()
            {
                SearchByNoteType = checkBoxSearchByNoteType.Checked,
                SearchByNoteFret = checkBoxSearchByNoteFret.Checked,
                MatchAllFrets = checkBoxMatchAllFrets.Checked,
                SearchByNoteStrum = checkBoxSearchByNoteStrum.Checked,
            };
        }

        public StoredChord SelectedStoredChord
        {
            get
            {
                return GetSelectedStoredChord();
            }
        }

        
        public GuitarChord FindFirstStoredChordMatch(StoredChord sc)
        {
            GuitarChord ret = null;

            try
            {
                if (SelectedStoredChord == null)
                    return ret;

                var fc = EditorPro.Messages.FirstChord;
                if (fc != null)
                {
                    ret = FindNextStoredChordMatch(fc, SelectedStoredChord, true);
                }
            }
            catch { ret = null; }

            return ret;
        }

        public GuitarChord FindLastStoredChordMatch()
        {
            GuitarChord ret = null;

            try
            {
                if (SelectedStoredChord == null)
                    return ret;

                
                var sc = EditorPro.Messages.LastChord;
                if (sc != null)
                {
                    ret = FindPreviousStoredChordMatch(sc, SelectedStoredChord, true);
                }
                
            }
            catch { ret = null; }

            return ret;
        }

        public StoredChord GetStoredChordFromChord(GuitarChord chord)
        {
            StoredChord ret = null;

            try
            {
                var curr = GetChordFromScreen();
                SetChordToScreen(chord, false);

                ret = GetStoredChordFromScreen();
            }
            catch{}

            return ret;
        }

        public GuitarChord FindPreviousStoredChordMatch(GuitarChord start, StoredChord sc, bool includeStart = false)
        {
            GuitarChord ret = null;
            try
            {
                

                int startOffset = -1;
                if (start != null)
                {
                    startOffset = start.DownTick;
                }

                var config = GetStoredChordSearchConfigFromScreen();

                ret = ProGuitarTrack.Messages.Chords.LastOrDefault(
                    x => (includeStart ? x.DownTick <= startOffset : x.DownTick < startOffset) &&
                        IsChordSameAsStoredChord(x, sc,
                        config.SearchByNoteType, config.SearchByNoteFret,
                        config.MatchAllFrets, config.SearchByNoteStrum) == true);
            }
            catch { }
            return ret;
        }

        private void click_FindPreviousStoredChordMatch()
        {
            

            try
            {

                var sc = GetSelectedStoredChord();
                if (sc == null)
                    return;

                var sel = SelectedChord;
                bool includeStart = false;
                if (sel == null)
                {
                    sel = EditorPro.Messages.LastChord;
                    includeStart = true;
                }

                var match = FindPreviousStoredChordMatch(sel, sc, includeStart);

                if (match != null)
                {
                    SetSelectedChord(match, false);
                    ScrollToSelection();
                    label15.Text = "Found Match";
                }
                else
                {
                    label15.Text = "No Match";
                }
            }
            catch { }
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            CheckNoteChannelVisibility();
        }



        private void checkViewNotesGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            CheckNotesGridSelection();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void button89_Click(object sender, EventArgs e)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                ExecuteBatch();
            });
        }

        private void ExecuteBatch()
        {
            try
            {
                if (FileNamePro.IsEmpty())
                {
                    SaveProFile(FileNamePro, true);
                    EditorPro.ClearBackups();
                }
            }
            catch { }

            ClearBatchResults();
            bool batchOK = true;

            WriteBatchResult("Beginning Batch");

            if (checkBoxBatchGenerateDifficulties.Checked)
            {
                batchOK = ExecuteBatchDifficulty();
            }
            if (checkBatchCopyTextEvents.Checked)
            {
                batchOK = ExecuteBatchBuildTextEvents();
            }
            if (batchOK && checkBoxBatchGuitarBassCopy.Checked)
            {
                batchOK = ExecuteBatchGuitarBassCopy();
            }
            if (batchOK && checkBoxBatchRebuildCON.Checked)
            {
                batchOK = ExecuteBatchRebuildCON();
            }
            if (batchOK && checkBoxBatchCheckCON.Checked)
            {
                batchOK = ExecuteBatchCheckCON();
            }
            if (batchOK && checkBoxBatchCopyUSB.Checked)
            {
                batchOK = ExecuteBatchCopyUSB();
            }

            WriteBatchResult("Batch Completed");
            try
            {
                EditorPro.ClearBackups();
                ReloadTracks();
            }
            catch { }

            if (batchOK == false)
            {
                WriteBatchResult("Batch Failed");
            }
        }



        private void button72_Click(object sender, EventArgs e)
        {
            GetCopyAllProFolder();
        }


        private void button73_Click(object sender, EventArgs e)
        {
            CopyAllProToLocation();
        }
        private void button82_Click(object sender, EventArgs e)
        {
            GetCopyAllG5Folder();
        }



        private void button90_Click(object sender, EventArgs e)
        {
            CopyAllG5MidiToLocation();
        }


        private void button91_Click(object sender, EventArgs e)
        {
            try
            {
                var pkPath = textBox49.Text;

                if (!pkPath.FileExists())
                {
                    pkPath = ShowOpenFileDlg("Select CON Package To Extract",
                        DefaultConFileLocation, pkPath);

                    if (pkPath.IsNotEmpty())
                        textBox49.Text = pkPath;
                }

                if (pkPath.FileExists())
                {
                    Package pk = null;
                    try
                    {
                        pk = Package.Load(pkPath, false);
                    }
                    catch { } 
                    
                    if (pk == null)
                    {
                        MessageBox.Show("Cannot load package");
                        return;
                    }
                    
                    
                    var targetPath = settings.GetValue("lastExtractConTargetFolder");
                    if (targetPath.IsEmpty())
                        targetPath = settings.GetValue("lastExtractConSourceFolder");

                    var sel = ShowSelectFolderDlg("Select Package Output Path", DefaultConFileLocation, targetPath);
                    if (sel.IsNotEmpty())
                    {
                        settings.SetValue("lastExtractConTargetFolder", targetPath);

                        string[] filters = null;
                        if (checkBox11.Checked)
                        {
                            filters = new string[] { ".dta", ".mid", ".midi" };
                        }
                        if (ExtractPackageContents(sel, pk.RootFolder, filters))
                        {
                            OpenExplorerFolder(sel);
                        }
                    }
                
                }
            }
            catch { }
        }


        private void button92_Click(object sender, EventArgs e)
        {
            try
            {
                string pkPath = textBox25.Text;
                if (string.IsNullOrEmpty(pkPath))
                {
                    pkPath = ShowSelectFolderDlg("Select Package Directory", settings.GetValue("lastExtractConSourceFolder"), pkPath);
                    if (!string.IsNullOrEmpty(pkPath))
                        textBox25.Text = pkPath;
                }
                if (!string.IsNullOrEmpty(pkPath))
                {
                    var files = Directory.GetFiles(pkPath);

                    if (files.Length > 0)
                    {

                        settings.SetValue("lastExtractConSourceFolder", pkPath);


                        var targetPath = settings.GetValue("lastExtractConTargetFolder");
                        if (string.IsNullOrEmpty(targetPath))
                            targetPath = settings.GetValue("lastExtractConSourceFolder");
                        if (string.IsNullOrEmpty(targetPath))
                        {
                            targetPath = pkPath;
                        }
                        var sel = ShowSelectFolderDlg("Select Output Path", targetPath, "");
                        if (!string.IsNullOrEmpty(sel))
                        {
                            settings.SetValue("lastExtractConTargetFolder", sel);
                            foreach (string fileName in files)
                            {
                                Package pk = null;
                                try
                                {
                                    pk = Package.Load(fileName);
                                }
                                catch { }

                                if (pk != null)
                                {
                                    var folder = Path.Combine(sel, Path.GetFileNameWithoutExtension(fileName));

                                    if (!Directory.Exists(folder))
                                    {
                                        Directory.CreateDirectory(folder);
                                    }

                                    ExtractPackageContents(folder, pk.RootFolder, null);
                                }
                            }
                            OpenExplorerFolder(targetPath);
                        }
                    }
                }

            }
            catch { }
        }

        private void button93_Click(object sender, EventArgs e)
        {
            try
            {
                var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);

                StringBuilder sb = new StringBuilder();
                foreach (SongCacheItem item in songs)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetText(sb.ToString());
            }
            catch { }
        }
        

        private void button94_Click(object sender, EventArgs e)
        {

        }

        private void saveProAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProFile("", false);
        }

        private void placeNote_Click(object sender, EventArgs e)
        {
            try
            {
                if (HasInvalidStrumSelected())
                    return;

                PlaceNote(SelectNextEnum.UseConfiguration);
            }
            catch { }
        }

        private void button50_Click(object sender, EventArgs e)
        {
            StoreScreenChord();
        }

        private void button116_Click(object sender, EventArgs e)
        {
            UpdateScreenChord();
        }

        private void button117_Click(object sender, EventArgs e)
        {
            ShowSelectCompressAllZipFile();
        
        }

        private string ShowSelectCompressAllZipFile()
        {
            var ret = ShowSaveFileDlg("Select Output Zip File", "", textBoxCompressAllZipFile.Text);
            if (!ret.IsEmpty())
            {
                textBoxCompressAllZipFile.Text = ret;
            }
            else
            {
                ret = string.Empty;
            }
            return ret;
        }

        private void button118_Click(object sender, EventArgs e)
        {
            var zipFileName = textBoxCompressAllZipFile.Text;
            if (zipFileName.IsEmpty())
            {
                zipFileName = ShowSelectCompressAllZipFile();
            }

            if(zipFileName.IsEmpty())
                return;

            
            try
            {
                var fileNames = new List<string>();

                if (checkBoxCompressAllInDefaultCONFolder.Checked)
                {
                    var folderToCompress = DefaultConFileLocation;
                    if (folderToCompress.IsEmpty())
                    {
                        folderToCompress = ShowSelectFolderDlg("Select folder to compress", "", "");
                    }

                    if (folderToCompress.FolderExists())
                    {
                        fileNames.AddRange(folderToCompress.GetFilesInFolder());
                    }
                }
                else
                {
                    foreach (var song in SongList)
                    {
                        if (song.IsComplete && 
                            song.G6ConFile.FileExists())
                        {
                            fileNames.Add(song.G6ConFile);
                        }
                    }
                }


                if (fileNames.Any())
                {
                    if (CreateZipFile(zipFileName, fileNames))
                    {
                        OpenExplorerFolder(zipFileName.GetFolderName());
                    }
                }
            }
            catch { MessageBox.Show("Error saving"); }
            
        }

        public bool CreateZipFile(string zipFileName, IEnumerable<string> localFilePaths)
        {
            var ret = false;
            ZipOutputStream zipStream = null;
            try
            {
                using (zipStream = new ZipOutputStream(File.Create(zipFileName)))
                {
                    zipStream.SetLevel(3);

                    foreach (var localFilePath in localFilePaths.Where(x=> x.FileExists()))
                    {
                        var entry = new ZipEntry(localFilePath.GetFileName());

                        var fileBytes = localFilePath.ReadFileBytes();

                        entry.DateTime = File.GetLastWriteTime(localFilePath);

                        entry.Size = fileBytes.Length;

                        zipStream.PutNextEntry(entry);
                        
                        zipStream.Write(fileBytes, 0, fileBytes.Length);
                        
                        zipStream.CloseEntry();
                    }
                }
            }
            finally 
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.IsStreamOwner = true;
                    zipStream.Close();
                    zipStream = null;
                }
            }
            return ret;
        }
        public bool ExtractZipFile(string zipFileName, 
            string outputFolder, 
            IEnumerable<string> extensions, bool overWriteExisting=false)
        {
            var ret = false;
            ZipFile zip = null;
            try
            {
                if (zipFileName.IsNotEmpty() && zipFileName.FileExists() && !outputFolder.IsEmpty())
                {
                    zip = new ZipFile(zipFileName);

                    byte[] buffer = new byte[4096];
                    try
                    {
                        foreach (var file in zip.ToEnumerable<ZipEntry>().Where(x => x.IsFile))
                        {
                            PUEExtensions.TryExec(delegate()
                            {
                                var entryName = ZipEntry.CleanName(file.Name);
                                entryName = entryName.Replace('/', '\\').Trim();

                                if (extensions == null || extensions.Any() == false ||
                                    extensions.Any(x => entryName.EndsWithEx(x)))
                                {
                                    var outputFileName = outputFolder.PathCombine(entryName);

                                    if (overWriteExisting || !outputFileName.FileExists())
                                    {
                                        outputFileName.GetFolderName().CreateFolderIfNotExists();

                                        Stream zipStream = zip.GetInputStream(file);

                                        using (FileStream streamWriter = File.Create(outputFileName))
                                        {
                                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                                        }
                                    }
                                }
                            });
                        }

                        ret = true;
                    }
                    finally
                    {
                        zip.Close();
                        zip = null;
                    }
                }
                
            }
            catch 
            {
                if (zip != null)
                {
                    zip.Close();
                    zip = null;
                }
            }
            return ret;
        }

        private void checkBoxRenderMouseSnap_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

        }

        private void button119_Click(object sender, EventArgs e)
        {
            int i = textBoxNoteSnapDistance.Text.ToInt();
            if (!i.IsNull())
            {
                if (i > 0 && i < 100000)
                {
                    Utility.NoteSnapDistance = i;
                }
            }
            i = textBoxGridSnapDistance.Text.ToInt();
            if (!i.IsNull())
            {
                if (i >0 && i < 100000)
                {
                    Utility.GridSnapDistance = i;
                }
            }
        }

        private void listG6Tracks_Click(object sender, EventArgs e)
        {
            
        }

        private void listG5Tracks_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBoxUseCurrentChord_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            if (EditorPro.CreationState == TrackEditor.EditorCreationState.CreatingNote)
            {
                if (checkBoxUseCurrentChord.Checked)
                {
                    EditorPro.CopyChords.Clear();
                    var gc = GetChordFromScreen();
                    if (gc != null)
                    {
                        EditorPro.CopyChords.Add(gc);
                    }
                }
            }
            
        }

        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            SaveSettingConfiguration();
        }
        private void saveConfigurationAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            var selConfig = ShowSaveFileDlg("Save Configuration Settings",
                settings.SettingFilePath, settings.SettingFileName);
            if (!string.IsNullOrEmpty(selConfig))
            {
                
                SaveSettingConfiguration();
                settings.SettingFilePath = selConfig;
            }
        }
        private void openConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            var selConfig = ShowOpenFileDlg("Load Configuration Settings",
                settings.SettingFilePath, settings.SettingFileName);
            
            if (!string.IsNullOrEmpty(selConfig))
            {
                if (settings.XMLRoot != null && File.Exists(settings.SettingFilePath))
                {
                    SaveSettingConfiguration();
                }
                settings.SettingFilePath = selConfig;

                LoadSettingConfiguration();
            }
        }

        public GuitarChord[] FindPreviousMatchingPattern()
        {
            var result = FindMatchingCopyPattern(false, true, true);

            if (result != null)
            {
                return result.Matches.LastOrDefault();
            }
            else
            {
                return null;
            }
        }

        public GuitarChord[] FindNextMatchingPattern()
        {
            var result = FindMatchingCopyPattern(true, false, true);


            if (result != null)
            {
                return result.Matches.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private void buttonReplaceFindPrev_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            var result = FindPreviousMatchingPattern();

            if (result != null && result.Length>0)
            {
                EditorPro.ClearSelection();

                var gc1 = EditorPro.GetStaleChord(result[0], false);
                
                SetSelectedChord(gc1, false);
                foreach (var gc in result)
                {
                    gc1 = EditorPro.GetStaleChord(gc, false);
                    if(gc1 != null)
                    {
                        gc1.Selected = true;
                    }
                }
                ScrollToSelection();
                EditorPro.Invalidate();
            }
        }

        private void buttonReplaceFindNext_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            var result = FindNextMatchingPattern();

            if (result != null && result.Length > 0)
            {
                EditorPro.ClearSelection();

                var gc1 = EditorPro.GetStaleChord(result[0], false);
                if (gc1 != null)
                {
                    SetSelectedChord(gc1, false);
                    if (result.Count() > 1)
                    {
                        foreach (var gc in result)
                        {
                            gc1 = EditorPro.GetStaleChord(gc, false);
                            if (gc1 != null)
                            {
                                gc1.Selected = true;
                            }
                        }
                    }
                }
                ScrollToSelection();
                EditorPro.Invalidate();
            }
        }

        private void listG6Tracks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listG5Tracks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button122_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            MoveSelectedDown12Frets();
        }

        public void MoveSelectedDown12Frets()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;

                foreach (var ch in EditorPro.SelectedChords)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        var n = ch.Notes[x];
                        if (n != null)
                        {
                            if (n.NoteFretDown >= 12)
                            {
                                n.NoteFretDown -= 12;
                            }
                        }
                    }
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        private void button123_Click(object sender, EventArgs e)
        {
            CreateTracks22FromTracks17();
        }

        public void CreateTracks22FromTracks17()
        {
            try
            {
                if (EditorPro.GetTrackGuitar22() == null)
                {
                    CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.GuitarTrackName22);
                }
                if (EditorPro.GetTrackBass22() == null)
                {
                    CopyTrack(GuitarTrack.BassTrackName17, GuitarTrack.BassTrackName22);
                }

                RefreshTracks();
            }
            catch { }
        }

        private void button124_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            MoveSelectedUpString();
        }

        public void MoveSelectedUpString()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;


                foreach (var ch in EditorPro.SelectedChords)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        var n = ch.Notes[x];
                        if (n != null)
                        {
                            if (x == 5)
                            {
                                
                            }
                            else if(x == 4 && n.NoteFretDown < 4)
                            {
                                
                            }
                            else if (n.NoteFretDown < 5)
                            {
                                
                            }
                            
                        }
                    }
                }

                foreach (var ch in EditorPro.SelectedChords)
                {
                    ch.Notes.ToList().ForEach(n =>
                    {
                        if (n.NoteString != 5)
                        {
                            n.NoteString++;

                            if (n.NoteString == 4)
                            {
                                n.NoteFretDown -= 4;
                            }
                            else if (n.NoteString == 5)
                            {
                                n.NoteFretDown -= 5;
                            }
                            else
                            {
                                n.NoteFretDown -= 5;
                            }
                            if (n.NoteFretDown < 0)
                                n.NoteFretDown = 0;
                        }
                        else
                        {
                            ch.RemoveNote(n);
                        }
                    });
                    
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        


        private void button125_Click(object sender, EventArgs e)
        {
            //open usb
            RefreshUSBList();
            OpenSelectedUSB(false);
            RefreshUSBSongs();

        }

        string GetUSBTreeNodePath(TreeNode node)
        {
            string ret = null;
            if (node != null)
            {
                ret = node.Name;
                TreeNode parent = node;
                while ((parent = parent.Parent) != null)
                {
                    if (parent.Tag is FATXFolderEntry || parent.Tag is USBDriveEntry)
                    {
                        ret = parent.Name + "/" + ret;
                    }
                }
            }
            return ret;
            
        }

        TreeNode AddSubFolder(FATXDrive drive, TreeNode parentNode, 
            string parentPath, string name)
        {
            string folderPath = name;

            bool addedNode = false;

            
            if (parentPath == null)
            {
                addedNode = true;
                
                TreeNode parent = parentNode;
                while ((parent = parent.Parent) != null)
                {
                    if (parent.Tag is FATXFolderEntry)
                    {
                        folderPath = parent.Text + "/" + folderPath;
                    }
                }
            }
            else
            {
                folderPath = parentPath + "/" + name;
            }

            TreeNode node;

            if (parentNode.ImageKey == "USBFlash")
            {
                node = parentNode.Nodes.Add(name, name + " Partition");
                node.ImageKey = "Partition";
                node.SelectedImageKey = "Partition";
            }
            else
            {
                node = parentNode.Nodes.Add(name, name);
                node.ImageKey = "XPFolder";
                node.SelectedImageKey = "OpenFolder";
            }
            

            FATXFolderEntry fe;
            var contents = drive.ReadToFolder(folderPath, out fe);
            node.Tag = fe;

            if (!addedNode)
            {
                if (contents != null)
                {
                    foreach (var folder in contents.Folders)
                    {
                        AddSubFolder(drive, node, folderPath, folder.Name);
                    }
                }
                return null;
            }
            else
            {
                return node;
            }
        }

        TreeNode GetChildNodeByName(TreeNodeCollection nodes, string name)
        {
            foreach (TreeNode n in nodes)
            {
                if (string.Compare(n.Name, name, true) == 0)
                {
                    return n;
                }
            }
            return null;
        }

        void RefreshSubTreeNode(TreeNodeCollection nodes, FATXReadContents contents)
        {
            if (contents == null)
            {
                return;
            }
            if (nodes == null || nodes.Count == 0)
            {
            }
            foreach(var f in contents.Folders)
            {
                var node = GetChildNodeByName(nodes, f.Name);
                if (node != null)
                {
                    node.Tag = f;

                    var path = GetUSBTreeNodePath(node);

                    FATXFolderEntry fe;
                    var subContent = SelectedUSB.Drive.xReadToFolder(path, out fe);
                    RefreshSubTreeNode(node.Nodes, subContent);
                }
            }
        }

        public bool OpenSelectedUSB(bool refreshing = true)
        {
            bool ret = false;

            if (SelectedUSB == null)
                return ret;

            if (!HasValidUSBDeviceSelection)
                return ret;
            
            
            treeUSBContents.BeginUpdate();
            var sel = treeUSBContents.SelectedNode;
            treeUSBContents.SelectedNode = null;
            if (!refreshing)
            {
                treeUSBContents.Nodes.Clear();
            }

            var opened = SelectedUSB.Open();
                
            try
            {
                
                if (SelectedUSB.Folders != null)
                {
                    TreeNode root;
                    if (!refreshing)
                    {
                        treeUSBContents.Nodes.Clear();
                            
                        treeUSBContents.Nodes.Add("Drive",
                            SelectedUSB.Drive.DriveName + " (XBox 360 USB Device)", "USBFlash", "USBFlash");
                        root = treeUSBContents.Nodes[0];
                    }
                    else
                    {
                        root = treeUSBContents.Nodes[0];
                    }
                    root.Tag = SelectedUSB.Drive;

                    if (refreshing)
                    {
                        foreach(var fld in SelectedUSB.Folders)
                        {
                            var tn = GetChildNodeByName(root.Nodes, fld.Name);
                            if (tn != null)
                            {
                                tn.Tag = fld;
                                RefreshSubTreeNode(tn.Nodes, fld.Contents);
                            }
                        }
                    }
                    else
                    {
                        foreach (var folder in SelectedUSB.Folders)
                        {
                            AddSubFolder(SelectedUSB.Drive, root, "", folder.Name);
                        }
                    }
                }
            }
            catch { }
            try
            {
                if (opened)
                {
                    SelectedUSB.Close();
                }
            }
            catch { }

            if (refreshing)
            {
                if (sel != null)
                {
                    if (!string.IsNullOrEmpty(sel.Text))
                    {
                        var nodes = treeUSBContents.Nodes.Find(sel.Name, true);
                        if (nodes != null && nodes.Length > 0)
                        {
                            foreach (var n in nodes)
                            {
                                if (string.Compare(n.Text, sel.Text, true) == 0)
                                {
                                    treeUSBContents.SelectedNode = n;
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                if (treeUSBContents.SelectedNode == null)
                {
                    var defFolder = settings.GetValue("textBoxUSBFolder");
                    if (!string.IsNullOrEmpty(defFolder))
                    {
                        var nodes = treeUSBContents.Nodes.Find(defFolder, true);
                        if (nodes != null && nodes.Length > 0)
                        {
                            treeUSBContents.SelectedNode = nodes[nodes.Length - 1];
                        }
                    }
                }
            }


            if (treeUSBContents.SelectedNode == null &&
                treeUSBContents.Nodes.Count > 0)
            {

                treeUSBContents.SelectedNode = treeUSBContents.Nodes[0];

                SelectedUSBNode.Expand();
                foreach (TreeNode n in SelectedUSBNode.Nodes)
                {
                    n.Expand();
                    foreach (TreeNode n2 in SelectedUSBNode.Nodes)
                    {
                        n2.Expand();
                    }
                }
            }
            if (treeUSBContents.SelectedNode != null)
            {
                treeUSBContents.SelectedNode.EnsureVisible();
            }
        
            treeUSBContents.EndUpdate();
            
            RefreshUSBFilesList();

            return ret;
        }

        private void buttonUSBRefresh_Click(object sender, EventArgs e)
        {
            RefreshUSBList();
            RefreshUSB();
        }

        void RefreshUSB()
        {
            RefreshUSBList();
            OpenSelectedUSB();
            RefreshUSBSongs();
        }

        public class USBDriveEntry
        {
            FATXFolderEntry entry;
            FATXReadContents contents;

            string name;

            public USBDriveEntry(string name, FATXReadContents contents, FATXFolderEntry entry)
            {
                this.entry = entry;
                this.name = name;
                this.contents = contents;
            }

            public FATXFolderEntry Entry
            {
                get { return this.entry; }
            }

            public FATXReadContents Contents
            {
                get { return this.contents; }
            }

            public string Name
            {
                get { return this.name; }
            }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class USBDrive
        {
            DeviceReturn device;
            string name;

            bool opened = false;
            FATXDrive drive = null;

            public bool IsOpen
            {
                get { return opened; }
            }


            public bool Open()
            {
                if (!opened)
                {
                    try
                    {
                        drive = new FATXDrive(device);

                        RefreshPartitions();

                        opened = true;
                        return true;
                    }
                    catch { }
                }

                return false;
            }

            public void Close()
            {
                if (opened)
                {
                    try
                    {
                        drive.Close();
                        drive = null;
                    }
                    catch { drive = null; }
                    opened = false;
                }
            }

            public USBDrive(DeviceReturn device)
            {
                this.device = device;

                if (Open())
                {
                    try
                    {
                        this.name = Drive.DriveName;

                        RefreshPartitions();
                    }
                    catch { }
                    finally { this.Close(); }
                }
            }

            public FATXFolderEntry GetFolder(FATXFolderEntry e)
            {
                return GetFolder(e.Path);
            }

            public FATXFolderEntry GetFolder(string path)
            {
                if (drive != null)
                {
                    bool o = Open();

                    var ret = drive.GetFolder(path);

                    if (o)
                        Close();

                    return ret;
                }
                else
                {
                    return null;
                }
            }

            public FATXFolderEntry GetFolder(TreeNode node)
            {
                if (node.Tag is FATXFolderEntry)
                    return GetFolder(node.Tag as FATXFolderEntry);
                else
                    return null;
            }


            public void RefreshPartitions()
            {
                folders = new USBDriveEntry[0];
                
                var fld = new List<USBDriveEntry>();
                foreach (var p in drive.Partitions)
                {
                    FATXFolderEntry fe;
                    FATXReadContents readContents = drive.ReadToFolder(
                        p.PartitionName, out fe);

                    var entry = new USBDriveEntry(p.PartitionName, readContents, fe);
                    
                    fld.Add(entry);
                }
                this.folders = fld.ToArray();
            }

            public override string ToString()
            {
                return this.name;
            }

            public FATXDrive Drive
            {
                get
                {
                    return drive;
                }
            }
            USBDriveEntry[] folders;
            public USBDriveEntry[] Folders
            {
                get
                {
                    return folders;
                }
            }
        }

        public void RefreshUSBList()
        {
            comboUSBList.BeginUpdate();
            bool hasSel = SelectedUSB != null;
            string selected = comboUSBList.Text;
            try
            {
                
                comboUSBList.Items.Clear();
                
                var drives = X360.FATX.FATXManagement.GetDrives(0);
                foreach (var drive in drives)
                {
                    try
                    {
                        X360.FATX.DriveTypes types;
                        if (X360.FATX.FATXManagement.IsFATX(drive, out types))
                        {
                            if (types == DriveTypes.USBFlashDrive)
                            {
                                comboUSBList.Items.Add(new USBDrive(drive));
                            }
                        }
                    }
                    catch 
                    {
                    }
                }

                if (hasSel)
                {
                    int i = comboUSBList.FindStringExact(selected);
                    if (i != -1)
                    {
                        comboUSBList.SelectedIndex = i;
                    }
                }
                else
                {
                    if (comboUSBList.Items.Count > 0)
                    {
                        comboUSBList.SelectedIndex = 0;
                    }
                }

            }
            catch { comboUSBList.Items.Clear(); }

            comboUSBList.EndUpdate();
        }

        void RefreshUSBSongs()
        {
            listBoxUSBSongs.BeginUpdate();
            listBoxUSBSongs.Items.Clear();
            foreach (var sng in SongList)
            {
                listBoxUSBSongs.Items.Add(sng.ToString()).Tag = sng;
            }
            listBoxUSBSongs.EndUpdate();
        }

        List<SongCacheItem> SelectedUSBSongs
        {
            get
            {
                var ret = new List<SongCacheItem>();

                if (!HasValidUSBDeviceSelection)
                    return ret;
                
                if(listBoxUSBSongs.SelectedItems != null && 
                   listBoxUSBSongs.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem li in listBoxUSBSongs.SelectedItems)
                    {
                        ret.Add(li.Tag as SongCacheItem);
                    }
                }

                return ret;
            }
        }

        public class ListViewColumnSorter : IComparer
        {
            enum USBColumns
            {
                Name=0,
                
                Size,
                UpdateDate,
                CreateDate,
            }
            
            
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                var sc = listviewX.SubItems[ColumnToSort];
                
                // Compare the two items
                if (ColumnToSort == (int)USBColumns.Name)
                {
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                }
                else if (ColumnToSort == (int)USBColumns.Size)
                {
                    if (listviewX.Tag != null &&
                        listviewY.Tag != null)
                    {
                        compareResult = ObjectCompare.Compare(
                            (listviewX.Tag as FATXFileEntry).Size,
                            (listviewY.Tag as FATXFileEntry).Size);
                    }
                    else
                    {
                        compareResult = 0;
                        
                    }
                }
                else if (ColumnToSort == (int)USBColumns.UpdateDate)
                {
                    compareResult = ObjectCompare.Compare(
                        (listviewX.SubItems[ColumnToSort].Text).ToDateTime(),
                        (listviewY.SubItems[ColumnToSort].Text).ToDateTime());
                }
                else if (ColumnToSort == (int)USBColumns.CreateDate)
                {
                    compareResult = ObjectCompare.Compare(
                        (listviewX.SubItems[ColumnToSort].Text).ToDateTime(),
                        (listviewY.SubItems[ColumnToSort].Text).ToDateTime());
                }
                else
                {
                    compareResult = 0;
                }

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }

        }
        ListViewColumnSorter sorter;

        int nextTimerID = 123;

        class ThreadTimer
        {
            public System.Threading.Timer timer;
            public int TimerID;
            public int TimerGroupID;
        }

        
        List<ThreadTimer> threadTimers = new List<ThreadTimer>();
        object timerLocker = new object();

        void StopTimer(int timerID)
        {
            lock (timerLocker)
            {
                var t = threadTimers.SingleOrDefault(x => x.TimerID == timerID);
                if (t != null)
                {
                    t.timer.Change(1000, 2000);
                    t.timer.Dispose();
                    threadTimers.Remove(t);
                    t.timer = null;
                }
            }
        }
        void StopTimerGroup(int timerGroupID)
        {
            lock (timerLocker)
            {
                var timers = threadTimers.Where(x => x.TimerGroupID == timerGroupID);
                if (timers != null)
                {
                    var removed = new List<ThreadTimer>();
                    foreach (var timer in timers)
                    {
                        timer.timer.Change(1000, 2000);
                        timer.timer.Dispose();
                        timer.timer = null;
                        removed.Add(timer);
                    }
                    foreach (var t in removed)
                    {
                        threadTimers.Remove(t);
                    }
                }
            }
        }
        bool TimerActive(int timerID)
        {
            lock (timerLocker)
            {
                var t = threadTimers.SingleOrDefault(x => x.TimerID == timerID);
                if (t != null)
                {
                    return true;
                }
            }
            return false;
        }

        bool TimerGroupActive(int timerGroupID)
        {
            lock (timerLocker)
            {
                var t = threadTimers.SingleOrDefault(x => x.TimerGroupID == timerGroupID);
                if (t != null)
                {
                    return true;
                }
            }
            return false;
        }

        class ThreadedTimerParam
        {
            public SynchronizationContext Context;
            public object Param;
            public int TimerID;
            public int TimerGroupID;
            public int NumTotalTicks;
            public Action<object> Func;
            public Action<object> EndFunc;
        }

        void OnThreadedTimerTick(object o)
        {
            Thread.Sleep(80);

            try
            {
                ThreadedTimerParam param;
                
                param = o as ThreadedTimerParam;
                

                if (param.NumTotalTicks >= 0)
                {
                    if (param.NumTotalTicks >= 0)
                    {
                        param.Context.Post(delegate(object funcParam)
                        {
                            if (param.Func != null)
                            {
                                param.Func(funcParam);

                            }
                            param.NumTotalTicks--;

                        }, param.Param);
                    }
                    
                }
                else
                {
                    bool isLast = false;
                    
                    if (param.Func != null)
                    {
                        param.Func = null;

                        isLast = true;
                    }
                    

                    if (isLast)
                    {
                        int id;
                       
                        id = param.TimerID;
                        

                        param.Context.Post(delegate(object tid)
                        {
                            StopTimer((int)tid);
                        }, id);

                        if (param.EndFunc != null)
                        {
                            param.Context.Post(delegate(object fparam)
                            {
                                param.EndFunc(fparam);
                            }, param.Param);
                        }
                        param.TimerID = -1;
                    }
                }
            }
            catch { }
        }


        int CreateTimer(Action<object> func, object funcParam = null,
            int groupID=0, int MSBetweenTicks = 400, int numTotalTicks = 8)
        {
            return CreateTimer(func, null, funcParam, groupID, -1, 
                MSBetweenTicks, numTotalTicks);
        }

        int CreateTimer(Action<object> func, Action<object> endFunc = null, 
            object funcParam = null, int groupID = 0, int timerID = -1,
            int MSBetweenTicks = 400, int numTotalTicks = 8,
            int startDelay = 500)
        {
            
            var param = new ThreadedTimerParam();
            param.Context = SynchronizationContext.Current;
            
            param.TimerID = nextTimerID++;
            param.TimerGroupID = groupID;

            param.NumTotalTicks = numTotalTicks;
            param.Func = func;
            param.EndFunc = endFunc;
            param.Param = funcParam;
                
            
            var t = new ThreadTimer();
            t.TimerID = param.TimerID;
            t.TimerGroupID = groupID;

            var cb = new TimerCallback(OnThreadedTimerTick);

            var timer = new System.Threading.Timer(cb,
                param, startDelay, MSBetweenTicks);

            t.timer = timer;
            lock (timerLocker)
            {
                threadTimers.Add(t);
            }
            
            return param.TimerID;
            
        }

        void OnUSBAlertFlash(object o)
        {
            try
            {
                var p = o as object[];
                var icon1 = p[0] as string;
                var icon2 = p[1] as string;
            }
            catch { }
        }
        void OnUSBResetIcons(object o)
        {
            try
            {
                var p = o as object[];
                var icon1 = p[0] as string;
                var icon2 = p[1] as string;
            }
            catch { }
        }

        int ShowUSBAlertFlash(int timerID, string flashIcon, string flashIcon2)
        {
            int group = 432437;

            if (!TimerActive(timerID))
            {
                StopTimerGroup(group);
                 
                return CreateTimer(new Action<object>(OnUSBAlertFlash),
                    new Action<object>(OnUSBResetIcons),
                    new object[] { flashIcon, flashIcon2 },
                        group, timerID, 100, 8);
            }
            return -1;
        }

        int ShowUSBBlueArrowFlash()
        {
            return ShowUSBAlertFlash(432, "USBBlueArrow", "USBFlash");
        }
        int ShowUSBGreenPlusFlash()
        {
            return ShowUSBAlertFlash(433, "USBGreenPlus", "USBFlash");
        }
        int ShowUSBIconFlash()
        {
            return ShowUSBAlertFlash(434, "USBLogo", "USBFlash");
        }
        int ShowUSBWarningFlash()
        {
            return ShowUSBAlertFlash(435, "USBExcl", "USBFlash");
        }
        int ShowUSBWritingFlash()
        {
            return ShowUSBAlertFlash(436, "USBPencil", "USBFlash");
        }
        int ShowUSBErrorFlash()
        {
            return ShowUSBAlertFlash(437, "USBRedMinus", "USBFlash");
        }

        void InitializeUSBList()
        {
            
            this.buttonUSBAddFolder.Click += new EventHandler(buttonUSBAddFolder_Click);
            this.buttonUSBSelectCompletedSongs.Click += new EventHandler(buttonUSBSelectCompletedSongs_Click);
            this.buttonUSBSetDefaultFolder.Click += new EventHandler(buttonUSBSetDefaultFolder_Click);
            this.buttonUSBRestoreImage.Click += new EventHandler(buttonUSBRestoreImage_Click);
            this.buttonUSBCreateImage.Click += new EventHandler(buttonUSBCreateImage_Click);
            this.buttonUSBCreateFolder.Click += new EventHandler(buttonUSBCreateFolder_Click);
            
            this.buttonUSBAddFile.Click += new EventHandler(buttonUSBAddFile_Click);
            this.buttonUSBCopySelectedSongToUSB.Click += new EventHandler(buttonUSBCopySelectedSongToUSB_Click);
            this.buttonUSBDeleteFile.Click += new EventHandler(buttonUSBDeleteFile_Click);
            this.buttonUSBDeleteSelected.Click += new EventHandler(buttonUSBDeleteSelected_Click);

            this.buttonUSBExtractFolder.Click += new EventHandler(buttonUSBExtractFolder_Click);
            this.buttonUSBExtractSelectedFiles.Click += new EventHandler(buttonUSBExtractSelectedFiles_Click);
            this.buttonUSBRenameFolder.Click += new EventHandler(buttonUSBRenameFolder_Click);

            this.listUSBFileView.SelectedIndexChanged += new EventHandler(listUSBFileView_SelectedIndexChanged);


            listUSBFileView.View = View.Details;
            listUSBFileView.Columns.Clear();
            listUSBFileView.Columns.Add("File Name", 180, HorizontalAlignment.Left);
            
            listUSBFileView.Columns.Add("Size", 100, HorizontalAlignment.Right);
            listUSBFileView.Columns.Add("Update Date", 100, HorizontalAlignment.Left);
            listUSBFileView.Columns.Add("Create Date", 100, HorizontalAlignment.Left);
            
            listUSBFileView.FullRowSelect = true;
            listUSBFileView.ShowGroups = false;
            listUSBFileView.HotTracking = false;
            listUSBFileView.GridLines = false;

            listUSBFileView.ColumnClick += new ColumnClickEventHandler(listUSBFileView_ColumnClick);
            
            sorter = new ListViewColumnSorter();
            listUSBFileView.ListViewItemSorter = sorter;
            listUSBFileView.SmallImageList = new ImageList();
            listUSBFileView.SmallImageList.Images.Add("XBJoy", IconResources.XBJoy);

            treeUSBContents.ShowPlusMinus = true;
            treeUSBContents.HotTracking = false;
            treeUSBContents.ImageList = new ImageList();
            treeUSBContents.ImageList.Images.Add("XPFolder", IconResources.XPFolder);
            treeUSBContents.ImageList.Images.Add("OpenFolder", IconResources.OpenFolder);
            treeUSBContents.ImageList.Images.Add("Partition", IconResources.Partition);
            treeUSBContents.ImageList.Images.Add("USBLogo", IconResources.USBLogo);
            treeUSBContents.ImageList.Images.Add("USBFlash", IconResources.USBFlash);
            treeUSBContents.ImageList.Images.Add("USBPencil", IconResources.USBPencil);
            treeUSBContents.ImageList.Images.Add("USBRedMinus", IconResources.USBRedMinus);
            treeUSBContents.ImageList.Images.Add("USBBlueArrow", IconResources.USBBlueArrow);
            treeUSBContents.ImageList.Images.Add("USBGreenPlus", IconResources.USBGreenPlus);
            treeUSBContents.ImageList.Images.Add("USBExcl", IconResources.USBExcl);

            treeUSBContents.AfterSelect += new TreeViewEventHandler(treeUSBContents_AfterSelect);
            treeUSBContents.BeforeSelect += new TreeViewCancelEventHandler(treeUSBContents_BeforeSelect);

            treeUSBContents.AllowDrop = true;
            treeUSBContents.LabelEdit = true;

            treeUSBContents.BeforeLabelEdit += new NodeLabelEditEventHandler(treeUSBContents_BeforeLabelEdit);
            treeUSBContents.AfterLabelEdit += new NodeLabelEditEventHandler(treeUSBContents_AfterLabelEdit);

            
            treeUSBContents.DragDrop += new DragEventHandler(treeUSBContents_DragDrop);
            treeUSBContents.DragOver += new DragEventHandler(treeUSBContents_DragOver);

            

            RefreshUSBList();
            OpenSelectedUSB(false);
            RefreshUSB();
        }

        

        

        

        void treeUSBContents_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                e.Cancel = true;
                return;
            }

            if (SelectedUSB.IsOpen)
                e.Cancel = true;
        }

        void treeUSBContents_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                e.CancelEdit = true;
                return;
            }
            if (VariousFunctions.IsValidXboxName(e.Node.Text) == false)
            {
                e.CancelEdit = true;
                MessageBox.Show("Invalid folder name");
                return;
            }

            if (e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }

            treeUSBContents.BeginUpdate();
            
            if (SelectedUSB.Open())
            {
                try
                {
                    var path = GetUSBTreeNodePath(e.Node);

                    FATXFolderEntry fe;
                    var contents = SelectedUSB.Drive.ReadToFolder(path, out fe);
                    if (fe != null)
                    {
                        
                        if (string.Compare(fe.Name, e.Label, true) != 0)
                        {
                            fe.Name = e.Label;
                            
                            if (!fe.xWriteEntry())
                            {
                                MessageBox.Show("Error saving");
                                e.CancelEdit = true;
                            }
                            else
                            {
                                e.Node.Name = fe.Name;
                                e.Node.Tag = fe;
                                e.Node.Text = fe.Name;
                            }
                        }
                        else
                        {
                            e.CancelEdit = true;
                        }
                    }
                    else
                    {
                        e.CancelEdit = true;
                    }
                }
                catch { }
                SelectedUSB.Close();
            }
            
            treeUSBContents.EndUpdate();
            if (!e.CancelEdit)
            {
                RefreshUSB();
            }
        }

        void treeUSBContents_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                e.CancelEdit = true;
            }
            else if (e.Node.Tag == null)
            {
                e.CancelEdit = true;
                return;
            }
            else
            {
                var folder = e.Node.Tag as FATXFolderEntry;
                if (folder == null)
                {
                    e.CancelEdit = true;
                    return;
                }
            }
        }


        void listUSBFileView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }
            if(listUSBFileView.SelectedItems.Count == 1)
            {
                textBoxUSBFileName.Text = listUSBFileView.SelectedItems[0].Text;
            }
        }

       

        void listUSBFileView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }
            //Determine if clicked column is already the column that is being sorted.
            if (e.Column == sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listUSBFileView.Sort();
        }

        void buttonUSBAddFolder_Click(object sender, EventArgs e)
        {
            
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSBNode == null)
                return;

            if (SelectedUSBFolderEntry == null)
                return;
            
            var filePath = ShowSelectFolderDlg("Select Folder to add", "", "");
            if (!string.IsNullOrEmpty(filePath))
            {
                AddFolderToUSB(filePath);
            }
        }

        public void AddFolderToUSB(string filePath, TreeNode folderNode = null)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (!string.IsNullOrEmpty(filePath))
            {

                treeUSBContents.BeginUpdate();
                if (SelectedUSB.Open())
                {
                    try
                    {
                        FATXFolderEntry folder;
                        FATXReadContents contents;
                        if (folderNode == null)
                        {
                            folder = SelectedUSBFolderEntry;
                            contents = SelectedUSBContents;
                        }
                        else
                        {
                            contents = SelectedUSB.Drive.ReadToFolder(GetUSBTreeNodePath(folderNode), out folder);
                        }

                        if (SelectedUSBFolderEntry != null)
                        {
                            AddSubFolderToUSB(filePath, folderNode, folder);
                        }
                        
                    }
                    catch { }
                    SelectedUSB.Close();
                }
                treeUSBContents.EndUpdate();

                RefreshUSB();
            }
        }

        bool AddSubFolderToUSB(string filePath,
            TreeNode parentNode,
            FATXFolderEntry parent)
        {
            string sfolderName;
            TreeNode sfolderNode;
            FATXFolderEntry sfatxFolder;

            bool ret = false;

            if (AddFolderToUSB(filePath, parentNode, parent, out sfolderName, out sfolderNode, out sfatxFolder))
            {
                ret = true;
                var di = new DirectoryInfo(filePath);
                var sub = di.GetDirectories();
                if (sub != null)
                {
                    foreach (var s in sub)
                    {
                        ret = AddSubFolderToUSB(s.FullName, sfolderNode, sfatxFolder);

                        if (!ret)
                        {
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        bool AddFolderToUSB(string filePath, 
            TreeNode parentNode,
            FATXFolderEntry parent, 
            out string folderName, 
            out TreeNode folderNode,
            out FATXFolderEntry fatxFolder)
        {
            folderName = string.Empty;
            folderNode = null;
            fatxFolder = null;
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var folder = parent;

                    if (filePath.EndsWith("\\") == false)
                        filePath += "\\";
                    var dir = Path.GetDirectoryName(filePath);
                    var parts = dir.Split('\\');
                    if (parts != null && parts.Length > 0)
                    {                       
                        string dirName = parts[parts.Length - 1];

                        FATXFolderEntry existFolder = null;
                        var subfolders = folder.xRead();
                        foreach (var s in subfolders.Folders)
                        {
                            if (string.Compare(s.Name, dirName, true) == 0)
                            {
                                existFolder = s;
                                break;
                            }
                        }
                        if (existFolder == null)
                        {
                            folder.AddFolder(dirName);
                        }

                        subfolders = folder.xRead();
                        FATXFolderEntry newFolder = null;

                        foreach (var s in subfolders.Folders)
                        {
                            if (string.Compare(s.Name, dirName, true) == 0)
                            {
                                newFolder = s;
                                break;
                            }
                        }
                        
                        if (newFolder != null)
                        {
                            var files = Directory.GetFiles(filePath);

                            foreach (var s in files)
                            {
                                var name = Path.GetFileName(s);
                                newFolder.AddFile(name, s, AddType.Inject);
                            }
                        }

                        folderName = dirName;

                        folderNode = AddSubFolder(SelectedUSB.Drive, parentNode, 
                                    null, folderName);

                        fatxFolder = newFolder;
                    }
                        
                }
            }
            catch 
            { 
                return false;
            }

            return fatxFolder != null && folderNode != null && !string.IsNullOrEmpty(folderName);
        }

        void buttonUSBSelectCompletedSongs_Click(object sender, EventArgs e)
        {
            listBoxUSBSongs.BeginUpdate();
            foreach (ListViewItem s in listBoxUSBSongs.SelectedItems) s.Selected = false;
            for (int x = 0; x < listBoxUSBSongs.Items.Count; x++)
            {
                var i = listBoxUSBSongs.Items[x];
                var sng = i.Tag as SongCacheItem;
                if (sng.IsComplete)
                {
                    i.Selected = true;
                }
            }
            listBoxUSBSongs.EndUpdate();
        }

        void buttonUSBAddFile_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            
            
            if (SelectedUSB.Open())
            {
                try
                {
                    var folder = SelectedUSBFolderEntry;
                    if (folder != null)
                    {
                        var contents = SelectedUSBContents;

                        var filePath = ShowOpenFileDlg("Select File to add", "", "");
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            var newName = Path.GetFileName(filePath);

                            bool overwrite = false;
                            if (contents != null)
                            {
                                foreach (var c in contents.Files)
                                {
                                    if (string.Compare(c.Name, newName, true) == 0)
                                    {
                                        overwrite = true;
                                        break;
                                    }
                                }
                            }

                            bool success = false;
                            if (overwrite)
                            {
                                success = folder.AddFile(newName, filePath, AddType.Replace);
                            }
                            else
                            {
                                success = folder.AddFile(newName, filePath, AddType.Inject);

                                if (success)
                                {
                                    var ccontents = folder.Read();
                                    if (ccontents != null)
                                    {
                                        foreach (var f in ccontents.Files)
                                        {
                                            if (string.Compare(f.Name, newName, true) == 0)
                                            {
                                                var li = CreateUSBListViewItem(f);
                                                var newli = listUSBFileView.Items.Add(li);
                                                newli.Tag = f;
                                                
                                                break;
                                            }
                                        }
                                    }

                                }
                            }

                            if (!success)
                            {
                                MessageBox.Show("Unable to create file: " + newName);

                            }
                        }
                    }
                }
                catch 
                { 
                    MessageBox.Show("Unable to add file");
                }
                SelectedUSB.Close();
            }
            RefreshUSB();
        }


        ListViewItem CreateUSBListViewItem(FATXFileEntry f)
        {
            
            var li = new ListViewItem(new string[]{ 
                    f.Name,
                    
                    VariousFunctions.GetFriendlySize(f.Size), 
                    TimeStamps.FatTimeDT(f.xT2).ToString("M/d/yyyy hh:mm tt") ,
                    TimeStamps.FatTimeDT(f.xT1).ToString("M/d/yyyy hh:mm tt")}, 
                "XBJoy");
            li.Tag = f;
            return li;
        }

        void buttonUSBRenameFolder_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            string name = textBoxUSBFolder.Text;
            
            if(!X360.Other.VariousFunctions.IsValidXboxName(name))
            {
                MessageBox.Show("invalid characters");
                return;
            }

            if (SelectedUSB.Open())
            {
                treeUSBContents.BeginUpdate();
                try
                {
                    var folder = SelectedUSBFolderEntry;
                    if (folder != null)
                    {
                        if (string.Compare(name, folder.Name, true) == 0)
                        {
                            MessageBox.Show("invalid name");
                        }
                        else
                        {
                            string path = GetUSBTreeNodePath(SelectedUSBNode);
                            if (!string.IsNullOrEmpty(path))
                            {
                                FATXFolderEntry fe;
                                var contents = SelectedUSB.Drive.ReadToFolder(path, out fe);
                                if (fe != null)
                                {
                                    fe.Name = name;
                                    if (fe.xWriteEntry())
                                    {
                                        SelectedUSBNode.Text = name;
                                        SelectedUSBNode.Name = fe.Name;
                                        SelectedUSBNode.Tag = fe;
                                    }
                                }
                            }
                        }
                    }
                }
                catch 
                {
                    MessageBox.Show("failed changing name"); 
                }
                treeUSBContents.EndUpdate();

                SelectedUSB.Close();
                RefreshUSB();
            }
        }

        void buttonUSBExtractSelectedFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HasValidUSBDeviceSelection)
                    return;

                var cnt = listUSBFileView.SelectedItems.Count;
                if (cnt > 0)
                {
                    if (SelectedUSB.Open())
                    {
                        int numCancelled = 0;
                        string lastFolder = string.Empty;
                        foreach (var file in SelectedUSBFiles)
                        {
                            string folder = lastFolder;
                            if (string.IsNullOrEmpty(folder))
                            {
                                folder = ShowSaveFileDlg("Save USB File", lastFolder, file.Name);
                            }
                            if (!string.IsNullOrEmpty(folder))
                            {
                                folder = Path.Combine(Path.GetDirectoryName(folder), file.Name);
                                lastFolder = folder;
                            }
                            if (string.IsNullOrEmpty(folder))
                            {
                                numCancelled++;
                                if (numCancelled > 5)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                numCancelled = 0;

                                try
                                {
                                    X360.IO.DJsIO io = new X360.IO.DJsIO(
                                        folder, X360.IO.DJFileMode.Create, true);

                                    var b = file.xExtract(ref io);
                                    if (b == false)
                                    {
                                        MessageBox.Show("Unable to extract file: " + file.Name);
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        SelectedUSB.Close();
                    }
                }
            }
            catch { MessageBox.Show("Error extracting"); }
        }

        void buttonUSBExtractFolder_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;


            if (SelectedUSB.Open())
            {
                var folder = SelectedUSBFolderEntry;
                if (folder != null)
                {

                    var outFolder = ShowSelectFolderDlg("Select USB Output Folder", "", "");
                    try
                    {

                        if (!string.IsNullOrEmpty(outFolder))
                        {
                            folder.Extract(outFolder, true);
                        }
                        
                    }
                    catch 
                    {
                        MessageBox.Show("Failed extracting folder");
                    }
                }
                SelectedUSB.Close();
            }
        }

        void buttonUSBDeleteSelected_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSB.Open())
            {
                treeUSBContents.BeginUpdate();
                try
                {
                    var fe = SelectedUSBFolderEntry;
                    if (fe != null)
                    {
                        if (!fe.Delete())
                        {
                            MessageBox.Show("Unable to delete folder");
                        }
                    }

                    
                    var n = SelectedUSBNode;
                    if (n != null && n.Parent != null)
                    {
                        var p = n.Parent;
                        n.Remove();
                        treeUSBContents.SelectedNode = p;
                    }
                    
                }
                catch 
                {
                    MessageBox.Show("Failed deleting folder"); 
                }
                treeUSBContents.EndUpdate();
                SelectedUSB.Close();
            }
            RefreshUSB();
        }

        void buttonUSBDeleteFile_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSB.Open())
            {
                try
                {
                    bool delete = false;
                    if (SelectedUSBFiles.Count > 1)
                    {
                        delete = MessageBox.Show("Delete " + SelectedUSBFiles.Count.ToString() + " files?", "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    }
                    else if(SelectedUSBFiles.Count == 1)
                    {
                        delete = MessageBox.Show("Delete " + SelectedUSBFiles[0].Name + "?", "Confirm Deletion", MessageBoxButtons.YesNo) == DialogResult.Yes;
                    }
                    if (delete)
                    {
                        foreach (var file in SelectedUSBFiles)
                        {
                            file.Delete();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Failed deleting file"); 
                }
                
                SelectedUSB.Close();
                
            }
            RefreshUSB();
        }

        void buttonUSBCopySelectedSongToUSB_Click(object sender, EventArgs e)
        {

            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSB.Open())
            {
                listUSBFileView.BeginUpdate();
                try
                {
                    var folder = SelectedUSBFolderEntry;
                    var contents = SelectedUSBContents;
                    if (folder != null)
                    {
                        progressUSBSongs.Value = 0;
                        progressUSBSongs.Maximum = SelectedUSBSongs.Count;

                        foreach (var song in SelectedUSBSongs)
                        {

                            progressUSBSongs.Value++;
                            if (!string.IsNullOrEmpty(song.G6ConFile))
                            {
                                if (File.Exists(song.G6ConFile))
                                {
                                    try
                                    {
                                        var fileName = Path.GetFileName(song.G6ConFile);
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
                                                success = folder.AddFile(fileName, song.G6ConFile, AddType.Replace);
                                            }
                                            else
                                            {
                                                success = folder.AddFile(fileName, song.G6ConFile, AddType.Inject);

                                                if (success)
                                                {
                                                    var ccontents = folder.Read();
                                                    if (ccontents != null)
                                                    {
                                                        foreach (var f in ccontents.Files)
                                                        {
                                                            if (string.Compare(f.Name, fileName, true) == 0)
                                                            {
                                                                var li = CreateUSBListViewItem(f);
                                                                var newli = listUSBFileView.Items.Add(li);
                                                                newli.Tag = f;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                            if (!success)
                                            {
                                                MessageBox.Show("Unable to write file: " + fileName);
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    
                }
                catch { }
                listUSBFileView.EndUpdate();
                SelectedUSB.Close();
            }
            RefreshUSB();
        }

        void AddFileToUSB(string fileName, TreeNode folderNode = null)
        {
            if (SelectedUSB.Open())
            {
                FATXFolderEntry folder = SelectedUSBFolderEntry;
                
                if (folder != null)
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (File.Exists(fileName))
                        {
                            try
                            {
                                var fileName2 = Path.GetFileName(fileName);
                                if (!string.IsNullOrEmpty(fileName2))
                                {
                                    bool overwrite = false;
                                    var contents = SelectedUSBContents;
                                    if (contents != null)
                                    {
                                        foreach (var c in contents.Files)
                                        {
                                            if (string.Compare(c.Name, fileName2, true) == 0)
                                            {
                                                overwrite = true;
                                                break;
                                            }
                                        }
                                    }

                                    bool success = false;
                                    if (overwrite)
                                    {
                                        success = folder.AddFile(fileName2, fileName, AddType.Replace);
                                    }
                                    else
                                    {
                                        success = folder.AddFile(fileName2, fileName, AddType.Inject);
                                    }

                                    if (!success)
                                    {
                                        MessageBox.Show("Unable to write file: " + fileName);
                                    }
                                }
                            }
                            catch { }
                        }
                    
                    }
                }
                SelectedUSB.Close();
            }
        }
        
        private void buttonCopyAllConToUSB_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                RefreshUSBList();
                OpenSelectedUSB(false);

                if (!HasValidUSBDeviceSelection)
                {
                    MessageBox.Show("Unable to open USB");
                    return;
                }
            }

            if (SelectedUSBFolderEntry == null)
            {
                RefreshUSB();

                if (SelectedUSBFolderEntry == null)
                {
                    var def = settings.GetValue("textBoxUSBFolder");
                    if (string.IsNullOrEmpty(def))
                    {
                        MessageBox.Show("Must set default usb folder");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Default usb folder not found");
                        return;
                    }
                }
            }

            if (SelectedUSBFolderEntry != null)
            {
                treeUSBContents.BeginUpdate();
                var songs = SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked);
                foreach (var sng in songs)
                {
                    if (!string.IsNullOrEmpty(sng.G6ConFile))
                    {
                        if (File.Exists(sng.G6ConFile))
                        {
                            AddFileToUSB(sng.G6ConFile);
                        }
                    }
                }
                treeUSBContents.EndUpdate();
                RefreshUSB();
            }
            
        }

        void buttonUSBSetDefaultFolder_Click(object sender, EventArgs e)
        {
            settings.SetValue("textBoxUSBFolder", textBoxUSBFolder.Text);
        }

        void buttonUSBRestoreImage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("warning. untested and slow. Continue?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;
            if (HasValidUSBDeviceSelection)
            {
                var fileName = ShowOpenFileDlg("Select image file", "", "usbdevice.img");
                if (!string.IsNullOrEmpty(fileName))
                {
                    bool success = false;
                    if (SelectedUSB.Open())
                    {
                        try
                        {
                            success = SelectedUSB.Drive.RestoreImage(fileName);
                        }
                        catch { }

                        if (!success)
                        {
                            MessageBox.Show("Error restoring image");
                        }
                        else
                        {
                            MessageBox.Show("Successful");
                        }

                        SelectedUSB.Close();
                    }
                }
            }
            RefreshUSB();
        }

        bool HasValidUSBDeviceSelection
        {
            get
            {
                if (comboUSBList.SelectedItem != null)
                {
                    var usb = comboUSBList.SelectedItem as USBDrive;

                    if (usb != null)
                    {
                        try
                        {
                            if(usb.IsOpen)
                                return true;

                            if (usb.Open())
                            {
                                usb.Close();
                                return true;
                            }
                        }
                        catch { }
                    }
                }
                return false;
            }
        }

        USBDrive SelectedUSB
        {
            get
            {
                return comboUSBList.SelectedItem as USBDrive;
            }
        }

        void buttonUSBCreateImage_Click(object sender, EventArgs e)
        {
            if (HasValidUSBDeviceSelection)
            {
                if (MessageBox.Show("Warning. Untested and slow. Continue?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;

                var fileName = ShowSaveFileDlg("Select image file", "", "usbdevice.img");
                if(!string.IsNullOrEmpty(fileName))
                {
                    bool success = false;
                    if(SelectedUSB.Open())
                    {
                        try
                        {
                            
                            success = SelectedUSB.Drive.ExtractImage(fileName);
                        }
                        catch { }

                        if (!success)
                        {
                            MessageBox.Show("Error extracting image");
                        }
                        else
                        {
                            MessageBox.Show("Extract Successful");
                        }
                        SelectedUSB.Close();
                    }
                }
                
            }
        }

        public List<FATXFileEntry> SelectedUSBFiles
        {
            get
            {
                var ret = new List<FATXFileEntry>();
                if (listUSBFileView.SelectedItems == null ||
                    listUSBFileView.SelectedItems.Count == 0)
                    return ret;

                var opened = SelectedUSB.Open();

                var contents = SelectedUSBContents;
                if (contents != null)
                {
                    foreach(var fl in contents.Files)
                    {
                        foreach (ListViewItem li in listUSBFileView.SelectedItems)
                        {
                            var f = li.Tag as FATXFileEntry;

                            if(string.Compare(f.Name, fl.Name, true)==0)
                                ret.Add(fl);
                        }
                    }
                }
                if (opened)
                    SelectedUSB.Close();

                return ret;
            }
        }

        public FATXFolderEntry SelectedUSBFolderEntry
        {
            get
            {
                if (SelectedUSB != null)
                {
                    if (SelectedUSBNode != null)
                    {
                        return SelectedUSB.GetFolder(SelectedUSBNode);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public FATXReadContents SelectedUSBContents
        {
            get
            {
                FATXReadContents ret = null;
                var node = SelectedUSBNode;
                if (node != null)
                {
                    bool opened = SelectedUSB.Open();

                    var folderEntry = SelectedUSBFolderEntry;
                    if (folderEntry != null)
                    {
                        ret = folderEntry.xRead();
                    }

                    if (opened)
                        SelectedUSB.Close();
                }
                return ret;
            }
        }

        public TreeNode SelectedUSBNode
        {
            get
            {
                return treeUSBContents.SelectedNode;
            }
        }

        void buttonUSBCreateFolder_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            var name = textBoxUSBFolder.Text;

            if(!X360.Other.VariousFunctions.IsValidXboxName(name))
            {
                MessageBox.Show("invalid characters");
                return;
            }

            treeUSBContents.BeginUpdate();
            var opened = SelectedUSB.Open();
            try
            {
                var parentFolder = SelectedUSBFolderEntry;

                if (parentFolder != null)
                {
                    if (parentFolder.AddFolder(name))
                    {
                        var added = AddSubFolder(SelectedUSB.Drive, SelectedUSBNode, null, name);
                        if (added != null)
                        {
                            treeUSBContents.SelectedNode = added;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not add folder.");
                    }
                }
            }
            catch { MessageBox.Show("Failed adding folder"); }

            if (opened)
                SelectedUSB.Close();
            treeUSBContents.EndUpdate();

            RefreshUSB();
        }

        void treeUSBContents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                var n = SelectedUSBNode;
                if (n != null)
                {
                    textBoxUSBFolder.Text = n.Name;
                    
                    if (!n.IsExpanded)
                    {
                        treeUSBContents.BeginUpdate();
                        n.Expand();
                        treeUSBContents.EndUpdate();
                    }

                    RefreshUSBFilesList();
                }
            }
        }

        private void RefreshUSBFilesList()
        {
            var lastFileSel = textBoxUSBFileName.Text;

            if (!HasValidUSBDeviceSelection)
                return;

            listUSBFileView.BeginUpdate();
            listUSBFileView.Items.Clear();
            listUSBFileView.Tag = null;

            var opened = SelectedUSB.Open();
            
            var sel = SelectedUSBNode;
            if (sel != null)
            {
                textBoxUSBFolder.Text = sel.Name;
                var c = SelectedUSBContents;
                if (c != null)
                {
                    listUSBFileView.Tag = sel;
                    foreach (var f in c.Files)
                    {
                        var newli = listUSBFileView.Items.Add(
                            CreateUSBListViewItem(f));
                        
                        if (string.Compare(f.Name, lastFileSel, true) == 0)
                        {
                            newli.Selected = true;
                        }
                    }
                }
            }
            if (opened)
                SelectedUSB.Close();

            if (listUSBFileView.SelectedItems.Count == 0)
                textBoxUSBFileName.Text = "";
            else
            {
                listUSBFileView.SelectedItems[0].EnsureVisible();
            }
            listUSBFileView.EndUpdate();
        }

        private void buttonUSBSelectAllSongs_Click(object sender, EventArgs e)
        {
            listBoxUSBSongs.BeginUpdate();
            
            if (listBoxUSBSongs.SelectedItems.Count == listBoxUSBSongs.Items.Count)
            {
                foreach (ListViewItem s in listBoxUSBSongs.SelectedItems) s.Selected = false;
            }
            else
            {
                foreach (ListViewItem s in listBoxUSBSongs.SelectedItems) s.Selected = true;
                
            }
            listBoxUSBSongs.EndUpdate();
        }

    

        private void button126_Click(object sender, EventArgs e)
        {
            if (!CopyPowerupsFromG5(checkBoxInitSelectedTrackOnly.Checked == false))
            {
                MessageBox.Show("Failed copying powerups");
            }
        }

        public bool CopyPowerupsFromG5(bool alltracks)
        {
            bool ret = true;
            ExecAndRestoreTrackDifficulty(delegate()
            {

                try
                {
                    EditorPro.BackupSequence();

                    if (!alltracks)
                    {
                        CopyPowerupDataForCurrentTrack();
                    }
                    else
                    {
                        foreach (var trackname in GuitarTrack.TrackNames6)
                        {
                            Track g5, g6;
                            if (GuitarTrack.IsBassTrackName(trackname))
                            {
                                g5 = EditorG5.GetTrack(GuitarTrack.BassTrackName5);
                            }
                            else
                            {
                                g5 = EditorG5.GetTrack(GuitarTrack.GuitarTrackName5);
                            }
                            g6 = EditorPro.GetTrack(trackname);

                            if (g5 != null && g6 != null)
                            {

                                if (EditorG5.SetTrack5(g5, GuitarDifficulty.Expert) &&
                                    EditorPro.SetTrack6(g6, GuitarDifficulty.Expert))
                                {
                                    CopyPowerupDataForCurrentTrack();
                                }
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

        private void button127_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecuteBatchCopyUSB();
        }

        private void buttonUSBViewPackage_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSB.Open())
            {
                var files = SelectedUSBFiles;
                if (files.Count == 1)
                {

                    Package p = Package.Load(files[0].GetFatXStream(),false);
                    
                    try
                    {
                        //Package p = Package.Load(bytes);
                        // if (p != null)
                        {
                            LoadPackageIntoTree(p);

                            textBoxPackageDTAText.Text = "";
                            tabContainerMain.SelectedTab = tabPackageEditor;
                            tabControl3.SelectedTab = tabPage8;
                        }
                            
                    }
                    catch { }
                    
                }
                else
                {
                    if (files.Count > 0)
                    {
                        MessageBox.Show("Must have only one file selected");
                    }
                }
                SelectedUSB.Close();
            }
        }

        private void buttonUSBRenameFile_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            var name = textBoxUSBFileName.Text;
            var files = SelectedUSBFiles;
            if (files.Count == 1 && SelectedUSBNode != null)
            {
                if (!RenameUSBFile(name))
                {
                    MessageBox.Show("Unable to rename");
                }
            }
            else
            {
                if (files.Count > 0)
                {
                    MessageBox.Show("Must have only one file selected");
                }
            }
        }

        private bool RenameUSBFile(string fileName)
        {
            bool ret = false;

            if (SelectedUSBFiles.Count != 1)
                return ret;

            if (SelectedUSB.Open())
            {
                bool valid = X360.Other.VariousFunctions.IsValidXboxName(fileName);

                if (valid)
                {
                    var folder = SelectedUSBFolderEntry;
                    var contents = SelectedUSBContents;

                    if (contents != null)
                    {
                        var file = SelectedUSBFiles[0];
                        if (!(string.Compare(fileName, file.Name, true) == 0))
                        {
                            valid = true;
                            foreach (var f in contents.Folders)
                            {
                                if (string.Compare(f.Name, fileName, true) == 0)
                                {
                                    valid = false;
                                    break;
                                }
                            }
                            if (valid)
                            {
                                file.Name = fileName;
                                ret = file.xWriteEntry();
                            }
                        }
                    }
                }
                
                SelectedUSB.Close();
                RefreshUSB();
            }
            return ret;
        }

     

        private void buttonBatchOpenResult_Click(object sender, EventArgs e)
        {
            OpenBatchResults();
        }

     

        private void OnDragEnterListUSBFileView(object sender, DragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSBFolderEntry != null)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    e.Effect = DragDropEffects.All;

                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string file in files)
                    {
                        if (!(File.Exists(file) || Directory.Exists(file)))
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
                {
                    try
                    {
                        object o = e.Data.GetData(DataFormats.Serializable, false);
                        if (o != null)
                        {
                            ListViewItem item = o as ListViewItem;
                            if (item != null && item.Tag != null &&
                                item.Tag is SongCacheItem)
                            {
                                var sng = item.Tag as SongCacheItem;

                                if (!string.IsNullOrEmpty(sng.G6ConFile))
                                {
                                    if (File.Exists(sng.G6ConFile))
                                    {
                                        e.Effect = DragDropEffects.All;
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void OnDragOverListUSBFileView(object sender, DragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }

            
            e.Effect = DragDropEffects.All;
                
            
        }
        private void OnDragDropListUSBFileView(object sender, DragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        AddFileToUSB(file);
                    }
                    else if (Directory.Exists(file))
                    {
                        AddFolderToUSB(file);
                    }
                }

                RefreshUSB();

            }
            else if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
            {
                try
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ListViewItem item = o as ListViewItem;
                        if (item != null && item.Tag != null &&
                            item.Tag is SongCacheItem)
                        {
                            var sng = item.Tag as SongCacheItem;

                            if (!string.IsNullOrEmpty(sng.G6ConFile))
                            {
                                if (File.Exists(sng.G6ConFile))
                                {
                                    AddFileToUSB(sng.G6ConFile);
                                    RefreshUSB();
                                }
                            }
                        }
                    }
                }
                catch { }
            }
                
            
        }

        void treeUSBContents_DragDrop(object sender, DragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }

            var tn = treeUSBContents.GetNodeAt(treeUSBContents.PointToClient(
                new Point(e.X, e.Y)));

            if (tn != null && tn.Tag != null && tn.Tag is FATXFolderEntry)
            {
                
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    treeUSBContents.BeginUpdate();
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            AddFileToUSB(file, tn);
                        }
                        else if (Directory.Exists(file))
                        {
                            AddFolderToUSB(file, tn);
                        }
                    }
                    treeUSBContents.EndUpdate();
                    RefreshUSB();
                }
                else if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ListViewItem item = o as ListViewItem;
                        if (item != null && item.Tag != null && 
                            listUSBFileView.Items.Contains(item) &&
                            item.Tag is FATXFileEntry)
                        {
                            var fe = item.Tag as FATXFileEntry;
                            
                            if (SelectedUSB.Open())
                            {
                                treeUSBContents.BeginUpdate();
                                            
                                try
                                {
                                    var fileFolderNode = listUSBFileView.Tag as TreeNode;

                                    FATXFolderEntry xfolder;
                                    var contents = SelectedUSB.Drive.ReadToFolder(
                                        GetUSBTreeNodePath(fileFolderNode), out xfolder);

                                    if (xfolder != null && contents != null)
                                    {
                                        bool found = false;
                                        foreach (var file in contents.Files)
                                        {
                                            if (string.Compare(file.Name, fe.Name, true) == 0)
                                            {
                                                fe = file;
                                                found = true;
                                                break;
                                            }
                                        }
                                        if (found)
                                        {
                                            item.Tag = fe;
                                            
                                            var b = fe.xExtractBytes();
                                            if (b != null)
                                            {

                                                var folder = tn.Tag as FATXFolderEntry;
                                                if (folder != null)
                                                {
                                                    FATXFolderEntry xfolder2;
                                                    var contents2 = SelectedUSB.Drive.ReadToFolder(
                                                        GetUSBTreeNodePath(tn), out xfolder2);

                                                    if (xfolder2.AddFile(item.Text, b, AddType.NoOverWrite))
                                                    {
                                                                    
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                                treeUSBContents.EndUpdate();

                                SelectedUSB.Close();
                            }
                            RefreshUSB();
                        }
                    }
                }
                
            }
        }

        void treeUSBContents_DragOver(object sender, DragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }

            var tn = treeUSBContents.GetNodeAt(treeUSBContents.PointToClient(
                new Point(e.X, e.Y)));

            if (tn != null && tn.Tag != null && tn.Tag is FATXFolderEntry)
            {
                
                if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ListViewItem item = o as ListViewItem;
                        if (item != null && item.Tag != null && listUSBFileView.Items.Contains(item) &&
                            listUSBFileView.Tag != null)
                        {
                            var folderNode = listUSBFileView.Tag as TreeNode;

                            if (folderNode != tn)
                            {
                                if ((ModifierKeys & Keys.Control) == Keys.Control)
                                {
                                    e.Effect = DragDropEffects.All;
                                }
                                else
                                {
                                    e.Effect = DragDropEffects.All;
                                }
                                treeUSBContents.SelectedNode = tn;
                            }
                        }
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.All;
                    treeUSBContents.SelectedNode = tn;
                }
                
            }
        }
        

  
        private void listUSBFileView_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                e.CancelEdit = true;
                return;
            }

            

            if (listUSBFileView.SelectedItems.Count == 1)
            {
                ListViewItem li = listUSBFileView.SelectedItems[0];
                if (li.Tag == null)
                {
                    e.CancelEdit = true;
                }
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void listUSBFileView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {

            if (e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }

            if (!HasValidUSBDeviceSelection)
            {
                e.CancelEdit = true;
                return;
            }

            if (listUSBFileView.SelectedItems.Count == 1)
            {
                ListViewItem li = listUSBFileView.SelectedItems[0];
                if (li.Tag == null)
                {
                    e.CancelEdit = true;
                }
                else
                {
                    if (!RenameUSBFile(e.Label))
                        e.CancelEdit = true;
                }
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void listUSBFileView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    listUSBFileView.DoDragDrop(e.Item, DragDropEffects.Copy);
                }
                else
                {
                    listUSBFileView.DoDragDrop(e.Item, DragDropEffects.Move);
                }
            }
        }

   

        private void OnDragEnterEditorPro(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    e.Effect = DragDropEffects.All;
                }
                else if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ListViewItem item = o as ListViewItem;
                        if (item != null && item.Tag != null &&
                            item.Tag is SongCacheItem)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else if (item != null && item.Tag != null &&
                            item.Tag is FATXFileEntry)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                    }
                }
            }
            catch { e.Effect = DragDropEffects.None; }
        }

        [Flags()]
        public enum EditorFileType
        {
            Midi6 = (1<<0),
            Midi5 = (1<<1),
            Con6   = (1<<2),
            Con5   = (1<<3),
            Any5 = (Midi5 | Con5),
            Any6 = (Midi6 | Con6),
            AnyMidi = (Midi5 | Midi6),
            AnyCon = (Con6 | Con5),
            Any = (AnyMidi | AnyCon),
        }

        public bool OpenEditorFile(string fileName, byte[] data=null, EditorFileType openFileType = EditorFileType.Any)
        {
            bool ret = false;
            try
            {
                if (fileName.IsEmpty())
                    fileName = "Unknown";

                
                if (data == null && !fileName.FileExists())
                    return ret;

                if (data == null && fileName.FileExists())
                {
                    data = ReadFileBytes(fileName);
                }

                if (data == null)
                    return ret;


                CloseSelectedSong();


                Track track6 = null;
                Track track5 = null;

                bool opened = false;
                if ((openFileType & EditorFileType.AnyCon) != 0)
                {
                    Package p = Package.Load(data);
                    if (p != null)
                    {
                        opened = true;

                        var midiFiles = p.GetFilesByExtension(".mid|.midi");

                        foreach (var mid in midiFiles)
                        {
                            try
                            {
                                var seq = mid.Data.LoadSequence();
                                
                                if (seq.FileType != FileType.Unknown)
                                {
                                    track6 = seq.FirstOrDefault(x => x.Name.IsProTrackName());
                                    track5 = seq.FirstOrDefault(x => x.Name.IsGuitarTrackName5() || x.Name.IsBassTrackName5());
                                }

                            }
                            catch
                            {
                            }
                        }

                    }
                }

                if (!opened)
                {
                    try
                    {
                        var seq = data.LoadSequence();
                        
                        if (openFileType.HasFlag(EditorFileType.Midi6))
                        {
                            var g6 = seq.Tracks.Where(x => x.Name.IsGuitarTrackName6());
                            var b6 = seq.Tracks.Where(x => x.Name.IsBassTrackName6());
                            if (g6.Any())
                                track6 = g6.FirstOrDefault();
                            else if (b6.Any())
                                track6 = b6.FirstOrDefault();
                        }
                        if (openFileType.HasFlag(EditorFileType.Midi5))
                        {
                            var g5 = seq.Tracks.Where(x => x.Name.IsGuitarTrackName5());
                            var b5 = seq.Tracks.Where(x => x.Name.IsBassTrackName5());
                            if (g5.Any())
                                track5 = g5.FirstOrDefault();
                            else if (b5.Any())
                                track5 = b5.FirstOrDefault();
                        }

                        opened = true;
                    }
                    catch
                    {

                    }
                }

                if (opened)
                {
                    if (track5 != null)
                    {
                        ret = EditorG5.SetTrack5(track5, GuitarDifficulty.Expert);
                        if (ret)
                        {
                            FileNameG5 = fileName;
                            RefreshTracks5();
                        }
                    }

                    if (track6 != null)
                    {
                        ret = EditorPro.SetTrack6(track6, GuitarDifficulty.Expert);
                        if (ret)
                        {
                            FileNamePro = fileName;
                            RefreshTracks6();
                        }
                    }
                }
            }
            catch { ret = false; }
            return ret;
        }



        public static FileType FindSequenceFileType(Sequence seq, EditorFileType openFileType = EditorFileType.Any)
        {
            if (openFileType.HasFlag(EditorFileType.Con6) || openFileType.HasFlag(EditorFileType.Midi6))
            {
                if (seq.Tracks.Any(x => x.Name.IsProTrackName()))
                {
                    seq.FileType = FileType.Pro;
                }
            }
            if (seq.FileType == FileType.Unknown)
            {
                if (openFileType.HasFlag(EditorFileType.Con5) || openFileType.HasFlag(EditorFileType.Midi5))
                {
                    if (seq.Tracks.Any(x => x.Name.IsBassTrackName5() || x.Name.IsGuitarTrackName5()))
                    {
                        seq.FileType = FileType.Guitar5;
                    }
                }
            }

            if (seq.FileType == FileType.Unknown)
            {
                if (seq.Tracks.Any(x => x.Name.IsProTrackName()))
                {
                    seq.FileType = FileType.Pro;
                }
                else if (seq.Tracks.Any(x => x.Name.IsBassTrackName5() || x.Name.IsGuitarTrackName5()))
                {
                    seq.FileType = FileType.Guitar5;
                }
            }
            return seq.FileType;
        }

        private void OnDragDropEditorPro(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                
                try
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            if (OpenEditorFile(file))
                                break;
                        }
                    }
                }
                catch { }
                
            }
            else if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
            {
                try
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ListViewItem item = o as ListViewItem;
                        if (item != null && item.Tag != null &&
                            item.Tag is SongCacheItem)
                        {
                            var sng = item.Tag as SongCacheItem;

                            OpenSongCacheItem(sng);
                        }
                        else if (item != null && item.Tag != null &&
                            item.Tag is FATXFileEntry)
                        {
                            if (SelectedUSB.Open())
                            {
                                try
                                {
                                    foreach (var f in SelectedUSBFiles)
                                    {
                                        if (OpenEditorFile("USBFile - " + f.Name, f.xExtractBytes()))
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch { }
                                SelectedUSB.Close();
                            }
                        }
                    }
                }
                catch { }
            }
        }

    

        private void listBoxUSBSongs_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e != null && e.Item != null)
            {
                listBoxUSBSongs.DoDragDrop(e.Item, DragDropEffects.All);
            }
        }




        
        private void button67_Click_1(object sender, EventArgs e)
        {
            if (treePackageContents.Tag != null)
            {
                CheckCONPackageBytes((treePackageContents.Tag as Package), false);
            }
        }


        private void midiTrackEditorPro_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSongCacheItem(null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioNoteEditDifficultyExpert_Click(object sender, EventArgs e)
        {
            
            if (DesignMode)
                return;
            if (((RadioButton)sender).Checked)
            {
                SetEditorDifficulty(GuitarDifficulty.Expert);
            }
        }

        private void radioNoteEditDifficultyHard_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (((RadioButton)sender).Checked)
            {
                SetEditorDifficulty(GuitarDifficulty.Hard);
            }
        }

        private void radioNoteEditDifficultyMedium_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (((RadioButton)sender).Checked)
            {
                SetEditorDifficulty(GuitarDifficulty.Medium);
            }
        }

        private void radioNoteEditDifficultyEasy_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (((RadioButton)sender).Checked)
            {
                SetEditorDifficulty(GuitarDifficulty.Easy);
            }
        }

        private void buttonUSBCheckFile_Click(object sender, EventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;

            if (SelectedUSB.Open())
            {
                trackEditorG5.Close();
                bool hasErrors = false;
                ClearBatchResults();
                var files = SelectedUSBFiles;
                foreach(var f in files)
                {
                    WriteBatchResult("Processing " + f.Name);
                    
                    try
                    {
                        var p = Package.Load(f.GetFatXStream());
                        if(p != null)
                        {
                            if (!CheckCONPackageBytes(p, true))
                            {
                                hasErrors = true;
                            }
                            else
                            {
                                WriteBatchResult("No Errors for " + f.Name);
                            }
                        }
                        else
                        {
                            WriteBatchResult("Unable to open " + f.Name);
                            hasErrors = true;
                        }
                    }
                    catch
                    {
                        WriteBatchResult("Error loading " + f.Name);
                        hasErrors = true;
                    }
                }
                
                SelectedUSB.Close();

                if (hasErrors)
                {
                    OpenBatchResults();
                }
                else
                {
                    MessageBox.Show("No errors found");
                }
            }
            
        }

        private void listUSBFileView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!HasValidUSBDeviceSelection)
                return;
            bool loaded = false;
            if (SelectedUSB.Open())
            {
                try
                {

                    
                    foreach (var f in SelectedUSBFiles)
                    {
                        if (OpenEditorFile("USBFile - " + f.Name, f.xExtractBytes()))
                        {
                            loaded = true;
                            break;
                        }
                    }
                    
                }
                catch { }
                SelectedUSB.Close();

                if (!loaded)
                {
                    MessageBox.Show("Could not open file");
                }
            }
        }

        private void buttonCloseG5Track_Click(object sender, EventArgs e)
        {
            trackEditorG5.Close();
        }

        private void buttonCloseG6Track_Click(object sender, EventArgs e)
        {
            trackEditorG6.Close();
        }

        private void treePackageContents_DoubleClick(object sender, EventArgs e)
        {
            if (treePackageContents.SelectedNode == null)
                return;
            if (treePackageContents.Tag == null)
                return;


            try
            {
                OpenSongCacheItem(null);

                var f = treePackageContents.SelectedNode.Tag as PackageFile;
                if (f != null)
                {
                    try
                    {
                        if (f.Name.IsMidiFileName())
                        {
                            var seq = f.Data.LoadSequence();
                            
                            bool pro = false;
                            foreach (var t in seq)
                            {
                                if (GuitarTrack.TrackNames6.Contains(t.Name))
                                {
                                    pro = true;
                                    break;
                                }
                            }

                            if (pro)
                            {
                                trackEditorG6.Close();

                                trackEditorG6.SetTrack6(seq, trackEditorG6.GetProTrack(seq), GuitarDifficulty.Expert);
                                FileNamePro = "Temp - " + f.Name;
                            }
                            else
                            {
                                trackEditorG5.Close();

                                Track t5 = null;
                                foreach (var tn in GuitarTrack.TrackNames5)
                                {
                                    t5 = seq.GetTrack(tn);
                                    if (t5 != null)
                                        break;
                                }
                                if (t5 == null && seq.Count > 0)
                                    t5 = seq.Tracks[0];
                                trackEditorG5.SetTrack5(seq, t5, GuitarDifficulty.Expert);
                                FileNameG5 = "Temp - " + f.Name;
                            }

                            ReloadTracks();
                        }
                        else if (f.Name.EndsWith(".dta", StringComparison.OrdinalIgnoreCase))
                        {
                            OpenNotepad(f.Data);
                        }
                    }
                    catch { }
                }
            }
            
            catch { }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedSong != null && File.Exists(SelectedSong.G6ConFile))
                {
                    var p = Package.Load(SelectedSong.G6ConFile);
                    if (p != null)
                    {
                        LoadPackageIntoTree(p);

                        textBoxPackageDTAText.Text = "";
                    }
                }
            }
            catch { }
        }

        private void tabControl1_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.All;
        }

        private void tabControl1_DragLeave(object sender, EventArgs e)
        {/*
            var t = tabControl1.SelectedTab;
            if (t != null)
            {
                tabControl1.TabPages.Remove(t);
                var po = new TabPopout(t);
                popOuts.Add(po);
            }*/
        }

        private void tabPage3_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.All;
        }


        List<TabPopout> popOuts = new List<TabPopout>();
        private void tabPage3_DragLeave(object sender, EventArgs e)
        {
            /*tabControl1.TabPages.Remove(tabPage3);
            var po = new TabPopout(tabPage3);
            popOuts.Add(po);*/
        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.All;
        }

        private void trackEditorG5_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                trackEditorG5.Close();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    try
                    {
                        file.LoadSequenceFile().IfObjectNotNull(seq =>
                        {
                            if (seq.IsFileTypeG5())
                            {
                                trackEditorG5.LoadedFileName = file;
                                trackEditorG5.SetTrack5(seq, seq.GetPrimaryTrack(), GuitarDifficulty.Expert);
                            }
                        });
                    }
                    catch { }
                }
                
            }
        }

        private void trackEditorG5_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    e.Effect = DragDropEffects.All;

                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string file in files)
                    {
                        if (!(File.Exists(file) || Directory.Exists(file)))
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonClearAllStrum_Click(object sender, EventArgs e)
        {
            try
            {
                EditorPro.BackupSequence();

                RemoveAllStrum();

                ReloadTrack();
            }
            catch { }
        }

        private void listUSBFileView_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button128_Click(object sender, EventArgs e)
        {
            CreateBassFromSelectedGuitarTrack();
        }

        private void CreateBassFromSelectedGuitarTrack()
        {
            try
            {
                EditorPro.BackupSequence();

                if (GuitarTrack.GuitarTrackName17 == EditorPro.GuitarTrack.Name)
                {
                    CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17);
                }
                if (GuitarTrack.GuitarTrackName22 == EditorPro.GuitarTrack.Name)
                {
                    CopyTrack(GuitarTrack.GuitarTrackName22, GuitarTrack.BassTrackName22);
                }

                ReloadTracks();
            }
            catch { }
        }

        private void button129_Click(object sender, EventArgs e)
        {
            if (SelectedSong != null)
            {
                if (CopySongToUSB(SelectedSong))
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Unable to copy to USB");
                }
            }
        }


        private void buttonInitTempo_Click(object sender, EventArgs e)
        {
            CopyG5Tempo();
        }

        private void button130_Click_1(object sender, EventArgs e)
        {
            CreateDummyTimeSig(EditorPro);
        }

        public void CreateDummyTimeSig(TrackEditor editor)
        {
            try
            {
                if (editor.IsLoaded)
                {
                    EditorPro.BackupSequence();
                    var tt = editor.GuitarTrack.FindTempoTrack();
                    if (tt != null)
                    {
                        var remove = new List<MidiEvent>();
                        foreach (var msg in tt.Tempo)
                        {
                            var mm = msg.MetaMessage;
                            if (mm != null)
                            {
                                if (mm.MetaType == MetaType.TimeSignature)
                                {
                                    remove.Add(msg);
                                }
                            }
                        }
                        foreach (var rm in remove)
                        {
                            tt.Remove(rm);
                        }

                        var gts = new GuitarTimeSignature(editor.GuitarTrack, null);
                        var ts = new TimeSignatureBuilder();
                        ts.ClocksPerMetronomeClick = (byte)gts.ClocksPerMetronomeClick;

                        
                        var numer = textBoxTempoNumerator.Text.ToInt((int)gts.Numerator);
                        
                        
                        var denom = textBoxTempoDenominator.Text.ToInt((int)gts.Denominator);
                        
                        ts.Denominator = (byte)denom;
                        ts.Numerator = (byte)numer;
                        ts.ThirtySecondNotesPerQuarterNote = (byte)gts.ThirtySecondNotesPerQuarterNote;
                        ts.Build();

                        var ev = tt.Insert(0, ts.Result);
                        gts.SetDownEvent(ev);
                       
                        ReloadTracks();
                    }
                }
            }
            catch { MessageBox.Show("Cannot create time signature"); }
        }

        private void button131_Click(object sender, EventArgs e)
        {
            if (EditorG5.IsLoaded && EditorG5.GuitarTrack != null &&
                EditorG5.GuitarTrack.HasInvalidTempo == false &&
                EditorG5.GuitarTrack.TempoTrack.TimeSig.Any())
            {
                var ts = EditorG5.GuitarTrack.TempoTrack.TimeSig.First();

                var tsb = new TimeSignatureBuilder(ts.MetaMessage);
                
                textBoxTempoDenominator.Text = tsb.Denominator.ToString();
                textBoxTempoNumerator.Text = tsb.Numerator.ToString();
            }
            else
            {
                textBoxTempoDenominator.Text = "4";
                textBoxTempoNumerator.Text = "4";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

       

        private void buttonDownString_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            MoveSelectedDownString();
        }


        public void MoveSelectedDownString()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;

                foreach (var ch in EditorPro.SelectedChords)
                {
                    foreach (var n in ch.Notes.ToList())
                    {
                        if (n.NoteString == 0)
                        {
                            ch.RemoveNote(n);
                        }
                        else
                        {
                            n.NoteString--;

                            if (n.NoteString == 3)
                            {
                                n.NoteFretDown += 4;
                            }
                            else
                            {
                                n.NoteFretDown += 5;
                            }
                            if (n.NoteFretDown > 23)
                                n.NoteFretDown = 23;
                        }
                    }
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        private void buttonUp12_Click(object sender, EventArgs e)
        {
            MoveSelectedUp12Frets();
        }

        public void MoveSelectedUp12Frets()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;

                foreach (var ch in EditorPro.SelectedChords)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        var n = ch.Notes[x];
                        if (n != null)
                        {
                            if (n.NoteFretDown <= 10)
                            {
                                n.NoteFretDown += 12;
                            }
                        }
                    }
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        private void groupBoxPowerup_Enter(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonUSBAddFolder_Click_1(object sender, EventArgs e)
        {

        }

        private void button99_Click_1(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void button100_Click_1(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void buttonNoteEditorCopyPatternPresetCreate_Click(object sender, EventArgs e)
        {
            AddSelectedCopyPatternPreset();
        }


        public class CopyPatternPreset
        {
            public int ID;
            public string Name;
            public bool ForwardOnly;
            public bool MatchLengths5;
            public bool MatchLengths6;
            public bool MatchSpacing;
            public bool MatchBeat;
            public bool KeepLengths;
            public bool FirstMatchOnly;
            public bool RemoveExisting;

            public override string ToString()
            {
                return Name;
            }
            public void CopyTo(CopyPatternPreset item)
            {
                item.Name = Name;
                item.ForwardOnly = ForwardOnly;
                item.MatchLengths5 = MatchLengths5;
                item.MatchLengths6 = MatchLengths6;
                item.MatchSpacing = MatchSpacing;
                item.MatchBeat = MatchBeat;
                item.KeepLengths = KeepLengths;
                item.FirstMatchOnly = FirstMatchOnly;
                item.RemoveExisting = RemoveExisting;
            }
        }

        public List<CopyPatternPreset> GetCopyPatternPresetsFromScreen()
        {
            var ret = new List<CopyPatternPreset>();
            foreach (CopyPatternPreset preset in comboNoteEditorCopyPatternPreset.Items)
            {
                ret.Add(preset);
            }
            return ret;
        }

        public CopyPatternPreset GetNewCopyPatternPresetFromScreen()
        {
            return new CopyPatternPreset()
            {
                ID = Int32.MinValue,
                Name = comboNoteEditorCopyPatternPreset.Text,
                ForwardOnly = checkBoxMatchForwardOnly.Checked,
                MatchLengths5 = checkBoxMatchLengths.Checked,
                MatchLengths6 = checkBoxMatchLength6.Checked,
                MatchSpacing = checkBoxMatchSpacing.Checked,
                MatchBeat = checkMatchBeat.Checked,
                KeepLengths = checkBoxKeepLengths.Checked,
                FirstMatchOnly = checkBoxFirstMatchOnly.Checked,
                RemoveExisting = checkBoxMatchRemoveExisting.Checked,
            };

           
        }
        public CopyPatternPreset GetSelectedCopyPatternPresetFromScreen()
        {
            var index = comboNoteEditorCopyPatternPreset.SelectedIndex;
            if (index != -1)
            {
                return comboNoteEditorCopyPatternPreset.Items[index] as CopyPatternPreset;
            }
            return null;
        }

        public void SetCopyPatternToScreen(CopyPatternPreset pattern)
        {
            comboNoteEditorCopyPatternPreset.Text = pattern.Name;
            checkBoxMatchForwardOnly.Checked = pattern.ForwardOnly;
            checkBoxMatchLengths.Checked = pattern.MatchLengths5;
            checkBoxMatchLength6.Checked = pattern.MatchLengths6;
            checkBoxMatchSpacing.Checked = pattern.MatchSpacing;
            checkMatchBeat.Checked = pattern.MatchBeat;
            checkBoxKeepLengths.Checked = pattern.KeepLengths;
            checkBoxFirstMatchOnly.Checked = pattern.FirstMatchOnly;
            checkBoxMatchRemoveExisting.Checked = pattern.RemoveExisting;
        }

        public void AddSelectedCopyPatternPreset()
        {
            var index = comboNoteEditorCopyPatternPreset.Items.Add(GetNewCopyPatternPresetFromScreen());
            comboNoteEditorCopyPatternPreset.SelectedIndex = index;
            comboNoteEditorCopyPatternPreset.Invalidate();
        }

        public void UpdateSelectedCopyPatternPreset()
        {
            
            var item = GetSelectedCopyPatternPresetFromScreen();
            var cp = GetNewCopyPatternPresetFromScreen();
            if (item != null)
            {
                cp.CopyTo(item);
            }
            else
            {
                item = comboNoteEditorCopyPatternPreset.Tag as CopyPatternPreset;
                if (item != null)
                {
                    cp.CopyTo(item);
                }
                else
                {
                    return;
                }
            }

            var items = GetCopyPatternPresetsFromScreen();
            comboNoteEditorCopyPatternPreset.Items.Clear();
            foreach (var i in items)
            {
                comboNoteEditorCopyPatternPreset.Items.Add(i);
            }
            if (item != null)
            {
                comboNoteEditorCopyPatternPreset.SelectedItem = item;
            }
        }

        public void RemoveSelectedCopyPatternPreset()
        {
            var sel = GetSelectedCopyPatternPresetFromScreen();
            if (sel != null)
            {
                comboNoteEditorCopyPatternPreset.Items.Remove(sel);
                comboNoteEditorCopyPatternPreset.SelectedIndex = -1;
            }
        }

        public void SelectedCopyPatternPresetChanged()
        {
            if (comboNoteEditorCopyPatternPreset.SelectedIndex != -1)
            {
                var pattern = GetSelectedCopyPatternPresetFromScreen();
                if (pattern != null)
                {
                    SetCopyPatternToScreen(pattern);
                }
            }
        }
        private void buttonNoteEditorCopyPatternPresetUpdate_Click(object sender, EventArgs e)
        {
            UpdateSelectedCopyPatternPreset();
        }

        private void buttonNoteEditorCopyPatternPresetRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedCopyPatternPreset();
        }

        private void comboNoteEditorCopyPatternPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = GetSelectedCopyPatternPresetFromScreen();
            if (item != null)
            {
                comboNoteEditorCopyPatternPreset.Tag = item;
                SelectedCopyPatternPresetChanged();
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteSelectedNotes_Click(object sender, EventArgs e)
        {
            try
            {
                EditorPro.RemoveSelectedNotes();
            }
            catch { }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        public List<GuitarChord> FindStoredChordMatches()
        {
            List<GuitarChord> ret = new List<GuitarChord>();
            try
            {
                var ssc = SelectedStoredChord;
                var sc = FindFirstStoredChordMatch(ssc);
                while (sc != null)
                {
                    ret.Add(sc);
                    sc = FindNextStoredChordMatch(sc, ssc, false);
                }
            }
            catch
            {
            }
            return ret;
        }

        private void buttonReplaceStoredChordWithCopyPattern_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditorPro.IsLoaded)
                {
                    var ssc = SelectedStoredChord;
                    if(ssc == null)
                        return;

                    var selected = EditorPro.SelectedChords.ToArray();

                    if (selected == null || selected.Length == 0)
                        return;

                    var mcp = new MatchingCopyPattern()
                    {
                        DeltaTimeStart = 0,
                        
                    };

                    mcp.OriginalChords6 = selected;

                    var matchingChords = FindStoredChordMatches();
                    if (matchingChords == null || matchingChords.Count==0)
                        return;

                    if (matchingChords.Count > 100)
                    {
                        var mb = MessageBox.Show(string.Format("There are currently {0} matching chords, Continue?", 
                            matchingChords.Count),
                            "Warning", MessageBoxButtons.YesNo);
                        if (mb == System.Windows.Forms.DialogResult.No)
                            return;
                    }

                    EditorPro.BackupSequence();

                    foreach(var chord in matchingChords)
                    {
                        mcp.Matches.Add(new GuitarChord[]{chord});
                    }

                    
                    int numReplaced = 0;
                    int minTick = int.MinValue;
                    foreach (var m in mcp.Matches)
                    {
                        int irep = 0;
                        minTick = ReplaceNotes(EditorPro.GuitarTrack, 
                            mcp.OriginalChords6, mcp.DeltaTimeStart,
                                 minTick,  m,
                                m.GetMinTick(), m.GetMaxTick(), out irep);

                        numReplaced += irep;
                    }


                    if (numReplaced == 0)
                    {
                        MessageBox.Show("No Matches found");
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Replaced {0} Matches", numReplaced));
                    }
                }
            }
            catch { }
        }

        private void checkIsTap_CheckedChanged(object sender, EventArgs e)
        {
            if (checkIsTap.Checked)
            {
                for (int x = 0; x < HoldBoxes.Length; x++)
                {
                    if (HoldBoxes[x].Text.ToInt().IsNull() == false)
                    {
                        NoteChannelBoxes[x].Text = Utility.ChannelTap.ToString();
                    }
                    else
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                }
            }
            else
            {
                for (int x = 0; x < HoldBoxes.Length; x++)
                {
                    if (HoldBoxes[x].Text.ToInt().IsNull() == false)
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                    else
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                }
            }
        }
        private void checkIsX_CheckedChanged(object sender, EventArgs e)
        {
            if (checkIsX.Checked)
            {
                for (int x = 0; x < HoldBoxes.Length; x++)
                {
                    if (HoldBoxes[x].Text.ToInt().IsNull() == false)
                    {
                        NoteChannelBoxes[x].Text = Utility.ChannelX.ToString();
                    }
                    else
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                }
            }
            else
            {
                for (int x = 0; x < HoldBoxes.Length; x++)
                {
                    if (HoldBoxes[x].Text.ToInt().IsNull() == false)
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                    else
                    {
                        NoteChannelBoxes[x].Text = "";
                    }
                }
            }
        }

        private void checkIsHammeron_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void contextMenuStripChannels_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var st = sender as ContextMenuStrip;

            if (st != null && e.ClickedItem.Tag != null)
            {
                st.SourceControl.Text = e.ClickedItem.Tag.ToString();
            }
        }

        private void contextMenuStripChannels_Opening(object sender, CancelEventArgs e)
        {
            var st = sender as ContextMenuStrip;
            if (st != null)
            {
                foreach (ToolStripMenuItem item in st.Items)
                {
                    if (item.Text.EndsWith("Normal"))
                    {
                        item.Tag = "0";
                        item.Checked = st.SourceControl.Text.ToInt(0) == 0;
                    }
                    else if (item.Text.EndsWith("Helper"))
                    {
                        item.Tag = "1";
                        item.Checked = st.SourceControl.Text.ToInt(0) == 1;
                    }
                    else if (item.Text.EndsWith("X Note"))
                    {
                        item.Tag = "3";
                        item.Checked = st.SourceControl.Text.ToInt(0) == 3;
                    }
                    else if (item.Text.EndsWith("String Bend"))
                    {
                        item.Tag = "2";
                        item.Checked = st.SourceControl.Text.ToInt(0) == 2;
                    }
                    else if (item.Text.EndsWith("Tap"))
                    {
                        item.Tag = "4";
                        item.Checked = st.SourceControl.Text.ToInt(0) == 4;
                    }
                }
            }
        }

        private void buttonCheckAllInFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string pkPath = textBox25.Text;
                if (string.IsNullOrEmpty(pkPath))
                {
                    pkPath = ShowSelectFolderDlg("Select Package Directory", settings.GetValue("lastExtractConSourceFolder"), pkPath);
                    if (!string.IsNullOrEmpty(pkPath))
                        textBox25.Text = pkPath;
                }
                if (!string.IsNullOrEmpty(pkPath))
                {
                    var files = Directory.GetFiles(pkPath);

                    if (files.Length > 0)
                    {

                        settings.SetValue("lastExtractConSourceFolder", pkPath);

                        var fileErrors = new List<string>();
                        foreach (string fileName in files)
                        {
                            Package pk = null;
                            try
                            {
                                pk = Package.Load(fileName);
                            }
                            catch { }

                            if (pk != null)
                            {
                                int l = fileErrors.Count;
                                fileErrors.Add("");
                                fileErrors.Add("Checking: " + fileName);
                                CheckConPackage(pk, ref fileErrors, null);
                                fileErrors.Add("");
                                fileErrors.Add("");
                            }
                        }

                        var sb = new StringBuilder();
                        if (fileErrors.Count > 0)
                        {
                            foreach (var s in fileErrors)
                            {
                                sb.AppendLine(s);
                            }
                            OpenNotepad(Encoding.ASCII.GetBytes(sb.ToString()));
                        }
                        else
                        {
                            MessageBox.Show("Check OK");
                        }
                        
                        
                    }
                }

            }
            catch { }
        }

        private void button132_Click(object sender, EventArgs e)
        {
            FindNoteOver17();
        }

        private void FindNoteOver17()
        {
            try
            {
                GuitarChord sel = null;
                foreach (var chord in EditorPro.Messages.Chords)
                {
                    if (chord.IsDeleted)
                        continue;
                    foreach (var n in chord.Notes)
                    {
                        if (n != null)
                        {
                            if (n.NoteFretDown > 17)
                            {
                                sel = chord;
                                break;
                            }
                        }
                    }
                    if (sel != null)
                        break;
                }
                if (sel != null)
                {
                    SetSelectedChord(sel, false);
                    ScrollToSelection();
                }
                else
                {
                    MessageBox.Show("No Matches");
                }
            }
            catch { }
        }

        bool SnapChordToG5(GuitarChord chord)
        {

            SetSelectedChord(chord, false, true);
            SetChordToScreen(chord, false, true);

            int os = GetChordStartBox().Text.ToInt();
            int oe = GetChordEndBox().Text.ToInt();
            int gs = GetChordStartBox().Text.ToInt();
            int ge = GetChordEndBox().Text.ToInt();
            bool snappedS, snappedE;
            gs = EditorG5.GetTickNoteSnap(gs, out snappedS);
            ge = EditorG5.GetTickNoteSnap(ge, out snappedE);
            if (snappedS || snappedE)
            {
                GetChordStartBox().Text = gs.ToStringEx();
                GetChordEndBox().Text = ge.ToString();

                textBox19.Text = (ge - gs).ToStringEx();
                if (textBox19.Text.ToInt() > Utility.MinimumNoteWidth)
                {
                    if (os != gs || oe != ge)
                    {
                        UpdateSelectedChordProperties(SelectNextEnum.ForceKeepSelection);
                        return true;
                    }
                }
            }
            return false;
        }

        bool SetChordToG5Length(GuitarChord chord)
        {
            try
            {
                
                var gc = EditorG5.GuitarTrack.GetChordsAtTick(chord.DownTick, chord.UpTick);
                if (gc != null && gc.Any())
                {
                    if (gc.Count() > 1)
                    {
                        gc = EditorG5.GuitarTrack.GetChordsAtTick(chord.DownTick + Utility.NoteCloseWidth, chord.UpTick + Utility.NoteCloseWidth);
                        if (gc.Count() > 1)
                        {
                            gc = EditorG5.GuitarTrack.GetChordsAtTick(chord.DownTick - Utility.NoteCloseWidth, chord.UpTick - Utility.NoteCloseWidth);
                            if (gc.Count() > 1)
                            {
                                gc = EditorG5.GuitarTrack.GetChordsAtTick(chord.DownTick, chord.UpTick);
                            }
                        }
                    }
                    if (gc.Any())
                    {
                        var c = gc.First();
                        chord.DownTick = c.DownTick;
                        chord.UpTick = c.UpTick;
                        chord.UpdateChordProperties();
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
        private void button133_Click(object sender, EventArgs e)
        {
            SnapToG5();
        }

        private void SnapToG5()
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                EditorPro.BackupSequence();
                int numSnapped = 0;

                GuitarChord[] chords = EditorPro.SelectedChords.ToArray();
                if (chords.Length == 0)
                    chords = EditorPro.Messages.Chords;

                foreach (var c in chords)
                {
                    if (SnapChordToG5(c))
                        numSnapped++;
                }

                ReloadTracks(SelectNextEnum.ForceKeepSelection);
                MessageBox.Show(numSnapped.ToString() + " notes snapped");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void button134_Click(object sender, EventArgs e)
        {
            ShortenNotes();
        }

        private void ShortenNotes()
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                foreach (var chord in EditorPro.Messages.Chords)
                {
                    chord.UpTick = chord.UpTick - 1;
                    if (chord.UpTick < chord.DownTick)
                    {
                        chord.DownTick = chord.UpTick - 1;
                    }
                    UpdateChordProperties(chord, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch { }
        }

        void SetTrainerToScreen(GuitarTrainer trainer)
        {
            if (trainer == null)
            {
                textBoxProGuitarTrainerBeginTick.Text = "";
                textBoxProGuitarTrainerEndTick.Text = "";
            
                textBoxProBassTrainerBeginTick.Text = "";
                textBoxProBassTrainerEndTick.Text = "";

                checkTrainerLoopableProBass.Checked = false;
                checkTrainerLoopableProGuitar.Checked = false;
            }
            else if (trainer.TrainerType == GuitarTrainerType.ProGuitar)
            {
                textBoxProGuitarTrainerBeginTick.Text = trainer.Start.AbsoluteTicks.ToStringEx();
                textBoxProGuitarTrainerEndTick.Text = trainer.End.AbsoluteTicks.ToStringEx();
                checkTrainerLoopableProGuitar.Checked = trainer.Loopable;
            }
            else if (trainer.TrainerType == GuitarTrainerType.ProBass)
            {
                textBoxProBassTrainerBeginTick.Text = trainer.Start.AbsoluteTicks.ToStringEx();
                textBoxProBassTrainerEndTick.Text = trainer.End.AbsoluteTicks.ToStringEx();
                checkTrainerLoopableProBass.Checked = trainer.Loopable;
            }
            
        }

        private void listProGuitarTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTrainerToScreen(SelectedProGuitarTrainer);

            if (SelectedProGuitarTrainer != null)
            {
                EditorPro.ScrollToTick(SelectedProGuitarTrainer.Start.AbsoluteTicks);
            }
            EditorPro.Invalidate();
        }

        private void listProBassTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTrainerToScreen(SelectedProBassTrainer);

            if (SelectedProBassTrainer != null)
            {
                EditorPro.ScrollToTick(SelectedProBassTrainer.Start.AbsoluteTicks);
            }
            EditorPro.Invalidate();
        }

        private void buttonRemoveProGuitarTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            var gt = SelectedProGuitarTrainer;
            if (gt != null)
            {
                EditorPro.BackupSequence();
                SetTrainerToScreen(null);
                
                ProGuitarTrack.Remove(gt);
            }
            
            RefreshTrainers();
        }

        public GuitarTrainer SelectedProGuitarTrainer
        {
            get
            {
                return listProGuitarTrainers.SelectedItem as GuitarTrainer;
            }
            set
            {
                listProGuitarTrainers.SetSelectedItem(value);
            }
        }
        public GuitarTrainer SelectedProBassTrainer
        {
            get
            {
                return listProBassTrainers.SelectedItem as GuitarTrainer;
            }
            set
            {
                listProBassTrainers.SetSelectedItem(value);
            }
        }

        private void buttonRemoveProBassTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            var gt = SelectedProBassTrainer;
            if (gt != null)
            {
                EditorPro.BackupSequence();
                SetTrainerToScreen(null);
                
                ProGuitarTrack.Remove(gt);
            }
            RefreshTrainers();
        }

        private void buttonCancelProGuitarTrainer_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();
        }

        private void buttonCancelProBassTrainer_Click(object sender, EventArgs e)
        {
            EditorPro.SetStatusIdle();
        }

        private void buttonAddProGuitarTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                if (EditorPro.SelectedChords.Count > 1)
                {
                    EditorPro.BackupSequence();
                    CreateTrainer(GuitarTrainerType.ProGuitar, EditorPro.SelectedChords.GetMinTick(), EditorPro.SelectedChords.GetMaxTick(), checkTrainerLoopableProGuitar.Checked);
                        
                    RefreshTrainer(GuitarTrainerType.ProGuitar);
                    return;
                }
            }
            catch { }

            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingProGuitarTrainer;
        }

        private void buttonCreateProBassTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                if (EditorPro.SelectedChords.Count > 1)
                {
                    EditorPro.BackupSequence();

                    CreateTrainer(GuitarTrainerType.ProBass,EditorPro.SelectedChords.GetMinTick(), EditorPro.SelectedChords.GetMaxTick(),checkTrainerLoopableProBass.Checked);
                        
                    RefreshTrainer(GuitarTrainerType.ProBass);
                    return;
                }
            }
            catch { }
            EditorPro.CreationState = TrackEditor.EditorCreationState.CreatingProBassTrainer;
        }

        private void button135_Click(object sender, EventArgs e)
        {
            ReloadTrack();
        }

        private void buttonRefreshProBassTrainer_Click(object sender, EventArgs e)
        {
            RefreshTrainers();
        }

        private void buttonUpdateProGuitarTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            SelectedProGuitarTrainer.IfObjectNotNull(x =>
            {
                EditorPro.BackupSequence();
                var start = textBoxProGuitarTrainerBeginTick.Text.ToInt();
                var end = textBoxProGuitarTrainerEndTick.Text.ToInt();
                if (end > start)
                {
                    x.UpdateTicks(start, end, checkTrainerLoopableProGuitar.Checked);

                    RefreshTrainers();
                    SelectedProGuitarTrainer = x;
                }
                else
                {
                    MessageBox.Show("Invalid start or end tick");
                }
            });
        }

        private void buttonUpdateProBassTrainer_Click(object sender, EventArgs e)
        {
            if(!EditorPro.IsLoaded)
                return;

            SelectedProBassTrainer.IfObjectNotNull(x =>
            {
                EditorPro.BackupSequence();
                var start = textBoxProBassTrainerBeginTick.Text.ToInt();
                var end = textBoxProBassTrainerEndTick.Text.ToInt();
                if (end > start)
                {
                    x.UpdateTicks(start, end, checkTrainerLoopableProBass.Checked);

                    RefreshTrainers();

                    SelectedProBassTrainer = x;
                }
                else
                {
                    MessageBox.Show("Invalid start or end tick");
                }
            });
        }


        void SetTextEventToScreen(GuitarTextEvent ev)
        {
            if (ev == null)
            {
                textBoxEventTick.Text = "";
                textBoxEventText.Text = "";
                
            }
            else
            {
                textBoxEventText.Text = ev.Text;
                textBoxEventTick.Text = ev.AbsoluteTicks.ToStringEx();
            }
        }
        private void listTextEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorPro.SelectedTextEvent = listTextEvents.SelectedItem as GuitarTextEvent;

            SetTextEventToScreen(EditorPro.SelectedTextEvent);
        }

        private void buttonDeleteTextEvent_Click(object sender, EventArgs e)
        {
            if (EditorPro.IsLoaded)
            {
                try
                {
                    if (EditorPro.HasTextEventSelection)
                    {
                        EditorPro.BackupSequence();
                        ProGuitarTrack.Remove(EditorPro.SelectedTextEvent);
                    }
                }
                catch { }
                RefreshTextEvents();
            }
        }

        private void buttonRefreshTextEvents_Click(object sender, EventArgs e)
        {
            RefreshTextEvents();
        }

        private void buttonUpdateTextEvent_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                if (EditorPro.HasTextEventSelection)
                {
                    EditorPro.BackupSequence();

                    int tick = textBoxEventTick.Text.ToInt();
                    if (!tick.IsNull())
                    {
                        if (string.IsNullOrEmpty(textBoxEventText.Text))
                        {
                            MessageBox.Show("Invalid text");
                        }
                        else
                        {
                            EditorPro.SelectedTextEvent.Text = textBoxEventText.Text;
                            EditorPro.SelectedTextEvent.AbsoluteTicks = tick;
                            var ev = EditorPro.SelectedTextEvent;
                            RefreshTextEvents();
                            listTextEvents.SetSelectedItem(ev);
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Tick");
                    }
                }
            }
            catch { }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            RefreshTextEvents();
        }

        private void button136_Click(object sender, EventArgs e)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                CopyTextEvents(true);
            });
        }

        
        public void GenerateTrainers(bool selectedTrackOnly)
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {

                    EditorPro.BackupSequence();
                    IEnumerable<Track> gtList;
                    if (selectedTrackOnly)
                    {
                        var l = new List<Track>();
                        l.Add(EditorPro.GuitarTrack.GetTrack());
                        gtList = l;
                    }
                    else
                    {
                        gtList = EditorPro.Tracks.Where(x => x.Name.IsProTrackName());
                    }


                    if (!gtList.Any())
                        return;


                    foreach (var tr in gtList)
                    {
                        EditorPro.SetTrack(tr);

                        if (!EditorPro.Messages.Trainers.Any() && EditorPro.Messages.Solos.Any())
                        {
                            foreach (var solo in EditorPro.Messages.Solos)
                            {
                                CreateTrainer(tr.Name.IsGuitarTrackName17() ?
                                    GuitarTrainerType.ProGuitar : GuitarTrainerType.ProBass,
                                    solo.DownTick, solo.UpTick, true);
                            }
                        }
                    }

                    RefreshTrainers();
                }
                catch { }
            });
        }

        public void CopyTextEvents(bool selectedTrackOnly)
        {
            try
            {
                if (!EditorG5.IsLoaded)
                {
                    return;
                }
                if (selectedTrackOnly)
                {
                    foreach (var te5 in G5Track.Messages.TextEvents)
                    {
                        if (!ProGuitarTrack.Messages.TextEvents.Any(x => x.AbsoluteTicks == te5.AbsoluteTicks && x.Text == te5.Text))
                        {
                            GuitarTextEvent.CreateTextEvent(ProGuitarTrack, te5.AbsoluteTicks, te5.Text);
                        }
                    }
                }
                else
                {
                    var mb = new MetaTextBuilder();
                    var gt17 = EditorPro.GetTrackGuitar17();
                    var gt22 = EditorPro.GetTrackGuitar22();
                    var bt17 = EditorPro.GetTrackBass17();
                    var bt22 = EditorPro.GetTrackBass22();
                    var btg5 = EditorG5.GetGuitar5BassMidiTrack();
                    var gtg5 = EditorG5.GetGuitar5MidiTrack();

                    if (gtg5 != null && gt17 != null)
                    {
                        EditorG5.SetTrack5(gtg5, GuitarDifficulty.Expert);
                        EditorPro.SetTrack6(gt17, GuitarDifficulty.Expert);
                       
                        CopyTextEvents(true);
                    }
                    if (gtg5 != null && gt17 == null && gt22 != null)
                    {
                        EditorG5.SetTrack5(gtg5, GuitarDifficulty.Expert);
                        EditorPro.SetTrack6(gt22, GuitarDifficulty.Expert);
                        
                        CopyTextEvents(true);
                    }
                    if (btg5 != null && bt17 != null)
                    {
                        EditorG5.SetTrack5(btg5, GuitarDifficulty.Expert);
                        EditorPro.SetTrack6(bt17, GuitarDifficulty.Expert);
                       
                        CopyTextEvents(true);
                    }
                    if (btg5 != null && bt17 == null && bt22 != null)
                    {
                        EditorG5.SetTrack5(btg5, GuitarDifficulty.Expert);
                        EditorPro.SetTrack6(bt22, GuitarDifficulty.Expert);
                      
                        CopyTextEvents(true);
                    }
                }
                
            }
            catch { }
            RefreshTextEvents();
            
        }

        private void midiTrackEditorPro_TrackAdded(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            if (track != null)
            {
                ReloadTracks();

                EditorPro.SetTrack6(track, difficulty);

                ReloadTracks();
                RefreshTracks();
            }
        }

        private void midiTrackEditorG5_TrackAdded(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            ReloadTracks();
            EditorG5.SetTrack5(track, difficulty);
            ReloadTracks();
            RefreshTracks();
        }

        private void buttonBatchBuildTextEvents_Click(object sender, EventArgs e)
        {
            ClearBatchResults();
            ExecuteBatchBuildTextEvents();
        }

        private void checkBoxShow108_CheckedChanged(object sender, EventArgs e)
        {
            EditorPro.Show108Events = checkBoxShow108.Checked;
        }

        private void buttonRefresh108Events_Click(object sender, EventArgs e)
        {
            Refresh108EventList();
        }

        private void Refresh108EventList()
        {

            list108.BeginUpdate();

            list108.Items.Clear();

            try
            {
                if (EditorPro.IsLoaded)
                {
                    list108.Items.AddRange(ProGuitarTrack.Messages.HandPositions.ToArray());
                }
            }
            catch { }
            list108.EndUpdate();
            

        }

        private void list108_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var msg = list108.SelectedItem as GuitarMessage;
                if (msg != null)
                {
                    EditorPro.ScrollToTick(msg.AbsoluteTicks);
                }
            }
            catch { }
        }

        private void trackEditorG6_Load(object sender, EventArgs e)
        {

        }

        private void buttonUpOctave_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            MoveSelectedUpOctave();
        }

        private void buttonDownOctave_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            MoveSelectedDownOctave();
        }

        public void MoveSelectedDownOctave()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;


                foreach (var ch in EditorPro.SelectedChords)
                {
                    foreach(var n in ch.Notes.ToList())
                    {
                        if (n.NoteString > 1)
                        {
                            n.NoteString -= 2;
                                
                            if (n.NoteString == 3 || n.NoteString == 2)
                            {
                                n.NoteFretDown -= 3;
                            }
                            else
                            {
                                n.NoteFretDown -= 2;
                            }
                            if (n.NoteFretDown < 0)
                                n.NoteFretDown = 0;
                        }
                        else
                        {
                            ch.RemoveNote(n);
                        }
                    
                    }
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        public void MoveSelectedUpOctave()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;


                foreach (var ch in EditorPro.SelectedChords)
                {
                    foreach(var n in ch.Notes.ToList())
                    {
                        if (n.NoteString < 4)
                        {
                            n.NoteString+=2;

                            if (n.NoteString >= 4)
                            {
                                n.NoteFretDown += 3;
                            }
                            else 
                            {
                                n.NoteFretDown += 2;
                            }
                            if (n.NoteFretDown > 22)
                                n.NoteFretDown = 22;
                        }
                        else
                        {
                            ch.RemoveNote(n);
                        }
                    }
                    UpdateChordProperties(ch, false, SelectNextEnum.ForceKeepSelection);
                }
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
            }
            catch
            {
                UndoLast();
            }
        }

        private void buttonUtilMethodFindNoteLenZero_Click(object sender, EventArgs e)
        {
            FindNoteZeroLength();
        }


        private void FindNoteZeroLength()
        {
            try
            {
                GuitarChord sel = null;
                foreach (var chord in EditorPro.Messages.Chords)
                {
                    if (chord.IsDeleted)
                        continue;
                    if (chord.TickLength < Utility.NoteCloseWidth)
                    {
                        sel = chord;
                        break;
                    }
                    
                }
                if (sel != null)
                {
                    SetSelectedChord(sel, false);
                    ScrollToSelection();
                }
                else
                {
                    MessageBox.Show("No Matches");
                }
            }
            catch { }
        }

        private void buttonUtilMethodSetToG5_Click(object sender, EventArgs e)
        {
            SetLengthToG5();
        }


        private void SetLengthToG5()
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {
                EditorPro.BackupSequence();
                int numSnapped = 0;

                var chords = EditorPro.SelectedChords.ToArray();
                if (chords.Length == 0)
                    chords = EditorPro.Messages.Chords;

                foreach (var c in chords.ToList())
                {
                    if (SetChordToG5Length(c))
                        numSnapped++;
                }

                ReloadTracks(SelectNextEnum.ForceKeepSelection);
                MessageBox.Show(numSnapped.ToString() + " notes snapped");
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void button102_Click(object sender, EventArgs e)
        {
            try
            {
                int numSnapped = 0;
                var chords = EditorPro.GuitarTrack.Messages.Chords.Where(x => !x.IsDeleted && x.TickLength < Utility.NoteCloseWidth).ToList();
                foreach (var c in chords)
                {
                    if (SetChordToG5Length(c))
                        numSnapped++;
                }
                
                ReloadTracks(SelectNextEnum.ForceKeepSelection);
                MessageBox.Show(numSnapped.ToString() + " notes snapped");
           
            }
            catch { }
        }

        private void buttonSongPropertiesMidiPlay_Click(object sender, EventArgs e)
        {
            PlayMidiFromSelection();
        }

        private void buttonSongPropertiesMidiPause_Click(object sender, EventArgs e)
        {
            StopMidiPlayback();
        }

        private void buttonSongPropertiesChooseMP3Location_Click(object sender, EventArgs e)
        {
            var location = textBoxSongPropertiesMP3Location.Text.GetFolderName();
            if(location.IsEmpty() && SelectedSong != null)
                location = SelectedSong.G6FileName.GetIfEmpty(SelectedSong.G5FileName);
            
            ShowOpenFileDlg("Select MP3 file",
                DefaultMidiFileLocationPro,
                location.GetFolderName()).IfNotEmpty(mp3File=>
            {
                textBoxSongPropertiesMP3Location.Text = mp3File;
                textBoxSongPropertiesMP3Location.ScrollToEnd();
                UpdateSongCacheItem(SelectedSong);
            });
        }

        private void buttonSongPropertiesExploreMP3Location_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBoxSongPropertiesMP3Location.Text);
        }

        private void trackBarMidiVolume_Scroll(object sender, EventArgs e)
        {
            if (checkBoxEnableMidiPlayback.Checked)
            {
                ApplyMidiVolumeChange(trackBarMidiVolume.Value);
            }
            else
            {
                ApplyMidiVolumeChange(0);
            }
        }

        private void ApplyMidiVolumeChange(int volume)
        {
            midiPlaybackDeviceVolume = volume;
        }

        private void trackBarMP3Volume_Scroll(object sender, EventArgs e)
        {
            if (checkBoxSongPropertiesEnableMP3Playback.Checked)
            {
                ApplyMP3VolumeChange(trackBarMP3Volume.Value);
            }
            else
            {
                ApplyMP3VolumeChange(0);
            }
        }

        
        private void ApplyMP3VolumeChange(int volume)
        {
            mp3Player.Volume = volume;
        }


        private void enableMidiPlayback(bool enable)
        {
            midiPlaybackEnabled = enable;
        }

        private void checkBoxSongPropertiesEnableMP3Playback_CheckedChanged(object sender, EventArgs e)
        {
            enableMP3Playback(checkBoxSongPropertiesEnableMP3Playback.Checked);
        }

        private void enableMP3Playback(bool enable)
        {
            mp3PlaybackEnabled = enable;

            if (enable)
            {
                mp3Player.Volume = trackBarMP3Volume.Value;
            }
            else
            {
                mp3Player.Stop();
            }
        }

        enum ControlAnimatorType
        {
            Height,
        }

        enum AnimationRoutine
        {
            EaseInOutSine,
        }

        class ControlAnimatorItem
        {
            public AnimationRoutine Routine;
            public Control Control;
            
            public DateTime TimeStart;
            public DateTime TimeEnd;
            public DateTime CurrentTime;

            public ControlAnimatorType Type;
            public Point Start;
            public Point End;

            bool completed = false;
            public bool Completed
            {
                get
                {
                    return completed;
                }
                set
                {
                    completed = value;
                }
            }

            public double TotalAnimTime
            {
                get { return (TimeEnd - TimeStart).TotalSeconds; }
            }

            public double TimeElapsed
            {
                get { return (CurrentTime - TimeStart).TotalSeconds; }
            }

            public double Progress
            {
                get { return TimeElapsed / TotalAnimTime; }
            }
        }

        class ControlAnimator
        {
            static List<ControlAnimatorItem> animatingControls = new List<ControlAnimatorItem>();


            public static void CreateHeightChange(Control control, int to, double seconds=0.50)
            {
                var item = new ControlAnimatorItem()
                {
                    Routine = AnimationRoutine.EaseInOutSine,
                    Control = control,
                    TimeStart = DateTime.Now,
                    TimeEnd = DateTime.Now + TimeSpan.FromSeconds(seconds),
                    Type = ControlAnimatorType.Height,
                };

                item.Start = new Point(0, control.Height);
                item.End = new Point(0, to);

                var oldItem = animatingControls.SingleOrDefault(x => x.Control == control && x.Type == ControlAnimatorType.Height);
                if (oldItem != null)
                {
                    control.Height = oldItem.End.Y;
                    int delta = control.Height - to;
                    item.End.Y += delta;
                    animatingControls.Remove(oldItem);
                }
                
                animatingControls.Add(item);
            }


            
            public static double EaseInOutSine(double x, double t, double b, double c, double d)
            {
                return -c / 2.0 * (Math.Cos(Math.PI * x) - 1.0) + b;
            }

            public static double GetProgress(AnimationRoutine routine, double elapsed, double totalAnimTime)
            {
                double ret = 1.0;
                if (routine == AnimationRoutine.EaseInOutSine)
                {
                    if (elapsed > totalAnimTime)
                        elapsed = totalAnimTime;
                    ret = EaseInOutSine(elapsed / totalAnimTime, elapsed, 0, totalAnimTime, 1.0);
                }
                return ret;
            }

            public static Point GetUpdatedPoint(
                double progress, 
                Point start, Point end)
            {
                var ret = new Point(start.X + (int)(Math.Round((end.X - start.X) * progress)),
                                 start.Y + (int)(Math.Round((end.Y - start.Y) * progress)));

                return ret;
            }
            /*
             * 
 x = 0 - 1 as float of completion
 t = elapsed time in ms
 b = 0
 c = 1
 d = duration in ms


    easeInQuad: function (x, t, b, c, d) {
        return c*(t/=d)*t + b;
    },
    easeOutQuad: function (x, t, b, c, d) {
        return -c *(t/=d)*(t-2) + b;
    },
    easeInOutQuad: function (x, t, b, c, d) {
        if ((t/=d/2) < 1) return c/2*t*t + b;
        return -c/2 * ((--t)*(t-2) - 1) + b;
    },
    easeInCubic: function (x, t, b, c, d) {
        return c*(t/=d)*t*t + b;
    },
    easeOutCubic: function (x, t, b, c, d) {
        return c*((t=t/d-1)*t*t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t/=d/2) < 1) return c/2*t*t*t + b;
        return c/2*((t-=2)*t*t + 2) + b;
    },
    easeInQuart: function (x, t, b, c, d) {
        return c*(t/=d)*t*t*t + b;
    },
    easeOutQuart: function (x, t, b, c, d) {
        return -c * ((t=t/d-1)*t*t*t - 1) + b;
    },
    easeInOutQuart: function (x, t, b, c, d) {
        if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
        return -c/2 * ((t-=2)*t*t*t - 2) + b;
    },
    easeInQuint: function (x, t, b, c, d) {
        return c*(t/=d)*t*t*t*t + b;
    },
    easeOutQuint: function (x, t, b, c, d) {
        return c*((t=t/d-1)*t*t*t*t + 1) + b;
    },
    easeInOutQuint: function (x, t, b, c, d) {
        if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
        return c/2*((t-=2)*t*t*t*t + 2) + b;
    },
    easeInSine: function (x, t, b, c, d) {
        return -c * Math.cos(t/d * (Math.PI/2)) + c + b;
    },
    easeOutSine: function (x, t, b, c, d) {
        return c * Math.sin(t/d * (Math.PI/2)) + b;
    },
    easeInOutSine: function (x, t, b, c, d) {
        return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;
    },
    easeInExpo: function (x, t, b, c, d) {
        return (t==0) ? b : c * Math.pow(2, 10 * (t/d - 1)) + b;
    },
    easeOutExpo: function (x, t, b, c, d) {
        return (t==d) ? b+c : c * (-Math.pow(2, -10 * t/d) + 1) + b;
    },
    easeInOutExpo: function (x, t, b, c, d) {
        if (t==0) return b;
        if (t==d) return b+c;
        if ((t/=d/2) < 1) return c/2 * Math.pow(2, 10 * (t - 1)) + b;
        return c/2 * (-Math.pow(2, -10 * --t) + 2) + b;
    },
             * */
            public static void Update(int elapsedMS)
            {
                DateTime now = DateTime.Now;
                double timeElapsed = elapsedMS / 1000.0;

                foreach (var item in animatingControls.ToList())
                {
                    if (now >= item.TimeStart)
                    {
                        item.CurrentTime = now;
                        Point point = item.End;
                        if (item.TimeElapsed < item.TotalAnimTime)
                        {
                            
                            var progress = GetProgress(item.Routine, item.Progress, item.TotalAnimTime);
                            point = GetUpdatedPoint(progress, item.Start, item.End);
                        }
                        else
                        {
                            item.Completed = true;
                        }
                        
                        if (item.Type == ControlAnimatorType.Height)
                        {
                            item.Control.Height = point.Y;
                        }

                        item.Control.Parent.Invalidate();

                        if (item.Completed)
                        {
                            animatingControls.Remove(item);
                        }
                    }
                }
            }
        }
        
        
        private void trackEditorG6_Enter(object sender, EventArgs e)
        {
           
        }

        private void trackEditorG6_Leave(object sender, EventArgs e)
        {
            
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            ControlAnimator.Update(animationTimer.Interval);
        }

        
        private void trackEditorG5_Enter(object sender, EventArgs e)
        {
            
        }

        private void trackEditorG5_Leave(object sender, EventArgs e)
        {
          
        }

        private void trackEditorG6_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void trackEditorG6_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void trackEditorG5_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void buttonMaximizeG6_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(panelProEditor, panelProEditor.Height+20);
        }

        private void buttonMinimizeG6_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(panelProEditor, (panelProEditor.Height-20).Max(60));
        }

        private void buttonMaximizeG5_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(
                panel5ButtonEditor, (panel5ButtonEditor.Height+20));
        }
        

        private void buttonMinimizeG5_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(
                panel5ButtonEditor, (panel5ButtonEditor.Height - 20).Max(60));
        }

        private void buttonFindMP3Offset_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBoxAutoGenBassEasy_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxAutoGenBassMedium_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBoxAutoGenBassHard_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBoxAutoGenGuitarEasy_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxAutoGenGuitarMedium_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxAutoGenGuitarHard_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBoxSongLibIsComplete_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBoxSongLibIsFinalized_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxSongLibHasGuitar_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBoxSongLibHasBass_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBoxSongLibCopyGuitar_CheckedChanged(object sender, EventArgs e)
        {
            
        }


        private void checkBoxEnableMidiPlayback_CheckedChanged(object sender, EventArgs e)
        {
            
            enableMidiPlayback(checkBoxEnableMidiPlayback.Checked);
        }

        private void textBoxSongPropertiesMP3StartOffset_Leave(object sender, EventArgs e)
        {

        }

        private void textBoxSongPropertiesMP3StartOffset_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxSongPropertiesMP3Location_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void midiTrackEditorPro_TrackClicked(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            EditorPro.SetTrack6(sequence, track, difficulty);
        }

        private void midiTrackEditorG5_TrackClicked(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            EditorG5.SetTrack5(sequence, track, difficulty);
        }

        private void buttonBatchUtilExtractXMLBrowse_Click(object sender, EventArgs e)
        {
            ShowBatchUtilExtractXMLSelectFolder();
        }

        private string ShowBatchUtilExtractXMLSelectFolder()
        {
            var sel = ShowSelectFolderDlg("Select Output Folder", "", textBoxBatchUtilExtractXML.Text);
            if (!sel.IsEmpty())
            {
                textBoxBatchUtilExtractXML.Text = sel;
            }
            return textBoxBatchUtilExtractXML.Text;
        }

        private void buttonBatchUtilExtractXML_Click(object sender, EventArgs e)
        {
            
            if (!Directory.Exists(textBoxBatchUtilExtractXML.Text))
            {
                ShowBatchUtilExtractXMLSelectFolder();
            }
            var dir = textBoxBatchUtilExtractXML.Text;
            if (Directory.Exists(dir))
            {
                try
                {
                    foreach (var sng in SongList.GetBatchSongList(checkBoxMultiSelectionSongList.Checked))
                    {
                        if (OpenSongCacheItem(sng))
                        {
                            if (EditorPro.IsLoaded && checkBoxBatchUtilExtractXMLPro.Checked)
                            {

                                var fileName6 = Path.GetFileNameWithoutExtension(sng.G6FileName);
                                var fileName5 = Path.GetFileNameWithoutExtension(sng.G5FileName);
                                if (!fileName5.IsEmpty() && !fileName6.IsEmpty())
                                    if (fileName6 == fileName5)
                                        fileName6 += "_pro";

                                var file = Path.Combine(dir, fileName6 + ".xml");
                                try
                                {
                                    var doc = GenerateXml(sng);
                                    doc.Save(file);
                                }
                                catch { }
                            }
                            if(EditorG5.IsLoaded && checkBoxBatchUtilExtractXMLG5.Checked)
                            {
                                var fileName5 = Path.GetFileNameWithoutExtension(sng.G5FileName);
                                var file = Path.Combine(dir, fileName5 + ".xml");
                                try
                                {
                                    var doc = GenerateXmlG5(sng);
                                    doc.Save(file);
                                }
                                catch { }

                            }
                        }
                                
                    }
                }
                catch { }
            }
        
        }

        private XmlDocument GenerateXml(SongCacheItem sng)
        {

            var doc = new XmlDocument();

            var root = doc.AddNode("song");

            root.AddAttribute("songid", sng.DTASongID);
            root.AddAttribute("pue_version", "46");
            root.AddAttribute("name", sng.SongName);
            root.AddAttribute("description", sng.Description);
            root.AddAttribute("shortname", sng.DTASongShortName);
            

            root.AddAttribute("length", ProGuitarTrack.TotalSongTime.ToStringEx());

            var tunings = root.AddNode("tunings");
            {
                var tuning = tunings.AddNode("tuning");
                tuning.AddAttribute("isGuitarTuning", "true");
                tuning.AddAttribute("E", sng.GuitarTuning[0].ToInt().ToStringEx());
                tuning.AddAttribute("A", sng.GuitarTuning[1].ToInt().ToStringEx());
                tuning.AddAttribute("D", sng.GuitarTuning[2].ToInt().ToStringEx());
                tuning.AddAttribute("G", sng.GuitarTuning[3].ToInt().ToStringEx());
                tuning.AddAttribute("B", sng.GuitarTuning[4].ToInt().ToStringEx());
                tuning.AddAttribute("HighE", sng.GuitarTuning[5].ToInt().ToStringEx());
            }
            {
                var tuning = tunings.AddNode("tuning");
                tuning.AddAttribute("isBassTuning", "true");
                tuning.AddAttribute("E", sng.BassTuning[0].ToInt().ToStringEx());
                tuning.AddAttribute("A", sng.BassTuning[1].ToInt().ToStringEx());
                tuning.AddAttribute("D", sng.BassTuning[2].ToInt().ToStringEx());
                tuning.AddAttribute("G", sng.BassTuning[3].ToInt().ToStringEx());
                tuning.AddAttribute("B", sng.BassTuning[4].ToInt().ToStringEx());
                tuning.AddAttribute("HighE", sng.BassTuning[5].ToInt().ToStringEx());
            }

            var tracks = root.AddNode("tracks");
            foreach (var t in EditorPro.Sequence)
            {
                var track = tracks.AddNode("track");
                track.AddAttribute("name", t.Name);

                EditorPro.CurrentDifficulty = GuitarDifficulty.Expert;
                extractXMLSerializeChords6(track);

                EditorPro.CurrentDifficulty = GuitarDifficulty.Hard;
                extractXMLSerializeChords6(track);

                EditorPro.CurrentDifficulty = GuitarDifficulty.Medium;
                extractXMLSerializeChords6(track);

                EditorPro.CurrentDifficulty = GuitarDifficulty.Easy;
                extractXMLSerializeChords6(track);

                EditorPro.CurrentDifficulty = GuitarDifficulty.Expert;

                var modifiers = ProGuitarTrack.GetEvents().Where(x => x.IsProModifier()).Select(x => x.ToGuitarMessage(ProGuitarTrack));
                if(modifiers.Any())
                {
                    var mods = track.AddNode("modifiers");
                    foreach (var msg in modifiers)
                    {

                        var type = string.Empty;
                        if (msg.MidiEvent.IsArpeggioEvent())
                            type = "Arpeggio";
                        else if (msg.MidiEvent.IsBigRockEnding())
                            type = "BigRockEnding";
                        else if (msg.MidiEvent.IsMultiStringTremeloEvent())
                            type = "MultiStringTremelo";
                        else if (msg.MidiEvent.IsSingleStringTremeloEvent())
                            type = "SingleStringTremelo";
                        else if (msg.MidiEvent.IsSoloEvent())
                            type = "Solo";
                        else if (msg.MidiEvent.IsPowerupEvent())
                            type = "Powerup";

                        if (!type.IsEmpty())
                        {
                            var mod = mods.AddNode("modifier");
                            mod.AddAttribute("type", type);

                            mod.AddAttribute("startTime", msg.StartTime.ToStringEx());
                            mod.AddAttribute("startTick", msg.DownTick.ToStringEx());

                            mod.AddAttribute("endTime", msg.EndTime.ToStringEx());
                            mod.AddAttribute("endTick", msg.UpTick.ToStringEx());
                        }
                    }
                }
                if (t.IsTempo())
                {
                    var temposNode = track.AddNode("tempos");
                    foreach (var tempo in ProGuitarTrack.Messages.Tempos)
                    {
                        var tempoNode = temposNode.AddNode("tempo");
                        tempoNode.AddAttribute("startTime", tempo.StartTime.ToStringEx());
                        tempoNode.AddAttribute("startTick", tempo.AbsoluteTicks.ToStringEx());
                        tempoNode.AddAttribute("rawTempo", tempo.Tempo.ToStringEx());
                        
                        tempoNode.AddAttribute("secondsPerQuarterNote", ProGuitarTrack.SecondsPerQuarterNote(tempo.Tempo).ToStringEx());
                        tempoNode.AddAttribute("secondsPerTick", ProGuitarTrack.SecondsPerTick(tempo.Tempo).ToStringEx());
                        tempoNode.AddAttribute("secondsPerBar", ProGuitarTrack.SecondsPerBar(tempo.DownTick).ToStringEx());
                    }

                    
                    var timeSigsNode = track.AddNode("timeSignatures");
                    foreach (var timeSig in ProGuitarTrack.Messages.TimeSignatures)
                    {
                        var timeSigNode = timeSigsNode.AddNode("timeSignature");
                        timeSigNode.AddAttribute("startTime", timeSig.StartTime.ToStringEx());
                        timeSigNode.AddAttribute("startTick", timeSig.AbsoluteTicks.ToStringEx());
                        
                        timeSigNode.AddAttribute("numerator", timeSig.Numerator.ToInt().ToStringEx());
                        timeSigNode.AddAttribute("denominator", timeSig.Denominator.ToInt().ToStringEx());
                    }
                }

                var meta = ProGuitarTrack.GetEvents().Where(x => x.IsMetaEvent() && x.IsTempoTimesigEvent()==false);
                if(meta.Any())
                {
                    var metaEvents = track.AddNode("metaEvents");
                    foreach (var ev in meta)
                    {
                        var te = metaEvents.AddNode("metaEvent");
                        te.AddAttribute("startTime", ProGuitarTrack.TickToTime(ev.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("startTick", ev.AbsoluteTicks.ToStringEx());

                        te.AddAttribute("metaType", ev.MetaMessage.MetaType.ToString());
                        ev.MetaMessage.Text.IfNotEmpty(x => te.AddAttribute("text", x));
                    }
                }
                if (ProGuitarTrack.Messages.Trainers.Any())
                {
                    var trainerEvents = track.AddNode("trainers");
                    foreach (var ev in ProGuitarTrack.Messages.Trainers)
                    {
                        var te = trainerEvents.AddNode("trainer");

                        te.AddAttribute("trainerType", ev.TrainerType.ToString());
                        te.AddAttribute("index", ev.TrainerIndex.ToStringEx());

                        te.AddAttribute("startTime", ProGuitarTrack.TickToTime(ev.Start.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("startTick", ev.Start.AbsoluteTicks.ToStringEx());
                        te.AddAttribute("startText", ev.Start.Text);

                        te.AddAttribute("endTime", ProGuitarTrack.TickToTime(ev.End.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("endTick", ev.End.AbsoluteTicks.ToStringEx());
                        te.AddAttribute("endText", ev.End.Text);

                        te.AddAttribute("loopable", ev.Loopable.ToStringEx());
                        if (ev.Loopable)
                        {
                            te.AddAttribute("normTime", ProGuitarTrack.TickToTime(ev.Norm.AbsoluteTicks).ToStringEx());
                            te.AddAttribute("normTick", ev.Norm.AbsoluteTicks.ToStringEx());
                            te.AddAttribute("normText", ev.Norm.Text);
                        }

                    }
                }
            }

            return doc;
        }

        private XmlDocument GenerateXmlG5(SongCacheItem sng)
        {

            var doc = new XmlDocument();

            var root = doc.AddNode("song");

            root.AddAttribute("songid", sng.DTASongID);
            
            root.AddAttribute("pue_version", "46");
            root.AddAttribute("name", sng.SongName);
            root.AddAttribute("description", sng.Description);
            root.AddAttribute("shortname", sng.DTASongShortName);

            root.AddAttribute("length", EditorG5.GuitarTrack.TotalSongTime.ToStringEx());
            
            var tracks = root.AddNode("tracks");
            foreach (var t in EditorG5.Sequence)
            {
                var track = tracks.AddNode("track");
                track.AddAttribute("name", t.Name);

                EditorG5.SetTrack5(t, GuitarDifficulty.Expert);
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Hard;
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Medium;
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Easy;
                extractXMLSerializeChords5(track);

                EditorG5.SetTrack5(t, GuitarDifficulty.Expert);

                var modifiers = EditorG5.GuitarTrack.GetEvents().Where(x => x.IsChannelEvent() && (
                    Utility.BigRockEndingData1.Contains(x.Data1) || 
                    Utility.SoloData1 == x.Data1 || 
                    Utility.ExpertSoloData1_G5 == x.Data1 || 
                    Utility.PowerupData1 == x.Data1))
                    .Select(x => x.ToGuitarMessage(EditorG5.GuitarTrack));
                if (modifiers.Any())
                {
                    var mods = track.AddNode("modifiers");
                    foreach (var msg in modifiers)
                    {

                        var type = string.Empty;
                        if (msg.MidiEvent.IsBigRockEnding())
                            type = "BigRockEnding";
                        else if (msg.MidiEvent.IsSoloEvent())
                            type = "Solo";
                        else if (msg.MidiEvent.IsPowerupEvent())
                            type = "Powerup";

                        if (!type.IsEmpty())
                        {
                            var mod = mods.AddNode("modifier");
                            mod.AddAttribute("type", type);
                            
                            mod.AddAttribute("startTime", msg.StartTime.ToStringEx());
                            mod.AddAttribute("startTick", msg.DownTick.ToStringEx());

                            mod.AddAttribute("endTime", msg.EndTime.ToStringEx());
                            mod.AddAttribute("endTick", msg.UpTick.ToStringEx());
                        }
                    }
                }
                if (t.IsTempo())
                {
                    var temposNode = track.AddNode("tempos");
                    foreach (var tempo in EditorG5.GuitarTrack.Messages.Tempos)
                    {
                        var tempoNode = temposNode.AddNode("tempo");
                        tempoNode.AddAttribute("startTime", tempo.StartTime.ToStringEx());
                        tempoNode.AddAttribute("startTick", tempo.AbsoluteTicks.ToStringEx());
                        tempoNode.AddAttribute("rawTempo", tempo.Tempo.ToStringEx());
                        
                        tempoNode.AddAttribute("secondsPerQuarterNote", EditorG5.GuitarTrack.SecondsPerQuarterNote(tempo.Tempo).ToStringEx());
                        tempoNode.AddAttribute("secondsPerTick", EditorG5.GuitarTrack.SecondsPerTick(tempo.Tempo).ToStringEx());
                        tempoNode.AddAttribute("secondsPerBar", EditorG5.GuitarTrack.SecondsPerBar(tempo.DownTick).ToStringEx());
                        
                    }
                }
                if (t.IsTempo())
                {
                    var timeSigsNode = track.AddNode("timeSignatures");
                    foreach (var timeSig in EditorG5.GuitarTrack.Messages.TimeSignatures)
                    {
                        var timeSigNode = timeSigsNode.AddNode("timeSignature");
                        timeSigNode.AddAttribute("startTime", timeSig.StartTime.ToStringEx());
                        timeSigNode.AddAttribute("startTick", timeSig.AbsoluteTicks.ToStringEx());

                        timeSigNode.AddAttribute("numerator", timeSig.Numerator.ToInt().ToStringEx());
                        timeSigNode.AddAttribute("denominator", timeSig.Denominator.ToInt().ToStringEx());
                        
                    }
                }
                var meta = EditorG5.GuitarTrack.GetEvents().Where(x => x.IsMetaEvent() && !x.IsTempoTimesigEvent());
                if (meta.Any())
                {
                    var metaEvents = track.AddNode("metaEvents");
                    foreach (var ev in meta)
                    {
                        var te = metaEvents.AddNode("metaEvent");
                        te.AddAttribute("startTime", EditorG5.GuitarTrack.TickToTime(ev.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("startTick", ev.AbsoluteTicks.ToStringEx());

                        te.AddAttribute("metaType", ev.MetaMessage.MetaType.ToString());
                        ev.MetaMessage.Text.IfNotEmpty(x => te.AddAttribute("text", x));
                    }
                }
            }

            return doc;
        }
        private void extractXMLSerializeChords6(XmlNode root)
        {
            if (!ProGuitarTrack.Messages.Chords.Any())
                return;

            var chords = root.AddNode("chords", false);
            foreach (var ev in ProGuitarTrack.Messages.Chords)
            {
                var chord = chords.AddNode("chord");
                chord.AddAttribute("difficulty", ev.Difficulty.ToString());

                chord.AddAttribute("startTime", ev.StartTime.ToStringEx());
                chord.AddAttribute("endTime", ev.EndTime.ToStringEx());
                chord.AddAttribute("startTick", ev.DownTick.ToStringEx());
                chord.AddAttribute("endTick", ev.UpTick.ToStringEx());
                if (ev.IsXNote)
                {
                    chord.AddAttribute("isMute", ev.IsXNote.ToStringEx());
                }
                if (ev.IsTap)
                {
                    chord.AddAttribute("isTap", ev.IsTap.ToStringEx());
                }
                if (ev.IsSlide)
                {
                    chord.AddAttribute("isSlide", ev.IsSlide.ToStringEx());
                }
                if (ev.IsSlideReversed)
                {
                    chord.AddAttribute("isSlideReversed", ev.IsSlideReversed.ToStringEx());
                }
                if (ev.IsHammeron)
                {
                    chord.AddAttribute("isHammeron", ev.IsHammeron.ToStringEx());
                }

                if (ev.StrumMode.HasFlag(ChordStrum.High))
                {
                    chord.AddAttribute("strumHigh", ev.StrumMode.HasFlag(ChordStrum.High).ToStringEx());
                }
                if (ev.StrumMode.HasFlag(ChordStrum.Mid))
                {
                    chord.AddAttribute("strumMid", ev.StrumMode.HasFlag(ChordStrum.Mid).ToStringEx());
                }
                if (ev.StrumMode.HasFlag(ChordStrum.Low))
                {
                    chord.AddAttribute("strumLow", ev.StrumMode.HasFlag(ChordStrum.Low).ToStringEx());
                }
                
                var notes = chord.AddNode("notes");
                foreach (var n in ev.Notes)
                {
                    var note = notes.AddNode("note");

                    note.AddAttribute("fret", n.NoteFretDown.ToStringEx());
                    note.AddAttribute("string", n.NoteString.ToStringEx());
                    if (n.IsTapNote)
                    {
                        note.AddAttribute("isTapNote", n.IsTapNote.ToStringEx());
                    }
                    if (n.IsArpeggioNote)
                    {
                        note.AddAttribute("isArpeggioNote", n.IsArpeggioNote.ToStringEx());
                    }
                    
                }
            }
        }

        private void extractXMLSerializeChords5(XmlNode root)
        {
            if (EditorG5.Messages.Chords.Any())
            {
                var chords = root.AddNode("chords", false);
                foreach (var ev in EditorG5.Messages.Chords)
                {
                    var chord = chords.AddNode("chord");
                    chord.AddAttribute("difficulty", ev.Difficulty.ToString());
                    chord.AddAttribute("startTime", ev.StartTime.ToStringEx());
                    chord.AddAttribute("endTime", ev.EndTime.ToStringEx());
                    chord.AddAttribute("startTick", ev.DownTick.ToStringEx());
                    chord.AddAttribute("endTick", ev.UpTick.ToStringEx());

                    var notes = chord.AddNode("notes");
                    foreach (var n in ev.Notes)
                    {
                        if (n != null)
                        {
                            var note = notes.AddNode("note");

                            note.AddAttribute("string", n.NoteString.ToStringEx());
                        }
                    }
                }
            }
        }

        private void groupBox25_Enter(object sender, EventArgs e)
        {

        }

        private void buttonSongPropertiesViewMp3Preview_Click(object sender, EventArgs e)
        {
            PUEExtensions.TryExec(delegate()
            {
                var path = textBoxSongPropertiesMP3Location.Text;
                if (path.FileExists())
                {
                    PEWaveViewer.ShowWave(path);
                }
            });
        }

        private void zipFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenFileDlg("Select Zip File", DefaultMidiFileLocationPro, "").IfNotEmpty(x =>
                ImportFile(x));
        }

        private void cONPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenFileDlg("Select Con File", DefaultMidiFileLocationPro, "").IfNotEmpty(x =>
                ImportFile(x));
        }

    }
}
