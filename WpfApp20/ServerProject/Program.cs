using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ServerProject
{
    class ProgramInServerProject
    {
        static TcpListener listener;
        static List<ClientInServerProject> clients;
        static void Main(string[] args)
        {
            clients = new List<ClientInServerProject>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            listener.Start();

            while (true)
            {
                var client = new ClientInServerProject(listener.AcceptTcpClient());
                clients.Add(client);

                BroadcastConnection();
            }
        }
        static void BroadcastConnection()
        {
            foreach (var user in clients)
            {
                foreach (var usr in clients)
                {
                    var broudcastPacket = new PacketBuilder();
                    broudcastPacket.WriteOpCode(1);
                    broudcastPacket.WriteString(usr.UserName);
                    broudcastPacket.WriteString(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broudcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string msg)
        {
            foreach (var user in clients)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteString(msg);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnected(string uid)
        {
            var discUser = clients.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            clients.Remove(discUser);
            foreach (var user in clients)
            {
                var brPacket = new PacketBuilder();
                brPacket.WriteOpCode(10);
                brPacket.WriteString(uid);
                user.ClientSocket.Client.Send(brPacket.GetPacketBytes());
            }
        }
    }
}
