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
using ProUpgradeEditor.DataLayer;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ProUpgradeEditor.UI
{

    partial class MainForm
    {

        public void RefreshModifierListBoxes()
        {
            foreach (var lm in listedModifiers)
            {
                RefreshModifierListBox(lm.modifierType);
            }

            try
            {
                RefreshTrainers();
            }
            catch{}

            try
            {
                RefreshTextEvents();
            }
            catch { }
        }

        public void RefreshTrainers()
        {
            RefreshTrainer(GuitarTrainerType.ProGuitar);
            RefreshTrainer(GuitarTrainerType.ProBass);   
        }

        public void RefreshTextEvents()
        {
            EditorPro.ClearTextEventSelection();
            
            listTextEvents.BeginUpdate();
            listTextEvents.Items.Clear();

            if (EditorPro.IsLoaded)
            {
                foreach (var mev in ProGuitarTrack.Messages.TextEvents)
                {
                    if((checkBoxShowTrainersInTextEvents.Checked == true && mev.Type != GuitarTrainerMetaEventType.Unknown) ||
                        mev.Type == GuitarTrainerMetaEventType.Unknown)
                    {
                        listTextEvents.Items.Add(mev);
                    }

                }
            }

            listTextEvents.EndUpdate();
            if (!reloading)
            {
                ReloadTrack(SelectNextEnum.ForceKeepSelection, false);
            }
            EditorPro.Invalidate();
        }

        public void RefreshTrainer(GuitarTrainerType type)
        {
            try
            {

                ListBox list = null;
                if (type == GuitarTrainerType.ProGuitar)
                {
                    list = listProGuitarTrainers;
                }
                else if (type == GuitarTrainerType.ProBass)
                {
                    list = listProBassTrainers;
                }

                if (list != null)
                {
                    list.BeginUpdate();

                    list.Items.Clear();
                    if (EditorPro.IsLoaded)
                    {
                        foreach (var trainer in ProGuitarTrack.Messages.Trainers)
                        {
                            if (trainer.TrainerType == type)
                            {
                                list.Items.Add(trainer);
                            }
                        }
                    }
                    list.EndUpdate();
                }


            }
            catch { }

            EditorPro.Invalidate();
        }

        public void RefreshModifierListBox(GuitarModifierType type)
        {
            var gmod = listedModifiers.Single(x => x.modifierType == type);

            var listBox = gmod.modifierList;
            var textBoxStart = gmod.modifierStartBox;
            var textBoxEnd = gmod.modifierEndBox;

            var gt = EditorPro.GuitarTrack;
            if (gt == null || gt.Messages == null)
                return;

            listBox.Items.Clear();

            var modList = gt.Messages.Where(x =>
                x is GuitarModifier &&
                ((GuitarModifier)x).ModifierType == type).ToArray();

            for (int x = 0; x < modList.Length; x++)
            {
                var obj = modList[x];
                listBox.Items.Add(new stringObject()
                {
                    Name = type.ToString() + (x + 1).ToString(),
                    Obj = obj,
                });

                obj.Selected = false;
            }

            EditorPro.Invalidate();
        }

        public void SelectedModifierChanged(GuitarModifierType type)
        {
            var gmod = listedModifiers.Single(x => x.modifierType == type);

            var listBox = gmod.modifierList;
            var textBoxStart = gmod.modifierStartBox;
            var textBoxEnd = gmod.modifierEndBox;


            var gt = EditorPro.GuitarTrack;

            if (gt == null || gt.Messages == null)
                return;

            foreach (var p in gt.Messages.Where(x => x is GuitarModifier && ((GuitarModifier)x).ModifierType == type))
            {
                p.Selected = false;
            }


            if (listBox.Items.Count >= 0 && listBox.SelectedItem != null)
            {
                var obj = (listBox.SelectedItem as stringObject).Obj as GuitarModifier;
                if (obj != null)
                {
                    obj.Selected = true;

                    textBoxStart.Text = obj.DownTick.ToStringEx();
                    textBoxEnd.Text = obj.UpTick.ToStringEx();
                }
            }

            EditorPro.Invalidate();
        }


        public void DeleteArpeggio()
        {
            try
            {
                if (Utility.GetArpeggioData1(GetEditorDifficulty()) == -1)
                    return;

                if (listBox3.Items.Count == 0)
                    return;

                var gt = EditorPro.GuitarTrack;
                if (gt == null)
                    return;

                if (gt.Messages == null || gt.Messages.Arpeggios == null)
                    return;

                if (listBox3.SelectedItem != null)
                {
                    var o = listBox3.SelectedItem as stringObject;
                    var m = o.Obj as GuitarArpeggio;

                    foreach (GuitarChord c in gt.Messages.Chords)
                    {
                        if (c.DownTick < m.UpTick &&
                            c.UpTick > m.DownTick)
                        {
                            for (int x = 0; x < c.Notes.Length; x++)
                            {
                                var n = c.Notes[x];
                                if (n != null)
                                {
                                    if (n.Channel == Utility.ChannelArpeggio)
                                    {
                                        gt.Remove(n);

                                    }
                                }
                            }
                        }
                    }
                    gt.Remove(m);

                    ReloadTracks();
                }
            }
            catch { }
        }


        public class ListedModifier
        {
            public GuitarModifierType modifierType;
            public ListBox modifierList;
            public TextBox modifierStartBox;
            public TextBox modifierEndBox;
        }


        public void RefreshSolosList()
        {
            RefreshModifierListBox(GuitarModifierType.Solo);
        }

        List<ListedModifier> listedModifiers = new List<ListedModifier>();
        public void AddListedModifiers()
        {
            listedModifiers.Clear();

            listedModifiers.Add(new ListedModifier()
            {
                modifierType = GuitarModifierType.Powerup,
                modifierList = listBox2,
                modifierStartBox = textBox27,
                modifierEndBox = textBox28,
            });

            listedModifiers.Add(new ListedModifier()
            {
                modifierType = GuitarModifierType.Arpeggio,
                modifierList = listBox3,
                modifierStartBox = textBox29,
                modifierEndBox = textBox30,
            });

            listedModifiers.Add(new ListedModifier()
            {
                modifierType = GuitarModifierType.Solo,
                modifierList = listBoxSolos,
                modifierStartBox = textBox1,
                modifierEndBox = textBox26,
            });

            listedModifiers.Add(new ListedModifier()
            {
                modifierType = GuitarModifierType.SingleStringTremelo,
                modifierList = listBox5,
                modifierStartBox = textBox17,
                modifierEndBox = textBox18,
            });

            listedModifiers.Add(new ListedModifier()
            {
                modifierType = GuitarModifierType.MultiStringTremelo,
                modifierList = listBox4,
                modifierStartBox = textBox15,
                modifierEndBox = textBox16,
            });
        }

        public void RemoveSelectedModifier(GuitarModifierType type)
        {
            try
            {
                var gmod = listedModifiers.Single(x => x.modifierType == type);

                var listBox = gmod.modifierList;


                var gt = EditorPro.GuitarTrack;

                if (gt == null || gt.Messages == null ||
                    listBox.Items.Count == 0 || listBox.SelectedItem == null)
                    return;


                var obj = (listBox.SelectedItem as stringObject).Obj as GuitarModifier;
                if (obj != null)
                {
                    gt.Remove(obj);
                    
                    obj.Selected = false;
                }

                ReloadTracks();
            }
            catch { }
        }


        public GuitarModifier GetSelectedModifier(GuitarModifierType type)
        {
            
            GuitarModifier ret = null;

            var gmod = listedModifiers.Single(x => x.modifierType == type);

            var listBox = gmod.modifierList;
            var textBoxStart = gmod.modifierStartBox;
            var textBoxEnd = gmod.modifierEndBox;

            var gt = EditorPro.GuitarTrack;

            if (gt == null || gt.Messages == null ||
                listBox.Items.Count == 0 || listBox.SelectedItem == null)
                return ret;


            var obj = (listBox.SelectedItem as stringObject).Obj as GuitarModifier;
            if (obj != null)
            {
                ret = obj;
            }
            return ret;
            
        }
        public void UpdateSelectedModifier(GuitarModifierType type)
        {
            try
            {

                var gmod = listedModifiers.Single(x => x.modifierType == type);

                var listBox = gmod.modifierList;
                var textBoxStart = gmod.modifierStartBox;
                var textBoxEnd = gmod.modifierEndBox;

                var gt = EditorPro.GuitarTrack;

                if (gt == null || gt.Messages == null ||
                    listBox.Items.Count == 0 || listBox.SelectedItem == null)
                    return;


                var obj = (listBox.SelectedItem as stringObject).Obj as GuitarModifier;
                if (obj != null)
                {
                    var ms = obj.DownEvent;
                    var me = obj.UpEvent;

                    int st = (textBoxStart.Text).ToInt();
                    int ed = (textBoxEnd.Text).ToInt();
                    if (!st.IsNull() && !ed.IsNull())
                    {
                        if (st < ed && !Utility.IsCloseTick(st, ed))
                        {
                            if (ms.AbsoluteTicks != st ||
                                me.AbsoluteTicks != ed)
                            {
                                EditorPro.GuitarTrack.Remove(ms);
                                EditorPro.GuitarTrack.Insert(
                                    st, ms.MidiMessage);

                                EditorPro.GuitarTrack.Remove(me);
                                EditorPro.GuitarTrack.Insert(
                                    ed, me.MidiMessage);
                            }
                        }
                    }
                }
                ReloadTracks();
            }
            catch { }
        }
    }
}
