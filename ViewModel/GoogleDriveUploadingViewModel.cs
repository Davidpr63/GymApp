﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymApp.ViewModel
{
    public class GoogleDriveUploadingViewModel : INotifyPropertyChanged
    {

        private string _percent = "0%";

        public string Percent { get => _percent; set { _percent = value; OnPropertyChanged(); } }
        private int _progressValue;

        public int ProgressValue { get => _progressValue; set { _progressValue = value; OnPropertyChanged(); } }
        public GoogleDriveUploadingViewModel()
        {
            
        }

        public async Task UpdateStatus(int percent)
        {
            if (percent == 0)
            {
                MessageBox.Show("Trenutno nemate interneta, podaci nece biti sacuvani na Google drive-u.",
                                "Greska",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            if (percent == 100)
            {
                Percent = $"Sačuvano : {percent}%";
                ProgressValue = percent;
                await Task.Delay(1000);
                Percent = "Uspesno sačuvani podaci!";
            }
            else
            {
                Percent = $"Sačuvano : {percent}%";
                ProgressValue = percent;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
