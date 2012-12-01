namespace EditorResources.Components
{
    partial class PEWaveViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.peWaveViewerControl1 = new EditorResources.Components.PEWaveViewerControl(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.peWaveViewerControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 351);
            this.panel1.TabIndex = 0;
            // 
            // peWaveViewerControl1
            // 
            this.peWaveViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peWaveViewerControl1.Location = new System.Drawing.Point(0, 0);
            this.peWaveViewerControl1.Name = "peWaveViewerControl1";
            this.peWaveViewerControl1.SamplesPerPixel = 128;
            this.peWaveViewerControl1.Size = new System.Drawing.Size(583, 351);
            this.peWaveViewerControl1.StartPosition = ((long)(0));
            this.peWaveViewerControl1.TabIndex = 0;
            this.peWaveViewerControl1.Load += new System.EventHandler(this.peWaveViewerControl1_Load);
            this.peWaveViewerControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.peWaveViewerControl1_MouseDown);
            this.peWaveViewerControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.peWaveViewerControl1_MouseMove);
            this.peWaveViewerControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.peWaveViewerControl1_MouseUp);
            // 
            // PEWaveViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 351);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "PEWaveViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PEPopupWindow";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private PEWaveViewerControl peWaveViewerControl1;
    }
}