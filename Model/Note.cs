using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Notes { get; set; }

        public int MemberId { get; set; }
        public Note()
        {
            
        }
    }
}
