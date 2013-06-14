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
    public class SongUtilFindInFileConfig
    {
        public bool FindDistinctText;
        public bool FindInProOnly;
        public string SearchFolder;

        public bool FirstMatchOnly;
        public bool MatchCountOnly;
        public bool SelectedSongOnly;
        public bool OpenResults;
        public bool MatchWholeWord;

        public string RootFolder;

        public int FindData1;
        public int FindData2;
        public int FindChannel;
        public string FindText;

    }
}