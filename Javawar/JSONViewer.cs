using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPocalipse.Json.Viewer;

namespace Javawar
{
    public partial class JSONViewer : Form
    {
        public JSONViewer()
        {
            InitializeComponent();
            jsonViewer1.ShowTab(Tabs.Text);
        }
    }
}
