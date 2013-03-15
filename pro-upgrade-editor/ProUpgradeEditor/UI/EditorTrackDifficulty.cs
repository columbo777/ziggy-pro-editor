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
                SelectedTrackDifficulty = new TrackDifficulty(trackPanel.SelectedTrack.Track.Name, trackPanel.SelectedTrack.SelectedDifficulty);
            }
            else
            {
                SelectedTrackDifficulty = null;
            }
        }
    }
}