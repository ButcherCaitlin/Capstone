using Capstone.Models;
using Capstone.Repositories;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Capstone.Services
{
    public class DataService : IDataService
    {
        public void AddProperty(Property property)
        {
            MockPropertDataStore.AddPropertyAsync(property);
        }

        public List<Property> GetAllProperties()
        {
            return MockPropertDataStore.mockProperties;
        }

        public void UpdateProperty(Property property)
        {
            MockPropertDataStore.UpdatePropertyAsync(property);
        }
    }
}
