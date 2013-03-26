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

    public class ControlAnimatorItem
    {
        public AnimationRoutine Routine;
        public Control Control;

        public DateTime TimeStart;
        public DateTime TimeEnd;
        public DateTime CurrentTime;

        public ControlAnimatorType Type;
        public Point Start;
        public Point End;

        bool completed = false;
        public bool Completed
        {
            get
            {
                return completed;
            }
            set
            {
                completed = value;
            }
        }

        public double TotalAnimTime
        {
            get { return (TimeEnd - TimeStart).TotalSeconds; }
        }

        public double TimeElapsed
        {
            get { return (CurrentTime - TimeStart).TotalSeconds; }
        }

        public double Progress
        {
            get { return TimeElapsed / TotalAnimTime; }
        }
    }
}