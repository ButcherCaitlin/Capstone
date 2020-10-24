using Capstone.API.Configuration;
using Capstone.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Property> GetAll()
        {
            return _properties.Find(property => true).ToEnumerable<Property>();
        }
        public Property Get(string id)
        {
            return _properties.Find<Property>(property => property.Id == id).FirstOrDefault();
        }
        public Property GetPropertyForUser(string userId, string propertyId)
        {
            return _properties.Find<Property>(property =>
            property.OwnerID == userId
            && property.Id == propertyId).FirstOrDefault();
        }
        public IEnumerable<Property> GetPropertiesForUser(string userId)
        {
            return _properties.Find<Property>(property => property.OwnerID == userId).ToEnumerable<Property>();
        }
        public Property CreateOne(Property property)
        {
            _properties.InsertOne(property);
            return property;
        }
        public IEnumerable<Property> CreateMany(IEnumerable<Property> properties)
        {
            _properties.InsertMany(properties);
            return properties;
        }
        public void UpdateOne(string id, Property propertyIn)
        {
            _properties.ReplaceOne(property => property.Id == id, propertyIn);
        }
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
