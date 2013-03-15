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
    public class KeyValueObject<TKey, TValue>
    {
        public virtual TKey Key { get; set; }
        public virtual TValue Value { get; set; }

        public KeyValueObject()
        {
            Key = default(TKey);
            Value = default(TValue);
        }

        public KeyValueObject(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}