using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProUpgradeEditor.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://rockband.scorehero.com/forum/viewtopic.php?t=34538");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://rockbandscores.com/scores.cgi?player=ziggy+xna+mvp&instrument=PROGUITAR&platform=Xbox&showall=on&showrb3=on&showrb2=on&showrb1=on&showlego=on&showdlc=on&showrbn=on");

        }
    }
}
