using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp21
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerControlCollection serverControlCollection = new ServerControlCollection();
        UserControlCollection userControlCollection = new UserControlCollection();

        public MainWindow()
        {
            InitializeComponent();
            ListBoxUser.DataContext = userControlCollection;
            ListBoxServer.DataContext = serverControlCollection;

            Task.Run(async () =>
            {
                await serverControlCollection.StartListening();
            });

        }

        private async void UserClick(object sender, RoutedEventArgs e)
        {
            string userText = UserText.Text;
            await userControlCollection.SendMessageToServer(new ChatItem { Item = userText });
            UserText.Clear();
        }

        private async void ServerClick(object sender, RoutedEventArgs e)
        {
            string serverText = ServerText.Text;
            await serverControlCollection.SendMessageToClient(new ChatItem { Item = serverText });
            ServerText.Clear();
        }

    }
}
