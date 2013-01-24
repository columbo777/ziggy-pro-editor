using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Gui;
using NAudio.Wave;

namespace EditorResources.Components
{
    public partial class PEWaveViewer : Form
    {

        PEWaveViewer()
            : base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Invalidate();
        }
        public static bool ShowWave(string fileName)
        {
            bool ret = false;
            try
            {
                var waveViewer = new PEWaveViewer();
                waveViewer.peWaveViewerControl1.WaveStream = new Mp3FileReader(fileName);
                waveViewer.Show();

                ret = true;
            }
            catch { }

            return ret;
        }

        protected override void OnClosed(EventArgs e)
        {
            peWaveViewerControl1.Dispose();
            base.OnClosed(e);
        }


        Point mouseDownPoint = new Point();

        private void peWaveViewerControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
            {
                mouseDownPoint = e.Location;
            }
        }

        private void peWaveViewerControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
            {

                var left = Math.Min(mouseDownPoint.X, e.Location.X);
                var right = Math.Max(mouseDownPoint.X, e.Location.X);

                if (Math.Abs(left - right) > 2)
                {
                    peWaveViewerControl1.Zoom(
                        peWaveViewerControl1.GetSamplePositionFromClient(left),
                        peWaveViewerControl1.GetSamplePositionFromClient(right));
                }
            }
        }

        private void peWaveViewerControl1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void peWaveViewerControl1_Load(object sender, EventArgs e)
        {
            Invalidate();
        }

    }
}
