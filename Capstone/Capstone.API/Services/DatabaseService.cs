using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Capstone.API.Configuration;
using System.Collections.Generic;
using System.Linq;
using Capstone.API.Utility;
using Capstone.API.Entities;

namespace Capstone.API.Services
{
    public class DatabaseService<T> : IDatabaseService<T> where T : MongoEntity 
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _databaseContext;
        protected readonly MongoCollectionBase<T> _recordCollection;
        public DatabaseService(ICapstoneDatabaseSettings settings){
            _client = new MongoClient(settings?.ConnectionString);
            _databaseContext = _client.GetDatabase(settings?.DatabaseName);
            _recordCollection = _databaseContext.GetCollection<T>(CollectionUtility.ServiceString(typeof(T))) as MongoCollectionBase<T>;
        }
        public IEnumerable<T> Get()
        {
            return _recordCollection.Find(record => true).ToEnumerable<T>();
        }
        public T Get(string id)
        {
            return _recordCollection.Find(tofind => tofind.Id == id).FirstOrDefault();
        }
        public IEnumerable<T> Get(IEnumerable<string> recordIdCollection)
        {
            var collection = _recordCollection.AsQueryable();
            collection = collection.Where(a => recordIdCollection.Contains(a.Id));
            return collection.ToList();
        }
        public T Create(T record)
        {
            _recordCollection.InsertOne(record);
            return record;
        }
        public IEnumerable<T> Create(IEnumerable<T> records)
        {
            _recordCollection.InsertMany(records);
            return records;
        }
        public void Update(T updateRecord)
        {
            _recordCollection.ReplaceOne(record => record.Id == updateRecord.Id, updateRecord);
        }
        public void Remove(T removeRecord)
        {
            _recordCollection.DeleteOne(record => record.Id == removeRecord.Id);
        }
        public void Remove(string id)
        {
            _recordCollection.DeleteOne(record => record.Id == id);
        }
    }
}
