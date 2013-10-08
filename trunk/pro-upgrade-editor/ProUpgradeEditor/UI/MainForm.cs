using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using EditorResources.Components;
using ProUpgradeEditor.Common;

using Sanford.Multimedia.Midi;
using X360;
using X360.FATX;
using X360.Other;
using XPackage;
using ZipLib.SharpZipLib.Core;
using ZipLib.SharpZipLib.Zip;


namespace ProUpgradeEditor.UI
{
    public partial class MainForm : Form
    {

        public List<TrackEditor> Editors;

        public TrackEditor EditorPro { get { return trackEditorG6; } }
        public TrackEditor EditorG5 { get { return trackEditorG5; } }


        public MainForm()
        {

            InitializeComponent();

            if (!DesignMode)
            {
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

                EditorPro.OnStatusIdle += new EventHandler(EditorPro_OnStatusIdle);

                EditorPro.OnZoom += new TrackEditor.ZoomHandler(EditorPro_OnZoom);

            }
        }


        void EditorPro_OnZoom(TrackEditor editor, int delta)
        {
            if (delta > 0)
            {
                while (delta > 0)
                {
                    ZoomIn();
                    delta--;
                    EditorPro.Invalidate();
                    Application.DoEvents();
                }
            }
            else if (delta < 0)
            {
                while (delta < 0)
                {
                    ZoomOut();
                    delta++;
                    EditorPro.Invalidate();
                    Application.DoEvents();
                }
            }

        }

        void EditorPro_OnStatusIdle(object sender, EventArgs e)
        {
            RefreshLists();
        }

        public void RefreshLists()
        {
            RefreshModifierListBoxes();
            RefreshTrainers();
            RefreshTextEvents();
            UpdateControlsForDifficulty(EditorPro.CurrentDifficulty);
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
                        ImportFile(pk, packageFiles.Where(x => x != pk));
                    }
                }
            }
            catch { }
        }

        public void ImportFile(string fileName, IEnumerable<string> allFiles)
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

                        if (ExtractZipFile(fileName, outputFolder, null, false))
                        {
                            var midiFiles = outputFolder.GetFilesInFolder(true, "*.mid|*.midi").ToList();
                            var dtaFiles = outputFolder.GetFilesInFolder(true, "*.dta").ToList();
                            var mp3Files = outputFolder.GetFilesInFolder(true, "*.mp3|*.mogg|*.ogg").ToList();

                            importDroppedMidi(dtaFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                midiFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                mp3Files.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes()))).ForEach(x =>
                                {
                                    if (x != null)
                                    {
                                        if (x.G6ConFile.IsEmpty())
                                        {
                                            x.G6ConFile = fileName;
                                        }
                                        if (x.G5FileName.IsEmpty())
                                        {
                                            CreateSongG5FromPro(x);
                                        }
                                    }
                                });
                        }
                    }
                    else if (fileName.EndsWithEx(".mp3") || fileName.EndsWithEx("ogg"))
                    {
                        var dtaFiles = GetDroppedDTAFiles(fileName, allFiles);
                        if (dtaFiles.Any())
                        {
                            importDroppedMP3(fileName, dtaFiles.Select(x => DTAFile.FromBytes(x.ReadFileBytes())));
                        }
                    }
                    else if (fileName.EndsWithEx(".mid") || fileName.EndsWithEx(".midi"))
                    {
                        var midiFiles = fileName.MakeEnumerable();
                        var dtaFiles = new List<string>();
                        var mp3Files = new List<string>();

                        dtaFiles = GetDroppedDTAFiles(fileName, allFiles);
                        if (dtaFiles.Any())
                        {
                            importDroppedMidi(dtaFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                midiFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                mp3Files.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes()))).ForEach(x =>
                                {
                                    if (x != null)
                                    {
                                        if (x.G6ConFile.IsEmpty())
                                        {
                                            x.G6ConFile = fileName;
                                        }
                                        if (x.G5FileName.IsEmpty())
                                        {
                                            CreateSongG5FromPro(x);
                                        }
                                    }
                                });
                        }
                        else
                        {
                            MessageBox.Show("Could not find dta file");
                        }
                    }
                    else
                    {
                        var package = Package.Load(fileName);

                        if (package != null)
                        {
                            var dtaFiles = GetDTAFiles(package);
                            var midiFiles = GetMidiFiles(package);

                            bool existingSong = false;
                            var songID = string.Empty;
                            var loadedDTAFiles = new List<DTAFile>();
                            SongCacheItem song = null;
                            CloseSelectedSong();

                            if (dtaFiles.Any() && midiFiles.Any())
                            {
                                var ldf = dtaFiles.Select(x => LoadDTAFile(x.Data)).Where(x => x != null);

                                if (ldf.Any())
                                {
                                    loadedDTAFiles.AddRange(ldf);

                                    var songIDs = ldf.SelectMany(x => x.GetSongIDs().Select(f => f.Value));

                                    song = SongList.FirstOrDefault(x => songIDs.Any(sid => x.DTASongID.EqualsEx(sid)));
                                    if (song != null)
                                    {
                                        songID = song.DTASongID;

                                        existingSong = songID.IsNotEmpty();
                                    }
                                }
                            }

                            if (!existingSong)
                            {
                                var outputFolder = proFolder.PathCombine(fileName.GetFileNameWithoutExtension()).AppendSlashIfMissing();
                                outputFolder.CreateFolderIfNotExists();

                                try
                                {
                                    var extractedDTAFiles = ExtractDTAFiles(package, outputFolder);
                                    var extractedMIDIFiles = ExtractMidiFiles(package, outputFolder);
                                    var extractedAudioFiles = ExtractAudioFiles(package, outputFolder);

                                    importDroppedMidi(extractedDTAFiles.
                                        Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                        extractedMIDIFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes())),
                                        extractedAudioFiles.Select(x => new KeyValueObject<string, byte[]>(x, x.ReadFileBytes()))).ToList().ForEach(x =>
                                        {
                                            if (x != null)
                                            {
                                                if (x.G6ConFile.IsEmpty())
                                                {
                                                    x.G6ConFile = fileName;
                                                }
                                                if (x.G5FileName.IsEmpty())
                                                {
                                                    CreateSongG5FromPro(x);
                                                }
                                            }
                                        });
                                }
                                catch { }
                            }
                            else
                            {
                                if (song.G6ConFile.IsEmpty())
                                {
                                    song.G6ConFile = fileName;
                                }

                                ImportIntoExistingSong(song,
                                    midiFiles.Select(x =>
                                    {
                                        return new KeyValueObject<string, byte[]>(x.Name, x.Data);
                                    }), loadedDTAFiles).IfNotNull(x =>
                                    {
                                        CreateSongG5FromPro(x);
                                    });
                            }
                        }

                    }
                }
            }
            catch { }
        }

        private bool CreateSongG5FromPro(SongCacheItem item)
        {
            var ret = false;
            if (item != null && item.G5FileName.IsEmpty() && item.G6FileName.FileExists())
            {
                if (OpenSongCacheItem(item))
                {
                    item.G5FileName = item.G6FileName.GetFolderName().PathCombine(SelectedSong.G6FileName.GetFileNameWithoutExtension() + "_g5.mid");
                    var tt = EditorPro.GuitarTrack.GetTempoTrack();
                    if (tt != null)
                    {
                        using (var seq = new Sequence(FileType.Guitar5, EditorPro.GuitarTrack.SequenceDivision.ToInt()))
                        {
                            seq.AddTempo(tt);

                            foreach (var track in EditorPro.Tracks.Where(x => x.Name.IsProTrackName17()))
                            {
                                var g5 = track.ConvertToG5();
                                g5.Name = track.Name.IsGuitarTrackName() ? GuitarTrack.GuitarTrackName5 : GuitarTrack.BassTrackName5;
                                seq.Add(g5);
                            }

                            TryWriteFile(item.G5FileName, seq.Save().GetBytes(true));
                        }

                    }

                    ret = OpenSongCacheItem(item);
                }
            }
            return ret;
        }

        public List<string> GetDroppedDTAFiles(string fileName, IEnumerable<string> allFiles)
        {
            List<string> dtaFiles = new List<string>();
            if (allFiles.Any(x => x.GetFileName().EqualsEx("songs.dta")))
            {
                dtaFiles.AddRange(allFiles.Where(x => x.GetFileName().EqualsEx("songs.dta")));
            }
            else
            {
                var dta = fileName.GetFolderName().GetFilesInFolder(true, "songs.dta*");
                if (dta.Any())
                {
                    dtaFiles.AddRange(dta);
                }
                else
                {
                    var dtaName = fileName.GetFolderName().PathCombine("upgrades.dta");
                    if (!dtaName.FileExists())
                    {
                        dtaName = fileName.GetParentFolder().PathCombine("songs.dta");
                        if (!dtaName.FileExists())
                        {
                            dtaName = fileName.GetParentFolder().PathCombine("songs.dta");
                            if (!dtaName.FileExists()) { dtaName = string.Empty; }
                        }
                    }
                    if (!dtaName.IsEmpty())
                    {
                        dtaFiles.Add(dtaName);
                    }
                }
            }
            return dtaFiles;
        }

        private IEnumerable<SongCacheItem> importDroppedMidi(IEnumerable<KeyValueObject<string, byte[]>> dtaFiles,
            IEnumerable<KeyValueObject<string, byte[]>> midiFiles,
            IEnumerable<KeyValueObject<string, byte[]>> mp3Files)
        {
            var ret = new List<SongCacheItem>();
            if (dtaFiles.Any() && midiFiles.Any())
            {
                var midiFilesG5 = midiFiles.Where(x => x.Key.GetMidiFileType() == FileType.Guitar5).ToList();

                var midiFilesPro = midiFiles.Where(x => !midiFilesG5.Contains(x)).ToList()
                    .Where(x => x.Key.GetMidiFileType() == FileType.Pro).ToList();

                if (!midiFilesG5.Any())
                {
                    if (midiFilesPro.Any() && dtaFiles.Any())
                    {
                        ret.AddRange(ImportProMidiOnly(dtaFiles, midiFilesPro));
                    }
                }
                else
                {
                    foreach (var midiFileNameG5 in midiFilesG5)
                    {
                        ImportNewSong(mp3Files, midiFilesPro, midiFileNameG5).IfNotNull(x =>
                        {
                            ret.Add(x);
                        });
                    }
                }
            }
            return ret;
        }

        private void importDroppedMP3(string fileName,
            IEnumerable<DTAFile> dtaFiles)
        {

            if (!dtaFiles.Where(x => x.GetSongIDs().Any()).Any() && fileName.FileExists())
            {
                var songNames = dtaFiles.Select(x => x.First().Name).ToList();

                var saved = GetSavedSongNameIDList().Where(x => songNames.Any(d => d.EqualsEx(x.Key)));
                if (saved.Any())
                {
                    var songs = SongList.Where(x => saved.Any(s => s.Value.EqualsEx(x.DTASongID)));
                    if (songs.Count() == 1)
                    {
                        ImportSongMP3(fileName, songs.First());
                    }
                }
            }
            else if (dtaFiles.Where(x => x.GetSongIDs().Any()).Any() && fileName.FileExists())
            {
                dtaFiles = dtaFiles.Where(x => x.GetSongIDs().Any());
                var songID = string.Empty;
                try
                {
                    var songNames = dtaFiles.SelectMany(x => x.GetSongIDs()).Select(x => x.SongShortName);
                    songNames = songNames.Where(x => fileName.GetFileNameWithoutExtension().StartsWithEx(x));
                    if (songNames.Any())
                    {
                        songID = dtaFiles.SelectMany(x => x.GetSongIDs()).ToList().Where(x =>
                            fileName.GetFileNameWithoutExtension().StartsWithEx(x.SongShortName)).Select(x => x.Value).FirstOrDefault();
                    }
                    else
                    {
                        var dtaIDs = dtaFiles.SelectMany(x => x.GetSongIDs()).Select(x => x.Value).ToList();
                        var songs = SongList.Where(s => dtaIDs.Any(x => s.DTASongID.EqualsEx(x)));
                        if (songs.Count() == 1)
                        {
                            var song = songs.First();
                            ImportSongMP3(fileName, song);
                        }
                    }

                }
                catch { }

            }
        }

        private void ImportSongMP3(string fileName, SongCacheItem song)
        {
            bool copied = false;
            song.SongMP3Location.IfNotEmpty(x => { copied = TryCopyFile(fileName, song.SongMP3Location); });
            if (!copied)
            {
                var midiFile = song.G6FileName.GetIfEmpty(song.G5FileName);
                if (midiFile.IsNotEmpty())
                {
                    var destFile = midiFile.GetFolderName().PathCombine(fileName.GetFileName());
                    if (TryCopyFile(fileName, destFile))
                    {
                        song.SongMP3Location = destFile;
                        copied = true;
                    }
                }
            }
            if (copied)
            {

                if (song.IsSelected)
                {
                    CloseSelectedSong();
                    OpenSongCacheItem(song);
                }
            }
        }
        public IEnumerable<StringPair> GetSavedSongNameIDList()
        {
            var lines = ASCIIEncoding.ASCII.GetString(Package.GetSongIDList()).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => x.IsNotEmpty());
            var splitLines = lines.Where(x => x.Trim().IsNotEmpty()).Select(x => x.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToArray());
            return splitLines.Where(x => x.Count() == 2).Select(line => new StringPair(line[0], line[1])).ToList();
        }



        private IEnumerable<SongCacheItem> ImportProMidiOnly(IEnumerable<KeyValueObject<string, byte[]>> dtaFiles, IEnumerable<KeyValueObject<string, byte[]>> midiFilesPro)
        {
            var ret = new List<SongCacheItem>();
            foreach (var dtafile in dtaFiles)
            {
                var dtaData = LoadDTAFile(dtafile.Value);
                if (dtaData.Any())
                {
                    var songIDs = new List<StringPair>();
                    if (dtaData.GetSongIDs().Count() == 0)
                    {
                        var saved = GetSavedSongNameIDList().Where(x => midiFilesPro.Any(m => m.Key.GetFileNameWithoutExtension().StartsWithEx(x.Key)));
                        if (saved.Any())
                        {
                            songIDs.AddRange(saved.Select(x => x));
                        }
                    }
                    else
                    {
                        songIDs.AddRange(dtaData.GetSongIDs().Select(x => new StringPair(x.SongShortName, x.Value)).Where(x => x.Key.IsNotEmpty() && x.Value.IsNotEmpty()));
                    }

                    if (!SongList.Any(x => songIDs.Any(sid => sid.Value.EqualsEx(x.DTASongID))))
                    {
                        CloseSelectedSong();
                        if (OpenEditorFile(midiFilesPro.FirstOrDefault().Key, null, EditorFileType.Midi6))
                        {
                            if (SelectedSong == null)
                            {
                                if (CreateSongFromOpenMidi())
                                {
                                    ret.Add(SelectedSong);
                                }
                            }
                        }

                        break;
                    }
                    else
                    {
                        var songs = SongList.Where(x => songIDs.Any(sid => sid.Value.EqualsEx(x.DTASongID)));
                        if (songs.Count() == 1)
                        {
                            var sng = ImportIntoExistingSong(songs.FirstOrDefault(), midiFilesPro, dtaData.MakeEnumerable());
                            if (sng.IsNotNull())
                            {
                                ret.Add(sng);
                            }
                            break;
                        }
                        else
                        {
                            songs = songs.Where(s => midiFilesPro.Any(x => x.Key.GetFileNameWithoutExtension().StartsWithEx(s.DTASongShortName)));
                            if (songs.Count() == 1)
                            {
                                var sng = ImportIntoExistingSong(songs.FirstOrDefault(), midiFilesPro, dtaData.MakeEnumerable());
                                if (sng.IsNotNull())
                                {
                                    ret.Add(sng);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private SongCacheItem ImportNewSong(IEnumerable<KeyValueObject<string, byte[]>> mp3Files,
            IEnumerable<KeyValueObject<string, byte[]>> midiFilesPro,
            KeyValueObject<string, byte[]> midiFileG5)
        {
            SongCacheItem ret = null;
            try
            {
                CloseSelectedSong();
                var midiFileNameG5 = (midiFileG5 == null ? "" : midiFileG5.Key);

                bool isMidi = (midiFileNameG5.IsNotEmpty() && (midiFileNameG5.EndsWithEx(".mid") || midiFileNameG5.EndsWithEx(".midi")));

                if (OpenEditorFile(midiFileNameG5, midiFileNameG5.ReadFileBytes(), isMidi ? EditorFileType.AnyMidi : EditorFileType.Any))
                {
                    var fn = midiFileNameG5.GetFileNameWithoutExtension();
                    var pro = midiFilesPro.Where(x => x.Key.GetFileNameWithoutExtension().StartsWithEx(fn));
                    if (!pro.Any())
                    {
                        pro = midiFilesPro.Where(x => x.Key.GetFolderName().EqualsEx(midiFileNameG5.GetFolderName()));
                    }
                    if (!pro.Any())
                    {
                        var seq5 = midiFileNameG5.LoadSequenceFile();
                        var tempo5 = seq5.GetTempoTrack();

                        if (tempo5 != null)
                        {
                            var t5Event = tempo5.Tempo.FirstOrDefault();

                            var cb5 = new TempoChangeBuilder((MetaMessage)t5Event.Clone());
                            MidiEvent tempo6 = null;
                            pro =
                                midiFilesPro.Where(x => (tempo6 = x.Value.LoadSequence().GetTempoTrack().Tempo.FirstOrDefault()) != null &&
                                (tempo6.AbsoluteTicks == t5Event.AbsoluteTicks &&
                                (new TempoChangeBuilder((MetaMessage)tempo6.Clone()).Tempo == cb5.Tempo))).ToList();
                        }
                    }
                    if (pro.Any())
                    {
                        pro.OrderByDescending(x =>
                            x.Key.GetFileModifiedTime()).FirstOrDefault().
                            IfObjectNotNull(x =>
                            {
                                var seq = x.Value.LoadSequence();
                                EditorPro.LoadedFileName = x.Key;
                                EditorPro.SetTrack6(seq, seq.GetPrimaryTrack());
                            });
                    }

                    if (CreateSongFromOpenMidi())
                    {
                        if (mp3Files.Any())
                        {
                            textBoxSongPropertiesMP3Location.Text = mp3Files.First().Key;
                            textBoxSongPropertiesMP3StartOffset.Text = "0";

                            if (SelectedSong != null)
                            {
                                UpdateSongCacheItem(SelectedSong);
                                if (OpenSongCacheItem(SelectedSong))
                                {
                                    ret = SelectedSong;

                                    if (ret.G5FileName.IsEmpty())
                                    {
                                        if (CreateSongG5FromPro(ret))
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        private SongCacheItem ImportIntoExistingSong(SongCacheItem song,
            IEnumerable<KeyValueObject<string, byte[]>> midiFilesPro,
            IEnumerable<DTAFile> dtaFiles)
        {
            SongCacheItem ret = null;
            var songIDs = dtaFiles.SelectMany(x => x.GetSongIDs().Select(y => y.Value)).Where(x => x != null).ToList();
            var songNames = dtaFiles.SelectMany(x => x.Select(y => y.Name)).Where(x => x != null).ToList();

            if (song != null)
            {
                midiFilesPro.Where(x => x != null).ToList().ForEach(midiPro =>
                {
                    CloseSelectedSong();

                    if ((song.G6FileName.IsEmpty() || !song.G6FileName.FileExists()) && song.G5FileName.FileExists())
                    {
                        var newFileName = song.G5FileName.GetFolderName().PathCombine(midiPro.Key);

                        if (OpenSongCacheItem(song))
                        {
                            if (!TryWriteFile(newFileName, midiPro.Value))
                            {
                                MessageBox.Show("Unable to write file: " + newFileName);
                            }
                            else
                            {
                                song.G6FileName = newFileName;
                                textBoxSongLibProMidiFileName.Text = song.G6FileName;
                                ret = song;
                            }
                        }
                    }
                    else if (song.G6FileName.FileExists())
                    {
                        if (ConfirmOverwrite(FileType.Pro))
                        {
                            if (OpenSongCacheItem(song))
                            {

                                if (!TryWriteFile(song.G6FileName, midiPro.Value))
                                {
                                    MessageBox.Show("Unable to write file: " + song.G6FileName);
                                }
                                else
                                {
                                    ret = song;
                                }
                            }
                        }

                        if (song.G6FileName.FileExists())
                        {
                            if (OpenSongCacheItem(song))
                            {
                                if (song.G5FileName.IsEmpty() || ConfirmOverwrite(FileType.Guitar5))
                                {
                                    if (CreateSongG5FromPro(song))
                                    {
                                        ret = song;
                                    }
                                }
                            }
                        }

                    }

                });
            }
            return ret;
        }

        public bool ConfirmOverwrite(FileType type)
        {
            return MessageBox.Show("Over-write existing " + type.ToString() + " file?", "Confirm Over-write", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes;
        }


        public List<string> ExtractMidiFiles(Package f, string outputDir)
        {
            var ret = new List<string>();
            try
            {

                foreach (var midi in GetMidiFiles(f))
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


        public List<string> ExtractAudioFiles(Package f, string outputDir)
        {
            var ret = new List<string>();
            try
            {

                foreach (var file in GetAudioFiles(f))
                {
                    try
                    {
                        var newFile = outputDir.PathCombine(file.Name);
                        if (!newFile.FileExists())
                        {
                            newFile.WriteFileBytes(file.Data);
                        }
                        ret.Add(newFile);
                    }
                    catch { }
                }

            }
            catch { }
            return ret;
        }


        public static IEnumerable<PackageFile> GetMidiFiles(Package f)
        {
            var ret = new List<PackageFile>();
            try
            {
                ret.AddRange(f.GetFilesByExtension(".mid|.midi"));
            }
            catch { }
            return ret;

        }


        public static IEnumerable<PackageFile> GetAudioFiles(Package f)
        {
            var ret = new List<PackageFile>();
            try
            {
                ret.AddRange(f.GetFilesByExtension(".mp3|.mogg|.ogg"));
            }
            catch { }
            return ret;

        }
        public static IEnumerable<PackageFile> GetDTAFiles(Package f)
        {
            var ret = new List<PackageFile>();
            try
            {
                ret.AddRange(f.GetFilesByExtension(".dta"));
            }
            catch { }
            return ret;

        }
        public static List<string> ExtractDTAFiles(Package f, string outputDir)
        {
            var ret = new List<string>();
            try
            {

                try
                {
                    GetDTAFiles(f).ForEach(dta =>
                    {
                        var newFile = Path.Combine(outputDir, dta.Name);

                        if (!File.Exists(newFile))
                        {
                            File.WriteAllBytes(newFile, dta.Data);
                        }
                        ret.Add(newFile);
                    });
                }
                catch { }

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

                checkScrollToSelection.CheckedChanged += new EventHandler(checkScrollToSelection_CheckedChanged);
                EditorPro.OnSetChordToScreen += new TrackEditor.SetChordToScreenHandler(EditorPro_OnSetChordToScreen);

                EditorPro.OnAddChordHandler += new TrackEditor.AddChordHandler(EditorPro_OnAddChordHandler);
                listBoxUSBSongs.Columns[0].Width = listBoxUSBSongs.Width - 2;

                CheckLoadLastFile();

                ChordScaleBoxes.ToList().ForEach(scaleBox =>
                {
                    scaleBox.CheckedChanged += (o, ev) =>
                    {
                        if (o.IsNotNull())
                        {
                            var ocheck = ((CheckBox)o);
                            if (ocheck.Checked)
                            {
                                ChordScaleBoxes.Where(x => x != ocheck).ToList().ForEach(x => x.Checked = false);
                            }
                        }
                    };
                });
            }
        }

        IEnumerable<GuitarChord> EditorPro_OnAddChordHandler(TrackEditor editor, GuitarChord chord)
        {
            EditorPro_OnSetChordToScreen(editor, chord, true);
            return PlaceNote(SelectNextEnum.ForceKeepSelection);
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
            var result = OnTrackEditorProClick(editor, e);
            RefreshModifierListBoxes();
            if (result == true)
                EditorPro.SetSelectionStateIdle();
            return result;
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
            midiTrackEditorPro.SetTrack(null, GuitarDifficulty.Expert);
            midiTrackEditorPro.Refresh();

            RefreshTracks6();
            RefreshLists();

        }

        void trackEditorG5_OnClose(TrackEditor editor)
        {
            labelStatusIconEditor5.ImageKey = "music--exclamation.png";
            this.FileNameG5 = "";
            editor.SetHScrollMaximum(0);
            midiTrackEditorG5.SetTrack(null, GuitarDifficulty.Expert);
            midiTrackEditorG5.Refresh();

            RefreshTracks5();
            RefreshLists();
        }

        void trackEditorG5_OnLoadTrack(TrackEditor editor, Sequence seq, Track t)
        {
            labelStatusIconEditor5.ImageKey = "music.png";
            FileNameG5 = editor.LoadedFileName;
            editor.visibleSelectors.Clear();
            RefreshTracks5();

            this.toolStripFileName5.Text = editor.LoadedFileName.GetFileName();

            RefreshLists();
        }


        void trackEditorG6_OnLoadTrack(TrackEditor editor, Sequence seq, Track t)
        {
            if (!trackEditorG6.IsLoaded)
                return;

            labelStatusIconEditor6.ImageKey = "music.png";
            FileNamePro = editor.LoadedFileName;

            editor.visibleSelectors.Clear();

            RefreshTracks6();

            this.toolStripFileName6.Text = editor.LoadedFileName.GetFileName();

            RefreshLists();
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
            EditorPro.SetSelectionStateIdle();
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

        DateTime lastPlayHoldBoxKeyDown = DateTime.MinValue;
        void buttonPlayHoldBoxMidi_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (lastPlayHoldBoxKeyDown.IsNull())
                {
                    lastPlayHoldBoxKeyDown = DateTime.Now;
                    PlayHoldBoxMidi();
                }
                else
                {
                    if ((lastPlayHoldBoxKeyDown - DateTime.Now).TotalSeconds > 2)
                    {
                        lastPlayHoldBoxKeyDown = DateTime.MinValue;
                    }
                }
            }
        }
        void buttonPlayHoldBoxMidi_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                StopHoldBoxMidi();
                lastPlayHoldBoxKeyDown = DateTime.MinValue;
            }
        }
        private void buttonRebuildPackage_Click(object sender, EventArgs e)
        {
            RebuildSongPackage();
        }


        public DataPair<EditorTrackDifficulty> GetSelectedTrackDifficulties()
        {
            return new DataPair<EditorTrackDifficulty>(new EditorTrackDifficulty(midiTrackEditorPro), new EditorTrackDifficulty(midiTrackEditorG5));
        }
        public void RestoreTrackDifficulty(DataPair<EditorTrackDifficulty> diff)
        {
            if (diff != null)
            {
                if (diff.A != null && diff.A.SelectedTrackDifficulty != null && diff.A.SelectedTrackDifficulty.Track != null)
                {

                    foreach (var item in diff.A.Difficulties.Where(x =>
                        midiTrackEditorPro.TrackDifficulties.Any(y => y.Track == x.Track && y.Difficulty != x.Difficulty)))
                    {
                        EditorPro.SetTrack(item.Track, item.Difficulty);
                    }

                    EditorPro.SetTrack(diff.A.SelectedTrackDifficulty.Track, diff.A.SelectedTrackDifficulty.Difficulty);
                }
                if (diff.B != null && diff.B.SelectedTrackDifficulty != null && diff.B.SelectedTrackDifficulty.Track != null)
                {

                    foreach (var item in diff.B.Difficulties.Where(x =>
                        midiTrackEditorG5.TrackDifficulties.Any(y => y.Track == x.Track && y.Difficulty != x.Difficulty)))
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
        private void RebuildSongPackage()
        {
            ExecAndRestoreTrackDifficulty(delegate()
            {
                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);
                    try
                    {
                        listBoxSongLibrary.SelectedItem = SelectedSong;
                    }
                    catch { }
                    if (SelectedSong.IsDirty || ProGuitarTrack.Dirty)
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

                    var config = new GenDiffConfig(SelectedSong, true,
                        SelectedSong.CopyGuitarToBass, false, false, Utility.HandPositionGenerationEnabled);

                    if (!GenerateDifficulties(false, config))
                    {
                        MessageBox.Show("Failed generating difficulties");
                    }
                    else
                    {
                        SavePro();
                    }
                    if (!SaveProCONFile(SelectedSong, true, false))
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
            if (SelectedSong != null && SelectedSong.G6ConFile.IsNotEmpty())
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
                        if (!LoadPackageIntoTree(p, selPack, true))
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
                                    ret = AddNewSongToLibrary(true);
                                }

                            }
                        }
                        else
                        {
                            ret = AddNewSongToLibrary(true);
                        }
                        ReloadTracks();
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

        private void buttonExecuteBatchGuitarBassCopy_Click(object sender, EventArgs e)
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
                if (SelectedSong != null && SelectedSong.G6ConFile.FileExists())
                {
                    var p = Package.Load(SelectedSong.G6ConFile);
                    if (p != null)
                    {
                        LoadPackageIntoTree(p, SelectedSong.G6ConFile, true);
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

        bool ValidateDefaultDirectories(bool silent)
        {

            var ret = (checkUseDefaultFolders.Checked &&
                    (DefaultMidiFileLocationPro.IsEmpty() ||
                     DefaultMidiFileLocationG5.IsEmpty() ||
                     DefaultConFileLocation.IsEmpty()));
            if (ret && !silent)
            {
                MessageBox.Show("Configure default directories on the settings tab first", "Missing Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return !ret;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            try
            {
                if (!ValidateDefaultDirectories(false))
                    return;
                EditorPro.ClearSelection();
                CloseSelectedSong();

                if (ShowOpenMidi5())
                {
                    ReloadTracks();

                    if (!InitializeFrom5Tar())
                    {
                        MessageBox.Show("Unable to initialize pro from 5 button file");
                    }
                    else
                    {
                        ReloadTracks();

                        var fileName = FileNameG5.GetFolderName().PathCombine(
                            FileNameG5.GetFileNameWithoutExtension() + Utility.DefaultPROFileExtension);

                        if (!SaveProFile(fileName, false))
                        {
                            MessageBox.Show("Unable to save pro file: " + fileName);
                        }
                        else
                        {
                            ReloadTracks();

                            if (SelectedSong == null)
                            {
                                if (!AddNewSongToLibrary(false))
                                {
                                    MessageBox.Show("Could not add new song to library");
                                }
                            }
                            if (SelectedSong != null)
                            {
                                SelectedSong.G6FileName = fileName;

                                if (OpenSongCacheItem(SelectedSong))
                                {
                                    var config = new GenDiffConfig(SelectedSong, true, false, false, false, false);
                                    if (!GenerateDifficulties(false, config))
                                    {
                                        MessageBox.Show("Unable to generate difficulties for pro file");
                                    }
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenMidi5();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileNameG5.IsEmpty())
                {
                    ShowG5MidiSaveAs();
                }
                else
                {
                    FileNameG5.IfNotEmpty(x => EditorG5.SaveTrack(FileNameG5, CreateBackup));
                }
            }
            catch { }
        }

        public bool CreateBackup { get { return settings.GetValueBool("Save pro backup on save", false); } }

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
            if (SelectedSong != null)
            {
                SaveSelectedSong();
            }
            else
            {
                SaveProFile(FileNamePro, false);
            }
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

                if (!EditorG5.IsLoaded)
                    return;

                var currTrackNameG5 = EditorG5.SelectedTrack.Name;
                var currentDifficulty5 = EditorG5.CurrentDifficulty;

                bool fullRebuild = false;

                if (EditorPro.IsLoaded == false)
                {
                    fullRebuild = true;

                    var p = EditorG5.Sequence.ConvertToPro();
                    EditorPro.SetTrack6(p, p.GetPrimaryTrack());
                }
                else
                {
                    var currTrackNamePro = string.Empty;
                    if (EditorPro.SelectedTrack != null)
                    {
                        currTrackNamePro = EditorPro.SelectedTrack.Name;
                    }
                    var currentDifficulty6 = EditorPro.CurrentDifficulty;

                    var tracks = EditorPro.Sequence.GetGuitarBassTracks().ToList();
                    if (checkBoxInitSelectedTrackOnly.Checked)
                    {
                        tracks = EditorPro.GuitarTrack.GetTrack().MakeEnumerable().ToList();
                    }

                    var difficulties = Utility.GetDifficultyIter();

                    if (checkBoxInitSelectedDifficultyOnly.Checked)
                    {
                        difficulties = difficulties.Where(x => x == EditorPro.CurrentDifficulty);
                    }

                    tracks.ForEach(proTrack =>
                    {
                        foreach (var diffiter in difficulties)
                        {
                            var diff = diffiter;
                            var ok = true;
                            ok = EditorPro.SetTrack6(proTrack, diff);
                            if (!ok)
                                return;

                            if (proTrack.Name.IsGuitarTrackName())
                            {
                                ok = EditorG5.SetTrack5(EditorG5.GetGuitar5MidiTrack(), diff);
                            }
                            else// if (proTrack.Name.IsBassTrackName())
                            {
                                ok = EditorG5.SetTrack5(EditorG5.GetGuitar5BassElseGuitar(), diff);
                            }
                            if (!ok)
                                return;


                            if (diff == GuitarDifficulty.Expert)
                                diff |= GuitarDifficulty.All;

                            var tempo = EditorPro.Sequence.GetTempoTrack();
                            EditorPro.Sequence.Remove(tempo);
                            EditorPro.Sequence.AddTempo(EditorG5.Sequence.GetTempoTrack().Clone(FileType.Pro));

                            EditorPro.Messages.Chords.ToList().ForEach(x => x.DeleteAll());
                            EditorG5.Messages.Chords.ForEach(x =>
                            {
                                var ev = x.CloneToMemory(EditorPro.Messages, EditorPro.CurrentDifficulty);
                                ev.IsNew = true;
                                ev.CreateEvents();
                            });

                            if (diff.IsAll())
                            {
                                EditorPro.Messages.Solos.ToList().ForEach(x => x.DeleteAll());
                                EditorG5.Messages.Solos.ForEach(x =>
                                {
                                    var ev = new GuitarSolo(EditorPro.Messages, x.TickPair);
                                    ev.IsNew = true;
                                    ev.CreateEvents();
                                });

                                EditorPro.Messages.Powerups.ToList().ForEach(x => x.DeleteAll());
                                EditorG5.Messages.Powerups.ForEach(x =>
                                {
                                    var ev = new GuitarPowerup(EditorPro.Messages, x.TickPair);
                                    ev.IsNew = true;
                                    ev.CreateEvents();
                                });

                                EditorPro.Messages.BigRockEndings.ToList().ForEach(x => x.DeleteAll());
                                EditorG5.Messages.BigRockEndings.ForEach(x =>
                                {
                                    var ev = new GuitarBigRockEnding(EditorPro.Messages, x.TickPair);
                                    ev.IsNew = true;
                                    ev.CreateEvents();
                                });
                            }
                        }
                    });

                    EditorPro.SetTrack(currTrackNamePro, currentDifficulty6);
                }

                EditorG5.SetTrack(currTrackNameG5, currentDifficulty5);

                if (SelectedSong == null)
                {
                    AddNewSongToLibrary(false);
                }

                ReloadTracks();

                if (EditorG5.IsLoaded && EditorPro.IsLoaded && SelectedSong != null)
                {

                    if (fullRebuild)
                    {
                        var config = new GenDiffConfig(SelectedSong,
                            true,
                            false,
                            checkBoxInitSelectedDifficultyOnly.Checked,
                            checkBoxInitSelectedTrackOnly.Checked,
                            true);

                        GenerateDifficulties(false, config);
                    }

                    ReloadTracks();
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
            EditorPro.SetSelectionStateIdle();

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
            EditorPro.SetSelectionStateIdle();

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
            EditorPro.SetSelectionStateIdle();

        }

        private void button27_Click(object sender, EventArgs e)
        {
            Utility.timeScalar = textBoxZoom.Text.ToDouble(1.0);
            AfterZoom();
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
            EditorPro.SetSelectionStateIdle();

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


        private void button33_Click(object sender, EventArgs e)
        {
            MoveHoldBoxChordUpString();
        }

        public void MoveHoldBoxChordUpString()
        {
            try
            {
                var hb = GetHoldBoxes();
                var ncb = NoteChannelBoxes;
                for (int x = 0; x < 5; x++)
                {
                    ncb[x].Text = ncb[x + 1].Text;
                    hb[x].Text = hb[x + 1].Text;
                    if (checkIndentBString.Checked && x == 1)
                    {
                        int i = hb[x].Text.ToInt();
                        if (!i.IsNull())
                        {
                            i++;
                            hb[x].Text = i.ToStringEx();
                        }
                    }
                }
                hb[5].Text = "";
                ncb[5].Text = "";
            }
            catch { }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            MoveHoldBoxChordDownString();
        }

        public void MoveHoldBoxChordDownString()
        {
            try
            {
                var hb = GetHoldBoxes();
                var ncb = NoteChannelBoxes;

                for (int x = 5; x > 0; x--)
                {
                    hb[x].Text = hb[x - 1].Text;
                    ncb[x].Text = ncb[x - 1].Text;

                    if (checkIndentBString.Checked == true &&
                        x == 2)
                    {
                        var i = hb[x].Text.ToInt();
                        if (!i.IsNull())
                        {
                            i--;
                            hb[x].Text = i.ToStringEx();
                        }
                    }
                }
                hb[0].Text = "";
                ncb[0].Text = "";
            }
            catch { }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            MoveHoldBoxChordsUpWholeStep();
        }

        public void MoveHoldBoxChordsUpWholeStep()
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
            MoveHoldBoxChordsDownWholeStep();
        }

        public void MoveHoldBoxChordsDownWholeStep()
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
            MoveHoldBoxChordsUpHalfStep();
        }

        public void MoveHoldBoxChordsUpHalfStep()
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
            MoveHoldBoxChordDownHalfStep();
        }

        public void MoveHoldBoxChordDownHalfStep()
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
            EditorPro.SetSelectionStateIdle();

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
            EditorPro.SetSelectionStateIdle();

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
            var ticks = GetChordTicksFromScreen();

            if (ticks.IsValid)
            {
                textBoxNoteEditorSelectedChordTickLength.SuspendLayout();
                textBoxNoteEditorSelectedChordTickLength.Text = (ticks.TickLength).ToStringEx();
                textBoxNoteEditorSelectedChordTickLength.ResumeLayout();
            }
        }

        private void textBox19_TextChanged(object sender, EventArgs ve)
        {
            if (DesignMode)
                return;

            var ticks = GetChordTicksFromScreen();

            var tickLength = textBoxNoteEditorSelectedChordTickLength.Text.ToInt();

            if (!ticks.Down.IsNull() && !tickLength.IsNull())
            {
                GetChordEndBox().Text = (ticks.Down + tickLength).ToStringEx();
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
                    SelectedSong == null ? checkGenDiffCopyGuitarToBass.Checked : SelectedSong.CopyGuitarToBass,
                    checkBoxInitSelectedDifficultyOnly.Checked,
                    checkBoxInitSelectedTrackOnly.Checked,
                    Utility.HandPositionGenerationEnabled);

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
            var path = textBoxSongLibG5MidiFileName.Text;

            if (string.IsNullOrEmpty(path))
                path = textBoxSongLibProMidiFileName.Text;

            var s = ShowOpenFileDlg("Select midi file",
                DefaultMidiFileLocationG5, path);
            if (!string.IsNullOrEmpty(s))
            {
                textBoxSongLibG5MidiFileName.Text = s;
                textBoxSongLibG5MidiFileName.ScrollToEnd();

                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);

                    OpenSongCacheItem(SelectedSong);
                }
            }
        }

        private void button66_Click(object sender, EventArgs e)
        {
            var path = textBoxSongLibProMidiFileName.Text;

            if (path.IsNotEmpty())
            {
                var name5 = textBoxSongLibG5MidiFileName.Text.Trim();
                if (name5.IsNotEmpty())
                {
                    var folder = name5.GetFolderName();

                    path = folder.PathCombine(name5.GetFileNameWithoutExtension() + Utility.DefaultPROFileExtension);
                }
            }

            var s = ShowOpenFileDlg("Select pro midi file",
                DefaultMidiFileLocationPro, path);
            if (s.IsNotEmpty())
            {
                textBoxSongLibProMidiFileName.Text = s;
                textBoxSongLibProMidiFileName.ScrollToEnd();

                if (SelectedSong != null)
                {
                    UpdateSongCacheItem(SelectedSong);

                    OpenSongCacheItem(SelectedSong);
                }
            }
        }


        private void button68_Click(object sender, EventArgs e)
        {
            try
            {
                SongList.SelectedSong = listBoxSongLibrary.SelectedItem as SongCacheItem;
                OpenSongCacheItem(SongList.SelectedSong);
            }
            catch { }
        }
        private void button67_Click(object sender, EventArgs e)
        {
            SaveSelectedSong();
        }

        public void SaveSelectedSong()
        {
            SelectedSong.IfObjectNotNull(song =>
            {
                if (EditorPro.IsLoaded)
                {
                    if (SaveProFile(song.G6FileName, false))
                    {
                        textBoxSongLibProMidiFileName.Text = song.G6FileName;
                    }
                }
                UpdateSongCacheItem(song);
            });
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
            try
            {
                var fileName = textBoxSongLibConFile.Text;
                var folder = DefaultConFileLocation;

                if (fileName.IsEmpty())
                {
                    if (SelectedSong != null)
                    {
                        var fn = (SelectedSong.G6FileName.GetIfEmpty(SelectedSong.G5FileName) ?? "").GetFileNameWithoutExtension();
                        if (fn.EndsWithEx("_pro"))
                            fn = fn.Replace("_pro", "");
                        fileName = fn + Utility.DefaultCONFileExtension;
                    }
                }
                else if (fileName.FileExists())
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
            catch { }
        }

        private void listBoxSongLibrary_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listBoxSongLibrary.SelectedIndex != -1)
                {
                    SongList.SelectedSong = listBoxSongLibrary.SelectedItem as SongCacheItem;
                    OpenSongCacheItem(SongList.SelectedSong);
                }
            }
            catch { }
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
            OpenExplorerFolder(textBoxSongLibG5MidiFileName.Text);
        }

        private void button79_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBoxSongLibProMidiFileName.Text);
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
            Utility.timeScalar -= Utility.timeScalarZoomSpeed;

            if (Utility.timeScalar > Utility.timeScalarMax)
                Utility.timeScalar = Utility.timeScalarMax;

            if (Utility.timeScalar < Utility.timeScalarMin)
                Utility.timeScalar = Utility.timeScalarMin;

            this.textBoxZoom.Text = Utility.timeScalar.ToString();
            AfterZoom();

        }

        private void AfterZoom()
        {
            if (EditorPro.IsLoaded)
            {
                EditorPro.SetTrackMaximum();

                if (SelectedChord == null)
                {
                    var vc = EditorPro.GetVisibleChords();
                    if (vc != null && vc.Any())
                    {
                        SetSelectedChord(vc.First(), true);
                    }
                }
            }
            if (EditorG5.IsLoaded)
            {
                EditorG5.SetTrackMaximum();
            }

            ScrollToSelection();
        }

        private void button84_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        public void ZoomIn()
        {
            Utility.timeScalar += Utility.timeScalarZoomSpeed;

            if (Utility.timeScalar > Utility.timeScalarMax)
                Utility.timeScalar = Utility.timeScalarMax;

            if (Utility.timeScalar < Utility.timeScalarMin)
                Utility.timeScalar = Utility.timeScalarMin;

            this.textBoxZoom.Text = Utility.timeScalar.ToString();
            AfterZoom();
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

        GuitarTrack GuitarTrackG5
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

                var fc = EditorPro.Messages.Chords.FirstOrDefault();
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


                var sc = EditorPro.Messages.Chords.LastOrDefault();
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

                ret = GetStoredChordFromScreen(chord.TickLength);
            }
            catch { }

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
                    sel = EditorPro.Messages.Chords.LastOrDefault();
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
                if (FileNamePro.IsNotEmpty())
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

                var sb = new StringBuilder();
                foreach (SongCacheItem item in songs)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetText(sb.ToString());
            }
            catch { }
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

            if (zipFileName.IsEmpty())
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

                    foreach (var localFilePath in localFilePaths.Where(x => x.FileExists()))
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
            IEnumerable<string> extensions, bool overWriteExisting = false)
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
                if (i > 0 && i < 100000)
                {
                    Utility.GridSnapDistance = i;
                }
            }
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
            var result = FindMatchingCopyPattern(new FindMatchingPatternConfig(false, true, true));

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
            var result = FindMatchingCopyPattern(new FindMatchingPatternConfig(true, false, true));

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
            ReplacePreviousMatchingPattern();
        }

        public bool ReplacePreviousMatchingPattern()
        {
            var result = FindPreviousMatchingPattern();

            if (result != null && result.Length > 0)
            {
                EditorPro.ClearSelection();

                result.ForEach(x => x.Selected = true);
                ScrollToSelection();
                return true;
            }
            return false;
        }

        private void buttonReplaceFindNext_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            ReplaceNextMatchingPattern();
        }

        public bool ReplaceNextMatchingPattern()
        {
            var result = FindNextMatchingPattern();

            if (result != null && result.Length > 0)
            {
                EditorPro.ClearSelection();
                result.ForEach(x => x.Selected = true);
                EditorPro.ScrollToSelection();

                return true;
            }
            else
            {
                return false;
            }
        }


        private void button122_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            EditorPro.BackupSequence();
            MoveSelectedDown12Frets();
        }

        public void MoveSelectedDown12Frets()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;

                foreach (var ch in EditorPro.SelectedChords.ToList())
                {
                    ch.DownTwelveFrets();
                }
                EditorPro.Invalidate();
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

                foreach (var ch in EditorPro.SelectedChords.ToList())
                {
                    ch.UpString();
                }

            }
            catch
            {
                UndoLast();
            }
            EditorPro.Invalidate();
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
            return nodes.ToEnumerable<TreeNode>().FirstOrDefault(x => x.Name.EqualsEx(name));
        }

        void RefreshSubTreeNode(TreeNodeCollection nodes, FATXReadContents contents)
        {
            if (contents == null)
            {
                return;
            }

            foreach (var f in contents.Folders)
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
            var oldPath = treeUSBContents.SelectedNode.GetIfNotNull(x=> x.FullPath);
            
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
                        foreach (var fld in SelectedUSB.Folders)
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
                if (oldPath.IsNotEmpty())
                {
                    SelectUSBFolder(oldPath);
                }
            }
            else
            {
                if (treeUSBContents.SelectedNode == null)
                {
                    SelectDefaultUSBFolder();
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

        private void SelectDefaultUSBFolder()
        {
            var defFolder = settings.GetValue("SelectedUSBFolderPath");
            SelectUSBFolder(defFolder);
        }

        public void SelectUSBFolder(string folder)
        {
            if (folder.IsNotEmpty())
            {
                var elems = folder.Split(new[] { treeUSBContents.PathSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var nodes = treeUSBContents.Nodes;
                TreeNode node = null;
                while (elems.Any())
                {
                    var foundNodes = nodes.GetChildByText(elems.First());
                    if (foundNodes.IsNotEmpty())
                    {
                        elems.RemoveAt(0);
                        node = foundNodes.First();
                        nodes = node.Nodes;
                    }
                    else
                    {
                        break;
                    }
                }
                if (node != null)
                {
                    treeUSBContents.SelectedNode = node;
                }
            }
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

                if (listBoxUSBSongs.SelectedItems != null &&
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


        ListViewColumnSorter sorter;

        int nextTimerID = 123;




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
            int groupID = 0, int MSBetweenTicks = 400, int numTotalTicks = 8)
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
            if (listUSBFileView.SelectedItems.Count == 1)
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
            foreach (ListViewItem s in listBoxUSBSongs.SelectedItems)
                s.Selected = false;
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
                                    var folderContents = folder.Read();
                                    if (folderContents != null)
                                    {
                                        foreach (var f in folderContents.Files)
                                        {
                                            if (f.Name.EqualsEx(newName))
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

            if (!X360.Other.VariousFunctions.IsValidXboxName(name))
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
                            if (path.IsNotEmpty())
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
                            if (folder.IsEmpty())
                            {
                                folder = ShowSaveFileDlg("Save USB File", lastFolder, file.Name);
                            }
                            if (folder.IsNotEmpty())
                            {
                                folder = Path.Combine(Path.GetDirectoryName(folder), file.Name);
                                lastFolder = folder;
                            }
                            if (folder.IsEmpty())
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
                    else if (SelectedUSBFiles.Count == 1)
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

        void AddFileToUSB(string fileName)
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
                    var def = settings.GetValue("SelectedUSBFolderPath");
                    if (def.IsEmpty())
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
            treeUSBContents.SelectedNode.IfNotNull(x=>
                settings.SetValue("SelectedUSBFolderPath", x.FullPath));
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
                            if (usb.IsOpen)
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
                if (!string.IsNullOrEmpty(fileName))
                {
                    bool success = false;
                    if (SelectedUSB.Open())
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
                    foreach (var fl in contents.Files)
                    {
                        foreach (ListViewItem li in listUSBFileView.SelectedItems)
                        {
                            var f = li.Tag as FATXFileEntry;
                            if (f != null)
                            {
                                if (string.Compare(f.Name, fl.Name, true) == 0)
                                    ret.Add(fl);
                            }
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

            if (!X360.Other.VariousFunctions.IsValidXboxName(name))
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
                foreach (ListViewItem s in listBoxUSBSongs.SelectedItems)
                    s.Selected = false;
            }
            else
            {
                foreach (ListViewItem s in listBoxUSBSongs.Items)
                    s.Selected = true;

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
                        CopyPowerupDataForCurrentTrack(EditorPro.Messages);
                    }
                    else
                    {
                        var copyGuitarToBass = SelectedSong != null && SelectedSong.CopyGuitarToBass;

                        foreach (var trackname in GuitarTrack.TrackNames6)
                        {
                            Track g5, g6;
                            if (!copyGuitarToBass && GuitarTrack.IsBassTrackName(trackname))
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
                                    CopyPowerupDataForCurrentTrack(EditorPro.Messages);
                                    RefreshTracks();
                                }
                            }
                        }
                    }
                    ReloadTracks();
                }
                catch
                {
                    ret = false;
                }
            });
            return ret;
        }

        private void buttonExecuteBatchCopyUSB_Click(object sender, EventArgs e)
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
                    try
                    {
                        var file = files[0];
                        var p = Package.Load(file.GetFatXStream(), false);
                        LoadPackageIntoTree(p, file);

                        textBoxPackageDTAText.Text = "";
                        tabContainerMain.SelectedTab = tabPackageEditor;
                        tabControl3.SelectedTab = tabPage8;
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
                treeUSBContents.SelectedNode = tn;

                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                {
                    treeUSBContents.BeginUpdate();
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            AddFileToUSB(file);
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
                        var item = o as ListViewItem;

                        if (item != null && item.Tag != null &&
                            listUSBFileView.Items.Contains(item) &&
                            item.Tag is FATXFileEntry)
                        {
                            var fileEntry = item.Tag as FATXFileEntry;
                            item.Tag = WriteUSBFile(tn.Tag as FATXFolderEntry, fileEntry.Name, fileEntry);
                        }
                    }
                }

            }
        }

        private FATXFileEntry WriteUSBFile(FATXEntry folderEntry, string fileName, FATXFileEntry fileEntry)
        {
            FATXFileEntry ret = null;
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
                            if (string.Compare(file.Name, fileEntry.Name, true) == 0)
                            {
                                fileEntry = file;
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {

                            var b = fileEntry.xExtractBytes();
                            if (b != null)
                            {
                                FATXFolderEntry xfolder2;
                                FATXEntry parent = folderEntry.Parent;
                                var path = folderEntry.Name;
                                while (parent != null)
                                {
                                    path = parent.Name + "/" + path;
                                    parent = parent.Parent;
                                }

                                var contents2 = SelectedUSB.Drive.ReadToFolder(
                                    path, out xfolder2);


                                if (xfolder2.AddFile(fileName, b, AddType.Replace))
                                {
                                    contents2 = SelectedUSB.Drive.ReadToFolder(path, out xfolder2);
                                    ret = contents2.Files.Where(x => string.Compare(x.Name, fileName, true) == 0).FirstOrDefault();
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
            return ret;
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
                                if (ModifierKeys.HasFlag(Keys.Control))
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
            Midi6 = (1 << 0),
            Midi5 = (1 << 1),
            Con6 = (1 << 2),
            Con5 = (1 << 3),
            Any5 = (Midi5 | Con5),
            Any6 = (Midi6 | Con6),
            AnyMidi = (Midi5 | Midi6),
            AnyCon = (Con6 | Con5),
            Any = (AnyMidi | AnyCon),
        }

        public bool OpenEditorFile(string fileName, byte[] data = null, EditorFileType openFileType = EditorFileType.Any)
        {
            bool ret = false;
            try
            {
                if (fileName.IsEmpty())
                    fileName = "Unknown";


                if (data == null && !fileName.FileExists())
                    return ret;

                var isMidi = fileName.FileExists() && (fileName.GetFileName().EndsWithEx(".mid") || fileName.GetFileName().EndsWithEx(".midi"));

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
                if (!isMidi && (openFileType & EditorFileType.AnyCon) != 0)
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
                        if (seq != null && seq.Any())
                        {
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
                    foreach (var file in files)
                    {
                        if (file.FileExists())
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
                CloseG5Track();

                bool hasErrors = false;
                ClearBatchResults();
                var files = SelectedUSBFiles;
                foreach (var f in files)
                {
                    WriteBatchResult("Processing " + f.Name);

                    try
                    {
                        var p = Package.Load(f.GetFatXStream());
                        if (p != null)
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
            CloseG5Track();
        }

        public void CloseG5Track()
        {
            trackEditorG5.Close();

        }

        private void buttonCloseG6Track_Click(object sender, EventArgs e)
        {
            CloseProTrack();
        }

        public void CloseProTrack()
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
                                CloseProTrack();

                                trackEditorG6.SetTrack6(seq, trackEditorG6.GetProTrack(seq), GuitarDifficulty.Expert);
                                FileNamePro = "Temp - " + f.Name;
                            }
                            else
                            {
                                CloseG5Track();

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
                        LoadPackageIntoTree(p, SelectedSong.G6ConFile, true);

                        textBoxPackageDTAText.Text = "";
                    }
                }
            }
            catch { }
        }

        private void trackEditorG5_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                CloseG5Track();
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

                if (GuitarTrack.GuitarTrackName17 == ProGuitarTrack.Name)
                {
                    CopyTrack(GuitarTrack.GuitarTrackName17, GuitarTrack.BassTrackName17);
                }
                if (GuitarTrack.GuitarTrackName22 == ProGuitarTrack.Name)
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

                    var tempoTrack = editor.GuitarTrack.GetTempoTrack();
                    if (tempoTrack == null)
                    {
                        tempoTrack = new Track(editor.Sequence.FileType, "tempo");
                        editor.Sequence.AddTempo(tempoTrack);
                    }
                    else
                    {
                        tempoTrack.TimeSig.ToList().ForEach(x =>
                            tempoTrack.Remove(x));
                    }

                    tempoTrack.Insert(0, GuitarTimeSignature.BuildMessage(
                        textBoxTempoNumerator.Text.ToInt(4),
                        textBoxTempoDenominator.Text.ToInt(4)));

                    ReloadTracks();
                }
            }
            catch { MessageBox.Show("Cannot create time signature"); }
        }

        private void button131_Click(object sender, EventArgs e)
        {
            if (EditorG5.IsLoaded)
            {
                GuitarTrackG5.GetTempoTrack().IfObjectNotNull(o =>
                    o.TimeSig.IfAny(timeSig =>
                    {
                        var ts = timeSig.First();
                        var tsb = new TimeSignatureBuilder((MetaMessage)ts.Clone());
                        textBoxTempoDenominator.Text = tsb.Denominator.ToString();
                        textBoxTempoNumerator.Text = tsb.Numerator.ToString();
                    },
                    Else =>
                    {
                        textBoxTempoDenominator.Text = "4";
                        textBoxTempoNumerator.Text = "4";
                    }));
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

                foreach (var ch in EditorPro.SelectedChords.ToList())
                {
                    ch.DownString();
                }
                EditorPro.Refresh();
            }
            catch
            {
                UndoLast();
            }
        }

        private void buttonUp12_Click(object sender, EventArgs e)
        {
            EditorPro.BackupSequence();
            MoveSelectedUp12Frets();
        }

        public void MoveSelectedUp12Frets()
        {
            try
            {
                if (!EditorPro.IsLoaded)
                    return;

                foreach (var ch in EditorPro.SelectedChords.ToList().
                    Where(x => x.Notes.Any(n => n.NoteFretDown <= 10)).ToList())
                {
                    ch.UpTwelveFrets();
                }

                EditorPro.Invalidate();
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
            var ret = new List<GuitarChord>();
            try
            {
                SelectedStoredChord.IfObjectNotNull(x =>
                {
                    var sc = FindFirstStoredChordMatch(x);
                    while (sc != null)
                    {
                        ret.Add(sc);
                        sc = FindNextStoredChordMatch(sc, x);
                    }
                });
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
                if (!EditorPro.IsLoaded)
                    return;

                var ssc = SelectedStoredChord;
                if (ssc == null)
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
                if (matchingChords == null || matchingChords.Count == 0)
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

                foreach (var chord in matchingChords)
                {
                    mcp.Matches.Add(new GuitarChord[] { chord });
                }


                int numReplaced = 0;
                int minTick = int.MinValue;
                foreach (var m in mcp.Matches)
                {
                    int irep = 0;
                    minTick = ReplaceNotes(EditorPro.Messages,
                        mcp.OriginalChords6, mcp.DeltaTimeStart,
                                minTick, m,
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
                var sel = EditorPro.Messages.Chords.FirstOrDefault(chord => chord.Notes.Any(x => x.NoteFretDown > 17));

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
            var ret = false;
            TickPair ticks;
            ret = EditorG5.SnapToNotes(chord.TickPair, out ticks);

            if (ret)
            {
                chord.SetTicks(ticks);
                chord.UpdateEvents();
            }
            return ret;
        }

        bool SetChordToG5Length(GuitarChord chord)
        {
            try
            {
                var g5Chords = EditorG5.Messages.Chords;

                var gc = g5Chords.GetBetweenTick(chord.TickPair);
                if (gc != null && gc.Any())
                {
                    if (gc.Count() > 1)
                    {
                        gc = g5Chords.GetBetweenTick(chord.TickPair.Expand(-1));
                        if (gc.Count() > 1)
                        {
                            gc = g5Chords.GetBetweenTick(chord.TickPair.Offset(Utility.TickCloseWidth));
                            if (gc.Count() > 1)
                            {
                                gc = g5Chords.GetBetweenTick(chord.TickPair.Expand(-Utility.TickCloseWidth));
                            }
                        }
                    }
                    if (gc.Any())
                    {
                        chord.SetTicks(gc.First().TickPair);
                        chord.UpdateEvents();

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
                var chords = EditorPro.SelectedChords.ToList();
                if (!chords.Any())
                    chords = EditorPro.Messages.Chords.ToList();

                chords.ForEach(x =>
                {
                    var newTicks = EditorPro.SnapLeftRightTicks(x.TickPair, new SnapConfig(true, false, false));
                    if (x.TickPair != newTicks)
                    {
                        x.SetTicks(newTicks);
                        x.UpdateEvents();
                    }
                });

                EditorPro.Invalidate();
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
                foreach (var chord in EditorPro.Messages.Chords.ToList())
                {
                    var ticks = new TickPair(chord.DownTick, chord.UpTick - 1);
                    if (!ticks.IsShort)
                    {
                        chord.SetTicks(ticks);
                        chord.UpdateEvents();
                    }
                }
            }
            catch { }
            EditorPro.Invalidate();
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
                gt.DeleteAll();
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
                gt.DeleteAll();
            }
            RefreshTrainers();
        }

        private void buttonCancelProGuitarTrainer_Click(object sender, EventArgs e)
        {
            EditorPro.SetSelectionStateIdle();
        }

        private void buttonCancelProBassTrainer_Click(object sender, EventArgs e)
        {
            EditorPro.SetSelectionStateIdle();
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

                    CreateTrainer(EditorPro.Messages,
                        GuitarTrainerType.ProGuitar,
                        EditorPro.SelectedChords.GetTickPair(),
                        checkTrainerLoopableProGuitar.Checked);

                    RefreshTrainers();
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

                    CreateTrainer(EditorPro.Messages,
                        GuitarTrainerType.ProBass,
                        EditorPro.SelectedChords.GetTickPair(),
                        checkTrainerLoopableProBass.Checked);

                    RefreshTrainers();
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

            var trainer = SelectedProGuitarTrainer;
            if (trainer != null)
            {
                EditorPro.BackupSequence();
                var pair = new TickPair(textBoxProGuitarTrainerBeginTick.Text.ToInt(),
                    textBoxProGuitarTrainerEndTick.Text.ToInt());
                if (pair.IsValid)
                {
                    trainer.SetTicks(pair);
                    trainer.Loopable = checkTrainerLoopableProGuitar.Checked;
                    trainer.UpdateEvents();

                    RefreshTrainers();
                }
                else
                {
                    MessageBox.Show("Invalid start or end tick");
                }
            }
        }

        private void buttonUpdateProBassTrainer_Click(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;

            var trainer = SelectedProBassTrainer;
            if (trainer != null)
            {
                EditorPro.BackupSequence();

                var pair = new TickPair(textBoxProBassTrainerBeginTick.Text.ToInt(),
                    textBoxProBassTrainerEndTick.Text.ToInt());
                if (pair.IsValid)
                {
                    trainer.SetTicks(pair);
                    trainer.Loopable = checkTrainerLoopableProBass.Checked;
                    trainer.UpdateEvents();

                    RefreshTrainers();
                }
                else
                {
                    MessageBox.Show("Invalid start or end tick");
                }
            }
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
                        EditorPro.SelectedTextEvent.DeleteAll();
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
                            EditorPro.SelectedTextEvent.SetDownTick(tick);
                            EditorPro.SelectedTextEvent.UpdateEvents();

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
                        l.Add(ProGuitarTrack.GetTrack());
                        gtList = l;
                    }
                    else
                    {
                        gtList = EditorPro.Tracks.Where(x => x.Name.IsProTrackName()).ToList();
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
                                CreateTrainer(EditorPro.Messages,
                                    tr.Name.IsGuitarTrackName() ?
                                    GuitarTrainerType.ProGuitar : GuitarTrainerType.ProBass,
                                    solo.TickPair, true);
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
                    var proEvents = ProGuitarTrack.Messages.TextEvents.ToList();
                    foreach (var ev in GuitarTrackG5.Messages.TextEvents.ToList())
                    {
                        if (!proEvents.Any(x =>
                            x.AbsoluteTicks == ev.AbsoluteTicks &&
                            x.Text.EqualsEx(ev.Text)))
                        {
                            GuitarTextEvent.CreateTextEvent(EditorPro.Messages, ev.AbsoluteTicks, ev.Text);
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
                var wasLoaded = EditorPro.IsLoaded;

                ReloadTracks();

                EditorPro.SetTrack6(track.Sequence, track, difficulty);

                ReloadTracks();
                RefreshTracks();

                if (!wasLoaded)
                {
                    SaveProFile(EditorPro.LoadedFileName, false);
                }
            }
        }

        private void midiTrackEditorG5_TrackAdded(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            var wasLoaded = EditorG5.IsLoaded;
            try
            {
                ReloadTracks();
                EditorG5.SetTrack5(track.Sequence, track, difficulty);
                ReloadTracks();
                RefreshTracks();


                if (!wasLoaded)
                {
                    ShowG5MidiSaveAs();
                }
            }
            catch { }
        }

        private void ShowG5MidiSaveAs()
        {
            var folder = DefaultMidiFileLocationG5.AppendSlashIfMissing();
            if (SelectedSong != null)
                folder = SelectedSong.G6FileName.GetIfEmpty(SelectedSong.G5FileName).GetFolderName();

            ShowSaveFileDlg("Save Midi 5", folder,
                "").IfNotEmpty(fileName =>
                {

                    if (EditorG5.SaveTrackAs(fileName))
                    {
                        FileNameG5 = fileName;
                        if (SelectedSong != null)
                        {
                            SelectedSong.G5FileName = FileNameG5;
                            textBoxSongLibG5MidiFileName.Text = SelectedSong.G5FileName;
                            UpdateSongCacheItem(SelectedSong);
                        }
                    }
                });
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

            comboBox180.BeginUpdate();

            comboBox180.Items.Clear();

            try
            {
                if (EditorPro.IsLoaded)
                {
                    comboBox180.Items.AddRange(ProGuitarTrack.Messages.HandPositions.ToArray());
                }
            }
            catch { }
            comboBox180.EndUpdate();


        }

        private void comboBox180_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var msg = comboBox180.SelectedItem as GuitarMessage;
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


                foreach (var ch in EditorPro.SelectedChords.ToList())
                {
                    ch.DownOctave();
                }
                EditorPro.Invalidate();
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


                foreach (var ch in EditorPro.SelectedChords.ToList())
                {
                    ch.UpOctave();
                }

                EditorPro.Invalidate();
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
                foreach (var chord in EditorPro.Messages.Chords.ToList())
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

                var chords = EditorPro.SelectedChords.ToList();
                if (!chords.Any())
                    chords = EditorPro.Messages.Chords.ToList();

                foreach (var c in chords)
                {
                    if (SetChordToG5Length(c))
                        numSnapped++;
                }

                EditorPro.Invalidate();


            }
            catch
            {
                EditorPro.Invalidate();
                MessageBox.Show("Error");
            }

        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void buttonShortToG5Len_Click(object sender, EventArgs e)
        {
            try
            {
                int numSnapped = 0;
                if (EditorPro.IsLoaded && EditorG5.IsLoaded)
                {
                    var chords = ProGuitarTrack.Messages.Chords.Where(x => !x.IsDeleted && x.TickLength < Utility.NoteCloseWidth).ToList();
                    foreach (var c in chords)
                    {
                        if (SetChordToG5Length(c))
                            numSnapped++;
                    }
                }

            }
            catch { }
            EditorPro.Invalidate();
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
            if (location.IsEmpty() && SelectedSong != null)
                location = SelectedSong.G6FileName.GetIfEmpty(SelectedSong.G5FileName);

            ShowOpenFileDlg("Select MP3 file",
                DefaultMidiFileLocationPro,
                location.GetFolderName()).IfNotEmpty(mp3File =>
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
            ControlAnimator.CreateHeightChange(panelProEditor, panelProEditor.Height + 20);
        }

        private void buttonMinimizeG6_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(panelProEditor, (panelProEditor.Height - 20).Max(60));
        }

        private void buttonMaximizeG5_Click(object sender, EventArgs e)
        {
            ControlAnimator.CreateHeightChange(
                panel5ButtonEditor, (panel5ButtonEditor.Height + 20));
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
            UpdateControlsForDifficulty(EditorPro.CurrentDifficulty);
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

                                var fileName6 = sng.G6FileName.GetFileNameWithoutExtension();
                                var fileName5 = sng.G5FileName.GetFileNameWithoutExtension();
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
                            if (EditorG5.IsLoaded && checkBoxBatchUtilExtractXMLG5.Checked)
                            {
                                var fileName5 = sng.G5FileName.GetFileNameWithoutExtension();
                                var file = dir.PathCombine(fileName5 + ".xml");
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

                var modifiers = ProGuitarTrack.Messages.GetModifiersBetweenTick(new TickPair(Int32.MinValue, Int32.MaxValue));
                if (modifiers.Any())
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
                        else if (msg.MidiEvent.IsSoloEvent(true))
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

                        tempoNode.AddAttribute("secondsPerWholeNote", (tempo.Tempo / 100000.0).ToStringEx());
                        tempoNode.AddAttribute("secondsPerTick", tempo.SecondsPerTick.ToStringEx());

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

                var meta = ProGuitarTrack.GetEvents().Where(x => x.IsMetaEvent() && x.IsTempoTimesigEvent() == false);
                if (meta.Any())
                {
                    var metaEvents = track.AddNode("metaEvents");
                    foreach (var ev in meta)
                    {
                        var te = metaEvents.AddNode("metaEvent");
                        te.AddAttribute("startTime", ProGuitarTrack.TickToTime(ev.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("startTick", ev.AbsoluteTicks.ToStringEx());

                        te.AddAttribute("metaType", ev.MetaType.ToString());
                        ev.Text.IfNotEmpty(x => te.AddAttribute("text", x));
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

            root.AddAttribute("length", GuitarTrackG5.TotalSongTime.ToStringEx());

            var tracks = root.AddNode("tracks");
            foreach (var track5 in EditorG5.Sequence)
            {
                var track = tracks.AddNode("track");
                track.AddAttribute("name", track5.Name);

                EditorG5.SetTrack5(track5, GuitarDifficulty.Expert);
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Hard;
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Medium;
                extractXMLSerializeChords5(track);
                EditorG5.CurrentDifficulty = GuitarDifficulty.Easy;
                extractXMLSerializeChords5(track);

                EditorG5.CurrentDifficulty = GuitarDifficulty.Expert;

                var modifiers = GuitarTrackG5.Messages.GetModifiersBetweenTick(new TickPair(Int32.MinValue, Int32.MaxValue)).ToList();

                if (modifiers.Any())
                {
                    var mods = track.AddNode("modifiers");
                    foreach (var msg in modifiers)
                    {

                        var type = string.Empty;
                        if (msg.MidiEvent.IsBigRockEnding())
                            type = "BigRockEnding";
                        else if (msg.MidiEvent.IsSoloEvent(false))
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
                if (track5.IsTempo())
                {
                    var temposNode = track.AddNode("tempos");
                    foreach (var tempo in GuitarTrackG5.Messages.Tempos)
                    {
                        var tempoNode = temposNode.AddNode("tempo");
                        tempoNode.AddAttribute("startTime", tempo.StartTime.ToStringEx());
                        tempoNode.AddAttribute("startTick", tempo.AbsoluteTicks.ToStringEx());
                        tempoNode.AddAttribute("rawTempo", tempo.Tempo.ToStringEx());

                        tempoNode.AddAttribute("secondsPerWholeNote", (tempo.Tempo / 100000.0).ToStringEx());
                        tempoNode.AddAttribute("secondsPerTick", tempo.SecondsPerTick.ToStringEx());


                    }
                }
                if (track5.IsTempo())
                {
                    var timeSigsNode = track.AddNode("timeSignatures");
                    foreach (var timeSig in GuitarTrackG5.Messages.TimeSignatures)
                    {
                        var timeSigNode = timeSigsNode.AddNode("timeSignature");
                        timeSigNode.AddAttribute("startTime", timeSig.StartTime.ToStringEx());
                        timeSigNode.AddAttribute("startTick", timeSig.AbsoluteTicks.ToStringEx());

                        timeSigNode.AddAttribute("numerator", timeSig.Numerator.ToInt().ToStringEx());
                        timeSigNode.AddAttribute("denominator", timeSig.Denominator.ToInt().ToStringEx());

                    }
                }
                var meta = GuitarTrackG5.GetEvents().Where(x => x.IsMetaEvent() && !x.IsTempoTimesigEvent());
                if (meta.Any())
                {
                    var metaEvents = track.AddNode("metaEvents");
                    foreach (var ev in meta)
                    {
                        var te = metaEvents.AddNode("metaEvent");
                        te.AddAttribute("startTime", GuitarTrackG5.TickToTime(ev.AbsoluteTicks).ToStringEx());
                        te.AddAttribute("startTick", ev.AbsoluteTicks.ToStringEx());

                        te.AddAttribute("metaType", ev.MetaType.ToString());
                        ev.Text.IfNotEmpty(x => te.AddAttribute("text", x));
                    }
                }
            }

            return doc;
        }
        private void extractXMLSerializeChords6(XmlNode root)
        {
            if (!ProGuitarTrack.Messages.Chords.Any())
                return;

            var chords = root.GetNode("chords");
            if (chords == null)
                chords = root.AddNode("chords");

            foreach (var ev in ProGuitarTrack.Messages.Chords.ToList())
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
                if (ev.HasSlide)
                {
                    chord.AddAttribute("isSlide", ev.HasSlide.ToStringEx());
                }
                if (ev.HasSlideReversed)
                {
                    chord.AddAttribute("isSlideReversed", ev.HasSlideReversed.ToStringEx());
                }
                if (ev.HasHammeron)
                {
                    chord.AddAttribute("isHammeron", ev.HasHammeron.ToStringEx());
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
                var chords = root.GetNode("chords");
                if (chords == null)
                    chords = root.AddNode("chords");

                foreach (var ev in EditorG5.Messages.Chords.ToList())
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
                ImportFile(x, new List<string>()));
        }

        private void cONPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenFileDlg("Select Con File", DefaultMidiFileLocationPro, "").IfNotEmpty(x =>
                ImportFile(x, new List<string>()));
        }

        private void checkBoxBatchProcessIncomplete_CheckedChanged(object sender, EventArgs e)
        {
            refreshSongListSelection();
        }

        private void checkBoxBatchProcessIncomplete_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxBatchProcessFinalized_CheckedChanged(object sender, EventArgs e)
        {
            refreshSongListSelection();
        }

        void refreshSongListSelection()
        {


        }

        private void buttonSongUtilSearchFolderExplore_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBoxSongUtilSearchFolder.Text);
        }

        private void buttonSongUtilSearchForG5FromOpenPro_Click(object sender, EventArgs e)
        {
            FindMatchingG5ForPro();


        }

        private void FindMatchingG5ForPro()
        {
            var searchPaths = new List<string>();
            if (textBoxSongUtilSearchFolder.Text.FolderExists())
                searchPaths.Add(textBoxSongUtilSearchFolder.Text.AppendSlashIfMissing());

            if (DefaultMidiFileLocationG5.FolderExists())
                searchPaths.Add(DefaultMidiFileLocationG5.AppendSlashIfMissing());

            if (DefaultMidiFileLocationPro.FolderExists())
                searchPaths.Add(DefaultMidiFileLocationPro.AppendSlashIfMissing());

            searchPaths = searchPaths.Distinct().Where(x => x.IsNotEmpty()).ToList();

            var resultFiles = new List<string>();

            var selectedSongs = listBoxSongLibrary.SelectedItems.ToEnumerable<SongCacheItem>().ToList();

            searchPaths.ForEach(x =>
            {
                resultFiles.AddRange(x.GetFilesInFolder(true, "*.mid").Where(f => !resultFiles.Contains(f)).ToList());
            });

            resultFiles = resultFiles.Where(x => SongList.Any(s =>
                x.GetFileNameWithoutExtension().StartsWith(s.G6FileName.GetFileNameWithoutExtension().Substring(0,
                Math.Min(6, s.G6FileName.GetFileNameWithoutExtension().Length))) && s.G5FileName.IsEmpty())).ToList();

            resultFiles = resultFiles.OrderBy(x => x.GetFileModifiedTime()).ToList();


            var sb = new StringBuilder();

            var midiG5Files = new List<KeyValuePair<string, Sequence>>();
            resultFiles.ForEach(res =>
            {
                try
                {
                    var seq = res.LoadSequenceFile();
                    if (seq != null)
                    {
                        if (seq.Tracks.Any(x => x.Name.IsGuitarTrackName5() || x.Name.IsBassTrackName5()))
                        {
                            midiG5Files.Add(new KeyValuePair<string, Sequence>(res, seq));
                        }
                    }
                }
                catch { }
            });

            sb = new StringBuilder();
            midiG5Files.ForEach(x => sb.AppendLine(x.Key));
            if (sb.ToString().IsNotEmpty())
            {
                OpenNotepad(sb.ToString().GetBytes());
            }

        }

        private void buttonNoteUtilSelectAll_Click(object sender, EventArgs e)
        {
            EditorPro.SelectAllChords();
        }

        private void textBoxSongLibListFilter_TextChanged(object sender, EventArgs e)
        {
            ApplySongFilter(textBoxSongLibListFilter.Text);
        }



        public void ApplySongFilter(string filter)
        {
            SongList.SongListFilter = filter;
            SongList.PopulateList();
        }

        private void buttonSongLibListFilterReset_Click(object sender, EventArgs e)
        {
            textBoxSongLibListFilter.Text = "";
            ApplySongFilter("");
        }


        private void radioSongLibSongListSortName_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSongLibSongListSortName.Checked)
            {
                if (SongList.SortMode != SongListSortMode.SortByName)
                {
                    SongList.SortMode = SongListSortMode.SortByName;
                    SongList.PopulateList();
                }
            }
        }

        private void radioSongLibSongListSortID_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSongLibSongListSortID.Checked)
            {
                if (SongList.SortMode != SongListSortMode.SortByID)
                {
                    SongList.SortMode = SongListSortMode.SortByID;
                    SongList.PopulateList();
                }
            }
        }

        private void radioSongLibSongListSortCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSongLibSongListSortCompleted.Checked)
            {
                if (SongList.SortMode != SongListSortMode.SortByCompleted)
                {
                    SongList.SortMode = SongListSortMode.SortByCompleted;
                    SongList.PopulateList();
                }
            }
        }

        private void checkBoxSongLibSongListSortAscending_CheckedChanged(object sender, EventArgs e)
        {
            if (SongList.SortAscending != checkBoxSongLibSongListSortAscending.Checked)
            {
                SongList.SortAscending = checkBoxSongLibSongListSortAscending.Checked;
                SongList.PopulateList();
            }
        }

        private void saveProXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedSong == null)
            {
                MessageBox.Show("No Song Loaded");
            }
            else
            {
                ShowSaveFileDlg("Save Pro XML", "", "").IfNotEmpty(fileName =>
                {
                    try
                    {
                        var doc = GenerateXml(SelectedSong);
                        doc.Save(fileName);
                    }
                    catch { }
                });
            }
        }

        private void radioGridHalfNote_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void radioGridWholeNote_CheckedChanged(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CheckNotesGridSelection();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                EditorPro.Invalidate();
                EditorG5.Invalidate();
            }
        }





        private void midiExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = ShowOpenMidiFile();
            if (!file.IsEmpty() && file.FileExists())
                ImportTabSiteExportXml(file);
        }

        private void ImportTabSiteExportXml(string file)
        {

            ExecAndRestoreTrackDifficulty(delegate()
            {
                try
                {

                    var seq = ConvertWebTabToPro(file);
                    ShowImportManipPopup(seq, EditorPro);
                }
                catch
                {

                }
            });

        }

        private void ShowImportManipPopup(Sequence seq, TrackEditor editorPro)
        {
            if (seq != null && seq.Count > 1)
            {
                var popup = new PEPopupWindow();
                var tabImport = new PEWebTabImportPanel(seq);
                popup.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                popup.SetControl(tabImport, EditorPro);
                popup.Text = "Web Tab Import";

                popup.FormClosed += (sender, e) =>
                {
                    if (popup != null && popup.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        seq = tabImport.Sequence;

                        var saveFileName = ShowSaveFileDlg("Save File", DefaultMidiFileLocationPro, "");
                        if (!saveFileName.IsEmpty())
                        {
                            TryWriteFile(saveFileName, seq.Save().GetBytes(true));

                            if (EditorPro.LoadMidi17(saveFileName, ReadFileBytes(saveFileName), false))
                            {
                                if (SelectedSong != null && SelectedSong.G6FileName.IsEmpty())
                                {
                                    SelectedSong.G6FileName = saveFileName;
                                    textBoxSongLibProMidiFileName.Text = saveFileName;
                                }
                            }
                        }
                    }
                };
                popup.Show(this);
            }
        }


        private Sequence ConvertWebTabToPro(string file)
        {
            var doc = Encoding.ASCII.GetString(ReadFileBytes(file)).ToXmlDocument();

            var seq = new Sequence(FileType.Pro, EditorG5.Sequence.Division);

            seq.AddTempo(EditorG5.GuitarTrack.GetTempoTrack().Clone());

            var names = XMLUtil.GetNodeList(doc, "//part-list/score-part/score-instrument/instrument-name").Select(x => XMLUtil.GetNodeValue(x)).ToArray();

            var partNodes = XMLUtil.GetNodeList(doc, "//score-partwise/part");
            foreach (var partNode in partNodes)
            {
                Track inst = new Track(FileType.Pro, names[partNodes.IndexOf(partNode)]);

                var measures = XMLUtil.GetNodeList(partNode, "measure");

                var tcb = new TempoChangeBuilder();

                var lastUpTick = EditorG5.Messages.Chords.Last().UpTick;
                var firstDownTick = EditorG5.Messages.Chords.First().DownTick;
                var tickOffset = 0;
                bool foundFirstNote = false;

                var attrib = new TabSiteMeasureAttribute();
                foreach (var measure in measures)
                {
                    TabSiteMeasureAttribute.FromMeasure(ref attrib, measure);

                    var division = (double)attrib.Divisions;

                    var measureNotes = XMLUtil.GetNodeList(measure, "note");
                    foreach (var note in measureNotes)
                    {
                        var duration = XMLUtil.GetNodeValue(note, "duration").ToDouble();

                        var actualNotes = XMLUtil.GetNodeValue(note, "time-modification/actual-notes").ToInt();
                        var normalNotes = XMLUtil.GetNodeValue(note, "time-modification/normal-notes").ToInt();

                        var noteString = XMLUtil.GetNodeValue(note, "notations/technical/string").ToInt();
                        var noteFret = XMLUtil.GetNodeValue(note, "notations/technical/fret").ToInt();

                        var rest = XMLUtil.GetNode(note, "rest");
                        var chordNode = XMLUtil.GetNode(note, "chord");


                        var numTicks = calculateTicks(seq, division, duration);

                        if (noteString.IsNull() == false && noteFret.IsNull() == false &&
                            noteFret < 25 && noteFret >= 0 && noteString >= 1 && noteString <= 6)
                        {
                            if (rest == null && noteFret.IsNull() == false && noteString.IsNull() == false)
                            {
                                var data1 = Utility.ExpertData1Strings[attrib.StaffLines - noteString];

                                if (foundFirstNote == false)
                                {
                                    foundFirstNote = true;
                                }
                                if (chordNode != null)
                                {
                                    inst.Insert(tickOffset - numTicks, new ChannelMessage(ChannelCommand.NoteOn, data1, 100 + noteFret, 0));
                                    inst.Insert(tickOffset, new ChannelMessage(ChannelCommand.NoteOff, data1, 0, 0));
                                }
                                else
                                {
                                    inst.Insert(tickOffset, new ChannelMessage(ChannelCommand.NoteOn, data1, 100 + noteFret, 0));
                                    inst.Insert(tickOffset + numTicks, new ChannelMessage(ChannelCommand.NoteOff, data1, 0, 0));
                                }
                            }
                        }
                        if (chordNode == null)
                        {
                            tickOffset += numTicks;
                        }

                    }
                }

                if (inst != null && inst.ChanMessages != null && inst.ChanMessages.Count() > 5)
                {
                    seq.Add(inst);
                }
            }
            return seq;
        }

        private static int calculateTicks(Sequence seq, double division, double duration)
        {

            var numTicks = ((duration * ((double)seq.Division) / (division))).Round();
            return numTicks;
        }



        private void buttonFixOverlappingNotes_Click(object sender, EventArgs e)
        {
            if (EditorPro.IsLoaded)
            {
                try
                {
                    EditorPro.BackupSequence();
                    EditorPro.Messages.Chords.ToList().ForEach(x => x.SnapEvents());
                    EditorPro.Invalidate();
                }
                catch { }
            }
        }

        private void buttonSelectForward_Click(object sender, EventArgs e)
        {
            if (EditorPro.IsLoaded)
            {
                try
                {
                    EditorPro.Messages.Chords.Where(x => x.DownTick > EditorPro.SelectedChords.GetTickPair().Down).ToList().ForEach(x => x.Selected = true);

                    EditorPro.Invalidate();
                }
                catch { }
            }
        }
        private void buttonSelectBack_Click(object sender, EventArgs e)
        {
            if (EditorPro.IsLoaded)
            {
                try
                {
                    EditorPro.Messages.Chords.Where(x => x.DownTick < EditorPro.SelectedChords.GetTickPair().Down).ToList().ForEach(x => x.Selected = true);

                    EditorPro.Invalidate();
                }
                catch { }
            }
        }

        private void mergeProMidiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowOpenMidiFile().IfNotEmpty(fileName =>
                {
                    var seq = new Sequence(FileType.Pro, EditorPro.Sequence.Division);
                    seq.Load(fileName);

                    for (int x = 1; x < seq.Count; x++)
                        EditorPro.Sequence.Add(seq[x]);

                    RefreshTracks();
                });
            }
            catch { }
        }

        void button11_Click(object sender, System.EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;

            var down = textBoxNoteEditorSelectedChordDownTick.Text.ToInt();
            var up = textBoxNoteEditorSelectedChordUpTick.Text.ToInt();
            var len = textBoxNoteEditorSelectedChordTickLength.Text.ToInt();

            if (EditorPro.NumSelectedChords == 1)
            {
                if (down.IsNotNull() && up.IsNotNull())
                {
                    var chord = EditorPro.SelectedChord;

                    chord.SetTicks(new TickPair(down, up));
                    chord.UpdateEvents();
                }
            }
            else if (EditorPro.NumSelectedChords > 1)
            {
                if (len.IsNotNull())
                {
                    EditorPro.SelectedChords.ToList().ForEach(x =>
                    {
                        x.SetTicks(new TickPair(x.DownTick, x.DownTick + len));
                        x.UpdateEvents();
                    });
                }
            }

        }

        private void tabPackageViewer_FileDrop(object sender, DragEventArgs e)
        {
            try
            {
                var fileNames = e.Data.GetData("FileDrop") as string[];
                if (fileNames != null && fileNames.Length > 0)
                {
                    foreach (var fileName in fileNames)
                    {
                        var pk = Package.Load(fileName);
                        if (pk != null)
                        {
                            LoadPackageIntoTree(pk, fileName, true);
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void tabPage8_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void tabPackageViewer_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void guitarProToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var file = ShowOpenFileDlg("Import Guitar Pro File", "", "");
                if (!file.IsEmpty())
                {
                    using (var fs = File.OpenRead(file))
                    {
                        var data = PhoneGuitarTab.Core.TabFactory.CreateFromGp(fs);

                        Sequence seq = new Sequence(FileType.Pro, EditorG5.Sequence.Division);

                        seq.AddTempo(TrackEditor.CopyTrack(EditorG5.GuitarTrack.GetTempoTrack(), "Tempo"));

                        foreach (var track in data.Tracks)
                        {
                            Track seqTrack = new Track(FileType.Pro, track.Name);
                            var position = 0;
                            foreach (var measure in track.Measures)
                            {

                                foreach (var beat in measure.Beats)
                                {
                                    double duration = 0;
                                    switch (beat.Duration)
                                    {
                                        case PhoneGuitarTab.Tablature.Duration.Whole:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.Whole);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.Half:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.Half);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.Quarter:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.Quarter);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.Eighth:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.Eight);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.Sixteenth:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.Sixteenth);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.ThirtySecond:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.ThirtySecond);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.SixtyFourth:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.SixtyFourth);
                                            break;
                                        case PhoneGuitarTab.Tablature.Duration.HundredTwentyEighth:
                                            duration = EditorG5.GuitarTrack.GetTempo(position).GetTicksPerBeat(TimeUnit.OneTwentyEigth);
                                            break;
                                    }


                                    foreach (var note in beat.Notes)
                                    {
                                        int noteString = 5 - beat.Notes.IndexOf(note);
                                        if (!note.Fret.IsEmpty())
                                        {

                                            seqTrack.Insert(position, new ChannelMessage(ChannelCommand.NoteOn, Utility.ExpertData1LowE + noteString,
                                                100 + note.Fret.ToInt()));

                                            seqTrack.Insert(position + duration.Floor(), new ChannelMessage(ChannelCommand.NoteOff, Utility.ExpertData1LowE + noteString,
                                                0));
                                        }
                                    }

                                    position += duration.Ceiling();
                                }
                            }
                            if (seqTrack.ChanMessages.Count() > 5)
                                seq.Add(seqTrack);
                        }

                        ShowImportManipPopup(seq, EditorPro);
                    }
                }
            }
            catch { }
        }

        private void treePackageContents_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var pk = (treePackageContents.Tag as Package);
                if (pk != null && treePackageContents.SelectedNode != null &&
                    treePackageContents.SelectedNode.Tag != null)
                {

                    if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                    {
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


                        if (treePackageContents.SelectedNode.Tag is PackageFile)
                        {
                            var pkFile = treePackageContents.SelectedNode.Tag as PackageFile;
                            if (pkFile != null && files.Count() == 1)
                            {
                                Package.AddFileToFolder(pkFile.Package, pkFile.Folder, files.First().GetFileName(), files.First().ReadFileBytes());
                            }
                        }
                        else if (treePackageContents.SelectedNode.Tag is PackageFolder)
                        {
                            var pkFolder = treePackageContents.SelectedNode.Tag as PackageFolder;
                            if (pkFolder != null)
                            {
                                foreach (var file in files)
                                {
                                    Package.AddFileToFolder(pkFolder.Package, pkFolder, file.GetFileName(), file.ReadFileBytes());
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void treePackageContents_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void treePackageContents_DragOver(object sender, DragEventArgs e)
        {
            if (treePackageContents.SelectedNode == null ||
                treePackageContents.Tag == null ||
                e.Data.GetDataPresent(DataFormats.FileDrop, false) == false ||
                (treePackageContents.Tag is Package) == false)
            {
                e.Effect = DragDropEffects.None;

            }
            else
            {
                e.Effect = DragDropEffects.All;
            }

        }

        private void buttonPackageViewerSave_Click(object sender, EventArgs e)
        {
            if (treePackageContents.Tag == null ||
                (treePackageContents.Tag is Package) == false)
            {
                return;
            }

            var pk = (treePackageContents.Tag as Package);

            var bytes = Package.RebuildPackageInMemory(pk);
            if (bytes == null)
            {
                MessageBox.Show("Cannot rebuild package");
            }
            else
            {
                if (treePackageContentsIsFATXFileEntry)
                {
                    WriteUSBFile(treePackageContentsFATXFileEntry.Parent,
                        treePackageContentsFATXFileEntry.Name,
                        treePackageContentsFATXFileEntry);
                }
                else if (treePackageContentsIsLocal)
                {
                    File.WriteAllBytes(treePackageContentsFilePath, bytes);

                    var pk2 = Package.Load(treePackageContentsFilePath, true);
                    LoadPackageIntoTree(pk2, treePackageContentsFilePath, treePackageContentsIsLocal);

                }

            }

        }

        private void toolStripPackageEditorDeleteFile_Click(object sender, EventArgs e)
        {
            if (treePackageContents.SelectedNode != null &&
                    treePackageContents.SelectedNode.Tag != null && treePackageContents.SelectedNode.Tag is PackageFile)
            {
                var pk = treePackageContents.Tag as Package;
                var pkf = treePackageContents.SelectedNode.Tag as PackageFile;
                bool reopened = false;
                if (pkf.Package.package.CanWrite == false)
                {
                    reopened = pkf.Package.package.OpenAgain();
                }
                pkf.FileEntry.Delete();

                if (reopened)
                {
                    pkf.Package.package.CloseIO();
                }

                if (treePackageContentsIsFATXFileEntry)
                {
                }
                else
                {
                    LoadPackageIntoTree(pk, treePackageContentsFilePath, treePackageContentsIsLocal);
                }
            }
        }

        private void contextToolStripPackageEditor_Opening(object sender, CancelEventArgs e)
        {
            toolStripPackageEditorDeleteFile.Enabled = (treePackageContents.SelectedNode != null &&
                    treePackageContents.SelectedNode.Tag != null && treePackageContents.SelectedNode.Tag is PackageFile);

        }





        private void buttonSongUtilFindInFileSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var config = GetSongUtilFindInFileSearchConfigFromScreen();

                SongUtilFindMatchesInFile(config);
            }
            catch { }
        }

        private SongUtilFindInFileConfig GetSongUtilFindInFileSearchConfigFromScreen()
        {
            var config = new SongUtilFindInFileConfig();
            config.FirstMatchOnly = checkBoxSongUtilFindInFileFirstMatchOnly.Checked;
            config.MatchCountOnly = checkBoxSongUtilFindInFileMatchCountOnly.Checked;
            config.SelectedSongOnly = checkBoxSongUtilFindInFileSelectedSongOnly.Checked;
            config.OpenResults = checkBoxSongUtilFindInFileResultsOpenCompleted.Checked;
            config.MatchWholeWord = checkBoxSongUtilFindInFileMatchWholeWord.Checked;
            config.RootFolder = textBoxSongUtilFindFolder.Text;
            config.FindInProOnly = checkBoxSongUtilFindInProOnly.Checked;
            if (config.RootFolder.FolderExists() == false)
            {
                if (DefaultMidiFileLocationPro.IsNotEmpty() && DefaultMidiFileLocationPro.FolderExists())
                {
                    config.RootFolder = DefaultMidiFileLocationPro;
                }
                else if (DefaultMidiFileLocationG5.IsNotEmpty() && DefaultMidiFileLocationG5.FolderExists())
                {
                    config.RootFolder = DefaultMidiFileLocationG5;
                }
            }
            if (config.RootFolder.IsNotEmpty())
            {
                config.RootFolder = config.RootFolder.AppendSlashIfMissing();
                if (config.RootFolder.FolderExists() == false)
                {
                    config.RootFolder = string.Empty;
                }
            }
            config.FindData1 = textBoxSongUtilFindInFileData1.Text.ToInt();
            config.FindData2 = textBoxSongUtilFindInFileData2.Text.ToInt();
            config.FindChannel = textBoxSongUtilFindInFileChan.Text.ToInt();
            config.FindText = textBoxSongUtilFindInFileText.Text.Trim();
            config.FindDistinctText = false;
            return config;
        }

        private void SongUtilFindMatchesInFile(SongUtilFindInFileConfig config)
        {

            textBoxSongUtilFindInFileResults.Text = "Starting Search";
            Application.DoEvents();


            var root = config.RootFolder.AppendSlashIfMissing();
            if (root.FolderExists())
            {
                var midiFiles = new List<string>();

                var results = new List<SongUtilSearchResult>();

                if (config.SelectedSongOnly)
                {
                    if (SongList.MultiSelectedSongs != null && SongList.MultiSelectedSongs.Any())
                    {
                        var l = SongList.MultiSelectedSongs.Where(x => x.G6FileName.IsNotEmpty() && x.G6FileName.FileExists()).ToList();
                        if (l.Any())
                        {
                            midiFiles = l.Select(v => v.G6FileName).GroupBy(v => v.GetFileName().ToLower()).Select(v => v.First()).ToList();
                        }
                        else
                        {
                            l = SongList.MultiSelectedSongs.Where(x => x.G5FileName.IsNotEmpty() && x.G5FileName.FileExists()).ToList();
                            if (l.Any())
                            {
                                midiFiles = l.Select(v => v.G5FileName).GroupBy(v => v.GetFileName().ToLower()).Select(v => v.First()).ToList();
                            }
                        }
                    }
                }
                else
                {
                    midiFiles = root.GetFilesInFolder(true, "*.mid").ToList();
                    midiFiles.AddRange(root.GetFilesInFolder(true, "*.midi").ToList());
                    midiFiles = midiFiles.GroupBy(v => v.GetFileName().ToLower()).Select(v => v.First()).ToList();
                }

                for (int idx = 0; idx < midiFiles.Count; idx++)
                {
                    var mf = midiFiles[idx];
                    var sbLog = new StringBuilder();
                    sbLog.AppendLine("Searching: " + (idx + 1) + "/" + midiFiles.Count);
                    sbLog.AppendLine(mf);
                    textBoxSongUtilFindInFileResults.Text = sbLog.ToString();

                    Application.DoEvents();

                    var res = new SongUtilSearchResult(mf);
                    var seq = mf.LoadSequenceFile();
                    if (seq == null)
                        continue;

                    using (seq)
                    {
                        var tracks = seq.Tracks.ToList();

                        if (config.FindInProOnly)
                        {
                            tracks = tracks.Where(x => x.Name.IsProTrackName()).ToList();
                        }

                        if (config.FindDistinctText)
                        {
                            var metaTracks = tracks.Where(v => v.Meta.Any()).ToList();
                            foreach (var track in metaTracks)
                            {
                                var meta = track.Meta.Where(m => m.Text.IsNotEmpty()).ToList();
                                if (meta.Any())
                                {
                                    res.Matches.AddRange(meta.Select(x => new SongUtilSearchResultItem(track.Name, x)));
                                }
                            }
                        }
                        else
                        {
                            var txt = config.FindText.ToLower();
                            if (txt.IsNotEmpty())
                            {
                                var metaTracks = tracks.Where(v => v.Meta.Any()).ToList();
                                foreach (var track in metaTracks)
                                {
                                    var meta = track.Meta.Where(m => m.Text.IsNotEmpty()).ToList();

                                    if (meta.Any())
                                    {
                                        var mt = new List<MidiEvent>();
                                        if (config.MatchWholeWord)
                                        {
                                            mt.AddRange(meta.Where(v => v.Text.EqualsEx(txt)).ToList());
                                        }
                                        else
                                        {
                                            mt.AddRange(meta.Where(v => v.Text.ToLower().Contains(txt)).ToList());
                                        }
                                        if (mt.Any())
                                        {
                                            res.Matches.AddRange(mt.Select(x => new SongUtilSearchResultItem(track.Name, x)));
                                        }
                                    }
                                }
                            }


                            if (config.FindData1.IsNull() == false || config.FindData2.IsNull() == false || config.FindChannel.IsNull() == false)
                            {
                                tracks = tracks.Where(cm => cm.ChanMessages.Any()).ToList();

                                foreach (var track in tracks)
                                {

                                    var cmlist = track.ChanMessages.Where(cm =>
                                        (config.FindData1.IsNull() ? true : cm.Data1 == config.FindData1) &&
                                        (config.FindData2.IsNull() ? true : cm.Data2 == config.FindData2) &&
                                        (config.FindChannel.IsNull() ? true : cm.Channel == config.FindChannel)
                                        );
                                    if (cmlist.Any())
                                    {
                                        if (config.FirstMatchOnly)
                                        {
                                            res.Matches.Add(new SongUtilSearchResultItem(track.Name, cmlist.First()));

                                        }
                                        else
                                        {
                                            res.Matches.AddRange(cmlist.ToList().Select(x => new SongUtilSearchResultItem(track.Name, x)));

                                        }
                                    }
                                }
                            }

                        }

                        if (res.Matches.Any())
                        {
                            results.Add(res);
                        }
                    }
                }


                if (!results.Any())
                {
                    textBoxSongUtilFindInFileResults.Text = "No Results";
                }
                else
                {
                    textBoxSongUtilFindInFileResults.Text = "";
                    var sb = new StringBuilder();

                    if (config.FindDistinctText)
                    {

                        var resList = results.SelectMany(vm => vm.Matches.Select(v => v.Event.Text)).ToList().Distinct().ToList();
                        resList.OrderBy(v => v).ToList().ForEach(r => sb.AppendLine(r));

                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("");
                        sb.AppendLine("--Match Breakdown--");

                        foreach (var res in results)
                        {
                            sb.AppendLine(res.MidiPath);

                            foreach (var trackGroup in res.Matches.GroupBy(o => o.TrackName))
                            {

                                sb.Append("\t");
                                sb.AppendLine(trackGroup.Key ?? "");
                                if (config.MatchCountOnly)
                                {
                                    sb.Append("\t\t");
                                    sb.Append(trackGroup.Count());
                                    sb.AppendLine(" Matches");
                                }
                                else
                                {

                                    foreach (var match in trackGroup)
                                    {
                                        sb.Append("\t\t");
                                        sb.Append(match.Event.AbsoluteTicks);
                                        sb.Append(" ");
                                        sb.Append(match.Event.MetaType);
                                        sb.Append(" ");
                                        sb.AppendLine(match.Event.ToString());
                                    }
                                }
                            }

                            sb.AppendLine();
                        }

                    }
                    else
                    {
                        foreach (var res in results)
                        {
                            sb.AppendLine(res.MidiPath);

                            foreach (var trackGroup in res.Matches.GroupBy(o => o.TrackName))
                            {
                                sb.Append("\t");
                                sb.AppendLine(trackGroup.Key ?? "");
                                if (config.MatchCountOnly)
                                {
                                    sb.Append("\t\t");
                                    sb.Append(trackGroup.Count());
                                    sb.AppendLine(" Matches");
                                }
                                else
                                {
                                    foreach (var match in trackGroup)
                                    {
                                        sb.Append("\t\t");
                                        sb.AppendLine(match.ToString());
                                    }
                                }
                            }

                            sb.AppendLine();
                        }
                    }
                    textBoxSongUtilFindInFileResults.Text = sb.ToString();

                    if (config.OpenResults)
                    {
                        OpenSongUtilSearchResultsNotepad();
                    }
                }
            }
        }

        private void buttonSongUtilFindInFileResultsOpenWindow_Click(object sender, EventArgs e)
        {
            OpenSongUtilSearchResultsNotepad();
        }

        private void OpenSongUtilSearchResultsNotepad()
        {
            if (textBoxSongUtilFindInFileResults.Text.IsNotEmpty())
            {
                OpenNotepad(Encoding.ASCII.GetBytes(textBoxSongUtilFindInFileResults.Text));
            }
        }

        private void buttonSongUtilFindInFileDistinctText_Click(object sender, EventArgs e)
        {
            try
            {
                var config = GetSongUtilFindInFileSearchConfigFromScreen();
                config.FindDistinctText = true;

                SongUtilFindMatchesInFile(config);
            }
            catch { }
        }

        private void buttonAddTextEvent_Click_1(object sender, EventArgs e)
        {
            if (!EditorPro.IsLoaded)
                return;
            try
            {

                EditorPro.BackupSequence();

                var tick = textBoxEventTick.Text.ToInt();
                var text = textBoxEventText.Text.Trim();
                if (tick.IsNull())
                {
                    MessageBox.Show("Invalid Tick");
                }
                else if (text.IsEmpty())
                {
                    MessageBox.Show("Invalid text");
                }
                else
                {
                    var te = GuitarTextEvent.CreateTextEvent(EditorPro.Messages, tick, text);

                    RefreshTextEvents();

                    listTextEvents.SetSelectedItem(te);

                }

            }
            catch { }
        }

        private void buttonSongToolSnapNotes_Click(object sender, EventArgs e)
        {
            if (EditorPro.IsLoaded && EditorG5.IsLoaded)
            {
                int count = 0;
                ExecAndRestoreTrackDifficulty(() =>
                {
                    try
                    {
                        EditorPro.BackupSequence();

                        var config = new
                        {
                            SelectedTrackOnly = checkBoxInitSelectedTrackOnly.Checked,
                            SelectedDiffOnly = checkBoxInitSelectedDifficultyOnly.Checked,
                            CopyGuitarToBass = SelectedSong != null && SelectedSong.CopyGuitarToBass,
                        };

                        if (EditorPro.SelectedTrack == null)
                            return;

                        var currTrackNamePro = EditorPro.SelectedTrack.Name;

                        var difficulties = GuitarDifficulty.EasyMediumHardExpert;
                        if (config.SelectedDiffOnly)
                        {
                            difficulties = EditorPro.CurrentDifficulty;
                        }


                        var proNames = EditorPro.Sequence.GetGuitarBassTracks().Select(x => x.Name).ToList();
                        if (config.SelectedTrackOnly)
                        {
                            proNames.Clear();
                            proNames.Add(EditorPro.MidiTrack.Name);
                        }
                        proNames.ToList().ForEach(ep =>
                        {
                            foreach (var currentDifficulty in difficulties.GetDifficulties())
                            {

                                if (config.SelectedTrackOnly && ep != ProGuitarTrack.Name)
                                    return;

                                EditorPro.SetTrack6(EditorPro.GetTrack(ep), currentDifficulty);

                                var valid = false;
                                if (ep.IsGuitarTrackName() || config.CopyGuitarToBass)
                                {
                                    valid = EditorG5.SetTrack5(EditorG5.GetGuitar5MidiTrack(), currentDifficulty);
                                }
                                else if (ep.IsBassTrackName())
                                {
                                    valid = EditorG5.SetTrack5(EditorG5.GetGuitar5BassMidiTrack(), currentDifficulty);
                                }
                                if (valid)
                                {
                                    EditorPro.Messages.Chords.ToList().ForEach(chord =>
                                    {

                                        var newTicks = EditorPro.SnapLeftRightTicks(chord.TickPair, new SnapConfig(true, false, false));

                                        if (chord.TickPair != newTicks)
                                        {
                                            count++;
                                            chord.SetTicks(newTicks);
                                            chord.UpdateEvents();
                                        }
                                    });
                                }
                            }
                        });

                    }
                    catch
                    {
                        MessageBox.Show("Error");
                    }
                });

                MessageBox.Show("" + count + " chords snapped");
            }
        }

        private void buttonSongUtilFindFolder_Click(object sender, EventArgs e)
        {
            OpenExplorerFolder(textBoxSongUtilFindFolder.Text);
        }

        private void checkChordNameHide_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button96_Click(object sender, EventArgs e)
        {

        }

    }
}
