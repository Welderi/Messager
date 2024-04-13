using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataBase;

namespace WpfApp20
{
    /// <summary>
    /// Interaction logic for FirstPage.xaml
    /// </summary>
    public partial class FirstPage : Window
    {
        ChatsList chatsList = new ChatsList();
        ChatWindow chatWindow = new ChatWindow();
        DataBaseDbContext data = new DataBaseDbContext();
        Server server;
        public static int userId;
        static int recId;
        public FirstPage()
        {
            InitializeComponent();

            //ContactsListBox.DataContext = chatsList;

            //ChatWindowListBox.DataContext = chatWindow;
        }
        private void InitializeAsync()
        {
            server = new Server();
            var user = data.Users.FirstOrDefault(u => u.UserID == userId);
            server.connectedEvent += UserConnected;
            server.messageReceivedEvent += MessageReceived;
            server.UserDisconnectedEvent += UserDisconnected;
            server.ConnectToServer(user.Name);
        }
        void UserConnected()
        {

        }
        void MessageReceived()
        {
            var msg = server.packet.ReadMessage();
            Application.Current.Dispatcher.Invoke(() =>
            {
                chatWindow.AddItem(new ChatItem { Message = msg });
            });
        }
        void UserDisconnected()
        {
            var uid = server.packet.ReadMessage();
        }
        public void ReceiveId(int id)
        {
            userId = id;
            chatsList.AddContactsFromDatabase(id);

            InitializeAsync();
        }
        public void DisplayContactAdd(object obj, RoutedEventArgs arg)
        {
            //BorderContact.Visibility = Visibility.Visible;
        }
        public void AddNewContact(object obj, RoutedEventArgs arg)
        {
            //if (data.CheckUserExists(txtUserContact.Text))
            //{
            //    var user = data.Users.FirstOrDefault(u => u.Name == txtUserContact.Text);
            //    data.AddToContacts(userId, user.UserID);
            //    chatsList.AddItem(new ContactItem { Name = user.Name });
            //}
            //else
            //{
            //    MessageBox.Show("This user doesnt exist");
            //}
        }
        public void SendMessage(object obj, RoutedEventArgs arg)
        {
            //server.SendMessageToServer(txtTextBoxMessage.Text);
            //data.AddToMessages(userId, recId, txtTextBoxMessage.Text, false, DateTime.Now);
            ////chatWindow.AddItem(new ChatItem { Message = txtTextBoxMessage.Text });
            //txtTextBoxMessage.Clear();
        }
        public void ContactsListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (ContactsListBox.SelectedItem != null)
            //{
            //    ContactItem selectedContact = (ContactItem)ContactsListBox.SelectedItem;
            //    int selectedUserId = data.GetId(selectedContact.Name);
            //    recId = selectedUserId;
            //    chatWindow.DisplayConversation(selectedUserId, userId);
            //}
        }
        public void Border_MouseLeftButtonDown(object obj, RoutedEventArgs arg)
        {

        }
        public void Border_MouseDown(object obj, RoutedEventArgs arg)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

