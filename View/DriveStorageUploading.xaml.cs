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
    /// Interaction logic for DriveStorageUploading.xaml
    /// </summary>
    public partial class DriveStorageUploading : Window
    {
        public DriveStorageUploading()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public void UpdateProgress(int percent)
        {
            ProgressBar.Value = percent;
            PercentageText.Text = $"{percent}%";
        }
    }
}
