using GymApp.Database.IRepository;
using GymApp.Database.Repository;
using GymApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace GymApp.View
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private readonly LoginViewModel _viewModel;

        
        public LoginPage()
        {
            InitializeComponent();
            string filePath = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Users.json";
            IUserRepository _userRepository = new UserRepository(filePath);
            _viewModel = new LoginViewModel(_userRepository);
            _viewModel.LoginSuccess = () =>
            {
                var mainWindow = new MainWindow(_viewModel.Username);
                mainWindow.Show();
                this.Close();
            };  

            this.DataContext = _viewModel;

        }

       
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
           
            _viewModel.Password = PasswordBox.Password;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameTB.Text.Equals("Username..."))
            {
                UsernameTB.Text = "";
                UsernameTB.Foreground = Brushes.Black;
            }
            
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTB.Text))
            {
                UsernameTB.Text = "Enter username";
                UsernameTB.Foreground = Brushes.Gray;
            }
        }
    }
}
