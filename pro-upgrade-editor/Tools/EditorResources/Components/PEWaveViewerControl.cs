using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.Drawing;
using System.Windows.Forms;

namespace EditorResources.Components
{
    [DesignerCategory("UserControl")]
    public partial class PEWaveViewerControl : PUEControl
    {
        public PEWaveViewerControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public PEWaveViewerControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            this.DoubleBuffered = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            FitToScreen();
        }
        public void FitToScreen()
        {
            if (waveStream == null)
                return;

            startPosition = 0;

            var samples = (int)(waveStream.Length / bytesPerSample);

            SamplesPerPixel = samples / Math.Max(1, this.Width);

            Invalidate();
        }

        public void Zoom(int leftSample, int rightSample)
        {
            if (rightSample - leftSample <= 1)
                return;
            startPosition = leftSample * bytesPerSample;
            SamplesPerPixel = (rightSample - leftSample) / Math.Max(1, this.Width);
            Invalidate();
        }

        public int GetSamplePositionFromClient(int x)
        {
            return (int)(StartPosition / bytesPerSample + SamplesPerPixel * x);
        }




        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                if (waveStream != null)
                {
                    waveStream.Dispose();
                    waveStream = null;
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }




        private WaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {

                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                startPosition = 0;
                this.Invalidate();
            }
        }

        public int FramesPerPixel
        {
            get;
            set;
        }
        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
                Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            var highPoints = new List<PointF>();
            var lowPoints = new List<PointF>();

            float height = (float)this.Height;
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                {

                    bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                    if (bytesRead == 0)
                        break;

                    float sampleLow = 0;
                    float sampleHigh = 0;
                    for (int n = 0; n < bytesRead; n += 2)
                    {
                        short sample = BitConverter.ToInt16(waveData, n);
                        if (sample < sampleLow)
                            sampleLow = sample;
                        if (sample > sampleHigh)
                            sampleHigh = sample;
                    }

                    float lowPercent = (((sampleLow) - short.MinValue) / ushort.MaxValue);
                    float highPercent = (((sampleHigh) - short.MinValue) / ushort.MaxValue);

                    highPoints.Add(new PointF(x, highPercent * height));
                    lowPoints.Add(new PointF(x, lowPercent * height));
                }

                lowPoints.Reverse();
                highPoints.AddRange(lowPoints);

                e.Graphics.FillPolygon(Brushes.Blue, highPoints.ToArray());


            }



            base.OnPaint(e);
        }


    }

}
