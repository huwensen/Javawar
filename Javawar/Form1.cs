using System;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using System.Runtime.InteropServices;

namespace Javawar
{
    public partial class Form1 : Form
    {
        List<FileInfo> listFi = new List<FileInfo>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("filename", "文件名");
            dataGridView1.Columns.Add("filesize", "大小");
            dataGridView1.Columns.Add("createdate", "创建时间");
            dataGridView1.Columns.Add("modifydate", "修改时间");
            dataGridView1.Columns.Add("fileobj", "文件OBJ");
            dataGridView1.Columns.Add("path", "路径");
            textBox1.Text = (new DateTime()).Date.ToString();
            //loadVersion();
            System.Collections.Specialized.StringEnumerator se= Properties.Settings.Default.hispath.GetEnumerator();
            while (se.MoveNext())
            {
                comboBox1.Items.Add(se.Current);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ButtonEnabled(false);

            ThreadMessage tm = new ThreadMessage();
            tm.countEnd += Tm_countEnd;
            tm.counting += Tm_counting;
            Thread thCountFiles = new Thread(new ThreadStart(tm.countFiles));
            thCountFiles.Start();

        }

        private void Tm_counting(object sender, MyEventArgs e)
        {
            if (progressBarInvoke())
            {
                Invoke(new ThreadMessage.MyEvent(Tm_counting), new object[] { sender, e });
            }
            else
            {
                setProgressBar(1, 1, e.count);
            }
        }

        private void Tm_countEnd(object sender, MyEventArgs e)
        {
            //if (progressBarInvoke())
            //{
            //    Invoke(new ThreadMessage.MyEvent(Tm_countEnd), new object[] { sender, e });
            //}
            //else
            //{
            //    setProgressBar(e.count, 0, "");
            //}
            ThreadMessage tm = sender as ThreadMessage;
            tm.searchFilesing += Tm_searchFilesing;
            tm.searchEnd += init_Tm_searchEnd;
            tm.searchFiles();

        }

        private void Tm_searchFilesing(object sender, MyEventArgs e)
        {
            try
            {
                if (progressBarInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(Tm_searchFilesing), new object[] { sender, e });
                }
                else
                {
                    setProgressBar(e.count, e.index, "正在搜索文件。。。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化查询结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void init_Tm_searchEnd(object sender, MyEventArgs e)
        {
            ThreadMessage tm = sender as ThreadMessage;
            tm.insertSql = R.sql00001;
            tm.insertEnd += init_Tm_insertEnd;
            tm.inserting += Tm_inserting;
            tm.insertDatabase();
        }
     
        private void Tm_inserting(object sender, MyEventArgs e)
        {
            try
            {
                if (progressBarInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(Tm_inserting), new object[] { sender, e });
                }
                else
                {
                    setProgressBar(e.count,e.index, "正在存储数据。。。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void init_Tm_insertEnd(object sender, EventArgs e)
        {
            try
            {
                if (ButtonInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(init_Tm_insertEnd), new object[] { sender, e });
                }
                else
                {
                    ButtonEnabled(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ButtonEnabled(false);

            ThreadMessage tm = new ThreadMessage();
            tm.insertSql = R.sql00002;
            tm.searchEnd += searchNew_Tm_searchEnd;
            tm.insertEnd += searchNew_Tm_insertEnd;
            Thread thCountFiles = new Thread(new ThreadStart(tm.countFiles));
            thCountFiles.Start();

        }
        /// <summary>
        /// 查找新版结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchNew_Tm_searchEnd(object sender, MyEventArgs e)
        {
            Thread thinsertDatabase = new Thread(new ThreadStart(((ThreadMessage)sender).insertDatabase));
            thinsertDatabase.Start();
        }
        private void searchNew_Tm_insertEnd(object sender, EventArgs e)
        {
            try
            {
                if (ButtonInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(searchNew_Tm_insertEnd), new object[] { sender, e });
                }
                else
                {
                    ButtonEnabled(true);
                    ThreadMessage tm = new ThreadMessage();
                    tm.searchNewing += Tm_searchNewing;
                    Thread thget4DataReader = new Thread(new ThreadStart(tm.searchNew));
                    thget4DataReader.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Tm_searchNewing(object sender, MyEventArgs e)
        {
            if (dataGridView1.InvokeRequired && progressBarInvoke())
            {
                Invoke(new ThreadMessage.MyEvent(Tm_searchNewing), new object[] { sender, e });
            }
            else
            {
                SqlDataReader sdr = sender as SqlDataReader;
                setProgressBar(e.count, e.index, "读取记录。。。");
                dataGridView1.Rows.Add(new object[] {  sdr.GetString(1), sdr.GetDecimal(2), sdr.GetDateTime(3), sdr.GetDateTime(4),sdr.GetString(0) });
            }
        }


        private bool ButtonInvoke()
        {
            return button2.InvokeRequired && button3.InvokeRequired && button4.InvokeRequired && button5.InvokeRequired && button1.InvokeRequired;
        }
        private void ButtonEnabled(bool b)
        {
            button1.Enabled = b;
            button2.Enabled = b;
            button3.Enabled = b;
            button4.Enabled = b;
            button5.Enabled = b;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            ThreadMessage tm = new ThreadMessage();
            tm.searchNewing += Tm_searchNewing;
            Thread thget4DataReader = new Thread(new ThreadStart(tm.searchNew));
            thget4DataReader.Start();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.RowIndex < 0)
            {
                return;
            }
            FileInfo fi = (FileInfo)dgv.Rows[e.RowIndex].Cells[4].Value;
            if (fi.Extension.Equals(".java"))
            {
                string strname = fi.FullName;
                string filePath = strname.Replace(comboBox1.Text+"\\src\\", Properties.Settings.Default.classPath).Replace(".java", ".class");
                
                listBox1.Items.Add(dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Replace(".java",".class"));
                listFi.Add(new FileInfo(filePath));
            }
            else
            { 
                listBox1.Items.Add(dgv.Rows[e.RowIndex].Cells[0].Value);
                listFi.Add(fi);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                list.Add(dgr.Cells[5].Value.ToString() + "\\" + dgr.Cells[0].Value.ToString());
            }
            ThreadMessage tm = new ThreadMessage();
            tm.copy2Temp(list);
        }
        private void loadVersion()
        {
            dbHelper a = new dbHelper();
            a.get4DataTableEnd += A_get4DataTableEnd;
            a.get4DataTable("select distinct version from filesinfo_version order by version desc");

        }

        private void A_get4DataTableEnd(object obj, MyEventArgs e)
        {
            toolStripComboBox3.ComboBox.DataSource= obj as DataTable;
            toolStripComboBox3.ComboBox.DisplayMember = "version";
        }


   
        private void Tm_searchVersioning(object sender, MyEventArgs e)
        {
            if (dataGridView1.InvokeRequired && progressBarInvoke())
            {
                Invoke(new ThreadMessage.MyEvent(Tm_searchVersioning), new object[] { sender, e });
            }
            else
            {
                SqlDataReader sdr = sender as SqlDataReader;
                setProgressBar(e.count, e.index, "读取记录。。。");
                dataGridView1.Rows.Add(new object[] {  sdr.GetString(1), sdr.GetDecimal(2), sdr.GetDateTime(3), sdr.GetDateTime(4), sdr.GetString(0) });
            }
        }
        private bool progressBarInvoke()
        {
            return statusStrip1.InvokeRequired;
        }
        private void setProgressBar(object obj)
        {
            toolStripStatusLabel1.Text = obj.ToString();
        }

        private void setProgressBar(int Value, object obj)
        {
            setProgressBar(-1, Value, obj);
        }
        private void setProgressBar(int Maximum, int Value, object obj)
        {
            if (Maximum > 0)
            {
                toolStripProgressBar1.Maximum = Maximum;
            }
            toolStripProgressBar1.Value = Value;
            if (obj.GetType().Equals(typeof(string)))
            {
                toolStripStatusLabel1.Text = obj.ToString() + (int)((toolStripProgressBar1.Value / Convert.ToDouble(toolStripProgressBar1.Maximum)) * 100) + "%";
            }
            else
            {
                toolStripStatusLabel1.Text = obj.ToString();
            }
        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            ThreadMessage tm = new ThreadMessage();
            tm.searchVersioning += Tm_searchVersioning;
            Thread t = new Thread(new ThreadStart(tm.searchVersion));
            t.Start();
        }

        private void 生成提交清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form2();
            f.ShowDialog();
        }

        private void 查找文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form3();
            f.Show();
        }

        private void btn_jsonSign_Click(object sender, EventArgs e)
        {
            
        }
        

        private void jsonSignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form4();
            f.Show();
        }

        private void stringFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form5();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") return;
            dataGridView1.Rows.Clear();
           
            ButtonEnabled(false);
            ThreadMessage tm = new ThreadMessage(new DirectoryInfo(comboBox1.Text));
            tm.countEnd += Tm_countEnd1;
            tm.counting += Tm_counting;
            Thread thCountFiles = new Thread(new ThreadStart(tm.countFiles));
            thCountFiles.Start();

        }


        private void Tm_countEnd1(object sender, MyEventArgs e)
        {
            ThreadMessage tm = sender as ThreadMessage;
            tm.searchFilesing += Tm_searchFilesing1; ;
            tm.searchEnd += init_Tm_searchEnd1;
            tm.searchFiles();

        }
        
        private void Tm_searchFilesing1(object sender, MyEventArgs e)
        {
            try
            {
                if (dataGridView1.InvokeRequired && progressBarInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(Tm_searchFilesing1), new object[] { sender, e });
                }
                else
                {
                    setProgressBar(e.count, e.index, "正在搜索文件。。。");
                    if (!e.fileInfo.Extension.Equals(".class"))
                    {
                        dataGridView1.Rows.Add(new object[] { e.fileInfo.Name, e.fileInfo.Length, e.fileInfo.CreationTime, e.fileInfo.LastWriteTime, e.fileInfo, e.fileInfo.DirectoryName });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void init_Tm_searchEnd1(object sender, MyEventArgs e)
        {
            try
            {
                if (ButtonInvoke())
                {
                    Invoke(new ThreadMessage.MyEvent(init_Tm_searchEnd1), new object[] { sender, e });
                }
                else
                {
                    ButtonEnabled(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                if (dgr.Cells[0].Value.ToString().Contains(textBox2.Text.Trim()))
                {
                    dgr.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = dgr.Index;
                    break;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ThreadMessage tm = new ThreadMessage();
            tm.copy2Temp(listFi,comboBox1.Text);
            setProgressBar("Copy done");
        }
        int currentSelectedIndex = -1;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (sender as ComboBox);
            currentSelectedIndex = cb.SelectedIndex;
            if (currentSelectedIndex == 0)
            {
                
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult  result=fbd.ShowDialog();
                if (result == DialogResult.OK)
                { 

                    cb.Items.Add(fbd.SelectedPath);
                    cb.SelectedIndex = cb.Items.Count-1;
                    Properties.Settings.Default.hispath.Add(fbd.SelectedPath);
                    Properties.Settings.Default.Save();

                }
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            ComboBox cb = (sender as ComboBox);
            if (cb.Text == "")
            {
                Properties.Settings.Default.hispath.Remove(cb.Items[currentSelectedIndex].ToString());
                Properties.Settings.Default.Save();
                cb.Items.RemoveAt(currentSelectedIndex);
            }
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new option();
            f.ShowDialog();
        }

        private void jSONViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new JSONViewer();
            f.Show();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (dgv.Rows[e.RowIndex].Selected == false)
                {
                    dgv.ClearSelection();
                    dgv.Rows[e.RowIndex].Selected = true;
                }
                //contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridView dgv = dataGridView1;
            var row = dgv.SelectedRows;
            if (row != null)
            {
                FileInfo fi = (FileInfo)row[0].Cells[4].Value;
                if (e.ClickedItem.Equals((sender as ContextMenuStrip).Items[0]))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/select," + fi.FullName;
                    System.Diagnostics.Process.Start(psi);
                }
                else if (e.ClickedItem.Equals((sender as ContextMenuStrip).Items[1]))
                {
                    string strname = fi.FullName;
                    string filePath = strname.Replace(comboBox1.Text + "\\src\\", Properties.Settings.Default.classPath).Replace(".java", ".class");
                    if (!fi.Extension.Equals(".java"))
                    {
                        filePath = fi.FullName;
                    }
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/select," + filePath;
                    System.Diagnostics.Process.Start(psi);
                }
            }
            

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListBox listBox = (sender as ListBox);
            listFi.RemoveAt(listBox.SelectedIndex);
            listBox.Items.Remove(listBox.SelectedItem);
            
        }
    }
}
