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
    public class TabSiteMeasureAttribute
    {
        public int Divisions;
        public int StaffLines;
        public string[] StaffLineTuning;
        public int Beats;
        public int BeatType;

        public static void FromMeasure(ref TabSiteMeasureAttribute ret, XmlNode measureNode)
        {
            var attributes = XMLUtil.GetNode(measureNode, "attributes");
            if (attributes != null)
            {
                var i = XMLUtil.GetNodeValue(attributes, "divisions").ToInt();
                if (i.IsNull() == false)
                {
                    ret.Divisions = i;
                }

                //num strings (ex: 6)
                i = XMLUtil.GetNodeValue(attributes, "staff-details/staff-lines").ToInt();
                if (i.IsNull() == false)
                {
                    ret.StaffLines = i;
                }

                //string tuning (ex: E)
                var n = XMLUtil.GetNode(attributes, "staff-details/staff-tuning");
                if (n != null)
                {
                    ret.StaffLineTuning = XMLUtil.GetNodeList(n, "tuning-step").Select(x => XMLUtil.GetNodeValue(x)).ToArray();
                }

                //beats per bar (ex: 6)
                var b = XMLUtil.GetNodeValue(attributes, "time/beats").ToInt();
                if (b.IsNull() == false) { ret.Beats = b; }

                //4 = quarter note
                b = XMLUtil.GetNodeValue(attributes, "time/beat-type").ToInt();
                if (b.IsNull() == false)
                {
                    ret.BeatType = b;
                }
            }

        }
    }
}