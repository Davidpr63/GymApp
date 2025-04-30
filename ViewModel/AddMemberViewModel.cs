using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.Model;
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
        private string _email = "Unesite email...(opciono)";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand AddNewMemberCommand { get; }
        #endregion
        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        public Action CloseAddWindow;
        public Action RefreshOutputList;
        public AddMemberViewModel(IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            _userRepository = userRepository;
            _notesRepository = notesRepository;
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
                    TypeUser = TypeUser.Member,
                    Email = Email,
                    IsMembershipPaid = true,
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
                MessageBox.Show($"Novi clan je uspesno dodat!\nNjegov ID : {newMember.Id}",
                    "Uspesno",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                ErrorMessage = "";
                MainWindowViewModel.FillOutOutputList(_userRepository.GetAll());
                CloseAddWindow?.Invoke();
            }
            else
                ErrorMessage = "Sva polja moraju biti popunjena!";
        }

        private bool IsAddFormValid()
        {
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
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
