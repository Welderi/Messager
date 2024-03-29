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
    public class UserControlCollection : INotifyPropertyChanged
    {

        private const int Port = 9000;
        private static readonly IPAddress IpAddress = IPAddress.Loopback;
        private static readonly IPEndPoint EndPoint = new IPEndPoint(IpAddress, Port);
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ChatItem> UserChat { get; set; } = new ObservableCollection<ChatItem>();
        public async Task SendMessageToServer(ChatItem message)
        {
            try
            {
                using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    await client.ConnectAsync(IpAddress, Port);
                    byte[] data = Encoding.UTF8.GetBytes(message.Item);
                    await client.SendAsync(data, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }
        public async Task ReceiveMessageFromServer()
        {
            try
            {
                byte[] buffer = new byte[1024];
                using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    await client.ConnectAsync(EndPoint);

                    while (true)
                    {
                        int bytesRead = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Application.Current.Dispatcher.Invoke(() => UserChat.Add(new ChatItem { Item = message }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }
    }
    
    public class ChatItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string item;
        public string Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

