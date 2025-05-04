using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Logger
{
    public class Logger : ILogger
    {
        private string _path = "C:/GymDatabase/Logger.txt";
        public Logger()
        {
            if (!File.Exists(_path))
            {

                File.WriteAllText(_path, "");
            }
        }
        public void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_path, true))  
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Log error: " + ex.Message);
            }
        }
    }
}
