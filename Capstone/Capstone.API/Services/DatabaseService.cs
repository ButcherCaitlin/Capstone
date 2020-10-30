using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Services
{
    public interface DatabaseService<T>
    {
        public IEnumerable<T> Get();
        public T Get(string id);
        public T Create(T record);
        public void Update(T updateRecord);
        public void Remove(T record);
        public void Remove(string id);
    }

    public class MongoDatabaseService<T>
    {
    }
}
