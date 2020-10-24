using Capstone.API.Configuration;
using Capstone.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Services
{
    public class ShowingService
    {
        //This class could become a generic classs.
        private readonly IMongoCollection<Showing> _showings;

        public ShowingService(ICapstoneDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _showings = database.GetCollection<Showing>(settings.CapstoneShowingCollection);
        }

        //Get all properties from the database.
        public IEnumerable<Showing> Get()
        {
            return _showings.Find(showing => true).ToEnumerable<Showing>();
        }

        //Get a single property with an ID.
        public Showing Get(string id)
        {
            return _showings.Find<Showing>(showing => showing.Id == id).FirstOrDefault();
        }

        //Get a single showing with an ID
        public IEnumerable<Showing> GetShowingsForUser(string userId)
        {
            return _showings.Find<Showing>(showing => showing.RealtorID == userId || showing.ProspectID == userId).ToEnumerable<Showing>();
        }

        //Create a new property in the database and gives it an ID. The property and new ID are returned.
        public Showing Create(Showing showing)
        {
            _showings.InsertOne(showing);
            return showing;
        }

        //Update a property in the database with a new property object.
        public void Update(string id, Showing showingIn)
        {
            _showings.ReplaceOne(showing => showing.Id == id, showingIn);
        }

        //Remove a property using the object with an ID or the ID string.
        public void Remove(Showing showingIn)
        {
            _showings.DeleteOne(showing => showing.Id == showingIn.Id);
        }
        public void Remove(string id)
        {
            _showings.DeleteOne(showing => showing.Id == id);
        }
    }
}
