using System;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp20
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }
        public string ReadMessage()
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;
        }
    }
}
