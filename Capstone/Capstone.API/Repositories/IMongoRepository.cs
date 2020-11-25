using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.API.Repositories
{
    public interface IMongoRepository<T>
    {
        public IEnumerable<T> Get();
        public T Get(string id);
        public IEnumerable<T> Get(IEnumerable<string> recordIdCollection);
        public T Create(T record);
        public IEnumerable<T> Create(IEnumerable<T> records);
        public void Update(T updateRecord);
        //public void Remove(T record);
        public void Remove(string id);
    }
}
