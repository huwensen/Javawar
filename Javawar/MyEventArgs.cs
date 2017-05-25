using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javawar
{
    public class MyEventArgs : EventArgs
    {
        public int index { get; set; }
        public int count { get; set; }
        public FileInfo fileInfo { get; set; }
        public MyEventArgs()
        {

        }
        public MyEventArgs(int index)
        {
            this.index = index;
        }
        public MyEventArgs(int index, int count)
        {
            this.index = index;
            this.count = count;
        }
    }
}
