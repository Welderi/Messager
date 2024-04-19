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

        Program program;

        public static int userId;

        static int recId;

        public FirstPage(int id)
        {
            InitializeComponent();

            userId = id;

            InitializeElements();

            program = new Program(userId.ToString());

            program.MessageReceived += OnMessageReceived;

            AllContactsInGroupCreating.DataContext = chatsList;

            ContactsListBox.DataContext = chatsList;

            ChatWindowListBox.DataContext = chatWindow;
        }

        //         -- Initialization --

        void InitializeElements()
        {
            var user = data.Users.Where(u => u.UserID == userId).FirstOrDefault();
            UserNameInMenu.DataContext = new NameObject(user.Name);
        }
        public void ReceiveId(int id)
        {
            userId = id;
            chatsList.AddContactsFromDatabase(id);
        }
        private void OnMessageReceived(object sender, string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                chatWindow.AddItem(new ChatItem { Message = message});
            });
        }

        //         -- Work of Buttons --

        //        1.  Buttons In Menu

        public void AddNewContact(object obj, RoutedEventArgs arg)
        {
            if (data.CheckUserExists(txtUserContact.Text))
            {
                var user = data.Users.FirstOrDefault(u => u.Name == txtUserContact.Text);
                data.AddToContacts(userId, user.UserID);
                chatsList.AddItem(new ContactItem { Name = user.Name, IsGroup = false });
            }
            else
            {
                MessageBox.Show("This user doesnt exist");
            }
        }
        private void LogOut_ButtonClick(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            lw.Show();
            this.Close();
        }
        public void CreateGroup(object sender, RoutedEventArgs arg)
        {
            if (ContactsInGroup.Items.Count > 0)
            {
                data.AddToGroups(GroupName.Text, GroupDesc.Text, DateTime.Now, userId);
                var group = data.Groups.FirstOrDefault(g => g.CreatorID == userId);
                foreach (var item in ContactsInGroup.Items)
                {
                    ContactItem contact = (ContactItem)item;
                    var user = data.Users.FirstOrDefault(u => u.Name == contact.Name);
                    data.AddToGroupMemberships(user.UserID, group.GroupID, "Reader" ,DateTime.Now);
                }
                chatsList.AddItem(new ContactItem { Name = GroupName.Text, IsGroup = true });
                data.AddToGroupMemberships(userId, group.GroupID, "Admin", DateTime.Now);
            }
            else
            {
                MessageBox.Show("Select contacts to adding");
            }
        }
        //         2. Another
        public async void SendMessage(object obj, RoutedEventArgs arg)
        {
            var con = (ContactItem)ContactsListBox.SelectedItem;
            if (con.IsGroup == true)
            {
                var g = data.Groups.FirstOrDefault(g => g.Name == con.Name);
                await program.SendMessageGroup(g.GroupID.ToString(),userId.ToString(), txtTextBoxMessage.Text);
                chatWindow.AddItem(new ChatItem { Message = txtTextBoxMessage.Text });
                data.AddToMessageGroup(userId, g.GroupID, txtTextBoxMessage.Text);
                txtTextBoxMessage.Clear();
            }
            else
            {
                await program.SendMessage(recId.ToString(), txtTextBoxMessage.Text);
                chatWindow.AddItem(new ChatItem { Message = txtTextBoxMessage.Text });
                data.AddToMessages(userId, recId, txtTextBoxMessage.Text, false, DateTime.Now);
            }
            txtTextBoxMessage.Clear();
        }

        //         3. Buttons in List

        public void RemoveContactFromGroup(object s, RoutedEventArgs arg)
        {
            Button button = (Button)s;
            ContactItem selectedContact = (ContactItem)button.DataContext;
            ContactsInGroup.Items.Remove(selectedContact);
        }
        public void RelocateContactInGroup(object s, RoutedEventArgs arg)
        {
            Button button = (Button)s; 
            ContactItem selectedContact = (ContactItem)button.DataContext;
            if (selectedContact.IsGroup == false)
            {
                ContactsInGroup.Items.Add(selectedContact);
            }
            else
            {
                MessageBox.Show("You can not choose group");
            }
        }

        //         -- Work with Lists --

        public void ContactsListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactsListBox.SelectedItem != null)
            {
                ContactItem selectedContact = (ContactItem)ContactsListBox.SelectedItem;
                if (selectedContact.IsGroup == true)
                {
                    int selectedMId = data.GetGroupId(selectedContact.Name);
                    chatWindow.DisplayGroupConversation(selectedMId);
                }
                else
                {
                    int selectedUserId = data.GetId(selectedContact.Name);
                    recId = selectedUserId;
                    chatWindow.DisplayConversation(selectedUserId, userId);
                }
            }
        }

        //         -- Interface work --
        public void DisplayContactAdd(object obj, RoutedEventArgs arg)
        {
            BorderContact.Visibility = (BorderContact.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
        public void Menu_ButtonClick(object sender, RoutedEventArgs arg)
        {
            MenuBorder.Visibility = (MenuBorder.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            
        }
        public void CreateGroup_ClickButton(object ob, RoutedEventArgs arg)
        {
            GroupCreateBorder.Visibility = (GroupCreateBorder.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}

