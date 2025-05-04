using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.Logger;
using GymApp.Model;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymApp.ViewModel
{
    public class AddMemberViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _firstname = "Unesite ime...";
        public string FirstName { get => _firstname; set { _firstname = value; OnPropertyChanged(); } }
        private string _lastname = "Unesite prezime...";
        
        public string LastName { get => _lastname; set { _lastname = value; OnPropertyChanged(); } }
        private string _username = "Unesite korisničko ime...";
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        private string _email = "Unesite email...(opciono)";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private string _isTrainer;
        public string IsTrainer { get => _isTrainer; set { _isTrainer = value; OnPropertyChanged(); } }

        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        private TypeUser _typeUser { get; set; }
        #endregion

        #region Commands
        public ICommand AddNewMemberCommand { get; }
        #endregion
        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly ILogger _logger;
        public Action CloseAddWindow;
        public Action RefreshOutputList;
        public AddMemberViewModel(TypeUser typeUser, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _notesRepository = notesRepository;
            _typeUser = typeUser;
            _logger = logger;
            if (_typeUser == TypeUser.Member)
            {
                IsTrainer = "Collapsed";
            }
            _paymentHistoryRepository = paymentHistoryRepository;
            AddNewMemberCommand = new RelayCommand(AddNewMember);
        }

        private void AddNewMember()
        {
            bool result = IsAddFormValid();
            if (result)
            {
                
                User newMember = new User()
                {
                    Firstname = FirstName,
                    Lastname = LastName,
                    Username = Username,
                    Password = Password,
                    TypeUser = _typeUser,
                    Email = Email,
                    IsMembershipPaid = true,
                    GotEmail = false,
                    PaymentDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddDays(30),
                };
                _userRepository.Add(newMember);
                Note note = new Note() { MemberId = newMember.Id, Notes = "Unesite beleske..." };
                _notesRepository.Add(note);
                PaymentHistory paymentHistory = new PaymentHistory()
                {
                    MemberId = newMember.Id,
                    IsPaid = true,
                    PaymentDate = DateTime.Now
                };
                _paymentHistoryRepository.Add(paymentHistory);
                if (_typeUser == TypeUser.Trainer)
                {
                    MessageBox.Show($"Dodat je novi trener {newMember.Firstname}",
                    "Uspesno",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                }
                else 
                {
                    MessageBox.Show($"Novi clan je uspesno dodat!\nNjegov ID : {newMember.Id}",
                        "Uspesno",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                        );
                
                }
                ErrorMessage = "";
                _logger.Log($"Dodat je novi član {newMember.Firstname} {newMember.Lastname}");
                MainWindowViewModel.FillOutOutputList(_userRepository.GetAll());
                CloseAddWindow?.Invoke();
            }
            else
                ErrorMessage = "Sva polja moraju biti popunjena!";
        }

        private bool IsAddFormValid()
        {
            if (_typeUser == TypeUser.Member)
            {
                Password = string.Empty;
                Username = string.Empty;
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName)) // add email later
                {
                    return false;
                }
                if (FirstName.Equals("Unesite ime...") || LastName.Equals("Unesite prezime...")) // add email later
                {
                    return false;
                }

                return true;
            }
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)) // add email later
            {
                return false;
            }
            if (FirstName.Equals("Unesite ime...") || LastName.Equals("Unesite prezime...") || Username.Equals("Unesite korisničko ime...")) // add email later
            {
                return false;
            }

            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
