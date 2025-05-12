using GymApp.Database.IRepository;
using GymApp.Database.Repository;
using GymApp.GoogleDrive;
using GymApp.Logger;
using GymApp.Model;
using GymApp.View;
using GymApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private LoginPage logIn = new LoginPage();
       
        
        public MainWindow(User trainer, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository, ILogger logger)
        {
            InitializeComponent();
           
            
            _viewModel = new MainWindowViewModel(trainer, userRepository, notesRepository, paymentHistoryRepository, logger);
            
            _viewModel.OpenAddMemberWindow = () =>
            {
                var addNewMemberWindow = new AddNewMemberPage(TypeUser.Member, userRepository, notesRepository, paymentHistoryRepository, logger);
                addNewMemberWindow.Show();
                addNewMemberWindow.Activate();
            };
            _viewModel.OpenDetailsWindow = (object id) =>
            {
                var detailsWindow = new DetailsPage(id, userRepository, notesRepository, paymentHistoryRepository);
                detailsWindow.Show();
                detailsWindow.Activate();
            };
            _viewModel.CloseMain = () =>
            {
                
                
                logIn.Show();
                logger.Log($"{trainer.Firstname} {trainer.Lastname} se odjavio!");

                this.Close();
                //CheckDatabase();

            };
            _viewModel.OpenConfirmationPage = (object id) =>
            {
                int Id = Convert.ToInt32(id);
                var confirmWindow = new Confirmation(Id, _viewModel ,userRepository, notesRepository, paymentHistoryRepository, logger);
                confirmWindow.Show();
                confirmWindow.Activate();
            };

            

            this.DataContext = _viewModel; 
        }
        public string GetFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hashBytes = sha256.ComputeHash(stream);
                    return Convert.ToBase64String(hashBytes);
                }
            }
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
                SearchTB.Text = "Unesite ID člana...";
                SearchTB.Foreground = Brushes.Gray;
            }
        }

        
        private void Search_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
