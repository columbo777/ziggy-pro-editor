using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using NAudio.Wave;

namespace ProUpgradeEditor.Common
{
    public class SnapConfig
    {
        public bool SnapG5;
        public bool SnapG6;
        public bool SnapGrid;


        public SnapConfig(bool snapG5, bool snapG6, bool snapGrid)
        {
            this.SnapG5 = snapG5;
            this.SnapG6 = snapG6;
            this.SnapGrid = snapGrid;
        }
    }
}