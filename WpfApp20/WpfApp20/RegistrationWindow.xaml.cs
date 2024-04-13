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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        DataBaseDbContext dataBaseDbContext = new DataBaseDbContext();
        public RegistrationWindow()
        {
            InitializeComponent();

        }
        public void BackToMain(object obj, RoutedEventArgs arg)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // ------------Red Color For txtUserLogin Background. You can remove marked code and add your code-----------
        public void UserLoginTextChanged(object obj, RoutedEventArgs arg)
        {
            string login = txtUserLogin.Text;

            if (dataBaseDbContext.CheckUserExists(login))
            {
                // This 
                txtUserLogin.Foreground = Brushes.Red;
                // 
            }
            else
            {
                // And this
                txtUserLogin.Foreground = Brushes.White;
                //
            }
        }

        // ------------Red Color For txtEmail Background. You can remove marked code and add your code-----------
        public void EmailTextChanged(object obj, RoutedEventArgs arg)
        {
            string email = txtEmail.Text;

            if (dataBaseDbContext.CheckEmailExists(email))
            {
                // This 
                txtEmail.Foreground = Brushes.Red;
                // 
            }
            else
            {
                // And this
                txtEmail.Foreground = Brushes.White;
                //
            }
        }
        public void CreateNewAccount(object obj, RoutedEventArgs arg)
        {
            if (txtUserPassword.Password == txtUserRePassword.Password)
            {
                dataBaseDbContext.AddToUsers(txtUserLogin.Text, txtUserPassword.Password, txtEmail.Text);
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
