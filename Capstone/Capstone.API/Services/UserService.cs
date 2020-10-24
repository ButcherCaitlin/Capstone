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

        //Get all properties from the database.
        public IEnumerable<User> Get()
        {
            return _users.Find(user => true).ToEnumerable<User>();
        }

        //Get a single property with an ID.
        public User Get(string id)
        {
            return _users.Find<User>(user => user.Id == id).FirstOrDefault();
        }

        //Create a new property in the database and gives it an ID. The property and new ID are returned.
        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        //Update a property in the database with a new property object.
        public void Update(string id, User userIn)
        {
            _users.ReplaceOne(user => user.Id == id, userIn);
        }

        //Remove a property using the object with an ID or the ID string.
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
