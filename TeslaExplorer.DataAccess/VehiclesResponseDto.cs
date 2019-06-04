using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace TeslaExplorer.DataAccess
{
    public class VehiclesResponseDto
    {
        [JsonProperty("response")]
        public List<VehicleDto> Response { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
