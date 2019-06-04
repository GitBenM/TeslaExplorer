using Newtonsoft.Json;
using System;

namespace TeslaExplorer.DataAccess
{
    public class AuthResponseDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public double ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("created_at")]
        public double CreatedAt { get; set; }
    }
}
