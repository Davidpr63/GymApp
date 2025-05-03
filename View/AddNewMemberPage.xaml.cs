using GymApp.Database.IRepository;
using GymApp.Model;
using GymApp.ViewModel;
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

namespace GymApp.View
{
    /// <summary>
    /// Interaction logic for AddNewMemberPage.xaml
    /// </summary>
    public partial class AddNewMemberPage : Window
    {
        private readonly AddMemberViewModel _viewModel;
        public AddNewMemberPage(TypeUser typeUser, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            InitializeComponent();
            _viewModel = new AddMemberViewModel(typeUser, userRepository, notesRepository, paymentHistoryRepository);
            _viewModel.CloseAddWindow = () =>
            {
                this.Close();
            };
            
            this.DataContext = _viewModel;
        }

        private void FirstnameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FirstnameTextBox.Text.Equals("Unesite ime..."))
            {
                FirstnameTextBox.Text = "";
                FirstnameTextBox.Foreground = Brushes.Black;
            }
        }

        private void FirstnameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstnameTextBox.Text))
            {
                FirstnameTextBox.Text = "Unesite ime...";
                FirstnameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void LastnameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LastnameTextBox.Text.Equals("Unesite prezime..."))
            {
                LastnameTextBox.Text = "";
                LastnameTextBox.Foreground = Brushes.Black;
            }
        }

        private void LastnameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LastnameTextBox.Text))
            {
                LastnameTextBox.Text = "Unesite prezime...";
                LastnameTextBox.Foreground = Brushes.Gray;
            }
        }
        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text.Equals("Unesite email...(opciono)"))
            {
                EmailTextBox.Text = "";
                EmailTextBox.Foreground = Brushes.Black;
            }
            
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                EmailTextBox.Text = "Unesite email...(opciono)";
                EmailTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text.Equals("Unesite korisničko ime..."))
            {
                UsernameTextBox.Text = "";
                UsernameTextBox.Foreground = Brushes.Black;
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text))
            {
                UsernameTextBox.Text = "Unesite korisničko ime...";
                UsernameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordTextBox.Password;
        }
    }
}
