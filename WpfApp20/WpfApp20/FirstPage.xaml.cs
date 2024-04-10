using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        static int userId;
        static int recId;
        public FirstPage()
        {
            InitializeComponent();

            ContactsListBox.DataContext = chatsList;

            ChatWindowListBox.DataContext = chatWindow;

        }
        public void ReceiveId(int id)
        {
            userId = id;
            chatsList.AddContactsFromDatabase(id);
        }
        public void DisplayContactAdd(object obj, RoutedEventArgs arg)
        {
            BorderContact.Visibility = Visibility.Visible;
        }
        public void AddNewContact(object obj, RoutedEventArgs arg)
        {
            if (data.CheckUserExists(txtUserContact.Text)) 
            {
                var user = data.Users.FirstOrDefault(u => u.Name == txtUserContact.Text);
                data.AddToContacts(userId, user.UserID);
                chatsList.AddItem(new ContactItem { Name = user.Name });
            }
            else
            {
                MessageBox.Show("This user doesnt exist");
            }
        }
        public void SendMessage(object obj, RoutedEventArgs arg)
        {
            chatWindow.AddItem(new ChatItem { Message = txtTextBoxMessage.Text });
            data.AddToMessages(userId, recId, txtTextBoxMessage.Text, false, DateTime.Now);
            txtTextBoxMessage.Clear();
        }
        public void ContactsListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactsListBox.SelectedItem != null)
            {
                ContactItem selectedContact = (ContactItem)ContactsListBox.SelectedItem;
                int selectedUserId = data.GetId(selectedContact.Name);
                recId = selectedUserId;
                chatWindow.DisplayConversation(selectedUserId, userId); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
