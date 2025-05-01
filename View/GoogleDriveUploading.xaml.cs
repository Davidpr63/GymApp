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
    /// Interaction logic for GoogleDriveUploading.xaml
    /// </summary>
    public partial class GoogleDriveUploading : Window
    {
         
        
        public GoogleDriveUploading(GoogleDriveUploadingViewModel vm)
        {
            InitializeComponent();
             
            this.DataContext = vm;
        }

        
    }
}
