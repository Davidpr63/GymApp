
using GymApp.Database.IRepository;
using GymApp.Database.Repository;
using GymApp.EmailService;
using GymApp.GoogleDrive;
using GymApp.Model;
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
using System.Windows.Shapes;

namespace GymApp.View
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private readonly LoginViewModel _viewModel;
        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly IGoogleDriveUploader _googleDriveUploader;
        private readonly IEmailService _emailService;
        private string filePathUsers = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Users.json";
        private string filePathNotes = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Notes.json";
        private string filePathPaymentHistory = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Payment history.json";
        private string _usersOldHash;
        private string _usersNewHash;
        private string _notesOldHash;
        private string _notesNewHash;
        private string _historyOldHash;
        private string _historyNewHash;
        private User LoggedInUser = new User();
        public LoginPage()
        {
            InitializeComponent();
            _emailService = new EmailService.EmailService();
            _userRepository = new UserRepository(filePathUsers, _emailService);
            _notesRepository = new NotesRepository(filePathNotes);
            _paymentHistoryRepository = new PaymentHistoryRepository(filePathPaymentHistory);
            _viewModel = new LoginViewModel(_userRepository);
            _googleDriveUploader = new GoogleDriveUploader();
            _usersOldHash = GetFileHash(filePathUsers);
            _notesOldHash = GetFileHash(filePathNotes);
            _historyOldHash = GetFileHash(filePathPaymentHistory);
            _viewModel.LoginSuccess = (object trainer) =>
            {
                LoggedInUser = trainer as User;
                var mainWindow = new MainWindow(LoggedInUser, _userRepository, _notesRepository, _paymentHistoryRepository);
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
            if (UsernameTB.Text.Equals("Unesite korisničko ime..."))
            {
                UsernameTB.Text = "";
                UsernameTB.Foreground = Brushes.Black; 
            }
            
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTB.Text))
            {
                UsernameTB.Text = "Unesite korisničko ime...";
                UsernameTB.Foreground = Brushes.Gray;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (DateTime.Now.Day == 25)
                _userRepository.CheckMembership();

            var task =  CheckDatabase();
            task.GetAwaiter().GetResult();
            
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
        private async Task CheckDatabase()
        {
            _usersNewHash = GetFileHash(filePathUsers);
            _notesNewHash = GetFileHash(filePathNotes);
            _historyNewHash = GetFileHash(filePathPaymentHistory);
            if (!_usersOldHash.Equals(_usersNewHash))
                _googleDriveUploader.UploadFile("Users.json");
            if (!_notesOldHash.Equals(_notesNewHash))
                _googleDriveUploader.UploadFile("Notes.json");
            if (!_historyOldHash.Equals(_historyNewHash))
                 _googleDriveUploader.UploadFile("Payment history.json");

            Console.WriteLine("");
        }
    }
}
