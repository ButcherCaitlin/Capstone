using Capstone.API.Configuration;
using Capstone.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Services
{
    public class PropertyService
    {
        //This class could become a generic classs.
        private readonly IMongoCollection<Property> _properties;

        public PropertyService(ICapstoneDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _properties = database.GetCollection<Property>(settings.CapstonePropertyCollection);
        }

        //Get all properties from the database.
        public IEnumerable<Property> Get()
        {
            return _properties.Find(property => true).ToEnumerable<Property>();
        }

        //Get a single property with an ID.
        public Property Get(string id)
        {
            return _properties.Find<Property>(property => property.Id == id).FirstOrDefault();
        }

        //Create a new property in the database and gives it an ID. The property and new ID are returned.
        public Property Create(Property property)
        {
            _properties.InsertOne(property);
            return property;
        }

        //Update a property in the database with a new property object.
        public void Update(string id, Property propertyIn)
        {
            _properties.ReplaceOne(property => property.Id == id, propertyIn);
        }

        //Remove a property using the object with an ID or the ID string.
        public void Remove(Property propertyIn)
        {
            _properties.DeleteOne(property => property.Id == propertyIn.Id);
        }
        public void Remove(string id)
        {
            _properties.DeleteOne(property => property.Id == id);
        }
    }
}
