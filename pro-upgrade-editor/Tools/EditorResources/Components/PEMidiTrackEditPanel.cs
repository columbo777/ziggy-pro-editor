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
using ProUpgradeEditor.DataLayer;

namespace EditorResources.Components
{

    

    

    [DesignerCategory("UserControl")]
    public partial class PEMidiTrackEditPanel : PUEControl
    {
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


        GuitarDifficulty selectedDifficulty;

        public GuitarDifficulty SelectedDifficulty
        {
            get { return selectedDifficulty; }
            internal set { selectedDifficulty = value; }
        }

        Track selectedTrack = null;
        public Track SelectedTrack
        {
            get { return selectedTrack; }
            internal set 
            {
                selectedTrack = value;
            }
        }

        public PEMidiTrackEditPanel() : base()
        {
            InitializeComponent();

            if (DesignMode)
                return;
            selectedDifficulty = GuitarDifficulty.All;
            this.listTracks.View = View.Details;
            this.listTracks.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listTracks.HideSelection = true;
            listTracks.GridLines = false;
            listTracks.Scrollable = true;
            
            listTracks.Activation = ItemActivation.OneClick;

            if (!DesignMode)
            {
                listTracks.OnSelectedItemChanged += new EventHandler<PEListView.PEListViewEventArgs>(listTracks_OnSelectedItemChanged);
                listTracks.OnItemClicked += new EventHandler<PEListView.PEListViewEventArgs>(listTracks_OnItemClicked);
            }
        }

        void listTracks_OnItemClicked(object sender, PEListView.PEListViewEventArgs e)
        {
            if (TrackClicked != null && e != null && e.Item != null)
            {
                listTracks.SelectedItem = e.Item;
                SetSelectedItem(e.Item.Track, e.Item.Difficulty);
                TrackClicked(this, sequence, e.Item.Track, e.Item.Difficulty);
            }
        }

        void listTracks_OnSelectedItemChanged(object sender, PEListView.PEListViewEventArgs e)
        {
            
        }


        private void listTracks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        [Category("Custom")]
        public event TrackEventHandler TrackAdded;

        [Category("Custom")]
        public event TrackEventHandler TrackRemoved;

        [Category("Custom")]
        public event TrackEventHandler TrackClicked;


        private void CreateItemList()
        {
            
            Track lastSel = selectedTrack;
            var lastDiff = selectedDifficulty;

            listTracks.BeginUpdate();
            listTracks.Items.Clear();

            if (sequence != null)
            {
                foreach (var t in sequence)
                {
                    var li = new PEListView.PEListViewItem(listTracks, null, t.Name, t, GuitarDifficulty.All);

                    var item = listTracks.Items.Add(li);

                    AddSubTracks(item, t);
                }
            }

            ReselectOldSelection(lastSel, lastDiff);

            listTracks.EndUpdate();
            
            
        }

        private void ReselectOldSelection(Track lastSel, GuitarDifficulty lastDiff)
        {
            DeselectAll();
            var item = GetItem(lastSel, lastDiff);
            if (item != null)
            {
                listTracks.ActivateItem(item);

                listTracks.EnsureVisible(item.Index);
            }
            
        }

        void AddSubTracks(PEListView.PEListViewItem item, Track track)
        {
            listTracks.Items.Add(new PEListView.PEListViewItem(listTracks, item, "Expert", track, GuitarDifficulty.Expert));
            listTracks.Items.Add(new PEListView.PEListViewItem(listTracks, item, "Hard", track, GuitarDifficulty.Hard));
            listTracks.Items.Add(new PEListView.PEListViewItem(listTracks, item, "Medium", track, GuitarDifficulty.Medium));
            listTracks.Items.Add(new PEListView.PEListViewItem(listTracks, item, "Easy", track, GuitarDifficulty.Easy));
        }

        public override void Refresh()
        {
            if (DesignMode)
                return;

            
            var track = selectedTrack;
            var diff = selectedDifficulty;

            CreateItemList();
            
            ReselectOldSelection(track, diff);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            WriteDebug("TrackEdit.CreateClick");

            if (!string.IsNullOrEmpty(textBoxTrackName.Text))
            {
                sequence = internalGetSequence(true);
                if (sequence != null)
                {
                    var track = new Track(sequence.FileType, textBoxTrackName.Text);
                    sequence.Add(track);

                    Refresh();

                    if (TrackAdded != null)
                    {
                        TrackAdded(this, sequence, track, GuitarDifficulty.All);
                    }
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            WriteDebug("TrackEdit.CopyClick");

            if (SelectedTrack != null && sequence != null && textBoxTrackName.Text.Length>0)
            {

                var track = new Track(sequence.FileType, textBoxTrackName.Text);
                track.Merge(SelectedTrack);
                sequence.Add(track);

                Refresh();

                if (TrackAdded != null)
                {
                    TrackAdded(this, sequence, track, GuitarDifficulty.All);
                }
                
            }
            
        }


        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedTrack != null && sequence != null)
            {
                sequence.Remove(selectedTrack);

                Refresh();

                if (TrackRemoved != null)
                {
                    TrackRemoved(this, sequence, SelectedTrack, GuitarDifficulty.All);
                }
            }
        }


        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listTracks.View = View.List;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listTracks.View = View.Details;
        }

        PEListView.PEListViewItem GetItem(Track track, GuitarDifficulty difficulty = GuitarDifficulty.All)
        {
            return listTracks.Items.SingleOrDefault(x=> x.Track == track && x.Difficulty == difficulty);
        }

        void DeselectAll()
        {
            listTracks.Items.ToList().ForEach(x=>{
                x.Selected = false;
            });
        }

        
        void SetSelectedItem(Track track, GuitarDifficulty difficulty)
        {
            if (track == null)
            {
                DeselectAll();
            }
            else
            {
                if (sequence == null || sequence != track.Sequence)
                {
                    sequence = track.Sequence;

                    Refresh();
                }
                
                internalSetSelected(GetItem(track, difficulty));
            }
            Invalidate();
        }

        void internalSetSelected(PEListView.PEListViewItem item)
        {
            if (item != null)
            {
                item.Selected = true;
                selectedTrack = item.Track;
                if (item.Track != null)
                {
                    textBoxTrackName.Text = item.Track.Name;
                }
                else
                {
                    textBoxTrackName.Text = "";
                }
                selectedDifficulty = item.Difficulty;
                listTracks.SelectedItem = item;
                
            }
            else
            {
                selectedTrack = null;
                selectedDifficulty = GuitarDifficulty.All;
                textBoxTrackName.Text = "";
            }
        }

        GuitarDifficulty GetDifficulty(PEListView.PEListViewItem item)
        {
            var ret = GuitarDifficulty.All;
            if (item != null)
            {
                if (item.Text == "Expert")
                {
                    ret = GuitarDifficulty.Expert;
                }
                else if (item.Text == "Hard")
                {
                    ret = GuitarDifficulty.Hard;
                }
                else if (item.Text == "Medium")
                {
                    ret = GuitarDifficulty.Medium;
                }
                else if (item.Text == "Easy")
                {
                    ret = GuitarDifficulty.Easy;
                }
            }
            return ret;
        }

        private void peListView1_ItemActivate(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            internalSetSelected(listTracks.SelectedItem);
            
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (SelectedTrack != null && textBoxTrackName.Text.Length > 0)
            {
                SelectedTrack.Name = textBoxTrackName.Text;
                
                Refresh();
            }
        }

        private void listTracks_DragEnter(object sender, DragEventArgs e)
        {
            if (DesignMode)
                return;

            if (GetDragDropItem(e) != null)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        PEListView.PEListViewItem GetDragDropItem(DragEventArgs e)
        {
            PEListView.PEListViewItem ret = null;

            if (e.Data.GetDataPresent(DataFormats.Serializable, false) == true)
            {
                try
                {
                    object o = e.Data.GetData(DataFormats.Serializable, false);
                    if (o != null)
                    {
                        ret = o as PEListView.PEListViewItem;
                    }
                }
                catch { }
            }
            return ret;
        }

        private void listTracks_DragDrop(object sender, DragEventArgs e)
        {
            if (DesignMode)
                return;

            var item = GetDragDropItem(e);

            if (item != null)
            {
                if (!listTracks.Items.Contains(item))
                {
                    sequence = internalGetSequence(true);
                    if (sequence != null)
                    {
                        var t = new Track(sequence.FileType);
                        t.Merge(item.Track);
                        t.Name = item.Track.Name;

                        var pItem = listTracks.GetItemFromScreenPoint(new Point(e.X, e.Y));

                        InsertTrackAfterItem(t, pItem);
                    }
                }
                else
                {
                    if (listTracks.Items.Count > 0)
                    {
                        sequence = internalGetSequence(false);
                        if (sequence != null)
                        {
                            var pItem = listTracks.GetItemFromScreenPoint(new Point(e.X, e.Y));

                            if (pItem != null && pItem != item)
                            {

                                var pIndex = listTracks.Items.IndexOf(pItem);
                                var index = listTracks.Items.IndexOf(item);

                                sequence.MoveTrack(index, pIndex);

                                CreateItemList();

                                if (TrackAdded != null)
                                {
                                    TrackAdded(this, sequence, pItem.Track, pItem.Difficulty);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void InsertTrackAfterItem(Track t, PEListView.PEListViewItem pItem)
        {
            var listItem = new PEListView.PEListViewItem(listTracks, null, t.Name, t, GuitarDifficulty.All);

            if (pItem == null)
            {
                sequence.Add(t);

                listTracks.Items.Add(listItem);
            }
            else
            {
                var index = listTracks.Items.IndexOf(pItem);

                if (index + 1 >= sequence.Count)
                {
                    sequence.Add(t);
                }
                else
                {
                    sequence.Insert(index + 1, t);
                }
            }

            CreateItemList();

            if (TrackAdded != null)
            {
                TrackAdded(this, sequence, t, GuitarDifficulty.All);
            }
        }

        public void SetTrack(Track track, GuitarDifficulty difficulty)
        {
            this.sequence = track != null ? track.Sequence : null;
            this.selectedTrack = track;
            this.selectedDifficulty = difficulty;
            this.Refresh();
        }
        

        private void listTracks_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (DesignMode)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                listTracks.DoDragDrop(e.Item, DragDropEffects.All);
            }
        }

        private void PEMidiTrackEditPanel_DragDrop(object sender, DragEventArgs e)
        {
            listTracks_DragDrop(sender, e);
        }

        private void PEMidiTrackEditPanel_DragEnter(object sender, DragEventArgs e)
        {
            listTracks_DragEnter(sender, e);
        }

    }
}
