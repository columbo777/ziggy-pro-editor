using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using X360;
using X360.FATX;


namespace ProUpgradeEditor.UI
{
    public enum SelectNextEnum
    {
        UseConfiguration,
        ForceSelectNext,
        ForceKeepSelection
    }
}