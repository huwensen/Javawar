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
    public partial class Form3 : Form
    {
        //string dirStr = @"C:\Users\joker\Desktop\e-future\DS.201503.CRV\CRV_ACC_Tool";
        DirectoryInfo di;
        public Form3()
        {
            InitializeComponent(); dataGridView1.Columns.Add("path", "路径");
            dataGridView1.Columns.Add("filename", "文件名");
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;
            //di = new DirectoryInfo(dirStr);
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
            e.RowBounds.Location.Y,
            dataGridView1.RowHeadersWidth - 4,
            e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            di = new DirectoryInfo(textBox2.Text.Trim());
            dataGridView1.Rows.Clear();
            ThreadMessage tm = new ThreadMessage();
            tm.searchFilesing += Tm_searchFilesing;
            tm.searchEnd += Tm_searchEnd;
            Thread t = new Thread(new ParameterizedThreadStart(tm.searchFiles));
            t.Start(di);
        }

        private void Tm_searchEnd(object sender, MyEventArgs e)
        {
            if (label1.InvokeRequired)
            { Invoke(new ThreadMessage.MyEvent(Tm_searchEnd), new object[] { sender, e }); }
            else
            { label1.Text = "查找结束"; }
        }

        private void Tm_searchFilesing(object sender, MyEventArgs e)
        {
            if (dataGridView1.InvokeRequired&& label1.InvokeRequired)
            {
                Invoke(new ThreadMessage.MyEvent(Tm_searchFilesing), new object[] { sender, e });
            }
            else
            {
                FileInfo f = e.fileInfo;
                label1.Text = f.Name;
                StreamReader sr = null;
                try
                {
                   sr = new StreamReader(f.OpenRead());
                    string strTemp = "";
                    while ((strTemp = sr.ReadLine()) != null)
                    {
                        if (strTemp.Contains(textBox1.Text.Trim()))
                        {
                            dataGridView1.Rows.Add(new object[] { f.DirectoryName, f.Name });
                        }
                    }
                    sr.Close();
                }
                catch {
                    return;
                }
            }
        }
    }
}
