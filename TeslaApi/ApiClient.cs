using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TeslaApi
{
    /// <summary>
    /// https://tesla-api.timdorr.com/
    /// </summary>
    public class ApiClient
    {
        public string UserAgent = "TeslaExplorer v1";
        public const string TESLA_CLIENT_ID = "81527cff06843c8634fdc09e8ac0abefb46ac849f38fe1e431c2ef2106796384";
        public const string TESLA_CLIENT_SECRET = "c7257eb71a564034f9419ee651c7d0e5f7aa6bfbd18bafb5c5c033b093bb2fa3";
        public string Username { get; set; }
        public string Password { get; set; }
        public ApiClientEndpoints Endpoints { get; set; } = new ApiClientEndpoints();
        public string Access_token { get; set; }
        public HttpClient HttpClient { get; set; }

        public ApiClient() {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://owner-api.teslamotors.com")
            };

            HttpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("TeslaExplorer", "1"));
        }

        public async Task AuthRequest()
        {

            var authRequest = new HttpRequestMessage(HttpMethod.Post, Endpoints.Authentication.Token);
            Endpoints.Authentication.Request = new ApiClientAuthRequest
            {
                client_id = TESLA_CLIENT_ID,
                client_secret = TESLA_CLIENT_SECRET,
                email = "benmilla21@gmail.com",
                grant_type = "password",
                password = "fatjoe21"
            };

            authRequest.Content = new StringContent(JsonConvert.SerializeObject(Endpoints.Authentication.Request), Encoding.UTF8, "application/json");
            
            var authResult = await HttpClient.SendAsync(authRequest);
            var stringContent = await authResult.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ApiClientAuthResponse>(stringContent);
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", response.access_token);
        }

        public async Task GetVehicles()
        {
            var vehiclesRequest = new HttpRequestMessage(HttpMethod.Get, "/api/1/vehicles");

            var response = await HttpClient.SendAsync(vehiclesRequest);
            var stringContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response: {stringContent}");
        }
    }

    public class ApiClientEndpoints
    {
        public ApiClientAuthentication Authentication { get; set; } = new ApiClientAuthentication();
    }

    public class ApiClientAuthentication
    {
        public string Token = "/oauth/token";
        public ApiClientAuthRequest Request { get; set; }
        public ApiClientAuthResponse Response { get; set; }
    }

    public class ApiClientAuthRequest
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class ApiClientAuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public double expires_in { get; set; }
        public string refresh_token { get; set; }
        public double created_at { get; set; }
    }

    public class ApiClientVehicles
    {

    }
}
