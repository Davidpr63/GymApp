using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Database.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(string filePath) : base(filePath)
        {
        }

        public bool Authenticate(string usename, string password)
        {
            return GetAll().Any(x => x.Username.Equals(usename) && x.Password.Equals(password));
        }
    }
}
