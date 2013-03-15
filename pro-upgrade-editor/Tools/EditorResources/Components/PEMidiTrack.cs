using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;
using ProUpgradeEditor.Common;

namespace EditorResources.Components
{
    public partial class PEMidiTrack : UserControl
    {

        [Category("Custom")]
        public event PEMidiTrackDifficultyDropEventHandler DifficultyItemDropped;

        [Category("Custom")]
        public event PEMidiTrackDropEventHandler ItemDropped;

        [Category("Custom")]
        public event PEMidiTrackDropEventHandler ItemBeginDrag;
        [Category("Custom")]
        public event PEMidiTrackDifficultyDropEventHandler ItemDifficultyBeginDrag;
        [Category("Custom")]
        public event PEMidiTrackDropEventHandler ItemDragCancel;


        [Category("Custom")]
        public event TrackEventHandler TrackClicked;

        [Category("Custom")]
        public event TrackEventHandler TrackNameDoubleClicked;

        [Category("Custom")]
        public event TrackEventHandler TrackDifficultyChanged;
        

        

        public PEMidiTrack(Track track)
        {
            InitializeComponent();

            if (!DesignMode)
            {
                DifficultyButtons.ForEach(x =>
                    {
                        x.Button.Click += delegate(object o, EventArgs e)
                            {
                                setDifficulty(x.Difficulty);
                            };
                        x.Button.MouseDown += delegate(object sender, MouseEventArgs e)
                        {
                            x.Button.Tag = e.Location;
                        };
                        x.Button.MouseUp += delegate(object sender, MouseEventArgs e)
                        {
                            x.Button.Tag = null;
                        };
                        x.Button.MouseMove += delegate(object sender, MouseEventArgs e)
                        {
                            if (e.Button == System.Windows.Forms.MouseButtons.Left &&
                                x.Button.Tag != null && ((Point)x.Button.Tag).Distance(e.Location) > 3)
                            {
                                x.Button.DoDragDrop(new PETrackDifficulty(this, x.Difficulty), DragDropEffects.All);
                                ItemDifficultyBeginDrag.IfObjectNotNull(bd => bd(this, x.Difficulty, null));
                            }

                        };

                        x.Button.AllowDrop = true;

                        x.Button.DragEnter += new DragEventHandler(Button_DragEnter);
                        x.Button.DragDrop += new DragEventHandler(Button_DragDrop);
                    });

                setDifficulty(GuitarDifficulty.Expert);
                Track = track;


                labelTrackName.AllowDrop = true;
                labelTrackName.DragEnter += new DragEventHandler(panelTrackName_DragEnter);
                labelTrackName.DragDrop += new DragEventHandler(panelTrackName_DragDrop);
                labelTrackName.DragOver += new DragEventHandler(panelTrackName_DragOver);

                labelTrackName.MouseMove += new MouseEventHandler(panelTrackName_MouseMove);
                labelTrackName.MouseUp += new MouseEventHandler(panelTrackName_MouseUp);
                labelTrackName.MouseDown += new MouseEventHandler(panelTrackName_MouseDown);
            }
        }

        void panelTrackName_MouseUp(object sender, MouseEventArgs e)
        {
            panelTrackName.Tag = null;
        }

        void panelTrackName_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left &&
                panelTrackName.Tag != null && ((Point)panelTrackName.Tag).Distance(e.Location) > 3)
            {
                this.DoDragDrop(this, DragDropEffects.All);
                ItemBeginDrag.IfObjectNotNull(bd => bd(this, null));
            }

        }


        private void panelTrackName_MouseDown(object sender, MouseEventArgs e)
        {
            panelTrackName.Tag = e.Location;
        }

        void panelTrackName_DragDrop(object sender, DragEventArgs e)
        {
            var d = e.GetDropObject<PEMidiTrack>();
            if (d != null && d != this)
            {
                ItemDropped.IfObjectNotNull(x => x(this, e));
            }
        }
        void panelTrackName_DragOver(object sender, DragEventArgs e)
        {
            OnMouseMove(new MouseEventArgs(MouseButtons, 0, MousePosition.X, MousePosition.Y, 0));
        }
        void panelTrackName_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            var d = e.GetDropObject<PEMidiTrack>();
            if (d != null && d != this)
            {
                e.Effect = DragDropEffects.All;
            }
        }


        void Button_DragDrop(object sender, DragEventArgs e)
        {

            var dropDiff = e.GetDropObject<PETrackDifficulty>();
            if (dropDiff != null)
            {
                var btn = DifficultyButtons.SingleOrDefault(x => x.Button == sender);

                if ((dropDiff.MidiTrack != btn.MidiTrack) || (dropDiff.MidiTrack == btn.MidiTrack && dropDiff.Difficulty != btn.Difficulty))
                {
                    DifficultyItemDropped.IfObjectNotNull(d => d(this, DifficultyButtons.Single(x => x.Button == sender).Difficulty, e));
                }
            }
        }

        void Button_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            var dropDiff = e.GetDropObject<PETrackDifficulty>();
            if (dropDiff != null)
            {
                var btn = DifficultyButtons.SingleOrDefault(x => x.Button == sender);

                if ((dropDiff.MidiTrack != btn.MidiTrack) || (dropDiff.MidiTrack == btn.MidiTrack && dropDiff.Difficulty != btn.Difficulty))
                {
                    e.Effect = DragDropEffects.All;
                }
            }
        }



        Track track;
        public Track Track
        {
            get { return track; }
            set
            {
                this.track = value;
                Refresh();
            }
        }

        public override void Refresh()
        {
            var name = (track == null ? "" : (track.Name ?? ""));
            if (labelTrackName.Text != name)
                labelTrackName.Text = name;
            base.Refresh();
        }

        public Color SelectedButtonBackColor
        {
            get
            {
                return Color.WhiteSmoke;
            }
        }
        public GuitarDifficulty SelectedDifficulty
        {
            get
            {
                if (!DifficultyButtons.Any(x => x.Button.BackColor == SelectedButtonBackColor))
                {
                    DifficultyButtons.Single(x => x.Difficulty == GuitarDifficulty.Expert).Button.BackColor = SelectedButtonBackColor;
                }
                return DifficultyButtons.Single(x => x.Button.BackColor == SelectedButtonBackColor).Difficulty;
            }
            set
            {
                DifficultyButtons.ForEach(x => x.Button.BackColor = Color.Transparent);
                DifficultyButtons.Single(x => x.Difficulty == value).Button.BackColor = SelectedButtonBackColor;
            }
        }

        DifficultyButton[] DifficultyButtons
        {
            get
            {
                return new DifficultyButton[] 
                { 
                    new DifficultyButton(new PETrackDifficulty(this, GuitarDifficulty.Easy), buttonEasy),
                    new DifficultyButton(new PETrackDifficulty(this, GuitarDifficulty.Medium), buttonMedium),
                    new DifficultyButton(new PETrackDifficulty(this, GuitarDifficulty.Hard), buttonHard),
                    new DifficultyButton(new PETrackDifficulty(this, GuitarDifficulty.Expert), buttonExpert),
                };
            }
        }

        void setDifficulty(GuitarDifficulty diff)
        {
            if (!diff.IsEasyMediumHardExpert())
                diff = GuitarDifficulty.Expert;

            if (SelectedDifficulty != diff)
            {
                SelectedDifficulty = diff;

                this.track.IfObjectNotNull(tr =>
                    TrackDifficultyChanged.IfObjectNotNull(x => x(this, tr, this.SelectedDifficulty)));
            }
            else
            {
                TrackClicked.IfObjectNotNull(x => x(this, this.track, this.SelectedDifficulty));
            }
        }

        private void labelTrackName_DoubleClick(object sender, EventArgs e)
        {
            TrackNameDoubleClicked.IfObjectNotNull(x => x(this, this.track, this.SelectedDifficulty));
        }

        private void panelMidiTrack_Click(object sender, EventArgs e)
        {
            TrackClicked.IfObjectNotNull(x => x(this, this.track, this.SelectedDifficulty));
        }

        private void labelTrackName_Click(object sender, EventArgs e)
        {
            TrackClicked.IfObjectNotNull(x => x(this, this.track, this.SelectedDifficulty));
        }

        private void PEMidiTrack_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                e.Action = DragAction.Cancel;
                ItemDragCancel.IfObjectNotNull(x => x(this, null));
            }
        }


    }
}
