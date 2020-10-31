using ServerLoginLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IO_2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerLogin server = new ServerLoginAPM(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
        }
    }
}
