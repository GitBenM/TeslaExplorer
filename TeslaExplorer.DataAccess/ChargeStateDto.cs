using Newtonsoft.Json;
using System;

namespace TeslaExplorer.DataAccess
{
    public class ChargeStateDto
    {
        [JsonProperty("charge_limit_soc")]
        public int ChargeLimitSoc { get; set; }
        [JsonProperty("battery_level")]
        public int BatteryLevel { get; set; }
        [JsonProperty("est_battery_range")]
        public decimal EstBatteryRange { get; set; }
        [JsonProperty("charger_power")]
        public int ChargerPower { get; set; }
        [JsonProperty("charger_voltage")]
        public int ChargerVoltage { get; set; }
        [JsonProperty("charging_state")]
        public string ChargingState { get; set; }
        [JsonProperty("charger_actual_current")]
        public int ChargerActualCurrent { get; set; }
        [JsonProperty("charge_rate")]
        public decimal ChargeRate { get; set; }

    }
}
