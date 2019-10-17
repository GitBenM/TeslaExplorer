using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeslaExplorer.Api
{
    /// <summary>
    /// https://tesla-api.timdorr.com/
    /// </summary>
    public class TeslaApi
    {
        public const string UserAgent = "TeslaExplorer";
        public const string BaseUrl = "https://owner-api.teslamotors.com";
        public string TESLA_CLIENT_ID = "81527cff06843c8634fdc09e8ac0abefb46ac849f38fe1e431c2ef2106796384";
        public string TESLA_CLIENT_SECRET = "c7257eb71a564034f9419ee651c7d0e5f7aa6bfbd18bafb5c5c033b093bb2fa3";
        internal TeslaApiEndpoints Endpoints { get; set; } = new TeslaApiEndpoints();
        public HttpClientWrapper HttpClient { get; set; }
        /// <summary>
        /// The string provided by the user when attempting to authenticate with Tesla
        /// </summary>
        public string Username { get; set; }

        public string AccessToken { get; set; }
        public TeslaApi(string username)
        {
            Username = username;

            HttpClient = new HttpClientWrapper(UserAgent, BaseUrl)
            {
                DateLastAccess = DateTimeOffset.Now
            };
        }

        public void SetAccessToken(string accessToken)
        {
            HttpClient.SetAuthorizationHeader(accessToken);
            AccessToken = accessToken;
        }

        public async Task<WebRequestResult<AuthResponseDto>> AuthRequest(AuthRequestDto dto)
        {
            return await HttpClient.SendAsync<AuthResponseDto, AuthRequestDto>(HttpMethod.Post, Endpoints.GetAuthenticationToken, dto);
        }

        public async Task<WebRequestResult<VehiclesResponseDto>> GetVehicles()
        {
            return await HttpClient.SendAsync<VehiclesResponseDto>(HttpMethod.Get, Endpoints.GetVehicles);
        }

        public async Task<WebRequestResult<DataRequest<ChargeStateDto>>> GetChargeState(string id)
        {
            return await HttpClient.SendAsync<DataRequest<ChargeStateDto>>(HttpMethod.Get, Endpoints.GetChargeState(id));
        }

        public async Task<WebRequestResult<DataRequest<VehicleDto>>> GetVehicleData(string id)
        {
            return await HttpClient.SendAsync<DataRequest<VehicleDto>>(HttpMethod.Get, Endpoints.GetVehicleData(id));
        }

        public async Task<WebRequestResult<DataRequest<VehicleDto>>> PostWakeUp(string id)
        {
            return await HttpClient.SendAsync<DataRequest<VehicleDto>>(HttpMethod.Post, Endpoints.GetWakeUp(id));
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeLimit(string id, int percent)
        {
            return await HttpClient.SendAsync<DataRequest<CommandResponseDto>, object>(HttpMethod.Post, Endpoints.PostChargeLimit(id), new { percent });
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeStart(string id)
        {
            return await HttpClient.SendAsync<DataRequest<CommandResponseDto>>(HttpMethod.Post, Endpoints.PostChargeStart(id));
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeStop(string id)
        {
            return await HttpClient.SendAsync<DataRequest<CommandResponseDto>>(HttpMethod.Post, Endpoints.PostChargeStop(id));
        }
    }

    internal class TeslaApiEndpoints
    {
        public string GetAuthenticationToken = "/oauth/token";
        public string GetChargeState(string id) => $"/api/1/vehicles/{id}/data_request/charge_state";
        public string GetVehicles = "/api/1/vehicles";
        public string GetVehicleData(string id) => $"/api/1/vehicles/{id}/vehicle_data";
        public string GetWakeUp(string id) => $"/api/1/vehicles/{id}/wake_up";
        public string PostChargeLimit(string id) => $"/api/1/vehicles/{id}/command/set_charge_limit";
        public string PostChargeStart(string id) => $"/api/1/vehicles/{id}/command/charge_start";
        public string PostChargeStop(string id) => $"/api/1/vehicles/{id}/command/charge_stop";
    }
}
