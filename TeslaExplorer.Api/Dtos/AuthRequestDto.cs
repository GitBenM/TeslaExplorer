﻿using Newtonsoft.Json;
using System;

namespace TeslaExplorer.Api
{
    public class AuthRequestDto
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
