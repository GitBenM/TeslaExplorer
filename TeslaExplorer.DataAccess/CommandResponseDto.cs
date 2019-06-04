using Newtonsoft.Json;
using System;

namespace TeslaExplorer.DataAccess
{
    public class CommandResponseDto
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
