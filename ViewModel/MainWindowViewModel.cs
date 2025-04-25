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
        public static ObservableCollection<User> Members { get; set; } = new ObservableCollection<User>();
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
        public ICommand LogoutCommand { get; }
        public ICommand AddNewMemberCommand {  get; }
        public ICommand ExtendCommand { get; }
        public ICommand DetailsCommand { get; }
        #endregion
        public Action CloseMain;
        public Action OpenAddMemberWindow;
        public Action<object> OpenDetailsWindow;
        public MainWindowViewModel(string username, IUserRepository userRepository)
        {
            LoggedInUsername = $"Welcome, {username}";
            _userRepository = userRepository;
            FillOutOutputList(_userRepository.GetAll());
            LogoutCommand = new RelayCommand(Logout);
            AddNewMemberCommand = new RelayCommand(AddNewMember);
            ExtendCommand = new RelayCommand(id => RenewMembership(id));
            DetailsCommand = new RelayCommand(id => OpenDetails(id));
        }

        private void Logout()
        {
            CloseMain?.Invoke();
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

        private void AddNewMember()
        {
            OpenAddMemberWindow?.Invoke();
            
        }

        private void RenewMembership(object id)
        {
            int Id = Convert.ToInt32(id);
            try
            {
                var member = _userRepository.GetAll().FirstOrDefault(x => x.Id == Id);
                member.IsMembershipPaid = true;
                member.PaymentDate = DateTime.Now;
                member.ExpiryDate = DateTime.Now.AddDays(30);

                _userRepository.Update(member);
                FillOutOutputList(_userRepository.GetAll());
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while update a member:" + e.Message,
                                       "Error",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                throw;
            }

        }

        private void OpenDetails(object id)
        {
            int Id = Convert.ToInt32(id);
            OpenDetailsWindow?.Invoke(id);    
        }
        public static void FillOutOutputList(List<User> list) 
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
