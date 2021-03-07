using Capstone.Models;
using System.Net.Http;
using Xamarin.Forms;
using Capstone.Services;
using System.Threading.Tasks;
using System;

namespace Capstone.Repositories
{
    public class ShowingDataStore : CapstoneDataStore<Showing>
    {
        public ShowingDataStore()
        {
            //_httpClient = new HttpClient();
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("userId", "5fa0af488c2c57009df03d1c");
            //baseUri = Device.RuntimePlatform == Device.Android ?
            //    "http://10.0.2.2:51044/api/showings" :
            //    "http://localhost:51044/api/showings";
            baseUri = Device.RuntimePlatform == Device.Android ?
                "http://10.0.2.2:8080/api/showings" :
                "http://localhost:8080/api/showings";

            //this is where you can also add an authentication token or user id
        }

        public override Task<bool> AddItemAsync(Showing item)
        {
            throw new NotImplementedException();
        }
    }
}
