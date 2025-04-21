using GymApp.Database.IRepository;
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
        public AddNewMemberPage(IUserRepository userRepository)
        {
            InitializeComponent();
            _viewModel = new AddMemberViewModel(userRepository);
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

        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
