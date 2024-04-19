using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;

namespace WpfApp20
{
    public class Program
    {
        public event EventHandler<string> MessageReceived;
        DataBaseDbContext data = new DataBaseDbContext();
        static string UserId;
        TcpClient client;
        NetworkStream stream;

        public Program(string userId)
        {
            UserId = userId;
            client = new TcpClient("localhost", 8888);
            stream = client.GetStream();
            byte[] clientIdBytes = Encoding.ASCII.GetBytes(UserId);
            stream.Write(clientIdBytes, 0, clientIdBytes.Length);
            Task.Run(async () => await ReceiveMessages());
        }

        public async Task SendMessage(string recId, string msg)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(recId + "|" + msg);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
        public async Task SendMessageGroup(string group, string id, string msg)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes($"g{group}|{id}|{msg}");
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
        private async Task ReceiveMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (message.StartsWith("g"))
                {
                    int pipeIndex = message.IndexOf("|");
                    string group = message.Substring(1, pipeIndex - 1);
                    message = message.Substring(pipeIndex + 1);

                    pipeIndex = message.IndexOf("|");
                    string id = message.Substring(0, pipeIndex);
                    string msg = message.Substring(pipeIndex + 1);
                    MessageReceived?.Invoke(this, msg);
                }
                else
                {
                    MessageReceived?.Invoke(this, message);
                }
            }
        }
    }
}