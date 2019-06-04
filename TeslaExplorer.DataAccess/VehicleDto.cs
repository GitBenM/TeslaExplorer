using Newtonsoft.Json;
using System;

namespace TeslaExplorer.DataAccess
{
    public class VehicleDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("vehicle_id")]
        public string VehicleId { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("vin")]
        public string Vin { get; set; }
        [JsonProperty("charge_state")]
        public ChargeStateDto ChargeState { get; set; }
    }
}
