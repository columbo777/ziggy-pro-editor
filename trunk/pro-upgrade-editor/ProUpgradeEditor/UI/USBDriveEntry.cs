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
    public class USBDriveEntry
    {
        FATXFolderEntry entry;
        FATXReadContents contents;

        string name;

        public USBDriveEntry(string name, FATXReadContents contents, FATXFolderEntry entry)
        {
            this.entry = entry;
            this.name = name;
            this.contents = contents;
        }

        public FATXFolderEntry Entry
        {
            get { return this.entry; }
        }

        public FATXReadContents Contents
        {
            get { return this.contents; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}