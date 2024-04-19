using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataBase;

namespace ServerProject
{
    class Program
    {
        static ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();
        static ConcurrentDictionary<int, int> groupClients = new ConcurrentDictionary<int, int>();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            Console.WriteLine("Server started...");

            using (var data = new DataBaseDbContext())
            {
                var list = data.GroupMemberships.ToList();

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        groupClients.TryAdd(item.UserGMID, item.GroupGMID);
                    }
                }

            }

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        static async Task HandleClient(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                string clientId = null;
                string message;

                try
                {
                    while (true)
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        if (clientId == null)
                        {
                            clientId = message;
                            clients.TryAdd(clientId, client);
                            Console.WriteLine("Client connected with ID: " + clientId);
                        }
                        else
                        {
                            if (message.StartsWith("g"))
                            {
                                int pipeIndex = message.IndexOf("|");
                                string group = message.Substring(1, pipeIndex - 1);
                                message = message.Substring(pipeIndex + 1);

                                pipeIndex = message.IndexOf("|");
                                string id = message.Substring(0, pipeIndex);
                                string msg = message.Substring(pipeIndex + 1);
                                int groupId = Convert.ToInt32(group);

                                await BroadcastToGroup(groupId, id, message);
                            }
                            else if (message.StartsWith("p"))
                            {
                                int pipeIndex = message.IndexOf("|");
                                string recipientId = message.Substring(1, pipeIndex - 1);
                                string msg = message.Substring(pipeIndex + 1);
                                if (clients.ContainsKey(recipientId))
                                {
                                    TcpClient destClient = clients[recipientId];
                                    NetworkStream destStream = destClient.GetStream();

                                    byte[] bytesToSend = Encoding.ASCII.GetBytes("p|" + recipientId + "|" + msg);
                                    await destStream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                                    await destStream.FlushAsync();
                                }
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
                        clients.TryRemove(clientId, out _);
                        Console.WriteLine("Client disconnected: " + clientId);
                    }
                }
            }
        }

        static async Task BroadcastToGroup(int groupId, string id, string message)
        {
            if (groupClients.ContainsKey(groupId))
            {
                var clientIdsInGroup = groupClients.Where(x => x.Value == groupId).Select(x => x.Key);

                foreach (var clientId in clientIdsInGroup)
                {
                    if (clientId.ToString() != id && clients.ContainsKey(clientId.ToString()))
                    {
                        TcpClient destClient = clients[clientId.ToString()];

                        NetworkStream destStream = destClient.GetStream();

                        byte[] bytesToSend = Encoding.ASCII.GetBytes($"g{groupId}|{id}|{message}");
                        await destStream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                        await destStream.FlushAsync();
                    }
                }
            }
        }
    }
}
