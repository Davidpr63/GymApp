using GymApp.Database.IRepository;
using GymApp.Database.Repository;
using GymApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Window
    {
        private int MemberId { get; set; }
        private readonly DetailsViewModel _viewModel;
        
       
        public DetailsPage(object id, IUserRepository userRepository, INotesRepository notesRepository, IPaymentHistoryRepository paymentHistory)
        {
            InitializeComponent();
            MemberId = Convert.ToInt32(id);
            
            _viewModel = new DetailsViewModel(MemberId, userRepository, notesRepository, paymentHistory);
            _viewModel.CloseWindow = () =>
            {
                this.Close();
            };
            DataContext = _viewModel;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Notes.Text.Equals("Unesite beleske..."))
            {
                Notes.Text = "";
                Notes.Foreground = Brushes.Black;
            }
            Notes.Foreground = Brushes.Black;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Notes.Text))
            {
                Notes.Text = "Unesite beleske...";
            }
            Notes.Foreground = Brushes.Gray;
        }
    }
}
