using Newtonsoft.Json;
using System;

namespace TeslaExplorer.Api
{
    public class DataRequest<T>
    {
        [JsonProperty("response")]
        public T Response { get; set; }
    }
}
