using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Javawar
{
    public partial class Form2 : Form
    {
        string dirStr = "C:\\Users\\joker\\Desktop\\VSS WORK\\提交程序";
        DirectoryInfo di;
        public Form2()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("path", "路径");
            dataGridView1.Columns.Add("filename", "文件名");
            di = new DirectoryInfo(dirStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ThreadMessage tm = new ThreadMessage();
            tm.searchFilesing += Tm_searchFilesing;
            Thread t = new Thread(new ParameterizedThreadStart(tm.searchFiles));
            t.Start(di);
        }

        private void Tm_searchFilesing(object sender, MyEventArgs e)
        {
            if (dataGridView1.InvokeRequired)
            {
                Invoke(new ThreadMessage.MyEvent(Tm_searchFilesing), new object[] { sender, e });
            }
            else
            {
                dataGridView1.Rows.Add(new object[] { e.fileInfo.DirectoryName, e.fileInfo.Name });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(di.FullName + "\\aa.txt");
            FileStream fs;
            StreamWriter sw;
            if (!fi.Exists)
            {
                fs = fi.Create();
            }
            else
            {
                fs = fi.OpenWrite();
            }
            sw = new StreamWriter(fs);
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                sw.WriteLine(dgvr.Cells[1].Value+"");
            }
            
            sw.Flush();
            fs.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
