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
using System.Windows;
using System.Windows.Input;

namespace GymApp.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;
        public ObservableCollection<User> Members { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<User> _filteredMembers { get; set; } = new ObservableCollection<User>();
     
        #region Properties
        public string LoggedInUsername { get; set; }
        private string _searchId = "Unesite ID clana...";
        public string SearchId { get => _searchId; set { _searchId = value; OnPropertyChanged(); SearchMember(); } }
        private bool _activeMembersIsChecked;
        public bool ActiveMembersIsChecked { get => _activeMembersIsChecked; set { _activeMembersIsChecked = value; CheckBoxs(); OnPropertyChanged(); } }
        private bool _activeMembersNotPaidIsChecked;
        public bool ActiveMembersNotPaidIsChecked { get => _activeMembersNotPaidIsChecked; set { _activeMembersNotPaidIsChecked = value; CheckBoxs(); OnPropertyChanged(); } }

        #endregion

        #region Commands
        #endregion
        public ICommand LogoutCommand { get; }
        public MainWindowViewModel(string username, IUserRepository userRepository)
        {
            LoggedInUsername = $"Welcome, {username}";
            _userRepository = userRepository;
            FillOutOutputList(_userRepository.GetAll());
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Logout()
        {
            // close this window and open login again, or shutdown app
            MessageBox.Show("log out");
        }

        private void SearchMember()
        {

            
            List<User> listOfFilterMember = new List<User>();
            try
            {
                bool isNumber = int.TryParse(SearchId, out int id);
                if (isNumber)
                {

                    listOfFilterMember = _userRepository.GetAll().Where(x => x.Id == id).ToList();
                    FillOutOutputList(listOfFilterMember);

                }
                else if (!string.IsNullOrEmpty(SearchId))
                {
                    FillOutOutputList(_userRepository.GetAll());
                    MessageBox.Show("ID mora biti broj");
                }
                else
                    FillOutOutputList(_userRepository.GetAll());
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while searching members: " + e.Message,
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }
           
            
                 
        }
        private void CheckBoxs()
        {
            if (ActiveMembersIsChecked && !ActiveMembersNotPaidIsChecked)
            {
                ActiveMembers();
            }
            if (!ActiveMembersIsChecked && ActiveMembersNotPaidIsChecked)
            {
                ActiveNotPaidMembers();
            }
            if (!ActiveMembersIsChecked && !ActiveMembersNotPaidIsChecked)
            {
                FillOutOutputList(_userRepository.GetAll());
            }
            if (ActiveMembersIsChecked && ActiveMembersNotPaidIsChecked)
            {
                ActiveMembersIsChecked = false;
                ActiveMembersNotPaidIsChecked = false;
                MessageBox.Show("Cekirajte jednu opciju",
                       "Greška",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
            }
        }
        private void ActiveMembers()
        {
            List<User> filteredMembers = new List<User>();
            try
            {
                filteredMembers = _userRepository.GetAll().Where(x => x.IsMembershipPaid == true).ToList();
                FillOutOutputList(filteredMembers);
            }
            catch (Exception e )
            {
                MessageBox.Show("An error occurred while filtering active members: " + e.Message,
                       "Greška",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }
        }

        private void ActiveNotPaidMembers()
        {
            List<User> filteredMembers = new List<User>();
            try
            {
                filteredMembers = _userRepository.GetAll().Where(x => x.IsMembershipPaid == false).ToList();
                FillOutOutputList(filteredMembers);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while filtering active members who haven't paid membership: " + e.Message,
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }
        }
        private void FillOutOutputList(List<User> list) 
        {
            Members.Clear();
            foreach (var item in list)
                Members.Add(item);
        }
            
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
