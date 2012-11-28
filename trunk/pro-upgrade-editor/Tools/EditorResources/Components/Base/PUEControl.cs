using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;

namespace EditorResources.Components
{
    public delegate void TrackEditPanelEventHandler(
        PEMidiTrackEditPanel sender, Sequence sequence, Track track, GuitarDifficulty difficulty);

    public delegate void TrackEventHandler(
        object sender, Track track, GuitarDifficulty difficulty);

    public delegate void PEMidiTrackDifficultyDropEventHandler(
        PEMidiTrack sender, GuitarDifficulty difficulty, DragEventArgs e);

    public delegate void PEMidiTrackDropEventHandler(
        PEMidiTrack sender, DragEventArgs e);

    public class PUEControl : UserControl
    {
        protected bool DebugMode { get; set; }

        public PUEControl()
            : base()
        {
            DebugMode = false;
        }

        protected void WriteDebug(string str)
        {
            if (DebugMode)
            {
                Util.WriteDebug(str);
            }
        }
    }
}
