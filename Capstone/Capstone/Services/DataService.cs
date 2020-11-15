using Capstone.Models;
using Capstone.Repositories;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public class DataService : IDataService
    {
        private readonly IDataStore<Property> _propertyDataStore;
        private readonly IDataStore<User> _userDataStore;
        private readonly IDataStore<Showing> _showingDataStore;

        public DataService(IDataStore<Property> propertyDataStore, IDataStore<User> userDataStore, IDataStore<Showing> showingDataStore)
        {
            _propertyDataStore = propertyDataStore;
            _userDataStore = userDataStore;
            _showingDataStore = showingDataStore;
        }

        public async Task<bool> AddItemAsync(Property item)
        {
            return await _propertyDataStore.AddItemAsync(item);
        }

        public async Task<bool> ScheduleShowingAsync(Showing item)
        {
            var pds = _propertyDataStore as PropertyDataStore;
            return await pds.AddRelatedItemAsync(item);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return await _propertyDataStore.DeleteItemAsync(id);
        }

        public async Task<Property> GetItemAsync(string id)
        {
            return await _propertyDataStore.GetItemAsync(id);
        }

        public async Task<IEnumerable<Property>> GetItemsAsync(bool forceRefresh = false)
        {
            return await _propertyDataStore.GetItemsAsync();
            //we need to be able to pass filters to this get items method.
        }

        public async Task<bool> UpsertItemAsync(Property item)
        {
            return await _propertyDataStore.UpsertItemAsync(item);
        }

    }
}
