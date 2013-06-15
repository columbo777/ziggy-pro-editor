using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ProUpgradeEditor.Common;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;

namespace EditorResources.Components
{


    public partial class PEWebTabImportPanel : UserControl
    {
        public Sequence Sequence;

        TrackEditor EditorPro { get { return ((PEPopupWindow)Parent).EditorPro; } }

        public PEWebTabImportPanel(Sequence seqImport)
        {

            this.Sequence = seqImport;

            InitializeComponent();

        }

        public PEWebTabImportPanel() : this(null) { }

        private void resetScale_Click(object sender, EventArgs e)
        {
            ResetScaleOffset();
        }

        private void ResetScaleOffset()
        {
            ProScale.Text = "1.0";
            ProOffset.Text = "3.0";
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);


            if (Sequence != null)
            {
                Track track = null;
                if (Sequence.Count > 1)
                {
                    track = Sequence.ElementAt(1);

                    trackEditorWebTab.SetTrack(track, GuitarDifficulty.Expert);
                    trackEditorPro.SetTrack(track, GuitarDifficulty.Expert);
                }
                else
                {
                    track = Sequence.FirstOrDefault();
                    trackEditorWebTab.SetTrack(track, GuitarDifficulty.Expert);
                    trackEditorPro.SetTrack(track, GuitarDifficulty.Expert);
                }

                if (EditorPro.Editor5.IsLoaded)
                {
                    var first = EditorPro.Editor5.Messages.Chords.FirstOrDefault();
                    ProOffset.Text = EditorPro.Editor5.GetTimeFromScreenPoint(first.ScreenPointPair.Down).ToStringEx();
                }

                if (trackEditorPro.SelectedTrack == null || trackEditorPro.SelectedTrack.Track != track)
                {
                    trackEditorPro.SetTrack(track, GuitarDifficulty.Expert);
                }

                PreviewChanges();

                ResetScaleOffset();
            }

            PEPopupWindow parent = (PEPopupWindow)(Parent);
            parent.Opacity = 0.9;
        }

        public List<WebTabTrackProperties> TrackProperties = new List<WebTabTrackProperties>();
        Track lastTrack = null;
        private void trackEditorPro_TrackClicked(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            sender.SetTrack(track, difficulty);
            SelectTrack(sequence, track, difficulty);
        }

        private void SelectTrack(Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            if (DesignMode)
                return;
            if (sequence != null && track != null)
            {

                if (lastTrack != null && sequence.Contains(lastTrack))
                {
                    TrackProperties.FirstOrDefault(x => x.Track == lastTrack).IfObjectNotNull(x => UpdateWebTabTrack(x));
                }

                lastTrack = track;


                if (!TrackProperties.Any(x => x.Track == track))
                {
                    TrackProperties.Add(new WebTabTrackProperties(track, EditorPro));
                }

            }
            else
            {
                TrackProperties.Clear();

                lastTrack = null;
            }

            setWebTabTrackToScreen(track == null ? null : TrackProperties.FirstOrDefault(x => x.Track == track));
        }

        void UpdateWebTabTrack(WebTabTrackProperties prop)
        {
            prop.Scale = ProScale.Text.ToDouble();

            prop.Offset = ProOffset.Text.ToDouble();
            prop.Import = ImportTrack.Checked;
        }

        void setWebTabTrackToScreen(WebTabTrackProperties prop)
        {
            if (prop == null)
            {
                ProScale.Text = "1.0";
                ProOffset.Text = "3.0";

                ImportTrack.Checked = false;
            }
            else
            {
                ProScale.Text = prop.Scale.ToStringEx();
                ProOffset.Text = prop.Offset.ToStringEx();
                ImportTrack.Checked = prop.Import;
            }

            PreviewChanges();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            if (Sequence != null && TrackProperties.Count > 0)
            {
                UpdateWebTabTrack(TrackProperties.FirstOrDefault(x => x.Track == lastTrack));
                if (TrackProperties.Any(x => x.Import))
                {
                    var import = TrackProperties.Where(x => x.Import == true).ToList();
                    import.ForEach(x => UpdateTrack(x.Track));

                    var tr = new List<Track>();
                    for (int x = 1; x < Sequence.Count; x++)
                    {
                        var t = TrackProperties.FirstOrDefault(f => f.Track == Sequence[x]);
                        if (t == null || t.Import == false)
                            tr.Add(Sequence[x]);
                    }

                    tr.ToList().ForEach(t => Sequence.Remove(t));

                    if (Sequence.Count > 1)
                    {
                        ((PEPopupWindow)Parent).Close(DialogResult.OK);
                    }
                }
                else
                {
                    MessageBox.Show("No tracks marked for import");
                }
            }

        }



        private void UpdateTrack(Track track)
        {
            try
            {

                TrackProperties.FirstOrDefault(x => x.Track == track).IfObjectNotNull(trackProp =>
                {

                    if (trackProp.Track.ChanMessages.Count() > 5)
                    {

                        if (!trackProp.Scale.IsNull())
                        {
                            track.Remove(track.ChanMessages.ToList());

                            var ev = trackProp.Events.Chords.ToList();

                            var scale = trackProp.Scale;
                            if (scale < 0.001)
                                scale = 0.001;

                            var offset = trackProp.Offset;

                            var firstTime = ev.First().TimePair;
                            var firstTick = ev.First().TickPair;

                            ev.ForEach(x =>
                            {
                                x.Notes.ForEach(n =>
                                {
                                    var scaledLength = x.TimeLength * scale;
                                    var scaledStart = (x.StartTime - firstTime.Down) * scale + offset;

                                    var downTick = EditorPro.GuitarTrack.TimeToTick(scaledStart);
                                    var upTick = EditorPro.GuitarTrack.TimeToTick(scaledStart + scaledLength);
                                    var data1 = Utility.GetStringLowE(GuitarDifficulty.Expert) + n.NoteString;
                                    track.Insert((int)(downTick), new ChannelMessage(ChannelCommand.NoteOn, data1, n.NoteFretDown + 100, n.Channel));
                                    track.Insert((int)(upTick), new ChannelMessage(ChannelCommand.NoteOff, data1, 0, n.Channel));
                                });
                            });

                        }
                    }

                });
            }
            catch { }
        }

        private void trackEditorPro_TrackRemoved(PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty)
        {
            if (DesignMode)
                return;
            TrackProperties.FirstOrDefault(x => x.Track == track).IfObjectNotNull(x => TrackProperties.Remove(x));
        }

        private void refreshClick(object sender, EventArgs e)
        {
            PreviewChanges();
        }

        private void PreviewChanges()
        {
            try
            {
                trackEditorPro.SelectedTrack.IfObjectNotNull(track =>
                {
                    TrackProperties.FirstOrDefault(x => x.Track == track.Track).IfObjectNotNull(x => UpdateWebTabTrack(x));

                    if (ProScale.Text.ToDouble().IsNull() || ProScale.Text.ToDouble() < 0.01)
                        return;
                    UpdateTrack(track.Track);

                    EditorPro.SetTrack6(track.Track, GuitarDifficulty.Expert);
                    
                    EditorPro.ClearSelection();
                    if (EditorPro.IsLoaded && EditorPro.Messages.Chords.Any())
                    {
                        EditorPro.Messages.Chords.FirstOrDefault().Selected = true;
                        EditorPro.ScrollToSelection();
                    }
                    InvalidateEditor();
                });
            }
            catch { }
        }

        private void InvalidateEditor()
        {
            EditorPro.Invalidate();
            EditorPro.Editor5.Invalidate();
            Application.DoEvents();

        }

        private void peCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = !AutoRefresh.Checked;
            InvalidateEditor();
        }

        private void ProScale_TextChanged(object sender, EventArgs e)
        {
            if (AutoRefresh.Checked && ProScale.Text.ToDouble().IsNull() == false && ProOffset.Text.ToDouble().IsNull() == false)
                PreviewChanges();
        }

        private void ImportTrack_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ImportTrack_Validating(object sender, CancelEventArgs e)
        {

        }

        private void trackEditorPro_Load(object sender, EventArgs e)
        {

        }
    }
}
