using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProUpgradeEditor.Common;

namespace EditorResources.Components
{
    public partial class PEPopupWindow : Form
    {
        public PEPopupWindow()
            : base()
        {
            InitializeComponent();
        }

        Control currentControl;
        public TrackEditor EditorPro;
        public void SetControl(Control ctrl, TrackEditor editorPro)
        {
            this.EditorPro = editorPro;
            if (currentControl != null)
            {
                if (Controls.Contains(currentControl))
                {
                    Controls.Remove(currentControl);
                }
                currentControl = null;
            }
            
            currentControl = ctrl;

            Controls.Add(currentControl);
            ctrl.Parent = this;
            this.ClientSize = new Size(currentControl.Width, currentControl.Height);
        }

        public void Close(DialogResult result) { this.DialogResult = result; Close(); }
    }
}
