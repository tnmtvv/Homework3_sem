using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Chatik
{
    class Server
    {
        static TcpListener listener;
        static TcpClient client;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task Server_work()
        {
            listener.Start();
            Socket connection;
            Console.WriteLine("Server Start");
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                client = new TcpClient();
                connection = listener.AcceptSocket();
                

                string cur_string = "";

                while (cur_string != "exit")
                {
                    cur_string = await Read();
                    if (cur_string == "exit") break;
                    else Console.WriteLine($"Client said :{cur_string} ");
                    cur_string = await Write();
                    if (cur_string == "exit") break;
                    else Console.WriteLine($"You said :{cur_string} ");
                }

                client.Close();
            }
            listener.Stop();
            Environment.Exit(0);
           
        }


        public async Task<string> Read()
        {
            StreamReader sr = new StreamReader(client.GetStream());
            string new_Message_from_Client = await sr.ReadLineAsync();

            
            return new_Message_from_Client;
        }

        public async Task<string> Write()
        {

            StreamWriter sw = new StreamWriter(client.GetStream()) { AutoFlush = true };

            Console.WriteLine("Your message: ");
            string new_Message_from_Server = Console.ReadLine();
            await sw.WriteLineAsync(new_Message_from_Server);

            return new_Message_from_Server;
        }



    }
}
