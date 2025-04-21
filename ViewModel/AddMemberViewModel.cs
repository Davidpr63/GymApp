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
        private string _email = "Unesite email...";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        private string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand AddNewMemberCommand { get; }
        #endregion
        private readonly IUserRepository _userRepository;
        public Action CloseAddWindow;
        public AddMemberViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            AddNewMemberCommand = new RelayCommand(AddNewMember);
        }

        private void AddNewMember()
        {
            bool result = IsAddFormValid();
            if (result)
            {
                User newMember = new User() { Firstname = FirstName, Lastname = LastName};
                _userRepository.Add(newMember);
                MessageBox.Show("Novi clan je uspesno dodat",
                    "Uspesno",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
                ErrorMessage = "";
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
