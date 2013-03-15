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
    public class CopyPatternPreset
    {
        public int ID;
        public string Name;
        public bool ForwardOnly;
        public bool MatchLengths5;
        public bool MatchLengths6;
        public bool MatchSpacing;
        public bool MatchBeat;
        public bool KeepLengths;
        public bool FirstMatchOnly;
        public bool RemoveExisting;

        public override string ToString()
        {
            return Name;
        }
        public void CopyTo(CopyPatternPreset item)
        {
            item.Name = Name;
            item.ForwardOnly = ForwardOnly;
            item.MatchLengths5 = MatchLengths5;
            item.MatchLengths6 = MatchLengths6;
            item.MatchSpacing = MatchSpacing;
            item.MatchBeat = MatchBeat;
            item.KeepLengths = KeepLengths;
            item.FirstMatchOnly = FirstMatchOnly;
            item.RemoveExisting = RemoveExisting;
        }
    }
}