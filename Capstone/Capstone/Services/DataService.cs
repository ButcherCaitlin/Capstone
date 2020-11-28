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
        public async Task<bool> AddItemAsync(Showing item)
        {
            var pds = _propertyDataStore as PropertyDataStore;
            return await pds.AddRelatedItemAsync(item);
        }
        public async Task<bool> AddItemAsync(User item)
        {
            return await _userDataStore.AddItemAsync(item);
        }
        public async Task<bool> UpsertItemAsync(Property item)
        {
            return await _propertyDataStore.UpsertItemAsync(item);
        }
        public async Task<bool> UpsertItemAsync(Showing item)
        {
            return await _showingDataStore.UpsertItemAsync(item);
        }
        public async Task<bool> UpsertItemAsync(User item)
        {
            return await _userDataStore.UpsertItemAsync(item);
        }
        public async Task<bool> DeleteItemAsync(Property item)
        {
            return await _propertyDataStore.DeleteItemAsync(item.Id);
        }
        public async Task<bool> DeleteItemAsync(Showing item)
        {
            return await _showingDataStore.DeleteItemAsync(item.Id);
        }
        public async Task<bool> DeleteItemAsync(User item)
        {
            return await _userDataStore.DeleteItemAsync(item.Id);
        }
        public async Task<Property> GetPropertyAsync(string id)
        {
            return await _propertyDataStore.GetItemAsync(id);
        }
        public async Task<User> GetUserAsync(string id)
        {
            return await _userDataStore.GetItemAsync(id);
        }
        public async Task<Showing> GetShowingAsync(string id)
        {
            return await _showingDataStore.GetItemAsync(id);
        }
        public async Task<IEnumerable<Property>> GetPropertiesAsync(bool forceRefresh = false)
        {
            return await _propertyDataStore.GetItemsAsync();
            //we need to be able to pass filters to this get items method.
        }



    }
}
