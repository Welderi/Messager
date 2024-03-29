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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp20
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChatsList chatsList = new ChatsList();
        ChatWindow chatWindow = new ChatWindow();
        public MainWindow()
        {
            InitializeComponent();

            chatsList.AddItem(new ContactItem { Name = "Mila" });

            ContactsListBox.DataContext = chatsList;

            chatWindow.AddItem(new ChatItem { Message = "fdsfsdf"});

            ChatWindowListBox.DataContext = chatWindow;
        }
    }
}
