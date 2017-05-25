using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Javawar
{
    public class ThreadMessage
    {
        public delegate void MyEvent(object sender, MyEventArgs e);
        public event MyEvent counting;
        public event MyEvent countEnd;
        public event MyEvent searchFilesing;
        public event MyEvent inserting;
        public event MyEvent insertEnd;
        public event MyEvent searchEnd;
        public event MyEvent searchNewing;
        public event MyEvent searchVersioning;
        private List<SqlParameter[]> lsp;
        private MyEventArgs e;
        private int filesCount;

        public string insertSql { get; set; }
        public DirectoryInfo di { get; set; }
        public ThreadMessage()
        {
            this.di = new DirectoryInfo(R.path001);
            lsp = new List<SqlParameter[]>();
        }
        public ThreadMessage(DirectoryInfo di)
        {
            this.di = di;
            lsp = new List<SqlParameter[]>();
        }
     
        public void insertDatabase()
        {
            insertDatabase(insertSql);
        }
        public void insertDatabase(string sql)
        {
            dbHelper dbh = new dbHelper();
            dbh.excuteing += Dbh_excuteing;
            dbh.excuteEnd += Dbh_excuteEnd;
            dbh.excuteSql(sql, lsp);
        }

        private void Dbh_excuteEnd(object obj,MyEventArgs e)
        {
            if (insertEnd != null)
            {
                insertEnd(this, e);
            }
        }

        private void Dbh_excuteing(object obj, MyEventArgs e)
        {
            if (inserting != null)
            {
                inserting(obj, e);
            }
        }
        
        public void countFiles()
        {
            countFiles(di);
        }
        public void countFiles(DirectoryInfo di)
        {
            e = new MyEventArgs();
            _countFiles(di);
            if (countEnd != null)
            {
                countEnd(this, e);
            }
        }
        private void _countFiles(DirectoryInfo di)
        {
            filesCount += di.GetFiles().Length;
            e.count = filesCount;
            if (counting != null)
            {
                counting(this,e);
            }
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                e.index++;
                _countFiles(d);
            }
        }

        public void searchFiles()
        {
            searchFiles(this.di);
        }
        public void searchFiles(object obj)
        {
            searchFiles(obj as DirectoryInfo);
        }
        public void searchFiles(DirectoryInfo di)
        {
            e = new MyEventArgs();
            e.count = this.filesCount;
            _searchFiles(di);
            if (searchEnd != null)
            {
                searchEnd(this, e);
            }
        }
        private void _searchFiles(DirectoryInfo di)
        {
            
            foreach (FileInfo fi in di.GetFiles())
            {
                SqlParameter[] sqlpar = new SqlParameter[5];
                sqlpar[0] = new SqlParameter("@path", fi.DirectoryName);
                sqlpar[1] = new SqlParameter("@filename", fi.Name);
                sqlpar[2] = new SqlParameter("@filesize", fi.Length);
                sqlpar[3] = new SqlParameter("@createdate", fi.CreationTime);
                sqlpar[4] = new SqlParameter("@modifydate", fi.LastWriteTime);
                lsp.Add(sqlpar);
                e.index++;
                e.fileInfo = fi;
                if (searchFilesing != null)
                {
                    searchFilesing(this, e);
                }
            }
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                _searchFiles(d);
            }

        }
        public void searchNew()
        {
            
            dbHelper dbh = new dbHelper();
            dbh.get4DataReadering += Dbh_get4DataReadering;
            dbh.get4DataReader(R.sql00003);
        }
        public void searchVersion()
        {

            dbHelper dbh = new dbHelper();
            dbh.get4DataReadering += Dbh_get4DataReadering1;
            dbh.get4DataReader(R.sql00004);
        }

        private void Dbh_get4DataReadering1(object obj, MyEventArgs e)
        {
            if (searchVersioning != null)
            {
                searchVersioning(obj, e);
            }
        }

        private void Dbh_get4DataReadering(object obj,MyEventArgs e)
        {
            if (searchNewing != null)
            {
                searchNewing(obj, e);
            }
        }

        public void copy2Temp(List<string> list)
        {
           
            foreach (string strPath in list)
            {
                FileInfo fi = new FileInfo(strPath);
                if (fi.Exists)
                {
                    string p = strPath.Replace(R.path001, R.path);
                    FileInfo f = new FileInfo(p);
                    if (!f.Exists)
                    {
                        f.Directory.Create();
                        f.Create().Close();
                        
                    }
                    fi.CopyTo(p,true);
                }
            }
        }
        public void copy2Temp(List<FileInfo> list,string seacherPath)
        {

            foreach (FileInfo fi in list)
            {
                if (fi.Exists)
                {
                    string p = Properties.Settings.Default.tempPath+fi.FullName.Replace(seacherPath, "");
                    FileInfo f = new FileInfo(p);
                    if (!f.Exists)
                    {
                        f.Directory.Create();
                        f.Create().Close();

                    }
                    fi.CopyTo(p, true);
                }
            }
        }
    }
}
