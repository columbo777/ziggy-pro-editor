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
    class ThreadedTimerParam
    {
        public SynchronizationContext Context;
        public object Param;
        public int TimerID;
        public int TimerGroupID;
        public int NumTotalTicks;
        public Action<object> Func;
        public Action<object> EndFunc;
    }
}