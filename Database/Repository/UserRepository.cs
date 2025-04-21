using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public override void Add(User entity)
        {
            var last = GetAll().LastOrDefault();
            entity.Id = ++last.Id;
            base.Add(entity);
        }

        public override void Update(User entity)
        {
            //base.Update(entity);
            var list = GetAll();
            int index = list.FindIndex(x => x.Id == entity.Id);
            list[index] = entity;
            SaveAll(list);
        }

        public bool Authenticate(string usename, string password)
        {
            return GetAll().Any(x => x.Username.Equals(usename) && x.Password.Equals(password));
        }
    }
}
