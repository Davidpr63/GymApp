using GymApp.GoogleDrive;
using GymApp.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace GymApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IGoogleDriveUploader _googleDriveUploader;

        private string filePathUsers = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Users.json";
        private string filePathNotes = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Notes.json";
        private string filePathPaymentHistory = ConfigurationManager.AppSettings["DatabaseFilePath"] + "/Payment history.json";
        private string _usersOldHash;
        private string _usersNewHash;
        private string _notesOldHash;
        private string _notesNewHash;
        private string _historyOldHash;
        private string _historyNewHash;

       
        protected override void OnStartup(StartupEventArgs e)
        {
            
            var loginWindow = new LoginPage();
            loginWindow.Show();
        }

       

       
        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }

        
    }
}
