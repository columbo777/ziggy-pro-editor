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

    [DesignerCategory("UserControl")]
    public partial class PEWaveViewerControl : UserControl
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


        Bitmap memoryBMP = null;
        int lastPixelOffset = Int32.MinValue;

        public Bitmap DrawToMemory()
        {
            bool redraw = false;
            if (memoryBMP == null)
            {
                CreateMemoryBMP();
                redraw = true;
            }
            else
            {
                if (memoryBMP.Width != DrawBounds.Width || memoryBMP.Height != DrawBounds.Height)
                {
                    UnloadMemoryBitmap();
                    CreateMemoryBMP();
                    redraw = true;
                }
            }

            if (ClientSize.Width != DrawBounds.Width || ClientSize.Height != DrawBounds.Height)
            {
                ClientSize = new Size(DrawBounds.Width, DrawBounds.Height);
                redraw = true;
            }

            if (lastPixelOffset != Owner.HScrollValue)
            {
                lastPixelOffset = Owner.HScrollValue;
                redraw = true;
            }
            if (Utility.timeScalar != lastTimeScalar)
            {
                lastTimeScalar = Utility.timeScalar;
                redraw = true;
            }

            if (redraw)
            {
                DrawToBitmap(memoryBMP, this.ClientRectangle);
            }
            return memoryBMP;
        }
        double lastTimeScalar = double.MinValue;
        private void CreateMemoryBMP()
        {
            memoryBMP = new Bitmap(this.DrawBounds.Width, this.DrawBounds.Height);
        }





        public Size DrawClientSize { get; set; }
        Rectangle drawBounds;
        public Rectangle DrawBounds
        {
            get { return drawBounds; }
            set
            {
                drawBounds = value;
                this.Width = drawBounds.Width;
                this.Height = drawBounds.Height;
            }
        }

        int BlockAlign(int value, int blockAlign, bool grow)
        {
            var remainder = (value % blockAlign);
            if (remainder != 0)
            {
                if (grow)
                {
                    value += (blockAlign - remainder);
                }
                else
                {
                    value -= remainder;
                }
            }
            return value;
        }

        int BlockAlign(int value)
        {
            var remainder = (value % Header.BlockAlign);
            if (remainder != 0)
            {
                value -= remainder;
            }
            return value;
        }

        public IEnumerable<Pitch.PitchTracker.PitchRecord> GetPitchRecords(float[] samples)
        {
            var ret = new List<Pitch.PitchTracker.PitchRecord>();
            try
            {

                ret.AddRange(tracker.ProcessBuffer(samples));

            }
            catch { }
            return ret;
        }

        public IEnumerable<Pitch.PitchTracker.PitchRecord> GetPitchRecords(GuitarChord chord)
        {
            return GetPitchRecords(getChordSamples(chord));
        }


        private static float[] GetSamples(int channel, int chordDownPixel, int chordUpPixel, PixelSampleCollection pixelSamples)
        {

            var samples = pixelSamples.GetChannel(channel).Where(x => x.Pixel >= chordDownPixel && x.Pixel <= chordUpPixel).SelectMany(x => x.Samples).ToArray();
            return samples;
        }

        private void CheckChannelPitch(List<Pitch.PitchTracker.PitchRecord> ret, float[] fpix)
        {

            if (fpix.Any())
            {
                var pitches = tracker.ProcessBuffer(fpix);

                if (pitches.Any())
                {
                    ret.AddRange(pitches);
                }

            }
        }

        public float[] getChordSamples(GuitarChord chord)
        {
            var chordSamples = ReadPixelSamples(chord.StartTime, chord.EndTime);

            return chordSamples.SelectMany(x => x.Samples).ToArray();
        }

        public class PeakFrequencyPair
        {
            public Peak Peak;

            public double Frequency;
        }
        public class Peak
        {
            public int SampleIndex;
            public double PeakHeight;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                try
                {

                    e.Graphics.FillRectangle(Brushes.White, ClientRectangle);

                    int width = Owner.ClientRectangle.Width;
                    var startTime = Owner.GetTimeFromClientPoint(0);
                    var endTime = Owner.GetTimeFromClientPoint(width);
                    var timePerPix = Owner.GetTimeFromClientPoint(1);

                    var maxValues = new List<PointF>[NumChannels];
                    var minValues = new List<PointF>[NumChannels];
                    for(int x=0;x<NumChannels;x++)
                    {
                        maxValues[x] = new List<PointF>();
                        minValues[x] = new List<PointF>();
                    }
                    
                    for(int pix=0;pix<width;pix++)
                    {
                        var samples = ReadPixelSamples(startTime + MP3AdjustTime, startTime + MP3AdjustTime + (timePerPix * pix));
                        
                        for(int c=0;c<NumChannels;c++)
                        {
                            var sampChan = samples.GetChannel(c).Where(v=> v.Samples.Any()).ToList();
                            if(sampChan.Any())
                            {
                                var max = sampChan.Max(v => v.MaxValue);
                                var min = sampChan.Min(v => v.MinValue);
                                
                                maxValues[c].Add(new PointF(pix, (float)max));
                                minValues[c].Add(new PointF(pix, (float)min));
                            
                            }
                        }
                    }
                    for (int x = 0; x < NumChannels; x++)
                    {
                        var pen = Pens.DarkBlue;
                        if(x == 1)
                            pen = Pens.Blue;
                        e.Graphics.DrawLines(pen, maxValues[x].ToArray());
                        e.Graphics.DrawLines(pen, minValues[x].ToArray());
                    }
                }

                catch { }
            }
            base.OnPaint(e);
        }

        private double CalculateFrequency(Peak a, Peak b, double time)
        {
            return (((double)Header.SampleRate) * (double)Math.Abs(a.SampleIndex - b.SampleIndex)) * time;
        }


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                UnloadMemory();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void UnloadWave()
        {
            if (waveStream != null)
            {
                waveStream.Dispose();
                waveStream = null;
            }
        }

        public void UnloadMemoryBitmap()
        {
            if (memoryBMP != null)
            {
                memoryBMP.Dispose();
                memoryBMP = null;
            }
        }

        public void UnloadMemory()
        {
            UnloadWave();
            UnloadMemoryBitmap();
        }




        private WaveStream waveStream;
        Pitch.PitchTracker tracker;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                if (waveStream != null)
                {
                    try
                    {
                        waveStream.Dispose();
                    }
                    catch { }
                }
                waveStream = value;

                readGarbageFromWave();

                WaveSamples = internalRead(0, TotalSongByteLength).ToArray();

                tracker = new Pitch.PitchTracker();
                tracker.RecordPitchRecords = false;
                tracker.DetectLevelThreshold = 0.01f;
                tracker.SampleRate = waveStream.WaveFormat.SampleRate;

            }
        }

        public int TotalSongByteLength { get { return (Header.AverageBytesPerSecond * WaveStreamLength).Ceiling(); } }

        AudioSample[] WaveSamples;

        private void readGarbageFromWave()
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                var buffer = new byte[BlockAlign(256)];

                for (int x = 0; x < 10; x++)
                {
                    waveStream.Read(buffer, 0, buffer.Length);
                }
            }
        }

        public class AudioSample
        {
            public float[] Samples { get; internal set; }
            public int Channel { get; internal set; }

            public AudioSample(int channel, float[] points)
            {
                this.Channel = channel;
                this.Samples = points;
            }

            public override string ToString()
            {
                return "Channel: " + Channel + " Count: " + Samples.Count();
            }
        }

        public class PixelSample
        {
            public int Channel { get; internal set; }
            public int Pixel { get; internal set; }
            public float[] Samples { get; internal set; }

            public double MinValue { get; internal set; }
            public double MaxValue { get; internal set; }

            public PixelSample(int channel, int pixel, float[] samples)
            {
                Pixel = pixel;

                this.Channel = channel;

                if (samples.Any())
                {
                    MinValue = samples.Min(x => x);
                    MaxValue = samples.Max(x => x);
                }
                else
                {
                    MinValue = 0;
                    MaxValue = 0;
                }

                this.Samples = samples;
            }


            public override string ToString()
            {
                return "Channel: " + Channel + " Pixel: " + Pixel + " MinValue: " + MinValue.ToString() + " MaxValue: " + MaxValue.ToString();
            }

        }


        PixelSampleCollection ReadPixelSamples(double startTime, double endTime)
        {
            var ret = new List<PixelSample>();
            var totalLen = 1.0 * Owner.GetScreenPointFromTime(Owner.GuitarTrack.TotalSongTime);
            var startPoint = 1.0 * Owner.GetScreenPointFromTime(startTime);
            var endPoint = 1.0 * Owner.GetScreenPointFromTime(endTime);

            var startSample = (WaveSamples[0].Samples.Length * (startPoint / totalLen / 2));
            var endSample = (WaveSamples[0].Samples.Length * (endPoint / totalLen / 2));


            var numSamples = endSample - startSample;
            var numPixels = endPoint - startPoint;

            for (int channel = 0; channel < NumChannels; channel++)
            {
                var pixelSamples = new List<float>();
                
                for (int x = startSample.ToInt(); x < endSample.ToInt(); x++)
                {
                    pixelSamples.Add(WaveSamples[channel].Samples[(startSample.ToInt() + x)]);
                }

                ret.Add(new PixelSample(channel, 0, pixelSamples.ToArray()));

            }
            return new PixelSampleCollection(ret);
        }

        public double TransformToZeroOne(double shortValue)
        {
            return (shortValue - (double)Int16.MinValue) / (double)UInt16.MaxValue;
        }
        public double TransformToNegPosOne(double shortValue)
        {
            return TransformToZeroOne(shortValue) * 2.0 - 1.0;
        }


        public double VisibleStartTime { get { return MP3AdjustTime + Owner.GetTimeFromClientPoint(0); } }
        public double VisibleEndTime { get { return MP3AdjustTime + Owner.GetTimeFromClientPoint(Width); } }

        public double VisibleStartByte
        {
            get
            {
                return VisibleStartTime * Header.SampleRate * Header.BlockAlign;
            }
        }

        public double VisibleEndByte
        {
            get
            {
                return VisibleEndTime * Header.SampleRate * Header.BlockAlign;
            }
        }

        public double VisibleByteLength { get { return VisibleEndByte - VisibleStartByte; } }


        double MP3AdjustTime { get { return Owner.MP3PlaybackOffset.ToDouble() / 1000.0; } }

        int TotalPixels { get { return Owner.HScrollMaximum - Owner.GetScreenPointFromTime(MP3AdjustTime); } }

        double GuitarSongLength { get { return Owner.GuitarTrack.TotalSongTime - MP3AdjustTime; } }

        double WaveStreamLength { get { return waveStream.TotalTime.TotalSeconds; } }


        public class FFTBlockResult
        {
            public int BlockSize { get; internal set; }

            /// <summary>
            /// Root Means Square
            /// </summary>
            public double RMS { get; internal set; }

            public double Volume { get; internal set; }

            /// <summary>
            /// Magnitude Spectrum
            /// </summary>
            public double[] Spectrum { get; internal set; }

            public FFTBlockResult(int blockSize, double rms, double volume, double[] spectrum)
            {
                this.BlockSize = blockSize;
                this.RMS = rms;
                this.Volume = volume;
                this.Spectrum = spectrum;
            }
        }

        public class PixelSampleCollection : IEnumerable<PixelSample>
        {
            IEnumerable<PixelSample> samples;

            public PixelSampleCollection(IEnumerable<PixelSample> samples)
            {
                if (samples == null)
                    samples = new List<PixelSample>();
                this.samples = samples;
            }

            public IEnumerator<PixelSample> GetEnumerator()
            {
                return samples.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return samples.GetEnumerator();
            }

            public List<PixelSample> GetChannel(int channel) { return samples.Where(x => x.Channel == channel).ToList(); }
            public float[] GetChannelSamples(int channel) { return samples.Where(x => x.Channel == channel).SelectMany(x => x.Samples).ToArray(); }
        }


        unsafe short[] ConvertToInt16(byte[] items, int itemLength)
        {
            int count = itemLength / 2;
            var ret = new short[count];
            fixed (short* retPtr = ret)
            {
                fixed (byte* itemPtr = items)
                {
                    var currPtr = (short*)itemPtr;
                    var endPtr = (short*)(currPtr + count);
                    var ptr = retPtr;
                    while (currPtr != endPtr)
                    {
                        *(ptr++) = *(currPtr++);
                    }
                }
            }
            return ret;
        }

        unsafe double[] ConvertToDoubleNegPosOne(short[] items, int itemLength)
        {
            int count = itemLength;
            var ret = new double[count];
            fixed (double* retPtr = ret)
            {
                fixed (short* itemPtr = items)
                {
                    var currPtr = (short*)itemPtr;
                    var endPtr = (short*)(currPtr + count);
                    var ptr = retPtr;

                    while (currPtr != endPtr)
                    {
                        *(ptr++) = (((*(currPtr++)) - (double)Int16.MinValue) / (double)UInt16.MaxValue) * 2.0 - 1.0;
                    }
                }
            }
            return ret;
        }
        unsafe double[] ConvertToDoubleAsShort(short[] items, int itemLength)
        {
            int count = itemLength;
            var ret = new double[count];
            fixed (double* retPtr = ret)
            {
                fixed (short* itemPtr = items)
                {
                    var currPtr = (short*)itemPtr;
                    var endPtr = (short*)(currPtr + count);
                    var ptr = retPtr;

                    while (currPtr != endPtr)
                    {
                        *(ptr++) = *(currPtr++);
                    }
                }
            }
            return ret;
        }

        unsafe double[] ConvertToDoubleZeroOneFromDoubleShort(double[] items, int startOffset, int count)
        {
            var ret = new double[count];
            int endOffset = startOffset + count;
            fixed (double* retPtr = ret)
            {
                fixed (double* itemPtr = items)
                {
                    var currPtr = (double*)(itemPtr + startOffset);
                    var endPtr = (double*)(itemPtr + endOffset);
                    var ptr = retPtr;

                    while (currPtr != endPtr)
                    {
                        *(ptr++) = (*(currPtr++) - (double)Int16.MinValue) / (double)UInt16.MaxValue;
                    }
                }
            }
            return ret;
        }

        unsafe double[] ConvertToDoubleAsShort(byte[] items, int itemLength)
        {
            int count = itemLength / 2;

            var ret = new double[count];
            fixed (double* retPtr = ret)
            {
                fixed (byte* itemPtr = items)
                {
                    var currPtr = (short*)itemPtr;
                    var endPtr = (short*)(currPtr + count);
                    var ptr = retPtr;

                    while (currPtr != endPtr)
                    {
                        *(ptr++) = *(currPtr++);
                    }
                }
            }
            return ret;
        }

        unsafe int ConvertToDoubleAsShort(byte[] items, int itemLength, out double[] channel0, out double[] channel1)
        {
            int count = BlockAlign(itemLength, 4, false) / 4;

            channel0 = new double[count];
            channel1 = new double[count];

            fixed (double* ptrZero = channel0)
            {
                fixed (double* ptrOne = channel1)
                {
                    fixed (void* itemPtr = items)
                    {
                        var currPtr = (short*)itemPtr;
                        var endPtr = (short*)(currPtr + count * 2);
                        var ptr0 = ptrZero;
                        var ptr1 = ptrOne;

                        while (currPtr != endPtr)
                        {
                            *(ptr0++) = *(currPtr++);
                            *(ptr1++) = *(currPtr++);
                        }
                    }
                }
            }
            return count;
        }
        unsafe int ConvertToDoubleNegPosOne(byte[] items, int itemLength, out double[] channel0, out double[] channel1)
        {
            int count = BlockAlign(itemLength, 4, false) / 4;

            channel0 = new double[count];
            channel1 = new double[count];

            fixed (double* ptrZero = channel0)
            {
                fixed (double* ptrOne = channel1)
                {
                    fixed (void* itemPtr = items)
                    {
                        var currPtr = (short*)itemPtr;
                        var endPtr = (short*)(currPtr + count * 2);
                        var ptr0 = ptrZero;
                        var ptr1 = ptrOne;

                        while (currPtr != endPtr)
                        {
                            *(ptr0++) = (((*(currPtr++)) - (double)Int16.MinValue) / (double)UInt16.MaxValue) * 2.0 - 1.0;
                            *(ptr1++) = (((*(currPtr++)) - (double)Int16.MinValue) / (double)UInt16.MaxValue) * 2.0 - 1.0;
                        }
                    }
                }
            }
            return count;
        }
        unsafe int ConvertToFloatNegPosOne(byte[] items, int itemLength, out float[] channel0, out float[] channel1)
        {
            int count = BlockAlign(itemLength, 4, false) / 4;

            channel0 = new float[count];
            channel1 = new float[count];

            fixed (float* ptrZero = channel0)
            {
                fixed (float* ptrOne = channel1)
                {
                    fixed (void* itemPtr = items)
                    {
                        var currPtr = (short*)itemPtr;
                        var endPtr = (short*)(currPtr + count * 2);
                        var ptr0 = ptrZero;
                        var ptr1 = ptrOne;

                        while (currPtr != endPtr)
                        {
                            *(ptr0++) = (((*(currPtr++)) - (float)Int16.MinValue) / (float)UInt16.MaxValue) * 2.0f - 1.0f;
                            *(ptr1++) = (((*(currPtr++)) - (float)Int16.MinValue) / (float)UInt16.MaxValue) * 2.0f - 1.0f;
                        }
                    }
                }
            }
            return count;
        }
        IEnumerable<AudioSample> internalRead(int byteOffset, int numBytes)
        {
            var ret = new List<AudioSample>();

            byteOffset = BlockAlign(byteOffset);

            waveStream.Position = byteOffset;

            numBytes = BlockAlign(numBytes);
            var buffer = new byte[numBytes];

            var byteCount = waveStream.Read(buffer, 0, buffer.Length);
            if (byteCount < Header.BlockAlign)
                return ret;

            switch (waveStream.WaveFormat.BitsPerSample)
            {
                case 8:
                    {
                        throw new NotImplementedException();
                    }
                //break;
                case 16:
                    {
                        if (NumChannels == 2)
                        {
                            float[] channel0;
                            float[] channel1;
                            var itemCount = ConvertToFloatNegPosOne(buffer, byteCount, out channel0, out channel1);
                            if (itemCount > 0)
                            {
                                ret.Add(new AudioSample(0, channel0));
                                ret.Add(new AudioSample(1, channel1));
                            }
                        }
                    }
                    break;
                case 24:
                    {
                        throw new NotImplementedException();
                    }
                // break;
                case 32:
                    {
                        throw new NotImplementedException();
                    }
                // break;
            }
            return ret;
        }


        private WaveFormat Header
        {
            get
            {
                return waveStream.WaveFormat;
            }
        }

        public TrackEditor Owner { get; set; }



        int NumChannels { get { return Header.Channels; } }

    }
    class Fourier
    {
        public const double W0Hanning = 0.5;
        public const double W0Hamming = 0.54;
        public const double W0Blackman = 0.42;
        private const double Pi = 3.14159265358979;

        private double[] cosarray;
        private double[] sinarray;
        private bool _forward;
        private int _arraySize;
        private int _ldArraysize = 0;

        public Fourier(int arraySize, bool forward)
        {
            _arraySize = arraySize;
            _forward = forward;
            cosarray = new double[arraySize];
            sinarray = new double[arraySize];

            double sign = 1.0;
            if (forward)
                sign = -1.0;

            double phase0 = 2.0 * Pi / arraySize;
            for (int i = 0; i <= arraySize - 1; i++)
            {
                sinarray[i] = sign * Math.Sin(phase0 * i);
                cosarray[i] = Math.Cos(phase0 * i);
            }

            int j = _arraySize;
            while (j != 1)
            {
                _ldArraysize++;
                j /= 2;
            }
        }

        public void MagnitudeSpectrum(double[] real, double[] imag, double w0, double[] magnitude)
        {
            for (var i = 0; i < (_arraySize / 2); i++)
            {
                var sum1 = Math.Sqrt(SquareSum(real[i], imag[i]));
                var sum2 = Math.Sqrt(SquareSum(real[_arraySize - i - 1], imag[_arraySize - i - 1]));

                magnitude[i] = sum1 + sum2 / w0;
            }
        }

        public static double Hanning(int n, int j)
        {
            return W0Hanning - 0.5 * Math.Cos(2.0 * Pi * j / n);
        }

        public static double Hamming(int n, int j)
        {
            return W0Hamming - 0.46 * Math.Cos(2.0 * Pi * j / n);
        }

        public static double Blackman(int n, int j)
        {
            return W0Blackman - 0.5 * Math.Cos(2.0 * Pi * j / n) + 0.08 * Math.Cos(4.0 * Pi * j / n);
        }

        private static void Swap(ref double a, ref double b)
        {
            double temp = a;
            a = b;
            b = temp;
        }

        private static double SquareSum(double a, double b)
        {
            return a * a + b * b;
        }

        public void FourierTransform(double[] real, double[] imag)
        {

            if (_forward)
            {
                for (var i = 0; i < _arraySize; i++)
                {
                    real[i] /= _arraySize;
                    imag[i] /= _arraySize;
                }
            }

            int j = 0;
            for (var i = 0; i < _arraySize - 1; i++)
            {
                if (i < j)
                {
                    Swap(ref real[i], ref real[j]);
                    Swap(ref imag[i], ref imag[j]);
                }
                var k = _arraySize / 2;

                while (k <= j)
                {
                    j -= k;
                    k /= 2;

                }

                j += k;
            }

            int a = 2;
            int b = 1;
            for (var count = 0; count < _ldArraysize; count++)
            {
                int c0 = _arraySize / a;
                int c1 = 0;
                for (var k = 0; k < b; k++)
                {
                    var i = k;
                    while (i < _arraySize / 2 - 1)
                    {
                        int arg = i + b;
                        double prodreal;
                        double prodimag;
                        if (k == 0)
                        {
                            prodreal = real[arg];
                            prodimag = imag[arg];
                        }
                        else
                        {
                            prodreal = real[arg] * cosarray[c1] - imag[arg] * sinarray[c1];
                            prodimag = real[arg] * sinarray[c1] + imag[arg] * cosarray[c1];
                        }
                        real[arg] = real[i] - prodreal;
                        imag[arg] = imag[i] - prodimag;
                        real[i] += prodreal;
                        imag[i] += prodimag;
                        i += a;
                    }
                    c1 += c0;
                }
                a *= 2;
                b *= 2;
            }
        }

    }
}
