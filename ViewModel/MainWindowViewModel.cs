using GymApp.Common;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymApp.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string LoggedInUsername { get; set; }
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public ICommand LogoutCommand { get; }
        public MainWindowViewModel(string username)
        {
            LoggedInUsername = $"Welcome, {username}";

            Users.Add(new User() { Id = 1, Firstname = "David", Lastname = "Petrovic", Email = "david@gmail.com", PaymentDate = DateTime.Now, ExpiryDate = DateTime.Now, IsMembershipPaid = true });
            Users.Add(new User() { Id = 2, Firstname = "Milan", Lastname = "Petrovic", Email = "milan@gmail.com", PaymentDate = DateTime.Now, ExpiryDate = DateTime.Now, IsMembershipPaid = false });

            LogoutCommand = new RelayCommand(Logout);
        }

        private void Logout()
        {
            // close this window and open login again, or shutdown app
            MessageBox.Show("log out");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
