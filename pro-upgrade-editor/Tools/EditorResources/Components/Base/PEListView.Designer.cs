namespace EditorResources.Components
{
    partial class PEListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && (components != null))
            {
                components.Dispose();
            }

        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PEListView
            // 
            this.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FullRowSelect = true;
            this.GridLines = true;
            this.HideSelection = false;
            this.LabelWrap = false;
            this.OwnerDraw = true;

            this.TileSize = new System.Drawing.Size(32, 32);
            this.View = System.Windows.Forms.View.Details;
            this.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.PEListView_ColumnWidthChanged);
            this.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.PEListView_DrawColumnHeader);
            this.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(PEListView_DrawItem);
            this.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(PEListView_DrawSubItem);
            this.ItemActivate += new System.EventHandler(this.PEListView_ItemActivate);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PEListView_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PEListView_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.PEListView_Resize);
            this.ResumeLayout(false);

        }


        #endregion



    }
}
