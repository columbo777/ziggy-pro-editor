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
    public class DTAFile : IEnumerable<DTASegment>
    {
        public List<DTASegment> Items;
        public byte[] DTAData;


        public IEnumerable<DTASegment> GetSongIDs()
        {
            var ret = new List<DTASegment>();
            var segments = FindSegment("song_id");
            if (segments.Any())
            {
                ret.AddRange(segments);
            }
            return ret;
        }
        public IEnumerable<DTASegment> FindSegment(string tag, DTASegment item = null)
        {
            var ret = new List<DTASegment>();
            ret.AddRange(Items.Where(x => x.Name.EqualsEx(tag)));

            Items.ForEach(x => x.FindFirstSegment(tag).IfObjectNotNull(o => ret.Add(o)));
            return ret;
        }

        DTAFile() { Items = new List<DTASegment>(); }
        public DTAFile(byte[] dta)
            : this()
        {
            DTAData = dta;
            Items.AddRange(ParseBetween(Encoding.ASCII.GetString(dta), '(', ')'));
        }

        public static DTAFile FromBytes(byte[] dta)
        {
            return new DTAFile(dta);
        }

        public static IEnumerable<DTASegment> ParseString(string str)
        {
            var ret = new List<DTASegment>();
            try
            {
                if (!str.IsEmpty())
                {
                    var items = ParseBetween(str, '(', ')');

                    if (items.Any())
                    {
                        ret.AddRange(items);
                    }
                }
            }
            catch { }
            return ret;
        }

        static string RemoveComments(string str)
        {
            var between = str.GetBetweenIndex(new[] { ';' }, new[] { '\n', '\r' });
            while (between != null && str.IsNotEmpty())
            {
                var l = str.Length;
                str = str.SubstringEx(0, between.A) + str.SubstringEx(between.B);
                if (str.Length >= l)
                    break;
                between = str.GetBetweenIndex(new[] { ';' }, new[] { '\n', '\r' });
            }
            return str;
        }

        static IEnumerable<DTASegment> ParseBetween(string str, char first, char last, DTASegment parent = null)
        {
            var ret = new List<DTASegment>();

            if (!str.IsEmpty())
            {
                str = str.Replace('\r', '\n');
                str = str.Replace("\n\n", "\n");
                while (str.IndexOf("  ") != -1)
                {
                    str = str.Replace("  ", " ");
                }
                if (str.IsEmpty())
                    return ret;

                str = RemoveComments(str);

                var start = str.IndexOf(first);

                var end = str.IndexOfClosing(first, last, start);
                if (start != -1 && end != -1)
                {
                    var len = end - start;
                    if (len > 0)
                    {
                        var root = new DTASegment(parent, str.Substring(start + 1, len - 1));

                        ret.Add(root);

                        if (end < str.Length - 1)
                        {
                            var remainder = str.Substring(end);
                            ret.AddRange(ParseBetween(str.Substring(end), first, last, parent));
                        }

                        root.Children.AddRange(ParseBetween(str.Substring(start + 1, len - 1), first, last, root));

                    }
                }
            }
            return ret;
        }

        public IEnumerator<DTASegment> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}