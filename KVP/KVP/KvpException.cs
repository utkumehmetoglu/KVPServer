using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Kvp
{
    class KvpException : Exception
    {
        public KvpException() { }

        public KvpException(string message) : base(String.Format("Kvp Exception: {0}" , message)) { }

    }
    class KvpSocketException : Exception
    {
        public KvpSocketException() { }

        public KvpSocketException(string message) : base(String.Format("Kvp Socket Exception: {0}",message)) { }
    }
}
