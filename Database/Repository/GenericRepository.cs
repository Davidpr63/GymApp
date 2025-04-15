using GymApp.Database.IRepository;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymApp.Database.Repository
{
    public class GenericRepository<T> : IGenericRepository<T>
    {
        protected readonly string _filePath;

        public GenericRepository(string filePath)
        {
            _filePath = filePath;
            string folderPath = ConfigurationManager.AppSettings["DatabaseFilePath"];
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(_filePath))
            {
                
                File.WriteAllText(_filePath, "[]");
            }
        }
        public void Add(T entity)
        {
            var list = GetAll();
            list.Add(entity);
            SaveAll(list);
        }

        public void Delete(int id)
        {
            var list = GetAll();
            var entity = list.FirstOrDefault(x => x.Equals(id));
            list.Remove(entity);
        }

        public T Get(int id)
        {
            var list = GetAll();
            return list.FirstOrDefault(x => x.Equals(id));
        }

        public List<T> GetAll()
        {
            var json = File.ReadAllText(_filePath);
            List<T> list = JsonSerializer.Deserialize<List<T>>(json);
            return list;
        }
        public void SaveAll(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
