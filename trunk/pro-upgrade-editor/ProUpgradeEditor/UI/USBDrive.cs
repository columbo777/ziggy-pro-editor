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
    public class USBDrive
    {
        DeviceReturn device;
        string name;

        bool opened = false;
        FATXDrive drive = null;

        public bool IsOpen
        {
            get { return opened; }
        }


        public bool Open()
        {
            if (!opened)
            {
                try
                {
                    drive = new FATXDrive(device);

                    RefreshPartitions();

                    opened = true;
                    return true;
                }
                catch { }
            }

            return false;
        }

        public void Close()
        {
            if (opened)
            {
                try
                {
                    drive.Close();
                    drive = null;
                }
                catch { drive = null; }
                opened = false;
            }
        }

        public USBDrive(DeviceReturn device)
        {
            this.device = device;

            if (Open())
            {
                try
                {
                    this.name = Drive.DriveName;

                    RefreshPartitions();
                }
                catch { }
                finally { this.Close(); }
            }
        }

        public FATXFolderEntry GetFolder(FATXFolderEntry e)
        {
            return GetFolder(e.Path);
        }

        public FATXFolderEntry GetFolder(string path)
        {
            if (drive != null)
            {
                bool o = Open();

                var ret = drive.GetFolder(path);

                if (o)
                    Close();

                return ret;
            }
            else
            {
                return null;
            }
        }

        public FATXFolderEntry GetFolder(TreeNode node)
        {
            if (node.Tag is FATXFolderEntry)
                return GetFolder(node.Tag as FATXFolderEntry);
            else
                return null;
        }


        public void RefreshPartitions()
        {
            folders = new USBDriveEntry[0];

            var fld = new List<USBDriveEntry>();
            foreach (var p in drive.Partitions)
            {
                FATXFolderEntry fe;
                FATXReadContents readContents = drive.ReadToFolder(
                    p.PartitionName, out fe);

                var entry = new USBDriveEntry(p.PartitionName, readContents, fe);

                fld.Add(entry);
            }
            this.folders = fld.ToArray();
        }

        public override string ToString()
        {
            return this.name;
        }

        public FATXDrive Drive
        {
            get
            {
                return drive;
            }
        }
        USBDriveEntry[] folders;
        public USBDriveEntry[] Folders
        {
            get
            {
                return folders;
            }
        }
    }
}