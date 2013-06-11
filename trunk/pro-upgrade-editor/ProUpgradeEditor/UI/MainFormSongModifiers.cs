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
            catch { }

            try
            {
                Refresh108EventList();
            }
            catch { }
        }

        public void RefreshTrainers()
        {
            RefreshTrainer(GuitarTrainerType.ProGuitar);
            RefreshTrainer(GuitarTrainerType.ProBass);
            RefreshTextEvents();   
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
                    if ((checkBoxShowTrainersInTextEvents.Checked == true &&
                        mev.IsTrainerEvent) || mev.IsTrainerEvent == false)
                    {
                        listTextEvents.Items.Add(mev);
                    }
                }
            }

            listTextEvents.EndUpdate();

            EditorPro.Invalidate();
        }

        public void RefreshTrainer(GuitarTrainerType type)
        {
            try
            {

                ListBox list = null;
                RefreshTextEvents();

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
                        foreach (var trainer in ProGuitarTrack.Messages.Trainers.Where(t=> t.TrainerType == type).ToList())
                        {
                            list.Items.Add(trainer);
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


            var modList = gt.Messages.GetModifiersByType(type).ToList();

            foreach (var obj in modList)
            {
                listBox.Items.Add(new StringObject()
                {
                    Name = type.ToString() + (modList.IndexOf(obj)).ToString(),
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

            foreach (var p in gt.Messages.GetModifiersByType(type).ToList())
            {
                p.Selected = false;
            }


            if (listBox.Items.Count >= 0 && listBox.SelectedItem != null)
            {
                var obj = (listBox.SelectedItem as StringObject).Obj as GuitarModifier;
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
                if (!EditorPro.IsLoaded)
                    return;

                if (Utility.GetArpeggioData1(EditorPro.CurrentDifficulty).IsNull())
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
                    var o = listBox3.SelectedItem as StringObject;
                    var m = o.Obj as GuitarArpeggio;
                    
                    foreach (GuitarChord c in gt.Messages.Chords.ToList())
                    {
                        if (c.DownTick < m.UpTick &&
                            c.UpTick > m.DownTick)
                        {
                            c.Notes.Where(x => x.IsArpeggioNote).ToList().ForEach(x => c.RemoveNote(x));
                        }
                        if (c.Notes.Count() == 0)
                            c.DeleteAll();
                    }
                    m.DeleteAll();

                    ReloadTracks();
                }
            }
            catch { }
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


                var obj = (listBox.SelectedItem as StringObject).Obj as GuitarModifier;
                if (obj != null)
                {
                    obj.DeleteAll();

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


            var obj = (listBox.SelectedItem as StringObject).Obj as GuitarModifier;
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


                var obj = (listBox.SelectedItem as StringObject).Obj as GuitarModifier;
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
                                obj.SetTicks(new TickPair(st, ed));
                                obj.UpdateEvents();
                                
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
