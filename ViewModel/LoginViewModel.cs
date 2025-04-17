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
        public Action LoginSuccess { get; set; }
        #region Properties
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        private string _passwordError;
        public string PasswordError { get => _passwordError; set { _passwordError = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        #endregion

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login() 
        {
            
            bool IsValid = LoginIsValid(); 
            if (IsValid)
            {
                LoginSuccess?.Invoke();
            }
            else
                PasswordError = "Invalid username or password";
            
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
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
