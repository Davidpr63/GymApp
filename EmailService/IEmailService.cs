using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(User member, bool typeOfEmail);
    }
}
