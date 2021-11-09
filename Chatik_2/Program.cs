using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Chatik_2
{
    class Program
    {
        private static async Task Main(string[] args)
        {


            if (args.Length > 2)
            {
                Console.WriteLine("Two Arguments are required:\n please enter IP adress and a port for Client or just a port ");
                Environment.Exit(0);
            }
            else if (args.Length == 1)
            {
                int port;
                int.TryParse(args[0], out port);

                Server server = new Server(port);
                await server.Server_work();
            }

            else if (args.Length == 2)
            {
                IPAddress.TryParse(args[0], out IPAddress IP);
                int.TryParse(args[1], out int port);

                Console.WriteLine("Enter your name");

                string name = Console.ReadLine();

                Client client = new Client(port, IP, name);
                await client.Start();
            }



        }
    }
}
