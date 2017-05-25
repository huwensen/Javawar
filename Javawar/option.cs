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
    public partial class option : Form
    {
        public option()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.tempPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.tempPath = textBox1.Text;
            Properties.Settings.Default.classPath = textBox2.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
