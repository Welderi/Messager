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
using DataBase;

namespace WpfApp20
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        DataBaseDbContext dataBaseDbContext = new DataBaseDbContext();
        public LoginWindow()
        {
            InitializeComponent();
        }
        public void GenerateCreateNewUser(object obj, RoutedEventArgs arg)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            this.Close();
            registrationWindow.Show();
        }

        // ------- Red Color For txtUserName Background. You can remove marked code and add your code -- --
        public void UserNameTextChanged(object ojb, RoutedEventArgs arg)
        {
            string login = txtUserName.Text;

            if (dataBaseDbContext.CheckUserExists(login))
            {
                // This 
                txtUserName.Foreground = Brushes.White;
                // 
            }
            else
            {
                // And this
                txtUserName.Foreground = Brushes.Red;
                //
            }
        }
        public void GenerateChatWindow(object obj, RoutedEventArgs arg)
        {
            string login = txtUserName.Text;
            string pass = txtPassword.Password;

            if (dataBaseDbContext.CheckUserPassword(login, pass) && dataBaseDbContext.CheckUserExists(login)) 
            {
                FirstPage firstPage = new FirstPage();
                firstPage.ReceiveId(dataBaseDbContext.GetId(login));
                firstPage.Show();
                this.Close();
            }
        }
    }
}
