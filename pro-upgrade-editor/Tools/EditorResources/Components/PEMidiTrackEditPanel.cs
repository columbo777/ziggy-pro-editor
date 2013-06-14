using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;
using ProUpgradeEditor.Common;

namespace EditorResources.Components
{


    [DesignerCategory("UserControl")]
    public partial class PEMidiTrackEditPanel : PUEControl
    {
        int ItemHeight
        {
            get
            {
                return 25;
            }
        }
        List<PEMidiTrack> TrackList
        {
            get
            {
                var ret = new List<PEMidiTrack>();
                if (panelTracks != null)
                {

                    foreach (var c in panelTracks.Controls)
                    {
                        var pc = c as PEMidiTrack;
                        if (pc != null)
                        {
                            ret.Add(pc);
                        }
                    }

                    ret.Sort((x, y) => x.TabIndex > y.TabIndex ? 1 : x.TabIndex < y.TabIndex ? -1 : 0);
                }
                return ret;
            }
        }

        Sequence sequence;
        public Sequence Sequence { get { return sequence; } }



        public bool IsPro { get; set; }

        Sequence internalGetSequence(bool createNew)
        {
            if (sequence == null && createNew)
            {
                sequence = new Sequence(IsPro ? FileType.Pro : FileType.Guitar5, 480);
            }
            return sequence;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PEMidiTrack SelectedTrack
        {
            get
            {
                return TrackList.SingleOrDefault(x => x.PanelMidiTrack.BackColor == Color.LightSteelBlue);
            }
            internal set
            {
                TrackList.Where(x => x != value).ForEach(x => x.PanelMidiTrack.BackColor = Color.Transparent);

                value.IfObjectNotNull(x =>
                    {
                        x.PanelMidiTrack.BackColor = Color.LightSteelBlue;
                        textBoxTrackName.Text = x.Track.Name;
                    },
                    Else =>
                    {
                        textBoxTrackName.Text = "";
                    });
            }
        }

        public PEMidiTrackEditPanel()
            : base()
        {
            InitializeComponent();

            if (DesignMode)
                return;

        }

        [Category("Custom")]
        public event EventHandler RequestBackup;

        [Category("Custom")]
        public event TrackEditPanelEventHandler TrackAdded;

        [Category("Custom")]
        public event TrackEditPanelEventHandler TrackRemoved;

        [Category("Custom")]
        public event TrackEditPanelEventHandler TrackClicked;


        void CreatePanelTracks(Sequence seq, bool forceRefresh = false)
        {

            bool refresh = false;
            if (forceRefresh == false && seq != null)
            {
                if (seq != this.sequence)
                    refresh = true;
                else
                {
                    if (panelTracks.Controls.Count != seq.Count)
                        refresh = true;
                    else
                    {
                        for (int x = 0; x < seq.Count; x++)
                        {
                            var trackListTrack = TrackList[x].Track;
                            var sequenceTrack = seq[x];
                            if (sequenceTrack != trackListTrack || (trackListTrack.Name != sequenceTrack.Name))
                            {
                                refresh = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                refresh = true;
            }
            if (!refresh)
            {
                return;
            }

            SuspendLayout();
            this.sequence = seq;
            Track osel = null;
            if (SelectedTrack != null)
            {
                osel = SelectedTrack.Track;
            }
            if (Sequence != null)
            {
                
                panelTracks.Controls.Clear();
                foreach (var tr in sequence.ToList())
                {
                    var peTrack = new PEMidiTrack(tr);
                    peTrack.TabIndex = panelTracks.Controls.Count;

                    peTrack.TrackClicked += new TrackEventHandler(t_TrackClicked);
                    peTrack.TrackDifficultyChanged += new TrackEventHandler(t_TrackDifficultyChanged);
                    peTrack.TrackNameDoubleClicked += new TrackEventHandler(t_TrackNameDoubleClicked);

                    peTrack.AllowDrop = true;

                    peTrack.DifficultyItemDropped += new PEMidiTrackDifficultyDropEventHandler(peTrack_DifficultyItemDropped);
                    peTrack.ItemDropped += new PEMidiTrackDropEventHandler(peTrack_ItemDropped);
                    peTrack.MouseMove += new MouseEventHandler(peTrack_MouseMove);
                    peTrack.Dock = DockStyle.Top;
                    peTrack.ItemBeginDrag += new PEMidiTrackDropEventHandler(peTrack_ItemBeginDrag);
                    peTrack.ItemDifficultyBeginDrag += new PEMidiTrackDifficultyDropEventHandler(peTrack_ItemDifficultyBeginDrag);
                    peTrack.ItemDragCancel += new PEMidiTrackDropEventHandler(peTrack_ItemDragCancel);
                    peTrack.DragOver += new DragEventHandler(peTrack_DragOver);
                    panelTracks.Controls.Add(peTrack);
                    peTrack.BringToFront();
                }


                if (osel != null)
                {
                    SelectedTrack = TrackList.FirstOrDefault(x => x.Track == osel);
                }
            }
            else
            {
                panelTracks.Controls.Clear();
                textBoxTrackName.Text = "";
            }

            ResumeLayout();
        }

        void peTrack_DragOver(object sender, DragEventArgs e)
        {
            panelTracks.Invalidate();
        }

        void peTrack_ItemDragCancel(PEMidiTrack sender, DragEventArgs e)
        {
            dragItem = null;
            panelTracks.Invalidate();
        }

        void peTrack_ItemDifficultyBeginDrag(PEMidiTrack sender, GuitarDifficulty difficulty, DragEventArgs e)
        {
            dragItem = sender;
            panelTracks.Invalidate();
        }

        PEMidiTrack dragItem;
        void peTrack_ItemBeginDrag(PEMidiTrack sender, DragEventArgs e)
        {
            dragItem = sender;
            panelTracks.Invalidate();
        }

        void peTrack_MouseMove(object sender, MouseEventArgs e)
        {
            panelTracks.Invalidate();
        }

        void DoRequestBackup()
        {
            if (RequestBackup != null)
                RequestBackup(this, null);
        }

        private void panelTracks_DragDrop(object sender, DragEventArgs e)
        {
            var droppedPETrack = e.GetDropObject<PEMidiTrack>();
            droppedPETrack.IfObjectNotNull(xx =>
            {
                if (droppedPETrack != sender)
                {
                    DoRequestBackup();

                    Track newTrack = null;
                    if (Sequence == null)
                    {
                        var targetType = this.IsPro ? FileType.Pro : FileType.Guitar5;

                        var seq = new Sequence(targetType, droppedPETrack.Track.Sequence.Division);
                        this.sequence = seq;
                        if (!droppedPETrack.Track.IsTempo())
                        {
                            var tempo = droppedPETrack.Track.Sequence.Tracks.Where(x => x.IsTempo());
                            if (tempo.Any())
                            {
                                seq.AddTempo(tempo.First().ConvertToPro());
                            }
                        }
                        newTrack = droppedPETrack.Track.Clone(seq.FileType);
                        seq.Add(newTrack);
                    }
                    else
                    {

                        if (this.Sequence == droppedPETrack.Track.Sequence)
                        {
                            this.Sequence.MoveTrack(droppedPETrack.Track.GetTrackIndex(), GetInsertAt());
                        }
                        else
                        {
                            newTrack = droppedPETrack.Track.Clone(Sequence.FileType);

                            sequence.Insert(GetInsertAt(), newTrack);

                            if (!sequence.Tracks.Any(x => x.IsTempo()))
                            {
                                var tempo = droppedPETrack.Track.Sequence.Tracks.Where(x => x.IsTempo());
                                if (tempo.Any())
                                {
                                    sequence.AddTempo(tempo.First().ConvertToPro());
                                }
                            }
                        }
                    }

                    CreatePanelTracks(this.sequence);
                    TrackAdded.IfObjectNotNull(tc => tc(this, this.sequence, newTrack, SelectedDifficulty));
                }
            });
            dragItem = null;
        }


        private void panelTracks_DragOver(object sender, DragEventArgs e)
        {
            if (e.HasDropObject<PEMidiTrack>())
                e.Effect = DragDropEffects.All;

        }


        void peTrack_ItemDropped(PEMidiTrack sender, DragEventArgs e)
        {
            var peTrack = e.GetDropObject<PEMidiTrack>();
            peTrack.IfObjectNotNull(x =>
            {
                if (peTrack != sender)
                {
                    DoRequestBackup();

                    Track newTrack = null;
                    if (sender.Track.Sequence == peTrack.Track.Sequence)
                    {
                        newTrack = peTrack.Track;
                        sender.Track.Sequence.MoveTrack(peTrack.Track.GetTrackIndex(), GetInsertAt());
                    }
                    else
                    {
                        if (IsPro && peTrack.Track.Name.IsProTrackName())
                        {
                            newTrack = new Track(FileType.Pro, peTrack.Track.Name);

                            newTrack.Merge(peTrack.Track);
                        }
                        else if (IsPro == false && peTrack.Track.Name.IsProTrackName() == false)
                        {
                            newTrack = new Track(FileType.Guitar5, peTrack.Track.Name);

                            newTrack.Merge(sender.Track);
                        }
                        else
                        {
                            newTrack = peTrack.Track.Clone(sender.Track.FileType);
                        }

                        newTrack.Name = sender.Track.Name;

                        sender.Track.Sequence.Insert(GetInsertAt(), newTrack);

                        if (!ModifierKeys.HasFlag(Keys.Shift))
                        {
                            sender.Track.Sequence.Remove(sender.Track);
                        }
                    }
                    Refresh();
                    SetSelectedItem(newTrack, SelectedDifficulty);

                    t_TrackClicked(this, newTrack, SelectedDifficulty);
                    
                }
            });
            dragItem = null;
        }



        void peTrack_DifficultyItemDropped(PEMidiTrack sender, GuitarDifficulty difficulty, DragEventArgs e)
        {
            var o = e.GetDropObject<PETrackDifficulty>();
            o.IfObjectNotNull(op =>
            {
                DoRequestBackup();

                var messages = sender.Track.GetChanMessagesByDifficulty(difficulty);
                sender.Track.Remove(messages);
                
                var otrack = o.MidiTrack.Track;

                var clonedTrack = otrack.CloneDifficulty(o.Difficulty, difficulty, sender.Track.FileType);
                
                var clonedMessages = clonedTrack.GetChanMessagesByDifficulty(difficulty).ToList();
                
                foreach(var msg in clonedMessages)
                {
                    sender.Track.Insert(msg.AbsoluteTicks, msg.Clone());
                }

                SetSelectedItem(sender.Track, o.Difficulty);
                
                TrackClicked.IfObjectNotNull(x => x(this, sender.Track.Sequence, sender.Track, difficulty));
            });
        }



        void t_TrackNameDoubleClicked(object sender, Track track, GuitarDifficulty difficulty)
        {
            textBoxTrackName.Text = track.Name;

            (sender as PEMidiTrack).IfObjectNotNull(x => SelectedTrack = x);
        }

        void t_TrackDifficultyChanged(object sender, Track track, GuitarDifficulty difficulty)
        {
            (sender as PEMidiTrack).IfObjectNotNull(x =>
                {
                    SelectedTrack = x;
                });

            this.SelectedDifficulty = difficulty;

            TrackClicked.IfObjectNotNull(x => x(this, track.Sequence, track, difficulty));
        }

        void t_TrackClicked(object sender, Track track, GuitarDifficulty difficulty)
        {
            TrackClicked.IfObjectNotNull(x => x(this, track.Sequence, track, difficulty));
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreatePanelTracks(this.sequence);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBoxTrackName.Text))
            {
                sequence = internalGetSequence(true);
                if (sequence != null)
                {
                    var track = new Track(sequence.FileType, textBoxTrackName.Text);
                    sequence.Add(track);

                    Refresh();

                    SetSelectedItem(track, SelectedDifficulty);

                    if (TrackAdded != null)
                    {
                        TrackAdded(this, sequence, track, SelectedDifficulty);
                    }
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (SelectedTrack != null && sequence != null && textBoxTrackName.Text.Length > 0)
            {
                var track = new Track(sequence.FileType, textBoxTrackName.Text);
                track.Merge(SelectedTrack.Track);
                sequence.Add(track);

                Refresh();
                SetSelectedItem(track, SelectedDifficulty);
                if (TrackAdded != null)
                {
                    TrackAdded(this, sequence, track, SelectedDifficulty);
                }
            }
        }

        bool internalRemoveTrack(Track track)
        {
            var ret = false;

            if (track == null)
                return ret;

            if (sequence != null && sequence.Contains(track))
            {
                sequence.Remove(track);
                ret = true;
            }

            var t = TrackList.SingleOrDefault(x => x.Track == track);
            if (t != null)
            {
                ret = true;
                if (panelTracks.Controls.Contains(t))
                {
                    panelTracks.Controls.Remove(t);
                }
            }
            return ret;
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedTrack != null && sequence != null)
            {
                DoRequestBackup();
                var name = SelectedTrack.Track.Name;
                if (internalRemoveTrack(SelectedTrack.Track))
                {
                    Refresh();

                    if (TrackRemoved != null)
                    {
                        TrackRemoved(this, sequence, SelectedTrack.Track, SelectedDifficulty);
                    }

                    SelectSimilarTrack(name);

                }
            }
        }

        private void SelectSimilarTrack(string name)
        {
            Track similar = null;
            if (name.IsGuitarTrackName() || name.IsBassTrackName())
            {
                var t = sequence.Tracks.FirstOrDefault(x => name.IsGuitarTrackName() ? x.Name.IsGuitarTrackName() : name.IsBassTrackName());

                if (t == null)
                {
                    similar = sequence.LastOrDefault();
                }
            }
            if (similar != null)
            {
                SetSelectedItem(similar, SelectedDifficulty);
            }
        }

        bool SetSelectedItem(Track item, GuitarDifficulty difficulty)
        {
            bool ret = true;
            if (item != null)
            {
                var sel = SelectedTrack;
                var newSel = TrackList.SingleOrDefault(x => x.Track == item);
                if (sel == null || sel != newSel)
                {
                    ret = true;
                    SelectedTrack = newSel;
                }
            }
            else
            {
                SelectedTrack = null;
            }
            SelectedDifficulty = difficulty;
            
            panelTracks.Invalidate();
            return ret;
        }

        public IEnumerable<TrackDifficulty> TrackDifficulties
        {
            get { return panelTracks.Controls.ToEnumerable<PEMidiTrack>().Select(x => new TrackDifficulty(x.Track.Name, x.SelectedDifficulty)); }
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (SelectedTrack != null && textBoxTrackName.Text.Length > 0)
            {
                DoRequestBackup();

                SelectedTrack.Track.Name = textBoxTrackName.Text;
                CreatePanelTracks(this.sequence, true);
            }
        }

        public GuitarDifficulty SelectedDifficulty
        {
            get
            {
                GuitarDifficulty ret = GuitarDifficulty.Expert;
                SelectedTrack.IfObjectNotNull(x => ret = x.SelectedDifficulty);
                return ret;
            }
            set
            {
                SelectedTrack.IfObjectNotNull(x => x.SelectedDifficulty = value);
            }
        }

        bool settingTrack = false;
        public void SetTrack(Track track, GuitarDifficulty difficulty)
        {
            if (!settingTrack)
            {
                settingTrack = true;
                try
                {
                    if (track != null)
                    {
                        this.sequence = track.Sequence;
                    }
                    else
                    {
                        this.sequence = null;
                    }
                    if (this.sequence == null)
                    {
                        panelTracks.Controls.Clear();
                        SelectedTrack = null;
                        Invalidate();
                    }
                    else
                    {
                        this.CreatePanelTracks(this.sequence);
                        if (SetSelectedItem(track, difficulty))
                        {
                            t_TrackClicked(this, track, difficulty);
                        }
                    }
                }
                finally
                {
                    settingTrack = false;
                }
            }
        }


        public override void Refresh()
        {
            this.CreatePanelTracks(this.sequence);
            base.Refresh();
            panelTracks.Invalidate();
        }

        private void panelTracks_Paint(object sender, PaintEventArgs e)
        {
            if (MouseButtons == System.Windows.Forms.MouseButtons.Left)
            {
                if (ClientRectangle.Contains(PointToClient(MousePosition)))
                {
                    var insertPoint = GetInsertPoint();
                    if (insertPoint.IsNull() == false)
                    {
                        e.Graphics.Flush();
                        using (var p = new Pen(Color.FromArgb(200, Color.Red), 1.5f))
                        {
                            e.Graphics.DrawLine(p, 0, (insertPoint), panelTracks.Width, (insertPoint));
                        }
                        e.Graphics.Flush();
                    }
                }
            }
        }

        private int GetInsertPoint()
        {
            int ret = Int32.MinValue;
            var panel = panelTracks.GetChildAtPoint(
                panelTracks.PointToClient(MousePosition)) as PEMidiTrack;
            if (panel == null && panelTracks.Controls.Count > 0)
                panel = TrackList.Last();
            if (panel != null)
            {
                if (panelTracks.PointToClient(MousePosition).Y
                    <= panel.Top + panel.Height / 2)
                    ret = panel.Top;
                else
                    ret = panel.Bottom;
            }
            return ret;
        }

        private int GetInsertAt()
        {
            int ret = 0;
            var panel = panelTracks.GetChildAtPoint(
                panelTracks.PointToClient(MousePosition)) as PEMidiTrack;
            if (panel == null && panelTracks.Controls.Count > 0)
                panel = TrackList.Last();
            if (panel != null)
            {
                if (panelTracks.PointToClient(MousePosition).Y
                    <= panel.Top + panel.Height / 2)
                    ret = panel.TabIndex;
                else
                    ret = panel.TabIndex + 1;
            }
            return ret;
        }

    }
}
