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
    public class Selector
    {
        public bool IsRight;
        public Point CurrentPoint;
        public Point StartPoint;
        public GuitarChord Chord;
        public bool IsMouseNear = false;
        public bool IsMouseOver = false;

        public Rectangle GetCurrentRect(TrackEditor editor)
        {
            var rect = editor.GetScreenRectFromMessage(Chord);

            return new Rectangle(CurrentPoint,
                new Size(Utility.SelectorWidth,
                     rect.Height));
        }
    }
}