using Capstone.API.Configuration;
using Capstone.API.Entities;
using Capstone.API.ResourceParameters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;

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
        public IEnumerable<Showing> GetAll()
        {
            return _showings.Find(showing => true).ToEnumerable<Showing>();
        }

        public IEnumerable<Showing> Get(ShowingsResourceParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.PropertyID)
                && string.IsNullOrWhiteSpace(parameters.RealtorID)
                && string.IsNullOrWhiteSpace(parameters.ProspectID)
                && string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                return GetAll();
            }

            var collection = _showings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.PropertyID))
            {
                var propertyID = parameters.PropertyID.Trim();
                collection = collection.Where(a => a.PropertyID == propertyID);
            }
            if (!string.IsNullOrWhiteSpace(parameters.RealtorID))
            {
                var realtorId = parameters.RealtorID.Trim();
                collection = collection.Where(a => a.PropertyID == realtorId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.ProspectID))
            {
                var prospectId = parameters.ProspectID.Trim();
                collection = collection.Where(a => a.PropertyID == prospectId);
            }


            if (!string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                var searchPhrase = parameters.SearchPhrase.Trim();
                collection = collection.Where(a => a.Id.Contains(searchPhrase));
            }

            return collection.ToList();
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
            var result = _showings.DeleteOne(showing => showing.Id == id);
        }
    }
}
