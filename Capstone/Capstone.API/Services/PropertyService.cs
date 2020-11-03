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
    public class PropertyService : DatabaseService<Property>
    {

        public PropertyService(ICapstoneDatabaseSettings settings):
            base(settings){ }

        public IEnumerable<Property> Get(PropertiesResourceParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.Type)
                && string.IsNullOrWhiteSpace(parameters.OwnerID)
                && string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                return Get();
            }

            var collection = _recordCollection.AsQueryable();

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
    }
}
