using GymApp.Database.IRepository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.ViewModel
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<string> PaymentHistory {  get; set; } = new ObservableCollection<string>();

        private readonly IUserRepository _userRepository;
        #region Propetries
        private string _firstname;
        public string FirstName { get => _firstname; set { _firstname = value; OnPropertyChanged(); } }
        private string _lastname;
        public string LastName { get => _lastname; set { _lastname = value; OnPropertyChanged(); } }
        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private DateTime _paymentDay;
        public DateTime PaymentDay { get => _paymentDay; set { _paymentDay = value; OnPropertyChanged(); } }
        private DateTime _expiryDay;
        public DateTime ExpiryDay { get => _expiryDay; set { _expiryDay = value; OnPropertyChanged(); } }

        private string _notes;

        public string Notes { get => _notes; set { _notes = value; OnPropertyChanged(); } }

        #endregion

        public DetailsViewModel(int id, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            PaymentHistory.Add("datum1");
            PaymentHistory.Add("datum2");
            PaymentHistory.Add("datum3");
            FillOutMemberData(id);
        }

        private void FillOutMemberData(int id)
        {
            var member = _userRepository.GetAll().FirstOrDefault(x => x.Id == id);
            FirstName = member.Firstname;
            LastName = member.Lastname;
            Email = member.Email;   
            PaymentDay = member.PaymentDate;
            ExpiryDay = member.ExpiryDate;
            Notes = "Unesite beleske...";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
