using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using NAudio.Wave;

namespace ProUpgradeEditor.Common
{
    public class TrackEditor : UserControl
    {
        public Track MidiTrack
        {
            get
            {
                return GuitarTrack != null ? GuitarTrack.GetTrack() : null;
            }
        }
        public bool IsLoaded
        {
            get { return guitarTrack != null && guitarTrack.IsLoaded; }
        }
        public bool ViewLyrics
        {
            get;
            set;
        }

        public bool EnableRenderMP3Wave
        {
            get;
            set;
        }
        public int MP3PlaybackOffset { get; set; }


        public void AddTrack(Track t)
        {
            if (IsLoaded)
                Sequence.Add(t);
        }

        TrackEditor ptrEditor;
        public TrackEditor Editor5
        {
            get
            {
                return EditorType == EEditorType.Guitar5 ? this : ptrEditor;
            }
            set
            {
                ptrEditor = value;
            }
        }
        public TrackEditor EditorPro
        {
            get
            {
                return EditorType == EEditorType.ProGuitar ? this : ptrEditor;
            }
            set
            {
                ptrEditor = value;
            }
        }

        public CopyChordList CopyChords;

        public TrackEditor()
        {
            if (!DesignMode)
            {
                this.DoubleBuffered = true;
                this.PlaybackPosition = 0;

                this.InPlayback = false;
                IsMouseOver = false;
                ShowNotesGrid = false;
                GridScalar = 0.25;
                CurrentSelectionState = EditorSelectionState.Idle;

                Control.CheckForIllegalCrossThreadCalls = true;
                CopyChords = new CopyChordList(this);

                HScroll = new HScrollBar();
                HScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
                HScroll.SmallChange = Utility.HScrollSmallChange;
                HScroll.LargeChange = Utility.HScrollLargeChange;

                this.Controls.Add(HScroll);


            }
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (IsLoaded)
            {
                var mouseDelta = e.Delta;

                var c = guitarTrack.Messages.Chords.GetBetweenTick(GetTickFromClientPoint(new TickPair(0, Width)));
                if (c.Any())
                {
                    if (!c.Any(x => x.Selected))
                    {
                        ClearSelection();
                        c.First().Selected = true;
                    }
                }
                if (OnZoom != null)
                {
                    OnZoom(this, (((double)mouseDelta) / 120.0).Ceiling());
                }

            }
        }

        public GuitarMessageList Messages
        {
            get { return guitarTrack == null ? null : guitarTrack.Messages; }
        }



        new HScrollBar HScroll;

        public void AddScrollHandler()
        {
            HScroll.ValueChanged += delegate(object o, EventArgs e)
            {

                try
                {
                    var value = HScrollValue;
                    if (this == Editor5)
                    {
                        if (EditorPro.HScrollValue != value)
                        {
                            EditorPro.HScrollValue = value;
                        }
                    }
                    else if (this == EditorPro)
                    {
                        if (Editor5.HScrollValue != value)
                        {
                            Editor5.HScrollValue = value;
                        }
                    }

                    UpdateSelectorVisibility();
                    Invalidate();
                }
                catch { }
            };

        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (DesignMode)
                return;
            EditorPro.Invalidate();
            Editor5.Invalidate();
            base.OnVisibleChanged(e);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HScrollValue
        {
            get
            {
                return HScroll.Value;
            }
            set
            {
                try
                {

                    if (HScroll != null)
                    {

                        if (value < 0)
                            value = 0;

                        if (value < HScroll.Maximum)
                        {
                            HScroll.Value = value;
                        }
                        else
                        {
                            HScroll.Value = HScroll.Maximum;
                        }

                        Invalidate();

                    }
                }
                catch { }
            }
        }

        public void SetHScrollMaximum(int max)
        {
            if (max < 0)
                max = 0;
            if (max < HScroll.Value)
            {
                HScroll.Value = max;
            }
            HScroll.Maximum = max;
        }


        public int HScrollMaximum
        {
            get
            {
                return HScroll.Maximum;
            }
        }

        public void Initialize(bool isPro)
        {
            editorType = isPro ? EEditorType.ProGuitar : EEditorType.Guitar5;
            guitarTrack = new GuitarTrack(this, isPro);
        }

        EEditorType editorType = EEditorType.None;
        public EEditorType EditorType
        {
            get { return editorType; }
            set { editorType = value; }
        }

        public bool IsPro { get { return editorType == EEditorType.ProGuitar; } }

        public bool InPlayback
        {
            get;
            set;
        }

        public double PlaybackPosition
        {
            get;
            set;
        }

        public bool IsMouseOver
        {
            get;
            set;
        }



        public bool IsSelecting
        {
            get;
            set;
        }

        Point selectStartPoint;
        public Point SelectStartPoint
        {
            get { return selectStartPoint; }
            set
            {
                selectStartPoint = value;
            }
        }


        Point selectCurrentPoint;
        public Point SelectCurrentPoint
        {
            get { return selectCurrentPoint; }
            set { selectCurrentPoint = value; }
        }



        public Rectangle SelectionBox
        {
            get
            {
                var p1 = SelectCurrentPoint;
                var p2 = SelectStartPoint;

                int minX = p1.X < p2.X ? p1.X : p2.X;
                int maxX = p1.X < p2.X ? p2.X : p1.X;
                int minY = p1.Y < p2.Y ? p1.Y : p2.Y;
                int maxY = p1.Y < p2.Y ? p2.Y : p1.Y;
                int width = maxX - minX;
                int height = maxY - minY;

                return new Rectangle(minX, minY, width, height);

            }
        }


        bool gridSnap = true;
        public bool GridSnap
        {
            set
            {
                gridSnap = value;
            }
            get
            {
                return gridSnap &&
                    (ModifierKeys & Keys.Shift) != Keys.Shift;
            }
        }

        public bool NoteSnap
        {
            get
            {
                return NoteSnapG5 || NoteSnapG6;
            }
        }

        bool noteSnapG5 = true;

        public bool NoteSnapG5
        {
            set { noteSnapG5 = value; }
            get
            {
                return noteSnapG5 &&
                    (ModifierKeys & Keys.Shift) != Keys.Shift;
            }
        }

        bool noteSnapG6 = true;
        public bool NoteSnapG6
        {
            set { noteSnapG6 = value; }
            get
            {
                return noteSnapG6 &&
                    (ModifierKeys & Keys.Shift) != Keys.Shift;
            }
        }

        public enum EEditorType
        {
            Guitar5,
            ProGuitar,
            None,
        }

        public enum EEditMode
        {
            Unknown,
            Events,
            Modifiers,
            Chords,
        }

        EEditMode editMode = EEditMode.Chords;
        public EEditMode EditMode
        {
            get { return editMode; }
            set { editMode = value; }
        }



        public void Close()
        {
            LoadedFileName = string.Empty;

            this.backupSequences.Clear();
            if (this.EditorType == EEditorType.ProGuitar)
            {
                SetTrack6(null);
            }
            else
            {
                SetTrack5(null);
            }
            ClearBackups();

            if (OnClose != null)
            {
                OnClose(this);
            }

        }

        public bool CreatedBackup = false;

        public int NumBackups { get { return backupSequences.Count; } }


        bool show108Events = false;
        public bool Show108Events
        {
            get
            {
                return show108Events;
            }
            set
            {
                show108Events = value;
                Invalidate();
            }
        }

        GuitarTrack guitarTrack;

        public GuitarTrack GuitarTrack
        {
            get
            {
                return guitarTrack;
            }
        }

        public List<Track> Tracks
        {
            get
            {
                return Sequence.ToList();
            }
        }

        public Sequence Sequence
        {
            get
            {
                if (guitarTrack == null)
                    return null;

                return guitarTrack.Sequence;
            }
        }

        public static Track CopyTrack(Track track, string newName)
        {
            Track ret = null;
            if (track != null)
            {
                try
                {
                    ret = new Track(track.FileType, newName);
                    ret.Merge(track);
                }
                catch { ret = null; }
            }
            return ret;
        }

        public static Track GetTrack(Sequence seq, string trackName)
        {
            return seq.GetTrack(trackName);
        }

        public Track GetGuitar5MidiTrack()
        {
            return GetTrack(GuitarTrack.GuitarTrackName5);
        }
        public Track GetGuitar5BassMidiTrack()
        {
            return GetTrack(GuitarTrack.BassTrackName5);
        }
        public Track GetGuitar5BassElseGuitar()
        {
            return GetTrack(GuitarTrack.BassTrackName5).GetIfNull(() => GetGuitar5MidiTrack());
        }
        public Track GetGuitar6MidiTrack()
        {
            var ret = GetTrack(GuitarTrack.GuitarTrackName17);
            if (ret == null)
            {
                ret = GetTrack(GuitarTrack.GuitarTrackName22);
            }
            return ret;
        }
        public Track GetTrackGuitar17()
        {
            return GetTrack(GuitarTrack.GuitarTrackName17);
        }
        public Track GetTrackGuitar22()
        {
            return GetTrack(GuitarTrack.GuitarTrackName22);
        }
        public Track GetGuitar6BassMidiTrack()
        {
            var ret = GetTrack(GuitarTrack.BassTrackName17);
            if (ret == null)
            {
                ret = GetTrack(GuitarTrack.BassTrackName22);
            }
            return ret;
        }
        public Track GetTrackBass17()
        {
            return GetTrack(GuitarTrack.BassTrackName17);
        }
        public Track GetTrackBass22()
        {
            return GetTrack(GuitarTrack.BassTrackName22);
        }

        List<MemoryStream> backupSequences = new List<MemoryStream>();

        public void ClearBackups()
        {
            backupSequences.Clear();

        }

        public void BackupSequence()
        {
            if (Sequence != null)
            {
                try
                {
                    CreatedBackup = true;
                    backupSequences.Add(Sequence.Save());

                    while (backupSequences.Count > Utility.MaxBackups)
                    {
                        backupSequences.RemoveAt(0);
                    }

                }
                catch { }
            }
        }

        public string LoadedFileName { get; set; }


        bool loading5 = false;
        public bool LoadMidi5(string fileName, byte[] bytes, bool silent)
        {

            bool ret = false;
            if (!loading5)
            {
                loading5 = true;

                try
                {
                    var midiSequence5 = bytes.LoadSequence();

                    var track = midiSequence5.GetPrimaryTrackG5();
                    if (track != null)
                    {
                        this.LoadedFileName = fileName;

                        SetTrack5(midiSequence5, track, CurrentDifficulty);

                        ret = true;
                    }

                }
                catch { }

                if (!ret)
                {
                    this.LoadedFileName = string.Empty;
                    if (!silent)
                    {
                        MessageBox.Show("Cannot load file");
                    }
                }

                loading5 = false;

                ClearBackups();
                Invalidate();
            }

            return ret;
        }
        bool loading17 = false;
        public bool LoadMidi17(string fileName, byte[] fileData, bool loadingBackup)
        {
            bool ret = false;
            if (!loading17)
            {

                loading17 = true;
                string of = this.LoadedFileName;

                Track t = null;
                try
                {
                    if (fileData == null)
                        fileData = fileName.ReadFileBytes();

                    var seq = fileData.LoadSequence();


                    if (loadingBackup)
                    {
                        try
                        {
                            var gt6 = EditorPro.GuitarTrack.GetTrack();
                            if (gt6 != null && gt6.Name.IsNotEmpty())
                            {
                                t = seq.GetTrack(gt6.Name);
                            }
                        }
                        catch { }
                    }

                    if (t == null)
                    {
                        t = seq.GetPrimaryTrack();
                    }

                    if (t != null)
                    {

                        if (!loadingBackup)
                        {
                            this.LoadedFileName = fileName;
                        }
                        ret = SetTrack6(seq, t, CurrentDifficulty);
                    }

                }
                finally
                {
                    loading17 = false;
                    if (!ret)
                    {
                        this.LoadedFileName = "";
                        Close();
                    }
                }
                Invalidate();
            }

            return ret;
        }

        public void ProcessErrors(Sequence seq)
        {
            if (seq != null)
            {
                foreach (var track in seq.Tracks)
                {
                    if (track != null)
                    {
                        ProcessErrors(track);
                    }
                }
            }
        }
        public void ProcessErrors(Track track)
        {
            var meta = track.Meta.Where(x => x.Text.IsNotEmpty()).ToList().
                Where(v => v.Text.IsTrainerGuitar() || v.Text.IsTrainerBass()).ToList();
            if (meta.Any())
            {
                var nullEvents = meta.Where(t => t.Text.GetTrainerEventIndex().IsNull()).ToList();

                track.Remove(nullEvents);

                if (track.Name.IsGuitarTrackName())
                {
                    var bt = meta.Where(v => v.Text.IsTrainerBass()).ToList();
                    if (bt.Any())
                    {
                        track.Remove(bt);
                    }
                }
                else
                {
                    if (track.Name.IsBassTrackName())
                    {
                        var gt = meta.Where(v => v.Text.IsGuitarTrackName()).ToList();
                        if (gt.Any())
                        {
                            track.Remove(gt);
                        }
                    }
                }
            }
        }

        public Track GetProTrack(Sequence seq)
        {
            Track t = null;

            t = seq.GetTrack(GuitarTrack.GuitarTrackName17);
            if (t == null)
            {
                t = seq.GetTrack(GuitarTrack.GuitarTrackName22);
            }
            if (t == null)
            {
                t = seq.GetTrack(GuitarTrack.BassTrackName17);
            }
            if (t == null)
            {
                t = seq.GetTrack(GuitarTrack.BassTrackName22);
            }

            if (t == null)
            {
                t = seq.Tracks.FirstOrDefault(x => x.ChanMessages.Any());

                if (t == null && seq.Tracks.Any())
                {
                    t = seq.Tracks.First();
                }
            }
            return t;
        }

        public bool RestoreBackup()
        {

            bool ret = true;
            try
            {
                if (backupSequences.Any())
                {
                    try
                    {
                        var mso = backupSequences.Last();
                        mso.Seek(0, SeekOrigin.Begin);
                        ret = LoadMidi17("", mso.ToArray(), true);
                    }
                    catch
                    {
                        ret = false;
                    }
                    finally
                    {
                        backupSequences.Remove(backupSequences.Last());
                    }
                    Invalidate();
                }
            }
            catch (Exception)
            {
                ret = false;
            }


            return ret;
        }

        public bool SaveTrackAs(string fileName)
        {
            var ret = false;
            try
            {
                if (Sequence != null)
                {
                    var bytes = Sequence.Save().GetBytes(true);
                    if (bytes.Any())
                    {
                        if (fileName.WriteFileBytes(bytes))
                        {
                            ret = fileName.FileExists();
                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public bool SaveTrack(string fileName, bool createBackup)
        {
            var ret = false;
            try
            {
                if (Sequence != null)
                {
                    var bytes = Sequence.Save().GetBytes(true);
                    var diff = false;

                    if (fileName.FileExists())
                    {
                        var existing = fileName.ReadFileBytes();
                        if (existing.Length != bytes.Length)
                        {
                            diff = true;
                        }
                        else
                        {
                            for (int x = 0; x < existing.Length; x++)
                            {
                                var ex = existing[x];
                                var nx = bytes[x];
                                if (ex != nx)
                                {
                                    diff = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (diff || !fileName.FileExists())
                    {
                        if (diff)
                        {
                            DoCreateBackup(fileName);
                        }

                        fileName.WriteFileBytes(bytes);
                    }

                    LoadedFileName = fileName;
                    ret = fileName.FileExists();
                }
            }
            catch { }
            return ret;
        }

        void DoCreateBackup(string existingFileName)
        {

            try
            {
                var backupName = LoadedFileName.GetFolderName() +
                    existingFileName.GetFileName() + ".bak";

                File.WriteAllBytes(backupName, LoadedFileName.ReadFileBytes());
            }
            catch { }

        }
        public bool SetTrack5(Track t, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            return SetTrack5(this.Sequence == null ? t != null ? t.Sequence : null : this.Sequence, t, difficulty);
        }


        bool settingTrack5 = false;
        public bool SetTrack5(Sequence seq, Track t, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            bool ret = false;
            if (settingTrack5)
                return true;

            settingTrack5 = true;
            if (!difficulty.IsEasyMediumHardExpert())
            {
                if (!CurrentDifficulty.IsEasyMediumHardExpert())
                    difficulty = GuitarDifficulty.Expert;
                else
                    difficulty = CurrentDifficulty;
            }
            try
            {
                guitarTrack.ViewLyrics = ViewLyrics;
                guitarTrack.SetTrack(t, difficulty);

                ret = guitarTrack.IsLoaded;
            }
            catch { }

            try
            {
                if (ret == true && OnLoadTrack != null)
                {
                    OnLoadTrack(this, seq, t);
                }
            }
            catch { }

            try
            {
                SetTrackMaximum();
            }
            catch { }

            settingTrack5 = false;
            Invalidate();

            return ret;
        }

        public bool SetTrack6(Track t, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            return SetTrack6(this.Sequence == null ? t != null ? t.Sequence : null : this.Sequence, t, difficulty);
        }

        bool settingTrack6 = false;
        public bool SetTrack6(Sequence seq, Track t, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            bool ret = true;

            if (settingTrack6)
                return ret;

            settingTrack6 = true;
            try
            {
                if (!difficulty.IsEasyMediumHardExpert())
                {
                    if (!CurrentDifficulty.IsEasyMediumHardExpert())
                        difficulty = GuitarDifficulty.Expert;
                    else
                        difficulty = CurrentDifficulty;
                }
                guitarTrack.SetTrack(t, difficulty);

                ret = guitarTrack.IsLoaded;

            }
            catch { }

            try
            {
                if (ret == true && OnLoadTrack != null)
                {
                    this.OnLoadTrack(this, this.Sequence, t);
                }
            }
            catch { }
            try
            {

                SetTrackMaximum();
            }
            catch { }

            settingTrack6 = false;

            Invalidate();
            return ret;
        }

        public void SetTrackMaximum()
        {
            try
            {

                if (!IsLoaded)
                {
                    HScrollValue = 0;
                    SetHScrollMaximum(0);
                    return;
                }
                var v = HScroll.Value;
                SetHScrollMaximum((int)Math.Round(Utility.ScaleUp(GuitarTrack.TotalSongTime)));
                if (v < HScrollMaximum)
                {
                    HScrollValue = v;
                }
                else
                {
                    HScrollValue = HScrollMaximum;
                }
                Invalidate();


            }
            catch { }
        }


        public int MidiPlaybackPosition { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.IsDisposed || this.IsHandleCreated == false || this.RecreatingHandle == true)
                return;


            try
            {

                if (Width < 20 || Height < 20 || e == null || e.Graphics == null)
                {
                    return;
                }


                var g = e.Graphics;
                var clipRect = e.ClipRectangle;

                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.FillRectangle(Utility.BackgroundBrush, clipRect);

                if (IsLoaded == false)
                {
                    DrawTabLines(g, Utility.linePen);
                    return;
                }

                if (EditorType == EEditorType.Guitar5)
                {
                    Draw5Tar(e, clipRect);
                }
                else if (EditorType == EEditorType.ProGuitar)
                {
                    Draw22Tar(e, clipRect);
                }
                else
                {
                    DrawTabLines(g, Utility.linePen);
                }

                if (InPlayback)
                {
                    var pos = GetClientPointFromTick(MidiPlaybackPosition);

                    if (pos >= 0 && pos < Width)
                    {
                        using (var p = new Pen(Color.Red, 1.0f))
                        {
                            g.DrawLine(p, pos, 0, pos, Height);
                        }
                    }
                }
            }
            catch { }
        }

        public int MinVisible
        {
            get
            {
                return GetTickFromClientPoint(0);
            }
        }

        public int MaxVisible
        {
            get
            {
                return GetTickFromClientPoint(Width);
            }
        }

        public IEnumerable<GuitarChord> GetVisibleChords()
        {
            IEnumerable<GuitarChord> ret = null;
            if (IsLoaded)
            {
                ret = guitarTrack.Messages.Chords.GetBetweenTick(new TickPair(MinVisible, MaxVisible));
            }
            return ret;
        }

        void Draw5Tar(PaintEventArgs e, Rectangle clipRect)
        {
            try
            {
                if (guitarTrack == null || guitarTrack.Messages == null)
                    return;

                var g = e.Graphics;
                var visTicks = GetTickFromClientPoint(new TickPair(clipRect.Left, clipRect.Right));

                if (visTicks.IsInvalid || visTicks.IsZeroLength)
                    return;

                var visChords = guitarTrack.Messages.Chords.GetBetweenTick(visTicks).ToList();
                var visTrainer = guitarTrack.Messages.Trainers.GetBetweenTick(visTicks).ToList();
                var visText = guitarTrack.Messages.TextEvents.GetBetweenTick(visTicks).ToList().Where(x => x.Text.IsChordNameTextEvent() == false).ToList();
                var visTempo = guitarTrack.Messages.Tempos.GetBetweenTick(visTicks).ToList();
                var visTimeSig = guitarTrack.Messages.TimeSignatures.GetBetweenTick(visTicks).ToList();
                var visMod = guitarTrack.Messages.GetModifiersBetweenTick(visTicks).ToList();

                if (EditMode != EEditMode.Events)
                {
                    DrawTabLines(g, Utility.linePen);

                    DrawTrainers(g, false, visTrainer);
                    DrawTrainers(g, true, visTrainer);

                    if (!ViewLyrics)
                    {
                        DrawTextEvents(g, false, false, visText);
                    }


                    DrawModifiers6(g, visMod);
                    DrawTransparentBackgroundOverlay(g, 40, clipRect);
                    DrawBeatNoteGrid5(e, visTempo, visTimeSig);

                    foreach (var c in visChords)
                    {
                        DrawChords5(g, c);
                    }

                    visTicks = DrawLyrics(g, visTicks, visText);
                }
                else
                {

                    DrawModifiers6(g, visMod);
                    DrawBeatNoteGrid5(e, visTempo, visTimeSig);


                    DrawTabLines(g, Utility.linePen);


                    foreach (var c in visChords)
                    {
                        DrawChords5(g, c);
                    }
                    DrawTransparentBackgroundOverlay(g, 40, clipRect);

                    DrawTrainers(g, false, visTrainer);
                    DrawTrainers(g, true, visTrainer);

                    DrawTextEvents(g, true, false, visText);
                    DrawTextEvents(g, true, true, visText);

                }


                if (NumSelectedChords == 1)
                {
                    DrawSelector(g);
                }

                if (CurrentSelectionState == EditorSelectionState.SelectingBox)
                {
                    if (SelectionBox.Width > 2 || SelectionBox.Height > 2)
                    {
                        g.DrawRectangle(Utility.rectSelectionPen, SelectionBox);
                        using (Brush b = new SolidBrush(Color.FromArgb(60, Utility.rectSelectionPen.Color)))
                        {
                            g.FillRectangle(b, SelectionBox);
                        }
                    }
                }

            }
            catch { }
        }

        private TickPair DrawLyrics(Graphics g, TickPair visTicks, List<GuitarTextEvent> visText)
        {
            if (ViewLyrics)
            {
                var visLyrics = new List<GuitarTextEvent>();
                try
                {
                    if (ViewLyrics)
                    {
                        if (Sequence.HasVocalTrack() &&
                            guitarTrack != null &&
                            guitarTrack.GetTrack() != null &&
                            guitarTrack.GetTrack().Name.IsVocalTrackName5() == false)
                        {
                            visLyrics.AddRange(Sequence.GetVocalTrack().Meta.Where(x => x.IsTextEvent()).
                                Where(x => x.AbsoluteTicks > visTicks.Down &&
                                    x.AbsoluteTicks < visTicks.Up).ToList()
                                .Where(x => x.Text.StartsWithEx("[") == false)
                                .Select(x => new GuitarTextEvent(guitarTrack.Messages, x)));
                        }
                    }
                }
                catch { }

                visLyrics.AddRange(visText);

                DrawTextEvents(g, true, false, visLyrics);
            }
            return visTicks;
        }

        private void DrawBeatNoteGrid5(PaintEventArgs e, IEnumerable<GuitarTempo> visTempo, IEnumerable<GuitarTimeSignature> visTimeSig)
        {
            if (Utility.DrawBeat != 0)
            {
                DrawBeat5(e, visTempo, visTimeSig, Utility.DrawBeat != 0, ShowNotesGrid);
            }
        }

        private void DrawTransparentBackgroundOverlay(Graphics g, int alpha, Rectangle clipRect)
        {
            using (var bgb = new SolidBrush(Color.FromArgb(alpha, Utility.BackgroundBrush.Color)))
            {
                g.FillRectangle(bgb, clipRect);
            }
        }

        public bool ShowNotesGrid
        {
            get;
            set;
        }

        public bool IsEditingGuitar
        {
            get
            {
                var ret = false;
                if (IsLoaded)
                {
                    try
                    {
                        GetTrack().IfNotNull(x => ret = x.Name.IsGuitarTrackName());
                    }
                    catch { }
                }
                return ret;
            }
        }

        public bool IsEditingBass
        {
            get
            {
                var ret = false;
                if (IsLoaded)
                {
                    try
                    {
                        GetTrack().IfNotNull(x => ret = x.Name.IsBassTrackName());
                    }
                    catch { }
                }
                return ret;
            }
        }
        private void DrawBeat5(PaintEventArgs e,
            IEnumerable<GuitarTempo> visTempo,
            IEnumerable<GuitarTimeSignature> visTimeSig, bool drawBeat, bool drawNoteGrid)
        {
            if (!drawBeat && !drawNoteGrid)
                return;

            var left = HScrollValue + e.ClipRectangle.Left;
            var right = HScrollValue + e.ClipRectangle.Right;

            var gridScale = (int)GridScale.GetTimeUnit(GridScalar);

            var gridPoints = GetVisibleGridPoints(left, right);

            foreach (var point in gridPoints)
            {
                var next = gridPoints.ElementAtOrDefault(gridPoints.IndexOf(point) + 1);

                var p = point.ScreenPoint;

                var endPoint = (next == null ? GetScreenPointFromTick(guitarTrack.TotalSongTicks) : next.ScreenPoint);
                var len = (endPoint - p).ToDouble();

                if (len >= 3)
                {
                    drawGridLine(e, point.ScreenPoint);
                    if (len >= 10)
                    {
                        var subTickDivision = (len / 2.0).Round();

                        var subTickPoint = point.ScreenPoint + subTickDivision;

                        drawGridLineSubTick(e, subTickPoint, 1);

                        if (len >= 30)
                        {
                            var subTickDivision2 = (subTickDivision / 2.0).Round();

                            drawGridLineSubTick(e, subTickPoint - subTickDivision2, 0);
                            drawGridLineSubTick(e, subTickPoint + subTickDivision2, 0);
                        }
                    }
                }
            }

            if (Utility.ShowMeasureNumbers)
            {
                foreach (var point in gridPoints.Where(x => x.IsWholeNote && x.MeasureNumber > 0))
                {
                    DrawTextAtTime(e.Graphics, 80, 180, true, false,
                        new TickPair(point.Tick, point.Tick + 120), point.MeasureNumber.ToStringEx(), Utility.TextEventBrush, false);
                }
            }

            if (Utility.ShowTempos)
            {

                int lastTempo = 0;
                foreach (var tempo in visTempo.ToList())
                {
                    var curTempo = (tempo.QuarterNotesPerSecond * 60.0).Round(3).Round();
                    var diff = (lastTempo - curTempo).Abs();
                    if (diff > 5)
                    {
                        lastTempo = curTempo;
                        DrawTextAtTime(e.Graphics, 80, 180, true, false,
                            tempo.TickPair,
                            "BPM: " + curTempo, Utility.TextEventBrush, false);
                    }
                }
            }

            if (Utility.ShowTimeSignatures)
            {
                double lastNumerator = double.MinValue;
                double lastDenominator = double.MinValue;

                foreach (var ts in visTimeSig.ToList())
                {
                    if (ts.Numerator != lastNumerator ||
                        ts.Denominator != lastDenominator)
                    {
                        lastNumerator = ts.Numerator;
                        lastDenominator = ts.Denominator;

                        DrawTextAtTime(e.Graphics, 80, 180, true, false,
                            ts.TickPair, "" + ts.Numerator + "/" + ts.Denominator, Utility.TextEventBrush, false);
                    }
                }
            }
        }

        private void drawGridLineSubTick(PaintEventArgs e, int screenPoint, int tickLevel)
        {
            using (var p = new Pen(Utility.beatPen.Color, 1.5f))
            {
                e.Graphics.DrawLine(p, screenPoint - HScrollValue, 0, screenPoint - HScrollValue, ClientSize.Height / (tickLevel == 0 ? 18 : 15));
            }
        }

        private void drawGridLine(PaintEventArgs e, int screenPoint)
        {
            using (var p = new Pen(Utility.beatPen.Color, 1.5f))
            {
                e.Graphics.DrawLine(p, screenPoint - HScrollValue, 0, screenPoint - HScrollValue, ClientSize.Height);
            }
        }

        IEnumerable<GridPoint> GetVisibleGridPoints(int screenLeft, int screenRight)
        {
            var ret = new List<GridPoint>();

            var points = guitarTrack.NoteGrid.Points.ToList();

            ret = points.Where(x => x.ScreenPoint.IsBetween(screenLeft, screenRight, true)).ToList();

            if (ret.Any())
            {
                var idx = points.IndexOf(ret.First());
                if (idx > 0)
                    ret.Insert(0, points.ElementAt(idx - 1));

                var next = points.ElementAtOrDefault(points.IndexOf(ret.Last()) + 1);
                if (next != null)
                    ret.Add(next);
            }

            return ret;
        }

        private Pen GetNoteGridPen()
        {
            Color c = Utility.beatPen.Color;
            return new Pen(Color.FromArgb(c.A, Color.FromArgb(Math.Min(255, c.R + 20), Math.Min(255, c.G + 20), Math.Min(255, c.B + 20))));
        }


        private void DrawChords5(Graphics g, GuitarMessage c)
        {
            var chord = c as GuitarChord;
            if (chord != null)
            {

                for (int k = 0; k < 5; k++)
                {

                    var note = chord.Notes[k];
                    if (note == null)
                        continue;

                    var nb = Utility.G5Brush(note.NoteString);
                    if (chord.Selected)
                    {
                        nb = Utility.noteBGBrushSel;
                    }

                    var downTime = GetClientPointFromTime(note.StartTime);
                    var upTime = GetClientPointFromTime(note.EndTime);

                    var width = upTime - downTime;
                    if (width < Utility.MinimumNoteWidth)
                    {
                        width = Utility.MinimumNoteWidth;
                    }

                    var noteBrush = nb;

                    g.FillRectangle(noteBrush,
                                    downTime,
                                    GetNoteMinYOffset(note),
                                    width, NoteHeight);


                    var boundPen = Utility.noteBoundPen;

                    if (width >= 2)
                    {
                        g.DrawRectangle(boundPen,
                                    downTime,
                                    GetNoteMinYOffset(note),
                                    width, NoteHeight);
                    }
                }
            }
        }

        void DrawRect(Graphics g, Pen pen, Rectangle rect)
        {
            g.DrawRectangle(pen, rect);
        }

        void FillRect(Graphics g, Brush b, TickPair ticks)
        {
            var rectTicks = GetClientPointFromTick(ticks);

            g.FillRectangle(b,
                        rectTicks.Down,
                        0, rectTicks.Up - rectTicks.Down,
                        Height - HScroll.Height - 1);
            if (rectTicks.TickLength >= 4)
            {
                using (var pen = new Pen(Color.FromArgb(30, Utility.noteBoundPen.Color)))
                {
                    g.DrawRectangle(pen,
                        rectTicks.Down,
                        0, rectTicks.Up - rectTicks.Down,
                        Height - HScroll.Height - 1);
                }
            }
        }
        void DrawRect(Graphics g, Brush b, TickPair ticks, int minString, int maxString)
        {
            var maxScreen = TopLineOffset + LineSpacing * (5 - minString) + NoteHeight - 2;
            var minScreen = TopLineOffset + LineSpacing * (5 - maxString) - NoteHeight + 2;

            g.FillRectangle(b,
                        ticks.Down,
                        minScreen, ticks.TickLength,
                        maxScreen - minScreen);
            if (maxScreen - minScreen >= 4)
            {
                g.DrawRectangle(Utility.noteBoundPen,
                    ticks.Down,
                    minScreen, ticks.TickLength,
                    maxScreen - minScreen);
            }
        }

        public GuitarChord SelectChordFromPoint(Point p, bool clear, bool unselect)
        {
            GuitarChord ret = null;

            if (guitarTrack == null)
                return ret;


            if (clear)
            {
                ClearSelection();
            }

            ret = GetChordFromPoint(p);
            if (ret != null)
            {
                if (unselect)
                {
                    ret.Selected = false;
                    ret = null;
                }
                else
                {
                    if (ret.Selected && !clear)
                    {
                        ret.Selected = false;
                        ret = null;
                    }
                    else
                    {
                        ret.Selected = true;
                    }
                }
            }

            if (NumSelectedChords == 1)
            {
                SetSelectedChord(SelectedChord, true);
            }
            Invalidate();
            return ret;
        }



        public void ScrollToSelection()
        {
            var gc = SelectedChord;
            if (gc != null)
            {
                int i = GetClientPointFromTick(gc.DownTick) -
                            Utility.ScollToSelectionOffset;

                HScrollValue = (HScrollValue + i).Max(0);
                Invalidate();
            }
        }

        public void SetSelected(IEnumerable<GuitarChord> chords)
        {
            if (IsLoaded)
            {
                ClearSelection();
                if (chords != null)
                {
                    chords.ForEach(x => x.Selected = true);
                }
                Invalidate();
            }
        }

        public bool ScrollToSelectionEnabled { get; set; }

        public void SetSelectedChord(GuitarChord gc, bool clicked, bool ignoreKeepSelection = false)
        {
            bool wasSelected = gc == null ? false : gc.Selected;

            ClearSelection();

            if (gc != null)
            {
                gc.Selected = true;

                if (OnSetChordToScreen != null)
                {
                    OnSetChordToScreen(this, gc, ignoreKeepSelection);
                }

                if (ScrollToSelectionEnabled && !clicked)
                {
                    ScrollToSelection();
                }

                Invalidate();
            }
        }

        public GuitarChord GetChordFromPoint(Point p)
        {
            GuitarChord ret = null;

            if (guitarTrack == null || guitarTrack.Messages == null)
                return ret;

            var vis = guitarTrack.Messages.Chords.GetBetweenTick(new TickPair(MinVisible, MaxVisible));

            foreach (var c in vis)
            {
                var screen = GetClientPointFromTick(c.TickPair);

                if (p.X < screen.Down || p.X > screen.Up)
                    continue;

                var chordMinMaxY = GetChordYMinMax(c);

                var cr = new Rectangle(screen.Down, chordMinMaxY.X, screen.TickLength, chordMinMaxY.Y - chordMinMaxY.X);

                if (cr.Contains(p))
                {
                    ret = c;
                    break;
                }
            }

            return ret;
        }

        private Point GetChordYMinMax(GuitarChord c)
        {
            return new Point(GetChordMinYOffset(c), GetChordMaxYOffset(c));
        }

        public int GetChordMinYOffset(GuitarChord c)
        {
            if (c.Notes.Any())
            {
                return c.Notes.Min(n => GetNoteMinYOffset(n));
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public int GetChordMaxYOffset(GuitarChord c)
        {
            if (c.Notes.Any())
            {
                return c.Notes.Max(n => GetNoteMaxYOffset(n));
            }
            else
            {
                return Int32.MinValue;
            }
        }

        public int GetNoteMaxYOffset(GuitarNote n)
        {
            return TopLineOffset + LineSpacing * (5 - n.NoteString) + NoteHeight / 2;
        }

        public int GetNoteMinYOffset(GuitarNote n)
        {
            return TopLineOffset + LineSpacing * (5 - n.NoteString) - NoteHeight / 2;
        }

        public List<GuitarChord> GetChordsFromScreenRect(Rectangle rect)
        {
            var ret = new List<GuitarChord>();
            if (!IsLoaded)
                return ret;

            var min = GetTickFromClientPoint(rect.Left);
            var max = GetTickFromClientPoint(rect.Right);

            ret = guitarTrack.Messages.Chords.GetBetweenTick(new TickPair(min, max)).Where(
                c => c.DownTick < max &&
                    c.UpTick > min &&
                    GetChordMinYOffset(c) < rect.Bottom &&
                    GetChordMaxYOffset(c) > rect.Top).ToList();

            return ret;

        }

        public void ClearSelection()
        {
            if (!IsLoaded)
                return;

            SelectedChords.ForEach(x => x.Selected = false);

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var f in fontBuffer)
                {
                    f.Value.Dispose();
                }
                fontBuffer.Clear();
            }
            base.Dispose(disposing);
        }

        public GuitarChord GetStaleChord(GuitarChord chord, bool nextIfNotFound)
        {
            GuitarChord ret = null;

            if (!IsLoaded)
                return null;

            if (chord != null)
            {
                var c = guitarTrack.Messages.Chords.FirstOrDefault(
                    x => x.IsDownEventClose(chord) &&
                         x.IsUpEventClose(chord));
                if (c != null)
                {
                    ret = c;
                }
                else if (nextIfNotFound == true)
                {
                    c = guitarTrack.Messages.Chords.FirstOrDefault(
                        x => x.DownTick >= chord.DownTick);
                    if (c != null)
                    {
                        ret = c;
                    }
                }
            }
            return ret;
        }
        public GuitarChord SelectNextChord(GuitarChord sel)
        {
            if (!IsLoaded)
                return null;

            GuitarChord ret = null;

            if (sel != null)
            {
                ret = GetNextChord(sel);
                if (ret != null)
                {
                    ClearSelection();

                    ret.Selected = true;
                }
            }
            Invalidate();
            return sel;
        }

        public GuitarChord GetNextChord(GuitarChord sel)
        {
            if (!IsLoaded)
                return null;

            if (sel != null)
            {
                return guitarTrack.Messages.Chords.FirstOrDefault(x => x.DownTick > sel.DownTick);
            }
            return null;
        }

        public GuitarChord GetPreviousChord(GuitarChord sel)
        {
            GuitarChord ret = null;

            if (sel != null)
            {
                return guitarTrack.Messages.Chords.LastOrDefault(x => x.DownTick < sel.DownTick);
            }

            return ret;
        }

        public GuitarChord SelectedChord
        {
            get
            {
                if (IsLoaded)
                {
                    return SelectedChords.FirstOrDefault();
                }
                return null;
            }
        }

        public void SelectAllChords()
        {
            if (IsLoaded)
            {
                ClearSelection();
                Messages.Chords.All(x => x.Selected = true);
                Invalidate();
            }
        }

        public List<GuitarChord> SelectedChords
        {
            get
            {
                var ret = new List<GuitarChord>();
                if (IsLoaded)
                {
                    ret.AddRange(guitarTrack.Messages.Chords.Where(x => x.IsDeleted == false && x.Selected));
                }
                return ret;
            }
        }

        public int NumSelectedChords
        {
            get
            {
                return IsLoaded ? SelectedChords.Count : 0;
            }
        }

        public double GridScalar
        {
            get;
            set;
        }


        List<int> SnapPoints = new List<int>();




        void Draw22Tar(PaintEventArgs e, Rectangle clipRect)
        {
            if (guitarTrack == null)
                return;
            if (guitarTrack.Messages == null)
                return;

            var screenTicks = GetTickFromClientPoint(new TickPair(clipRect.Left, clipRect.Right));

            if (screenTicks.IsInvalid)
                return;

            var g = e.Graphics;

            var visChords = guitarTrack.Messages.Chords.GetBetweenTick(screenTicks).ToList();
            var visTrainer = guitarTrack.Messages.Trainers.GetBetweenTick(screenTicks).ToList();
            var visText = guitarTrack.Messages.TextEvents.GetBetweenTick(screenTicks).ToList().Where(x => x.Text.IsChordNameTextEvent() == false).ToList();
            var visTempo = guitarTrack.Messages.Tempos.GetBetweenTick(screenTicks).ToList();
            var visTimeSig = guitarTrack.Messages.TimeSignatures.GetBetweenTick(screenTicks).ToList();
            var visMod = guitarTrack.Messages.GetModifiersBetweenTick(screenTicks).ToList();
            var visHand = guitarTrack.Messages.HandPositions.GetBetweenTick(screenTicks).ToList();

            if (EditMode == EEditMode.Events)
            {

                DrawModifiers6(g, visMod);

                DrawTransparentBackgroundOverlay(g, 20, clipRect);

                DrawTabLines(g, Utility.linePen22);

                DrawBeatAndGrid(e, visTempo, visTimeSig);
                DrawChords6(g, visChords, false);
                DrawChords6(g, visChords, true);

                DrawTransparentBackgroundOverlay(g, 20, clipRect);



                Draw108Events(g, true, false, visHand);
                Draw108Events(g, true, true, visHand);

                DrawTrainers(g, false, visTrainer);
                DrawTrainers(g, true, visTrainer);
                DrawTextEvents(g, true, false, visText);
                DrawTextEvents(g, true, true, visText);

            }
            else
            {

                if (EnableRenderMP3Wave)
                {
                    //RenderWaveViewer(g);
                }

                Draw108Events(g, false, false, visHand);


                DrawTrainers(g, false, visTrainer);
                DrawTrainers(g, true, visTrainer);
                DrawTextEvents(g, false, false, visText);
                DrawTextEvents(g, false, true, visText);


                if (EditMode == EEditMode.Modifiers)
                {
                    DrawTransparentBackgroundOverlay(g, 80, clipRect);

                    DrawTabLines(g, Utility.linePen22);
                    DrawChords6(g, visChords, false);
                    DrawChords6(g, visChords, true);
                    DrawTransparentBackgroundOverlay(g, 5, clipRect);

                    DrawModifiers6(g, visMod);

                    DrawBeatAndGrid(e, visTempo, visTimeSig);
                }
                else
                {
                    DrawModifiers6(g, visMod);

                    DrawTabLines(g, Utility.linePen22);
                    DrawBeatAndGrid(e, visTempo, visTimeSig);

                    DrawChords6(g, visChords, false);
                    DrawChords6(g, visChords, true);
                }
            }

            DrawMovePasteNotes(g);
        }

        private void DrawMovePasteNotes(Graphics g)
        {

            if (CurrentSelectionState == EditorSelectionState.PastingNotes ||
                CurrentSelectionState == EditorSelectionState.MovingNotes)
            {
                if (!CopyChords.Chords.Any())
                {
                    CurrentSelectionState = EditorSelectionState.Idle;
                }
                else
                {

                    DrawPasteChords(g);

                }
            }
            else if (CurrentSelectionState == EditorSelectionState.SelectingBox)
            {
                g.DrawRectangle(Utility.rectSelectionPen, SelectionBox);
                using (Brush b = new SolidBrush(Color.FromArgb(60, Utility.rectSelectionPen.Color)))
                {
                    g.FillRectangle(b, SelectionBox);
                }
            }
            DrawSnapPoints(g);
        }

        private void Draw108Events(Graphics g, bool tabActive, bool drawSelected, IEnumerable<GuitarHandPosition> vis)
        {

            if (Show108Events)
            {
                foreach (var item in vis.Where(x => x.Selected == drawSelected))
                {
                    DrawTextAtTime(g, 80, 180, tabActive, drawSelected,
                        new TickPair(item.DownTick, item.UpTick),
                        "Fret: " + item.NoteFretDown,
                        drawSelected ? Utility.SelectedTextEventBrush : Utility.TextEventBrush, true);
                }
            }
        }

        private void DrawBeatAndGrid(PaintEventArgs e, IEnumerable<GuitarTempo> visTempo, IEnumerable<GuitarTimeSignature> visTimeSig)
        {
            DrawBeat5(e, visTempo, visTimeSig, Utility.DrawBeat != 0, ShowNotesGrid);
        }

        private void DrawSnapPoints(Graphics g)
        {

            foreach (var p in SnapPoints)
            {
                using (var rectPen = new Pen(Color.FromArgb(40, Utility.selectedPen.Color), 2.0f))
                {
                    g.DrawLine(rectPen, new Point(p, 0), new Point(p, this.Height));
                }
            }

            SnapPoints.Clear();
        }


        public List<GuitarTrainer> SelectedProGuitarTrainers
        {
            get
            {
                return Messages.Trainers.Where(x => x.Selected &&
                    x.TrainerType == GuitarTrainerType.ProGuitar).ToList();
            }
        }

        public List<GuitarTrainer> SelectedProBassTrainers
        {
            get
            {
                return Messages.Trainers.Where(x => x.Selected &&
                    x.TrainerType == GuitarTrainerType.ProBass).ToList();
            }
        }

        private void DrawTrainers(Graphics g, bool drawSelected, IEnumerable<GuitarTrainer> trainers)
        {

            foreach (var tr in trainers)
            {

                var ticks = GetClientPointFromTick(
                    new TickPair(tr.Start.AbsoluteTicks, tr.End.AbsoluteTicks));

                if (ticks.Up > 0 && ticks.Down < Width)
                {
                    if (ticks.Down < 0)
                        ticks.Down = 0;
                    if (ticks.Up > Width)
                        ticks.Up = Width;

                    var tb = Utility.TrainerBrush;

                    if (tr.TrainerType == GuitarTrainerType.ProGuitar)
                    {
                        if (SelectedProGuitarTrainers.Any() &&
                            SelectedProGuitarTrainers.Contains(tr))
                        {
                            tb = Utility.SelectedTrainerBrush;
                        }
                    }
                    else if (tr.TrainerType == GuitarTrainerType.ProBass)
                    {
                        if (SelectedProBassTrainers.Any() &&
                            SelectedProBassTrainers.Contains(tr))
                        {
                            tb = Utility.SelectedTrainerBrush;
                        }
                    }
                    if ((drawSelected && tb == Utility.SelectedTrainerBrush) ||
                        (drawSelected == false && tb == Utility.TrainerBrush))
                    {

                        using (var sb = new SolidBrush(Color.FromArgb(
                            EditMode == EEditMode.Events ? tb.Color.A :
                            40, tb.Color)))
                        {

                            g.FillRectangle(sb,
                                ticks.Down,
                                0, ticks.TickLength,
                                Height - HScroll.Height - 1);
                        }
                        using (var p = new Pen(tb))
                        {
                            g.DrawRectangle(p, ticks.Down, 0, ticks.TickLength, Height - HScroll.Height - 1);
                        }
                    }
                }

            }
        }





        public void ClearTextEventSelection()
        {
            if (IsLoaded)
            {
                Messages.TextEvents.Where(x => x.Selected == true).ToList().ForEach(x => x.Selected = false);
                Invalidate();
            }
        }

        public bool HasTextEventSelection { get { return SelectedTextEvent != null; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GuitarTextEvent SelectedTextEvent
        {
            get { return IsLoaded ? Messages.TextEvents.FirstOrDefault(x => x.Selected == true) : null; }
            set
            {
                ClearTextEventSelection();
                if (value != null && Messages.TextEvents.Contains(value))
                {
                    value.Selected = true;

                    ScrollToTick(SelectedTextEvent.AbsoluteTicks);
                }
            }
        }

        VisibleTextEventContainer VisibleTextEvents = new VisibleTextEventContainer();

        private void DrawTextEvents(Graphics g, bool tabActive, bool drawSelected, IEnumerable<GuitarTextEvent> textEvents)
        {
            if (drawSelected == false)
                VisibleTextEvents.Clear();

            foreach (var tr in textEvents)
            {

                var pg = SelectedProGuitarTrainers.Any(x =>
                    x.TrainerType == GuitarTrainerType.ProGuitar &&
                    ((x.Start != null && x.Start.MidiEvent != null && x.Start.MidiEvent == tr.MidiEvent) ||
                    (x.Norm != null && x.Norm.MidiEvent != null && x.Norm.MidiEvent == tr.MidiEvent) ||
                    (x.End != null && x.End.MidiEvent != null && x.End.MidiEvent == tr.MidiEvent)));

                var pb = SelectedProBassTrainers.Any(x =>
                    x.TrainerType == GuitarTrainerType.ProBass &&
                    ((x.Start != null && x.Start.MidiEvent != null && x.Start.MidiEvent == tr.MidiEvent) ||
                    (x.Norm != null && x.Norm.MidiEvent != null && x.Norm.MidiEvent == tr.MidiEvent) ||
                    (x.End != null && x.End.MidiEvent != null && x.End.MidiEvent == tr.MidiEvent)));

                bool isSel = false;
                var tb = Utility.TextEventBrush;
                if ((HasTextEventSelection &&
                   SelectedTextEvent == tr) ||
                    pg || pb)
                {
                    tb = Utility.SelectedTextEventBrush;
                    isSel = true;
                }
                else
                {
                    isSel = false;
                }

                if (drawSelected == isSel)
                {
                    DrawTextEvent(g, tabActive, drawSelected, tr, tb);
                }
            }
        }

        private void DrawTextEvent(Graphics g, bool tabActive, bool drawSelected, GuitarTextEvent tr, SolidBrush tb)
        {

            var st = GetClientPointFromTick(tr.AbsoluteTicks);
            var et = st + 1;
            if (et > 0 &&
                st < Width &&
                st < et)
            {
                if (st < 0)
                    st = 0;
                if (et > Width)
                    et = Width;

                g.FillRectangle(tb,
                    st,
                    0, et - st,
                    InnerHeight - 1);

                using (var p = new Pen(tb))
                {
                    g.DrawRectangle(p, st, 0, et - st, InnerHeight - 1);
                }

                var size = g.MeasureString(tr.Text, Utility.fretFont);
                if (size.Width > 4 || size.Height > 4)
                {

                    bool found = false;

                    var textRect = new RectangleF((float)st, (size.Height * (0)) + (0 * size.Height * 0.1f),
                            size.Width, size.Height);
                    for (int idx = 0; idx < 8; idx++)
                    {
                        var ntr = new RectangleF((float)st, (size.Height * (idx)) + (idx * size.Height * 0.1f),
                            size.Width, size.Height);

                        if (VisibleTextEvents.CountOverlapping(ntr) == 0)
                        {
                            VisibleTextEvents.Add(tr, ntr);
                            textRect = ntr;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        VisibleTextEvents.Add(tr, textRect);
                    }

                    using (var tbg = new SolidBrush(Color.FromArgb(tabActive ? (drawSelected ? 255 : 180) : 80, Utility.BackgroundBrush.Color)))
                    {
                        g.FillRectangle(tbg,
                            textRect.X, textRect.Y, textRect.Width, textRect.Height + 1);
                    }
                    using (var tbg = new Pen(Color.FromArgb(180, Utility.noteBoundPen.Color)))
                    {
                        g.DrawRectangle(tbg,
                            textRect.X, textRect.Y, textRect.Width, textRect.Height + 1);
                    }
                    using (var fb = new SolidBrush(Color.FromArgb(180, Utility.fretBrush.Color)))
                    {
                        var tl = textRect.Location;
                        tl.X -= 1;

                        textRect.Location = tl;
                        textRect.Width += 3;

                        g.DrawString(tr.Text, Utility.fretFont, fb, textRect, StringFormat.GenericDefault);
                    }
                }
            }
        }


        private void DrawTextAtTime(Graphics g, int alpha80, int alpha180,
            bool tabActive, bool drawSelected, TickPair ticks, string text, SolidBrush tb, bool fullHeight)
        {
            var screenTicks = GetClientPointFromTick(ticks);
            if (screenTicks.TickLength <= 0)
                screenTicks.Up = screenTicks.Down + 1;

            if (screenTicks.Up > 0 && screenTicks.Down < Width)
            {

                var size = g.MeasureString(text, Utility.fretFont);

                var textRect = new RectangleF((float)screenTicks.Down, (size.Height * (0)) + (0 * size.Height * 0.1f),
                        size.Width, size.Height);
                for (int idx = 0; idx < 8; idx++)
                {
                    var ntr = new RectangleF((float)screenTicks.Down, (size.Height * (idx)) + (idx * size.Height * 0.1f),
                        size.Width, size.Height);

                    if (VisibleTextEvents.CountOverlapping(ntr) == 0)
                    {
                        textRect = ntr;
                        VisibleTextEvents.Add(new GuitarTextEvent(Messages, screenTicks.Down, text),
                            ntr);
                        break;
                    }
                }

                if (fullHeight)
                {

                    using (var br = new SolidBrush(Color.FromArgb(alpha80, tb.Color)))
                    {
                        g.FillRectangle(br,
                            screenTicks.Down,
                            0, screenTicks.TickLength,
                            fullHeight ? (InnerHeight - 1) : size.Height + 1);
                    }
                    using (var p = new Pen(tb))
                    {
                        g.DrawRectangle(p,
                            screenTicks.Down, 0,
                            screenTicks.TickLength,
                            fullHeight ? (InnerHeight) : size.Height + 2);
                    }
                }
                using (var tbg = new SolidBrush(Color.FromArgb(tabActive ? (drawSelected ? 255 : 200) : alpha80, Utility.BackgroundBrush.Color)))
                {
                    g.FillRectangle(tbg,
                        textRect.X, textRect.Y, textRect.Width, textRect.Height + 1);
                }
                using (var tbg = new Pen(Color.FromArgb(alpha180, Utility.noteBoundPen.Color)))
                {
                    g.DrawRectangle(tbg,
                        textRect.X, textRect.Y, textRect.Width, textRect.Height + 1);
                }
                using (var fb = new SolidBrush(Color.FromArgb(alpha180, Utility.fretBrush.Color)))
                {
                    var tl = textRect.Location;
                    tl.X -= 1;

                    textRect.Location = tl;
                    textRect.Width += 3;
                    g.DrawString(text, Utility.fretFont, fb, textRect, StringFormat.GenericDefault);
                }

            }
        }

        public void ScrollToTick(int tick)
        {
            try
            {
                var i = (int)Math.Round(Utility.ScaleUp(guitarTrack.TickToTime(tick)) - Utility.ScollToSelectionOffset);
                HScrollValue = i;
            }
            catch { }
        }



        public TickPair GetTickFromClientPoint(TickPair pair)
        {
            return new TickPair(GetTickFromClientPoint(pair.Down),
                GetTickFromClientPoint(pair.Up));
        }

        public int GetTickFromClientPoint(int x)
        {
            return guitarTrack.TimeToTick(GetTimeFromClientPoint(x));
        }


        public TickPair GetClientPointFromScreenPoint(TickPair pair)
        {
            return new TickPair(GetClientPointFromScreenPoint(pair.Down),
                GetClientPointFromScreenPoint(pair.Up));
        }

        public int GetClientPointFromScreenPoint(int x)
        {
            return x - HScrollValue;
        }

        public TickPair GetScreenPointFromClientPoint(TickPair pair)
        {
            return new TickPair(GetScreenPointFromClientPoint(pair.Down),
                GetScreenPointFromClientPoint(pair.Up));
        }

        public int GetScreenPointFromClientPoint(int x)
        {
            return x + HScrollValue;
        }

        public TickPair GetTickFromScreenPoint(TickPair pair)
        {
            return new TickPair(GetTickFromScreenPoint(pair.Down),
                GetTickFromScreenPoint(pair.Up));
        }

        public int GetTickFromScreenPoint(int x)
        {
            return guitarTrack.TimeToTick(GetTimeFromScreenPoint(x));
        }

        public double GetTimeFromClientPoint(int x)
        {
            return Utility.ScaleDown(HScrollValue + x);
        }

        public TimePair GetTimeFromScreenPoint(TickPair screenPoint)
        {
            return new TimePair(Utility.ScaleDown(screenPoint.Down), Utility.ScaleDown(screenPoint.Up));
        }

        public double GetTimeFromScreenPoint(int x)
        {
            return Utility.ScaleDown(x);
        }

        public int GetClientPointFromTick(int x)
        {
            return GetClientPointFromTime(guitarTrack.TickToTime(x));
        }
        public int GetScreenPointFromTick(int x)
        {
            return GetScreenPointFromTime(guitarTrack.TickToTime(x));
        }

        public TickPair GetScreenPointFromTick(TickPair p)
        {
            return new TickPair(GetScreenPointFromTick(p.Down),
                                GetScreenPointFromTick(p.Up));
        }

        public TickPair GetClientPointFromTick(TickPair p)
        {
            return new TickPair(GetClientPointFromTick(p.Down),
                                GetClientPointFromTick(p.Up));
        }

        public int GetScreenPointFromTime(double d)
        {
            return (int)Math.Round(Utility.ScaleUp(d));
        }

        public int GetClientPointFromTime(double d)
        {
            return (int)Math.Round(Utility.ScaleUp(d)) - HScrollValue;
        }


        private void AddSnapPointToRender(int closestX)
        {
            if (!SnapPoints.Contains(closestX))
                SnapPoints.Add(closestX);
        }

        public bool GetGridSnapPointFromClientPoint(Point p, out int ret)
        {
            if (SnapTick(GetTickFromClientPoint(p.X), out ret))
            {
                ret = GetClientPointFromTick(ret);
                return true;
            }
            else
            {
                ret = p.X;
            }
            return false;
        }

        public int SnapToString(int screenY)
        {
            int closestDist = int.MaxValue;
            var ret = screenY;

            for (int x = 0; x < 6; x++)
            {
                int noteY = TopLineOffset + LineSpacing * (5 - x);

                var delta = Math.Abs(screenY - noteY);

                if (delta < closestDist)
                {
                    closestDist = delta;

                    ret = x;
                }
            }

            return ret;
        }

        public bool SnapToNotes(TickPair ticks, out TickPair ret)
        {
            ret = new TickPair(ticks);
            bool snapped = false;

            int t1;
            if (!SnapToNotes(ticks.Down, out t1))
            {
                t1 = ticks.Down;
            }
            else
            {
                snapped = true;
            }
            int t2;
            if (!SnapToNotes(ticks.Up, out t2))
            {
                t2 = ticks.Up;
            }
            else
            {
                snapped = true;
            }
            ret = new TickPair(t1, t2);
            return snapped;
        }
        public bool SnapToNotes(int tick, out int ret)
        {
            bool snapped = false;
            ret = tick;

            int screen;
            snapped = SnapClientPointToChords(GetClientPointFromTick(tick), out screen);

            if (snapped)
            {
                ret = GetTickFromClientPoint(screen);
            }
            return snapped;
        }

        public bool SnapTick(int tick, out int closeTick)
        {
            closeTick = tick;
            IEnumerable<int> close;
            var ret = SnapTick(tick, out close);
            if (ret && close.Any())
            {
                closeTick = close.Select(x => new { Dist = (x - tick).Abs(), Val = x }).OrderBy(x => x.Dist).FirstOrDefault().Val;
            }
            return ret;
        }
        public bool SnapTick(int tick, out IEnumerable<int> closeTicks)
        {
            closeTicks = null;

            var ret = new List<int>();

            if (IsLoaded && GridSnap)
            {

                if (NoteSnapG5 && (EditorType == EEditorType.ProGuitar && Editor5.IsLoaded))
                {
                    int point;
                    if (Editor5.SnapToNotes(tick, out point))
                    {
                        ret.Add(point);
                    }
                }

                if (((NoteSnapG5 && EditorType == EEditorType.Guitar5) ||
                    (NoteSnapG6 && EditorType == EEditorType.ProGuitar)))
                {
                    int point;
                    if (SnapToNotes(tick, out point))
                    {
                        ret.Add(point);
                    }
                }
                if (((NoteSnapG5 && EditorType == EEditorType.Guitar5) ||
                    (NoteSnapG6 && EditorType == EEditorType.ProGuitar)))
                {
                    var closeUnit = guitarTrack.GetCloseGridPointToScreenPoint(GetScreenPointFromTick(tick));
                    if (closeUnit != null)
                    {
                        ret.Add(closeUnit.Tick);
                    }
                }
            }

            closeTicks = ret.Distinct();

            closeTicks.ForEach(x => AddSnapPointToRender(GetClientPointFromTick(x)));

            return closeTicks.Any();
        }
        public bool SnapClientPointToChords(int clientPoint, out int ret)
        {
            ret = clientPoint;

            bool snapped = false;

            if ((NoteSnapG6 && EditorType == EEditorType.ProGuitar) ||
                (NoteSnapG5 && EditorType == EEditorType.Guitar5))
            {
                var minMax = new TickPair(
                    clientPoint - Utility.NoteSnapDistance / 2,
                    clientPoint + Utility.NoteSnapDistance / 2);

                var chords = guitarTrack.Messages.Chords.GetBetweenTick(GetTickFromClientPoint(minMax)).ToList();

                if (EditorType == EEditorType.ProGuitar && SelectedChords.Any())
                {
                    chords = chords.Where(x => SelectedChords.Contains(x) == false).ToList();
                }

                int closest = Int32.MaxValue;
                foreach (var x in chords)
                {
                    var distDown = Math.Abs(x.ScreenPointPair.Down - (HScrollValue + clientPoint));
                    var distUp = Math.Abs(x.ScreenPointPair.Up - (HScrollValue + clientPoint));

                    if (distDown <= (Utility.NoteSnapDistance / 2) && distDown < closest)
                    {
                        closest = distDown;
                        ret = x.ScreenPointPair.Down - HScrollValue;
                        snapped = true;
                    }

                    if (distUp <= (Utility.NoteSnapDistance / 2) && distUp < closest)
                    {
                        closest = distUp;
                        ret = x.ScreenPointPair.Up - HScrollValue;
                        snapped = true;
                    }
                }
            }
            return snapped;
        }



        public List<Selector> visibleSelectors = new List<Selector>();


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (IsLoaded == false || InPlayback)
                return;

            IsMouseOver = true;

            lastMouseClient = PointToClient(MousePosition);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsLoaded == false || InPlayback)
                return;

            IsMouseOver = false;
        }

        public bool MouseOverSelector
        {
            get
            {
                return NumSelectedChords > 0 &&
                    visibleSelectors.Count(x => x.IsMouseOver == true) > 0;
            }
        }



        Selector currentSelector;
        public Selector CurrentSelector
        {
            get { return currentSelector; }
        }

        public enum EditorSelectionState
        {
            Idle = 0,
            SelectingBox,
            MovingSelector,
            PastingNotes,
            MovingNotes,
        }

        public enum EditorCreationState
        {
            Idle,
            CreatingNote,
            CreatingSolo,
            CreatingPowerup,
            CreatingArpeggio,
            CopyingPattern,
            CreatingProGuitarTrainer,
            CreatingProBassTrainer,
            CreatingMultiTremelo,
            CreatingSingleTremelo,
        }



        EditorCreationState creationState = EditorCreationState.Idle;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorCreationState CreationState
        {
            get { return creationState; }
            set
            {
                creationState = value;

                if (value == EditorCreationState.Idle)
                {
                    SetSelectionStateIdle();
                }

                if (OnCreationStateChanged != null)
                {
                    OnCreationStateChanged(this, creationState);
                }
            }
        }

        EditorSelectionState _currentSelectionState = EditorSelectionState.Idle;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorSelectionState CurrentSelectionState
        {
            get { return _currentSelectionState; }
            set
            {
                var oldState = _currentSelectionState;
                _currentSelectionState = value;

                InPlayback = false;

                IsSelecting = false;

                if (EditorType == EEditorType.Guitar5)
                {
                    if (value == EditorSelectionState.MovingNotes ||
                        value == EditorSelectionState.MovingSelector ||
                        value == EditorSelectionState.PastingNotes)
                    {
                        _currentSelectionState = EditorSelectionState.Idle;
                        visibleSelectors.Clear();
                        currentSelector = null;
                    }
                }
                else
                {
                    if (value == EditorSelectionState.MovingSelector)
                    {
                        Cursor = Cursors.SizeWE;
                    }
                    else if (value == EditorSelectionState.MovingNotes)
                    {
                        visibleSelectors.Clear();
                        currentSelector = null;
                        Cursor = Cursors.Default;
                    }
                    else
                    {
                        visibleSelectors.Clear();
                        currentSelector = null;
                        Cursor = Cursors.Default;

                    }
                }

                if (value == EditorSelectionState.SelectingBox)
                {
                    IsSelecting = true;

                }

                if (OnSelectionStateChange != null)
                {
                    OnSelectionStateChange(this, oldState, _currentSelectionState);
                }

                Invalidate();
            }
        }



        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (Capture == true)
            {
                Capture = false;
            }

            if (DesignMode || GuitarTrack == null || InPlayback || IsLoaded == false)
                return;

            try
            {

                if (CurrentSelectionState == EditorSelectionState.MovingSelector)
                {

                    if (!MovingSelectorMouseUp())
                    {
                        if (SelectedChords.Count == 1)
                        {
                            if (OnSetChordToScreen != null)
                            {
                                OnSetChordToScreen(this, SelectedChord, true);
                            }
                        }
                    }

                    CurrentSelectionState = EditorSelectionState.Idle;
                }
                else if (CurrentSelectionState == EditorSelectionState.SelectingBox)
                {
                    SelectionBoxMouseUp();
                    CurrentSelectionState = EditorSelectionState.Idle;
                }
                else if (CurrentSelectionState == EditorSelectionState.PastingNotes)
                {
                    CurrentSelectionState = EditorSelectionState.Idle;
                }
                else if (CurrentSelectionState == EditorSelectionState.MovingNotes)
                {
                    try
                    {
                        PasteCopyBuffer(CurrentPastePoint);
                    }
                    catch { }

                    CurrentSelectionState = EditorSelectionState.Idle;
                }

            }
            catch
            {
                CurrentSelectionState = EditorSelectionState.Idle;
            }
            SelectStartPoint = Point.Empty;
        }

        public IEnumerable<Track> GetTracks(IEnumerable<string> trackNames)
        {
            var ret = new List<Track>();
            if (Sequence != null && trackNames != null)
            {
                ret.AddRange(Sequence.Tracks.Where(x => x.Name.IsEmpty() == false && trackNames.Contains(x.Name)));
            }
            return ret;
        }

        public Track GetTrack(string name)
        {
            if (Sequence != null && !string.IsNullOrEmpty(name))
            {
                return Sequence.GetTrack(name);
            }
            return null;
        }

        private void SelectionBoxMouseUp()
        {
            bool ctrlDown = ((ModifierKeys & Keys.Control) == Keys.Control);
            bool altDown = ((ModifierKeys & Keys.Alt) == Keys.Alt);

            var chords = GetChordsFromScreenRect(SelectionBox);

            if (ctrlDown)
            {
                foreach (var c in chords)
                {
                    c.Selected = true;
                }
            }
            else if (altDown)
            {
                foreach (var c in chords)
                {
                    c.Selected = false;
                }
            }
            else
            {
                ClearSelection();
                foreach (var c in chords)
                {
                    c.Selected = true;
                }
            }

            if (NumSelectedChords > 0)
            {
                if (OnSetChordToScreen != null)
                {
                    OnSetChordToScreen(this, SelectedChords[SelectedChords.Count - 1], false);
                }

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            OnLoadTrack += new LoadTrackHandler(TrackEditor_OnLoadTrack);
        }

        void LogDebugEvent(string text)
        {
            Debug.WriteLine(EditorType.ToString() + " - " + text);
        }

        void TrackEditor_OnLoadTrack(TrackEditor editor, Sequence seq, Track t)
        {
            editor.Refresh();
            //LogDebugEvent("onloadtrack " + (t == null ? "" : t.Name));
        }

        public delegate void CreationStateChangedHandler(TrackEditor editor, EditorCreationState newState);
        public event CreationStateChangedHandler OnCreationStateChanged;

        public delegate bool MouseDownEventHandler(TrackEditor editor, MouseEventArgs e);
        public event MouseDownEventHandler OnMouseDownEvent;

        public delegate void CloseTrackHandler(TrackEditor editor);
        public event CloseTrackHandler OnClose;


        public delegate void LoadTrackHandler(TrackEditor editor, Sequence seq, Track t);
        public event LoadTrackHandler OnLoadTrack;

        public delegate void SetChordToScreenHandler(TrackEditor editor, GuitarChord chord, bool ignoreKeepSelection);
        public event SetChordToScreenHandler OnSetChordToScreen;


        public delegate IEnumerable<GuitarChord> AddChordHandler(TrackEditor editor, GuitarChord chord);
        public event AddChordHandler OnAddChordHandler;

        public delegate void SelectionStateChangeHandler(TrackEditor editor, EditorSelectionState oldState, EditorSelectionState newState);
        public event SelectionStateChangeHandler OnSelectionStateChange;


        public delegate void ZoomHandler(TrackEditor editor, int delta);
        public event ZoomHandler OnZoom;

        private bool MovingSelectorMouseUp()
        {
            bool ret = false;
            if (CurrentSelector != null && CurrentSelector.Chord != null)
            {
                var sel = CurrentSelector;

                if (sel.StartPoint.X != sel.CurrentPoint.X)
                {
                    var tickPair = sel.Chord.TickPair;

                    if (sel.IsRight)
                    {
                        tickPair.Up = GetTickFromClientPoint(sel.CurrentPoint.X + Utility.SelectorWidth);
                    }
                    else
                    {
                        tickPair.Down = GetTickFromClientPoint(sel.CurrentPoint.X);
                    }
                    try
                    {
                        if (tickPair.TickLength > Utility.TickCloseWidth)
                        {
                            EditorPro.BackupSequence();

                            sel.Chord.SetTicks(tickPair);

                            var newChords = OnAddChordHandler(this, sel.Chord);

                            currentSelector = null;
                            if (newChords.Any())
                            {
                                SetSelected(newChords);
                            }
                        }
                        ret = true;
                    }
                    catch { }

                }
            }
            EditorPro.Invalidate();
            CurrentSelectionState = EditorSelectionState.Idle;
            return ret;
        }

        public TickPair SnapLeftRightClientPoint(TickPair clientPoint)
        {
            var screenPoint = GetScreenPointFromClientPoint(clientPoint);

            var gridPointDown = guitarTrack.GetCloseGridPointToScreenPoint(screenPoint.Down);
            var gridPointUp = guitarTrack.GetCloseGridPointToScreenPoint(screenPoint.Up);

            var timePair = GetTimeFromScreenPoint(screenPoint.Expand(10));

            var closeChords6 = Messages.Chords.GetBetweenTime(timePair).ToList();
            var closeChords5 = new List<GuitarChord>();
            if (IsPro && Editor5.IsLoaded && NoteSnapG5)
            {
                closeChords5.AddRange(Editor5.Messages.Chords.GetBetweenTime(timePair).ToList());
            }

            var closeToLeft6 = closeChords6.Where(x => x.ScreenPointPair.Up.IsCloseScreenPoint(screenPoint.Down)).ToList();
            var closeToLeft5 = closeChords5.Where(x => x.ScreenPointPair.Up.IsCloseScreenPoint(screenPoint.Down)).ToList();

            var closeToRight6 = closeChords6.Where(x => x.ScreenPointPair.Down.IsCloseScreenPoint(screenPoint.Up)).ToList();
            var closeToRight5 = closeChords5.Where(x => x.ScreenPointPair.Down.IsCloseScreenPoint(screenPoint.Up)).ToList();

            var origPair = screenPoint;
            var screenPointLeft = snapLeftScreenPoint(origPair, gridPointDown, closeToLeft6, closeToLeft5);
            var screenPointRight = snapRightScreenPoint(origPair, gridPointUp, closeToRight6, closeToRight5);


            return GetClientPointFromScreenPoint(new TickPair(screenPointLeft.Down, screenPointRight.Up));
        }


        public TickPair SnapLeftRightTicks(TickPair tickPair, SnapConfig config)
        {

            GridPoint gridPointDown = null;
            GridPoint gridPointUp = null;

            if (config.SnapGrid)
            {
                gridPointDown = guitarTrack.GetCloseGridPointToTick(tickPair.Down);
                gridPointUp = guitarTrack.GetCloseGridPointToTick(tickPair.Up);
            }

            IEnumerable<TickPair> closeLeft6 = null;
            IEnumerable<TickPair> closeRight6 = null;

            IEnumerable<TickPair> closeLeft5 = null;
            IEnumerable<TickPair> closeRight5 = null;

            if (config.SnapG6 && EditorPro.IsLoaded)
            {
                var closeChords6 = Messages.Chords.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                closeLeft6 = closeChords6.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair);
                closeRight6 = closeChords6.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair);

                var closePowerups = Messages.Powerups.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                if (closePowerups.Any())
                {
                    closeLeft6 = closeLeft6.Concat(closePowerups.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair));
                    closeRight6 = closeRight6.Concat(closePowerups.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair));
                }

                var closeSolo = Messages.Solos.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                if (closeSolo.Any())
                {
                    closeLeft6 = closeLeft6.Concat(closeSolo.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair));
                    closeRight6 = closeRight6.Concat(closeSolo.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair));
                }
            }
            if (IsPro && config.SnapG5 && Editor5.IsLoaded)
            {
                var closeChords5 = Editor5.Messages.Chords.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                closeLeft5 = closeChords5.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair);
                closeRight5 = closeChords5.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair);

                var closePowerups = Editor5.Messages.Powerups.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                if (closePowerups.Any())
                {
                    closeLeft5 = closeLeft5.Concat(closePowerups.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair));
                    closeRight5 = closeRight5.Concat(closePowerups.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair));
                }

                var closeSolo = Editor5.Messages.Solos.GetBetweenTick(tickPair.Expand(Utility.TickCloseWidth)).ToList();
                if (closeSolo.Any())
                {
                    closeLeft5 = closeLeft5.Concat(closeSolo.Where(x => x.TickPair.IsClose(tickPair.Down)).Select(x => x.TickPair));
                    closeRight5 = closeRight5.Concat(closeSolo.Where(x => x.TickPair.IsClose(tickPair.Up)).Select(x => x.TickPair));
                }
            }

            var oldPair = tickPair;

            var tickPairDown = snapLeftTick(oldPair, gridPointDown, closeLeft6, closeLeft5, config);
            var tickPairUp = snapRightTick(oldPair, gridPointUp, closeRight6, closeRight5, config);

            if (tickPairDown.Down != tickPair.Down)
            {
                AddSnapPointToRender(GetClientPointFromTick(tickPairDown.Down));
            }
            if (tickPairUp.Up != tickPair.Up)
            {
                AddSnapPointToRender(GetClientPointFromTick(tickPairUp.Up));
            }
            return new TickPair(tickPairDown.Down, tickPairUp.Up);
        }

        private TickPair snapLeftTick(TickPair tickPair, GridPoint gridPoint,
            IEnumerable<TickPair> closeToLeft,
            IEnumerable<TickPair> closeToLeft5,
            SnapConfig config)
        {
            var gridTick = Int32.MinValue;
            var tickPairDown = tickPair.Down;

            var tickPair5 = Int32.MinValue;
            var tickPair6 = Int32.MinValue;

            if (gridPoint != null && config.SnapGrid)
            {
                if (tickPairDown.IsCloseTick(gridPoint.Tick))
                    gridTick = gridPoint.Tick;
            }
            if (config.SnapG5 && closeToLeft5 != null && closeToLeft5.Any())
            {
                var closeDown = closeToLeft5.Where(x => x.Down.IsCloseTick(tickPairDown)).ToList();
                var closeUp = closeToLeft5.Where(x => x.Up.IsCloseTick(tickPairDown)).ToList();

                if (closeDown.Any())
                {
                    var maxDown = closeDown.Select(x => new { Dist = (tickPair.Down - x.Down).Abs(), Val = x.Down }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (tickPairDown.IsCloseTick(maxDown))
                    {
                        tickPair5 = maxDown;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPairDown);
                        if (gd < maxDown.DistSq(tickPairDown))
                        {
                            if (tickPairDown.IsCloseTick(gridTick))
                                tickPair5 = gridTick;
                        }
                        else
                        {
                            if (tickPairDown.IsCloseTick(maxDown))
                                tickPair5 = maxDown;
                        }
                    }

                }

                if (closeUp.Any())
                {
                    var maxUp = closeUp.Select(x => new { Dist = (tickPair.Down - x.Up).Abs(), Val = x.Up }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (maxUp > tickPairDown)
                    {
                        if (tickPairDown.IsCloseTick(maxUp))
                            tickPair5 = maxUp;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPairDown);
                        if (gd < maxUp.DistSq(tickPairDown))
                        {
                            if (tickPairDown.IsCloseTick(gridTick))
                                tickPair5 = gridTick;
                        }
                        else
                        {
                            if (tickPairDown.IsCloseTick(maxUp))
                                tickPair5 = maxUp;
                        }
                    }

                }

            }
            if (config.SnapG6 && closeToLeft != null && closeToLeft.Any())
            {
                var closeDown = closeToLeft.Where(x => x.Down.IsCloseTick(tickPair.Down)).ToList();
                var closeUp = closeToLeft.Where(x => x.Up.IsCloseTick(tickPair.Down)).ToList();

                if (closeDown.Any())
                {
                    var minDown = closeDown.Select(x => new { Dist = (tickPair.Down - x.Down).Abs(), Val = x.Down }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (tickPairDown.IsCloseTick(minDown))
                    {
                        tickPair6 = minDown;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPairDown);
                        if (gd < minDown.DistSq(tickPairDown))
                        {
                            if (tickPairDown.IsCloseTick(gridTick))
                                tickPair6 = gridTick;
                        }
                        else
                        {
                            if (tickPairDown.IsCloseTick(minDown))
                                tickPair6 = minDown;
                        }
                    }
                }

                if (closeUp.Any())
                {
                    var maxUp = closeUp.Select(x => new { Dist = (tickPair.Down - x.Up).Abs(), Val = x.Up }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (tickPairDown.IsCloseTick(maxUp))
                    {
                        tickPair6 = maxUp;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPairDown);
                        if (gd < maxUp.DistSq(tickPairDown))
                        {
                            if (tickPairDown.IsCloseTick(gridTick))
                                tickPair6 = gridTick;
                        }
                        else
                        {
                            if (tickPairDown.IsCloseTick(maxUp))
                                tickPair6 = maxUp;
                        }
                    }

                }

            }
            else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
            {
                if (tickPairDown.IsCloseTick(gridTick))
                    tickPairDown = gridTick;
            }

            var ret = new[] { tickPair6, tickPair5, gridTick }.Where(x => x.IsNotNull());
            if (ret.Any())
            {
                tickPairDown = ret.Select(x => new { Dist = (x - tickPairDown).Abs(), Val = x }).OrderBy(x => x.Dist).FirstOrDefault().Val;
            }

            if (tickPairDown.IsNull() == false &&
                tickPairDown > tickPair.Up)
            {
                tickPairDown = tickPair.Up;
            }
            return new TickPair(tickPairDown, tickPair.Up);
        }
        private TickPair snapRightTick(TickPair tickPair, GridPoint gridPoint,
            IEnumerable<TickPair> closeToRight,
            IEnumerable<TickPair> closeToRight5, SnapConfig config)
        {
            var gridTick = Int32.MinValue;
            var tickPair5 = Int32.MinValue;
            var tickPair6 = Int32.MinValue;

            if (gridPoint != null && config.SnapGrid)
            {
                if (tickPair.Up.IsCloseTick(gridPoint.Tick))
                    gridTick = gridPoint.Tick;
            }
            if (config.SnapG5 && closeToRight5 != null && closeToRight5.Any())
            {
                var closeDown = closeToRight5.Where(x => x.Down.IsCloseTick(tickPair.Up)).ToList();
                var closeUp = closeToRight5.Where(x => x.Up.IsCloseTick(tickPair.Up)).ToList();

                if (closeDown.Any())
                {
                    var minDown = closeDown.Select(x => new { Dist = (tickPair.Up - x.Down).Abs(), Val = x.Down }).OrderBy(x => x.Dist).FirstOrDefault().Val;


                    if (tickPair.Up.IsCloseTick(minDown))
                    {
                        tickPair5 = minDown;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPair.Up);
                        if (gd < minDown.DistSq(tickPair.Up))
                        {
                            if (tickPair.Up.IsCloseTick(gridTick))
                                tickPair5 = gridTick;
                        }
                        else
                        {
                            if (tickPair.Up.IsCloseTick(minDown))
                                tickPair5 = minDown;
                        }
                    }

                }
                if (closeUp.Any())
                {
                    var maxUp = closeUp.Select(x => new { Dist = (tickPair.Up - x.Up).Abs(), Val = x.Up }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (tickPair.Up.IsCloseTick(maxUp))
                    {
                        tickPair5 = maxUp;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPair.Up);
                        if (gd < maxUp.DistSq(tickPair.Up))
                        {
                            if (tickPair.Up.IsCloseTick(gridTick))
                                tickPair5 = gridTick;
                        }
                        else
                        {
                            if (tickPair.Up.IsCloseTick(maxUp))
                                tickPair5 = maxUp;
                        }
                    }

                }

            }
            if (config.SnapG6 && closeToRight != null && closeToRight.Any())
            {
                var closeDown = closeToRight.Where(x => x.Down.IsCloseTick(tickPair.Up)).ToList();
                var closeUp = closeToRight.Where(x => x.Up.IsCloseTick(tickPair.Up)).ToList();

                if (closeDown.Any())
                {
                    var minDown = closeDown.Select(x => new { Dist = (tickPair.Up - x.Down).Abs(), Val = x.Down }).OrderBy(x => x.Dist).FirstOrDefault().Val;

                    if (tickPair.Up.IsCloseTick(minDown))
                    {
                        tickPair6 = minDown;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPair.Up);
                        if (gd < minDown.DistSq(tickPair.Up))
                        {
                            if (tickPair.Up.IsCloseTick(gridTick))
                                tickPair6 = gridTick;
                        }
                        else
                        {
                            if (tickPair.Up.IsCloseTick(minDown))
                                tickPair6 = minDown;
                        }
                    }

                }
                if (closeUp.Any())
                {
                    var maxUp = closeUp.Select(x => new { Dist = (tickPair.Up - x.Up).Abs(), Val = x.Up }).OrderBy(x => x.Dist).FirstOrDefault().Val;


                    if (tickPair.Up.IsCloseTick(maxUp))
                    {
                        tickPair6 = maxUp;
                    }
                    else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
                    {
                        var gd = gridTick.DistSq(tickPair.Up);
                        if (gd < maxUp.DistSq(tickPair.Up))
                        {
                            if (tickPair.Up.IsCloseTick(gridTick))
                                tickPair6 = gridTick;
                        }
                        else
                        {
                            if (tickPair.Up.IsCloseTick(maxUp))
                                tickPair6 = maxUp;
                        }
                    }

                }
            }
            else if (gridPoint != null && config.SnapGrid && gridTick.IsNotNull())
            {
                if (tickPair.Up.IsCloseTick(gridTick))
                    tickPair.Up = gridTick;
            }
            var ret = new[] { tickPair6, tickPair5, gridTick }.Where(x => x.IsNotNull());
            if (ret.Any())
            {
                tickPair.Up = ret.Select(x => new { Dist = (x - tickPair.Up).Abs(), Val = x }).OrderBy(x => x.Dist).FirstOrDefault().Val;
            }
            if (tickPair.Up.IsNull() == false &&
                tickPair.Up < tickPair.Down)
            {
                tickPair.Up = tickPair.Down;
            }
            return tickPair;
        }

        private TickPair snapLeftScreenPoint(TickPair screenPair, GridPoint gridPoint, List<GuitarChord> closeToLeft, List<GuitarChord> closeToLeft5)
        {
            var gridScreenPoint = Int32.MinValue;
            if (gridPoint != null)
            {
                if (screenPair.Down.IsCloseScreenPoint(gridPoint.ScreenPoint))
                    gridScreenPoint = gridPoint.ScreenPoint;
            }
            if (closeToLeft5.Any())
            {
                var maxLeft = closeToLeft5.Max(x => x.ScreenPointPair.Up);

                if (maxLeft > screenPair.Down)
                {
                    if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                        screenPair.Down = maxLeft;
                }
                else if (gridPoint != null)
                {
                    var gd = gridScreenPoint.DistSq(screenPair.Down);
                    if (gd < maxLeft.DistSq(screenPair.Down))
                    {
                        if (screenPair.Down.IsCloseScreenPoint(gridScreenPoint))
                            screenPair.Down = gridScreenPoint;
                    }
                    else
                    {
                        if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                            screenPair.Down = maxLeft;
                    }
                }
                else
                {
                    if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                        screenPair.Down = maxLeft;
                }
            }
            if (closeToLeft.Any())
            {
                var maxLeft = closeToLeft.Max(x => x.ScreenPointPair.Up);

                if (maxLeft > screenPair.Down)
                {
                    if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                        screenPair.Down = maxLeft;
                }
                else if (gridPoint != null)
                {
                    var gd = gridScreenPoint.DistSq(screenPair.Down);
                    if (gd < maxLeft.DistSq(screenPair.Down))
                    {
                        if (screenPair.Down.IsCloseScreenPoint(gridScreenPoint))
                            screenPair.Down = gridScreenPoint;
                    }
                    else
                    {
                        if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                            screenPair.Down = maxLeft;
                    }
                }
                else
                {
                    if (screenPair.Down.IsCloseScreenPoint(maxLeft))
                        screenPair.Down = maxLeft;
                }
            }
            else if (gridPoint != null)
            {
                if (screenPair.Down.IsCloseScreenPoint(gridScreenPoint))
                    screenPair.Down = gridScreenPoint;
            }
            return screenPair;
        }


        private TickPair snapRightScreenPoint(TickPair screenPair, GridPoint gridPoint, List<GuitarChord> closeToRight, List<GuitarChord> closeToRight5)
        {
            var gridScreenPoint = Int32.MinValue;
            if (gridPoint != null)
            {
                if (screenPair.Up.IsCloseScreenPoint(gridPoint.ScreenPoint))
                    gridScreenPoint = gridPoint.ScreenPoint;
            }

            if (closeToRight.Any())
            {
                var minDown = closeToRight.Min(x => x.ScreenPointPair.Down);

                if (minDown < screenPair.Up)
                {
                    if (screenPair.Up.IsCloseScreenPoint(minDown))
                        screenPair.Up = minDown;
                }
                else if (gridPoint != null)
                {
                    var gd = gridScreenPoint.DistSq(screenPair.Up);
                    if (gd < minDown.DistSq(screenPair.Up))
                    {
                        if (screenPair.Up.IsCloseScreenPoint(gridScreenPoint))
                            screenPair.Up = gridScreenPoint;
                    }
                    else
                    {
                        if (screenPair.Up.IsCloseScreenPoint(minDown))
                            screenPair.Up = minDown;
                    }
                }
                else
                {
                    if (screenPair.Up.IsCloseScreenPoint(minDown))
                        screenPair.Up = minDown;
                }
            }
            else if (gridPoint != null)
            {
                if (screenPair.Up.IsCloseScreenPoint(gridScreenPoint))
                    screenPair.Up = gridScreenPoint;
            }
            return screenPair;
        }

        Point lastMouseClient = Point.Empty;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e != null)
            {
                base.OnMouseMove(e);
            }

            if (InPlayback || IsLoaded == false)
                return;

            var mouseClient = PointToClient(MousePosition);

            if (CurrentSelectionState == EditorSelectionState.MovingSelector)
            {
                if (MouseButtons.HasFlag(MouseButtons.Left))
                {
                    MoveChordSelector(mouseClient);
                }
            }
            else if (CurrentSelectionState == EditorSelectionState.SelectingBox)
            {
                SelectCurrentPoint = mouseClient;
            }
            else if (CurrentSelectionState == EditorSelectionState.PastingNotes ||
                     CurrentSelectionState == EditorSelectionState.MovingNotes)
            {
                CopyChords.UpdatePastePoint(mouseClient, new Point(mouseClient.X - lastMouseClient.X, mouseClient.Y - lastMouseClient.Y));
            }
            else if (CurrentSelectionState == EditorSelectionState.Idle)
            {

                if (EditMode == EEditMode.Chords)
                {

                    if (MouseButtons.HasFlag(MouseButtons.Left) && SelectedChords.Any())
                    {
                        if (SelectStartPoint.IsEmpty == false)
                        {
                            var mc = GetChordFromPoint(SelectStartPoint);
                            if (mc != null)
                            {
                                int stringY1 = SnapToString(SelectStartPoint.Y);
                                int stringY2 = SnapToString(mouseClient.Y);

                                if (stringY1 != stringY2 ||
                                    Math.Abs(mouseClient.X - SelectStartPoint.X) > Utility.NoteSnapDistance)
                                {
                                    if (EditorType != EEditorType.Guitar5)
                                    {
                                        CopyChords.Clear();
                                        CopyChords.AddRange(SelectedChords);
                                        CopyChords.Begin(mouseClient);

                                        if (!IsAltKeyDown)
                                        {
                                            RemoveSelectedNotes();
                                        }
                                        CurrentSelectionState = EditorSelectionState.MovingNotes;

                                    }
                                }
                            }
                        }
                    }
                }
            }

            UpdateSelectorVisibility();

            lastMouseClient = mouseClient;
        }


        public void SetChordToClipboard(GuitarChord gc)
        {
            CopyChords.Clear();
            if (gc != null)
            {
                CurrentSelectionState = EditorSelectionState.PastingNotes;
                CopyChords.Add(gc);
                CopyChords.BeginPaste(PointToClient(MousePosition));
            }
        }
        public void SetSelectedToClipboard()
        {
            if (IsLoaded)
            {
                if (SelectedChords.Count > 0)
                {
                    CopyChords.Clear();
                    CurrentSelectionState = EditorSelectionState.PastingNotes;
                    CopyChords.AddRange(EditorPro.SelectedChords);
                    CopyChords.BeginPaste(PointToClient(MousePosition));
                }
            }
        }

        private void UpdateSelectorVisibility()
        {
            if (CurrentSelectionState == EditorSelectionState.Idle && NumSelectedChords > 0)
            {
                visibleSelectors = GetVisibleSelectors();
            }
            else
            {
                visibleSelectors.Clear();
            }

            Invalidate();
        }

        public Point MousePositionSnapped
        {
            get
            {
                var ret = PointToClient(MousePosition);

                if (GridSnap)
                {
                    int mc;
                    if (GetGridSnapPointFromClientPoint(ret, out mc))
                    {
                        ret.X = mc;
                    }
                }
                return new Point(ret.X, SnapToString(ret.Y));
            }
        }
        private void MoveChordSelector(Point mouseClient)
        {
            var sel = CurrentSelector;

            if (sel == null || sel.Chord == null)
                return;

            if (GridSnap)
            {
                var tickPair = sel.Chord.TickPair;

                if (sel.IsRight)
                {
                    tickPair.Up = GetTickFromClientPoint(mouseClient.X);
                }
                else
                {
                    tickPair.Down = GetTickFromClientPoint(mouseClient.X);
                }

                tickPair = SnapLeftRightTicks(tickPair, new SnapConfig(true, true, true));

                var cli = GetClientPointFromTick(tickPair);
                int mc = cli.Down;
                if (sel.IsRight)
                {
                    mc = cli.Up;
                }


                mouseClient = new Point(mc, SnapToString(mouseClient.Y));
            }

            if (sel.IsRight)
            {
                sel.CurrentPoint = new Point(mouseClient.X - Utility.SelectorWidth, sel.StartPoint.Y);
            }
            else
            {
                sel.CurrentPoint = new Point(mouseClient.X, sel.StartPoint.Y);
            }
            Invalidate();
        }


        public void RemoveSelectedNotes()
        {
            try
            {
                BackupSequence();

                guitarTrack.Remove(SelectedChords.ToList());
                Invalidate();
            }
            catch { RestoreBackup(); }
        }

        public bool OnMouseDown5Tar(MouseEventArgs e)
        {
            if (!IsLoaded)
                return false;

            bool ret = false;
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu mnu = new ContextMenu();
                    MenuItem mnuInsertIntoPro = new MenuItem("Insert Into Pro");
                    mnuInsertIntoPro.Click += new EventHandler(mnuInsertIntoPro_Click);
                    mnu.MenuItems.Add(mnuInsertIntoPro);
                    mnu.Show(this, PointToClient(MousePosition));
                    ret = true;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    bool clear = ((ModifierKeys & Keys.Control) != Keys.Control);
                    bool unselect = ((ModifierKeys & Keys.Alt) == Keys.Alt);
                    if (unselect)
                        clear = false;

                    var gc = GetChordFromPoint(e.Location);

                    bool wasSelected = gc != null && gc.Selected;
                    if (clear)
                    {
                        ClearSelection();
                    }

                    if (gc != null)
                    {
                        if (unselect)
                        {
                            gc.Selected = false;
                        }
                        else
                        {
                            SetSelectedChord(gc, true, wasSelected);
                        }
                    }
                    Invalidate();
                }
            }
            catch { }
            return ret;
        }


        public void mnuInsertIntoPro_Click(object sender, EventArgs e)
        {


            try
            {
                if (EditorPro.IsLoaded && Editor5.IsLoaded)
                {
                    EditorPro.BackupSequence();

                    var newChords = new List<GuitarChord>();
                    Editor5.SelectedChords.ToList().ForEach(chord5 =>
                    {
                        var newChord = GuitarChord.CreateChord(EditorPro.Messages, CurrentDifficulty,
                            SnapTickPairPro(chord5.TickPair),
                            new GuitarChordConfig(chord5.Notes.FretArrayZero,
                            chord5.Notes.ChannelArrayZero,
                            false, false, false, ChordStrum.Normal,
                                null));

                        if (newChord != null)
                        {
                            newChords.Add(newChord);
                        }
                    });

                    EditorPro.SetSelected(newChords);

                    EditorPro.Invalidate();
                }
            }
            catch { }
        }

        public TickPair SnapTickPairPro(TickPair pair)
        {
            int down;
            if (!EditorPro.SnapTick(pair.Down, out down))
            {
                down = pair.Down;
            }
            int up;
            if (!EditorPro.SnapTick(pair.Up, out up))
            {
                up = pair.Up;
            }

            TickPair ret = new TickPair(pair);

            TickPair p;
            if (EditorPro.SnapToNotes(pair, out p))
            {
                ret = p;
            }
            return ret;
        }


        Track GetTrack() { return GuitarTrack == null ? null : GuitarTrack.GetTrack(); }

        public void ReloadTrack()
        {
            if (EditorType == EEditorType.Guitar5)
            {
                SetTrack5(GetTrack(), CurrentDifficulty);
            }
            else
            {
                SetTrack6(GetTrack(), CurrentDifficulty);
            }

        }

        public GuitarDifficulty CurrentDifficulty
        {
            get
            {
                return guitarTrack != null ? guitarTrack.CurrentDifficulty : GuitarDifficulty.Expert;
            }
            set
            {
                if (guitarTrack != null)
                {
                    guitarTrack.SetTrack(guitarTrack.GetTrack(), value);
                    Invalidate();
                }
            }
        }

        public List<GuitarChord> PasteCopyBuffer(PastePointParam pastePoint)
        {
            var newChords = new List<GuitarChord>();
            try
            {
                if (CopyChords.Chords.Any())
                {
                    if (CurrentSelectionState != TrackEditor.EditorSelectionState.PastingNotes &&
                        CurrentSelectionState != TrackEditor.EditorSelectionState.MovingNotes)
                    {
                        CurrentSelectionState = TrackEditor.EditorSelectionState.PastingNotes;
                        return newChords;
                    }

                    BackupSequence();

                    var copyRange = CopyChords.Chords.GetTickPair();

                    var stringOffset = (pastePoint.MousePos.Y) - pastePoint.MinNoteString - pastePoint.Offset.Y;

                    var startTick = GetTickFromClientPoint(pastePoint.MousePos.X + pastePoint.Offset.X);// pastePoint.Offset.X + pastePoint.MousePos.X);

                    var pasteRange = new TickPair(startTick, startTick + copyRange.TickLength);

                    var copyTime = guitarTrack.TickToTime(copyRange);

                    var pasteTime = guitarTrack.TickToTime(pasteRange);

                    pasteTime.TimeLength = copyTime.TimeLength;
                    pasteRange.Up = guitarTrack.TimeToTick(pasteTime.Up);

                    pasteRange = EditorPro.SnapTickPairPro(pasteRange);

                    guitarTrack.Remove(Messages.Chords.GetBetweenTick(pasteRange.Expand(-Utility.NoteCloseWidth)).ToList());

                    foreach (var c in CopyChords.Chords)
                    {
                        var noteTime = guitarTrack.TickToTime(c.TickPair);

                        var delta = noteTime.Down - copyTime.Down;

                        var startEndTick = new TickPair(
                            guitarTrack.TimeToTick(pasteTime.Down + delta),
                            guitarTrack.TimeToTick(pasteTime.Down + delta + c.TimeLength));

                        var snappedTicks = SnapLeftRightTicks(startEndTick, new SnapConfig(true, true, true));

                        var newChord = c.CloneAtTime(Messages, snappedTicks, stringOffset);
                        if (newChord != null)
                        {
                            newChords.Add(newChord);
                        }
                    }

                    ClearSelection();

                    foreach (var c in newChords.Where(x => !x.IsDeleted)) { c.Selected = true; }

                    Invalidate();
                }
            }
            catch { RestoreBackup(); }


            return newChords;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Track SelectedTrack
        {
            get { return IsLoaded ? guitarTrack.GetTrack() : null; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackDifficulty SelectedTrackDifficulty
        {
            get { return IsLoaded ? new TrackDifficulty(SelectedTrack.Name, CurrentDifficulty) : null; }

            set
            {
                if (value != null)
                {
                    SetTrack(value.Track, value.Difficulty);
                }
            }
        }

        public bool SetTrack(string name, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            var ret = false;
            if (difficulty.IsUnknown())
                difficulty = this.CurrentDifficulty;
            var track = GetTrack(name);
            if (track != null)
            {
                ret = SetTrack(track, difficulty);
            }
            return ret;
        }
        public bool SetTrack(Track track, GuitarDifficulty difficulty = GuitarDifficulty.Unknown)
        {
            var ret = false;
            if (difficulty.IsUnknown())
                difficulty = this.CurrentDifficulty;

            if (track.IsFileTypePro())
            {
                ret = SetTrack6(track, difficulty);
            }
            else
            {
                ret = SetTrack5(track, difficulty);
            }
            return ret;
        }

        public event EventHandler OnStatusIdle;

        public void SetSelectionStateIdle()
        {
            if (CurrentSelectionState != EditorSelectionState.Idle)
            {
                CurrentSelectionState = EditorSelectionState.Idle;

                OnStatusIdle.IfObjectNotNull(x => x(null, null));
            }

            Invalidate();
        }

        public void SetCreationStateIdle()
        {
            if (CreationState != EditorCreationState.Idle)
            {
                CreationState = EditorCreationState.Idle;
            }
            Invalidate();
        }

        public bool IsStatusIdle()
        {
            return CurrentSelectionState == EditorSelectionState.Idle;
        }

        public bool HandleProRightMouseMenu(MouseEventArgs ev)
        {
            if (!IsLoaded)
                return false;

            if (ev.Button == MouseButtons.Right)
            {

                if (IsStatusIdle() == false)
                {
                    SetSelectionStateIdle();
                }

                var nx = PointToClient(MousePosition);

                var mnu = new ContextMenu();
                {
                    var mnuSelect = new MenuItem("Delete");
                    if (SelectedChord != null)
                    {
                        mnuSelect.Click += new EventHandler(delegate(object o, EventArgs e)
                        {
                            RemoveSelectedNotes();
                        });

                    }
                    else
                    {
                        mnuSelect.Enabled = false;
                    }
                    mnu.MenuItems.Add(mnuSelect);
                }

                {
                    var mnuSelect = new MenuItem("Copy");
                    if (SelectedChord != null)
                    {
                        mnuSelect.Click += new EventHandler(delegate(object o, EventArgs e)
                        {
                            SetSelectedToClipboard();
                        });
                    }
                    else
                    {
                        mnuSelect.Enabled = false;
                    }
                    mnu.MenuItems.Add(mnuSelect);
                }

                {

                    var mnuSelect = new MenuItem("Paste");
                    if (CopyChords.Chords.Any())
                    {
                        mnuSelect.Enabled = true;
                        mnuSelect.Click += new EventHandler(delegate(object o, EventArgs e)
                        {
                            if (CopyChords.Chords.Any())
                            {
                                if (CurrentSelectionState != TrackEditor.EditorSelectionState.PastingNotes)
                                {
                                    CurrentSelectionState = TrackEditor.EditorSelectionState.PastingNotes;
                                }

                                PasteCopyBuffer(CurrentPastePoint);
                            }

                        });
                    }
                    else
                    {
                        mnuSelect.Enabled = false;
                    }

                    mnu.MenuItems.Add(mnuSelect);
                }

                mnu.Show(this, nx);

                Invalidate();

                return true;

            }

            return false;
        }



        bool IsControlKeyDown
        {
            get
            {
                return ((ModifierKeys & Keys.Control) == Keys.Control);
            }
        }
        bool IsAltKeyDown
        {
            get
            {
                return ((ModifierKeys & Keys.Alt) == Keys.Alt);
            }
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Capture == false)
            {
                Capture = true;
            }


            if (DesignMode || InPlayback || guitarTrack == null || IsLoaded == false)
                return;


            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SelectStartPoint = PointToClient(MousePosition);
            }

            if (EditorType != EEditorType.Guitar5)
            {
                Editor5.ClearSelection();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right &&
                EditorType != EEditorType.Guitar5)
            {
                if (CurrentSelectionState == EditorSelectionState.MovingNotes)
                {
                    CurrentSelectionState = EditorSelectionState.Idle;
                    RestoreBackup();
                    Invalidate();
                    return;
                }
                else
                {
                    HandleProRightMouseMenu(e);

                    CurrentSelectionState = EditorSelectionState.Idle;
                    Invalidate();
                    return;
                }
            }
            else if (EditorType == EEditorType.Guitar5)
            {
                if (OnMouseDown5Tar(e))
                {
                    Invalidate();
                    return;
                }
            }

            bool handledEvent = false;

            if (EditorType != EEditorType.Guitar5)
            {
                if (CurrentSelectionState == EditorSelectionState.Idle)
                {
                    if (OnMouseDownEvent != null)
                    {
                        handledEvent = OnMouseDownEvent(this, e);
                    }


                    if (!handledEvent && !IsControlKeyDown && !IsAltKeyDown)
                    {
                        var mouseChord = GetChordFromPoint(PointToClient(MousePosition));
                        if (NumSelectedChords == 0 || mouseChord == null || mouseChord != SelectedChord)
                        {

                        }
                        else
                        {
                            if (EditMode == EEditMode.Chords)
                            {
                                if (mouseChord != null && SelectedChord != null &&
                                    mouseChord == SelectedChord)
                                {
                                    UpdateSelectorVisibility();

                                    if (MouseOverSelector)
                                    {
                                        foreach (var sel in visibleSelectors)
                                        {
                                            if (sel.GetCurrentRect(this).Contains(PointToClient(MousePosition)))
                                            {
                                                CurrentSelectionState = EditorSelectionState.MovingSelector;
                                                currentSelector = sel;
                                            }
                                        }
                                        if (CurrentSelectionState != EditorSelectionState.MovingSelector)
                                        {
                                            CurrentSelectionState = EditorSelectionState.Idle;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!handledEvent &&
                (CurrentSelectionState == EditorSelectionState.Idle ||
                CurrentSelectionState == EditorSelectionState.MovingSelector))
            {

                var mc = GetChordFromPoint(PointToClient(MousePosition));
                if (mc == null)
                {
                    if (CurrentSelectionState == EditorSelectionState.Idle)
                    {
                        if (!handledEvent)
                        {
                            CurrentSelectionState = EditorSelectionState.SelectingBox;
                        }
                    }
                }
                else
                {

                    if (CurrentSelectionState == EditorSelectionState.Idle)
                    {
                        if (IsControlKeyDown)
                        {
                            mc.Selected = true;
                        }
                        else if (IsAltKeyDown)
                        {
                            mc.Selected = false;
                        }
                        else
                        {
                            SetSelectedChord(mc, true, mc.Selected);

                            visibleSelectors = GetVisibleSelectors();
                        }
                    }
                }

                SelectCurrentPoint = SelectStartPoint;

            }
            UpdateSelectorVisibility();
            Invalidate();

        }

        void DrawChord(GuitarChord chord, Graphics g, bool drawSelected)
        {

            foreach (var note in chord.Notes)
            {
                var i = note.NoteString;

                var startEnd = new TickPair(GetClientPointFromTime(note.StartTime), GetClientPointFromTime(note.EndTime));


                if (CurrentSelectionState == EditorSelectionState.MovingSelector)
                {
                    var sel = CurrentSelector;

                    if (sel != null &&
                        chord.Selected && drawSelected)
                    {
                        if (sel.IsRight)
                        {
                            startEnd.Up = sel.CurrentPoint.X + Utility.SelectorWidth;

                        }
                        else
                        {
                            startEnd.Down = sel.CurrentPoint.X;
                        }
                    }
                }

                int noteX = startEnd.Down;
                int noteY = GetNoteMinYOffset(note);

                var width = startEnd.Up - startEnd.Down;


                if (CurrentSelectionState != EditorSelectionState.MovingSelector)
                {
                    if (drawSelected == false)
                    {
                        if (width < Utility.MinimumNoteWidth)
                        {
                            width = Utility.MinimumNoteWidth;
                        }
                    }
                }

                var noteRect = new Rectangle(noteX, noteY, width, NoteHeight);

                if (width > 0)
                {

                    int minus = 2;
                    int plus = 5;
                    if (chord.Selected)
                    {
                        minus = 1;
                        plus = 3;
                    }
                    if ((chord.StrumMode & ChordStrum.Low) > 0)
                    {
                        if (i == 0 || i == 1)
                        {
                            g.FillRectangle(Utility.noteStrumBrush,
                                noteX - minus, noteY - minus,
                                width + plus,
                                NoteHeight + plus);
                        }
                    }
                    if ((chord.StrumMode & ChordStrum.Mid) > 0)
                    {
                        if (i == 2 || i == 3)
                        {
                            g.FillRectangle(Utility.noteStrumBrush,
                                noteX - minus, noteY - minus,
                                width + plus,
                                NoteHeight + plus);
                        }
                    }

                    if ((chord.StrumMode & ChordStrum.High) > 0)
                    {
                        if (i == 4 || i == 5)
                        {
                            g.FillRectangle(Utility.noteStrumBrush,
                                noteX - minus, noteY - minus,
                                width + plus,
                                NoteHeight + plus);
                        }
                    }

                    var sb = Utility.noteBGBrush;
                    string noteTxt = note.NoteFretDown.ToString();
                    if (note.IsArpeggioNote)
                    {
                        noteTxt = Utility.ArpeggioHelperPrefix + noteTxt + Utility.ArpeggioHelperSuffix;
                        sb = Utility.noteArpeggioBrush;

                    }

                    if (chord.IsXNote)
                    {
                        sb = Utility.noteXBrush;
                    }
                    else if (note.IsTapNote)
                    {
                        sb = Utility.noteTapBrush;
                    }

                    g.FillRectangle(sb,
                                noteX, noteY,
                                width, NoteHeight);


                    if (width > 3)
                    {

                        var rectPen = Utility.noteBoundPen;
                        g.DrawRectangle(rectPen, noteRect);

                        var len = GetClientPointFromTick(chord.UpTick) - GetClientPointFromTick(chord.DownTick);

                        if (len > 4)
                        {
                            if (chord.HasHammeron && chord.HasSlide)
                            {
                                using (var slb = new SolidBrush(Color.FromArgb(Utility.hammerOnPen.Color.A, Utility.hammerOnPen.Color)))
                                {
                                    g.FillPolygon(slb, new Point[] { new Point(noteX, noteY), new Point(noteX + width, noteY), new Point(noteX, noteY + NoteHeight) });
                                }
                                using (var slb = new SolidBrush(Color.FromArgb(Utility.slidePen.Color.A, Utility.slidePen.Color)))
                                {
                                    g.FillPolygon(slb, new Point[] { new Point(noteX + width, noteY), new Point(noteX + width, noteY + NoteHeight), new Point(noteX, noteY + NoteHeight) });
                                }
                            }
                            else if (chord.HasHammeron)
                            {
                                using (var slb = new SolidBrush(Color.FromArgb(Utility.hammerOnPen.Color.A, Utility.hammerOnPen.Color)))
                                {
                                    g.FillRectangle(slb, noteRect);
                                }
                            }
                            else if (chord.HasSlide)
                            {
                                using (var slb = new SolidBrush(Color.FromArgb(Utility.slidePen.Color.A, Utility.slidePen.Color)))
                                {
                                    g.FillRectangle(slb, noteRect);
                                }
                            }


                            if (width > 4)
                            {

                                using (var slb = new Pen(Color.FromArgb(80, Color.White)))
                                {
                                    g.DrawLine(slb, new Point(noteX, noteY + 1), new Point(noteX + width - 1, noteY + 1));
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 1), new Point(noteX + 1, noteY + NoteHeight - 1));
                                }

                                using (var slb = new Pen(Color.FromArgb(40, Color.White)))
                                {
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 2), new Point(noteX + width - 1, noteY + 2));
                                    g.DrawLine(slb, new Point(noteX + 2, noteY + 1), new Point(noteX + 1, noteY + NoteHeight - 1));
                                }

                                using (var slb = new Pen(Color.FromArgb(40, Color.White)))
                                {
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 3), new Point(noteX + 4, noteY + 3));
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 4), new Point(noteX + 3, noteY + 4));
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 5), new Point(noteX + 2, noteY + 5));
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + 6), new Point(noteX + 2, noteY + 6));
                                }

                                using (var slb = new Pen(Color.FromArgb(40, Color.Black)))
                                {
                                    g.DrawLine(slb, new Point(noteX + width + 1, noteY + 1), new Point(noteX + width + 1, noteY + NoteHeight + 1));
                                    g.DrawLine(slb, new Point(noteX + 1, noteY + NoteHeight + 1), new Point(noteX + width, noteY + NoteHeight + 1));
                                }
                                using (var slb = new Pen(Color.FromArgb(10, Color.Black)))
                                {
                                    g.DrawLine(slb, new Point(noteX + width + 2, noteY + 2), new Point(noteX + width + 2, noteY + NoteHeight + 2));
                                    g.DrawLine(slb, new Point(noteX + 3, noteY + NoteHeight + 2), new Point(noteX + width + 1, noteY + NoteHeight + 2));
                                }

                                float fontSize = (float)NoteHeight;

                                if (width < fontSize)
                                {
                                    fontSize = width;
                                }

                                if (fontSize / 2.0f - (int)(fontSize / 2.0f) > 0)
                                {
                                    fontSize = fontSize - 1;
                                }


                                var font = GetFontForRect(noteRect);

                                if (font != null)
                                {
                                    if (chord.IsXNote && Utility.XNoteText.Trim().IsNotEmpty())
                                    {
                                        noteTxt = Utility.XNoteText.Trim();
                                    }
                                    if (note.IsArpeggioNote)
                                    {
                                        noteY--;
                                    }
                                    noteX += Utility.NoteTextXOffset;
                                    noteY += Utility.NoteTextYOffset;

                                    g.DrawString(noteTxt,
                                        font,
                                        Utility.fretBrush,
                                        new RectangleF(new PointF(noteX, noteY - 1), new SizeF(width + 20, NoteHeight)),
                                         Utility.GetStringFormatNoWrap());
                                }
                            }
                        }
                    }
                    if (chord.Selected)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(200, Utility.noteBGBrushSel.Color)))
                        {
                            g.FillRectangle(Utility.noteBGBrushSel, noteRect);
                        }
                    }
                }
            }
        }

        List<KeyValuePair<Rectangle, Font>> fontBuffer = new List<KeyValuePair<Rectangle, Font>>();
        Font GetFontForRect(Rectangle rect)
        {
            Font ret = Utility.fretFont;

            var item = fontBuffer.Where(x => x.Key.Height == rect.Height);

            if (item.Any())
            {
                ret = item.Single().Value;
            }
            else
            {
                var font = CreateFontForRect(rect);
                if (font != null)
                {
                    fontBuffer.Add(new KeyValuePair<Rectangle, Font>(rect, font));
                    ret = font;
                }
            }

            return ret;
        }

        Font CreateFontForRect(Rectangle rect)
        {
            Font ret = null;

            if (rect.Width < 5 || rect.Height < 5)
                return ret;


            var g = Graphics.FromHwnd(this.Handle);

            rect.Height += 2;
            float initialSize = rect.Height - 2;

            float currentSize = initialSize;

            var fmt = Utility.GetStringFormatNoWrap();

            for (int x = 0; x < 10; x++)
            {
                var font = new Font(this.Font.FontFamily, currentSize, FontStyle.Regular, GraphicsUnit.Pixel);

                var height = font.GetHeight(g);

                var measure = g.MeasureString("8", font, rect.Height, fmt);


                var diff = Math.Abs(measure.Height - rect.Height);
                if (diff > 0.1)
                {
                    if (measure.Height > rect.Height)
                    {
                        currentSize -= diff / 2.0f;
                    }
                    else if (measure.Height < rect.Height)
                    {
                        currentSize += diff / 2.0f;
                    }
                    font.Dispose();
                    font = null;
                }
                else
                {
                    ret = font;
                    break;
                }
            }
            return ret;
        }


        public PastePointParam CurrentPastePoint = new PastePointParam();


        public bool IsClientPointVisible(TickPair pair)
        {
            return pair.Up >= 0 && pair.Down <= ClientSize.Width;
        }

        void DrawPasteChords(Graphics g)
        {

            foreach (var c in CopyChords.Chords)
            {
                var clientPair = GetClientPointFromScreenPoint(c.ScreenPointPair);

                if (IsClientPointVisible(clientPair))
                {
                    foreach (GuitarNote note in c.Notes)
                    {
                        int noteY = GetScreenPointForString(CurrentPastePoint, note);

                        var rect = new Rectangle(clientPair.Down, noteY, clientPair.Up - clientPair.Down, NoteHeight);
                        DrawCopyChord(g, 120, note, clientPair.Down, noteY, rect);
                    }
                }
            }
        }

        private int GetScreenPointForString(PastePointParam param, GuitarNote note)
        {
            int noteY = TopLineOffset + LineSpacing *
                (5 - (param.MousePos.Y - param.Offset.Y + note.NoteString - param.MinNoteString)) -
                NoteHeight / 2;
            return noteY;
        }

        private static void DrawCopyChord(Graphics g, int alpha, GuitarNote note, int noteX, int noteY, Rectangle rect)
        {

            using (var p = new Pen(Color.FromArgb(alpha, Utility.noteBGBrushSel.Color)))
            {
                g.DrawRectangle(p, rect);
            }

            Color bgColor = Utility.noteBGBrush.Color;
            if (note.Channel == Utility.ChannelX)
            {
                bgColor = Utility.noteXBrush.Color;
            }
            else if (note.Channel == Utility.ChannelTap)
            {
                bgColor = Utility.noteTapBrush.Color;
            }
            using (var sb = new SolidBrush(Color.FromArgb(alpha, bgColor)))
            {
                g.FillRectangle(sb, rect);
            }


            using (var rectPen = new Pen(Color.FromArgb(alpha, Utility.noteBoundPen.Color)))
            {
                g.DrawRectangle(rectPen, rect);
            }


            using (var selPen = new Pen(Color.FromArgb(alpha, Utility.selectedPen.Color)))
            {
                g.DrawRectangle(selPen, rect);
            }

            using (var fb = new SolidBrush(Color.FromArgb(alpha, Utility.fretBrush.Color)))
            {

                var noteTxt = note.NoteFretDown.ToString();
                if (note.IsXNote && Utility.XNoteText.Trim().IsNotEmpty())
                {
                    noteTxt = Utility.XNoteText.Trim();
                    noteX += Utility.XNoteTextXOffset;
                    noteY += Utility.XNoteTextYOffset;
                }
                else
                {
                    noteX += Utility.NoteTextXOffset;
                    noteY += Utility.NoteTextYOffset;
                }

                g.DrawString(noteTxt,
                    Utility.fretFont,
                    fb,
                    noteX,
                    noteY - (int)(Utility.fontHeight / 4.0 + Utility.NoteTextYOffset));
            }

        }
        private void DrawChords6(Graphics g, IEnumerable<GuitarChord> vis, bool selected)
        {

            foreach (var chord in vis)
            {
                if (chord.Selected != selected)
                {
                    continue;
                }

                DrawChord(chord, g, selected);
            }

            if (selected && NumSelectedChords == 1)
            {
                DrawSelector(g);
            }
        }

        private void DrawSelector(Graphics g)
        {

            if (EditorType == EEditorType.Guitar5 ||
                ClientRectangle.Contains(PointToClient(MousePosition)) == false)
            {
                return;
            }
            if (CurrentSelectionState == EditorSelectionState.MovingSelector)
            {
                if (CurrentSelector != null)
                {
                    DrawSelector(g, CurrentSelector);
                }
            }
            else if (CurrentSelectionState == EditorSelectionState.Idle)
            {
                if (visibleSelectors != null &&
                    visibleSelectors.Count > 0 &&
                    SelectedChords.Count == 1)
                {

                    if (IsMouseOver)
                    {
                        foreach (var sel in visibleSelectors)
                        {
                            DrawSelector(g, sel);
                        }
                    }
                }

            }
        }

        public Rectangle GetScreenRectFromMessage(GuitarMessage message)
        {
            var minMaxY = new Point(0, Height);
            if (message is GuitarChord)
            {
                minMaxY = GetChordYMinMax(message as GuitarChord);
            }
            var start = GetClientPointFromTick(message.DownTick);
            var end = GetClientPointFromTick(message.UpTick);

            return new Rectangle(start, minMaxY.X, end - start, minMaxY.Y - minMaxY.X);
        }

        List<Selector> GetVisibleSelectors()
        {

            var ret = new List<Selector>();
            var chord = SelectedChord;
            if ((SelectedChord != null && SelectedChord.IsDeleted) || chord == null || chord.HasNotes == false)
                return ret;

            Point p = PointToClient(MousePosition);

            var minMaxY = GetChordYMinMax(chord);
            var start = GetClientPointFromTick(chord.DownTick);
            var end = GetClientPointFromTick(chord.UpTick);

            var chordScreen = new Rectangle(start, minMaxY.X, end - start, minMaxY.Y - minMaxY.X);


            var distC = p.Distance(chordScreen);

            if (distC <= Utility.ShowSelectorDist)
            {

                for (int sl = 0; sl < 2; sl++)
                {
                    var selector = new Rectangle(
                        chordScreen.X,
                        chordScreen.Top,
                        Utility.SelectorWidth,
                        chordScreen.Height);

                    Point startPos;

                    if (sl == 0)
                    {
                        selector.X += chordScreen.Width - Utility.SelectorWidth;

                        startPos = new Point(
                            chordScreen.Right - selector.Width,
                            chordScreen.Top);
                    }
                    else
                    {
                        startPos = new Point(chordScreen.X,
                            chordScreen.Top);
                    }

                    var sel = new Selector()
                    {
                        StartPoint = startPos,
                        CurrentPoint = startPos,
                        Chord = SelectedChord,
                        IsRight = sl == 0
                    };

                    Rectangle r = sel.GetCurrentRect(this);

                    var dist = p.Distance(r);

                    sel.IsMouseOver = false;
                    if (dist <= Utility.ShowSelectorDist)
                    {
                        sel.IsMouseNear = true;
                        if (r.Contains(p))
                        {
                            sel.IsMouseOver = true;

                        }
                    }
                    ret.Add(sel);
                }
            }

            return ret;
        }

        void DrawSelector(Graphics g,
            Selector sel)
        {

            var c = Utility.noteBGBrushSel.Color;
            var color = Color.FromArgb(40, 80, 40);
            if (sel == null || sel.Chord.HasNotes == false)
                return;

            var trans = sel.IsMouseOver ? 205 : sel.IsMouseNear ? 120 : 80;

            var r = sel.GetCurrentRect(this);

            using (Pen pn = new Pen(Color.FromArgb(trans, color), 1.0f))
            {
                int w = 3;
                if (sel.IsMouseOver == false)
                    w = 2;
                for (int j = 0; j < w; j++)
                {
                    Point[] p = new Point[2];
                    if (sel.IsRight)
                    {
                        p[0] = new Point(
                            r.X + Utility.SelectorWidth - 1 + j,
                            r.Y - 1);
                        p[1] = new Point(
                            r.X + Utility.SelectorWidth - 1 + j,
                            r.Y + r.Height + 2);
                    }
                    else
                    {
                        p[0] = new Point(
                             r.X - 1 + j,
                            r.Y - 1);
                        p[1] = new Point(
                            r.X - 1 + j,
                            r.Y + r.Height + 2);
                    }

                    g.DrawLine(pn, p[0], p[1]);
                }

                g.DrawLine(pn,
                    r.X,
                    r.Y - 1,
                    r.X + r.Width,
                    r.Y - 2);
                g.DrawLine(pn,
                    r.X,
                    r.Y + r.Height + 2,
                    r.X + r.Width,
                    r.Y + r.Height + 2);
            }
        }

        private void DrawModifiers6(Graphics g, IEnumerable<GuitarMessage> vis)
        {

            foreach (var bre in vis.GetMessages(GuitarMessageType.GuitarBigRockEnding))
            {
                var brush = Utility.noteBREBrush;
                if (bre.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, bre.TickPair);
            }
            foreach (var solo in vis.GetMessages(GuitarMessageType.GuitarSolo))
            {
                var brush = Utility.noteSoloBrush;
                if (solo.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, solo.TickPair);
            }

            foreach (var pwer in vis.GetMessages(GuitarMessageType.GuitarPowerup))
            {
                var brush = Utility.notePowerupBrush;
                if (pwer.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                FillRect(g, brush, pwer.TickPair);
            }
            foreach (var msg in vis.GetMessages(GuitarMessageType.GuitarSingleStringTremelo))
            {
                var brush = Utility.noteSingleStringTremeloBrush;
                if (msg.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                var chords = vis.GetMessages(GuitarMessageType.GuitarChord).Cast<GuitarChord>();
                if (chords.Any())
                {
                    int minString = chords.Min(x => x.Notes.Min(n => n.NoteString));
                    int maxString = chords.Max(x => x.Notes.Max(n => n.NoteString));

                    if (minString != Int32.MaxValue && maxString != Int32.MinValue)
                    {
                        DrawRect(g, brush, msg.TickPair, minString, maxString);
                    }
                }
                else
                {
                    FillRect(g, brush, msg.TickPair);
                }
            }

            foreach (var msg in vis.GetMessages(GuitarMessageType.GuitarMultiStringTremelo))
            {
                var brush = Utility.noteMultiStringTremeloBrush;
                if (msg.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                var msgs = vis.GetBetweenTick(msg.TickPair).Where(x => x is GuitarChord).Cast<GuitarChord>();

                var chords = vis.GetMessages(GuitarMessageType.GuitarChord).Cast<GuitarChord>();
                if (chords.Any())
                {
                    int minString = chords.Min(x => x.Notes.Min(n => n.NoteString));
                    int maxString = chords.Max(x => x.Notes.Max(n => n.NoteString));

                    if (minString != Int32.MaxValue && maxString != Int32.MinValue)
                    {
                        DrawRect(g, brush, msg.TickPair, minString, maxString);
                    }
                }
                else
                {
                    FillRect(g, brush, msg.TickPair);
                }
            }
            foreach (var arp in vis.GetMessages(GuitarMessageType.GuitarArpeggio))
            {
                var brush = Utility.noteArpeggioBrush;
                if (arp.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, arp.TickPair);
            }
        }

        private void DrawTabLines(Graphics g, Pen p)
        {

            int lineOffset = TopLineOffset;
            using (Pen pa = new Pen(Color.FromArgb(10, p.Color), 3.0f))
            {
                for (int x = 0; x < 6; x++)
                {
                    g.DrawLine(pa, 0, lineOffset, Width, lineOffset);
                    g.DrawLine(p, 0, lineOffset, Width, lineOffset);
                    lineOffset += LineSpacing;
                }
            }

        }

        public int InnerHeight { get { return (this.ClientSize.Height - this.HScroll.Height); } }
        public int LineSpacing { get { return InnerHeight / 6; } }

        public int NoteHeight { get { return LineSpacing - LineSpacing / 4; } }

        public int TopLineOffset { get { return (int)(NoteHeight * 0.75); } }

    }
}