using Newtonsoft.Json;
using System.Collections.Generic;

namespace TeslaExplorer.Api
{
    public class VehiclesResponseDto
    {
        [JsonProperty("response")]
        public List<VehicleDto> Response { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
