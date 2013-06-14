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
    public class SongUtilSearchResultItem
    {
        public MidiEvent Event { get; internal set; }
        public string TrackName { get; internal set; }

        public SongUtilSearchResultItem(string trackName, MidiEvent ev)
        {
            this.Event = ev;
            this.TrackName = TrackName;
        }

        public override string ToString()
        {
            return Event.ToString();
        }
    }

    public class SongUtilSearchResult
    {
        public string MidiPath { get; internal set; }
        public List<SongUtilSearchResultItem> Matches { get; internal set; }

        public SongUtilSearchResult(string path) 
        { 
            MidiPath = path;
            Matches = new List<SongUtilSearchResultItem>(); 
        }

    }
}