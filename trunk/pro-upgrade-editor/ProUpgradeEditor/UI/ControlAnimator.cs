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
    public class ControlAnimator
    {
        static List<ControlAnimatorItem> animatingControls = new List<ControlAnimatorItem>();


        public static void CreateHeightChange(Control control, int to, double seconds = 0.50)
        {
            var item = new ControlAnimatorItem()
            {
                Routine = AnimationRoutine.EaseInOutSine,
                Control = control,
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now + TimeSpan.FromSeconds(seconds),
                Type = ControlAnimatorType.Height,
            };

            item.Start = new Point(0, control.Height);
            item.End = new Point(0, to);

            var oldItem = animatingControls.SingleOrDefault(x => x.Control == control && x.Type == ControlAnimatorType.Height);
            if (oldItem != null)
            {
                control.Height = oldItem.End.Y;
                int delta = control.Height - to;
                item.End.Y += delta;
                animatingControls.Remove(oldItem);
            }

            animatingControls.Add(item);
        }



        public static double EaseInOutSine(double x, double t, double b, double c, double d)
        {
            return -c / 2.0 * (Math.Cos(Math.PI * x) - 1.0) + b;
        }

        public static double GetProgress(AnimationRoutine routine, double elapsed, double totalAnimTime)
        {
            double ret = 1.0;
            if (routine == AnimationRoutine.EaseInOutSine)
            {
                if (elapsed > totalAnimTime)
                    elapsed = totalAnimTime;
                ret = EaseInOutSine(elapsed / totalAnimTime, elapsed, 0, totalAnimTime, 1.0);
            }
            return ret;
        }

        public static Point GetUpdatedPoint(
            double progress,
            Point start, Point end)
        {
            var ret = new Point(start.X + (int)(Math.Round((end.X - start.X) * progress)),
                             start.Y + (int)(Math.Round((end.Y - start.Y) * progress)));

            return ret;
        }
        /*
         * 
x = 0 - 1 as float of completion
t = elapsed time in ms
b = 0
c = 1
d = duration in ms


easeInQuad: function (x, t, b, c, d) {
    return c*(t/=d)*t + b;
},
easeOutQuad: function (x, t, b, c, d) {
    return -c *(t/=d)*(t-2) + b;
},
easeInOutQuad: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t + b;
    return -c/2 * ((--t)*(t-2) - 1) + b;
},
easeInCubic: function (x, t, b, c, d) {
    return c*(t/=d)*t*t + b;
},
easeOutCubic: function (x, t, b, c, d) {
    return c*((t=t/d-1)*t*t + 1) + b;
},
easeInOutCubic: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t + b;
    return c/2*((t-=2)*t*t + 2) + b;
},
easeInQuart: function (x, t, b, c, d) {
    return c*(t/=d)*t*t*t + b;
},
easeOutQuart: function (x, t, b, c, d) {
    return -c * ((t=t/d-1)*t*t*t - 1) + b;
},
easeInOutQuart: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
    return -c/2 * ((t-=2)*t*t*t - 2) + b;
},
easeInQuint: function (x, t, b, c, d) {
    return c*(t/=d)*t*t*t*t + b;
},
easeOutQuint: function (x, t, b, c, d) {
    return c*((t=t/d-1)*t*t*t*t + 1) + b;
},
easeInOutQuint: function (x, t, b, c, d) {
    if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
    return c/2*((t-=2)*t*t*t*t + 2) + b;
},
easeInSine: function (x, t, b, c, d) {
    return -c * Math.cos(t/d * (Math.PI/2)) + c + b;
},
easeOutSine: function (x, t, b, c, d) {
    return c * Math.sin(t/d * (Math.PI/2)) + b;
},
easeInOutSine: function (x, t, b, c, d) {
    return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;
},
easeInExpo: function (x, t, b, c, d) {
    return (t==0) ? b : c * Math.pow(2, 10 * (t/d - 1)) + b;
},
easeOutExpo: function (x, t, b, c, d) {
    return (t==d) ? b+c : c * (-Math.pow(2, -10 * t/d) + 1) + b;
},
easeInOutExpo: function (x, t, b, c, d) {
    if (t==0) return b;
    if (t==d) return b+c;
    if ((t/=d/2) < 1) return c/2 * Math.pow(2, 10 * (t - 1)) + b;
    return c/2 * (-Math.pow(2, -10 * --t) + 2) + b;
},
         * */
        public static void Update(int elapsedMS)
        {
            DateTime now = DateTime.Now;
            double timeElapsed = elapsedMS.CastObject<double>() / 1000.0;

            foreach (var item in animatingControls.ToList())
            {
                if (now >= item.TimeStart)
                {
                    item.CurrentTime = now;
                    Point point = item.End;
                    if (item.TimeElapsed < item.TotalAnimTime)
                    {
                        var progress = GetProgress(item.Routine, item.Progress, item.TotalAnimTime);
                        point = GetUpdatedPoint(progress, item.Start, item.End);
                    }
                    else
                    {
                        item.Completed = true;
                    }

                    if (item.Type == ControlAnimatorType.Height)
                    {
                        item.Control.Height = point.Y;
                    }

                    item.Control.Parent.Invalidate();

                    if (item.Completed)
                    {
                        animatingControls.Remove(item);
                    }
                }
            }
        }
    }
}