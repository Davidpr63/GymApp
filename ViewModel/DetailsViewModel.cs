using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private DateTime _paymentDate;
        public DateTime PaymentDate { get => _paymentDate; set { _paymentDate = value; OnPropertyChanged(); } }
        private DateTime _expiryDate;
        public DateTime ExpiryDate { get => _expiryDate; set { _expiryDate = value; OnPropertyChanged(); } }

        private string _notes;

        public string Notes { get => _notes; set { _notes = value; OnPropertyChanged(); SaveNotes(); } }

        #endregion
        #region Commands
        public ICommand CloseDetailWindowCommand { get; }
        #endregion
        public Action CloseWindow;
        public int _idNotes { get; set; }

        public DetailsViewModel(int id, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository)
        {
            _userRepository = userRepository;
            _notesRepository = notesRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
            CloseDetailWindowCommand = new RelayCommand(CloseDetailWindow);
           
            
            FillOutMemberData(id);
            
        }
        private void SaveNotes()
        {
           
            if (!Notes.Equals("Unesite beleske..."))
            {
                var memberNotes = _notesRepository.Get(_idNotes);
                memberNotes.Notes = Notes;
                _notesRepository.Update(memberNotes);
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
