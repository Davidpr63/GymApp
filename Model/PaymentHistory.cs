using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Model
{
    public class PaymentHistory
    {
        public int Id { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentDate { get; set; }
        public PaymentHistory()
        {
            
        }
    }
}
