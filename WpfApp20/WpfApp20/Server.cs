using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp20
{
    class Server
    {
        TcpClient client;
        public PacketReader packet;

        public event Action connectedEvent;
        public event Action messageReceivedEvent;
        public event Action UserDisconnectedEvent;
        public Server()
        {
            client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!client.Connected)
            {
                client.Connect("127.0.0.1", 7891);
                packet = new PacketReader(client.GetStream());

                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(username);
                client.Client.Send(connectPacket.GetPacketBytes());

                ReadPackets();
            }
        }
        void ReadPackets()
        {
            Task.Run(() => {
                while (true)
                {
                    var opcode = packet.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            messageReceivedEvent?.Invoke();
                            break;
                        case 10:
                            UserDisconnectedEvent?.Invoke();
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string msg)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(msg);
            client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
