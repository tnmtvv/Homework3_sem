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
    class Client
    {

        
            TcpClient client { get; set; }
            string Name { get; set; }
            string Login { get; set; }

            private readonly int Port;
            private readonly IPAddress Ip;
            private TcpClient My_client;

            public Client(int port, IPAddress ip, string name)
            {
                Port = port;
                Ip = ip;
                My_client = new TcpClient();
                Name = name;
            }


            public async Task Start()
            {
                await client.ConnectAsync(Ip, Port);
                Console.WriteLine("Client Start");

                string cur_string = "";

                while (cur_string != "exit")
                {
                    cur_string = await Read();
                    if (cur_string == "exit") break;
                    cur_string = await Write();
                }

                client.Close();
            }


            public async Task<string> Read()
            {
                StreamReader sr = new StreamReader(client.GetStream());
                string new_Message_from_Server = await sr.ReadLineAsync();

                Console.WriteLine($"Server said :{new_Message_from_Server} ");
                return new_Message_from_Server;
            }

            public async Task<string> Write()
            {

                StreamWriter sw = new StreamWriter(client.GetStream()) { AutoFlush = true };

                Console.WriteLine(this.Name + ": ");
                string new_Message_from_Client = Console.ReadLine();
                await sw.WriteLineAsync(new_Message_from_Client);

                return new_Message_from_Client;
            }

        }
    }

