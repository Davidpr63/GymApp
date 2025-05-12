using GymApp.Database.IRepository;
using GymApp.Logger;
using GymApp.Model;
using GymApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymApp.View
{
    /// <summary>
    /// Interaction logic for Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Window
    {
        private MainWindowViewModel _mainWindowViewModel;
        private int Id { get; set; }
        private readonly IUserRepository _userRepository;
        private readonly INotesRepository _notesRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;
        private readonly ILogger _logger;
        public Confirmation(int id ,MainWindowViewModel mainWindowViewModel, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistoryRepository, ILogger logger)
        {
            InitializeComponent();
            _logger = logger;
            User user = new User();
            _mainWindowViewModel = mainWindowViewModel;
            _userRepository = userRepository;
            _notesRepository = notesRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
            Id = id;
            this.DataContext = mainWindowViewModel;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var member = _userRepository.Get(Id);
                var notes = _notesRepository.Get(Id);
                var payments = _paymentHistoryRepository.Get(Id);

                _userRepository.Delete(member);
                _notesRepository.Delete(notes);
                _paymentHistoryRepository.Delete(payments);
                _logger.Log($"Obrisan član {member.Firstname} {member.Lastname}");
                MainWindowViewModel.FillOutOutputList(_userRepository.GetAll());
                this.Close();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while trying to delete the member: " + ex.Message,
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                throw;
            }

        }
    }
}
