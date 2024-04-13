using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;

namespace ServerProject
{
    class ClientInServerProject
    {
        public string UserName { get; set; }
        public int UID { get; set; }
        PacketReader packet;
        public TcpClient ClientSocket { get; set; }
        public ClientInServerProject(TcpClient client)
        {
            User user = new User();

            ClientSocket = client;

            packet = new PacketReader(ClientSocket.GetStream());

            var opcode = packet.ReadByte();
            UserName = packet.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: Client - {UserName} - has connected");

            Task.Run(() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = packet.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = packet.ReadMessage();
                            ProgramInServerProject.BroadcastMessage(msg);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ProgramInServerProject.BroadcastDisconnected(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
