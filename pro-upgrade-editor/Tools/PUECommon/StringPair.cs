using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace ProUpgradeEditor.Common
{
    public class StringPair : KeyValueObject<string, string>
    {
        public StringPair() { Key = string.Empty; Value = string.Empty; }

        public StringPair(string key, string value) : base(key, value) { }

        public StringPair(StringPair pair) : this(pair.Key, pair.Value) { }

        public override string ToString()
        {
            return "Key: " + (Key ?? "") + " Value: " + (Value ?? "");
        }
    }
}