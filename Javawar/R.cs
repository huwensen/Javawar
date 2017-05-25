using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javawar
{
    public class R
    {
        public static string sql00001 = @"insert into filesinfo([path],[filename],filesize,createdate,modifydate)values(@path,@filename,@filesize,@createdate,@modifydate)";
        public static string sql00002 = @"insert into filesinfo_temp([tempId],[path],[filename],filesize,createdate,modifydate)values('" + Guid.NewGuid().ToString() + "',@path,@filename,@filesize,@createdate,@modifydate)";
        public static string sql00003 = @"select [path] ,[filename],[filesize],[createdate] ,[modifydate] from filesinfo_temp
                            except
                            select [path] ,[filename],[filesize],[createdate] ,[modifydate] from filesinfo";
        public static string sql00004 = @"select [path] ,[filename],[filesize],[createdate] ,[modifydate] from filesinfo_version";
        public static string sql00005 = @"";
        public static string sql00006 = @"";
        public static string sql00007 = @"";
        public static string sql00008 = @"";
        public static string sql00009 = @"";
        public static string sql00010 = @"";


        public static string path = System.IO.Directory.GetCurrentDirectory()+"\\temp_file";

        public static string path001
        {
            get
            {
                //if (path001 != null)
                //{
                //    return path001;
                //}
                //else
                {
                    return @"C:\Users\joker\Desktop\e-future\DS.201503.CRV\CRV_HQ_VSM\WebContent";
                }
            }
            set { path001 = value; }
        }


    }
}
