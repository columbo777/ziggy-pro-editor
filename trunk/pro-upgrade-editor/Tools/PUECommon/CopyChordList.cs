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
    public class CopyChordList : GuitarMessageList
    {
        public CopyChordList(TrackEditor owner)
            : base(owner)
        {

        }

        public void Clear()
        {
            this.ToList().ForEach(x => x.RemoveFromList());
        }

        public override void Add<T>(T mess)
        {
            if (mess is GuitarChord)
            {
                Chords.Add((mess as GuitarChord).CloneToMemory(this, Owner.CurrentDifficulty));
            }
            else
            {
                base.Add(mess);
            }
        }

        public void AddRange(IEnumerable<GuitarChord> chords)
        {
            foreach (var chord in chords.ToList())
            {
                Add(chord);
            }
        }


        int MinSelectionString
        {
            get
            {
                if (base.Chords.Any())
                {
                    return base.Chords.Min(i => i.Notes.Min(x => x.NoteString));
                }
                else
                {
                    return 0;
                }
            }
        }


        Point mousePointBegin;
        int mouseStringBegin;
        Point FirstChordOffset;

        public void Begin(Point mousePoint)
        {
            mousePointBegin = Owner.SelectStartPoint;
            mouseStringBegin = Owner.SnapToString(Owner.SelectStartPoint.Y);
            var sp = this.Chords.First().ScreenPointPair;
            FirstChordOffset = new Point(sp.Down - Owner.HScrollValue, mouseStringBegin);

            UpdatePastePoint(mousePoint, Point.Empty);
        }
        public void BeginPaste(Point startPoint)
        {
            Owner.SelectStartPoint = startPoint;

            mousePointBegin = Owner.SelectStartPoint;
            mouseStringBegin = MinSelectionString;

            var sp = Chords.First().ScreenPointPair;

            FirstChordOffset = new Point(mousePointBegin.X, 0);

            UpdatePastePoint(mousePointBegin, Point.Empty);
        }
        public void UpdatePastePoint(Point newMousePosition, Point mouseDelta)
        {
            if (!Chords.Any())
            {
                return;
            }

            var mouseString = Owner.SnapToString(newMousePosition.Y);

            var offset = new Point(FirstChordOffset.X - mousePointBegin.X, mouseStringBegin - MinSelectionString);


            if (Owner.GridSnap)
            {
                var screenPoint = Owner.GetClientPointFromTick(Chords.GetTickPair());

                var offsetPointLeft = new Point(newMousePosition.X + offset.X, mouseString - offset.Y);
                var offsetPointRight = new Point(newMousePosition.X + offset.X + screenPoint.TickLength, mouseString - offset.Y);

                int snapLeft;
                var snappedLeft = Owner.GetGridSnapPointFromClientPoint(offsetPointLeft, out snapLeft);
                int snapRight;
                var snappedRight = Owner.GetGridSnapPointFromClientPoint(offsetPointRight, out snapRight);

                if (snappedLeft && snappedRight)
                {
                    if (mouseDelta.X < 0)// Math.Abs(offsetPointLeft.X - snapLeft) < Math.Abs(offsetPointRight.X - snapRight))
                    {
                        newMousePosition.X = snapLeft - offset.X;
                    }
                    else
                    {
                        newMousePosition.X = snapRight - screenPoint.TickLength - offset.X;
                    }
                }
                else if (snappedLeft)
                {
                    newMousePosition.X = snapLeft - offset.X;
                }
                else if (snappedRight)
                {
                    newMousePosition.X = snapRight - screenPoint.TickLength - offset.X;
                }

            }


            Owner.CurrentPastePoint.MousePos = new Point(newMousePosition.X, mouseString);
            Owner.CurrentPastePoint.MinChordX = newMousePosition.X;
            Owner.CurrentPastePoint.Offset = offset;
            Owner.CurrentPastePoint.MinNoteString = MinSelectionString;

            var pastePoint = Owner.CurrentPastePoint;
            var copyRange = Chords.GetTickPair();

            var stringOffset = (pastePoint.MousePos.Y) - pastePoint.MinNoteString - pastePoint.Offset.Y;

            var startTick = Owner.GetTickFromClientPoint(pastePoint.MousePos.X + pastePoint.Offset.X);

            var pasteRange = new TickPair(startTick, startTick + copyRange.TickLength);

            var copyTime = Owner.GuitarTrack.TickToTime(copyRange);

            var pasteTime = Owner.GuitarTrack.TickToTime(pasteRange);

            pasteTime.TimeLength = copyTime.TimeLength;
            pasteRange.Up = Owner.GuitarTrack.TimeToTick(pasteTime.Up);

            pasteRange = Owner.SnapTickPairPro(pasteRange);

            foreach (var c in Chords)
            {
                var noteTime = Owner.GuitarTrack.TickToTime(c.TickPair);

                var delta = noteTime.Down - copyTime.Down;

                var startEndTick = new TickPair(
                    Owner.GuitarTrack.TimeToTick(pasteTime.Down + delta),
                    Owner.GuitarTrack.TimeToTick(pasteTime.Down + delta + c.TimeLength));

                c.SetTicks(startEndTick);
            }
        }
    }
}