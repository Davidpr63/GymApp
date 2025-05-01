using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.ViewModel
{
    public class GoogleDriveUploadingViewModel : INotifyPropertyChanged
    {

        private string _percent = "0%";

        public string Percent { get => _percent; set { _percent = value; OnPropertyChanged(); } }
        public GoogleDriveUploadingViewModel()
        {
            
        }

        public async Task UpdateStatus(int percent)
        {
            if (percent == 100)
            {
                Percent = $"Sačuvano : {percent}%";
                await Task.Delay(1000);
                Percent = "Uspesno sačuvani podaci!";
                
            }
            else
                Percent = $"Sačuvano : {percent}%";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
