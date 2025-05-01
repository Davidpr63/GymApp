using GymApp.Database.IRepository;
using GymApp.EmailService;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace GymApp.Database.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IEmailService _emailService;
        public UserRepository(string filePath, IEmailService emailService) : base(filePath)
        {
            _emailService = emailService;
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
            var Admin = list.FirstOrDefault(x => x.Username.Equals(usename));
            string storedsalt = "";
            if (Admin != null)
            {
                storedsalt = Admin.Salt;
                return VerifyPassword(password, Admin.Password, storedsalt);
            }
            return false;
            //return list.Any(x => x.Username.Equals(usename) && x.Password.Equals(password));
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            return Convert.ToBase64String(hash) == storedHash;
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

        public void CheckMembership()
        {
            
            var list = GetAll();
            foreach (var item in list)
            {
                if (item.ExpiryDate.Date.AddDays(-6) == DateTime.Now.Date && !string.IsNullOrEmpty(item.Email) && !item.Email.Equals("Unesite email..."))
                {
                    item.IsMembershipPaid = false;
                    _emailService.SendEmail(item, false);
                }
            }
            SaveAll(list);
        }
    }
}
