using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Javawar
{
    public class dbHelper

    {
        public delegate void MyEvent(object obj,MyEventArgs e);
        public event MyEvent excuteing;
        public event MyEvent excuteEnd;
        public event MyEvent get4DataTableEnd;
        public event MyEvent get4DataReadering;
        public event MyEvent get4DataReaderEnd;

        private static string connStr = "Data Source=JOKER-PC\\SQLEXPRESS;Initial Catalog=javawar;Integrated Security=True;Pooling=False";
        /// <summary>
        /// 执行一条简单的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int excuteSql(string sql)
        {
            SqlConnection conn = null;
            try
            {
                SqlCommand cmd;
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = sql;// 
                return cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 执行多条相同的SQL,开启事务
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlPar"></param>
        /// <returns></returns>
        public int excuteSql(string sql, List<SqlParameter[]> sqlPar)
        {
            SqlConnection conn = null;
            SqlTransaction tran = null;
            try
            {
                int result = 0;
                SqlCommand cmd;
                conn = new SqlConnection(connStr);
                conn.Open();
                tran = conn.BeginTransaction();
                cmd = conn.CreateCommand();
                cmd.Transaction = tran;
                cmd.CommandText = sql;
                foreach (SqlParameter[] par in sqlPar)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(par);
                    result += cmd.ExecuteNonQuery();
                    if (excuteing != null)
                    {
                        excuteing(this,new MyEventArgs(result, sqlPar.Count));
                    }
                }
                tran.Commit();
                excuteEnd(this,new MyEventArgs(result, result));
                return result;

            }
            catch (Exception e)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }

            }
        }
        /// <summary>
        /// 执行多条不同的SQL
        /// </summary>
        /// <param name="sqlDic"></param>
        /// <returns></returns>

        public static int excuteSql(Dictionary<string, SqlParameter[]> sqlDic)
        {
            SqlConnection conn = null;
            try
            {
                int result = 0;
                SqlCommand cmd;
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = conn.CreateCommand();
                Dictionary<string, SqlParameter[]>.Enumerator enu = sqlDic.GetEnumerator();
                while (enu.MoveNext())
                {
                    cmd.CommandText = enu.Current.Key;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(enu.Current.Value);
                    result += cmd.ExecuteNonQuery();
                }
                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void get4DataTable(string sql)
        {
            get4DataTable(sql, null);
        }
        public void get4DataTable(string sql, SqlParameter[] sqlPar)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    if (sqlPar != null)
                    {
                        cmd.Parameters.AddRange(sqlPar);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    if (get4DataTableEnd != null)
                    {
                        get4DataTableEnd(dataTable,new MyEventArgs(-1, dataTable.Rows.Count));
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void get4DataReader(string sql)
        {
            get4DataReader(sql, null);
        }
        public void get4DataReader(string sql, SqlParameter[] sqlPar)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    if (sqlPar != null)
                    {
                        cmd.Parameters.AddRange(sqlPar);
                    }
                    MyEventArgs mea = new MyEventArgs();
                    cmd.CommandText = "select count(1) from (" + sql + ") table_temp";
                    mea.count = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.CommandText = sql;
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        mea.index++;
                        if (get4DataReadering != null)
                        {
                            get4DataReadering(sdr, mea);
                        }
                    }
                    if (get4DataReaderEnd != null)
                    {
                        get4DataReaderEnd(sdr, mea);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
