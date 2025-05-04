using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymApp.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;
        public Action<object> LoginSuccess { get; set; }
        #region Properties
        private string _username = "Unesite korisničko ime...";
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        private string _passwordError;
        public string PasswordError { get => _passwordError; set { _passwordError = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand AddTrainerCommand { get; }

        #endregion

        public Action OpenAddNewMember;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            LoginCommand = new RelayCommand(Login);
            AddTrainerCommand = new RelayCommand(AddNewTrainer);
        }

        private void Login() 
        {
            
            bool IsValid = LoginIsValid();
            object trainer = _userRepository.GetAll().FirstOrDefault(x => x.Username == Username);
            if (IsValid)
            {

                LoginSuccess?.Invoke(trainer);
            }
            else
                PasswordError = "Pogrešno korisničko ime ili lozinka";
            
        }
        private bool LoginIsValid()
        {
           
            try
            {
                if (_userRepository.Authenticate(Username, Password))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Došlo je do greške prilikom autentifikacije: " + e.Message,
                       "Greška",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                return false;

            }

            return false;
        }
        private void AddNewTrainer()
        {
            OpenAddNewMember?.Invoke();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
