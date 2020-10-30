using Capstone.API.Configuration;
using Capstone.API.Entities;
using Capstone.API.ResourceParameters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Services
{
    public class PropertyService
    {
        //This class could become a generic classs.
        private readonly IMongoCollection<Property> _properties;
        private readonly IMongoDatabase _context;

        public PropertyService(ICapstoneDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _context = client.GetDatabase(settings.DatabaseName);

            _properties = _context.GetCollection<Property>(settings.CapstonePropertyCollection);
        }


        public IEnumerable<Property> Get(PropertiesResourceParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.Type)
                && string.IsNullOrWhiteSpace(parameters.OwnerID)
                && string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                return GetAll();
            }

            var collection = _properties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Type))
            {
                var type = parameters.Type.Trim();
                collection = collection.Where(a => a.Type == type);
            }

            if (!string.IsNullOrWhiteSpace(parameters.OwnerID))
            {
                var ownerId = parameters.OwnerID.Trim();
                collection = collection.Where(a => a.OwnerID == ownerId);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                var searchPhrase = parameters.SearchPhrase.Trim();
                collection = collection.Where(a => a.Address.Contains(searchPhrase) ||
                    a.Description.Contains(searchPhrase));
            }

            return collection.ToList();
        }

        #region Implimented in Generic
        public IEnumerable<Property> GetAll()
        {
            return _properties.Find(property => true).ToEnumerable<Property>();
        }
        public IEnumerable<Property> Get(IEnumerable<string> propertyIdCollection)
        {
            var collection = _properties.AsQueryable();
            collection = collection.Where(a => propertyIdCollection.Contains(a.Id));
            return collection.ToList();
        }
        public Property Get(string id)
        {
            return _properties.Find<Property>(property => property.Id == id).FirstOrDefault();
        }
        public IEnumerable<Property> GetPropertiesForUser(string userId)
        {
            return _properties.Find<Property>(property => property.OwnerID == userId).ToEnumerable<Property>();
        }
        public Property Create(Property property)
        {
            _properties.InsertOne(property);
            return property;
        }
        public IEnumerable<Property> CreateMany(IEnumerable<Property> properties)
        {
            _properties.InsertMany(properties);
            return properties;
        }
        public void Update(string id, Property propertyIn)
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
        #endregion
    }
}
