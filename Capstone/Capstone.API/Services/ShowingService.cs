using Capstone.API.Configuration;
using Capstone.API.Entities;
using Capstone.API.ResourceParameters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;

namespace Capstone.API.Services
{
    public class ShowingService : DatabaseService<Showing>
    {
        public ShowingService(ICapstoneDatabaseSettings settings):
            base(settings){ }

        public IEnumerable<Showing> Get(ShowingsResourceParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.PropertyID)
                && string.IsNullOrWhiteSpace(parameters.RealtorID)
                && string.IsNullOrWhiteSpace(parameters.ProspectID)
                && string.IsNullOrWhiteSpace(parameters.SearchPhrase))
            {
                return Get();
            }

            var collection = _recordCollection.AsQueryable();

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
    }
}
