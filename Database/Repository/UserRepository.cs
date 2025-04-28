using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace GymApp.Database.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(string filePath) : base(filePath)
        {
        }

        public override void Add(User entity)
        {
            if (GetAll().Count == 0)
                entity.Id = 1;
            entity.Id = ++GetAll().LastOrDefault().Id;
            
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
            var list = GetAll().Where(x => x.TypeUser == TypeUser.Trainer).ToList();
            
            
            return list.Any(x => x.Username.Equals(usename) && x.Password.Equals(password));
        }

        public override User Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
            //return base.Get(id);
        }

        public override void Delete(User entity)
        {
            var list = GetAll();
            list.RemoveAll(x => x.Id == entity.Id);
            SaveAll(list);

            //base.Delete(entity);
        }
    }
}
