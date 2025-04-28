using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GymApp.Database.Repository
{
    public class PaymentHistoryRepository : GenericRepository<PaymentHistory>, IPaymentHistoryRepository
    {
        public PaymentHistoryRepository(string filePath) : base(filePath)
        {
        }

        public override void Add(PaymentHistory entity)
        {
            var list = GetAll();
            if (list.Count == 0)
            {
                entity.Id = 1;
            }
            else
                entity.Id = ++list.LastOrDefault().Id;
            base.Add(entity);
        }

        public override PaymentHistory Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.MemberId == id);
           // return base.Get(id);
        }

        public override void Delete(PaymentHistory entity)
        {
            var list = GetAll();
            list.RemoveAll(x => x.Id == entity.Id);
            SaveAll(list);
        }

       
    }
}
