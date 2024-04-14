using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerProject
{
    class Program
    {
        static Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            string clientId = null;
            string message;

            try
            {
                while (true)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    if (clientId == null)
                    {
                        clientId = message;
                        clients.Add(clientId, client);
                        Console.WriteLine("Client connected with ID: " + clientId);
                    }
                    else
                    {
                        string[] data = message.Split('|');
                        if (data.Length == 2 && clients.ContainsKey(data[0]))
                        {
                            TcpClient destClient = clients[data[0]];
                            NetworkStream destStream = destClient.GetStream();
                            byte[] bytesToSend = Encoding.ASCII.GetBytes(data[1]);
                            destStream.Write(bytesToSend, 0, bytesToSend.Length);
                            destStream.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (!string.IsNullOrEmpty(clientId))
                {
                    clients.Remove(clientId);
                    Console.WriteLine("Client disconnected: " + clientId);
                }
                client.Close();
            }
        }
    }
}
