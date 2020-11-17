using Capstone.API.Entities;
using Newtonsoft.Json;

namespace Capstone.API.Authentication
{

    public class UserResponses
    {
        public UserResponses(User user)
        {
            Success = true;
            User = user;
            AuthToken = user.AuthToken;
        }

        public UserResponses(bool success, string message)
        {
            Success = success;
            if (success)
            {
                SuccessMessage = message;
            }
            else
            {
                ErrorMessage = message;
            }
        }

        public bool Success { get; set; }
        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }
        public User User { get; set; }
    }
}
