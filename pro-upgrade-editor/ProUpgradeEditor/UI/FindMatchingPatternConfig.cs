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
    public class FindMatchingPatternConfig
    {
        public bool FindNext;
        public bool FindPrevious;
        public bool FirstMatchOnly;

        public FindMatchingPatternConfig(bool findNext, bool findPrevious, bool firstMatchOnly)
        {
            this.FindNext = findNext;
            this.FindPrevious = findPrevious;
            this.FirstMatchOnly = firstMatchOnly;
        }
    }
}