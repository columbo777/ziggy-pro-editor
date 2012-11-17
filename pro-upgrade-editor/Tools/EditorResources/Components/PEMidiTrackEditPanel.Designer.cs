namespace EditorResources.Components
{
    partial class PEMidiTrackEditPanel
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PEMidiTrackEditPanel));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxTrackName = new System.Windows.Forms.TextBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listTracks = new EditorResources.Components.PEListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonDelete = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "XPLens.gif");
            this.imageList1.Images.SetKeyName(1, "XPRecycle.gif");
            this.imageList1.Images.SetKeyName(2, "music-beam-16.png");
            this.imageList1.Images.SetKeyName(3, "music--exclamation.png");
            this.imageList1.Images.SetKeyName(4, "plus_16.png");
            this.imageList1.Images.SetKeyName(5, "textfield_rename.png");
            this.imageList1.Images.SetKeyName(6, "copy.png");
            this.imageList1.Images.SetKeyName(7, "Add.png");
            this.imageList1.Images.SetKeyName(8, "redx.png");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem2.Text = "Details";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem3.Text = "List";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(240, 24);
            this.menuStrip.TabIndex = 5;
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listToolStripMenuItem,
            this.detailsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Visible = false;
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.listToolStripMenuItem.Text = "List";
            this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refreshToolStripMenuItem.Image = global::EditorResources.PEResources.gtk_refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.refreshToolStripMenuItem.ToolTipText = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // textBoxTrackName
            // 
            this.textBoxTrackName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTrackName.Location = new System.Drawing.Point(3, 304);
            this.textBoxTrackName.Name = "textBoxTrackName";
            this.textBoxTrackName.Size = new System.Drawing.Size(161, 20);
            this.textBoxTrackName.TabIndex = 6;
            // 
            // buttonRename
            // 
            this.buttonRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRename.ImageIndex = 5;
            this.buttonRename.ImageList = this.imageList1;
            this.buttonRename.Location = new System.Drawing.Point(167, 301);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(24, 24);
            this.buttonRename.TabIndex = 7;
            this.buttonRename.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonRename, "Rename Track");
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.ImageIndex = 7;
            this.buttonCreate.ImageList = this.imageList1;
            this.buttonCreate.Location = new System.Drawing.Point(190, 301);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(24, 24);
            this.buttonCreate.TabIndex = 9;
            this.buttonCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonCreate, "Create New Track");
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCopy.ImageIndex = 6;
            this.buttonCopy.ImageList = this.imageList1;
            this.buttonCopy.Location = new System.Drawing.Point(213, 301);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(24, 24);
            this.buttonCopy.TabIndex = 10;
            this.buttonCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonCopy, "Copy Track");
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Track Name:";
            // 
            // listTracks
            // 
            this.listTracks.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listTracks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listTracks.Cursor = System.Windows.Forms.Cursors.Default;
            this.listTracks.FullRowSelect = true;
            this.listTracks.GridLines = true;
            this.listTracks.HeaderColorBottom = System.Drawing.Color.Silver;
            this.listTracks.HeaderColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.listTracks.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTracks.HideSelection = false;
            this.listTracks.LabelWrap = false;
            this.listTracks.Location = new System.Drawing.Point(-1, 24);
            this.listTracks.Name = "listTracks";
            this.listTracks.OwnerDraw = true;
            this.listTracks.SelectedItem = null;
            this.listTracks.Size = new System.Drawing.Size(242, 261);
            this.listTracks.TabIndex = 4;
            this.listTracks.TileSize = new System.Drawing.Size(32, 32);
            this.listTracks.UseCompatibleStateImageBehavior = false;
            this.listTracks.View = System.Windows.Forms.View.Details;
            this.listTracks.ItemActivate += new System.EventHandler(this.peListView1_ItemActivate);
            this.listTracks.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listTracks_ItemDrag);
            this.listTracks.SelectedIndexChanged += new System.EventHandler(this.listTracks_SelectedIndexChanged);
            this.listTracks.DragDrop += new System.Windows.Forms.DragEventHandler(this.listTracks_DragDrop);
            this.listTracks.DragEnter += new System.Windows.Forms.DragEventHandler(this.listTracks_DragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tracks";
            this.columnHeader1.Width = 240;
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonDelete.ImageIndex = 8;
            this.ButtonDelete.ImageList = this.imageList1;
            this.ButtonDelete.Location = new System.Drawing.Point(3, 326);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(24, 24);
            this.ButtonDelete.TabIndex = 3;
            this.toolTip1.SetToolTip(this.ButtonDelete, "Delete Selected Track");
            this.ButtonDelete.UseVisualStyleBackColor = true;
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // PEMidiTrackEditPanel
            // 
            this.AllowDrop = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.textBoxTrackName);
            this.Controls.Add(this.listTracks);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.ButtonDelete);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PEMidiTrackEditPanel";
            this.Size = new System.Drawing.Size(240, 354);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PEMidiTrackEditPanel_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PEMidiTrackEditPanel_DragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button ButtonDelete;
        private PEListView listTracks;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxTrackName;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
