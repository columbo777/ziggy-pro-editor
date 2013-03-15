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
    public struct GuitarMessagePair : DownUpPair<GuitarMessage>
    {
        GuitarMessage down;
        GuitarMessage up;
        public GuitarMessagePair(GuitarMessage down)
        {
            this.down = down;
            this.up = null;
        }
        public GuitarMessagePair(GuitarMessage down, GuitarMessage up)
        {
            this.down = down;
            this.up = up;
        }

        public GuitarMessage Down
        {
            get { return this.down; }
            set { this.down = value; }
        }

        public GuitarMessage Up
        {
            get { return this.up; }
            set { this.up = value; }
        }


        public bool IsNull
        {
            get { return (up == null && down == null); }
        }
    }
}