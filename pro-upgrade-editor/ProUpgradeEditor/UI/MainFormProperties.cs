using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;


namespace ProUpgradeEditor.UI
{

    partial class MainForm
    {
        SettingMgr settings;


        public string DefaultConFileLocation
        {
            get
            {
                return textBoxDefaultCONFileLocation.Text.AppendSlashIfMissing();
            }
            set
            {
                var dir = (value ?? "").AppendSlashIfMissing();
                dir.CreateFolderIfNotExists();
                textBoxDefaultCONFileLocation.Text = dir;
            }
        }
        public string DefaultMidiFileLocationPro
        {
            get
            {
                return textBoxDefaultMidiProFileLocation.Text.AppendSlashIfMissing();
            }
            set
            {
                var dir = (value ?? "").AppendSlashIfMissing();
                dir.CreateFolderIfNotExists();
                textBoxDefaultMidiProFileLocation.Text = dir;
            }
        }
        public string DefaultMidiFileLocationG5
        {
            get
            {
                return textBoxDefaultMidi5FileLocation.Text.AppendSlashIfMissing();
            }
            set
            {
                var dir = (value ?? "").AppendSlashIfMissing();
                dir.CreateFolderIfNotExists();
                textBoxDefaultMidi5FileLocation.Text = dir;
            }
        }

        SongCacheList SongList = null;

        UtilProperty[] Properties = UtilProperty.GetInitialProperties();


        private void CreatePropertiesGrid()
        {

            var itemHeight = 20;
            var itemPadding = 8;
            var itemWidth = panel6.Width / 2;
            var tbX = panel6.Width / 2;

            int totalHeight = 0;

            panel6.SuspendLayout();
            panel6.Controls.Clear();

            var items = Properties;
            int tabIndex = 0;

            var btreset = new Button();
            btreset.Text = "Reset All";
            btreset.Size = new Size(80, itemHeight);
            btreset.Location = new Point(panel6.Width - btreset.Width - itemPadding, 20);
            btreset.TabIndex = tabIndex++;
            panel6.Controls.Add(btreset);

            btreset.Click += new EventHandler(delegate(object ob, EventArgs eg)
            {
                foreach (Control p in panel6.Controls)
                {
                    if (p.Tag != null)
                    {
                        var i = p.Tag as UtilProperty;
                        if (i != null)
                        {
                            if (i.OriginalData != null)
                            {
                                i.Data = i.OriginalData;
                            }
                            OnUtilPropertyChange(i);
                        }
                    }
                }
                CreatePropertiesGrid();
                return;
            });

            for (int x = 0; x < items.Length; x++)
            {
                var item = items[x];
                int itemY = itemPadding + (itemHeight + itemPadding) * (x + 2);

                var lb = new Label();
                lb.Location = new Point(itemPadding, itemY);
                lb.Text = item.Description;
                lb.TextAlign = ContentAlignment.MiddleRight;
                lb.Size = new Size(itemWidth - 10, itemHeight);
                panel6.Controls.Add(lb);

                if (item.Type == PropertyType.Integer ||
                    item.Type == PropertyType.Double ||
                    item.Type == PropertyType.String)
                {

                    var tb = new TextBox();
                    tb.TextAlign = HorizontalAlignment.Right;
                    tb.Location = new Point(tbX + 5, itemY);
                    tb.Name = "tb" + x;
                    tb.Size = new System.Drawing.Size(itemWidth - 60, itemHeight);
                    tb.Text = item.Data.ToString();
                    tb.Tag = item;
                    tb.TabIndex = tabIndex++;
                    tb.LostFocus += new EventHandler(delegate(object obj, EventArgs egs)
                    {
                        var itm = tb.Tag as UtilProperty;
                        if (itm.Data.ToString() != tb.Text)
                        {
                            if (itm.Type == PropertyType.Integer)
                            {
                                int i = tb.Text.ToInt();
                                if (!i.IsNull())
                                {
                                    itm.Data = i;
                                }
                                else
                                {
                                    tb.Text = itm.Data.ToString();
                                }
                            }
                            else if (itm.Type == PropertyType.Double)
                            {
                                double i;
                                if (double.TryParse(tb.Text, out i))
                                {
                                    itm.Data = i;
                                }
                                else
                                {
                                    tb.Text = itm.Data.ToString();
                                }
                            }
                            else if (itm.Type == PropertyType.String)
                            {
                                itm.Data = tb.Text;
                            }

                            OnUtilPropertyChange(itm);
                        }
                    });
                    panel6.Controls.Add(tb);

                    var bt = new Button();
                    bt.Text = "X";
                    bt.Location = new Point(tb.Right + 5, itemY);
                    bt.Size = new Size(25, itemHeight);
                    bt.Tag = item;
                    bt.TabIndex = tabIndex++;
                    panel6.Controls.Add(bt);

                    bt.Click += new EventHandler(delegate(object ob, EventArgs eg)
                    {
                        var itm = tb.Tag as UtilProperty;
                        itm.Data = itm.OriginalData;
                        tb.Text = itm.Data.ToString();
                        OnUtilPropertyChange(itm);
                    });
                }
                if (item.Type == PropertyType.Bool)
                {

                    var tb = new CheckBox();
                    lb.Width += 100;
                    tb.Location = new Point(lb.Location.X + lb.Width + itemPadding, itemY);
                    tb.Name = "tb" + x;
                    tb.Size = new System.Drawing.Size(20, itemHeight);
                    tb.Checked = item.Data.ToString().ToBool();
                    tb.Tag = item;
                    tb.TabIndex = tabIndex++;
                    tb.CheckedChanged += new EventHandler(delegate(object obj, EventArgs egs)
                    {
                        var itm = tb.Tag as UtilProperty;
                        if (itm.Data.ToString().ToBool() != tb.Checked)
                        {
                            itm.Data = tb.Checked;

                            OnUtilPropertyChange(itm);
                        }
                    });
                    panel6.Controls.Add(tb);

                    var bt = new Button();
                    bt.Text = "X";
                    bt.Location = new Point(tb.Right + 5, itemY);
                    bt.Size = new Size(25, itemHeight);
                    bt.Tag = item;
                    bt.TabIndex = tabIndex++;
                    panel6.Controls.Add(bt);

                    bt.Click += new EventHandler(delegate(object ob, EventArgs eg)
                    {
                        var itm = tb.Tag as UtilProperty;
                        itm.Data = itm.OriginalData;

                        tb.Checked = itm.Data.ToString().ToBool();

                        OnUtilPropertyChange(itm);
                    });
                }
                if (item.Type == PropertyType.Pen ||
                    item.Type == PropertyType.Brush)
                {
                    int panelRight = -1;


                    var tb = new Panel();
                    tb.Location = new Point(tbX + 5, itemY);
                    tb.Name = "pn" + x;
                    tb.Size = new System.Drawing.Size(itemHeight * 2, itemHeight);

                    panelRight = tb.Location.X + tb.Size.Width;

                    tb.Tag = item;
                    tb.TabIndex = tabIndex++;
                    if (item.Type == PropertyType.Pen)
                    {
                        Pen p = (Pen)item.Data;
                        tb.BackColor = p.Color;
                    }
                    else
                    {
                        SolidBrush p = (SolidBrush)item.Data;
                        tb.BackColor = p.Color;
                    }
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.Click += new EventHandler(delegate(object ob, EventArgs eg)
                    {
                        var cd = new ColorDialog();
                        var itm = tb.Tag as UtilProperty;
                        if (itm.Type == PropertyType.Pen)
                        {
                            var ip = itm.Data as Pen;
                            cd.Color = ip.Color;
                            cd.AnyColor = true;
                            cd.AllowFullOpen = true;
                            if (cd.ShowDialog() == DialogResult.OK)
                            {
                                if (ip.Color != cd.Color)
                                {
                                    itm.Data = new Pen(cd.Color, ip.Width);
                                    tb.BackColor = cd.Color;
                                    OnUtilPropertyChange(itm);
                                }
                            }
                        }
                        else
                        {
                            var ip = itm.Data as SolidBrush;
                            cd.Color = ip.Color;
                            cd.AnyColor = true;
                            cd.AllowFullOpen = true;

                            if (cd.ShowDialog() == DialogResult.OK)
                            {
                                if (ip.Color != cd.Color)
                                {
                                    itm.Data = new SolidBrush(cd.Color);
                                    tb.BackColor = cd.Color;
                                    OnUtilPropertyChange(itm);
                                }
                            }
                        }
                    });

                    var tbw = new TextBox();
                    tbw.TextAlign = HorizontalAlignment.Right;
                    if (item.Type == PropertyType.Pen)
                    {
                        var lb2 = new Label();
                        lb2.Location = new Point(itemPadding + panelRight, itemY);
                        lb2.Text = "W:";
                        lb2.TextAlign = ContentAlignment.MiddleLeft;
                        lb2.AutoSize = true;
                        //lb2.Size = new Size(itemWidth - 10, itemHeight);
                        panel6.Controls.Add(lb2);


                        tbw.Location = new Point(lb2.Location.X + lb2.Width + itemPadding, itemY);
                        tbw.Name = "tb" + x;
                        tbw.Size = new System.Drawing.Size(30, itemHeight);
                        tbw.Text = ((Pen)item.Data).Width.ToString();
                        tbw.Tag = item;
                        tbw.TabIndex = tabIndex++;

                        panelRight = tbw.Location.X + tbw.Width;
                        tbw.LostFocus += new EventHandler(delegate(object obj, EventArgs egs)
                        {
                            var itm = tbw.Tag as UtilProperty;
                            Pen p = (Pen)itm.Data;
                            if (p.Width.ToString() != tbw.Text)
                            {
                                double i;
                                if (double.TryParse(tbw.Text, out i))
                                {
                                    itm.Data = new Pen(p.Color, (float)i);
                                }
                                else
                                {
                                    tbw.Text = p.Width.ToString();
                                }
                                OnUtilPropertyChange(itm);
                            }
                        });
                        panel6.Controls.Add(tbw);
                    }
                    var bt = new Button();
                    bt.Text = "X";
                    bt.Location = new Point(panelRight + 5, itemY);
                    bt.Size = new Size(25, itemHeight);
                    bt.Tag = item;
                    bt.TabIndex = tabIndex++;
                    panel6.Controls.Add(bt);
                    var eb = new TextBox();
                    eb.TextAlign = HorizontalAlignment.Right;
                    bt.Click += new EventHandler(delegate(object ob, EventArgs eg)
                    {
                        var itm = tb.Tag as UtilProperty;
                        itm.Data = itm.OriginalData;
                        if (itm.Type == PropertyType.Pen)
                        {
                            tbw.Text = ((Pen)itm.Data).Width.ToString();
                            tb.BackColor = ((Pen)itm.Data).Color;
                            eb.Text = ((Pen)itm.Data).Color.A.ToString();
                        }
                        else
                        {
                            tb.BackColor = ((SolidBrush)itm.Data).Color;
                            eb.Text = ((SolidBrush)itm.Data).Color.A.ToString();
                        }
                        OnUtilPropertyChange(itm);

                    });

                    {

                        var itmp = tb.Tag as UtilProperty;
                        if (itmp.Type == PropertyType.Pen)
                        {
                            eb.Text = ((Pen)itmp.Data).Color.A.ToString();
                        }
                        else
                        {
                            eb.Text = ((SolidBrush)itmp.Data).Color.A.ToString();
                        }

                        eb.Location = new Point(bt.Right + 5, itemY);
                        eb.Size = new Size(25, itemHeight);
                        eb.Tag = itmp;
                        eb.TabIndex = tabIndex++;
                        panel6.Controls.Add(eb);

                        eb.LostFocus += new EventHandler(delegate(object ob, EventArgs eg)
                        {
                            var itm = eb.Tag as UtilProperty;
                            bool changed = false;

                            if (itm.Type == PropertyType.Pen)
                            {
                                Pen p = itm.Data as Pen;
                                int lastVal = p.Color.A;
                                int newVal = eb.Text.ToInt(255);
                                if (lastVal != newVal)
                                {
                                    changed = true;
                                    p.Color = Color.FromArgb(newVal, p.Color);
                                }
                            }
                            else
                            {
                                SolidBrush p = itm.Data as SolidBrush;
                                int lastVal = p.Color.A;
                                int newVal = eb.Text.ToInt(255);
                                if (lastVal != newVal)
                                {
                                    changed = true;
                                    p.Color = Color.FromArgb(newVal, p.Color);
                                }
                            }
                            if (changed)
                            {
                                OnUtilPropertyChange(itm);
                            }

                            EditorPro.Invalidate();
                            EditorG5.Invalidate();

                        });
                    }

                    panel6.Controls.Add(tb);
                }


                totalHeight = itemY + itemHeight + itemPadding * 2;
            }
            panel6.Height = totalHeight + 80;


            int overFlow = totalHeight + itemHeight * 10 - panel7.Height;


            this.vScrollBar1.Value = 0;
            this.vScrollBar1.Maximum = overFlow;
            this.vScrollBar1.SmallChange = itemHeight / 4;
            this.vScrollBar1.LargeChange = itemHeight * 10;
            int pTop = panel6.Location.Y;

            var topLeft = panel6.PointToClient(panel7.PointToScreen(panel7.Location));

            panel6.ResumeLayout();

            this.vScrollBar1.ValueChanged += new EventHandler(delegate(object o, EventArgs se)
            {
                panel6.Location = new Point(panel6.Location.X, pTop - vScrollBar1.Value);
            });
        }



        void OnUtilPropertyChange(UtilProperty p)
        {
            switch (p.Description)
            {
                case "Clear Chord Names":
                    {
                        Utility.ClearChordNames = (bool)p.Data;
                    }
                    break;
                case "108 Generation Enabled": { Utility.HandPositionGenerationEnabled = (bool)p.Data; }
                    break;
                case "108 Marker Start Offset": { Utility.HandPositionMarkerStartOffset = (int)p.Data; }
                    break;
                case "108 Marker End Offset": { Utility.HandPositionMarkerEndOffset = (int)p.Data; }
                    break;
                case "108 Marker Max Fret": { Utility.HandPositionMarkerMaxFret = (int)p.Data; }
                    break;
                case "108 Marker Min Fret": { Utility.HandPositionMarkerMinFret = (int)p.Data; }
                    break;
                case "108 Marker By Difficulty": { Utility.HandPositionMarkerByDifficulty = (bool)p.Data; }
                    break;
                case "108 Marker First Begin": { Utility.HandPositionMarkerFirstBeginOffset = (int)p.Data; }
                    break;
                case "108 Marker First End": { Utility.HandPositionMarkerFirstEndOffset = (int)p.Data; }
                    break;
                case "Enable Render MP3 Wave": { EditorPro.EnableRenderMP3Wave = (bool)p.Data; }
                    break;
                case "Background Brush": { Utility.BackgroundBrush = (SolidBrush)p.Data; }
                    break;

                case "G5 Green Note": { Utility.G5GreenNoteBrush = (SolidBrush)p.Data; }
                    break; // Color.Green),
                case "G5 Red Note": { Utility.G5RedNoteBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(255, 120, 120)),
                case "G5 Yellow Note": { Utility.G5YellowNoteBrush = (SolidBrush)p.Data; }
                    break; // Color.Yellow),
                case "G5 Blue Note": { Utility.G5BlueNoteBrush = (SolidBrush)p.Data; }
                    break; // Color.Blue),
                case "G5 Orange Note": { Utility.G5OragneNoteBrush = (SolidBrush)p.Data; }
                    break; // Color.Orange),

                case "G5 Line Pen": { Utility.linePen = (Pen)p.Data; }
                    break; // Color.Black),
                case "Pro Line Pen": { Utility.linePen22 = (Pen)p.Data; }
                    break; // Color.Red),
                case "Selected Pen": { Utility.selectedPen = (Pen)p.Data; }
                    break; // Color.Red),
                case "Beat Pen": { Utility.beatPen = (Pen)p.Data; }
                    break; // Color.FromArgb(180, 180, 180)),

                case "Slide": { Utility.slidePen = (Pen)p.Data; }
                    break; // Color.Green),
                case "Hammeron": { Utility.hammerOnPen = (Pen)p.Data; }
                    break; // Color.Blue),
                case "Note Bounds": { Utility.noteBoundPen = (Pen)p.Data; }
                    break; // Color.Black),
                case "Grid Snap Pointer Color": { Utility.gridSnapCursorBrush = (SolidBrush)p.Data; }
                    break;
                case "Grid Snap Pointer Size": { Utility.gridSnapCursorSize = (int)p.Data; }
                    break;

                //case "Note Height": { Utility.NoteHeight = (int)p.Data; } break; //"12"),

                case "Note Text Color": { Utility.fretBrush = (SolidBrush)p.Data; }
                    break; // Color.Black),

                case "Note Background Shadow": { Utility.noteBGBrushShadow = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(100, 100, 100)),
                case "Selected Note Background": { Utility.noteBGBrushSel = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(255, 0, 0)),
                case "Single String Tremelo": { Utility.noteSingleStringTremeloBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(120, 240, 100)),
                case "Multi String Tremelo": { Utility.noteMultiStringTremeloBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(120, 240, 190)),

                case "Note Background": { Utility.noteBGBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(252, 183, 180)),
                case "Note Tap": { Utility.noteTapBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(242, 223, 220)),
                case "Note X": { Utility.noteXBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(152, 152, 247)),
                case "Note Arpeggio": { Utility.noteArpeggioBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(159, 253, 222)),
                case "Note Powerup": { Utility.notePowerupBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(242, 253, 200)),
                case "Note Solo": { Utility.noteSoloBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(202, 203, 250)),
                case "Note Big Rock Ending": { Utility.noteBREBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(152, 203, 250)),
                case "Note Strum": { Utility.noteStrumBrush = (SolidBrush)p.Data; }
                    break; // Color.FromArgb(180, 180, 255)),

                case "Bar Pen": { Utility.barPen = (Pen)p.Data; }
                    break; //1"),
                case "Draw Beat": { Utility.DrawBeat = (int)p.Data; }
                    break; //1"),
                case "Rectangle Select Pen": { Utility.rectSelectionPen = (Pen)p.Data; }
                    break;

                case "X Note Text": { Utility.XNoteText = (string)p.Data; }
                    break; //x"),
                case "X Note Text Y Offset": { Utility.XNoteTextYOffset = (int)p.Data; }
                    break; //-2"),
                case "X Note Text X Offset": { Utility.XNoteTextXOffset = (int)p.Data; }
                    break; //-2"),
                case "Note Text Y Offset": { Utility.NoteTextYOffset = (int)p.Data; }
                    break; //-1"),
                case "Note Text X Offset": { Utility.NoteTextXOffset = (int)p.Data; }
                    break; //1"),


                case "Arpeggio Helper Prefix": { Utility.ArpeggioHelperPrefix = (string)p.Data; }
                    break; //("),
                case "Arpeggio Helper Suffix": { Utility.ArpeggioHelperSuffix = (string)p.Data; }
                    break; //)"),

                case "Max Backups": { Utility.MaxBackups = (int)p.Data; }
                    break; //20"),

                case "Default CON File Extension": { Utility.DefaultCONFileExtension = (string)p.Data; }
                    break; // "pro"),
                case "Default PRO File Extension": { Utility.DefaultPROFileExtension = (string)p.Data; }
                    break; // "_pro.mid"),

                case "Stored Chord Prefix": { Utility.StoredChordPrefix = (string)p.Data; }
                    break; // "["),
                case "Stored Chord Note Separator": { Utility.StoredChordNoteSeparator = (string)p.Data; }
                    break; // " - "),
                case "Stored Chord Empty Note": { Utility.StoredChordEmptyNote = (string)p.Data; }
                    break; // "_"),
                case "Stored Chord Suffix": { Utility.StoredChordSuffix = (string)p.Data; }
                    break; // "]  "),
                case "Stored Chord Slide": { Utility.StoredChordSlide = (string)p.Data; }
                    break; // "[S]"),
                case "Stored Chord Reverse": { Utility.StoredChordReverse = (string)p.Data; }
                    break; // "[R]"),
                case "Stored Chord Hammeron": { Utility.StoredChordHammeron = (string)p.Data; }
                    break; // "[H]"),
                case "Stored Chord Tap": { Utility.StoredChordTap = (string)p.Data; }
                    break; // "[T]"),
                case "Stored Chord X Note": { Utility.StoredChordXNote = (string)p.Data; }
                    break; // "[X]"),
                case "Stored Chord Strum Low": { Utility.StoredChordStrumLow = (string)p.Data; }
                    break; // "[SL]"),
                case "Stored Chord Strum Med": { Utility.StoredChordStrumMed = (string)p.Data; }
                    break; // "[SM]"),
                case "Stored Chord Strum High": { Utility.StoredChordStrumHigh = (string)p.Data; }
                    break; // "[SH]"),

                case "Dummy Tempo": { Utility.DummyTempo = (int)p.Data; }
                    break; //, "405405"),



                case "Trainer Color": { Utility.TrainerBrush = (SolidBrush)p.Data; }
                    break;
                case "Sel. Trainer Color": { Utility.SelectedTrainerBrush = (SolidBrush)p.Data; }
                    break;
                case "Text Event Color": { Utility.TextEventBrush = (SolidBrush)p.Data; }
                    break;
                case "Sel. Text Event Color": { Utility.SelectedTextEventBrush = (SolidBrush)p.Data; }
                    break;

                case "Text Event Begin": { Utility.TextEventBeginTag = (string)p.Data; }
                    break;
                case "Text Event End": { Utility.TextEventEndTag = (string)p.Data; }
                    break;
                case "ProG Trainer Begin": { Utility.SongTrainerBeginPGText = (string)p.Data; }
                    break;
                case "ProB Trainer Begin": { Utility.SongTrainerBeginPBText = (string)p.Data; }
                    break;
                case "ProG Trainer Norm": { Utility.SongTrainerNormPGText = (string)p.Data; }
                    break;
                case "ProB Trainer Norm": { Utility.SongTrainerNormPBText = (string)p.Data; }
                    break;
                case "ProG Trainer End": { Utility.SongTrainerEndPGText = (string)p.Data; }
                    break;
                case "ProB Trainer End": { Utility.SongTrainerEndPBText = (string)p.Data; }
                    break;

                case "ProG Trainer": { Utility.SongTrainerPGText = (string)p.Data; }
                    break;
                case "ProB Trainer": { Utility.SongTrainerPBText = (string)p.Data; }
                    break;
                case "Trainer Norm Offset": { Utility.SongTrainerNormOffset = (double)p.Data; }
                    break;
                case "View Lyrics in G5 Editor": { trackEditorG5.ViewLyrics = (bool)p.Data; trackEditorG5.Invalidate(); }
                    break;
                case "Show Measure Numbers": { Utility.ShowMeasureNumbers = (bool)p.Data; EditorPro.Invalidate(); }
                    break;
                case "Show Tempos": { Utility.ShowTempos = (bool)p.Data; EditorPro.Invalidate(); }
                    break;
                case "Show Time Signatures": { Utility.ShowTimeSignatures = (bool)p.Data; EditorPro.Invalidate(); }
                    break;
                case "Modify WebTab Scale": { Utility.ModifyWebTabScale = (bool)p.Data; EditorPro.Invalidate(); }
                    break;

                default:
                    {
                        if (p.OnUpdate != null)
                        {
                            if (!p.OnUpdate(p))
                            {
                                p.Data = p.OriginalData;
                                EditorPro.Invalidate();
                            }
                        }
                        else
                        {
                            switch (p.Type)
                            {
                                case PropertyType.Integer:
                                    settings.SetValue("Util_" + p.Description, (int)p.Data);
                                    break;
                                case PropertyType.Bool:
                                    settings.SetValue("Util_" + p.Description, (bool)p.Data);
                                    break;
                                case PropertyType.String:
                                    settings.SetValue("Util_" + p.Description, (string)p.Data);
                                    break;
                            }
                        }
                    }
                    break;
            }
            EditorG5.Invalidate();
            EditorPro.Invalidate();
        }

        int GetNextSongID()
        {
            int maxSongID = 1;
            foreach (var s in SongList)
            {
                if (s.CacheSongID > maxSongID)
                {
                    maxSongID = s.CacheSongID;
                }
            }

            settings.SetValue("SC_NextID", maxSongID + 1);

            return maxSongID + 1;
        }





        private void LoadSettingConfiguration()
        {
            if (settings == null)
            {
                settings = new SettingMgr();
            }
            settings.LoadSettings();

            checkBoxAutoSelectNext.Checked = settings.GetValueBool("checkBoxAutoSelectNext", true);
            checkBatchOpenWhenCompleted.Checked = settings.GetValueBool("checkBatchOpenWhenCompleted", true);
            checkBoxBatchCopyUSB.Checked = settings.GetValueBool("checkBoxBatchCopyUSB", false);
            checkBatchCopyTextEvents.Checked = settings.GetValueBool("checkBatchCopyTextEvents", true);
            checkBatchGenerateTrainersIfNone.Checked = settings.GetValueBool("checkBatchGenerateTrainersIfNone", true);
            
            textBoxUSBFolder.Text = settings.GetValue("textBoxUSBFolder");

            DefaultConFileLocation = settings.GetValue("textBoxDefaultCONFileLocation");
            DefaultMidiFileLocationG5 = settings.GetValue("textBoxDefaultMidi5FileLocation");
            DefaultMidiFileLocationPro = settings.GetValue("textBoxDefaultMidiProFileLocation");
            checkBoxShow108.Checked = settings.GetValueBool("checkBoxShow108", false);

            textBoxZoom.Text = settings.GetValue("textBoxZoom", textBoxZoom.Text);
            Utility.timeScalarZoomSpeed = settings.GetValue("timeScalarZoomSpeed", Utility.timeScalarZoomSpeed.ToStringEx()).ToDouble(10);

            checkUseDefaultFolders.Checked = settings.GetValueBool("useDefaultFolders", true);
            checkBoxInitSelectedTrackOnly.Checked = settings.GetValueBool("checkBoxInitSelectedTrackOnly", false);
            checkBoxInitSelectedDifficultyOnly.Checked = settings.GetValueBool("checkBoxInitSelectedDifficultyOnly", false);

            checkKeepSelection.Checked = settings.GetValueBool("checkKeepSelection", true);
            checkBoxClearAfterNote.Checked = settings.GetValueBool("checkBoxClearAfterNote", false);
            checkRealtimeNotes.Checked = settings.GetValueBool("checkRealtimeNotes", false);
            checkTwoNotePowerChord.Checked = settings.GetValueBool("checkTwoNotePowerChord", false);
            checkThreeNotePowerChord.Checked = settings.GetValueBool("checkThreeNotePowerChord", false);
            checkChordMode.Checked = settings.GetValueBool("checkChordMode", false);
            checkScrollToSelection.Checked = settings.GetValueBool("checkScrollToSelection", true);
            checkKBQuickEdit.Checked = settings.GetValueBool("checkKBQuickEdit", true);
            checkIndentBString.Checked = settings.GetValueBool("checkIndentBString", false);
            checkBoxSearchByNoteType.Checked = settings.GetValueBool("checkBoxSearchByNoteType", true);
            checkBoxSearchByNoteStrum.Checked = settings.GetValueBool("checkBoxSearchByNoteStrum", true);
            checkBoxSearchByNoteFret.Checked = settings.GetValueBool("checkBoxSearchByNoteFret", true);
            checkBoxKeepLengths.Checked = settings.GetValueBool("checkBoxSetLengths5", true);
            checkMatchBeat.Checked = settings.GetValueBool("checkMatchBeat", true);
            checkBoxMatchLengths.Checked = settings.GetValueBool("checkBoxMatchLengths", true);
            checkBoxMatchLength6.Checked = settings.GetValueBool("checkBoxMatchLength6", true);
            checkBoxMatchSpacing.Checked = settings.GetValueBool("checkBoxMatchSpacing", true);
            checkBoxMatchForwardOnly.Checked = settings.GetValueBool("checkBoxMatchForwardOnly", false);
            checkBoxFirstMatchOnly.Checked = settings.GetValueBool("checkBoxFirstMatchOnly", false);
            checkBoxShowMidiChannelEdit.Checked = settings.GetValueBool("checkBoxShowMidiChannelEdit", true);

            textBoxTempoNumerator.Text = settings.GetValue("textBoxTempoNumerator", "4");
            textBoxTempoDenominator.Text = settings.GetValue("textBoxTempoDenominator", "4");

            checkBoxUseCurrentChord.Checked = settings.GetValueBool("checkUseCurrentChord", true);
            checkBoxAllowOverwriteChord.Checked = settings.GetValueBool("checkAllowOverwriteChord", true);
            textBoxPlaceNoteFret.Text = settings.GetValue("textBoxPlaceNoteFret", "0");

            checkBoxGridSnap.Checked = settings.GetValueBool("checkBoxGridSnap", true);
            checkSnapToCloseG5.Checked = settings.GetValueBool("checkSnapToCloseG5", true);

            checkBoxCreateArpeggioHelperNotes.Checked = settings.GetValueBool("checkBoxCreateArpeggioHelperNotes", true);

            checkViewNotesGridPro.Checked = settings.GetValueBool("checkViewNotesGrid", true);
            checkViewNotesGrid5Button.Checked = settings.GetValueBool("checkViewNotesGrid5", true);
            textBoxCopyAllCONFolder.Text = settings.GetValue("textBoxCopyAllCONFolder");
            textBoxMinimumNoteWidth.Text = settings.GetValue("textBoxMinimumNoteWidth", "0");
            textBoxCopyAllProFolder.Text = settings.GetValue("textBoxCopyAllProFolder");
            textBoxCopyAllG5MidiFolder.Text = settings.GetValue("textBoxCopyAllG5MidiFolder");
            checkBoxMidiPlaybackScroll.Checked = settings.GetValueBool("checkBoxMidiPlaybackScroll", true);
            CheckMinimumNoteWidth();
            SetGridScalar(settings.GetValue("gridScalar", "0.25"));
            textBoxScrollToSelectionOffset.Text = settings.GetValue("textBoxScrollToSelectionOffset", Utility.ScollToSelectionOffset.ToString());
            checkBoxMultiSelectionSongList.Checked = settings.GetValueBool("checkBoxMultiSelectionSongList", false);
            checkBoxSkipGenIfEasyNotes.Checked = settings.GetValueBool("checkBoxSkipGenIfEasyNotes", false);
            checkGenDiffCopyGuitarToBass.Checked = settings.GetValueBool("checkGenDiffCopyGuitarToBass", true);
            checkBoxLoadLastSongStartup.Checked = settings.GetValueBool("checkBoxLoadLastSongStartup", true);
            resetTime = settings.GetValueInt("textClearHoldBox", 1);
            textClearHoldBox.Text = resetTime.ToString();
            checkBoxEnableMidiInput.Checked = settings.GetValueBool("checkBoxEnableMidiInput", false);
            checkBoxEnableClearTimer.Checked = settings.GetValueBool("checkBoxEnableClearTimer", true);
            checkBoxPlayMidiStrum.Checked = settings.GetValueBool("checkBoxPlayMidiStrum", false);
            checkBoxClearIfNoFrets.Checked = settings.GetValueBool("checkBoxClearIfNoFrets", true);
            checkBoxChordStrum.Checked = settings.GetValueBool("checkBoxChordStrum", false);
            checkBoxMidiInputStartup.Checked = settings.GetValueBool("checkBoxMidiInputStartup", true);
            checkView5Button.Checked = settings.GetValueBool("checkView5Button", true);

            Utility.NoteCloseWidth = settings.GetValueInt("textBoxNoteCloseDist", 8);
            textBoxNoteCloseDist.Text = Utility.NoteCloseWidth.ToString();
            checkBoxBatchCheckCON.Checked = settings.GetValueBool("checkBoxBatchCheckCON", true);
            checkBoxBatchGenerateDifficulties.Checked = settings.GetValueBool("checkBoxBatchGenerateDifficulties", true);
            checkBoxBatchGuitarBassCopy.Checked = settings.GetValueBool("checkBoxBatchGuitarBassCopy", true);
            checkBoxBatchRebuildCON.Checked = settings.GetValueBool("checkBoxBatchRebuildCON", true);

            checkBoxMatchAllFrets.Checked = settings.GetValueBool("checkBoxMatchAllFrets", true);
            checkBoxCompressAllInDefaultCONFolder.Checked = settings.GetValueBool("checkBoxCompressAllInDefaultCONFolder", true);

            textBoxCompressAllZipFile.Text = settings.GetValue("textBoxCompressAllZipFile");
            checkBoxRenderMouseSnap.Checked = settings.GetValueBool("checkBoxRenderMouseSnap", false);

            checkBoxSnapToCloseNotes.Checked = settings.GetValueBool("checkBoxSnapToCloseNotes", true);

            Utility.NoteSnapDistance = settings.GetValueInt("textBoxNoteSnapDistance", 4);
            Utility.GridSnapDistance = settings.GetValueInt("textBoxGridSnapDistance", 4);
            textBoxNoteSnapDistance.Text = Utility.NoteSnapDistance.ToString();
            textBoxGridSnapDistance.Text = Utility.GridSnapDistance.ToString();


            RefreshMidiInputList();
            CheckMidiInputVisibility();
            UpdateProperties(true);

            comboNoteEditorCopyPatternPreset.Items.Clear();
            comboNoteEditorCopyPatternPreset.SelectedIndex = -1;
            foreach (XmlNode node in XMLUtil.GetNodeList(settings.XMLRoot, "searchPatterns/searchPattern"))
            {
                var preset = new CopyPatternPreset()
                {
                    ID = XMLUtil.GetNodeValueInt(node, "@ID"),
                    Name = XMLUtil.GetNodeValue(node, "@Name") ?? "",
                    ForwardOnly = XMLUtil.GetNodeValueBool(node, "@ForwardOnly"),
                    MatchLengths5 = XMLUtil.GetNodeValueBool(node, "@MatchLengths5"),
                    MatchLengths6 = XMLUtil.GetNodeValueBool(node, "@MatchLengths6"),
                    MatchSpacing = XMLUtil.GetNodeValueBool(node, "@MatchSpacing"),
                    MatchBeat = XMLUtil.GetNodeValueBool(node, "@MatchBeat"),
                    KeepLengths = XMLUtil.GetNodeValueBool(node, "@KeepLengths"),
                    FirstMatchOnly = XMLUtil.GetNodeValueBool(node, "@FirstMatchOnly"),
                    RemoveExisting = XMLUtil.GetNodeValueBool(node, "@RemoveExisting", true),

                };

                comboNoteEditorCopyPatternPreset.Items.Add(preset);
            }

            if (comboNoteEditorCopyPatternPreset.Items.Count > 0)
            {
                comboNoteEditorCopyPatternPreset.SelectedIndex = 0;
            }


            SongList = new SongCacheList(listBoxSongLibrary);


            var snglist = XMLUtil.GetNodeList(settings.XMLRoot, "docLib/song");
            if (snglist != null)
            {
                var lst = new List<SongCacheItem>();

                foreach (XmlNode song in snglist)
                {

                    var sc = new SongCacheItem();
                    sc.SongName = XMLUtil.GetNodeValue(song, "@name");
                    sc.G5FileName = XMLUtil.GetNodeValue(song, "@G5FileName");
                    sc.G6FileName = XMLUtil.GetNodeValue(song, "@G6FileName");
                    sc.G6ConFile = XMLUtil.GetNodeValue(song, "@G6ConFile");
                    sc.Description = XMLUtil.GetNodeValue(song, "@Description");

                    sc.HasBass = XMLUtil.GetNodeValueBool(song, "@HasBass");
                    sc.HasGuitar = XMLUtil.GetNodeValueBool(song, "@HasGuitar");
                    sc.CopyGuitarToBass = XMLUtil.GetNodeValueBool(song, "@CopyGuitarToBass");
                    sc.IsComplete = XMLUtil.GetNodeValueBool(song, "@IsComplete");
                    sc.IsFinalized = XMLUtil.GetNodeValueBool(song, "@IsFinalized");

                    sc.GuitarTuning[0] = XMLUtil.GetNodeValue(song, "@GuitarLowE", "0");
                    sc.GuitarTuning[1] = XMLUtil.GetNodeValue(song, "@GuitarA", "0");
                    sc.GuitarTuning[2] = XMLUtil.GetNodeValue(song, "@GuitarD", "0");
                    sc.GuitarTuning[3] = XMLUtil.GetNodeValue(song, "@GuitarG", "0");
                    sc.GuitarTuning[4] = XMLUtil.GetNodeValue(song, "@GuitarB", "0");
                    sc.GuitarTuning[5] = XMLUtil.GetNodeValue(song, "@GuitarHighE", "0");

                    sc.BassTuning[0] = XMLUtil.GetNodeValue(song, "@BassLowE", "0");
                    sc.BassTuning[1] = XMLUtil.GetNodeValue(song, "@BassA", "0");
                    sc.BassTuning[2] = XMLUtil.GetNodeValue(song, "@BassD", "0");
                    sc.BassTuning[3] = XMLUtil.GetNodeValue(song, "@BassG", "0");
                    sc.BassTuning[4] = XMLUtil.GetNodeValue(song, "@BassB", "0");
                    sc.BassTuning[5] = XMLUtil.GetNodeValue(song, "@BassHighE", "0");

                    sc.DTABassDifficulty = XMLUtil.GetNodeValueInt(song, "@DTABassDifficulty", 0);
                    sc.DTAGuitarDifficulty = XMLUtil.GetNodeValueInt(song, "@DTAGuitarDifficulty", 0);
                    sc.DTASongID = XMLUtil.GetNodeValue(song, "@DTASongID");
                    sc.DTASongShortName = XMLUtil.GetNodeValue(song, "@DTASongShortName");


                    sc.SongMP3Location = XMLUtil.GetNodeValue(song, "@SongMP3Location", "");
                    sc.SongMP3PlaybackOffset = XMLUtil.GetNodeValue(song, "@SongMP3PlaybackOffset", "").ToInt();
                    sc.EnableSongMP3Playback = XMLUtil.GetNodeValueBool(song, "@EnableSongMP3Playback", false);

                    sc.EnableSongMidiPlayback = XMLUtil.GetNodeValueBool(song, "@EnableSongMidiPlayback", true);
                    sc.SongMP3PlaybackVolume = XMLUtil.GetNodeValueInt(song, "@SongMP3PlaybackVolume", 100);
                    sc.SongMidiPlaybackVolume = XMLUtil.GetNodeValueInt(song, "@SongMidiPlaybackVolume", 100);


                    sc.AutoGenGuitarHard = XMLUtil.GetNodeValueBool(song, "@AutoGenGuitarHard", true);
                    sc.AutoGenGuitarMedium = XMLUtil.GetNodeValueBool(song, "@AutoGenGuitarMedium", true);
                    sc.AutoGenGuitarEasy = XMLUtil.GetNodeValueBool(song, "@AutoGenGuitarEasy", true);


                    sc.AutoGenBassHard = XMLUtil.GetNodeValueBool(song, "@AutoGenBassHard", true);
                    sc.AutoGenBassMedium = XMLUtil.GetNodeValueBool(song, "@AutoGenBassMedium", true);
                    sc.AutoGenBassEasy = XMLUtil.GetNodeValueBool(song, "@AutoGenBassEasy", true);


                    var sid = XMLUtil.GetNodeValue(song, "@CacheSongID");
                    if (!string.IsNullOrEmpty(sid))
                    {
                        int songid = sid.ToInt();
                        if (!songid.IsNull())
                        {
                            sc.CacheSongID = songid;
                        }
                        else
                        {
                            sc.CacheSongID = GetNextSongID();
                        }
                    }

                    sc.IsDirty = false;

                    try
                    {
                        if (string.IsNullOrEmpty(sc.DTASongID) && !string.IsNullOrEmpty(sc.G5FileName))
                        {
                            FindDTAInformation(sc);
                        }
                    }
                    catch { }



                    lst.Add(sc);

                }
                lst.Sort();
                SongList.AddRange(lst);
                SongList.PopulateList();
            }
            CheckNotesGridSelection();
            CheckNoteChannelVisibility();

            PostLoadSettings();
        }

        private void UpdateProperties(bool loading)
        {

            foreach (UtilProperty prop in Properties)
            {
                if (prop.Type == PropertyType.Brush)
                {
                    SolidBrush p = prop.Data as SolidBrush;
                    var v = settings.GetValue("Util_" + prop.Description, p.Color);
                    if (loading || p.Color != v)
                    {
                        prop.Data = new SolidBrush(v);
                        OnUtilPropertyChange(prop);
                    }
                }
                else if (prop.Type == PropertyType.Pen)
                {
                    Pen p = prop.Data as Pen;
                    var v = settings.GetValue("Util_" + prop.Description, p.Color);
                    if (loading || p.Color != v)
                    {
                        prop.Data = new Pen(v, p.Width);
                        OnUtilPropertyChange(prop);
                    }
                }
                else if (prop.Type == PropertyType.Double)
                {
                    double d = (double)prop.Data;
                    var v = settings.GetValue("Util_" + prop.Description, d.ToString());
                    if (loading || d.ToString() != v)
                    {
                        prop.Data = (v).ToDouble(0);
                        OnUtilPropertyChange(prop);
                    }
                }
                else if (prop.Type == PropertyType.Integer)
                {
                    int d = (int)prop.Data;
                    var v = settings.GetValue("Util_" + prop.Description, d.ToString());
                    if (loading || d.ToString() != v)
                    {
                        prop.Data = (v).ToInt(0);
                        OnUtilPropertyChange(prop);
                    }
                }
                else if (prop.Type == PropertyType.Bool)
                {
                    var b = (bool)prop.Data;
                    var v = settings.GetValue("Util_" + prop.Description, b.ToString());
                    if (loading || b.ToString() != v)
                    {
                        prop.Data = v.ToBool();
                        OnUtilPropertyChange(prop);
                    }
                }
                else if (prop.Type == PropertyType.String)
                {
                    string d = (string)prop.Data;
                    var v = settings.GetValue("Util_" + prop.Description, d);
                    if (loading || d != v)
                    {
                        prop.Data = v;
                        OnUtilPropertyChange(prop);
                    }
                }
            }
        }

        private void RefreshMidiInputList()
        {
            comboBoxMidiInput.Items.Clear();
            string midiInput = settings.GetValue("comboBoxMidiInput");
            try
            {
                for (int x = 0; x < InputDevice.DeviceCount; x++)
                {
                    try
                    {
                        var caps = InputDevice.GetDeviceCapabilities(x);
                        comboBoxMidiInput.Items.Add(new MidiInputListItem() { index = x, Caps = caps });
                    }
                    catch { }
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(midiInput))
            {
                for (int x = 0; x < comboBoxMidiInput.Items.Count; x++)
                {
                    if (string.Compare(comboBoxMidiInput.Items[x].ToString(),
                        midiInput, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        comboBoxMidiInput.SelectedIndex = x;
                        break;
                    }
                }
            }
            else
            {
                if (comboBoxMidiInput.Items.Count > 0)
                {
                    comboBoxMidiInput.SelectedIndex = 0;
                }
            }
        }


        private void SaveSettingConfiguration()
        {

            settings.SetValue("checkBoxAutoSelectNext", checkBoxAutoSelectNext.Checked);
            settings.SetValue("checkBatchOpenWhenCompleted", checkBatchOpenWhenCompleted.Checked);
            settings.SetValue("lastg5FileName", FileNameG5);
            settings.SetValue("checkBoxBatchCopyUSB", checkBoxBatchCopyUSB.Checked);
            settings.SetValue("checkBatchGenerateTrainersIfNone", checkBatchGenerateTrainersIfNone.Checked);
            settings.SetValue("checkBatchCopyTextEvents", checkBatchCopyTextEvents.Checked);
            settings.SetValue("lastg6FileName", FileNamePro);

            settings.SetValue("textBoxDefaultCONFileLocation", DefaultConFileLocation);
            settings.SetValue("textBoxDefaultMidi5FileLocation", DefaultMidiFileLocationG5);
            settings.SetValue("textBoxDefaultMidiProFileLocation", DefaultMidiFileLocationPro);

            settings.SetValue("checkBoxShow108", checkBoxShow108.Checked);

            settings.SetValue("useDefaultFolders", checkUseDefaultFolders.Checked);

            settings.SetValue("checkBoxInitSelectedTrackOnly", checkBoxInitSelectedTrackOnly.Checked);
            settings.SetValue("checkBoxInitSelectedDifficultyOnly", checkBoxInitSelectedDifficultyOnly.Checked);

            settings.SetValue("textBoxZoom", textBoxZoom.Text);
            settings.SetValue("timeScalarZoomSpeed", Utility.timeScalarZoomSpeed.ToStringEx());

            settings.SetValue("checkKeepSelection", checkKeepSelection.Checked);
            settings.SetValue("checkBoxClearAfterNote", checkBoxClearAfterNote.Checked);
            settings.SetValue("checkRealtimeNotes", checkRealtimeNotes.Checked);
            settings.SetValue("checkTwoNotePowerChord", checkTwoNotePowerChord.Checked);
            settings.SetValue("checkThreeNotePowerChord", checkThreeNotePowerChord.Checked);
            settings.SetValue("checkChordMode", checkChordMode.Checked);
            settings.SetValue("checkScrollToSelection", checkScrollToSelection.Checked);
            settings.SetValue("checkKBQuickEdit", checkKBQuickEdit.Checked);
            settings.SetValue("checkIndentBString", checkIndentBString.Checked);

            settings.SetValue("checkBoxGridSnap", checkBoxGridSnap.Checked);
            settings.SetValue("checkSnapToCloseG5", checkSnapToCloseG5.Checked);

            settings.SetValue("checkBoxCreateArpeggioHelperNotes", checkBoxCreateArpeggioHelperNotes.Checked);
            settings.SetValue("checkViewNotesGrid", checkViewNotesGridPro.Checked);
            settings.SetValue("checkViewNotesGrid5", checkViewNotesGrid5Button.Checked);
            settings.SetValue("textBoxCopyAllCONFolder", textBoxCopyAllCONFolder.Text);
            settings.SetValue("textBoxCopyAllProFolder", textBoxCopyAllProFolder.Text);
            settings.SetValue("textBoxCopyAllG5MidiFolder", textBoxCopyAllG5MidiFolder.Text);

            settings.SetValue("checkBoxMidiPlaybackScroll", checkBoxMidiPlaybackScroll.Checked);

            settings.SetValue("textBoxMinimumNoteWidth", textBoxMinimumNoteWidth.Text);
            settings.SetValue("checkBoxSetLengths5", checkBoxKeepLengths.Checked);
            settings.SetValue("checkMatchBeat", checkMatchBeat.Checked);
            settings.SetValue("checkBoxMatchLengths", checkBoxMatchLengths.Checked);
            settings.SetValue("checkBoxMatchLength6", checkBoxMatchLength6.Checked);
            settings.SetValue("checkBoxMatchSpacing", checkBoxMatchSpacing.Checked);
            settings.SetValue("checkBoxMatchForwardOnly", checkBoxMatchForwardOnly.Checked);

            settings.SetValue("checkBoxFirstMatchOnly", checkBoxFirstMatchOnly.Checked);
            settings.SetValue("gridScalar", GetGridScalar().ToString());

            settings.SetValue("checkBoxSearchByNoteType", checkBoxSearchByNoteType.Checked);
            settings.SetValue("checkBoxSearchByNoteStrum", checkBoxSearchByNoteStrum.Checked);
            settings.SetValue("checkBoxSearchByNoteFret", checkBoxSearchByNoteFret.Checked);

            settings.SetValue("checkUseCurrentChord", checkBoxUseCurrentChord.Checked);
            settings.SetValue("checkAllowOverwriteChord", checkBoxAllowOverwriteChord.Checked);
            settings.SetValue("textBoxPlaceNoteFret", textBoxPlaceNoteFret.Text);

            settings.SetValue("checkBoxShowMidiChannelEdit", checkBoxShowMidiChannelEdit.Checked);
            settings.SetValue("textBoxScrollToSelectionOffset", textBoxScrollToSelectionOffset.Text);
            settings.SetValue("checkBoxMultiSelectionSongList", checkBoxMultiSelectionSongList.Checked);
            settings.SetValue("checkBoxSkipGenIfEasyNotes", checkBoxSkipGenIfEasyNotes.Checked);
            settings.SetValue("checkGenDiffCopyGuitarToBass", checkGenDiffCopyGuitarToBass.Checked);
            settings.SetValue("checkBoxLoadLastSongStartup", checkBoxLoadLastSongStartup.Checked);
            settings.SetValue("textClearHoldBox", textClearHoldBox.Text);
            settings.SetValue("checkBoxEnableMidiInput", checkBoxEnableMidiInput.Checked);
            settings.SetValue("checkBoxEnableClearTimer", checkBoxEnableClearTimer.Checked);
            settings.SetValue("checkBoxPlayMidiStrum", checkBoxPlayMidiStrum.Checked);
            settings.SetValue("checkBoxClearIfNoFrets", checkBoxClearIfNoFrets.Checked);
            settings.SetValue("checkBoxChordStrum", checkBoxChordStrum.Checked);
            settings.SetValue("checkBoxMidiInputStartup", checkBoxMidiInputStartup.Checked);
            settings.SetValue("checkView5Button", checkView5Button.Checked);
            settings.SetValue("comboBoxMidiInput", comboBoxMidiInput.Text);

            settings.SetValue("textBoxNoteCloseDist", textBoxNoteCloseDist.Text);

            settings.SetValue("checkBoxBatchCheckCON", checkBoxBatchCheckCON.Checked);
            settings.SetValue("checkBoxBatchGenerateDifficulties", checkBoxBatchGenerateDifficulties.Checked);
            settings.SetValue("checkBoxBatchGuitarBassCopy", checkBoxBatchGuitarBassCopy.Checked);
            settings.SetValue("checkBoxBatchRebuildCON", checkBoxBatchRebuildCON.Checked);

            settings.SetValue("checkBoxMatchAllFrets", checkBoxMatchAllFrets.Checked);
            settings.SetValue("checkBoxCompressAllInDefaultCONFolder", checkBoxCompressAllInDefaultCONFolder.Checked);

            settings.SetValue("textBoxCompressAllZipFile", textBoxCompressAllZipFile.Text);
            settings.SetValue("checkBoxRenderMouseSnap", checkBoxRenderMouseSnap.Checked);
            settings.SetValue("checkBoxSnapToCloseNotes", checkBoxSnapToCloseNotes.Checked);

            settings.SetValue("textBoxNoteSnapDistance", Utility.NoteSnapDistance);
            settings.SetValue("textBoxGridSnapDistance", Utility.GridSnapDistance);

            int maxSearchID = int.MinValue;
            var searchPatterns = XMLUtil.GetNode(settings.XMLRoot, "searchPatterns");
            if (searchPatterns == null)
            {
                searchPatterns = XMLUtil.AddNode(settings.XMLRoot, "searchPatterns");
            }
            else
            {
                var presets = GetCopyPatternPresetsFromScreen();
                var idList = XMLUtil.GetNodeList(searchPatterns, "searchPattern");
                foreach (var item in idList)
                {
                    var id = XMLUtil.GetNodeValueInt(item, "@ID");
                    if (!id.IsNull())
                    {
                        if (presets.Count(x => x.ID == id) == 0)
                        {
                            searchPatterns.RemoveChild(item);
                        }
                        if (id > maxSearchID)
                        {
                            maxSearchID = id;
                        }
                    }
                }
                foreach (var item in presets)
                {
                    if (item.ID.IsNull() == false)
                    {
                        if (item.ID > maxSearchID)
                        {
                            maxSearchID = item.ID;
                        }
                    }
                }
            }

            if (maxSearchID.IsNull())
            {
                maxSearchID = 123;
            }
            foreach (CopyPatternPreset preset in comboNoteEditorCopyPatternPreset.Items)
            {
                XmlNode node = null;
                if (!preset.ID.IsNull())
                {
                    node = XMLUtil.GetNode(searchPatterns, "searchPattern[@ID=" + preset.ID.ToString() + "]");
                }
                if (node == null)
                {
                    node = XMLUtil.AddNode(searchPatterns, "searchPattern");
                }
                if (preset.ID.IsNull())
                {
                    preset.ID = maxSearchID + 1;
                    maxSearchID = maxSearchID + 1;
                }
                XMLUtil.AddAttribute(node, "ID", preset.ID.ToString());
                XMLUtil.AddAttribute(node, "Name", preset.Name);
                XMLUtil.AddAttribute(node, "ForwardOnly", preset.ForwardOnly.ToString());
                XMLUtil.AddAttribute(node, "MatchLengths5", preset.MatchLengths5.ToString());
                XMLUtil.AddAttribute(node, "MatchLengths6", preset.MatchLengths6.ToString());
                XMLUtil.AddAttribute(node, "MatchSpacing", preset.MatchSpacing.ToString());
                XMLUtil.AddAttribute(node, "MatchBeat", preset.MatchBeat.ToString());
                XMLUtil.AddAttribute(node, "KeepLengths", preset.KeepLengths.ToString());
                XMLUtil.AddAttribute(node, "FirstMatchOnly", preset.FirstMatchOnly.ToString());
                XMLUtil.AddAttribute(node, "RemoveExisting", preset.RemoveExisting.ToString());
            }

            var docLibRoot = XMLUtil.GetNode(settings.XMLRoot, "docLib");
            if (docLibRoot == null)
            {
                docLibRoot = XMLUtil.AddNode(settings.XMLRoot, "docLib");
            }

            foreach (SongCacheItem dsc in SongList.RemovedSongs)
            {
                var ddocLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@CacheSongID='{0}']", dsc.CacheSongID));
                if (ddocLibSong == null)
                {
                    ddocLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@name='{0}']", dsc.SongName));
                }

                if (ddocLibSong != null)
                {
                    XmlNode docLibSong = null;
                    foreach (var sc in SongList)
                    {
                        docLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@CacheSongID='{0}']", sc.CacheSongID));
                        if (docLibSong == null)
                        {
                            docLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@name='{0}']", sc.SongName));
                        }
                        if (docLibSong != null)
                            break;
                    }

                    if (docLibSong == null)
                    {
                        docLibSong.ParentNode.RemoveChild(docLibSong);
                    }
                }
            }

            foreach (SongCacheItem sc in SongList)
            {
                var docLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@CacheSongID='{0}']", sc.CacheSongID));
                if (docLibSong == null)
                {
                    docLibSong = XMLUtil.GetNode(docLibRoot, string.Format("song[@name='{0}']", sc.SongName));
                }

                if (docLibSong == null)
                    docLibSong = XMLUtil.AddNode(docLibRoot, "song");

                XMLUtil.AddAttribute(docLibSong, "CacheSongID", sc.CacheSongID.ToString());
                XMLUtil.AddAttribute(docLibSong, "name", sc.SongName);
                XMLUtil.AddAttribute(docLibSong, "G5FileName", sc.G5FileName);
                XMLUtil.AddAttribute(docLibSong, "G6FileName", sc.G6FileName);
                XMLUtil.AddAttribute(docLibSong, "G6ConFile", sc.G6ConFile);
                XMLUtil.AddAttribute(docLibSong, "Description", sc.Description);

                XMLUtil.AddAttribute(docLibSong, "HasBass", sc.HasBass.ToString());
                XMLUtil.AddAttribute(docLibSong, "HasGuitar", sc.HasGuitar.ToString());
                XMLUtil.AddAttribute(docLibSong, "CopyGuitarToBass", sc.CopyGuitarToBass.ToString());
                XMLUtil.AddAttribute(docLibSong, "IsComplete", sc.IsComplete.ToString());
                XMLUtil.AddAttribute(docLibSong, "IsFinalized", sc.IsFinalized.ToString());

                XMLUtil.AddAttribute(docLibSong, "GuitarLowE", sc.GuitarTuning[0]);
                XMLUtil.AddAttribute(docLibSong, "GuitarA", sc.GuitarTuning[1]);
                XMLUtil.AddAttribute(docLibSong, "GuitarD", sc.GuitarTuning[2]);
                XMLUtil.AddAttribute(docLibSong, "GuitarG", sc.GuitarTuning[3]);
                XMLUtil.AddAttribute(docLibSong, "GuitarB", sc.GuitarTuning[4]);
                XMLUtil.AddAttribute(docLibSong, "GuitarHighE", sc.GuitarTuning[5]);

                XMLUtil.AddAttribute(docLibSong, "BassLowE", sc.BassTuning[0]);
                XMLUtil.AddAttribute(docLibSong, "BassA", sc.BassTuning[1]);
                XMLUtil.AddAttribute(docLibSong, "BassD", sc.BassTuning[2]);
                XMLUtil.AddAttribute(docLibSong, "BassG", sc.BassTuning[3]);
                XMLUtil.AddAttribute(docLibSong, "BassB", sc.BassTuning[4]);
                XMLUtil.AddAttribute(docLibSong, "BassHighE", sc.BassTuning[5]);

                XMLUtil.AddAttribute(docLibSong, "DTABassDifficulty", sc.DTABassDifficulty.ToString());
                XMLUtil.AddAttribute(docLibSong, "DTAGuitarDifficulty", sc.DTAGuitarDifficulty.ToString());
                XMLUtil.AddAttribute(docLibSong, "DTASongID", sc.DTASongID);
                XMLUtil.AddAttribute(docLibSong, "DTASongShortName", sc.DTASongShortName);

                XMLUtil.AddAttribute(docLibSong, "SongMP3Location", sc.SongMP3Location ?? "");
                XMLUtil.AddAttribute(docLibSong, "SongMP3PlaybackOffset", sc.SongMP3PlaybackOffset.ToStringEx());
                XMLUtil.AddAttribute(docLibSong, "EnableSongMP3Playback", sc.EnableSongMP3Playback.ToString());

                XMLUtil.AddAttribute(docLibSong, "EnableSongMidiPlayback", sc.EnableSongMidiPlayback.ToString());
                XMLUtil.AddAttribute(docLibSong, "SongMP3PlaybackVolume", sc.SongMP3PlaybackVolume.ToStringEx());
                XMLUtil.AddAttribute(docLibSong, "SongMidiPlaybackVolume", sc.SongMidiPlaybackVolume.ToStringEx());


                XMLUtil.AddAttribute(docLibSong, "AutoGenGuitarHard", sc.AutoGenGuitarHard.ToString());
                XMLUtil.AddAttribute(docLibSong, "AutoGenGuitarMedium", sc.AutoGenGuitarMedium.ToString());
                XMLUtil.AddAttribute(docLibSong, "AutoGenGuitarEasy", sc.AutoGenGuitarEasy.ToString());

                XMLUtil.AddAttribute(docLibSong, "AutoGenBassHard", sc.AutoGenBassHard.ToString());
                XMLUtil.AddAttribute(docLibSong, "AutoGenBassMedium", sc.AutoGenBassMedium.ToString());
                XMLUtil.AddAttribute(docLibSong, "AutoGenBassEasy", sc.AutoGenBassEasy.ToString());

            }



            foreach (UtilProperty prop in Properties)
            {
                if (prop.Type == PropertyType.Brush)
                {
                    settings.SetValue("Util_" + prop.Description, ((SolidBrush)prop.Data).Color.ToArgb().ToString());
                }
                else if (prop.Type == PropertyType.Pen)
                {
                    settings.SetValue("Util_" + prop.Description, ((Pen)prop.Data).Color.ToArgb().ToString());
                    settings.SetValue("Util_W_" + prop.Description, ((Pen)prop.Data).Width.ToString());
                }
                else
                {
                    settings.SetValue("Util_" + prop.Description, prop.Data.ToString());
                }
            }

            settings.SaveConfig();
        }
    }
}