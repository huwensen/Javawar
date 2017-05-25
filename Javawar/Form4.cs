using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Javawar
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        [DllImport("JsonSign32.dll", EntryPoint = "jsonSign")]
        static extern int jsonSign(string jsonStr, [MarshalAs(UnmanagedType.LPStr)] StringBuilder sign);

        public string jsonSign()
        {
            StringBuilder sign = new StringBuilder();
            jsonSign(richTextBox1.Text, sign);
            return sign.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0;i<1000;i++)
            { 
                sb.Append( jsonSign()+"\r\n");
                //textBox2.Text = MD5Encrypt( MD5Encrypt(richTextBox1.Text));
            }
            textBox1.Text = sb.ToString();

        }
        public static string MD5Encrypt(string strText)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X"));
            }
            return sb.ToString();
        }
    }
}
