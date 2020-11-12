using System.Net.Http;

namespace Capstone.Services
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
