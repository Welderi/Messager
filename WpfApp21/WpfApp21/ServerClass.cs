using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp21
{
    public class ServerControlCollection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const int Port = 9000;
        private static readonly IPAddress IpAddress = IPAddress.Loopback;
        private static readonly IPEndPoint EndPoint = new IPEndPoint(IpAddress, Port);
        public ObservableCollection<ChatItem> ServerChat { get; set; } = new ObservableCollection<ChatItem>();

        private Socket _listener;

        public async Task StartListening()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(EndPoint);
            _listener.Listen(10);

            while (true)
            {
                using (var client = await _listener.AcceptAsync())
                {
                    await ReceiveMessageFromClient(client);
                }
            }
        }

        private async Task ReceiveMessageFromClient(Socket client)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Application.Current.Dispatcher.Invoke(() => ServerChat.Add(new ChatItem { Item = message }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        public async Task SendMessageToClient(ChatItem message)
        {
            try
            {
                using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    client.Connect(IpAddress, Port);
                    byte[] data = Encoding.UTF8.GetBytes(message.Item);
                    await client.SendAsync(data, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
