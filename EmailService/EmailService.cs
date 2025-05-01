using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Windows.Markup;
using GymApp.Model;

namespace GymApp.EmailService
{
    public class EmailService : IEmailService
    {
        private string _email;
        private string _password;
        private string _message = "";
        public void SendEmail(User member, bool typeOfEmail)
        {
            _email = ConfigurationManager.AppSettings["Email"];
            _password = ConfigurationManager.AppSettings["Password"];
            string smtpServer = "smtp.gmail.com";
            int port = 587;
            if (typeOfEmail)
                _message = $"Zdravo,\n\n" +
                           $"Uspesno Vam je obnovljena članarina!\n" +
                           $"Važi do : {member.ExpiryDate.ToString("dd/MM/yyyy")}\n" +
                           $"\n" +
                           $"Sportski pozdrav!\n" +
                           $"Vaša teretana.\n";
            else
                _message = $"Zdravo,\n\n" +
                           $"Vaša članarina uskoro ističe,\nvaži do : {member.ExpiryDate.ToString("dd/MM/yyyy")}.\n" +
                           $"Nadamo se daljoj saradnji!\n" +
                           $"\n" +
                           $"Sportski pozdrav!\n" +
                           $"Vaša teretana.\n";
            
            
        
            string username = _email;
            member.Email = "david.02.petrovic@gmail.com";

            SmtpClient client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(username, _password);


            MailMessage message = new MailMessage(_email, member.Email);
            message.Subject = "Trening centar";
            message.Body = _message;

            try
            {
            
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
