using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Model
{
    
    public enum TypeUser { Trainer, Member };
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }  
        public string Lastname { get; set; }  

        public string Username {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty ;
        public string Email { get; set; }  
        
        public bool IsMembershipPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public TypeUser TypeUser { get; set; }  

        public User()
        {
             
        }
    }
}
