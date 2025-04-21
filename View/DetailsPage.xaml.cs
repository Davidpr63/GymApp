using GymApp.Database.IRepository;
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
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Window
    {
        private int MemberId { get; set; }
        private readonly DetailsViewModel _viewModel;
        public DetailsPage(object id, IUserRepository userRepository)
        {
            InitializeComponent();
            MemberId = Convert.ToInt32(id);
            _viewModel = new DetailsViewModel(MemberId, userRepository);

            this.DataContext = _viewModel;
        }
    }
}
