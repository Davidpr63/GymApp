using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GymApp.ViewModel
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PaymentHistory> PaymentHistory {  get; set; } = new ObservableCollection<PaymentHistory>();

        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        #region Propetries
        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        private string _firstname;
        public string FirstName { get => _firstname; set { _firstname = value; OnPropertyChanged(); } }
        private string _lastname;
        public string LastName { get => _lastname; set { _lastname = value; OnPropertyChanged(); } }
        private string _email = "Unesite email...";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); SaveEmail(); } }
        private string _newPassword = "Nova lozinka...";
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(); ChangeAdminPassword(); } }

        private DateTime _paymentDate;
        public DateTime PaymentDate { get => _paymentDate; set { _paymentDate = value; OnPropertyChanged(); } }
        private DateTime _expiryDate;
        public DateTime ExpiryDate { get => _expiryDate; set { _expiryDate = value; OnPropertyChanged(); } }

        private string _notes;

        public string Notes { get => _notes; set { _notes = value; OnPropertyChanged(); SaveNotes(); } }
        private bool _isTextReadOnly = false;
        public bool IsTextReadOnly { get => _isTextReadOnly; set { _isTextReadOnly = value; OnPropertyChanged(); } }
        #endregion
        #region Commands
        public ICommand CloseDetailWindowCommand { get; }
        #endregion
        public Action CloseWindow;
        public int _idNotes { get; set; }

        public int _idMember {  get; set; }

        public DetailsViewModel(int id, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            _userRepository = userRepository;
            _notesRepository = notesRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
            CloseDetailWindowCommand = new RelayCommand(CloseDetailWindow);
            _idMember = id;
            
            FillOutMemberData(id);
            
        }
        private void SaveNotes()
        {
            var member = _userRepository.Get(_idMember);
            var memberNotes = _notesRepository.Get(_idNotes);
            if (!Notes.Equals("Unesite beleske...") && !string.IsNullOrEmpty(Notes))
            {

                member.HaveNote = true;


                memberNotes.Notes = Notes;
                _userRepository.Update(member);
                _notesRepository.Update(memberNotes);
                MainWindowViewModel.FillOutOutputList(_userRepository.GetAll());
            }
            else
            {
                member.HaveNote = false;
                memberNotes.Notes = Notes;
                _userRepository.Update(member);
                _notesRepository.Update(memberNotes);
                MainWindowViewModel.FillOutOutputList(_userRepository.GetAll());

            }

        }

        private void ChangeAdminPassword()
        {
            if (!NewPassword.Equals("Nova lozinka..."))
            {
                string salt;
                var admin = _userRepository.Get(_idMember);
                admin.Password = HashPassword(NewPassword, out salt);
                admin.Salt = salt;
                _userRepository.Update(admin);
            }
        }
        public string HashPassword(string password, out string salt)
        {
            // Generiši slučajnu so
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            salt = Convert.ToBase64String(saltBytes);

            // Derivuj ključ koristeći PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // 256-bit

            return Convert.ToBase64String(hash);
        }
        private void SaveEmail()
        {

            if(!Email.Equals("Unesite email..."))
            {
                var member = _userRepository.Get(_idMember);
                member.Email = Email;
                _userRepository.Update(member);
            }
        }
        private void FillOutMemberData(int id)
        {
            _idNotes = id;
            var member = _userRepository.Get(id);
            var memberNotes = _notesRepository.Get(id);
            Id = member.Id;
            FirstName = member.Firstname;
            LastName = member.Lastname;
            
            Email = member.Email;
           
            PaymentDate = member.PaymentDate;
            ExpiryDate = member.ExpiryDate;
            if (_notesRepository.Get(id) == null)
                Notes = "Unesite beleske...";
            else
                Notes = memberNotes.Notes;
            foreach(var item in _paymentHistoryRepository.GetAll())
            {
                if (item.Id == id)
                {
                    PaymentHistory.Add(item);
                }
                
            }
            
            
        }
        private void CloseDetailWindow()
        {
            CloseWindow?.Invoke();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
