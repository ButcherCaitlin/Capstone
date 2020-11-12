using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading.Tasks;

namespace Capstone.Repositories
{
    public interface IDataStore <T>
    {
        //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#what-is-httpclientfactory

        Task<bool> AddItemAsync(T item);
        Task<bool> UpsertItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
