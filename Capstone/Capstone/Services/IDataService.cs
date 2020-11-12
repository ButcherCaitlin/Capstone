using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Services
{
    public interface IDataService
    {
        Task<bool> AddItemAsync(Property item);
        Task<bool> UpsertItemAsync(Property item);
        Task<bool> DeleteItemAsync(string id);
        Task<Property> GetItemAsync(string id);
        Task<IEnumerable<Property>> GetItemsAsync(bool forceRefresh = false);
    }
}
