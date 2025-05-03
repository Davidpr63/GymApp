using GymApp.EmailService;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Database.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        string GenericSalt();
        
        string HashPassword(string password, string salt);
        bool Authenticate(string usename, string password);

        Task CheckMembership();

    }
}
