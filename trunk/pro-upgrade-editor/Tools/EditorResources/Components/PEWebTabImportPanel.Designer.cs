using ProUpgradeEditor.Common;
namespace EditorResources.Components
{
    partial class PEWebTabImportPanel
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
            this.buttonImport = new EditorResources.Components.PEButton();
            this.trackEditorPro = new EditorResources.Components.PEMidiTrackEditPanel();
            this.trackEditorWebTab = new EditorResources.Components.PEMidiTrackEditPanel();
            this.peGroupBox1 = new EditorResources.Components.PEGroupBox();
            this.peGroupBox2 = new EditorResources.Components.PEGroupBox();
            this.peGroupBox3 = new EditorResources.Components.PEGroupBox();
            this.ProOffset = new EditorResources.Components.PEEditBox();
            this.ProScale = new EditorResources.Components.PEEditBox();
            this.buttonRefresh = new EditorResources.Components.PEButton();
            this.ImportTrack = new EditorResources.Components.PECheckBox();
            this.peLabel2 = new EditorResources.Components.PELabel();
            this.resetScale = new EditorResources.Components.PEButton();
            this.peLabel1 = new EditorResources.Components.PELabel();
            this.AutoRefresh = new EditorResources.Components.PECheckBox();
            this.peGroupBox2.SuspendLayout();
            this.peGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.BackColor = System.Drawing.Color.Transparent;
            this.buttonImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonImport.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonImport.ForeColor = System.Drawing.Color.Black;
            this.buttonImport.Location = new System.Drawing.Point(470, 464);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(66, 27);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = false;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // trackEditorPro
            // 
            this.trackEditorPro.AllowDrop = true;
            this.trackEditorPro.BackColor = System.Drawing.SystemColors.Control;
            this.trackEditorPro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trackEditorPro.IsPro = false;
            this.trackEditorPro.Location = new System.Drawing.Point(285, 31);
            this.trackEditorPro.Margin = new System.Windows.Forms.Padding(0);
            this.trackEditorPro.Name = "trackEditorPro";
            this.trackEditorPro.SelectedDifficulty = GuitarDifficulty.Expert;
            this.trackEditorPro.Size = new System.Drawing.Size(239, 268);
            this.trackEditorPro.TabIndex = 1;
            this.trackEditorPro.TrackRemoved += new EditorResources.Components.TrackEditPanelEventHandler(this.trackEditorPro_TrackRemoved);
            this.trackEditorPro.TrackClicked += new EditorResources.Components.TrackEditPanelEventHandler(this.trackEditorPro_TrackClicked);
            this.trackEditorPro.Load += new System.EventHandler(this.trackEditorPro_Load);
            // 
            // trackEditorWebTab
            // 
            this.trackEditorWebTab.AllowDrop = true;
            this.trackEditorWebTab.BackColor = System.Drawing.SystemColors.Control;
            this.trackEditorWebTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trackEditorWebTab.IsPro = false;
            this.trackEditorWebTab.Location = new System.Drawing.Point(13, 31);
            this.trackEditorWebTab.Margin = new System.Windows.Forms.Padding(0);
            this.trackEditorWebTab.Name = "trackEditorWebTab";
            this.trackEditorWebTab.SelectedDifficulty = GuitarDifficulty.Expert;
            this.trackEditorWebTab.Size = new System.Drawing.Size(239, 409);
            this.trackEditorWebTab.TabIndex = 0;
            // 
            // peGroupBox1
            // 
            this.peGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.peGroupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peGroupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.peGroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.peGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.peGroupBox1.Name = "peGroupBox1";
            this.peGroupBox1.Size = new System.Drawing.Size(265, 451);
            this.peGroupBox1.TabIndex = 0;
            this.peGroupBox1.TabStop = false;
            this.peGroupBox1.Text = "Web Tab Player";
            // 
            // peGroupBox2
            // 
            this.peGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.peGroupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peGroupBox2.Controls.Add(this.peGroupBox3);
            this.peGroupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.peGroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.peGroupBox2.Location = new System.Drawing.Point(274, 3);
            this.peGroupBox2.Name = "peGroupBox2";
            this.peGroupBox2.Size = new System.Drawing.Size(262, 451);
            this.peGroupBox2.TabIndex = 2;
            this.peGroupBox2.TabStop = false;
            this.peGroupBox2.Text = "Import Tracks";
            // 
            // peGroupBox3
            // 
            this.peGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.peGroupBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peGroupBox3.Controls.Add(this.ProOffset);
            this.peGroupBox3.Controls.Add(this.ProScale);
            this.peGroupBox3.Controls.Add(this.buttonRefresh);
            this.peGroupBox3.Controls.Add(this.ImportTrack);
            this.peGroupBox3.Controls.Add(this.peLabel2);
            this.peGroupBox3.Controls.Add(this.resetScale);
            this.peGroupBox3.Controls.Add(this.peLabel1);
            this.peGroupBox3.Controls.Add(this.AutoRefresh);
            this.peGroupBox3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.peGroupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(213)))));
            this.peGroupBox3.Location = new System.Drawing.Point(11, 299);
            this.peGroupBox3.Name = "peGroupBox3";
            this.peGroupBox3.Size = new System.Drawing.Size(239, 138);
            this.peGroupBox3.TabIndex = 0;
            this.peGroupBox3.TabStop = false;
            this.peGroupBox3.Text = "Track Properties";
            // 
            // ProOffset
            // 
            this.ProOffset.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ProOffset.Location = new System.Drawing.Point(111, 78);
            this.ProOffset.Name = "ProOffset";
            this.ProOffset.Size = new System.Drawing.Size(89, 23);
            this.ProOffset.TabIndex = 5;
            this.ProOffset.Text = "3.0";
            this.ProOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ProOffset.WordWrap = false;
            this.ProOffset.TextChanged += new System.EventHandler(this.ProScale_TextChanged);
            // 
            // ProScale
            // 
            this.ProScale.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ProScale.Location = new System.Drawing.Point(111, 50);
            this.ProScale.Name = "ProScale";
            this.ProScale.Size = new System.Drawing.Size(89, 23);
            this.ProScale.TabIndex = 2;
            this.ProScale.Text = "1.0";
            this.ProScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ProScale.WordWrap = false;
            this.ProScale.TextChanged += new System.EventHandler(this.ProScale_TextChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.Enabled = false;
            this.buttonRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.buttonRefresh.ForeColor = System.Drawing.Color.Black;
            this.buttonRefresh.Image = global::EditorResources.PEResources.gtkrefresh;
            this.buttonRefresh.Location = new System.Drawing.Point(206, 76);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(26, 25);
            this.buttonRefresh.TabIndex = 6;
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.refreshClick);
            // 
            // ImportTrack
            // 
            this.ImportTrack.AutoSize = true;
            this.ImportTrack.BackColor = System.Drawing.Color.Transparent;
            this.ImportTrack.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ImportTrack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ImportTrack.ForeColor = System.Drawing.Color.Black;
            this.ImportTrack.Location = new System.Drawing.Point(106, 25);
            this.ImportTrack.Name = "ImportTrack";
            this.ImportTrack.Size = new System.Drawing.Size(94, 19);
            this.ImportTrack.TabIndex = 0;
            this.ImportTrack.Text = "Import Track";
            this.ImportTrack.UseVisualStyleBackColor = false;
            this.ImportTrack.CheckedChanged += new System.EventHandler(this.ImportTrack_CheckedChanged);
            this.ImportTrack.Validating += new System.ComponentModel.CancelEventHandler(this.ImportTrack_Validating);
            // 
            // peLabel2
            // 
            this.peLabel2.AutoSize = true;
            this.peLabel2.BackColor = System.Drawing.Color.Transparent;
            this.peLabel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peLabel2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.peLabel2.ForeColor = System.Drawing.Color.Black;
            this.peLabel2.Location = new System.Drawing.Point(16, 81);
            this.peLabel2.Name = "peLabel2";
            this.peLabel2.Size = new System.Drawing.Size(89, 15);
            this.peLabel2.TabIndex = 4;
            this.peLabel2.Text = "Offset Seconds:";
            // 
            // resetScale
            // 
            this.resetScale.BackColor = System.Drawing.Color.Transparent;
            this.resetScale.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.resetScale.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.resetScale.ForeColor = System.Drawing.Color.Black;
            this.resetScale.Image = global::EditorResources.PEResources.XPRecycle;
            this.resetScale.Location = new System.Drawing.Point(206, 48);
            this.resetScale.Name = "resetScale";
            this.resetScale.Size = new System.Drawing.Size(26, 25);
            this.resetScale.TabIndex = 3;
            this.resetScale.UseVisualStyleBackColor = false;
            this.resetScale.Click += new System.EventHandler(this.resetScale_Click);
            // 
            // peLabel1
            // 
            this.peLabel1.AutoSize = true;
            this.peLabel1.BackColor = System.Drawing.Color.Transparent;
            this.peLabel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.peLabel1.ForeColor = System.Drawing.Color.Black;
            this.peLabel1.Location = new System.Drawing.Point(68, 53);
            this.peLabel1.Name = "peLabel1";
            this.peLabel1.Size = new System.Drawing.Size(37, 15);
            this.peLabel1.TabIndex = 1;
            this.peLabel1.Text = "Scale:";
            // 
            // AutoRefresh
            // 
            this.AutoRefresh.AutoSize = true;
            this.AutoRefresh.BackColor = System.Drawing.Color.Transparent;
            this.AutoRefresh.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoRefresh.Checked = true;
            this.AutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.AutoRefresh.ForeColor = System.Drawing.Color.DimGray;
            this.AutoRefresh.Location = new System.Drawing.Point(136, 107);
            this.AutoRefresh.Name = "AutoRefresh";
            this.AutoRefresh.Size = new System.Drawing.Size(96, 19);
            this.AutoRefresh.TabIndex = 7;
            this.AutoRefresh.Text = "Auto Preview";
            this.AutoRefresh.UseVisualStyleBackColor = false;
            this.AutoRefresh.CheckedChanged += new System.EventHandler(this.peCheckBox1_CheckedChanged);
            // 
            // PEWebTabImportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.trackEditorPro);
            this.Controls.Add(this.trackEditorWebTab);
            this.Controls.Add(this.peGroupBox1);
            this.Controls.Add(this.peGroupBox2);
            this.Name = "PEWebTabImportPanel";
            this.Size = new System.Drawing.Size(549, 494);
            this.peGroupBox2.ResumeLayout(false);
            this.peGroupBox3.ResumeLayout(false);
            this.peGroupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PEGroupBox peGroupBox1;
        private PEMidiTrackEditPanel trackEditorWebTab;
        private PEGroupBox peGroupBox2;
        private PEMidiTrackEditPanel trackEditorPro;
        private PEButton buttonImport;
        private PEGroupBox peGroupBox3;
        private PELabel peLabel1;
        private EditorResources.Components.PEEditBox ProScale;
        private PEButton resetScale;
        private PECheckBox ImportTrack;
        private PEButton buttonRefresh;
        private PECheckBox AutoRefresh;
        private PEEditBox ProOffset;
        private PELabel peLabel2;
    }
}
