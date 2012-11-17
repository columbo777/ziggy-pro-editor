using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProUpgradeEditor.UI;

namespace ProUpgradeEditor
{
    public static class Program
    {
        
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
           
            Application.Run(new MainForm());
            
        }
    }
}
