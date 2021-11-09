using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Chatik
{
    class Server
    {
        static TcpListener listener;
        static List<TcpClient> Clients;

        Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        void Server_work()
        {
            listener.Start();

            while (true)
            {
                TcpClient client = new TcpClient();


            }
        }



    }
}
