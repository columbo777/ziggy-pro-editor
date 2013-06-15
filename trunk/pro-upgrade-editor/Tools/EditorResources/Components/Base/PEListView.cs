using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Globalization;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor;
using System.Collections;
using ProUpgradeEditor.Common;


namespace EditorResources.Components
{

    public partial class PEListView : ListView
    {


        public class PEListViewEventArgs : EventArgs
        {
            public PEListViewItem Item;
        }

        public class PEListViewItem : ListViewItem
        {
            Track track;
            GuitarDifficulty difficulty;
            PEListView.PEListViewItem parentItem;
            PEListView ownerView;


            public PEListViewItem(PEListView ownerView, PEListView.PEListViewItem parentItem, string text, Track track, GuitarDifficulty difficulty)
                : base(text)
            {
                this.StateImageIndex = 0;

                this.ownerView = ownerView;
                this.parentItem = parentItem;
                this.track = track;
                this.difficulty = difficulty;
            }

            public PEListView Owner { get { return ownerView; } }
            public PEListView.PEListViewItem ParentItem { get { return parentItem; } }
            public Track Track { get { return track; } }
            public GuitarDifficulty Difficulty { get { return difficulty; } }

        }

        public class PEListViewItemCollection : ListViewItemCollection, IEnumerable<PEListViewItem>
        {
            PEListView owner;


            public PEListViewItemCollection(PEListView view)
                : base(view)
            {
                owner = view;
            }

            public PEListView Owner { get { return owner; } }

            public PEListView.PEListViewItem Add(PEListView.PEListViewItem value)
            {
                return base.Add(value) as PEListView.PEListViewItem;
            }

            public new void RemoveAt(int index)
            {
                base.RemoveAt(IndexOf(this[index]));
            }

            public PEListViewItem Insert(int index, PEListViewItem item)
            {
                return base.Insert(index, item) as PEListViewItem;
            }

            public int IndexOf(PEListViewItem item) { return base.IndexOf(item); }

            public new int Count { get { return base.Count; } }

            public new PEListViewItem this[int index]
            {
                get { return base[index] as PEListViewItem; }
                set { base[index] = value; }
            }

            public new void Clear()
            {
                foreach (PEListViewItem item in this)
                {
                    item.Selected = false;
                }
                base.Clear();
            }

            public new IEnumerator<PEListViewItem> GetEnumerator()
            {
                var enumer = base.GetEnumerator();
                while (enumer.MoveNext())
                {
                    yield return enumer.Current as PEListViewItem;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        Font headerFont;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        private ColumnHeaderCollection headers
        {
            get { return Columns; }
            set
            {
                Columns.Clear();
                foreach (ColumnHeader h in value)
                {
                    Columns.Add(h);
                }
            }
        }

        PEListViewItemCollection items;
        public new PEListViewItemCollection Items
        {
            get { return items; }
        }

        PEListViewItem selectedItem = null;
        public PEListViewItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        private bool drawGrid
        {
            get { return GridLines; }
            set
            {
                GridLines = value;
            }
        }
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font HeaderFont
        {
            get { return headerFont; }
            set { headerFont = value; }
        }

        Color headerColorTop = Color.FromArgb(240, 240, 240);
        Color headerColorBottom = Color.FromArgb(220, 220, 220);

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HeaderColorTop
        {
            get { return headerColorTop; }
            set { headerColorTop = value; }
        }
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HeaderColorBottom
        {
            get { return headerColorBottom; }
            set { headerColorBottom = value; }
        }



        public PEListView()
        {
            headerFont = this.Font;
            headers = Columns;
            items = new PEListViewItemCollection(this);

            InitializeComponent();
            resizeColumn();
        }

        public void ActivateItem(PEListViewItem li)
        {
            foreach (var i in Items)
            {
                i.Selected = false;
            }
            if (li != null)
            {
                li.Selected = true;
            }

            selectedItem = li;

            OnItemActivate(null);
        }

        void PEListView_DrawItem(object sender, System.Windows.Forms.DrawListViewItemEventArgs e)
        {
            var item = e.Item as PEListViewItem;

            var bounds = item.Bounds;
            var g = e.Graphics;
            e.DrawBackground();

            bool selected = SelectedItems.Contains(item);
            Color itemColor = BackColor;
            if (selected)
            {
                itemColor = Color.LightSkyBlue;
            }
            using (var highlightBrush = new SolidBrush(Color.FromArgb(200, itemColor)))
            {
                var drawBound = bounds;

                g.FillRectangle(highlightBrush, drawBound);
            }

            if (selected)
            {
                var drawBound = bounds;
                drawBound.Width -= 4;
                drawBound.Height -= 4;
                drawBound.X += 2;
                drawBound.Y += 1;

                using (Pen p = new Pen(Color.FromArgb(100, Color.SteelBlue), 1.5f))
                {
                    g.DrawRectangle(p, drawBound);
                }
            }
            if (item.ParentItem == null)
            {
                TextRenderer.DrawText(g,
                    item.Text,
                    this.Font,
                    bounds,
                    ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis | TextFormatFlags.PreserveGraphicsClipping);
            }
        }
        private void PEListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            PEListViewItem item = e.Item as PEListViewItem;
            if (item.ParentItem != null)
            {
                var bounds = e.SubItem.Bounds;
                bounds.Width -= 40;
                bounds.X += 40;

                TextRenderer.DrawText((IDeviceContext)e.Graphics,
                    e.SubItem.Text,
                    this.Font,
                    bounds, ForeColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                            TextFormatFlags.LeftAndRightPadding |
                            TextFormatFlags.EndEllipsis |
                            TextFormatFlags.PreserveGraphicsClipping);
            }
        }

        private void PEListView_Resize(object sender, EventArgs e)
        {
            resizeColumn();
        }

        private void resizeColumn()
        {
            if (Columns.Count == 1)
            {
                Columns[0].Width = Width - 2;
            }
        }


        private void PEListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {

            using (LinearGradientBrush brush =
                new LinearGradientBrush(e.Bounds, headerColorTop,
                    headerColorBottom, LinearGradientMode.Vertical))
            {
                //#FFD3D3D3
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // Draw the header text.
            TextRenderer.DrawText((IDeviceContext)e.Graphics, e.Header.Text, HeaderFont,
                e.Bounds, Color.Black, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);


            e.Graphics.DrawLine(Pens.Black,
                new Point(e.Bounds.Location.X, e.Bounds.Height - 1),
                new Point(e.Bounds.Location.X + e.Bounds.Width, e.Bounds.Height - 1));
        }

        private void PEListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            this.NotifyInvalidate(ClientRectangle);
            Invalidate();
        }

        public event EventHandler<PEListViewEventArgs> OnSelectedItemChanged;
        public event EventHandler<PEListViewEventArgs> OnItemClicked;

        private void PEListView_ItemActivate(object sender, EventArgs e)
        {
            if (SelectedItems.Count > 0)
            {
                var li = SelectedItems[0] as PEListViewItem;

                if (selectedItem != li)
                {
                    selectedItem = li;

                    if (OnSelectedItemChanged != null)
                    {
                        OnSelectedItemChanged(this, new PEListViewEventArgs() { Item = selectedItem });
                    }
                    Invalidate();
                }
            }
        }

        public PEListView.PEListViewItem GetItemFromScreenPoint(Point screenPoint, bool allowSubItem = false)
        {
            var p = PointToClient(screenPoint);
            var pItem = GetItemAt(p.X, p.Y) as PEListView.PEListViewItem;
            if (pItem == null && Items.Count > 0)
            {
                pItem = Items[Items.Count - 1];
            }
            else if (pItem != null && allowSubItem == false && pItem.ParentItem != null)
            {
                pItem = pItem.ParentItem;
            }

            return pItem;
        }

        private void PEListView_MouseClick(object sender, MouseEventArgs e)
        {
            OnItemActivate(e);

            if (OnItemClicked != null)
            {
                var item = GetItemFromScreenPoint(PointToScreen(new Point(e.X, e.Y)), true);
                if (item != null)
                {
                    OnItemClicked(this, new PEListViewEventArgs() { Item = item });
                }
            }
        }

        private void PEListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnItemActivate(e);
        }

    }
}