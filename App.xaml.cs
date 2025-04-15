using GymApp.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GymApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
