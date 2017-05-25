using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Javawar
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            toolInitSet();
        }
        void toolInitSet()
        {
            ricBox.WordWrap = Properties.Settings.Default.WordWrap;
            if (Properties.Settings.Default.WordWrap)
            {
                自动换行ToolStripMenuItem.Image = Properties.Resources.selected002;
            }
            else
            {
                自动换行ToolStripMenuItem.Image = null;
            }
        }
        private void singleLine_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string str = ricBox.Text;
            char c1;
            foreach (char c in str.ToCharArray())
            {
                if (c == 10)
                {
                    continue;
                }
                c1 = c;
                sb.Append(c);
            }
            ricBox.Text = sb.ToString();
        }
        
        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.WordWrap = !Properties.Settings.Default.WordWrap;
            Properties.Settings.Default.Save();
            toolInitSet();
        }
    }
}
