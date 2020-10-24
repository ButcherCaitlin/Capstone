using Capstone.API.Configuration;
using Capstone.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Services
{
    public class UserService
    {
        //This class could become a generic classs.
        private readonly IMongoCollection<User> _users;

        public UserService(ICapstoneDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.CapstoneUserCollection);
        }
        public IEnumerable<User> Get()
        {
            return _users.Find(user => true).ToEnumerable<User>();
        }
        public User Get(string id)
        {
            return _users.Find<User>(user => user.Id == id).FirstOrDefault();
        }
        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }
        public void Update(string id, User userIn)
        {
            _users.ReplaceOne(user => user.Id == id, userIn);
        }
        public void Remove(User userIn)
        {
            _users.DeleteOne(user => user.Id == userIn.Id);
        }
        public void Remove(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }
    }
}
