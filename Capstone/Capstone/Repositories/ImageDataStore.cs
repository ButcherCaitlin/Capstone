using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Net.Http;
using Xamarin.Forms;
using Capstone.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Capstone.Repositories
{
    public class ImageDataStore : CapstoneDataStore<Images>
    {
        public ImageDataStore()
        {
            //_httpClient = new HttpClient();
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("userId", "5fa0af488c2c57009df03d1c");
            //baseUri = Device.RuntimePlatform == Device.Android ?
            //        "http://10.0.2.2:51044/api/images" :
            //        "http://localhost:51044/api/images";
            baseUri = Device.RuntimePlatform == Device.Android ?
                    "http://10.0.2.2:8080/api/images" :
                    "http://localhost:8080/api/images";
        }
    }
}
