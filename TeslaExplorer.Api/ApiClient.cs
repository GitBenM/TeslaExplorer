using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeslaExplorer.DataAccess;

namespace TeslaExplorer.Api
{
    /// <summary>
    /// https://tesla-api.timdorr.com/
    /// </summary>
    public class TeslaClient
    {
        public const string UserAgent = "TeslaExplorer";
        public const string BaseUrl = "https://owner-api.teslamotors.com";
        public const string TESLA_CLIENT_ID = "81527cff06843c8634fdc09e8ac0abefb46ac849f38fe1e431c2ef2106796384";
        public const string TESLA_CLIENT_SECRET = "c7257eb71a564034f9419ee651c7d0e5f7aa6bfbd18bafb5c5c033b093bb2fa3";
        public ApiClientEndpoints Endpoints { get; set; } = new ApiClientEndpoints();
        
        public HttpClientWrapper Client { get; set; }

        public TeslaClient()
        {
            Client = new HttpClientWrapper(UserAgent, BaseUrl);
        }

        public async Task<WebRequestResult<AuthResponseDto>> AuthRequest(AuthRequestDto dto)
        {
            return await Client.SendAsync<AuthResponseDto, AuthRequestDto>(HttpMethod.Post, Endpoints.GetAuthenticationToken, dto);
        }

        public async Task<WebRequestResult<VehiclesResponseDto>> GetVehicles()
        {
            return await Client.SendAsync<VehiclesResponseDto>(HttpMethod.Get, Endpoints.GetVehicles);
        }

        public async Task<WebRequestResult<DataRequest<ChargeStateDto>>> GetChargeState(string id)
        {
            return await Client.SendAsync<DataRequest<ChargeStateDto>>(HttpMethod.Get, Endpoints.GetChargeState(id));
        }

        public async Task<WebRequestResult<DataRequest<VehicleDto>>> GetVehicleData(string id)
        {
            return await Client.SendAsync<DataRequest<VehicleDto>>(HttpMethod.Get, Endpoints.GetVehicleData(id));
        }

        public async Task<WebRequestResult<DataRequest<VehicleDto>>> PostWakeUp(string id)
        {
            return await Client.SendAsync<DataRequest<VehicleDto>>(HttpMethod.Post, Endpoints.GetWakeUp(id));
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeLimit(string id, int percent)
        {
            return await Client.SendAsync<DataRequest<CommandResponseDto>, object>(HttpMethod.Post, Endpoints.PostChargeLimit(id), new { percent });
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeStart(string id)
        {
            return await Client.SendAsync<DataRequest<CommandResponseDto>>(HttpMethod.Post, Endpoints.PostChargeStart(id));
        }

        public async Task<WebRequestResult<DataRequest<CommandResponseDto>>> PostChargeStop(string id)
        {
            return await Client.SendAsync<DataRequest<CommandResponseDto>>(HttpMethod.Post, Endpoints.PostChargeStop(id));
        }
    }

    public class ApiClientEndpoints
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
