using GymApp.Common;
using GymApp.Database.IRepository;
using GymApp.EmailService;
using GymApp.GoogleDrive;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GymApp.EmailService;
using GymApp.Logger;

namespace GymApp.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private ILogger _logger;
      
        public static ObservableCollection<User> Members { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<User> _filteredMembers { get; set; } = new ObservableCollection<User>();
     
        #region Properties
        public string LoggedInUsername { get; set; }
        private string _searchId = "Unesite ID člana...";
        public string SearchId { get => _searchId; set { _searchId = value; OnPropertyChanged(); SearchMember(); } }
        private bool _activeMembersIsChecked;
        public bool ActiveMembersIsChecked { get => _activeMembersIsChecked; set { _activeMembersIsChecked = value; CheckBoxs(); OnPropertyChanged(); } }
        private bool _activeMembersNotPaidIsChecked;
        public bool ActiveMembersNotPaidIsChecked { get => _activeMembersNotPaidIsChecked; set { _activeMembersNotPaidIsChecked = value; CheckBoxs(); OnPropertyChanged(); } }
        private bool _haveNoteIsChecked;
        public bool HaveNotesIsChecked { get => _haveNoteIsChecked; set { _haveNoteIsChecked = value; CheckBoxs(); OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand LogoutCommand { get; }
        public ICommand AddNewMemberCommand {  get; }
        public ICommand ExtendCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenDeleteConfirmationCommand { get; }
     
        #endregion
        public Action CloseMain;
        public Action CloseConfirmPage;
        public Action OpenAddMemberWindow;
        public Action<object> OpenDetailsWindow;
        public Action<object> OpenConfirmationPage;
        private readonly IEmailService _emailService;
        public MainWindowViewModel(string username, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository, ILogger loggger)
        {
            LoggedInUsername = $"Trener - {username}";
            _userRepository = userRepository;
            _notesRepository = notesRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
            _emailService = new EmailService.EmailService();
            _logger = loggger;
            FillOutOutputList(_userRepository.GetAll());
            LogoutCommand = new RelayCommand(Logout);
            AddNewMemberCommand = new RelayCommand(AddNewMember);
            ExtendCommand = new RelayCommand(id => RenewMembership(id));
            DetailsCommand = new RelayCommand(id => OpenDetails(id));
            
            OpenDeleteConfirmationCommand = new RelayCommand(id => OpenConfirmDeletePage(id));
            //DeleteCommand = new RelayCommand(id => DeleteMember(id));

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
                    return;
                }
                if (string.IsNullOrWhiteSpace(SearchId) || SearchId == "Unesite ID člana...")
                {
                    FillOutOutputList(_userRepository.GetAll());
                    return;
                }

                 
                FillOutOutputList(_userRepository.GetAll());
                MessageBox.Show("ID mora biti broj");

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
            if (ActiveMembersIsChecked && !ActiveMembersNotPaidIsChecked && !HaveNotesIsChecked)
            {
                ActiveMembers();
            }
            if (!ActiveMembersIsChecked && ActiveMembersNotPaidIsChecked && !HaveNotesIsChecked)
            {
                ActiveNotPaidMembers();
            }
            if (!ActiveMembersIsChecked && !ActiveMembersNotPaidIsChecked && HaveNotesIsChecked)
            {
                HaveNoteMembers();
            }
            if (!ActiveMembersIsChecked && !ActiveMembersNotPaidIsChecked && !HaveNotesIsChecked)
            {
                FillOutOutputList(_userRepository.GetAll());
            }
            if (ActiveMembersIsChecked && HaveNotesIsChecked && !ActiveMembersNotPaidIsChecked)
            {
                ActiveMembersWithNotes();
            }
            if (!ActiveMembersIsChecked && HaveNotesIsChecked && ActiveMembersNotPaidIsChecked)
            {
                InActiveMembersWithNotes();
            }
            if (ActiveMembersIsChecked && ActiveMembersNotPaidIsChecked && HaveNotesIsChecked)
            {
                ActiveMembersIsChecked = false;
                ActiveMembersNotPaidIsChecked = false;
                HaveNotesIsChecked = false;
                MessageBox.Show("Ne mogu sve opcije biti čekirane",
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

        private void HaveNoteMembers()
        {
            List<User> filteredMembers = new List<User>();
            try
            {
                filteredMembers = _userRepository.GetAll().Where(x => x.HaveNote == true).ToList();
                FillOutOutputList(filteredMembers);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while filtering  members who have notes: " + e.Message,
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }
        }
        private void ActiveMembersWithNotes()
        {
            List<User> filteredMembers = new List<User>();
            try
            {
                filteredMembers = _userRepository.GetAll().Where(x =>x.IsMembershipPaid == true && x.HaveNote == true).ToList();
                FillOutOutputList(filteredMembers);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while filtering  members who are active and have notes: " + e.Message,
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }
        }
        private void InActiveMembersWithNotes()
        {
            List<User> filteredMembers = new List<User>();
            try
            {
                filteredMembers = _userRepository.GetAll().Where(x => x.IsMembershipPaid == false && x.HaveNote == true).ToList();
                FillOutOutputList(filteredMembers);
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while filtering  members who aren't active and have notes: " + e.Message,
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

        private void OpenConfirmDeletePage(object id)
        {
            OpenConfirmationPage?.Invoke(id);
        }
        private void RenewMembership(object id)
        {
            int Id = Convert.ToInt32(id);

            try
            {
                var membersPaymennts = _paymentHistoryRepository.Get(Id);
                var member = _userRepository.Get(Id);
                if (member.TypeUser == TypeUser.Trainer)
                {
                    MessageBox.Show($"{member.Firstname} je trener, nema potrebe za obnavljanjem :)!",
                                       "",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                    return;
                }
                var payments = _paymentHistoryRepository.Get(Id);
                member.GotEmail = false;
                member.IsMembershipPaid = true;
                member.PaymentDate = DateTime.Now;
                member.ExpiryDate = DateTime.Now.AddDays(30);
                membersPaymennts.PaymentDate = member.PaymentDate;
                membersPaymennts.IsPaid = true;
                membersPaymennts.MemberId = Id;
                _paymentHistoryRepository.Add(membersPaymennts);
                _userRepository.Update(member);
                _paymentHistoryRepository.Add(payments);
                _logger.Log($"{member.Firstname} {member.Lastname} je obnovljena članarina");
                FillOutOutputList(_userRepository.GetAll());
                MessageBox.Show($"Uspesno obnovljena clanarina članu -> {member.Firstname + " " + member.Lastname}",
                                        "",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                if (!string.IsNullOrEmpty(member.Email) && !member.Email.Equals("Unesite email...(opciono)"))
                {
                    _emailService.SendEmail(member, true);

                }
                 
                CloseConfirmPage?.Invoke();
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
