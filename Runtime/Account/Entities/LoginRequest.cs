using Newtonsoft.Json;

namespace RabbitWings.Account.Entities
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
