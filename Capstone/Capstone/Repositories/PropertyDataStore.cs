using Capstone.Models;
using System.Net.Http;
using Xamarin.Forms;
using Capstone.Services;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;

namespace Capstone.Repositories
{
    public class PropertyDataStore : CapstoneDataStore<Property>
    {
        public PropertyDataStore()
        {
            //_httpClient = new HttpClient();
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                baseUri = Device.RuntimePlatform == Device.Android ?
                    "http://10.0.2.2:51044/api/properties" :
                    "http://localhost:51044/api/properties";

            //this is where you can also add an authentication token or user id
        }
        public virtual async Task<bool> AddRelatedItemAsync(Showing showing)
        {
            var uri = new Uri($"{baseUri}/{showing.PropertyID}/showings");
            var json = JsonConvert.SerializeObject(showing);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}
