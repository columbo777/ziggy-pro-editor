using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using ProUpgradeEditor.DataLayer;
using ProUpgradeEditor.Common;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProUpgradeEditor
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
            get { return guitarTrack.IsLoaded; }
        }
        
        public void AddTrack(Track t)
        {
            if(IsLoaded)
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

            
                HScroll = new HScrollBar();
                HScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
                HScroll.SmallChange = Utility.HScrollSmallChange;
                HScroll.LargeChange = Utility.HScrollLargeChange;

                this.Controls.Add(HScroll);

                
            }
        }

        
        public GuitarMessageList Messages
        {
            get { return guitarTrack == null ? null : guitarTrack.Messages; }
        }

        

        new HScrollBar HScroll;
        public int LastHScrollValue = 0;


        public void AddScrollHandler()
        {
            HScroll.Tag = HScroll.Value;
            LastHScrollValue = HScroll.Value;
            HScroll.ValueChanged += delegate(object o, EventArgs e)
            {
                try
                {
                    var value = HScrollValue;
                    if (this == Editor5)
                    {
                        if (Editor5.HScrollValue != EditorPro.HScrollValue)
                        {
                            EditorPro.HScrollValue = value;
                        }
                    }
                    else if (this == EditorPro)
                    {
                        if (Editor5.HScrollValue != EditorPro.HScrollValue)
                        {
                            Editor5.HScrollValue = value;
                        }
                    }

                    LastHScrollValue = HScrollValue;
                    UpdateSelectorVisibility();
                    Editor5.Invalidate();
                    EditorPro.Invalidate();

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
                    }
                }
                catch { }
            }
        }

        public void SetHScrollMaximum(int max)
        {
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
            guitarTrack = new GuitarTrack(isPro);
        }

        EEditorType editorType = EEditorType.None;
        public EEditorType EditorType
        {
            get { return editorType; }
            set { editorType = value; }
        }

        
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
        
        GuitarChord selectStartPointChord;
        public GuitarChord SelectStartPointChord
        {
            get { return selectStartPointChord; }

            set { selectStartPointChord = value;  }
        }

        Point selectCurrentPoint;
        public Point SelectCurrentPoint
        {
            get { return selectCurrentPoint; }
            set { selectCurrentPoint = value;  }
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
        
        public int NumRedoBackups { get { return redoSequences.Count; } }

        public void CreateRedoBackup()
        {
            try
            {
                if (NumBackups > 0)
                {
                    var ms = new MemoryStream(backupSequences[backupSequences.Count - 1].GetBuffer());
                    ms.Seek(0, SeekOrigin.Begin);
                    redoSequences.Add(ms);
                }
            }
            catch { }
        }

        public bool RedoBackup()
        {
            try
            {
                if (NumRedoBackups > 0)
                {
                    var ms = redoSequences[redoSequences.Count - 1];
                    backupSequences.Add(ms);
                    redoSequences.RemoveAt(redoSequences.Count-1);

                    RestoreBackup();
                }
                return true;
            }
            catch 
            {
                RestoreBackup();
            }
            return false;
        }

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
        List<MemoryStream> redoSequences = new List<MemoryStream>();
        
        public void ClearBackups()
        {
            backupSequences.Clear();
            redoSequences.Clear();
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

                    while (redoSequences.Count > Utility.MaxBackups)
                    {
                        redoSequences.RemoveAt(0);
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
                            if (gt6 != null && !string.IsNullOrEmpty(gt6.Name))
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
            }

            return ret;
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

        public bool RestoreBackup(int recursion=0)
        {

            bool ret = true;
            try
            {
                if (backupSequences.Count > 0 && recursion < 2)
                {
                    try
                    {
                        var mso = backupSequences[backupSequences.Count - 1];
                        mso.Seek(0, SeekOrigin.Begin);

                        ret = LoadMidi17("", mso.ToArray(), true);
                    }
                    catch
                    {
                        ret = false;
                    }
                    finally
                    {
                        backupSequences.RemoveAt(backupSequences.Count - 1);
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }
            
            if (ret == false)
            {
                ret = RestoreBackup(recursion + 1);
            }
            

            return ret;
        }

        

        public void SaveTrack(string fileName)
        {
            
            if (Sequence != null)
            {
                Sequence.Save(fileName);
            }
            
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
                difficulty = GuitarDifficulty.Expert;
            }
            try
            {
                guitarTrack.SetTrack(t, difficulty);
                ret = guitarTrack.IsLoaded;
            }
            catch{}

            try
            {
                if (ret == true && OnLoadTrack != null)
                {
                    OnLoadTrack(this, seq, t);
                }
            }
            catch{}

            try
            {
                SetTrackMaximum();
            }
            catch{}

            settingTrack5 = false;
            
            return ret;
        }

        public bool SetTrack6(Track t, GuitarDifficulty difficulty=GuitarDifficulty.Unknown)
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
                    difficulty = GuitarDifficulty.Expert;
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
            

            return ret;
        }

        private void SetTrackMaximum()
        {
            try
            {
                BeginInvoke(new Action<TrackEditor>(delegate(TrackEditor editor)
                {
                    try
                    {
                        if (!IsLoaded)
                            return;
                        var v = editor.HScroll.Value;
                        editor.SetHScrollMaximum((int)Math.Round(Utility.ScaleTimeUp(editor.GuitarTrack.TotalSongTime)));
                        if (v < HScrollMaximum)
                        {
                            editor.HScrollValue = v;
                        }
                        else
                        {
                            editor.HScrollValue = editor.HScrollMaximum;
                        }
                    }
                    catch { }
                }), this);
            }
            catch { }
        }

        public bool ZoomedOutFar
        {
            get { return Utility.timeScalar <= 90; }
        }
        public bool ZoomedOutRealFar
        {
            get { return Utility.timeScalar <= 35; }
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

                g.FillRectangle(Utility.BackgroundBrush, clipRect);

                if (IsLoaded == false)
                {

                    if (!ZoomedOutFar)
                    {
                        DrawTabLines(g, Utility.linePen);
                    }

                    return;
                }

                if (EditorType == EEditorType.Guitar5)
                {
                    Draw5Tar(g, clipRect);
                }
                else if (EditorType == EEditorType.ProGuitar)
                {
                    Draw22Tar(g, clipRect);
                }
                else
                {
                    DrawTabLines(g, Utility.linePen);
                }
                
                if (InPlayback)
                {
                    var pos = GetScreenPointFromTick(MidiPlaybackPosition);

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
                return GetTickFromScreenPoint(0);
            }
        }

        public int MaxVisible
        {
            get
            {
                return GetTickFromScreenPoint(Width);
            }
        }

        public IEnumerable<GuitarChord> GetVisibleChords()
        {
            IEnumerable<GuitarChord> ret = null;
            if (IsLoaded)
            {
                ret = guitarTrack.Messages.Chords.GetChordsAtTick(MinVisible, MaxVisible);
            }
            return ret;
        }

        void Draw5Tar(Graphics g, Rectangle clipRect)
        {
            try
            {
                if (guitarTrack == null || guitarTrack.Messages == null)
                    return;

                
                var vis = guitarTrack.GetMessagesAtTick(
                    GetTickFromScreenPoint(clipRect.Left), 
                    GetTickFromScreenPoint(clipRect.Right));

                var visChords = vis.GetMessages<GuitarChord>();
                var visTrainer = vis.GetMessages<GuitarTrainer>();
                var visText = vis.GetMessages<GuitarTextEvent>();
                var visTempo = vis.GetMessages<GuitarTempo>();
                var visMod = vis.GetMessages<GuitarModifier>();

                if (EditMode != EEditMode.Events)
                {
                    DrawTrainers(g, false, visTrainer);
                    DrawTrainers(g, true, visTrainer);

                    DrawTextEvents(g, false, false, visText);

                    DrawTransparentBackgroundOverlay(g, 40, clipRect);

                    DrawTabLines(g, Utility.linePen);


                    DrawModifiers6(g, visMod);
                    DrawBeatNoteGrid5(g, visTempo);

                    foreach (var c in visChords)
                    {
                        DrawChords5(g, c);
                    }
                }
                else
                {

                    DrawModifiers6(g, visMod);
                    DrawBeatNoteGrid5(g, visTempo);

                    
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

        private void DrawBeatNoteGrid5(Graphics g, IEnumerable<GuitarTempo> vis)
        {

            if (!ZoomedOutFar && Utility.DrawBeat != 0)
            {
                foreach (var c in vis)
                {
                    DrawBeat5(g, c);
                }
            }

            if (!ZoomedOutFar && ShowNotesGrid)
            {
                foreach (var m in vis)
                {
                    DrawNoteGrid(g, m);
                }
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

        private void DrawBeat5(Graphics g, GuitarTempo bp)
        {
            if (bp != null)
            {
                var nb = Utility.beatPen;

                double ct = bp.StartTime;

                

                while (true)
                {
                    double increment = guitarTrack.SecondsPerBeatQuarterNote(bp.DownTick);

                    increment /= 4.0;
                    increment *= guitarTrack.GetTimeSignature(bp.DownTick).Numerator;
                    
                    var st = GetScreenPointFromTime(ct);
                    var se = GetScreenPointFromTime(bp.EndTime);

                    if (st >= se)
                        break;

                    var stH = st;
                    if (stH > 0 && stH < this.Width)
                    {
                        g.DrawLine(nb, stH, 0, stH, this.Height);
                    }
                    if (stH >= this.Width)
                        break;

                    ct += increment;
                }
                ct = bp.StartTime;
                
                nb = Utility.barPen;
                while (true)
                {
                    double increment = guitarTrack.SecondsPerBar(bp.DownTick);
                    
                    var st = GetScreenPointFromTime(ct);
                    var se = GetScreenPointFromTime(bp.EndTime);

                    if (st >= se)
                        break;

                    var stH = st ;
                    if (stH > 0 && stH < this.Width)
                    {
                        g.DrawLine(nb, stH+1, 0, stH+1, this.Height);
                    }
                    if (stH >= this.Width)
                        break;

                    ct += increment;
                }
            }
        }


        private void DrawNoteGrid(Graphics g, GuitarTempo bp)
        {
            if (bp != null)
            {
                var nb = Utility.beatPen;

                double ct = bp.StartTime;

                
                while (true)
                {
                    double increment = guitarTrack.SecondsPerBeat(bp.DownTick,GridScalar);
                    
                    var stH = GetScreenPointFromTime(ct);

                    var se = GetScreenPointFromTime(bp.EndTime);

                    if (stH >= se)
                        break;

                    if (stH > 0 && stH < this.Width)
                    {
                        g.DrawLine(nb, stH, 0, stH, this.Height);
                    }
                    if (stH >= this.Width)
                        break;

                    ct += increment;
                }
            }
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

                    

                    var downTime = GetScreenPointFromTick(note.DownTick);
                    var upTime = GetScreenPointFromTick(note.UpTick);

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
                    if (ZoomedOutRealFar)
                    {
                        boundPen = new Pen(Color.FromArgb(40, Utility.noteBoundPen.Color));
                    }
                    g.DrawRectangle(boundPen,
                                downTime,
                                GetNoteMinYOffset(note),
                                width, NoteHeight);

                    if (ZoomedOutRealFar)
                    {
                        boundPen.Dispose();
                    }
                    
                }
            }
        }

        void DrawRect(Graphics g, Pen pen, Rectangle rect)
        {
            g.DrawRectangle(pen, rect);
        }

        void FillRect(Graphics g, Brush b, int downTick, int upTick)
        {
            downTick = GetScreenPointFromTick(downTick);
            upTick = GetScreenPointFromTick(upTick);

            g.FillRectangle(b,
                        downTick,
                        0, upTick - downTick,
                        Height - HScroll.Height - 1);

            if (!ZoomedOutRealFar)
            {

                using (var pen = new Pen(Color.FromArgb(30, Utility.noteBoundPen.Color)))
                {
                    g.DrawRectangle(pen,
                        downTick,
                        0, upTick - downTick,
                        Height - HScroll.Height - 1);
                }
            }
        }
        void DrawRect(Graphics g, Brush b, int downTick, int upTick, int minString, int maxString)
        {
            var maxScreen = TopLineOffset + LineSpacing * (5 - minString) + NoteHeight-2;
            var minScreen = TopLineOffset + LineSpacing * (5 - maxString) - NoteHeight+2;

            g.FillRectangle(b,
                        downTick ,
                        minScreen, upTick - downTick,
                        maxScreen - minScreen );

            g.DrawRectangle(Utility.noteBoundPen,
                downTick,
                minScreen, upTick - downTick,
                maxScreen - minScreen);
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
            
            return ret;
        }



        public void ScrollToSelection()
        {
            var gc = SelectedChord;
            if (gc != null)
            {
                int i = GetScreenPointFromTick(gc.DownTick) -
                            Utility.ScollToSelectionOffset;

                HScrollValue = (HScrollValue+i).Max(0);
                
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
                

            }
            
        }

        public GuitarChord GetChordFromPoint(Point p)
        {
            GuitarChord ret = null;

            if (guitarTrack == null || guitarTrack.Messages==null)
                return ret;

            var vis = guitarTrack.GetChordsAtTick(MinVisible, MaxVisible);

            foreach (var c in vis)
            {
                
                int chordX = GetScreenPointFromTick(c.DownTick);
                var chordWidth = GetScreenPointFromTick(c.UpTick) - GetScreenPointFromTick(c.DownTick);
                
                if (chordWidth < Utility.MinimumNoteWidth)
                    chordWidth = Utility.MinimumNoteWidth;

                if (p.X < chordX || p.X > chordX + chordWidth)
                    continue;

                var chordMinMaxY = GetChordYMinMax(c);

                var cr = new Rectangle(chordX, chordMinMaxY.X, chordWidth, chordMinMaxY.Y - chordMinMaxY.X);

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
            return c.Notes.Min(n => GetNoteMinYOffset(n));
        }

        public int GetChordMaxYOffset(GuitarChord c)
        {
            return c.Notes.Max(n => GetNoteMaxYOffset(n));
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

            var min = GetTickFromScreenPoint(rect.Left);
            var max = GetTickFromScreenPoint(rect.Right);

            ret = guitarTrack.GetChordsAtTick(min, max).Where(
                c=> c.DownTick < max && 
                    c.UpTick > min && 
                    GetChordMinYOffset(c) < rect.Bottom && 
                    GetChordMaxYOffset(c) > rect.Top).ToList();
                
            return ret;
            
        }

        public void ClearSelection()
        {
            if (guitarTrack == null)
                return;

            SelectedChords.ForEach(x => x.Selected = false);
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

            if (guitarTrack == null)
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
                else if(nextIfNotFound==true)
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
        public GuitarChord GetStaleChord(int downTick, int upTick, bool nextIfNotFound)
        {
            GuitarChord ret = null;

            if (guitarTrack == null)
                return null;

            
            var c = guitarTrack.Messages.Chords.SingleOrDefault(
                x => Utility.IsCloseTick(x.DownTick, downTick) &&
                        Utility.IsCloseTick(x.UpTick, upTick));
            if (c != null)
            {
                ret = c;
            }
            else if (nextIfNotFound == true)
            {
                c = guitarTrack.Messages.Chords.FirstOrDefault(
                    x => x.DownTick >= downTick);
                if (c != null)
                {
                    ret = c;
                }
            }
            
            return ret;
        }
        

        

        public GuitarChord SelectNextChord(GuitarChord sel)
        {
            if (guitarTrack == null)
                return null;

            var n = GetNextChord(sel);


            ClearSelection();

            if(sel != null)
            {
                var gc = guitarTrack.Messages.Chords.FirstOrDefault(
                    c => c.IsAfter(sel));

                if (gc != null)
                {
                    gc.Selected = true;
                    sel = gc;
                }
            }
            
            return sel;
        }
        public GuitarChord GetNextChord(GuitarChord sel)
        {
            if (guitarTrack == null)
                return null;

            if (sel != null)
            {
                return guitarTrack.Messages.Chords.FirstOrDefault(
                    x => x.IsAfter(sel));
            }
            return null;
        }

        public GuitarChord GetPreviousChord(GuitarChord sel)
        {
            GuitarChord ret = null;
            if (sel != null)
            {
                ret = guitarTrack.Messages.Chords.LastOrDefault(
                    x => x.IsBefore(sel));
            }

            return ret;
        }

        public GuitarChord SelectedChord
        {
            get
            {
                if (guitarTrack != null)
                {
                    return guitarTrack.Messages.Chords.FirstOrDefault(x =>
                        x.Selected == true);
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
                    ret = guitarTrack.Messages.Chords.Where(x =>
                        x.Selected == true).Cast<GuitarChord>().ToList();

                }

                return ret;
            }
        }

        public int NumSelectedChords
        {
            get
            {
                int ret = 0;
                if (guitarTrack != null)
                {
                    ret = guitarTrack.Messages.Chords.Count(x =>
                        (x is GuitarChord) &&
                        ((GuitarChord)x).Selected == true);
                }
                return ret;
            }
        }
        public double GridScalar
        {
            get;set;
        }

        public int ClosestString(Point p)
        {
            int closestY = int.MaxValue;
            int closestDist = int.MaxValue;
            int ret = 0;
            for (int x = 0; x < 6; x++)
            {
                int noteY = TopLineOffset + LineSpacing * (5 - x);
                if (Math.Abs(p.Y - noteY) < closestDist)
                {
                    closestDist = Math.Abs(p.Y - noteY);
                    closestY = noteY;
                    ret = x;
                }
            }
            return ret;
        }

        List<int> SnapPoints = new List<int>();

        
        public List<GuitarChord> CopyChords = new List<GuitarChord>();
        void Draw22Tar(Graphics g, Rectangle clipRect)
        {
            if (guitarTrack == null)
                return;
            if (guitarTrack.Messages == null)
                return;



            var vis = guitarTrack.GetMessagesAtTick(
                GetTickFromScreenPoint(clipRect.Left),
                GetTickFromScreenPoint(clipRect.Right));

            var visChords = vis.GetMessages<GuitarChord>();
            var visTrainer = vis.GetMessages<GuitarTrainer>();
            var visText = vis.GetMessages<GuitarTextEvent>();
            var visTempo = vis.GetMessages<GuitarTempo>();
            var visMod = vis.GetMessages<GuitarModifier>();
            var visHand = vis.GetMessages<GuitarHandPosition>();

            if (EditMode == EEditMode.Events)
            {
                if (!ZoomedOutRealFar)
                {
                    DrawModifiers6(g, visMod);
                }

                DrawTransparentBackgroundOverlay(g, 20, clipRect);

                DrawTabLines(g, Utility.linePen22);

                DrawBeatAndGrid(g, visTempo);
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
                if (!ZoomedOutFar)
                {
                    Draw108Events(g, false, false, visHand);


                    DrawTrainers(g, false,visTrainer);
                    DrawTrainers(g, true, visTrainer);
                    DrawTextEvents(g, false, false, visText);
                    DrawTextEvents(g, false, true, visText);
                }

                if (EditMode == EEditMode.Modifiers)
                {
                    DrawTransparentBackgroundOverlay(g, 80, clipRect);

                    DrawTabLines(g, Utility.linePen22);
                    DrawChords6(g, visChords, false);
                    DrawChords6(g, visChords, true);
                    DrawTransparentBackgroundOverlay(g, 5, clipRect);

                    DrawModifiers6(g, visMod);

                    DrawBeatAndGrid(g, visTempo);
                }
                else
                {
                    DrawModifiers6(g, visMod);

                    DrawTabLines(g, Utility.linePen22);
                    DrawBeatAndGrid(g, visTempo);

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
                if (CopyChords.Count == 0)
                {
                    CurrentSelectionState = EditorSelectionState.Idle;
                    return;
                }
                UpdatePastePoint();
                foreach (var c in CopyChords)
                {
                    DrawPasteChord(
                        CurrentPastePoint,
                        c, g);
                }

            }

            if (CurrentSelectionState == EditorSelectionState.SelectingBox)
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
                foreach (var item in vis.Where(x=> x.Selected==drawSelected))
                {
                    Draw108Event(g, tabActive, drawSelected, item, item.DownTick, item.UpTick + 2, "Fret: " + item.NoteFretDown,
                        drawSelected ? Utility.SelectedTextEventBrush : Utility.TextEventBrush);
                }
            }
        }

        private void DrawBeatAndGrid(Graphics g, IEnumerable<GuitarTempo> vis)
        {

            if (!ZoomedOutRealFar && Utility.DrawBeat != 0)
            {

                foreach (GuitarTempo m in vis)
                {
                    DrawBeat5(g, m);
                }
            }
            if (!ZoomedOutRealFar && ShowNotesGrid)
            {
                foreach (GuitarTempo m in vis)
                {
                    DrawNoteGrid(g, m);
                }
            }
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

        public void UpdatePastePoint()
        {
            Point p = PointToClient(MousePosition);
            Point offset1 = new Point(0, 0);
            if (SelectStartPointChord != null)
            {
                var strngS = GetStringFromScreenCoord(SelectStartPoint.Y);

                int topPt = -1;
                for (int ij = 0; ij < 6; ij++)
                {
                    if (SelectStartPointChord.Notes[ij] != null &&
                        SelectStartPointChord.Notes[ij].NoteString > topPt)
                        topPt = SelectStartPointChord.Notes[ij].NoteString;
                }

                int offstY = 0;
                if (topPt != strngS)
                {
                    offstY = LineSpacing * (topPt - strngS);
                }

                offset1 = new Point(GetScreenPointFromTick(SelectStartPointChord.DownTick) - SelectStartPoint.X, offstY);
            }
            bool snapped = false;
            if (GridSnap)
            {

                var np = GetGridSnapPointFromClientPoint(new Point(p.X + offset1.X, p.Y + offset1.Y), out snapped);

                if (snapped)
                {
                    p = np;
                    p.X -= offset1.X;
                }
                else
                {
                    var p2 = p;

                    bool snappedgs = false;
                    p2.X = GetTickGridSnap(GetTickFromScreenPoint(p.X + offset1.X) +
                        CopyChords[
                        CopyChords.Count - 1].TickLength,
                        out snappedgs);


                    if (snappedgs)
                    {
                        p.X = GetScreenPointFromTick(p2.X -
                            CopyChords[
                                CopyChords.Count - 1].TickLength);


                        p.X -= offset1.X;
                    }
                }
            }

            int noteString = ClosestString(p);

            int minNoteString = -1;

            int minChordX = int.MaxValue;
            int endChordX = int.MinValue;
            foreach (var c in CopyChords)
            {
                if (GetScreenPointFromTick(c.DownTick) < minChordX)
                    minChordX = (int)GetScreenPointFromTick(c.DownTick);

                if (GetScreenPointFromTick(c.UpTick) > endChordX)
                    endChordX = (int)GetScreenPointFromTick(c.UpTick);

                for (int x = 0; x < 6; x++)
                {
                    var n = c.Notes[x];
                    if (n != null)
                    {
                        if (x > minNoteString)
                        {
                            minNoteString = x;
                        }

                    }
                }
            }



            //CurrentPastePoint.XOffset = (int)Math.Round((p.Y + ProUpgradeEditor.Common.TopLineOffset - NoteHeight) / (double)ProUpgradeEditor.Common.LineSpacing);
            CurrentPastePoint.MinNoteString = minNoteString;


            CurrentPastePoint.MousePos = new Point(p.X, GetNoteStringFromMouse());
            CurrentPastePoint.MinChordX = minChordX;

            Point offset = new Point(0, 0);
            if (SelectStartPointChord != null)
            {
                offset = new Point(GetScreenPointFromTick(SelectStartPointChord.DownTick) - SelectStartPoint.X, -offset1.Y);
            }
            CurrentPastePoint.Offset = offset;
        }

        public List<GuitarTrainer> SelectedProGuitarTrainers
        {
            get { return Messages.Trainers.Where(x => x.Selected && x.TrainerType == GuitarTrainerType.ProGuitar).ToList(); }
        }

        public List<GuitarTrainer> SelectedProBassTrainers
        {
            get { return Messages.Trainers.Where(x => x.Selected && x.TrainerType == GuitarTrainerType.ProBass).ToList(); }
        }

        private void DrawTrainers(Graphics g, bool drawSelected, IEnumerable<GuitarTrainer> trainers)
        {

            foreach (var tr in trainers)
            {
                var st = GetScreenPointFromTick(tr.Start.AbsoluteTicks);
                var et = GetScreenPointFromTick(tr.End.AbsoluteTicks);
                if (et > 0 && st < Width)
                {
                    if (st < 0)
                        st = 0;
                    if (et > Width)
                        et = Width;

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
                        
                        using(var sb = new SolidBrush(Color.FromArgb(
                            EditMode == EEditMode.Events ? tb.Color.A :
                            40,tb.Color)))
                        {
                        
                            g.FillRectangle(sb,
                                st,
                                0, et - st,
                                Height - HScroll.Height - 1);
                        }
                        using(var p = new Pen(tb))
                        {
                            g.DrawRectangle(p, st, 0, et - st, Height - HScroll.Height - 1);
                        }
                    }
                }
                
            }
        }

        class VisibleTextEvent
        {
            public GuitarTextEvent Event { get; set; }
            public RectangleF DrawRect { get; set; }
        }

        class VisibleTextEventContainer
        {
            List<VisibleTextEvent> Events = new List<VisibleTextEvent>();

            public void Clear() { Events.Clear(); }
            public int CountOverlapping(RectangleF rect)
            {
                RectangleF rs = rect;
                rs.Inflate(-2, -2);
                rs.Location = new PointF(rs.Location.X + 1f, rs.Location.Y + 1f);
                return Events.Where(x => x.DrawRect.IntersectsWith(rect)).Count();
            }

            public void Add(GuitarTextEvent ev, RectangleF rect)
            {
                Events.Add(new VisibleTextEvent() { DrawRect = rect, Event = ev });
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

        private void DrawTextEvents(Graphics g,bool tabActive, bool drawSelected, IEnumerable<GuitarTextEvent> textEvents)
        {
            if(drawSelected==false)
                VisibleTextEvents.Clear();

            foreach (var tr in textEvents)
            {

                var pg = SelectedProGuitarTrainers.Any(x=> 
                    x.TrainerType == GuitarTrainerType.ProGuitar && 
                    ((x.Start != null && x.Start.MidiEvent != null && x.Start.MidiEvent == tr.MidiEvent) ||
                    (x.Norm != null && x.Norm.MidiEvent != null && x.Norm.MidiEvent == tr.MidiEvent) ||
                    (x.End != null && x.End.MidiEvent != null && x.End.MidiEvent == tr.MidiEvent)));

                var pb = SelectedProBassTrainers.Any(x=> 
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

                if(drawSelected == isSel)
                {

                    DrawTextEvent(g, tabActive, drawSelected, tr, tb);
                    
                }
            }
        }

        private void DrawTextEvent(Graphics g, bool tabActive, bool drawSelected, GuitarTextEvent tr, SolidBrush tb)
        {

            var st = GetScreenPointFromTick(tr.AbsoluteTicks);
            var et = GetScreenPointFromTick(tr.AbsoluteTicks + 20);
            if (et > 0 && st < Width)
            {
                if (st < 0)
                    st = 0;
                if (et > Width)
                    et = Width;

                g.FillRectangle(tb,
                    st,
                    0, et - st,
                    Height - HScroll.Height - 1);
                using (var p = new Pen(tb))
                {
                    g.DrawRectangle(p, st, 0, et - st, Height - HScroll.Height );
                }

                if (!ZoomedOutFar)
                {
                    var size = g.MeasureString(tr.Text, Utility.fretFont);
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


        private void Draw108Event(Graphics g, bool tabActive, bool drawSelected,
            GuitarHandPosition ev, int downTick, int upTick, string text, SolidBrush tb)
        {

            var st = GetScreenPointFromTick(downTick);
            var et = GetScreenPointFromTick(upTick);
            if (et > 0 && st < Width)
            {
                if (st < 0)
                    st = 0;
                if (et > Width)
                    et = Width;

                g.FillRectangle(tb,
                    st,
                    0, et - st,
                    Height - HScroll.Height - 1);
                using (var p = new Pen(tb))
                {
                    
                    g.DrawRectangle(p, st, 0, et - st, Height - HScroll.Height - 1);
                }

                if (!ZoomedOutFar)
                {
                    var size = g.MeasureString(text, Utility.fretFont);


                    var textRect = new RectangleF((float)st, (size.Height * (0)) + (0 * size.Height * 0.1f),
                            size.Width, size.Height);
                    for (int idx = 0; idx < 8; idx++)
                    {
                        var ntr = new RectangleF((float)st, (size.Height * (idx)) + (idx * size.Height * 0.1f),
                            size.Width, size.Height);

                        if (VisibleTextEvents.CountOverlapping(ntr) == 0)
                        {
                            textRect = ntr;
                            VisibleTextEvents.Add(GuitarTextEvent.GetTextEvent(GuitarTrack, downTick, text),
                                ntr);
                            break;
                        }
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
                        g.DrawString(text,
                        Utility.fretFont,
                        fb,
                        textRect, StringFormat.GenericDefault);
                    }
                }
            }
        }

        public void ScrollToTick(int tick)
        {
            try
            {
                var pos = GuitarTrack.TickToTime(tick);
                var i = (int)Math.Round(Utility.ScaleTimeUp(pos) - Utility.ScollToSelectionOffset);
                HScrollValue = i;
            }
            catch { }
        }

        public int GetTickFromScreenPoint(int x)
        {
            int ret = guitarTrack.TimeToTick(
                Utility.ScaleTimeDown(HScrollValue + x));
            return ret;
            
        }

        public double GetTimeFromScreenPoint(int x)
        {
            return Utility.ScaleTimeDown(HScrollValue + x);
        }

        public int GetScreenPointFromTick(int x)
        {
            return (int)Utility.ScaleTimeUp(guitarTrack.TickToTime(x))
                        - HScrollValue;
        }

        public int GetScreenPointFromTime(double d)
        {
            return (int)Utility.ScaleTimeUp(d)
                        -HScrollValue;
        }

        public Point GetGridSnapPointFromMouse()
        {
            bool snapped = false;
            return GetGridSnapPointFromMouse(out snapped);
        }

        public Point GetGridSnapPointFromMouse(out bool snapped)
        {
            snapped = false;
            var gridMousePos = PointToClient(MousePosition);
            var mouseXT = GetTickFromScreenPoint(gridMousePos.X);

            var gst = GetTickGridSnap(mouseXT, out snapped);
            if (snapped)
            {
                var closestX = GetScreenPointFromTick(gst);

                int closestY = GetStringScreenSnap(gridMousePos.Y);

                AddSnapPointToRender(closestX);

                return new Point(closestX, closestY);
            }
            else
            {
                return gridMousePos;
            }
        }

        private void AddSnapPointToRender(int closestX)
        {
            if (!SnapPoints.Contains(closestX))
                SnapPoints.Add(closestX);
        }

        public Point GetGridSnapPointFromClientPoint(Point p, out bool snapped)
        {
            snapped = false;

            var t = GetTickFromScreenPoint(p.X);
            
            var tgs = GetTickGridSnap(t, out snapped);

            var closestX = GetScreenPointFromTick(tgs);
            int closestY = GetStringScreenSnap(p.Y);

            return new Point(closestX, closestY);
        }

        public int GetStringScreenSnap(int screenY)
        {
            int closestY = int.MaxValue;
            int closestDist = int.MaxValue;
            for (int x = 0; x < 6; x++)
            {
                int noteY = TopLineOffset + LineSpacing * (5 - x);
                var delta = Math.Abs(screenY - noteY);
                if (delta < closestDist && delta <= Utility.GridSnapDistance)
                {
                    closestDist = Math.Abs(screenY - noteY);
                    closestY = noteY;
                }
            }
            if (closestY == int.MaxValue)
            {
                closestY = screenY;
            }
            
            return closestY;
        }

        public int GetStringFromScreenCoord(int screenY)
        {
            int ret = -1;
            int closestY = int.MaxValue;
            int closestDist = int.MaxValue;
            for (int x = 0; x < 6; x++)
            {
                int noteY = TopLineOffset + LineSpacing * (5 - x);
                var delta = Math.Abs(screenY - noteY);
                if (delta < closestDist)
                {
                    closestDist = Math.Abs(screenY - noteY);
                    closestY = noteY;
                    ret = x;
                }
            }
            if (closestY == int.MaxValue)
            {
                closestY = screenY;
            }

            return ret;
        }

        public int GetTickNoteSnap(int tick, out bool snapped)
        {
            snapped = false;
            if (!IsLoaded)
            {
                return tick;
            }
            if (NoteSnap)
            {
                Point pm = new Point(GetScreenPointFromTick(tick));
                int newTick;
                if (GetMouseXNoteSnap(ref pm, out newTick))
                {
                    tick = newTick;
                    snapped = true;
                }
            }
            return tick;
        }
        public int GetTickGridSnap(int tick)
        {
            bool snapped;
            return GetTickGridSnap(tick, out snapped);
        }
        public int GetTickGridSnap(int tick, out bool snapped)
        {
            snapped = false;
            if (!IsLoaded)
            {
                return tick;
            }
            if (GridSnap)
            {
                int closestX = int.MaxValue;
                int closestDist = int.MaxValue;
                double cd = double.MaxValue;
                int newTick = tick;
                int sp = GetScreenPointFromTick(tick);

                
                if (NoteSnapG5 &&
                    (EditorType != EEditorType.Guitar5 && Editor5.IsLoaded))
                {
                    bool ns = false;
                    var g5t = Editor5.GetTickNoteSnap(tick, out ns);

                    if (ns)
                    {
                        

                        var nsp = GetScreenPointFromTick(g5t);
                        var delta = Math.Abs(nsp - sp);
                        if (delta < closestDist && delta <= Utility.NoteSnapDistance)
                        {
                            closestDist = delta;
                            closestX = nsp;
                            newTick = g5t;
                            snapped = true;

                            AddSnapPointToRender(closestX);
                            return g5t;
                        }
                    }
                }

                if ((NoteSnapG5 && EditorType == EEditorType.Guitar5) ||
                    (NoteSnapG6 && EditorType != EEditorType.Guitar5))
                {
                    var bp = guitarTrack.GetTempo(tick);
                    var tm = guitarTrack.TickToTime(tick);
                    double ct = bp.StartTime;

                    while (true)
                    {
                        double increment = guitarTrack.SecondsPerBeat(bp.DownTick, GridScalar);

                        var st = ct;
                        var se = bp.EndTime;
                        var delta = Math.Abs(tm - st);
                        var cdelta = guitarTrack.TimeToTick(delta);
                        var sp2 = GetScreenPointFromTick(tick + cdelta) - sp;

                        if (delta < cd && sp2 <= Utility.GridSnapDistance)
                        {
                            cd = delta;
                            closestDist = sp2;
                            closestX = guitarTrack.TimeToTick(st);
                            snapped = true;
                        }

                        if (st >= se)
                        {
                            break;
                        }

                        ct += increment;
                    }

                    if (closestX != int.MaxValue)
                    {
                        newTick = closestX;

                        AddSnapPointToRender(closestX);

                        snapped = true;
                    }

                    var pm = new Point(sp);
                    int mouseSnapTick;

                    if (GetMouseXNoteSnap(ref pm, out mouseSnapTick))
                    {
                        var delta = Math.Abs(pm.X - sp);
                        if (delta < closestDist && delta <= Utility.NoteSnapDistance)
                        {
                            closestDist = delta;
                            closestX = pm.X;
                            newTick = mouseSnapTick;


                            AddSnapPointToRender(closestX);

                            snapped = true;
                        }
                    }



                    tick = newTick;
                }
            }
            return tick;
        }

        public bool GetMouseXNoteSnap(ref Point gridMousePos, out int newTick)
        {
            newTick = -1;
            bool snapped = false;
            if ((NoteSnapG6 && EditorType != EEditorType.Guitar5) ||
                ((EditorType == EEditorType.Guitar5) && NoteSnapG5))
            {
                var chords = GuitarTrack.GetChordsAtTick(
                    GetTickFromScreenPoint(gridMousePos.X - Utility.NoteSnapDistance),
                    GetTickFromScreenPoint(gridMousePos.X + Utility.NoteSnapDistance)).ToArray();

                if (EditorType != EEditorType.Guitar5)
                {
                    if (SelectedChord != null)
                    {
                        chords = chords.Where(x => x != SelectedChord).ToArray();
                    }
                }

                if (chords.Length > 0)
                {
                    int iClosest = int.MaxValue;
                    int closestIndex = -1;
                    int offset = 0;

                    var mpos = gridMousePos.X;
                    for (int x = 0; x < chords.Length; x++)
                    {
                        var c = chords[x];
                        var start = GetScreenPointFromTime(c.StartTime);
                        var end = GetScreenPointFromTime(c.EndTime);

                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                var distStart = Math.Abs(start - mpos);

                                if (distStart <= Utility.NoteSnapDistance
                                    && distStart < iClosest)
                                {
                                    newTick = c.DownTick;
                                    offset = start;
                                    iClosest = distStart;
                                    closestIndex = x;
                                }
                            }
                            else
                            {
                                var distEnd = Math.Abs(end - mpos);
                                if (distEnd <= Utility.NoteSnapDistance
                                    && distEnd < iClosest)
                                {
                                    newTick = c.UpTick;
                                    offset = end;
                                    iClosest = distEnd;
                                    closestIndex = x;
                                }
                            }
                        }
                    }

                    if (closestIndex != -1)
                    {
                        var c = chords[closestIndex];

                        gridMousePos.X = offset;
                        
                        snapped = true;
                    }
                }
            }
            return snapped;
        }

        public int GetNoteStringFromMouse()
        {
            int closestY = int.MaxValue;
            var closestDist = int.MaxValue;
            var mousePos = PointToClient(MousePosition);
            int stringIndex = -1;
            for (int x = 0; x < 6; x++)
            {
                int noteY = TopLineOffset + LineSpacing * (5 - x);
                var delta = Math.Abs(mousePos.Y - noteY);
                if (delta < closestDist)
                {
                    closestDist = delta;
                    closestY = noteY;
                    stringIndex = x;
                }
            }
            return stringIndex;
        }

        public class Selector
        {
            public bool IsRight;
            public Point CurrentPoint;
            public Point StartPoint;
            public GuitarChord Chord;
            public bool IsMouseNear = false;
            public bool IsMouseOver = false;

            public Rectangle GetCurrentRect(TrackEditor editor)
            {
                var rect = editor.GetScreenRectFromMessage(Chord);

                return new Rectangle(CurrentPoint,
                    new Size(Utility.SelectorWidth, 
                         rect.Height));
            }
        }
        public List<Selector> visibleSelectors = new List<Selector>();


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (DesignMode || InPlayback || guitarTrack == null || IsLoaded == false)
                return;

            

            IsMouseOver = true;
            
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (DesignMode || InPlayback || guitarTrack == null || IsLoaded == false)
                return;

            
            IsMouseOver = false;
            
        }

        public bool MouseOverSelector
        {
            get 
            { 
                return NumSelectedChords > 0 &&
                    visibleSelectors.Count(x => x.IsMouseOver == true)>0; 
            } 
        }

        

        Selector currentSelector;
        public Selector CurrentSelector
        {
            get { return currentSelector; }
        }

        public enum EditorSelectionState
        {
            Idle=0,
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
            SelectingChordStartOffset,
            SelectingChordEndOffset,
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
                    SetStatusIdle();
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
                    SelectStartPoint = PointToClient(MousePosition);
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

            if (DesignMode || GuitarTrack == null || InPlayback || IsLoaded==false)
                return;

            try
            {

                if (CurrentSelectionState == EditorSelectionState.MovingSelector)
                {

                    if (!MovingSelectorMouseUp())
                    {
                        if (SelectedChords.Count == 1)
                        {
                            var mouseChord = GetChordFromPoint(PointToClient(MousePosition));

                            if (mouseChord != null && mouseChord.Selected)
                            {
                                if (OnSetChordToScreen != null)
                                {
                                    OnSetChordToScreen(this, mouseChord, true);
                                }

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
                    //SelectStartPoint = new Point(0, 0);
                }
                else if (CurrentSelectionState == EditorSelectionState.MovingNotes)
                {
                    try
                    {

                        UpdatePastePoint();
                        PasteCopyBuffer(CurrentPastePoint);

                    }
                    catch { }

                    CurrentSelectionState = EditorSelectionState.Idle;
                }
                else if (CurrentSelectionState == EditorSelectionState.Idle)
                {

                }
            }
            catch
            {
                CurrentSelectionState = EditorSelectionState.Idle;
            }


            if (OnReloadTrack != null)
            {
                OnReloadTrack(this, SelectNextEnum.ForceKeepSelection);
            }
                

            SelectStartPointChord = null;
        }

        public IEnumerable<Track> GetTracks(IEnumerable<string> trackNames)
        {
            var ret = new List<Track>();
            if (Sequence != null && trackNames != null)
            {
                ret.AddRange(Sequence.Tracks.Where(x=> x.Name.IsEmpty()==false && trackNames.Contains(x.Name)));
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

        public delegate void ReloadTrackHandler(TrackEditor editor, SelectNextEnum selectNext);
        public event ReloadTrackHandler OnReloadTrack;

        public delegate void LoadTrackHandler(TrackEditor editor, Sequence seq, Track t);
        public event LoadTrackHandler OnLoadTrack;

        public delegate void SetChordToScreenHandler(TrackEditor editor, GuitarChord chord, bool ignoreKeepSelection);
        public event SetChordToScreenHandler OnSetChordToScreen;

        public delegate void SelectionStateChangeHandler(TrackEditor editor, EditorSelectionState oldState, EditorSelectionState newState);
        public event SelectionStateChangeHandler OnSelectionStateChange;
        

        private bool MovingSelectorMouseUp()
        {

            bool ret = false;
            var sel = CurrentSelector;

            if (sel.StartPoint.X != sel.CurrentPoint.X)
            {
                
                var sc = sel.Chord;

                double time =0;
                
                if(sel.IsRight)
                {
                    time = GetTimeFromScreenPoint(sel.CurrentPoint.X + Utility.SelectorWidth);
                }
                else
                {
                    time = GetTimeFromScreenPoint(sel.CurrentPoint.X);
                }

                bool update=false;
                var tickTime = guitarTrack.TimeToTick(time);

                var uptick = sc.UpTick;
                var downtick = sc.DownTick;
                if(sel.IsRight)
                {
                    if (tickTime > sc.DownTick + Utility.NoteCloseWidth)
                    {
                        uptick = GetTickGridSnap(tickTime);
                        update=true;
                    }
                }
                else
                {
                    if (tickTime < sc.UpTick - Utility.NoteCloseWidth)
                    {
                        downtick = GetTickGridSnap(tickTime);
                        update=true;
                    }
                }
                if(update)
                {
                    
                    ret = true;
                    try
                    {
                        EditorPro.BackupSequence();
                        sel.Chord.CloneAtTime(guitarTrack, downtick, uptick).Selected = true;
                    }
                    catch {  }
                }
            }

            if (OnReloadTrack != null)
            {
                OnReloadTrack(this, SelectNextEnum.ForceKeepSelection);
            }
                
            CurrentSelectionState = EditorSelectionState.Idle;
            return ret;
        }

        

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e != null)
            {
                base.OnMouseMove(e);
            }

            if (DesignMode || GuitarTrack == null || InPlayback || IsLoaded == false)
                return;



            if (CurrentSelectionState == EditorSelectionState.MovingSelector)
            {
                MoveChordSelector();
            }
            else if (CurrentSelectionState == EditorSelectionState.SelectingBox)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    UpdateSelectionBox();
                }
            }
            else if (CurrentSelectionState == EditorSelectionState.PastingNotes ||
                CurrentSelectionState == EditorSelectionState.MovingNotes)
            {
                UpdateSelectionBox();
            }
            else if (CurrentSelectionState == EditorSelectionState.Idle)
            {

                if (EditMode == EEditMode.Chords)
                {

                    if (e.Button == System.Windows.Forms.MouseButtons.Left && 
                        SelectedChords.Count > 0)
                    {
                        if (SelectStartPoint.IsEmpty == false)
                        {
                            UpdateSelectionBox();
                            var mc = GetChordFromPoint(SelectStartPoint);
                            if (mc != null)
                            {
                                var mp = GetGridSnapPointFromMouse();

                                int stringY1 = GetStringFromScreenCoord(SelectStartPoint.Y);
                                int stringY2 = GetStringFromScreenCoord(SelectCurrentPoint.Y);

                                if ((stringY1 != stringY2 &&
                                    stringY1 != -1 &&
                                    stringY2 != -1) ||
                                    Math.Abs(mp.X - SelectStartPoint.X) > Utility.NoteCloseWidth)
                                {

                                    SetSelectedToClipboard();

                                    if (EditorType != EEditorType.Guitar5)
                                    {
                                        if (!IsAltKeyDown)
                                        {
                                            RemoveSelectedNotes();
                                        }
                                        CurrentSelectionState = EditorSelectionState.MovingNotes;
                                        SelectStartPointChord = mc;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            UpdateSelectorVisibility();

            
            
        }


        public void SetChordToClipboard(GuitarChord gc)
        {
            CopyChords.Clear();
            if (gc != null)
            {
                CurrentSelectionState =
                    EditorSelectionState.PastingNotes;
                CopyChords.Add(gc);
            }
        }
        public void SetSelectedToClipboard()
        {
            if (SelectedChords.Count > 0)
            {
                CopyChords.Clear();
                CurrentSelectionState =
                    EditorSelectionState.PastingNotes;
                foreach (var sc in EditorPro.SelectedChords)
                {
                    if (sc != null)
                    {
                        CopyChords.Add(sc);
                    }
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


        private void MoveChordSelector()
        {
            var sel = CurrentSelector;

            if (sel == null)
                return;

            Point currentMousePos = GetGridSnapPointFromMouse();


            if (sel.IsRight)
            {
                sel.CurrentPoint = new Point(
                    currentMousePos.X - Utility.SelectorWidth,
                    sel.StartPoint.Y);
            }
            else
            {
                sel.CurrentPoint = new Point(
                    currentMousePos.X,
                    sel.StartPoint.Y);
            }
            Invalidate();
        }



        private void UpdateSelectionBox()
        {
            SelectCurrentPoint = PointToClient(MousePosition);
            if (GridSnap)
            {
                SelectCurrentPoint = GetGridSnapPointFromMouse();
            }
            Invalidate();
        }


        public void RemoveSelectedNotes()
        {
            try
            {
                BackupSequence();

                SelectedChords.ForEach(x => guitarTrack.Remove(x));

                if (OnReloadTrack != null)
                {
                    OnReloadTrack(this, SelectNextEnum.ForceKeepSelection);
                }
                
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
                    
                }
            }
            catch { }
            return ret;
        }


        public void mnuInsertIntoPro_Click(object sender, EventArgs e)
        {


            try
            {
                EditorPro.BackupSequence();

                var clist = Editor5.SelectedChords;

                foreach (var c5 in clist)
                {
                    var gt = EditorPro.GuitarTrack;

                    if (c5 != null && gt != null)
                    {
                        var notes = new int[6]{Int32.MinValue, Int32.MinValue,Int32.MinValue,Int32.MinValue,Int32.MinValue,Int32.MinValue};
                        var channels = new int[6] { Int32.MinValue,Int32.MinValue,Int32.MinValue,Int32.MinValue,Int32.MinValue,Int32.MinValue};

                        gt.GetChordsAtTick(c5.DownTick, c5.UpTick).ForEach(x=> {
                            gt.Remove(x);
                        });
                        
                        for(int x=0;x<6;x++){
                            var n = c5.Notes[x];
                            if (n == null)
                                continue;
                            notes[x] = 0;
                            channels[x] = 0;
                        }
                        gt.CreateChord(notes, channels, EditorPro.CurrentDifficulty,
                            c5.DownTick, c5.UpTick, false, false, false, ChordStrum.Normal);


                        if (EditorPro.OnReloadTrack != null)
                        {
                            EditorPro.OnReloadTrack(EditorPro, SelectNextEnum.UseConfiguration);
                        }
                        
                    }
                }


            }
            catch { }
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
                if (CopyChords.Count() > 0)
                {
                    if (CurrentSelectionState != TrackEditor.EditorSelectionState.PastingNotes &&
                        CurrentSelectionState != TrackEditor.EditorSelectionState.MovingNotes)
                    {
                        CurrentSelectionState = TrackEditor.EditorSelectionState.PastingNotes;
                        return newChords;
                    }

                    BackupSequence();

                    double selectedStartTime =
                        Utility.ScaleTimeDown(HScrollValue +
                            pastePoint.Offset.X+pastePoint.MousePos.X);


                    int minTick = CopyChords.GetMinTick();
                    int maxTick = CopyChords.GetMaxTick();

                    var pasteLen = guitarTrack.TickToTime(maxTick) - guitarTrack.TickToTime(minTick);

                    int selLen = maxTick - minTick;

                    double minTime = GuitarTrack.TickToTime(minTick);
                    int testTick = Int32.MinValue;

                    var selectedEndTime = selectedStartTime + pasteLen;

                    guitarTrack.Remove(
                        guitarTrack.GetChordsAtTick(
                            guitarTrack.TimeToTick(selectedStartTime) + Utility.NoteCloseWidth,
                            guitarTrack.TimeToTick(selectedEndTime) - Utility.NoteCloseWidth));

                    foreach (var c in CopyChords)
                    {
                        var noteDown = GuitarTrack.TickToTime(c.DownTick);
                        var noteUp = GuitarTrack.TickToTime(c.UpTick);

                        var delta = noteDown - minTime;

                        var startTick = GuitarTrack.TimeToTick(selectedStartTime +
                            delta);

                        var endTick = GuitarTrack.TimeToTick(selectedStartTime +
                            delta + (noteUp - noteDown));


                        var minNoteString = pastePoint.MinNoteString;
                        var mouseString = pastePoint.MousePos.Y;
                        var offset = (mouseString) - pastePoint.MinNoteString - (pastePoint.Offset.Y / LineSpacing);

                        var st = GetTickGridSnap(startTick);
                        var et = GetTickGridSnap(endTick);

                        if (!testTick.IsNull() && st < testTick)
                        {
                            st = testTick;
                            if (Utility.IsCloseTick(et, st))
                            {
                                et = st + Utility.NoteCloseWidth;
                            }
                        }

                        var newChord = c.CloneAtTime(guitarTrack, st, et, offset, testTick);
                        if (newChord != null)
                        {
                            newChords.Add(newChord);

                            testTick = newChord.UpTick;
                        }
                    }

                    ClearSelection();
                    foreach (var c in newChords.Where(x => !x.IsDeleted)) { c.Selected = true; }
                }
            }
            catch { RestoreBackup(); }


            if (OnReloadTrack != null)
            {
                OnReloadTrack(this, SelectNextEnum.ForceKeepSelection);
            }
                
            
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
            get { return IsLoaded ? new TrackDifficulty(SelectedTrack, CurrentDifficulty) : null; }

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

        public void SetStatusIdle()
        {
            CurrentSelectionState = EditorSelectionState.Idle;
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
                    SetStatusIdle();
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
                    if (CopyChords.Count > 0)
                    {
                        mnuSelect.Enabled = true;
                        mnuSelect.Click += new EventHandler(delegate(object o, EventArgs e)
                        {
                            if (CopyChords.Count > 0)
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
            
            
            if (DesignMode || InPlayback || guitarTrack == null || IsLoaded==false)
                return;


            if ( e.Button == System.Windows.Forms.MouseButtons.Left)
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
                    return;
                }
                else
                {
                    HandleProRightMouseMenu(e);
                    
                    CurrentSelectionState = EditorSelectionState.Idle;
                    return;
                }
            }
            else if (EditorType == EEditorType.Guitar5)
            {
                if (OnMouseDown5Tar(e))
                {
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
                        var mouseChord = GetChordFromPoint(PointToClient( MousePosition));
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
            
            if (CurrentSelectionState == EditorSelectionState.Idle ||
                CurrentSelectionState == EditorSelectionState.MovingSelector)
            {
                
                var mc = GetChordFromPoint(PointToClient( MousePosition));
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
                SelectStartPointChord = mc;
            }
            UpdateSelectorVisibility();
            
            
        }

        void DrawChord(GuitarChord chord, Graphics g, bool drawSelected)
        {

            foreach(var note in chord.Notes)
            {
                var i = note.NoteString;

                var start = GetScreenPointFromTick(note.DownTick);
                var end = GetScreenPointFromTick(note.UpTick);

                if (CurrentSelectionState == EditorSelectionState.MovingSelector)
                {
                    var sel = CurrentSelector;

                    if (sel != null &&
                        chord.Selected && drawSelected)
                    {
                        if (sel.IsRight)
                        {
                            end = sel.CurrentPoint.X + Utility.SelectorWidth;

                        }
                        else
                        {
                            start = sel.CurrentPoint.X;
                        }
                    }
                }

                int noteX = start;
                int noteY = GetNoteMinYOffset(note);

                var width = end - start;

                
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


                using (var bs = new SolidBrush(Color.FromArgb(20, Utility.noteBGBrushShadow.Color)))
                {
                    var shadowRect = noteRect;
                    shadowRect.Width += 2;
                    shadowRect.Height += 2;
                    g.FillRectangle(bs, shadowRect);
                }

                if (chord.Selected)
                {
                    g.FillRectangle(Utility.noteBGBrushSel,
                    noteX + Utility.NoteSelectionOffsetLeft, noteY + Utility.NoteSelectionOffsetUp,
                    width + Utility.NoteSelectionOffsetRight, NoteHeight + Utility.NoteSelectionOffsetDown);
                }

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
                var rectPen = Utility.noteBoundPen;

                if (!ZoomedOutRealFar)
                {
                    g.DrawRectangle(rectPen, noteRect);
                }

                var len = GetScreenPointFromTick(chord.UpTick) - GetScreenPointFromTick(chord.DownTick);
                
                if (len > 5 && !ZoomedOutRealFar)
                {
                    if (chord.IsHammeron && chord.IsSlide)
                    {
                        rectPen = Utility.hammerOnPen;

                        int hw = (int)rectPen.Width;
                        g.DrawRectangle(rectPen, noteRect);

                        rectPen = Utility.slidePen;

                        g.DrawRectangle(rectPen,
                             noteX - hw,
                             noteY - hw,
                             width + hw * 2,
                             NoteHeight + hw * 2);
                    }
                    else if (chord.IsHammeron)
                    {
                        rectPen = Utility.hammerOnPen;

                        g.DrawRectangle(rectPen, noteRect);
                    }
                    else if (chord.IsSlide)
                    {
                        rectPen = Utility.slidePen;

                        g.DrawRectangle(rectPen, noteRect);
                        
                    }

                   
                    if(width > 2)
                    {
                        float fontSize = (float)NoteHeight;
                        
                        if (width < fontSize)
                        {
                            fontSize = width;
                        }

                        if (fontSize/2.0f - (int)(fontSize/2.0f) > 0)
                        {
                            fontSize = fontSize - 1;
                        }


                        var font = GetFontForRect(noteRect);

                        if (font != null)
                        {
                            g.DrawString(noteTxt,
                                font,
                                Utility.fretBrush,
                                new RectangleF(new PointF(noteX, noteY), new SizeF(width+20, NoteHeight)),
                                 Utility.GetStringFormatNoWrap());
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

        public class PastePointParam
        {
            public int MinChordX;
            public int MinNoteString;
            public Point MousePos;
            public Point Offset;

        }
        public PastePointParam CurrentPastePoint = new PastePointParam();


        void DrawPasteChord(PastePointParam param, GuitarChord chord, Graphics g)
        {
            int alpha = 120;
            foreach(var note in chord.Notes)
            {
                var i = note.NoteString;

                var start = GetScreenPointFromTick(note.DownTick);
                var end = GetScreenPointFromTick(note.UpTick);



                int noteX = param.MousePos.X;

                noteX = noteX + (int)(start - param.MinChordX) 
                    + param.Offset.X;

                

                int noteY = TopLineOffset + LineSpacing *
                    (5-(param.MousePos.Y -param.MinNoteString + note.NoteString )) - 
                    NoteHeight / 2
                    + param.Offset.Y;

                var width = end - start;

                using (var p = new Pen(Color.FromArgb(alpha, Utility.noteBGBrushSel.Color)))
                {
                    g.DrawRectangle(p,
                        noteX + Utility.NoteSelectionOffsetLeft,
                        noteY + Utility.NoteSelectionOffsetUp,
                        width + Utility.NoteSelectionOffsetRight-1,
                        NoteHeight + Utility.NoteSelectionOffsetDown-1);
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
                    g.FillRectangle(sb,
                                noteX, noteY,
                                width, NoteHeight);
                }

                using (var rectPen = new Pen(Color.FromArgb(alpha, Utility.noteBoundPen.Color)))
                {
                    g.DrawRectangle(rectPen,
                            noteX, noteY,
                            width,
                            NoteHeight);
                }


                using (var selPen = new Pen(Color.FromArgb(alpha, Utility.selectedPen.Color)))
                {
                    g.DrawRectangle(selPen,
                            noteX, noteY,
                            width,
                            NoteHeight);
                }

                using (var fb = new SolidBrush(Color.FromArgb(alpha, Utility.fretBrush.Color)))
                {
                  
                    string noteTxt = note.NoteFretDown.ToString();
                    if (note.Channel == Utility.ChannelX)
                    {
                        noteTxt = Utility.XNoteText;
                    }
                    g.DrawString(noteTxt,
                        Utility.fretFont,
                        fb,
                        noteX + Utility.NoteTextXOffset,
                        noteY - (int)(Utility.fontHeight / 4.0 + Utility.NoteTextYOffset));
                }
                
            }
        }
        private void DrawChords6(Graphics g, IEnumerable<GuitarChord> vis, bool selected)
        {

            foreach(var chord in vis)
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
            else if(CurrentSelectionState == EditorSelectionState.Idle)
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
            var start = GetScreenPointFromTick(message.DownTick);
            var end = GetScreenPointFromTick(message.UpTick);

            return new Rectangle(start, minMaxY.X, end - start, minMaxY.Y - minMaxY.X);
        }

        List<Selector> GetVisibleSelectors()
        {
            var ret = new List<Selector>();
            var chord = SelectedChord;
            if (chord == null)
                return ret;

            Point p = PointToClient(MousePosition);

            var minMaxY = GetChordYMinMax(chord);
            var start = GetScreenPointFromTick(chord.DownTick);
            var end = GetScreenPointFromTick(chord.UpTick);

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
                        chordScreen.Height );

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
            var color = Color.FromArgb(40,80,40);
            if (sel == null)
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
                            r.Y - 2);
                        p[1] = new Point(
                            r.X + Utility.SelectorWidth - 1 + j,
                            r.Y + r.Height + 2);
                    }
                    else
                    {
                        p[0] = new Point(
                             r.X - 1 + j,
                            r.Y - 2);
                        p[1] = new Point(
                            r.X - 1 + j,
                            r.Y + r.Height + 2);
                    }

                    g.DrawLine(pn, p[0], p[1]);
                }

                g.DrawLine(pn,
                    r.X,
                    r.Y - 2,
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
            
            foreach (var bre in vis.GetMessages<GuitarBigRockEnding>())
            {
                var brush = Utility.noteBREBrush;
                if (bre.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, bre.DownTick, bre.UpTick);
            }
            foreach (var solo in vis.GetMessages<GuitarSolo>())
            {
                var brush = Utility.noteSoloBrush;
                if (solo.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, solo.DownTick, solo.UpTick);
            }

            foreach (var pwer in vis.GetMessages<GuitarPowerup>())
            {
                var brush = Utility.notePowerupBrush;
                if (pwer.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                FillRect(g, brush, pwer.DownTick, pwer.UpTick);
            }
            foreach (var bre in vis.GetMessages<GuitarSingleStringTremelo>())
            {
                var brush = Utility.noteSingleStringTremeloBrush;
                if (bre.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                var msgs = vis.GetBetweenTick(bre.DownTick, bre.UpTick).Where(x => x is GuitarChord).Cast<GuitarChord>();

                int minString = Int32.MaxValue, maxString = Int32.MinValue;
                foreach (var m in msgs)
                {
                    foreach (var n in m.Notes)
                    {
                        if (n != null)
                        {
                            if (n.NoteString < minString)
                                minString = n.NoteString;
                            if (n.NoteString > maxString)
                                maxString = n.NoteString;
                        }
                    }
                }
                if (minString != Int32.MaxValue && maxString != Int32.MinValue)
                {
                    DrawRect(g, brush, bre.DownTick, bre.UpTick, minString, maxString);
                }
                else
                {
                    FillRect(g, brush, bre.DownTick, bre.UpTick);
                }
            }

            foreach (var bre in vis.GetMessages<GuitarMultiStringTremelo>())
            {
                var brush = Utility.noteMultiStringTremeloBrush;
                if (bre.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }

                var msgs = vis.GetBetweenTick(bre.DownTick, bre.UpTick).Where(x=> x is GuitarChord).Cast<GuitarChord>();

                int minString = Int32.MaxValue, maxString = Int32.MinValue;
                foreach (var m in msgs)
                {
                    foreach (var n in m.Notes)
                    {
                        if (n != null)
                        {
                            if (n.NoteString < minString)
                                minString = n.NoteString;
                            if (n.NoteString > maxString)
                                maxString = n.NoteString;
                        }
                    }
                }
                if (minString != Int32.MaxValue && maxString != Int32.MinValue)
                {
                    DrawRect(g, brush, bre.DownTick, bre.UpTick, minString, maxString);
                }
                else
                {
                    FillRect(g, brush, bre.DownTick, bre.UpTick);
                }
            }
            foreach (var arp in vis.GetMessages<GuitarArpeggio>())
            {
                var brush = Utility.noteArpeggioBrush;
                if (arp.Selected)
                {
                    brush = Utility.noteBGBrushSel;
                }
                FillRect(g, brush, arp.DownTick, arp.UpTick);
            }
        }

        private void DrawTabLines(Graphics g, Pen p)
        {
            if (!ZoomedOutFar)
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
        }

        public int InnerHeight { get { return (this.ClientSize.Height - this.HScroll.Height) ; } }
        public int LineSpacing { get { return InnerHeight / 6; } }
        
        public int NoteHeight { get { return LineSpacing - LineSpacing/4; } }

        public int TopLineOffset { get { return (int)(NoteHeight *0.75); } }

    }

    public enum SelectNextEnum
    {
        ForceSelectNext,
        ForceKeepSelection,
        UseConfiguration,
    }
}
