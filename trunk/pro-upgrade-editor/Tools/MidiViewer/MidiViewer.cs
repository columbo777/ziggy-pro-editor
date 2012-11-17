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


namespace MidiViewer
{
    public partial class MidiViewer : Form
    {
        public delegate void RenderHandler(PaintEventArgs e);

        class MidiRenderPanel : Panel
        {
            public MidiRenderPanel(Panel parent) : base()
            {

                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.SupportsTransparentBackColor, true);

                

                parent.Controls.Add(this);

                Dock = DockStyle.Fill;
                Location = new System.Drawing.Point(0, 0);
                Name = "midiRenderPanel";
                Size = new System.Drawing.Size(684, 344);
                TabIndex = 3;

            }

            public event RenderHandler OnRender;

            protected override void OnPaint(PaintEventArgs e)
            {
                if (OnRender != null)
                {
                    OnRender(e);
                }
            }
            
        }

        MidiRenderPanel renderPanel;

        void CreateRenderPanel()
        {
            renderPanel = new MidiRenderPanel(this.panel1);

            renderPanel.OnRender += new RenderHandler(renderPanel_OnRender);
        }

        int numCellsForText = 2;
        int numCells = 0;
        int cellHeight = 12;
        int cellWidth = 20;
        int maxItemsVisX = 0;
        int maxItemsVisY = 0;
        bool[,] vis;

        void renderPanel_OnRender(PaintEventArgs e)
        {
            maxItemsVisX = (renderPanel.Width / cellWidth) - numCellsForText;
            maxItemsVisY = (renderPanel.Height / cellHeight);
            

            var visTracks = new List<Track>();
            foreach (Track t in listTracks.SelectedItems)
            {
                visTracks.Add(t);
            }

            if (visTracks.Count == 0)
                return;

            vis = new bool[maxItemsVisX+1, NumData+1];

            var track = visTracks[0];

            bool viewChan = listMessages.SelectedItems.Contains("All Channel Commands");
            bool viewKeySig = listMessages.SelectedItems.Contains("Meta KeySignature");
            bool viewTempo = listMessages.SelectedItems.Contains("Meta Tempo");
            bool viewTimeSig = listMessages.SelectedItems.Contains("Meta TimeSignature");
            bool viewOtherMeta = listMessages.SelectedItems.Contains("Meta Other");

            this.g = e.Graphics;


            FillEmptyCell(renderPanel.ClientRectangle);



            Rectangle left = new Rectangle(0, 0, cellWidth * numCellsForText, renderPanel.Height);
            DrawLeftRect(left);

            for (int x = 0; x < numCells; x++)
            {
                int cellTop = ((NumData - (x+vScrollBar1.Value))) * cellHeight;
                int cellBottom = cellTop + cellHeight;
                Rectangle leftText = new Rectangle(0, cellTop, cellWidth * numCellsForText, cellHeight);

                if (cellBottom > 0 &&
                    cellTop < renderPanel.Height)
                {
                    DrawText(x.ToString(), leftText);
                    DrawRect(leftText);
                }
            }
            

            var minTick = hScrollBar1.Value * tickScalar;
            var maxTick = minTick + maxItemsVisX * tickScalar;

            
            IEnumerable<MidiEvent> chanMessages = null;
            if (viewChan)
            {
                chanMessages = messagesWhereData1Between(getMessages(track, minTick, maxTick),
                    0, NumData);

                if (chanMessages.Count() > 0)
                {
                    foreach (var m in chanMessages)
                    {
                        var cm = (m.MidiMessage as ChannelMessage);
                        int cellX = (int)Math.Round((double) m.AbsoluteTicks / (double)tickScalar, MidpointRounding.ToEven) - hScrollBar1.Value;

                        vis[cellX, NumData-cm.Data1] = true;
                    }
                }
            }

            int max = hScrollBar1.Value+maxItemsVisX;
            if(max >= numCells)
                max = numCells;
            for (int cellXIndex = hScrollBar1.Value; cellXIndex < max; cellXIndex++)
            {
                int cellX = (cellXIndex - hScrollBar1.Value + numCellsForText) * cellWidth;

                int cellRight = cellX + cellWidth;

                bool rightGTL = cellRight > e.ClipRectangle.Left;
                bool leftLTR = cellX < e.ClipRectangle.Right;

                int cellTick = cellXIndex * tickScalar;

                if (rightGTL && leftLTR)
                {
                   
                    for (int data1 = 0; data1 <= NumData; data1++)
                    {
                        int cellY = (data1-vScrollBar1.Value) * cellHeight;
                        int cellBottom = cellY + cellHeight;

                        if (cellBottom > e.ClipRectangle.Top &&
                            cellY < e.ClipRectangle.Bottom)
                        {
                            var rect = new Rectangle(cellX, cellY, cellWidth, cellHeight);

                            if (vis[cellXIndex - hScrollBar1.Value, data1])
                            {
                                FillDataCell(rect);
                            }
                            else
                            {
                                
                            }
                            DrawRect(rect);
                        }

                    }
                }
                else
                {
                }
            }
        }

        private static IEnumerable<MidiEvent> messagesWhereData1Between(IEnumerable<MidiEvent> chanMessages, int minData1, int maxData1)
        {
            int id;
            return chanMessages.Where(x => (id=(x.MidiMessage as ChannelMessage).Data1) >= minData1 && id <= maxData1  );
        }

        private IEnumerable<MidiEvent> getMessages(Track track, int minTick, int maxTick)
        {
            var messages = track.ChanMessages.Where(x => x.AbsoluteTicks >= minTick &&
                                x.AbsoluteTicks < maxTick &&
                                channels[((x.MidiMessage)as ChannelMessage).MidiChannel]==true);

            return messages;
        }

        Sequence sequence;
        
        Brush brushBack;
        Brush brushEmptyCell;
        Brush brushDataCell;
        Brush brushLeft;
        Pen penGrid;

        Graphics g;
        int tickScalar = 480;
        const int NumData = 128;

        public MidiViewer()
        {
            InitializeComponent();
        }

        void DrawGridLine(Point a, Point b)
        {
            g.DrawLine(penGrid, a, b);
        }

        void FillEmptyCell(Rectangle rect)
        {
            g.FillRectangle(brushEmptyCell, rect);
        }

        void FillDataCell(Rectangle rect)
        {
            g.FillRectangle(brushDataCell, rect);
        }

        void DrawRect(Rectangle rect)
        {
            g.DrawRectangle(penGrid, rect);
        }

        void DrawLeftRect(Rectangle rect)
        {
            g.FillRectangle(brushLeft, rect);

        }

        void DrawText(string str, Rectangle rect)
        {
            TextRenderer.DrawText(g, str, this.Font, rect,
                this.ForeColor, this.BackColor, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.Default);
        }

        void LoadSequence()
        {
            
            var kb = new KeySignatureBuilder();
            var tc = new TempoChangeBuilder();
            var ts = new TimeSignatureBuilder();

            listTracks.Items.Clear();

            int minDelta=int.MaxValue;
            
            foreach (var track in sequence)
            {
                listTracks.Items.Add(track);

                int last = int.MaxValue;
                foreach(var ev in track.Events)
                {
                    if (last != int.MaxValue)
                    {
                        if (last != ev.AbsoluteTicks)
                        {
                            int delta = ev.AbsoluteTicks - last;
                            if (delta < minDelta && delta > 5)
                            {
                                minDelta = delta;
                            }
                        }
                    }
                    last = ev.AbsoluteTicks;
                }
            }

            tickScalar = minDelta;
            if (tickScalar > 1)
            {
                int scale = -1;
                while (tickScalar > cellWidth)
                {
                    tickScalar = tickScalar / 2;
                    scale++;
                }

                tickScalar = minDelta;
                if (scale > 0)
                    tickScalar *= scale;
            }
            else
            {
                tickScalar = 1;
            }
            numCells = (int)Math.Round(((double)sequence.GetLength() / (double)tickScalar), MidpointRounding.AwayFromZero);

            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = numCells;

            if (listTracks.Items.Count > 0)
            {
                listTracks.SelectedItem = listTracks.Items[0];
            }

            listMessages.SelectedIndex = 0;
        }

        #region comm
        /*
                currPos = AddLabel("Track:", track.Name, currPos);

                currPos.X += 20;
                foreach (var ev in track.Events)
                {
                    currPos = AddLabel("Ticks:", ev.AbsoluteTicks, currPos);

                    var mess = ev.MidiMessage;
                    if (mess is MetaMessage)
                    {
                        var mm = mess as MetaMessage;
                        
                        currPos.X += 10;
                        currPos = AddLabel("Meta:", mm.MetaType.ToString(), currPos);
                        
                        switch (mm.MetaType)
                        {
                            case MetaType.KeySignature:
                                {
                                    kb.Initialize(mm);
                                    currPos = AddLabel("Key:", kb.Key.ToString(), currPos);
                                }
                                break;
                            case MetaType.Tempo:
                                {
                                    tc.Initialize(mm);
                                    currPos = AddLabel("Tempo:", tc.Tempo.ToString(), currPos);
                                }
                                break;
                            case MetaType.TimeSignature:
                                {
                                    ts.Initialize(mm);
                                    currPos = AddLabel("Numerator:", ts.Numerator.ToString(), currPos);
                                    currPos = AddLabel("Denominator:", ts.Denominator.ToString(), currPos);
                                    currPos = AddLabel("ClocksPerMetronomeClick:", ts.ClocksPerMetronomeClick.ToString(), currPos);
                                    currPos = AddLabel("ThirtySecondNotesPerQuarterNote:", ts.ThirtySecondNotesPerQuarterNote.ToString(), currPos);
                                }
                                break;
                            default:
                                {
                                    currPos = AddLabel("Data:", Encoding.ASCII.GetString(mm.GetBytes()), currPos);
                                }
                                break;
                        }
                        
                        currPos.X -= 10;
                    }
                    else if (mess is ChannelMessage)
                    {
                        var cm = mess as ChannelMessage;

                        currPos.X += 10;

                        currPos = AddLabel("Channel:", cm.MidiChannel.ToString(), currPos);
                        currPos = AddLabel("Command:", cm.Command.ToString(), currPos);
                        currPos = AddLabel("Data1:", cm.Data1.ToString(), currPos);
                        currPos = AddLabel("Data2:", cm.Data2.ToString(), currPos);

                        currPos.X -= 10;
                    }
                }
                currPos.X -= 20;

                contentPanel.Height = currPos.Y + 30;

                break;
            }
            
        }
        */
#endregion

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

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            if(renderPanel != null)
                renderPanel.Invalidate();
            base.OnInvalidated(e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = ShowOpenFileDlg("Open File", "", "");
            if (!string.IsNullOrEmpty(f))
            {
                try
                {
                    this.sequence = new Sequence(FileType.Unknown,f);
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
            renderPanel.Invalidate();
        }

        private void checkMidiChannelClick(object sender, EventArgs e)
        {
            
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            renderPanel.Invalidate();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            renderPanel.Invalidate();
        }

        private void listMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderPanel.Invalidate();
        }

        private void MidiViewer_Load(object sender, EventArgs e)
        {
            brushBack = new SolidBrush(Color.FromArgb(70, 70, 100));
            brushEmptyCell = new SolidBrush(Color.FromArgb(170, 170, 200));
            brushDataCell = new SolidBrush(Color.FromArgb(250, 170, 170));
            brushLeft = new SolidBrush(Color.FromArgb(220, 220, 240));
            penGrid = new Pen(Color.FromArgb(200, 200, 220));
            CreateRenderPanel();

            this.vScrollBar1.Value = 0;
            this.vScrollBar1.Maximum = NumData;
            

            this.hScrollBar1.Value = 0;
            this.hScrollBar1.Maximum = 0;


            listMessages.Items.Add("All Channel Commands");
            
            listMessages.Items.Add("Meta KeySignature");
            listMessages.Items.Add("Meta Tempo");
            listMessages.Items.Add("Meta TimeSignature");
            listMessages.Items.Add("Meta Other");

            CheckAllChannels();
        }

        private void MidiViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            brushBack.Dispose();
            brushEmptyCell.Dispose();
            brushDataCell.Dispose();
            penGrid.Dispose();
        }


        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckAllChannels();
        }

        private void CheckAllChannels()
        {
            foreach (var cb in checkBoxes)
            {
                cb.Checked = true;
            }
            GetChannels();
            renderPanel.Invalidate();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            renderPanel.Invalidate();
        }

        List<CheckBox> cbs = null;
        List<CheckBox> checkBoxes
        {
            get
            {
                if (cbs == null)
                {
                    cbs = new List<CheckBox>();

                    foreach (var c in groupBox1.Controls)
                    {
                        var cb = c as CheckBox;
                        if (cb != null)
                        {
                            cbs.Add(cb);
                        }
                    }
                }
                return cbs;
            }
        }
        bool[] channels = new bool[22];
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var cb in checkBoxes)
            {
                cb.Checked = false;
            }
            GetChannels();

            renderPanel.Invalidate();
        }

        private void GetChannels()
        {
            char[] nums = new char[] { '1', '2' };
            foreach (var cb in checkBoxes)
            {
                var n = cb.Name.Substring(cb.Name.Length - 2);

                if (!nums.Contains(n[0]))
                {
                    n = n.Substring(1);
                }
                int i = int.Parse(n)-1;
                channels[i] = cb.Checked;
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            GetChannels();
            renderPanel.Invalidate();

        }
    }
}
