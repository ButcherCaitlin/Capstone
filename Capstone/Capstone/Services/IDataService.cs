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
        Task<bool> AddItemAsync(Showing item);
        Task<bool> AddItemAsync(User user);
        Task<bool> UpsertItemAsync(Property item);
        Task<bool> UpsertItemAsync(Showing item);
        Task<bool> UpsertItemAsync(User item);
        Task<bool> DeleteItemAsync(Property item);
        Task<bool> DeleteItemAsync(Showing item);
        Task<bool> DeleteItemAsync(User item);
        Task<Property> GetPropertyAsync(string id);
        Task<User> GetUserAsync(string id);
        Task<Showing> GetShowingAsync(string id);
        Task<IEnumerable<Property>> GetPropertiesAsync(bool forceRefresh = false);
    }
}
