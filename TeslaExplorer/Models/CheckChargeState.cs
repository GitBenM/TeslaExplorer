using Hangfire;
using System;
using System.Threading.Tasks;
using TeslaExplorer.Api;

namespace TeslaExplorer.Models
{
    public class CheckChargeState
    {
        public static async Task<bool> KeepCharging(string vehicleId, int maxSoc, TeslaApi api)
        {
            var chargeState = await api.GetChargeState(vehicleId);

            if (!(chargeState?.IsSuccess ?? false) || chargeState.Result.Response.ChargingState == "Disconnected" || chargeState.Result.Response.BatteryLevel >= maxSoc)
                return false;

            if (chargeState.Result.Response.ChargingState == "Stopped")
            {
                await api.PostChargeStart(vehicleId);
            }

            return true;
        }

        public static async Task CheckChargingStatus(string username, string vehicleId, int maxSoc, string accessToken)
        {
            var api = new TeslaApi(username);

            api.SetAccessToken(accessToken);

            var vehicleState = await api.GetVehicleData(vehicleId);

            //wake up the car
            if (vehicleState.Result.Response.State != "online")
            {
                await api.PostWakeUp(vehicleId);
                BackgroundJob.Schedule(() => CheckChargingStatus(username, vehicleId, maxSoc, accessToken), TimeSpan.FromMinutes(2));
                return;
            }

            if (await KeepCharging(vehicleId, maxSoc, api))
            {
                BackgroundJob.Schedule(() => CheckChargingStatus(username, vehicleId, maxSoc, accessToken), TimeSpan.FromMinutes(5));
                return;
            }

            await api.PostChargeStop(vehicleId);
        }
    }
}