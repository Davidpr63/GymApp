using GymApp.Database.IRepository;
using GymApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GymApp.Database.Repository
{
    public class NotesRepository : GenericRepository<Note>, INotesRepository
    {
        public NotesRepository(string filePath) : base(filePath)
        {
        }

        public override Note Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.MemberId == id);
            //return base.Get(id);
        }

        public override void Add(Note entity)
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

        public override void Update(Note entity)
        {
            if (entity != null)
            {
                var list = GetAll();
                foreach (var item in list)
                {
                    if (item.Id == entity.Id)
                    {
                        item.Notes = entity.Notes;
                    }
                }
                SaveAll(list);
            }
            
        }

        public override void Delete(Note entity)
        {
            var list = GetAll();
            list.RemoveAll(x => x.Id == entity.Id);
            SaveAll(list);
        }
    }
}
