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


namespace ProUpgradeEditor.UI
{
    public class DTASegment
    {
        public List<DTASegment> Children;

        public string Value;
        public string Name;
        public DTASegment Parent;

        public DTASegment(DTASegment parent) { Parent = parent; Children = new List<DTASegment>(); }
        public DTASegment(DTASegment parent, string text)
            : this(parent)
        {
            var first = text.IndexOf(' ');
            if (first != -1)
            {
                Name = text.SubstringEx(0, first).Trim();
                Value = text.SubstringEx(first + 1).Trim();
            }
            else
            {
                Name = text.Trim();
                Value = text.Trim();
            }
        }

        public override string ToString()
        {
            return "Name: [" + Name + "] Value: [" + Value + "]";
        }

        public DTASegment FindFirstSegment(string tag)
        {
            DTASegment ret = null;

            if (Name.EqualsEx(tag))
            {
                ret = this;
            }
            else
            {
                Children.FirstOrDefault(x => (ret = x.FindFirstSegment(tag)) != null);
            }
            return ret;
        }

        public string SongShortName
        {
            get
            {
                var p = this;
                while (p.Parent != null)
                {
                    p = p.Parent;
                }
                return p.Name;
            }
        }
    }
}