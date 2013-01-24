using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;

using ProUpgradeEditor;
using System.Drawing.Drawing2D;

namespace MidiViewer
{
    public partial class MidiViewer : Form
    {
        public delegate void RenderHandler(PaintEventArgs e);

        List<KeyValuePair<Rectangle, Font>> fontBuffer = new List<KeyValuePair<Rectangle, Font>>();
        public Font GetFontForRect(Rectangle rect)
        {
            Font ret = Utility.fretFont;

            var item = fontBuffer.Where(x => x.Key.Height == rect.Height);

            if (item.Any())
            {
                ret = item.Single().Value;
            }
            else
            {
                var font = CreateFontForRect(rect);
                if (font != null)
                {
                    fontBuffer.Add(new KeyValuePair<Rectangle, Font>(rect, font));
                    ret = font;
                }
            }

            return ret;
        }

        public Font CreateFontForRect(Rectangle rect)
        {
            Font ret = null;

            if (rect.Width < 5 || rect.Height < 5)
                return ret;


            var g = Graphics.FromHwnd(this.Handle);

            rect.Height += 2;
            float initialSize = rect.Height - 2;

            float currentSize = initialSize;

            var fmt = Utility.GetStringFormatNoWrap();

            for (int x = 0; x < 10; x++)
            {
                var font = new Font(this.Font.FontFamily, currentSize, FontStyle.Regular, GraphicsUnit.Pixel);

                var height = font.GetHeight(g);

                var measure = g.MeasureString("8", font, rect.Height, fmt);


                var diff = Math.Abs(measure.Height - rect.Height);
                if (diff > 0.1)
                {
                    if (measure.Height > rect.Height)
                    {
                        currentSize -= diff / 2.0f;
                    }
                    else if (measure.Height < rect.Height)
                    {
                        currentSize += diff / 2.0f;
                    }
                    font.Dispose();
                    font = null;
                }
                else
                {
                    ret = font;
                    break;
                }
            }
            return ret;
        }
        Bitmap memoryBmp;

        Sequence sequence;
        BmpRenderer bmpRenderer;

        Data1Renderer data1Renderer;
        class BmpRenderer : PictureBox
        {
            Image bmp;

            public int OffsetX { get; set; }
            public int OffsetY { get; set; }

            double zoomX = 0;
            double zoomY = 0;
            public double ZoomX
            {
                get { return zoomX; }
                set { zoomX = value; }
            }
            public double ZoomY
            {
                get { return zoomY; }
                set { zoomY = value; }
            }

            public BmpRenderer(string name, SmoothingMode smoothingMode)
                : base()
            {
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.Name = name;
                bmp = null;
            }

            public void SetImage(Image img)
            {
                this.Image = img;
                this.bmp = img;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    releaseResources();
                }
                base.Dispose(disposing);
            }

            private void releaseResources()
            {
                if (bmp != null)
                {
                    this.Image = null;
                    bmp.Dispose();
                    bmp = null;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {

                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                if (bmp != null)
                {
                    var b = Bounds;

                    b.Width = (int)(bmp.Width + bmp.Width * zoomX * 150.0);
                    b.Height = (int)(bmp.Height + bmp.Height * zoomY * 100.0);

                    e.Graphics.DrawImage(bmp, b, OffsetX, OffsetY,
                        bmp.Width - OffsetX, bmp.Height - OffsetY, GraphicsUnit.Pixel);
                }
            }
        }
        class Data1Renderer : PictureBox
        {
            Image bmp;
            public static int Data1TextWidth = 30;

            int data1TextSpacing;


            public int OffsetX { get; set; }
            public int OffsetY { get; set; }

            double zoomX = 0;
            double zoomY = 0;
            public double ZoomX
            {
                get { return zoomX; }
                set { zoomX = value; }
            }
            public double ZoomY
            {
                get { return zoomY; }
                set { zoomY = value; }
            }

            public Data1Renderer(string name)
                : base()
            {

                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                this.Name = name;

                bmp = null;
                data1TextSpacing = 0;
            }

            public void SetImage(Image img)
            {
                this.Image = img;
                this.bmp = img;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    releaseResources();
                }
                base.Dispose(disposing);
            }

            private void releaseResources()
            {
                if (bmp != null)
                {
                    this.Image = null;
                    bmp.Dispose();
                    bmp = null;
                }
            }


            public void InitData1Renderer(MidiViewer viewer,
                Panel contentPanel, BmpRenderer bmpRenderer)
            {
                releaseResources();


                this.Location = new Point(bmpRenderer.Location.X - Data1TextWidth,
                    bmpRenderer.Location.Y);

                this.SizeMode = PictureBoxSizeMode.Normal;

                this.Size = new Size(Data1TextWidth, bmpRenderer.Height);
                data1TextSpacing = (int)Math.Round((double)bmpRenderer.Height / (double)128);

                var f = viewer.GetFontForRect(
                    new Rectangle(0, 0,
                        data1TextSpacing.Min(Data1TextWidth), data1TextSpacing.Min(Data1TextWidth)));

                if (f != null)
                {
                    this.Image = null;
                    this.bmp = new Bitmap(Data1TextWidth, bmpRenderer.Height);
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                        for (int x = 128 - 1; x >= 0; x--)
                        {
                            g.DrawString(x.ToStringEx(), f, Brushes.Black,
                                new PointF(0, x * data1TextSpacing));
                        }
                    }
                    this.Image = bmp;
                }

            }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
            }
        }
        public MidiViewer()
        {
            InitializeComponent();


            bmpRenderer = new BmpRenderer("bmpRenderer", SmoothingMode.None);
            bmpRenderer.Location = new Point(Data1Renderer.Data1TextWidth, 0);

            bmpRenderer.SizeMode = PictureBoxSizeMode.Normal;

            this.contentPanel.Controls.Add(bmpRenderer);
            bmpRenderer.Dock = DockStyle.Fill;

            data1Renderer = new Data1Renderer("data1Renderer");

            data1Renderer.InitData1Renderer(this, contentPanel, bmpRenderer);
            data1Renderer.Location = new Point(0, 0);
            data1Renderer.Dock = DockStyle.Left;
            data1Renderer.SizeMode = PictureBoxSizeMode.Normal;
            this.contentPanel.Controls.Add(data1Renderer);


            data1Renderer.BringToFront();
        }

        int ScaleTick(int tick)
        {
            return (int)Math.Ceiling((double)tick / (double)sequence.Division);
        }

        void LoadSequence()
        {
            memoryBmp = new Bitmap(ScaleTick(sequence.GetLength()), 128);
            using (var g = Graphics.FromImage(memoryBmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, memoryBmp.Width, memoryBmp.Height);

                using (var p = new Pen(Color.Black))
                {



                }
            }

            this.bmpRenderer.SetImage(memoryBmp);
            UpdateZoom();
        }

        #region Done
        public string ShowOpenFileDlg(string caption, string defaultFolder, string startupFolder)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = caption;
            dlg.AutoUpgradeEnabled = true;
            dlg.InitialDirectory = startupFolder;
            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.FileName;
            }
            return string.Empty;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = ShowOpenFileDlg("Open File", "", "");
            if (!string.IsNullOrEmpty(f))
            {
                try
                {
                    this.sequence = f.LoadSequenceFile();

                    LoadSequence();
                }
                catch { }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sequence = null;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion


        private void listTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void checkMidiChannelClick(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void listMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void MidiViewer_Load(object sender, EventArgs e)
        {
            listMessages.Items.Add("All Channel Commands");

            listMessages.Items.Add("Meta KeySignature");
            listMessages.Items.Add("Meta Tempo");
            listMessages.Items.Add("Meta TimeSignature");
            listMessages.Items.Add("Meta Other");

            CheckAllChannels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckAllChannels();
        }

        IEnumerable<CheckBox> CheckBoxes
        {
            get { return groupBox1.Controls.ToEnumerable<CheckBox>().Where(x => x != null); }
        }

        private void CheckAllChannels()
        {
            CheckBoxes.ForEach(x => x.Checked = true);
            GetChannels();
            contentPanel.Invalidate();
        }

        List<int> ActiveChannels = new List<int>();

        private void button2_Click(object sender, EventArgs e)
        {
            CheckBoxes.ForEach(x => x.Checked = false);
            GetChannels();

            contentPanel.Invalidate();
        }

        private void GetChannels()
        {
            ActiveChannels.Clear();
            char[] nums = new char[] { '1', '2' };

            CheckBoxes.ForEach(cb =>
            {
                var n = cb.Name.Substring(cb.Name.Length - 2);

                if (!nums.Contains(n[0]))
                {
                    n = n.Substring(1);
                }
                if (cb.Checked)
                {
                    n.ToInt().IfNotNull(x => ActiveChannels.Add(x - 1));
                }
            });
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            contentPanel.Invalidate();

        }

        private void trackBarHZoom_ValueChanged(object sender, EventArgs e)
        {
            contentPanel.Invalidate();
        }

        private void trackBarHZoom_Scroll(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        double GetScrollPercent(ScrollProperties sb)
        {
            var ret = (double)sb.Value / (double)(sb.Maximum - sb.Minimum);
            if (ret > 1.0)
                ret = 1.0;
            if (ret < 0)
                ret = 0;
            return ret;
        }
        void SetScrollPercent(ScrollProperties sb, double percent)
        {
            var v = (int)Math.Round(sb.Minimum + (percent * (double)(sb.Maximum - sb.Minimum)));
            if (v > sb.Maximum)
                v = sb.Maximum;
            if (v < sb.Minimum)
                v = sb.Minimum;
            sb.Value = v;
        }

        private void UpdateZoom()
        {
            var vs = GetScrollPercent(contentPanel.VerticalScroll);
            var hs = GetScrollPercent(contentPanel.HorizontalScroll);

            bmpRenderer.OffsetX = contentPanel.HorizontalScroll.Value;
            bmpRenderer.OffsetY = contentPanel.VerticalScroll.Value;

            bmpRenderer.ZoomX = ((double)trackBarHZoom.Value) / 100.0;
            bmpRenderer.ZoomY = ((double)trackBarVZoom.Value) / 100.0;

            data1Renderer.OffsetY = contentPanel.VerticalScroll.Value;
            data1Renderer.ZoomX = 0;
            data1Renderer.ZoomY = ((double)trackBarVZoom.Value) / 100.0;


            data1Renderer.Invalidate();
            bmpRenderer.Invalidate();
        }

        private void trackBarVZoom_Scroll(object sender, EventArgs e)
        {
            UpdateZoom();
        }
    }
}
