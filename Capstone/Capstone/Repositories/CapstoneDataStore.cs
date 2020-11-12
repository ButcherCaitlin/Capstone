using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Capstone.Repositories
{
    public abstract class CapstoneDataStore<T> : IDataStore<T> where T : Storable
    {
        protected HttpClient _httpClient;
        protected string baseUri;
        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            var uri = new Uri(baseUri);

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
            return null;
        }
        public async Task<T> GetItemAsync(string id)
        {
            var uri = new Uri($"{baseUri}/{id}");

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }
        public virtual async Task<bool> AddItemAsync(T item)
        {
            var uri = new Uri(baseUri);
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteItemAsync(string id)
        {
            var uri = new Uri($"{baseUri}/{id}");
            HttpResponseMessage response = await _httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UpsertItemAsync(T item)
        {
            var uri = new Uri($"{baseUri}/{item.Id}");
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application.json");

            HttpResponseMessage response = await _httpClient.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
