using GymApp.Database.IRepository;
using GymApp.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GymApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow(string username, IUserRepository userRepository)
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel(username, userRepository);
            _viewModel.OpenAddMemberWindow = () =>
            {
                var addNewMemberWindow = new AddNewMemberPage(userRepository);
                addNewMemberWindow.Show();
            };
            _viewModel.OpenDetailsWindow = (object id) =>
            {
                var detailsWindow = new DetailsPage(id, userRepository);
                detailsWindow.Show();
            };
            this.DataContext = _viewModel; 
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "";
            SearchTB.Foreground = Brushes.Black;
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(SearchTB.Text))
            {
                SearchTB.Text = "Unesite ID clana...";
                SearchTB.Foreground = Brushes.Gray;
            }
        }

        private void Search_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
