using GymApp.Database.IRepository;
using GymApp.Database.Repository;
using GymApp.GoogleDrive;
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
        
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly IGoogleDriveUploader _googleDriveUploader;
        private string filePathUsers = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Users.json";
        private string filePathNotes = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Notes.json";
        private string filePathPaymentHistory = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Payment history.json";
        private string _usersOldHash;
        private string _usersNewHash;
        private string _notesOldHash;
        private string _notesNewHash;
        private string _historyOldHash;
        private string _historyNewHash;
        public MainWindow(string username, IUserRepository userRepository)
        {
            InitializeComponent();
            _notesRepository = new NotesRepository(filePathNotes);
            _paymentHistoryRepository = new PaymentHistoryRepository(filePathPaymentHistory);
            _googleDriveUploader = new GoogleDriveUploader();
            _viewModel = new MainWindowViewModel(username, userRepository);
            _usersOldHash = GetFileHash(filePathUsers);
            _notesOldHash = GetFileHash(filePathNotes);
            _historyOldHash = GetFileHash(filePathPaymentHistory);
            _viewModel.OpenAddMemberWindow = () =>
            {
                var addNewMemberWindow = new AddNewMemberPage(userRepository, _notesRepository, _paymentHistoryRepository);
                addNewMemberWindow.Show();
            };
            _viewModel.OpenDetailsWindow = (object id) =>
            {
                var detailsWindow = new DetailsPage(id, userRepository, _notesRepository, _paymentHistoryRepository);
                detailsWindow.Show();
            };
            _viewModel.CloseMain = () =>
            {
                
                var logIn = new LoginPage();
                logIn.Show();
                this.Close();
                _usersNewHash = GetFileHash(filePathUsers);
                _notesNewHash = GetFileHash(filePathNotes);
                _historyNewHash = GetFileHash(filePathPaymentHistory);
                if (!_usersOldHash.Equals(_usersNewHash))
                    _googleDriveUploader.UploadFile("Users.json");
                if (!_notesOldHash.Equals(_notesNewHash))
                    _googleDriveUploader.UploadFile("Notes.json");
                if (!_historyOldHash.Equals(_historyNewHash))
                    _googleDriveUploader.UploadFile("Payment history.json");

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
                SearchTB.Text = "Unesite ID clana...";
                SearchTB.Foreground = Brushes.Gray;
            }
        }

        private void Search_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
